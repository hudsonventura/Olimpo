import React, {createContext, useState} from 'react';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';


import Main from './pages/Main';
import ListErrors from './pages/ListErrors';
import NotFound from './pages/NotFound';
import Sensors from './pages/Sensors.jsx';
import Sensor from './pages/Sensor.jsx';

import Navigation from './components/Navigation';


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
        <Navigation />
        <RouterProvider router={router} />
    </>
  );
}

export default App;
