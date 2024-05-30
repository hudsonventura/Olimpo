import React, { useState, useEffect } from 'react';

export function GetMainData({setData, setLoading, setError}) {
   

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
            console.log('erro')
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

    
    return (<></>);
}

export default GetMainData;