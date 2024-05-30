import React, { useState, useEffect } from 'react';

import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row'
import Button from 'react-bootstrap/Button';
import Table from 'react-bootstrap/Table';
import Tooltip from 'react-bootstrap/Tooltip';
import OverlayTrigger from 'react-bootstrap/OverlayTrigger';
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




function Main() {

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        // URL da API
        const url = 'https://10.10.1.100:5001/Api';
    
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
