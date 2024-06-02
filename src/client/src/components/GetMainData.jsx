import React, { useContext, useEffect } from 'react';

import { AppContext } from '../components/AppContext';

export function GetMainData({ setData, setLoading, setError }) {

    const { settings } = useContext(AppContext);

    useEffect(() => {
        if (!settings) {
            return;
        }

        // Função para buscar dados
        const fetchData = async () => {
            try {
                const response = await fetch(`${settings.backend_url}/Api`);

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
    }, [settings]);


    return (<></>);
}

export default GetMainData;