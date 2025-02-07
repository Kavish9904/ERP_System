import React, { useState, useEffect } from 'react';
import * as signalR from "@microsoft/signalr";

const defaultImageSrc = '/img/image.png';

const initialFieldValues = {
    employeeID: 0,
    employeeName: '',
    occupation: '',
    imageName: '',
    imageSrc: defaultImageSrc,
    imageFile: null,
    category: 'Development',
    priority: 1
};

export default function Employee(props) {
    const { addOrEdit, recordForEdit } = props;
    const [values, setValues] = useState(initialFieldValues);
    const [errors, setErrors] = useState({});
    const [connection, setConnection] = useState(null);

    useEffect(() => {
        if (recordForEdit) setValues(recordForEdit);
    }, [recordForEdit]);

    useEffect(() => {
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5150/employeehub", {
                withCredentials: true, // Allows cookies/authentication
                skipNegotiation: true, // Bypass the negotiation step for WebSockets
                transport: signalR.HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .configureLogging(signalR.LogLevel.Information)
            .build();

        newConnection.start()
            .then(() => console.log("SignalR Connected"))
            .catch(err => console.error("SignalR Connection Error: ", err));


        newConnection.on("ReceiveMessage", message => {
            alert(message);  // Notify user of new employee addition
        });

        setConnection(newConnection);

        return () => {
            if (newConnection) newConnection.stop();
        };
    }, []);

    const handleInputChange = e => {
        const { name, value } = e.target;
        setValues({ ...values, [name]: value });
    };

    const showPreview = e => {
        if (e.target.files && e.target.files[0]) {
            let imageFile = e.target.files[0];
            const reader = new FileReader();
            reader.onload = x => {
                setValues({ ...values, imageFile, imageSrc: x.target.result });
            };
            reader.readAsDataURL(imageFile);
        } else {
            setValues({ ...values, imageFile: null, imageSrc: defaultImageSrc });
        }
    };

    const validate = () => {
        let temp = {};
        temp.employeeName = values.employeeName ? true : false;
        temp.imageSrc = values.imageSrc !== defaultImageSrc;
        temp.category = values.category ? true : false;
        temp.priority = values.priority > 0;
        setErrors(temp);
        return Object.values(temp).every(x => x === true);
    };

    const resetForm = () => {
        setValues(initialFieldValues);
        document.getElementById('image-uploader').value = null;
        setErrors({});
    };

    const handleFormSubmit = e => {
        e.preventDefault();
        if (validate()) {
            const formData = new FormData();
            formData.append('employeeID', values.employeeID);
            formData.append('employeeName', values.employeeName);
            formData.append('occupation', values.occupation);
            formData.append('imageName', values.imageName);
            formData.append('imageFile', values.imageFile);
            formData.append('category', values.category);
            formData.append('priority', values.priority);

            // const token = localStorage.getItem("token");
            console.log(formData);

            fetch("http://localhost:5150/api/Employee", {
                method: "POST",
                body: formData
            })
                .then(response => response.json())
                .then(data => {
                    if (connection) connection.invoke("SendMessage", "New employee added");
                    addOrEdit(data, resetForm);
                })
                .catch(error => console.error("Error:", error));
        }
    };

    const applyErrorClass = field => (field in errors && !errors[field] ? ' invalid-field' : '');

    return (
        <>
            <div className="container text-center">
                <p className="lead">An Employee</p>
            </div>
            <form autoComplete="off" noValidate onSubmit={handleFormSubmit}>
                <div className="card">
                    <img src={values.imageSrc} className="card-img-top" alt="Employee" style={{ height: "100px", width: "100px" }} />
                    <div className="card-body">
                        <div className="form-group">
                            <input type="file" accept="image/*" className={"form-control-file" + applyErrorClass('imageSrc')}
                                onChange={showPreview} id="image-uploader" />
                        </div>
                        <div className="form-group">
                            <input className={"form-control" + applyErrorClass('employeeName')} placeholder="Employee Name" name="employeeName"
                                value={values.employeeName}
                                onChange={handleInputChange} />
                        </div>
                        <div className="form-group">
                            <input className="form-control" placeholder="Occupation" name="occupation"
                                value={values.occupation}
                                onChange={handleInputChange} />
                        </div>

                        {/* Category Dropdown */}
                        <div className="form-group">
                            <select className={"form-control" + applyErrorClass('category')} name="category"
                                value={values.category} onChange={handleInputChange}>
                                <option value="Development">Development</option>
                                <option value="Management">Management</option>
                                <option value="Support">Support</option>
                            </select>
                        </div>

                        {/* Priority Dropdown */}
                        <div className="form-group">
                            <select className={"form-control" + applyErrorClass('priority')} name="priority"
                                value={values.priority} onChange={handleInputChange}>
                                <option value={1}>Low</option>
                                <option value={2}>Medium</option>
                                <option value={3}>High</option>
                            </select>
                        </div>

                        <div className="form-group text-center">
                            <button type="submit" className="btn btn-light">Submit</button>
                        </div>
                    </div>
                </div>
            </form>
        </>
    );
}
