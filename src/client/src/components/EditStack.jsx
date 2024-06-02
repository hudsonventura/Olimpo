import React, { useState, useEffect, useRef, useContext } from 'react';


import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';


import { AppContext } from '../components/AppContext';



function EditStack({stack, setStack, showModal, setShowModal}) {

    const {settings} = useContext(AppContext);


    const formRef = useRef(null);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setStack(prevStack => ({
            ...prevStack,
            [name]: value
        }));
    };

    const handleForm = () => {
        const form = formRef.current;
        const formData = new FormData(form);
        var data = {};
        formData.forEach((value, key) => {
            if(value > '')
                data[key] = value;
        });

        var url = settings.backend_url;

        //creating stack
        if(data['id'] == undefined){
            fetch(`${url}/stack/`, {
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
        //updating stack
        else{
            fetch(`${url}/stack/${data['id']}`, {
                method: 'PUT',
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
                    Modal heading
                </Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form ref={formRef}>
                <Form.Group className="mb-3" controlId="name">
                <Form.Label>Stack Name</Form.Label>
                <Form.Control
                    type="text"
                    placeholder="Just an identification"
                    autoFocus
                    name='name'
                    value={stack.name}
                    onChange={handleInputChange}
                />
                <Form.Control type="hidden" name='id' value={stack.id} />
                </Form.Group>
            </Form>
            </Modal.Body>
            <Modal.Footer>
            <Button variant='default' onClick={() => setShowModal(false)}>Cancel</Button>
            <Button variant='success' onClick={() => handleForm()}>Save</Button>
            </Modal.Footer>
            </Modal>
    ); 
}



function Edit(){
    
}

export default EditStack;