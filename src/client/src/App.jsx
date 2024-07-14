import { useState } from 'react'
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import { AppProvider } from './AppContext';

import Main from './pages/Main';
import Problems from './pages/Problems';
import NotFound from './pages/NotFound';

const router = createBrowserRouter(
	[
		{
			path: "/",
			element: <Main />,
		},
		{
			path: "/Main",
			element: <Main />,
		},
		{
			path: "/Problems",
			element: <Problems />,
		},
		{
			//path: "/Sensors",
			//element: <Sensors />,
		},
		{
			//path: "/Sensor/:id",
			//element: <Sensor />,
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