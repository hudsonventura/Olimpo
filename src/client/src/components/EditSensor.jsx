import React, { useState, useEffect } from 'react';


import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';




function EditSensor({id, showModalSensor, setShowModalSensor}) {

    useEffect(() => {
        
        if(showModalSensor == true){
            console.log("Mostrando ... "+id);
        }

        if(showModalSensor == false){
            console.log("Desmontrando ... "+id);
        }

    }, [showModalSensor]); 

    return (
        <Modal
            show={showModalSensor}
            size="lg"
            aria-labelledby="contained-modal-title-vcenter"
            centered
            >
            <Modal.Header closeButton onClick={() => setShowModalSensor(false)}>
                <Modal.Title id="contained-modal-title-vcenter">
                    Modal heading
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                <Form.Label>Email address</Form.Label>
                <Form.Control
                    type="email"
                    placeholder="name@example.com"
                    autoFocus
                />
                </Form.Group>
                <Form.Group
                className="mb-3"
                controlId="exampleForm.ControlTextarea1"
                >
                <Form.Label>Example textarea</Form.Label>
                <Form.Control as="textarea" rows={3} />
                </Form.Group>
            </Form>
            </Modal.Body>
            <Modal.Footer>
            <Button variant='default' onClick={() => setShowModalSensor(false)}>Cancel</Button>
            <Button variant='success' onClick={() => setShowModalSensor(false)}>Save</Button>
            </Modal.Footer>
            </Modal>
    ); 
}

export default EditSensor;