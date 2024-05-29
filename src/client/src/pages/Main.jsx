import React, { useState, useEffect } from 'react';
import logo from './../logo.svg';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Col from 'react-bootstrap/Col'

import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Tooltip from 'react-bootstrap/Tooltip';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
import Badge from 'react-bootstrap/Badge';

import Toasty from '../components/Toasty';
import Channel from '../components/Channel';



function Main() {

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        // URL da API
        const url = 'https://localhost:5001/Api';
    
        // Função para buscar dados
        const fetchData = async () => {
          try {
            const response = await fetch(url);
    
            if (!response.ok) {
              throw new Error(`Erro: ${response.status}`);
            }
    
            const jsonData = await response.json();
            setData(jsonData);
          } catch (error) {
            setError(error.message);
          } finally {
            setLoading(false);
          }
        };
    
        // Configura um intervalo para buscar dados a cada segundo
        const intervalId = setInterval(fetchData, 1000);
    
        // Chama a função fetchData imediatamente na montagem
        fetchData();
    
        // Limpa o intervalo quando o componente desmontar
        return () => clearInterval(intervalId);
      }, []); 
    
    if (loading) {
        return <div>Carregando...</div>;
    }

    if (error) {
        return <div>Erro: {error}</div>;
    }


    // Funções de contagem de sensor
    const countChannelsInSensor = (sensor) => {
        return sensor.channels.length+1;
    };
    const countChannelsInSensor_Error = (sensor) => {
        return sensor.channels.filter(channel => channel.current_metric?.error_code > 0).length;
    };

    const countChannelsInSensor_Success = (sensor) => {
        return sensor.channels.filter(channel => channel.current_metric?.error_code == 0).length;
    };

  
    // Funções de contagem de serviço
    const countChannelsInService = (service) => {
        return service.sensors.reduce((sensorTotal, sensor) => {
        return sensorTotal + countChannelsInSensor(sensor);
        }, 1);
    };

    const countChannelsInService_Error = (service) => {
        return service.sensors.reduce((sensorTotal, sensor) => {
        return sensorTotal + countChannelsInSensor_Error(sensor);
        }, 0);
    };

    const countChannelsInService_Success = (service) => {
        return service.sensors.reduce((sensorTotal, sensor) => {
        return sensorTotal + countChannelsInSensor_Success(sensor);
        }, 0);
    };
  


    // Funções de contagem de stack
    const countChannelsInStack = (stack) => {
        return stack.services.reduce((serviceTotal, service) => {
        return serviceTotal + countChannelsInService(service);
        }, 1);
    };

    const countChannelsInStack_Error = (stack) => {
        return stack.services.reduce((serviceTotal, service) => {
        return serviceTotal + countChannelsInService_Error(service);
        }, 0);
    };

    const countChannelsInStack_Success = (stack) => {
        return stack.services.reduce((serviceTotal, service) => {
        return serviceTotal + countChannelsInService_Success(service);
        }, 0);
    };


    //funções de contagem para os contadores superiores
    const countChannelsInAllStacks = (data) => {
        return data.reduce((stackTotal, stack) => {
            return stackTotal + countChannelsInStack(stack);
        }, 0);
    };

    const countChannelsInAllStacks_Success = (data) => {
        return data.reduce((stackTotal, stack) => {
            return stackTotal + countChannelsInStack_Success(stack);
        }, 0);
    };

    const countChannelsInAllStacks_Error = (data) => {
        return data.reduce((stackTotal, stack) => {
            return stackTotal + countChannelsInStack_Error(stack);
        }, 0);
    };



    return (
    <>
        <Container style={{marginTop: '15px'}}>
            <Row>
                <Toasty type="success" value={countChannelsInAllStacks_Success(data)} text={"Ok"} title="Number of channels online"></Toasty>
                <Toasty type="warning" value="Fake" text={"Warnning"} title="Number of channels with some alert"></Toasty>
                <Toasty type="danger"  value={countChannelsInAllStacks_Error(data)} text={"Error"} title="Number of channels offline / error"></Toasty>
            </Row>
        </Container>
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
                                                {/* <td>{service.name}</td>
                                                <td>-</td>
                                                <td>-</td>
                                                <td>-</td>
                                                <td>-</td>
                                                <td>-</td>
                                                <td style={{color:"red"}}>The SENSORS have not been added yet. Add it manually</td> */}
                                                <td>-</td>
                                                <td>-</td>
                                                <td colSpan={5}>
                                                    <Row>
                                                        
                                                            <Channel value={10} title={"HTTPS - Site - Days for certifier expire"} type="danger" unit="days"></Channel>
                                                        
                                                            <Channel value={200} title={"HTTPS - Site - SSL check on"} type="warning" unit=""></Channel>
                                                        
                                                            <Channel value={3} title={"Ping"} type="success" unit="ms"></Channel>
                                                        
                                                    </Row>
                                                </td>
                                            </tr>
                                        
                                    }
                                    
                                    


                                    
                                    {
                                        service.sensors.map((sensor, index3) => (
                                            <>
                                                {
                                                    (countChannelsInSensor(sensor) > 1)
                                                    ?
                                                <tr>
                                                    <td rowSpan={countChannelsInSensor(sensor)}>{sensor.name} {(countChannelsInSensor_Error(sensor) > 0 ? <Badge bg="danger">{countChannelsInSensor_Error(sensor)}</Badge> : <></>)} </td>
                                                </tr>
                                                    :
                                                    <>
                                                        <tr>
                                                            <td>{sensor.name}</td>
                                                            <td>-</td>
                                                            <td>-</td>
                                                            <td>-</td>
                                                            <td>-</td>
                                                            <td style={{color:"red"}}>The CHANNELS have not been created yet</td>
                                                        </tr>
                                                        </>
                                                }
                                                
                                                {
                                                    sensor.channels.map((channel, index4) => (
                                                        <>
                                                            <tr key={index4}>
                                                            
                                                            {/* <td><b style={{color:"red"}}>{sensor.name}</b></td> */}
                                                            <td>{channel.name}</td>
                                                            <td>{sensor.type}{(sensor.port == null) ? "" :  " / "+sensor.port}</td>
                                                            <td>{channel.current_metric.latency} ms</td>
                                                            <td>
                                                                <Button size="sm" style={{width: "100px"}} variant={(channel.current_metric.error_code > 0) ? "danger" : "success"}>
                                                                    {channel.current_metric.value} {channel.unit}</Button>
                                                                </td>
                                                                <td style={(channel.current_metric.error_code > 0) ? {color: "red", fontWeight: "bold", textWrap: "nowrap"} : {color: "green"}}>
                                                                    <OverlayTrigger placement="left" delay={{ show: 100, hide: 400 }} overlay={<Tooltip id="button-tooltip-2">{channel.current_metric.message}</Tooltip>}>
                                                                        <text>{(channel.current_metric.message.length > 50) ? channel.current_metric.message.substr(0, 50) + ' ...': channel.current_metric.message}</text>
                                                                    </OverlayTrigger>
                                                                </td>
                                                            </tr>
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

export default Main;
