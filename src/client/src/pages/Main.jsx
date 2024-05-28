import React, { useState, useEffect } from 'react';
import logo from './../logo.svg';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';

import Card from 'react-bootstrap/Card';
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Table from 'react-bootstrap/Table';



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


    // Funções de contagem
const countChannelsInSensor = (sensor) => {
    return sensor.channels.length;
  };
  
  const countChannelsInService = (service) => {
    return service.sensors.reduce((sensorTotal, sensor) => {
      return sensorTotal + countChannelsInSensor(sensor);
    }, 1);
  };
  
  const countChannelsInStack = (stack) => {
    return stack.services.reduce((serviceTotal, service) => {
      return serviceTotal + countChannelsInService(service);
    }, 1);
  };

    return (
    <>
        <Container>
        <Table striped bordered hover>
            <thead>
                <tr>
                <th>Stack</th>
                <th>Service</th>
                <th>Sensor</th>
                <th>Channel</th>
                <th>Type/Port</th>
                <th>Latency</th>
                <th>Value</th>
                <th>Message</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        <tr>
                            <td key={index} rowSpan={countChannelsInStack(stack)}><b>{stack.name}</b></td>
                        </tr>
                        {
                            stack.services.map((service, index2) => (
                                <>
                                    <tr>
                                        <td key={index2} rowSpan={countChannelsInService(service)}><b style={{color:"blue"}}>{service.name}</b></td>
                                    </tr>
                                    {
                                        service.sensors.map((sensor, index3) => (
                                            <>
                                                {/* <tr>
                                                    <td rowspan={countChannelsInSensor(sensor)+1}><b style={{color:"red"}}>{sensor.name}</b> - {countChannelsInSensor(sensor)}</td>
                                                </tr> */}
                                                
                                                {
                                                    sensor.channels.map((channel, index4) => (
                                                        <>
                                                            <tr key={index4}>
                                                            <td><b style={{color:"red"}}>{sensor.name}</b></td>
                                                            <td><b style={{color:"green"}}>{channel.name}</b></td>
                                                            <td>{sensor.type}{(sensor.port == null) ? "" :  " / "+sensor.port}</td>
                                                            <td>{channel.current_metric.latency} ms</td>
                                                            <td>{channel.current_metric.value} {channel.unit}</td>
                                                            <td>{channel.current_metric.message}</td>
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
