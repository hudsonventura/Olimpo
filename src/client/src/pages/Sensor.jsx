import React from 'react';
import { useParams, useNavigate } from 'react-router-dom';

import Navigation from '../components/Navigation';

function Sensor() {
    var {id} = useParams();

    const navigate = useNavigate();
    const PostForm = () => {
        console.log("Form enviado");
        return navigate("/Main");
    }

    return (
        <>
        <Navigation data={null} />
        <h1>Sensor : {id}</h1>
        <button onClick={PostForm}>Post form</button>
        </>
    );
}

export default Sensor;
