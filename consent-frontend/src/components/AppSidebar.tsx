import React from "react";
import { Divider, Drawer, List, ListItem, ListItemIcon, ListItemText, Toolbar } from "@mui/material";
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import GavelIcon from '@mui/icons-material/Gavel';
import PeopleIcon from '@mui/icons-material/People';
import CodeIcon from '@mui/icons-material/Code';
import { NavLink } from "react-router-dom";

const drawerWidth = 240;

export default function AppSidebar(): JSX.Element {

    return (
        <Drawer
            sx={{
                width: drawerWidth,
                flexShrink: 0,
                '& .MuiDrawer-paper': {
                    width: drawerWidth,
                    boxSizing: 'border-box',
                },
            }}
            variant="permanent"
            anchor="left"
        >
            <Toolbar />
            <div>Consent : Demo</div>
            <Divider />
            <List>
                <AppNavLink to="/permissions" name="Permissions" icon={<CheckBoxIcon/>}/>
                <AppNavLink to="/contracts" name="Contracts" icon={<GavelIcon/>}/>
                <AppNavLink to="/participants" name="Participants" icon={<PeopleIcon/>}/>
            </List>
            <Divider />
            <List>
                <AppNavLink to="/api" name="Api" icon={<CodeIcon/>}/>
            </List>
        </Drawer>
    )
}

function AppNavLink({ to, name, icon }: { to: string, name: string, icon: JSX.Element }): JSX.Element {
    return (
        <ListItem
            button
            key={name}
            component={NavLink} to={to}
        >
            <ListItemIcon>
                {icon}
            </ListItemIcon>
            <ListItemText primary={name} />
        </ListItem>
    );
}
