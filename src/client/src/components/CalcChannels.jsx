import React, { useState, useEffect } from 'react';


export function countChannelsInAllStacks_Error(data){
    return data.reduce((stackTotal, stack) => {
        return stackTotal + countChannelsInStack_Error(stack);
    }, 0);
};


// Funções de contagem de sensor
export const countChannelsInSensor = (sensor) => {
    return 1;
};
export const countChannelsInSensor_Error = (sensor) => {
    return sensor.channels.filter(channel => channel.current_metric?.error_code > 0).length;
};

export const countChannelsInSensor_Success = (sensor) => {
    return sensor.channels.filter(channel => channel.current_metric?.error_code == 0).length;
};


// Funções de contagem de serviço
export const countChannelsInService = (service) => {
    return service.sensors.reduce((sensorTotal, sensor) => {
    return sensorTotal + countChannelsInSensor(sensor);
    }, 1);
};

export const countChannelsInService_Error = (service) => {
    return service.sensors.reduce((sensorTotal, sensor) => {
    return sensorTotal + countChannelsInSensor_Error(sensor);
    }, 0);
};

export const countChannelsInService_Success = (service) => {
    return service.sensors.reduce((sensorTotal, sensor) => {
    return sensorTotal + countChannelsInSensor_Success(sensor);
    }, 0);
};



// Funções de contagem de stack
export const countChannelsInStack = (stack) => {
    return stack.services.reduce((serviceTotal, service) => {
    return serviceTotal + countChannelsInService(service);
    }, 1);
};

export const countChannelsInStack_Error = (stack) => {
    return stack.services.reduce((serviceTotal, service) => {
    return serviceTotal + countChannelsInService_Error(service);
    }, 0);
};

export const countChannelsInStack_Success = (stack) => {
    return stack.services.reduce((serviceTotal, service) => {
    return serviceTotal + countChannelsInService_Success(service);
    }, 0);
};


//funções de contagem para os contadores superiores
export const countChannelsInAllStacks = (data) => {
    return data.reduce((stackTotal, stack) => {
        return stackTotal + countChannelsInStack(stack);
    }, 0);
};

export const countChannelsInAllStacks_Success = (data) => {
    return data.reduce((stackTotal, stack) => {
        return stackTotal + countChannelsInStack_Success(stack);
    }, 0);
};


function CalcChannels(){

}

//export default CalcChannels;