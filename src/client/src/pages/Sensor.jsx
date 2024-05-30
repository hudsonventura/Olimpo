import React from 'react';
import Navigation from '../components/Navigation';
import { useParams, useNavigate } from 'react-router-dom';

function Sensor() {
    var {id} = useParams();

    const navigate = useNavigate();
    const PostForm = () => {
        console.log("Form enviado");
        return navigate("/Main");
    }

    return (
        <>
        <Navigation />
        <h1>Sensor : {id}</h1>
        <button onClick={PostForm}>Post form</button>
        </>
    );
}

export default Sensor;
