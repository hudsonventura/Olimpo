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
    
        fetchData();
      }, []); // O array vazio [] significa que este efeito roda apenas uma vez após o componente montar
    
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
    }, 0);
  };
  
  const countChannelsInStack = (stack) => {
    return stack.services.reduce((serviceTotal, service) => {
      return serviceTotal + countChannelsInService(service);
    }, 0);
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
                <th>Latency</th>
                <th>Value</th>
                <th>Message</th>
                </tr>
            </thead>
            <tbody>
                {data.map((stack, index) => (
                    <>
                        <tr>
                            <td rowspan={countChannelsInStack(stack)*2}>Stack: {stack.name} - {countChannelsInStack(stack)}</td>
                        </tr>
                        {
                            stack.services.map(service => (
                                <>
                                    <tr>
                                        <td rowspan={countChannelsInService(service)}>Service: {service.name} - {countChannelsInService(service)}</td>
                                    </tr>
                                    {
                                        service.sensors.map(sensor => (
                                            <>
                                                <tr>
                                                    <td rowspan={countChannelsInSensor(sensor)+1}>Sensor: {sensor.name} - {countChannelsInSensor(sensor)}</td>
                                                </tr>
                                                
                                                {
                                                    sensor.channels.map(channel => (
                                                        <>
                                                            <tr>
                                                            <td>Channel: {channel.name}</td>
                                                            <td>{channel.current_metric.latency} ms</td>
                                                            <td>{channel.current_metric.value} {channel.unit}</td>
                                                            <td>Message</td>
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
