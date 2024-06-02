import React, { useState } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Table from 'react-bootstrap/Table';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import Badge from 'react-bootstrap/Badge';

import Channel from '../components/Channel';
import EditStack from '../components/EditStack';

import Navigation from '../components/Navigation';
import CalcChannels, {
    countChannelsInStack, 
    countChannelsInStack_Error, 

    countChannelsInService, 
    countChannelsInService_Error, 

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

    const [showModalService, setShowModalService] = useState(false);
    const [editService, setEditService] = useState(0);
    const handleEditService = (Service) => {
        setEditService(Service);
        setShowModalService(true);
    }

    const [showModalSensor, setShowModalSensor] = useState(false);
    const [editSensor, setEditSensor] = useState(0);
    const handleEditSensor = (Sensor) => {
        setEditSensor(Sensor);
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
        
        <Table bordered hover size="sm" responsive="lg">
            <thead>
                <tr>
                <th>Stack   <Tips message="Add new stack"><FaPlus onClick={() => handleEditStack({name: null, id: null})} /></Tips></th>
                <th>Service <Tips message="Add new service"><FaPlus onClick={() => setShowModalService(true)} /></Tips></th>
                <th>Sensor  <Tips message="Add new sensor"><FaPlus onClick={() => setShowModalSensor(true)} /></Tips></th>
                <th>Channel</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        {
                            (stack.services.length == 0)
                            ?
                            <tr>
                                <td>
                                    <TiEdit onClick={() => handleEditStack(stack)} /> 
                                    <FaSortUp /> 
                                    <FaSortDown /> 
                                    <a> {stack.name} </a>
                                </td>
                                <td><FaPlus onClick={() => setShowModalStack(true)} /></td>
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
                            stack.services.map((service, index2) => (
                                <>
                                    {
                                        (countChannelsInService(service) > 1)
                                        ?
                                    <tr>
                                        <td key={index2} rowSpan={countChannelsInService(service)}>{service.name} {(countChannelsInService_Error(service) > 0 ? <Badge bg="danger">{countChannelsInService_Error(service)}</Badge> : <></>)} </td>
                                    </tr>
                                        :
                                            <tr>
                                                <td><FaPlus onClick={() => setShowModalStack(true)} /></td>
                                                <td>-</td>
                                                <td>-</td>
                                            </tr>
                                        
                                    }

                                    {
                                        service.sensors.map((sensor, index3) => (
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
