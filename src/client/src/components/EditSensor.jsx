import React, { useState, useEffect, useRef, useContext } from 'react';


import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';


import { AppContext } from './AppContext';



function EditSensor({device, sensor, setSensor, showModal, setShowModal}) {

    const {settings} = useContext(AppContext);


    const formRef = useRef(null);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setSensor(prevSensor => ({
            ...prevSensor,
            [name]: value
        }));
    };

    const handleDelete = () => {
        const form = formRef.current;
        const formData = new FormData(form);
        var data = {};
        formData.forEach((value, key) => {
            if(value > '')
                data[key] = value;
        });


    }

    const handleForm = () => {
        const form = formRef.current;
        const formData = new FormData(form);
        var data = {};
        formData.forEach((value, key) => {
            if(value > '')
                data[key] = value;
        });

        var url = settings.backend_url;

        //creating sensor
        if(data['id'] == undefined){
            fetch(`${url}/sensor/`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            .then(data => {
                console.log('Success:', data);
                setShowModal(false); // Fechar o modal após o sucesso
            })
            .catch((error) => {
                console.error('Error:', error);
            });
        }
        //updating sensor
        else{
            fetch(`${url}/sensor/${data['id']}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            })
            .then(response => response.json()) // Parse the JSON body from the response
            .then(body => {
                console.log('Success:', body); // Log the parsed body
                setShowModal(false); // Fechar o modal após o sucesso
            })
            .catch((error) => {
                console.error('Error:', error);
            });
        }


        

        
    }

    return (
        <Modal
            show={showModal}
            size="lg"
            aria-labelledby="contained-modal-title-vcenter"
            centered
            >
            <Modal.Header closeButton onClick={() => setShowModal(false)}>
                <Modal.Title id="contained-modal-title-vcenter">
                    <>
                    {
                        (sensor.id == undefined)
                        ?
                            `Creating sensor`
                        :
                            <>
                                Updating sensor <b>{sensor.name}</b> <p style={{fontSize: '11px'}}>{sensor.id}</p>
                            </>
                    }
                    </>
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form ref={formRef}>
                <Form.Control type="text" name='id' value={sensor.id} />
                <Form.Control type="text" name='id_device' value={device.id} />

                <Form.Group className="mb-3" controlId="name">
                    <Form.Label>Sensor Name</Form.Label>
                    <Form.Control type="text" placeholder="Just an identification" autoFocus
                        name='name'
                        value={sensor.name}
                        onChange={handleInputChange}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="name">
                    <Form.Label>Hostname<small>, hostname or IP without port and protocol</small></Form.Label>
                    <Form.Control type="text" placeholder="Host, hostname or IP without port and protocol" autoFocus
                        name='host'
                        value={sensor.host}
                        onChange={handleInputChange}
                    />
                </Form.Group>

                </Form>
            </Modal.Body>
            <Modal.Footer style={{ display: 'flex', justifyContent: 'space-between' }}>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <Button variant='danger' onClick={() => handleDelete()}>Delete TO IMPLEMENT</Button> {/*//TODO: Implement delete sensor*/}
                </div>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <Button variant='outline-dark' onClick={() => setShowModal(false)}>Cancel</Button>
                    <Button variant='success' onClick={() => handleForm()}>Save</Button>
                </div>
            </Modal.Footer>
            </Modal>
    ); 
}



function Edit(){
    
}

export default EditSensor;