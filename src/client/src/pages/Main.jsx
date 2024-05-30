import React, { useState, useEffect, use } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Table from 'react-bootstrap/Table';
import Alert from 'react-bootstrap/Alert';
import Spinner from 'react-bootstrap/Spinner';
import Badge from 'react-bootstrap/Badge';

import ToastyGroup from '../components/ToastyGroup';
import Channel from '../components/Channel';

import Navigation from '../components/Navigation';
import CalcChannels, {

    countChannelsInAllStacks_Success, 
    countChannelsInAllStacks_Error,

    countChannelsInStack, 
    countChannelsInStack_Error, 

    countChannelsInService, 
    countChannelsInService_Error, 

    countChannelsInSensor, 
    countChannelsInSensor_Error, 
    } from '../components/CalcChannels';

import GetMainData from '../components/GetMainData';
import Col from 'react-bootstrap/esm/Col';


function Main() {

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    GetMainData({setData, setLoading, setError});
    

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
        
        <Table bordered hover size="sm" responsive="lg">
            <thead>
                <tr>
                <th>Stack</th>
                <th>Service</th>
                <th>Sensor</th>
                <th>Channel</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        <tr>
                            <td key={index} rowSpan={countChannelsInStack(stack)}>{stack.name} {(countChannelsInStack_Error(stack) > 0 ? <Badge bg="danger">{countChannelsInStack_Error(stack)}</Badge> : <></>)} </td>
                        </tr>
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
                                                <td>-</td>
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
