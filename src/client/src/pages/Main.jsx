import React, { useState } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'
import Table from 'react-bootstrap/Table';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import Badge from 'react-bootstrap/Badge';
import Button from 'react-bootstrap/Button';

import Channel from '../components/Channel';
import EditStack from '../components/EditStack';
import EditDevice from '../components/EditDevice';
import EditSensor from '../components/EditSensor';

import Navigation from '../components/Navigation';
import CalcChannels, {
    countChannelsInStack, 
    countChannelsInStack_Error, 

    countChannelsInDevice, 
    countChannelsInDevice_Error
    } from '../components/CalcChannels';

import GetMainData from '../components/GetMainData';
import Tips from '../components/Tips';

import { FaPlus } from "react-icons/fa6";
import { FaSortUp, FaSortDown } from "react-icons/fa";
import { TiEdit } from "react-icons/ti";







function Main() {

    const [readOnlyMode, setReadOnlyMode] = useState(true);

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    GetMainData({setData, setLoading, setError});
    
    const [showModalStack, setShowModalStack] = useState(false);
    const [editStack, setEditStack] = useState(0);
    const handleEditStack = (stack) => {
        setEditStack(stack);
        setShowModalStack(true);
    }

    const [showModalDevice, setShowModalDevice] = useState(false);
    const [editDevice, setDevice] = useState(0);
    const handleEditDevice = ({stack, device}) => {
        console.log('clicou')
        if(stack != undefined){
            setEditStack(stack);
        }

        if(device == undefined)
            device = {}; //zera o device pois trata-se de um novo
        setDevice(device);
        
        setShowModalDevice(true);
    }

    const [showModalSensor, setShowModalSensor] = useState(false);
    const [editSensor, setSensor] = useState(0);
    const handleEditSensor = ({device, sensor}) => {
        if(device != undefined){
            setDevice(device);
        }

        if(sensor == undefined)
            sensor = {}; //zera o device pois trata-se de um novo
        setSensor(sensor);
        
        setShowModalSensor(true);
    }



    if (loading) {
        return (
            <>
                <div style={{position: 'fixed', left: '50%', top: '50%'}}>
                    <Spinner animation="border" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </Spinner>
                </div>
            </>
        );
    }

    if (error) {
        return (
            <>
                <div style={{position: 'fixed', left: '40%', top: '45%'}}>
                    <Alert variant='danger'>
                        {error} - Verify the connection and the backend's container
                    </Alert>
                </div>
                
            </>
        );
    }

    
    
    
    



    return (
    <>
        <Navigation data={data} readOnlyMode={readOnlyMode} setReadOnlyMode={setReadOnlyMode} />
        {/* <ToastyGroup success={countChannelsInAllStacks_Success(data)} warning={"Fake"} error={countChannelsInAllStacks_Error(data)}/> */}
        <Container fluid style={{marginTop: '15px'}}>
        <EditStack stack={editStack} setStack={setEditStack} showModal={showModalStack} setShowModal={setShowModalStack} />
        <EditDevice stack={editStack} device={editDevice} setDevice={setDevice} showModal={showModalDevice} setShowModal={setShowModalDevice} />
        <EditSensor device={editDevice} sensor={editSensor} setSensor={setSensor} showModal={showModalSensor} setShowModal={setShowModalSensor} />
        
        <Table bordered hover size="sm" responsive="lg">
            <thead>
                <tr>
                    <th>Stack <Tips message="Add new stack"><FaPlus onClick={() => handleEditStack({stack: {name: null, id: null}})} /></Tips></th>
                    <th>Device</th>
                    <th>Sensors / Channels</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        {
                            (stack.devices.length == 0)
                            ?
                            <tr key={index} >
                                <td style={{height: '60px'}}>
                                    <div style={{position: "absolute", width: "20px", height: "90%"}}>
                                        <FaSortUp />
                                        <FaSortDown />
                                    </div>
                                    <div style={{position: "absolute", marginLeft: "20px", width: "20px", height: "90%"}}>
                                        <TiEdit onClick={() => handleEditStack(stack)} />
                                    </div>
                                    <div className="position-relative ml-3" style={{marginLeft: "40px", height: "90%"}}>
                                        <a> {stack.name} {stack.id}</a>
                                    </div>
                                </td>
                                <td>
                                    {
                                        (readOnlyMode == false) ? <Button variant="primary" onClick={() => handleEditDevice({stack})}><FaPlus />Add new device</Button> : <>-</>
                                    }
                                </td>
                                <td>-</td>
                            </tr>
                            :
                            <tr key={index} >
                                <td style={{height: '60px'}} rowSpan={countChannelsInStack(stack)}>
                                    {
                                        (readOnlyMode == false)
                                        ? 
                                            <>
                                                <div style={{position: "absolute", width: "20px", height: "90%"}}>
                                                    <FaSortUp />
                                                    <FaSortDown />
                                                </div>
                                                <div style={{position: "absolute", marginLeft: "20px", width: "20px", height: "90%"}}>
                                                    <TiEdit onClick={() => handleEditStack(stack)} />
                                                </div>
                                                <div className="position-relative ml-3" style={{marginLeft: "40px", height: "90%"}}>
                                                    <a> {stack.name} </a>
                                                    {(countChannelsInStack_Error(stack) > 0 ? <Badge bg="danger">{countChannelsInStack_Error(stack)}</Badge> : <></>)} 
                                                    <div style={{position: 'absolute', bottom: '0', right: '0'}}>
                                                        <Button style={{height: '25px', padding: '0px 6px 6px 0px'}} variant="primary" size="sm" onClick={() => handleEditDevice({stack})}><FaPlus style={{padding: '3px'}} />Device</Button>
                                                    </div>
                                                </div>
                                            </>
                                        : 
                                            <>
                                                <a> {stack.name} </a>
                                                {(countChannelsInStack_Error(stack) > 0 ? <Badge bg="danger">{countChannelsInStack_Error(stack)}</Badge> : <></>)} 
                                            </>
                                    }
                                    
                                </td>
                            </tr>
                        }
                        
                        {
                            stack.devices.map((device, index2) => (
                                <>
                                    {
                                        (countChannelsInDevice(device) > 1)
                                        ?
                                            <tr>
                                                <td key={index2} rowSpan={countChannelsInDevice(device)}>
                                                {
                                                    (readOnlyMode == false)
                                                    ?
                                                    <>
                                                        <div style={{position: "absolute", width: "20px", height: "90%"}}>
                                                            <FaSortUp />
                                                            <FaSortDown />
                                                        </div>
                                                        <div style={{position: "absolute", marginLeft: "20px", width: "20px", height: "0%"}}>
                                                            <TiEdit onClick={() => handleEditDevice({device})} /> 
                                                        </div>
                                                        <div className="position-relative ml-3" style={{marginLeft: "40px", height: "90%"}}>
                                                            <a> {device.name} - <small class="text-body-secondary">{device.host}</small> </a>
                                                            {(countChannelsInDevice_Error(device) > 0 ? <Badge bg="danger">{countChannelsInDevice_Error(device)}</Badge> : <></>)} 
                                                        </div>
                                                    </>
                                                    :
                                                    <>
                                                        <a> {device.name} - <small class="text-body-secondary">{device.host}</small> </a>
                                                        {(countChannelsInDevice_Error(device) > 0 ? <Badge bg="danger">{countChannelsInDevice_Error(device)}</Badge> : <></>)} 
                                                    </>
                                                }
                                                </td>
                                            </tr>
                                        :
                                            <tr>
                                                <td>
                                                    {
                                                        (readOnlyMode == false)
                                                        ?
                                                            <>
                                                                <div style={{position: "absolute", width: "20px", height: "90%"}}>
                                                                    <FaSortUp />
                                                                    <FaSortDown />
                                                                </div>
                                                                <div style={{position: "absolute", marginLeft: "20px", width: "20px", height: "90%"}}>
                                                                    <TiEdit onClick={() => handleEditDevice({device})} /> 
                                                                </div>
                                                                <div className="position-relative ml-3" style={{marginLeft: "40px", height: "90%"}}>
                                                                    <a> {device.name} - <small class="text-body-secondary">{device.host}</small> </a>
                                                                    {(countChannelsInDevice_Error(device) > 0 ? <Badge bg="danger">{countChannelsInDevice_Error(device)}</Badge> : <></>)} 
                                                                </div>
                                                            </>
                                                        :
                                                        <>
                                                            <a> {device.name} - <small class="text-body-secondary">{device.host}</small> </a>
                                                            {(countChannelsInDevice_Error(device) > 0 ? <Badge bg="danger">{countChannelsInDevice_Error(device)}</Badge> : <></>)} 
                                                        </>
                                                    }
                                                    
                                                </td>
                                                <td>
                                                    {
                                                        (readOnlyMode == false)
                                                        ?
                                                        <Button variant="primary" onClick={() => handleEditSensor({device})}><FaPlus />Add new sensor</Button>
                                                        :
                                                        <>-</>
                                                    }
                                                    
                                                </td>
                                            </tr>
                                        
                                    }
                                    
                                        
                                        {
                                            (device.sensors.length > 0)
                                            ?
                                                <tr>
                                                    <td>
                                                        {
                                                            (readOnlyMode == false)
                                                            ?
                                                            <Button style={{height: '25px', width: "55px", padding: '0px 6px 6px 0px', marginLeft: "15px", position: "absolute", right: 24, marginTop: -3, zIndex: 100}} 
                                                                variant="primary" size="sm" onClick={() => handleEditSensor({device})}>
                                                                <FaPlus style={{padding: '3px'}} /> Add
                                                            </Button>
                                                            :
                                                            <></>
                                                        }
                                                        
                                                        {
                                                            device.sensors.map((sensor, index3) => (
                                                                <>
                                                                    {
                                                                        (readOnlyMode == false)
                                                                        ?
                                                                            <>
                                                                                <div style={{position: "absolute", width: "20px", height: "90%"}}>
                                                                                    <FaSortUp />
                                                                                    <FaSortDown />
                                                                                </div>
                                                                                <div style={{position: "absolute", marginLeft: "20px", width: "20px", height: "90%"}}>
                                                                                    <TiEdit onClick={() => handleEditSensor({sensor})} title="Edit the sensor" /> 
                                                                                </div>
                                                                                <div className="position-relative ml-3" style={{marginLeft: "40px", height: "90%"}}>
                                                                                    <Row style={{marginLeft: '12px'}}>
                                                                                        <Row>{sensor.name}</Row>
                                                                                        <Row xs={2} md={4} lg={6} style={{borderStyle: "solid", borderWidth: "1px", borderColor: "#D3D3D3", borderRadius: "9px", 
                                                                                            padding: "3px", 
                                                                                            boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)"
                                                                                            }} >
                                                                                            {
                                                                                                
                                                                                                sensor.channels.map((channel, index4) => (
                                                                                                    <Channel readOnlyMode={readOnlyMode} channel={channel} sensor={sensor} />
                                                                                                ))
                                                                                            }
                                                                                        </Row>
                                                                                    </Row> 
                                                                                </div>
                                                                            </>
                                                                        :
                                                                        <>
                                                                            <Row style={{marginLeft: '12px'}}>
                                                                                <Row>{sensor.name}</Row>
                                                                                <Row xs={2} md={4} lg={6} style={{borderStyle: "solid", borderWidth: "1px", borderColor: "#D3D3D3", borderRadius: "9px", 
                                                                                    padding: "3px", 
                                                                                    boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)"
                                                                                    }} >
                                                                                    {
                                                                                        sensor.channels.map((channel, index4) => (
                                                                                            <Channel readOnlyMode={readOnlyMode} channel={channel} sensor={sensor}></Channel>
                                                                                        ))
                                                                                    }
                                                                                </Row>
                                                                            </Row> 
                                                                        </>
                                                                    }
                                                                       
                                                                </>
                                                            ))
                                                        }
                                                    </td>
                                                </tr>
                                            :
                                            <></>
                                        }

                                
                                </>
                            ))
                        }
                    </>
                ))}
            </tbody>
        </Table>
        </Container>
    </>
    
    
  );
}

export default Main;
