import React, { useState, useEffect } from 'react';

import Container from 'react-bootstrap/Container';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Tooltip from 'react-bootstrap/Tooltip';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';

import ToastyGroup from '../components/ToastyGroup';

import Navigation from '../components/Navigation';
import CalcChannels, { countChannelsInAllStacks_Success, countChannelsInAllStacks_Error } from '../components/CalcChannels';

import GetMainData from '../components/GetMainData';

function ListErrors() {

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    GetMainData({setData, setLoading, setError});
    
    if (loading) {
        return <div>Carregando...</div>;
    }

    if (error) {
        return <div>Erro: {error}</div>;
    }


    



    return (
    <>
        <Navigation data={data} />
        <ToastyGroup success={countChannelsInAllStacks_Success(data)} warning={"Fake"} error={countChannelsInAllStacks_Error(data)}/>

        <Container fluid style={{marginTop: '15px'}}>
            
        <Table bordered hover size="sm">
            <thead>
                <tr>
                <th>Stack</th>
                <th>Service</th>
                <th>Sensor</th>
                <th>Channel</th>
                <th>Type/Port</th>
                <th style={{width: "100px"}}>Latency</th>
                <th>Value</th>
                <th style={{width: "600px"}}>Message</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        {
                            stack.services.map((service, index2) => (
                                <>
                                    {
                                        service.sensors.map((sensor, index3) => (
                                            <>
                                                {
                                                    sensor.channels.map((channel, index4) => (
                                                        <>
                                                            {
                                                                (channel.current_metric.status == 'Success')
                                                                ?
                                                                <>
                                                                    <tr key={index4}>
                                                                    <td>{stack.name}</td>
                                                                    <td>{service.name}</td>
                                                                    <td>{sensor.name}</td>
                                                                    <td>{channel.name}</td>
                                                                    <td>{sensor.type}{(sensor.port == null) ? "" :  " / "+sensor.port}</td>
                                                                    <td>{channel.current_metric.latency} ms</td>
                                                                    <td>
                                                                        <Button size="sm" style={{width: "100px"}} variant={(channel.current_metric.status == 'Error') ? "danger" : "success"}>
                                                                            {channel.current_metric.value} {channel.unit}</Button>
                                                                        </td>
                                                                        <td style={(channel.current_metric.status == 'Error') ? {color: "red", fontWeight: "bold", textWrap: "nowrap"} : {color: "green"}}>
                                                                            <OverlayTrigger placement="left" delay={{ show: 100, hide: 400 }} overlay={<Tooltip id="button-tooltip-2">{channel.current_metric.message}</Tooltip>}>
                                                                                <text>{(channel.current_metric.message.length > 50) ? channel.current_metric.message.substr(0, 50) + ' ...': channel.current_metric.message}</text>
                                                                            </OverlayTrigger>
                                                                        </td>
                                                                    </tr>
                                                                </>
                                                                :
                                                                <></>
                                                            }
                                                            
                                                        </>
                                                    ))
                                                }
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

export default ListErrors;
