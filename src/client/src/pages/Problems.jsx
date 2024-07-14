import React, { useState } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
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
import {
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







function Problems() {

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
                        {error} - Verify the connection
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
    
        
        <Table bordered hover size="sm" responsive="lg">
            <thead>
                <tr>
                    <th>Stack <Tips message="Add new stack"><FaPlus onClick={() => handleEditStack({stack: {name: null, id: null}})} /></Tips></th>
                    <th>Device</th>
                    <th>Sensors / Channels</th>
                </tr>
            </thead>
            <tbody>
                {
                    data.map((stack, index) => (
                        stack.devices.map((device, index2) => (
                            device.sensors.map((sensor, index3) => (
                                    
                                    (countChannelsInDevice_Error(device) > 0)
                                    ?
                                        <tr key={index} >
                                            <td>{stack.name}</td>
                                            <td key={index2}>{device.name} - <small class="text-body-secondary">{device.host}</small></td>
                                            <td key={index3} >
                                                <Row style={{marginLeft: '12px'}}>
                                                    <Row>{sensor.name}</Row>
                                                    <Row xs={2} md={4} lg={6} style={{borderStyle: "solid", borderWidth: "1px", borderColor: "#D3D3D3", borderRadius: "9px", 
                                                        padding: "3px", 
                                                        boxShadow: "0 4px 8px rgba(0, 0, 0, 0.1)"
                                                        }} >
                                                        {
                                                            sensor.channels.map((channel, index4) => (
                                                                <Channel key={index4} readOnlyMode={readOnlyMode} channel={channel}></Channel>
                                                            ))
                                                        }
                                                    </Row>
                                                </Row> 
                                            </td>
                                        </tr>
                                    :<></>
                                ))
                            ))
                    ))
                }
            </tbody>
        </Table>
        </Container>
    </>
    
    
  );
}

export default Problems;
