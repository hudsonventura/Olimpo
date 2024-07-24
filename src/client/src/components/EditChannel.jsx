import { useState, useContext, useRef, useEffect} from 'react';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';


import { AppContext } from '../AppContext';
import ErrorMessage from '../components/ErrorMessage';
import ConfirmDialog from '../components/ConfirmDialog';



function EditChannel({showModal, setShowModal, income_channel }) {
    const {settings} = useContext(AppContext);
    var url = settings.backend_url;

    const formRef = useRef(null);


    const [channel, setChannel] = useState(income_channel);



    const [showError, setShowError] = useState(false); //for messages
    const [errorMessage, setErrorMessage] = useState(
        { time: 100000, type: 'error', subject: "", message: "" }
    );

    const [showConfirm, setShowConfirm] = useState(false);
    const handleDelete = () => {
        if(showConfirm == false){
            setShowConfirm(true);
            return;
        }

        const form = formRef.current;
        const formData = new FormData(form);
        var data = {};
        formData.forEach((value, key) => {
            if(value > '')
                data[key] = value;
        });

        //console.log(data)
        fetch(`${url}/sensor/${data['id']}`, {
            method: 'DELETE'
        })
        .then(response => response.json()) // Parse the JSON body from the response
        .then(body => {
            if(body.status >= 300){
                setErrorMessage({ time: 3000, type: 'error', subject: "Delete sensor", message: "Sensor was deleted successfully" });
                setShowError(true);
            }else{
                CloseModal(); // Fechar o modal após o sucesso
            }
        })
        .catch((error) => {
            setErrorMessage({ time: 100000, type: 'error', subject: "Delete sensor", message: error });
        });
    }

    const CloseModal = () => {
        setShowError(false); //close the error message
        setShowModal(false); //close this modal
    }

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setChannel(prevChannel => ({
            ...prevChannel,
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

        fetch(`${url}/channel/${data['id']}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        })
        .then(response => {
            if (!response.ok) {
                // Lançar um erro se o status não estiver ok (2xx)
                return response.json().then(error => {
                    throw error;
                });
            }
            // Retornar o JSON se o status estiver ok
            return response.json();
        })
        .then(body => {
            //console.log('Success:', body); // Log the parsed body
            setShowModal(false); // Fechar o modal após o sucesso
        })
        .catch((error) => {
            console.error(error);
        });
    }



    return (
        <Modal
            show={showModal}
            size="lg"
            aria-labelledby="contained-modal-title-vcenter"
            centered
            >
            <ConfirmDialog show={showConfirm} setShow={setShowConfirm} executeFunction={handleDelete} message="Do you really want to delete the channel?" />
            <ErrorMessage show={showError} setShow={setShowError} time={errorMessage.time} type={errorMessage.type} message={errorMessage.message} subject={errorMessage.subject} />
            <Modal.Header closeButton onClick={() => CloseModal()}>
                <Modal.Title id="contained-modal-title-vcenter">Updating channel <b>{channel.name}</b> <p style={{fontSize: '11px'}}>{channel.id}</p></Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form ref={formRef}>
                <Form.Control type="text" name='id' value={channel.id} />

                <Row>
                    <Col>
                        <FloatingLabel label="Channel Name (Just an identification)" className="mb-3">
                            <Form.Control type="text" placeholder="Just an identification" autoFocus name='name' id='name' value={channel.name} onChange={handleInputChange} />
                        </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel label="Unit" className="mb-3">
                            <Form.Control type="text" placeholder="Unit" autoFocus name='unit' id='unit' value={channel.unit} onChange={handleInputChange} />
                        </FloatingLabel>
                    </Col>
                </Row>

                

                <Row>
                    <Col>
                    <FloatingLabel label="The channel is OK when it's value is ..." className="mb-3" >
                        <Form.Select id="success_orientation" name="success_orientation" style={{borderColor: 'green', borderWidth: '2px'}}>
                            {(channel.success_orientation == 0) ? <option selected value='0'>Disabled</option> : <option value='0'>Disabled</option>}
                            {(channel.success_orientation == 1) ? <option selected value='1'>Greater than ({'>'})</option> : <option value='1'>Greater than ({'>'})</option>}
                            {(channel.success_orientation == 1) ? <option selected value='2'>Greater than or equal ({'>='})</option> : <option value='2'>Greater than or equal ({'>='})</option>}
                            {(channel.success_orientation == 1) ? <option selected value='3'>Equal (==)</option> : <option value='3'>Equal (==)</option>}
                            {(channel.success_orientation == 1) ? <option selected value='4'>Less than or equal ({'<='})</option> : <option value='4'>Less than or equal ({'<='})</option>}
                            {(channel.success_orientation == 1) ? <option selected value='5'>Less than ({'<'})</option> : <option value='5'>Less than ({'<'})</option>}
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" id='success_value' name='success_value' value={channel.success_value} onChange={handleInputChange} style={{borderColor: 'green', borderWidth: '2px'}} />
                        </FloatingLabel>
                    </Col>
                </Row>
                <Row>
                    <Col>
                    <FloatingLabel label="The channel is WARNING when it's value is ..." className="mb-3" >
                        <Form.Select id="warning_orientation" name="warning_orientation" style={{borderColor: '#ffcd39', borderWidth: '2px'}}>
                            {(channel.warning_orientation == 0) ? <option selected value='0'>Disabled</option> : <option value='0'>Disabled</option>}
                            {(channel.warning_orientation == 1) ? <option selected value='1'>Greater than ({'>'})</option> : <option value='1'>Greater than ({'>'})</option>}
                            {(channel.warning_orientation == 2) ? <option selected value='2'>Greater than or equal ({'>='})</option> : <option value='2'>Greater than or equal ({'>='})</option>}
                            {(channel.warning_orientation == 3) ? <option selected value='3'>Equal (==)</option> : <option value='3'>Equal (==)</option>}
                            {(channel.warning_orientation == 4) ? <option selected value='4'>Less than or equal ({'<='})</option> : <option value='4'>Less than or equal ({'<='})</option>}
                            {(channel.warning_orientation == 5) ? <option selected value='5'>Less than ({'<'})</option> : <option value='5'>Less than ({'<'})</option>}
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" id='warning_value' name='warning_value' value={channel.warning_value} onChange={handleInputChange} style={{borderColor: '#ffcd39', borderWidth: '2px'}} />
                        </FloatingLabel>
                    </Col>
                </Row>
                <Row>
                    <Col>
                    <FloatingLabel label="The channel DANGER ok when it's value is ..." className="mb-3" >
                        <Form.Select name="danger_orientation" style={{borderColor: '#cd3545', borderWidth: '2px'}}>
                            {(channel.danger_orientation == 0) ? <option selected value='0'>Disabled</option> : <option value='0'>Disabled</option>}
                            {(channel.danger_orientation == 1) ? <option selected value='1'>Greater than ({'>'})</option> : <option value='1'>Greater than ({'>'})</option>}
                            {(channel.danger_orientation == 2) ? <option selected value='2'>Greater than or equal ({'>='})</option> : <option value='2'>Greater than or equal ({'>='})</option>}
                            {(channel.danger_orientation == 3) ? <option selected value='3'>Equal (==)</option> : <option value='3'>Equal (==)</option>}
                            {(channel.danger_orientation == 4) ? <option selected value='4'>Less than or equal ({'<='})</option> : <option value='4'>Less than or equal ({'<='})</option>}
                            {(channel.danger_orientation == 5) ? <option selected value='5'>Less than ({'<'})</option> : <option value='5'>Less than ({'<'})</option>}
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" name='danger_value' value={channel.danger_value} onChange={handleInputChange} style={{borderColor: '#cd3545', borderWidth: '2px'}} />
                        </FloatingLabel>
                    </Col>
                </Row>
                

                </Form>
            </Modal.Body>
            <Modal.Footer style={{ display: 'flex', justifyContent: 'space-between' }}>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <Button variant='danger' onClick={() => handleDelete()}>Delete</Button>
                </div>
                <div style={{ display: 'flex', gap: '10px' }}>
                    <Button variant='outline-dark' onClick={() => CloseModal()}>Cancel</Button>
                    <Button variant='success' onClick={() => handleForm()}>Save</Button>
                </div>
            </Modal.Footer>
            </Modal>
    ); 
}

export default EditChannel;