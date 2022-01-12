import React from 'react';
import { AppBar, Box } from '@mui/material';
import { Outlet, Route, Router, Routes } from 'react-router';
import { BrowserRouter } from 'react-router-dom';
import './App.css';
import AppSidebar from './components/AppSidebar';
import Permissions from './components/Permissions';
import Contracts from './components/Contracts';
import Contract from './components/Contract';
import Participants from './components/Participants';
import Participant from './components/Participant';
import ApiDocs from './components/ApiDocs';

const drawerWidth = 240;

export default function App(): JSX.Element {
  return (
    <div className="App">
      <BrowserRouter>
        <Box sx={{ display: 'flex' }}>
          <AppBar
            position="fixed"
            sx={{ width: `calc(100% - ${drawerWidth}px)`, ml: `${drawerWidth}px` }}
          />
          <AppSidebar />
          <Box
            component="main"
            sx={{ flexGrow: 1, bgcolor: 'background.default', p: 3 }}
          >
            <Routes>
              <Route path="/permissions" element={<Permissions />} />
              <Route path="/contracts" element={<Contracts />} />
              <Route path="/contracts/:id" element={<Contract />} />
              <Route path="/participants" element={<Participants />} />
              <Route path="/participants/:id" element={<Participant />} />
              <Route path="/api" element={<ApiDocs />} />
            </Routes>
          </Box>
        </Box>
      </BrowserRouter>
    </div>
  );
}

