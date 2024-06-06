import React, { useState } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Table from 'react-bootstrap/Table';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import Badge from 'react-bootstrap/Badge';

import Channel from '../components/Channel';
import EditStack from '../components/EditStack';
import EditDevice from '../components/EditDevice';
import EditSensor from '../components/EditSensor';

import Navigation from '../components/Navigation';
import CalcChannels, {
    countChannelsInStack, 
    countChannelsInStack_Error, 

    countChannelsInDevice, 
    countChannelsInDevice_Error, 

    countChannelsInSensor, 
    countChannelsInSensor_Error, 
    } from '../components/CalcChannels';

import GetMainData from '../components/GetMainData';
import Tips from '../components/Tips';

import { FaPlus } from "react-icons/fa6";
import { FaSortUp, FaSortDown } from "react-icons/fa";
import { TiEdit } from "react-icons/ti";







function Main() {

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
                        {error} - Verify the connection
                    </Alert>
                </div>
                
            </>
        );
    }

    
    
    
    



    return (
    <>
        <Navigation data={data} />
        {/* <ToastyGroup success={countChannelsInAllStacks_Success(data)} warning={"Fake"} error={countChannelsInAllStacks_Error(data)}/> */}
        <Container fluid style={{marginTop: '15px'}}>
        <EditStack stack={editStack} setStack={setEditStack} showModal={showModalStack} setShowModal={setShowModalStack} />
        <EditDevice stack={editStack} device={editDevice} setDevice={setDevice} showModal={showModalDevice} setShowModal={setShowModalDevice} />
        <EditSensor device={editDevice} sensor={editSensor} setDevice={setSensor} showModal={showModalSensor} setShowModal={setShowModalSensor} />
        
        <Table bordered hover size="sm" responsive="lg">
            <thead>
                <tr>
                <th>Stack <Tips message="Add new stack"><FaPlus onClick={() => handleEditStack({name: null, id: null})} /></Tips></th>
                <th>Device</th>
                <th>Sensor</th>
                <th>Channel</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        {
                            (stack.devices.length == 0)
                            ?
                            <tr>
                                <td>
                                    <TiEdit onClick={() => handleEditStack(stack)} />
                                    <FaSortUp /> 
                                    <FaSortDown /> 
                                    <a> {stack.name} </a>
                                </td>
                                <td><FaPlus onClick={() => handleEditDevice({stack})} /></td>
                                <td>-</td>
                                <td>-</td>
                            </tr>
                            :
                            <tr>
                                <td key={index} rowSpan={countChannelsInStack(stack)}>
                                    <TiEdit onClick={() => handleEditStack(stack)} />
                                    <FaSortUp /> 
                                    <FaSortDown /> 
                                    <a> {stack.name} </a>
                                    {(countChannelsInStack_Error(stack) > 0 ? <Badge bg="danger">{countChannelsInStack_Error(stack)}</Badge> : <></>)} 
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
                                                    <TiEdit onClick={() => handleEditDevice({device})} /> 
                                                    <FaSortUp />
                                                    <FaSortDown /> 
                                                    {device.name} 
                                                    {(countChannelsInDevice_Error(device) > 0 ? <Badge bg="danger">{countChannelsInDevice_Error(device)}</Badge> : <></>)} 
                                                </td>
                                            </tr>
                                        :
                                            <tr>
                                                <td>
                                                    <TiEdit onClick={() => handleEditDevice({device})} /> 
                                                    <FaSortUp />
                                                    <FaSortDown />
                                                    {device.name}
                                                </td>
                                                <td><FaPlus onClick={() => handleEditSensor({device})} />Implementar adicionar sensor</td>
                                                <td>-</td>
                                            </tr>
                                        
                                    }

                                    {
                                        device.sensors.map((sensor, index3) => (
                                            <>
                                                <tr>
                                                    <td rowSpan_FAKE={countChannelsInSensor(sensor)}>{sensor.name} {(countChannelsInSensor_Error(sensor) > 0 ? <Badge bg="danger">{countChannelsInSensor_Error(sensor)}</Badge> : <></>)} </td>
                                                    <td colSpan={5}>
                                                        <Row>
                                                            {
                                                                sensor.channels.map((channel, index4) => (
                                                                    <Channel id={channel.id} value={channel.current_metric.value} title={channel.name} type="success" unit={channel.unit}></Channel>
                                                                ))
                                                            }
                                                        </Row>
                                                    </td>
                                                </tr>
                                            </>
                                        ))
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
