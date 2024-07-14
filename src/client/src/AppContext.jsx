import React, { createContext, useState, useEffect} from 'react';

// Crie o contexto
const AppContext = createContext();

// Crie o provedor do contexto
const AppProvider = ({ children }) => {

  const [settings, setSettings] = useState(null);
  useEffect(() => {
    const fetchData = async () => {
      const data = await fetchAppSettings();
      setSettings(data);
    };

    fetchData();

    
  }, []);



  return (
    <AppContext.Provider value={{ settings }}>
      {children}
    </AppContext.Provider>
  );
};


const fetchAppSettings = async () => {
    try {
      const response = await fetch('/appsettings.json');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      const data = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching the settings:', error);
      return null;
    }
  };


export { AppContext, AppProvider };
