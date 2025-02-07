import React, { useState, useEffect } from 'react';
import Employee from './Employee';
import axios from 'axios';
import * as signalR from '@microsoft/signalr';

const defaultImageSrc = '/img/image.png';

export default function EmployeeList() {
    const [employeeList, setEmployeeList] = useState([]);
    const [recordForEdit, setRecordForEdit] = useState(null);
    const [categoryFilter, setCategoryFilter] = useState('');
    const [priorityFilter, setPriorityFilter] = useState('');
    const [searchTerm, setSearchTerm] = useState('');

    useEffect(() => {
        refreshEmployeeList();
        setupSignalRConnection();
    }, []);

    const employeeAPI = (url = 'http://localhost:5150/api/Employee/') => {
        // const token = localStorage.getItem('token');
        return {
            fetchAll: () => axios.get(url),
            search: (query) => axios.get(`${url}search?name=${query}`),
            create: newRecord => axios.post(url, newRecord),
            update: (id, updatedRecord) => axios.put(url + id, updatedRecord),
            delete: id => axios.delete(url + id)
        };
    };

    function refreshEmployeeList() {
        employeeAPI().fetchAll()
            .then(res => {
                setEmployeeList(res.data);
            })
            .catch(err => console.log(err));
    }

    const addOrEdit = (formData, onSuccess) => {
        if (formData.get('employeeID') === "0")
            employeeAPI().create(formData)
                .then(res => {
                    onSuccess();
                    refreshEmployeeList();
                })
                .catch(err => console.log(err));
        else
            employeeAPI().update(formData.get('employeeID'), formData)
                .then(res => {
                    onSuccess();
                    refreshEmployeeList();
                })
                .catch(err => console.log(err));
    };

    const showRecordDetails = data => {
        setRecordForEdit(data);
    };

    const onDelete = (e, id) => {
        e.stopPropagation();
        if (window.confirm('Are you sure to delete this record?'))
            employeeAPI().delete(id)
                .then(res => refreshEmployeeList())
                .catch(err => console.log(err));
    };

    const setupSignalRConnection = () => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5150/employeehub")
            .configureLogging(signalR.LogLevel.Information)
            .build();


        connection.start().then(() => {
            console.log("Connected to SignalR");
        }).catch(err => console.log(err));

        connection.on("ReceiveMessage", message => {
            alert(message);
            refreshEmployeeList();
        });
    };

    const handleSearchChange = (e) => {
        setSearchTerm(e.target.value);
        employeeAPI().search(e.target.value)
            .then(res => setEmployeeList(res.data))
            .catch(err => console.log(err));
    };

    return (
        <div className="row">
            <div className="col-md-12">
                <div className="jumbotron jumbotron-fluid py-4">
                    <div className="container text-center">
                        <h1 className="display-4">Employee Register</h1>
                    </div>
                </div>
            </div>
            <div className="col-md-4">
                <Employee addOrEdit={addOrEdit} recordForEdit={recordForEdit} />
            </div>
            <div className="col-md-8">
                <div className="row mb-3">
                    <div className="col-md-4">
                        <input type="text" className="form-control" placeholder="Search by name..." value={searchTerm} onChange={handleSearchChange} />
                    </div>
                    <div className="col-md-4">
                        <select className="form-control" value={categoryFilter} onChange={(e) => setCategoryFilter(e.target.value)}>
                            <option value="">All Categories</option>
                            <option value="Development">Development</option>
                            <option value="Management">Management</option>
                            <option value="Support">Support</option>
                        </select>
                    </div>
                    <div className="col-md-4">
                        <select className="form-control" value={priorityFilter} onChange={(e) => setPriorityFilter(e.target.value)}>
                            <option value="">All Priorities</option>
                            <option value={1}>Low</option>
                            <option value={2}>Medium</option>
                            <option value={3}>High</option>
                        </select>
                    </div>
                </div>
                <div className="row">
                    {employeeList.map(employee => (
                        <div className="col-md-4" key={employee.employeeID}>
                            <div className="card" onClick={() => showRecordDetails(employee)}>
                                <img src={defaultImageSrc} className="card-img-top rounded-circle" style={{ height: "100px", width: "100px" }} alt="Employee" />
                                <div className="card-body">
                                    <h5>{employee.employeeName}</h5>
                                    <span>{employee.occupation}</span>
                                    <br />
                                    <button className="btn btn-light delete-button" onClick={e => onDelete(e, employee.employeeID)}>
                                        <i className="far fa-trash-alt"></i>
                                    </button>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </div>
    );
}