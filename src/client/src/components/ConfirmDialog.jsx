import React, {useState, useEffect} from 'react';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';


function ConfirmDialog({show, setShow, executeFunction, message}) {

    

    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const hadleConfirm = () => {
        executeFunction();
        setShow(false);
    }
    
    return (
        <>
       
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                <Modal.Title>Modal heading</Modal.Title>
                </Modal.Header>
                <Modal.Body>{message}</Modal.Body>
                <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={hadleConfirm}>
                    Save Changes
                </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ConfirmDialog;