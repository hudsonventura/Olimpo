import React from 'react';

import Toast from 'react-bootstrap/Toast';


function ErrorMessage({show, setShow, time, type, subject, message}) {

    let bg = "success";
    let bg_text = "text-black";
    if(type == "error"){
        bg = "danger";
        bg_text = "text-white";
    }
    
    return (
        <Toast bg={bg} onClose={() => setShow(false)} show={show} delay={time} autohide style={{position: "fixed", top: "10px", left:"20px"}}>
            <Toast.Header bg={bg}>
                <strong className="me-auto">{subject}</strong>
                <small>Right now</small>
            </Toast.Header>
            <Toast.Body bg={{bg}} className="text-white">
                {message}
            </Toast.Body>
        </Toast>
    ); 
}

export default ErrorMessage;