import { useState, useContext, useRef} from 'react';

import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';


import { AppContext } from '../AppContext';
import ErrorMessage from '../components/ErrorMessage';
import ConfirmDialog from '../components/ConfirmDialog';



function EditChannel({showModal, setShowModal, channel}) {
    const {settings} = useContext(AppContext);
    var url = settings.backend_url;

    const formRef = useRef(null);


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
        setSensor(prevSensor => ({
            ...prevSensor,
            [name]: value
        }));
    };

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

                <FloatingLabel controlId="floatingInput" label="Channel Name (Just an identification)" className="mb-3">
                    <Form.Control type="text" placeholder="Just an identification" autoFocus name='name' value={channel.name} onChange={handleInputChange} />
                </FloatingLabel>

                <Row>
                    <Col>
                    <FloatingLabel controlId="floatingInput" label="The channel is OK when it's value is ..." className="mb-3" >
                        <Form.Select id="ok" name="type" style={{borderColor: 'green', borderWidth: '2px'}}>
                                <option value='>'>Greater than ({'>'})</option>
                                <option value='>='>Greater than or equal ({'>='})</option>
                                <option value='='>Equal (==)</option>
                                <option value='<='>Less than or equal ({'<='})</option>
                                <option value='<'>Less than ({'<'})</option>
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel controlId="floatingInput" label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" name='ok_value' value={channel.port} onChange={handleInputChange} style={{borderColor: 'green', borderWidth: '2px'}} />
                        </FloatingLabel>
                    </Col>
                </Row>
                <Row>
                    <Col>
                    <FloatingLabel controlId="floatingInput" label="The channel is WARNING when it's value is ..." className="mb-3" >
                        <Form.Select id="warning" name="type" style={{borderColor: '#ffcd39', borderWidth: '2px'}}>
                                <option value='>'>Greater than ({'>'})</option>
                                <option value='>='>Greater than or equal ({'>='})</option>
                                <option value='='>Equal (==)</option>
                                <option value='<='>Less than or equal ({'<='})</option>
                                <option value='<'>Less than ({'<'})</option>
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel controlId="floatingInput" label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" name='warning_value' value={channel.port} onChange={handleInputChange} style={{borderColor: '#ffcd39', borderWidth: '2px'}} />
                        </FloatingLabel>
                    </Col>
                </Row>
                <Row>
                    <Col>
                    <FloatingLabel controlId="floatingInput" label="The channel DANGER ok when it's value is ..." className="mb-3" >
                        <Form.Select id="danger" name="type" style={{borderColor: '#cd3545', borderWidth: '2px'}}>
                                <option value='>'>Greater than ({'>'})</option>
                                <option value='>='>Greater than or equal ({'>='})</option>
                                <option value='='>Equal (==)</option>
                                <option value='<='>Less than or equal ({'<='})</option>
                                <option value='<'>Less than ({'<'})</option>
                        </Form.Select>
                    </FloatingLabel>
                    </Col>
                    <Col>
                        <FloatingLabel controlId="floatingInput" label={'Value in ' + channel.unit} className="mb-3">
                            <Form.Control type="number" name='danger_value' value={channel.unit} onChange={handleInputChange} style={{borderColor: '#cd3545', borderWidth: '2px'}} />
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