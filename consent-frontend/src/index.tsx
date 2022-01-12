import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { AppSettings } from './appSettings';
import { ConfigureDependencies, DIContext } from './dependencies';
import { configureBackend } from './backend/backend';

async function getAppSettings(): Promise<AppSettings> {
  const resp = await fetch('/appsettings.json')
  return await resp.json();
}

(async () => {
  const appSettings: AppSettings = await getAppSettings();
configureBackend();

  ReactDOM.render(
    <React.StrictMode>
      <DIContext.Provider value={ConfigureDependencies(appSettings)}>
        <App />
      </DIContext.Provider>
    </React.StrictMode>,
    document.getElementById('root')
  );
})();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
