import React, {createContext, useState} from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import {AppProvider} from './components/AppContext';

import Main from './pages/Main';
import ListErrors from './pages/ListErrors';
import NotFound from './pages/NotFound';
import Sensors from './pages/Sensors.jsx';
import Sensor from './pages/Sensor.jsx';


const router = createBrowserRouter(
    [
        {
            path: "/Main",
            element: <Main />,
        },
        {
            path: "/ListErrors",
            element: <ListErrors />,
        },
        {
            path: "/Sensors",
            element: <Sensors />,
        },
        {
            path: "/Sensor/:id",
            element: <Sensor />,
        },

        {
          path: "*",
          element: <NotFound />,
        },
    ]
)



function App() {

  return (
    <>
        <AppProvider>
            <RouterProvider router={router} />
        </AppProvider>
    </>
  );
}

export default App;
