import React, { useState } from "react";
import { Collapse, Divider, Drawer, List, ListItem, ListItemIcon, ListItemText, Toolbar } from "@mui/material";
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import GavelIcon from '@mui/icons-material/Gavel';
import PeopleIcon from '@mui/icons-material/People';
import CodeIcon from '@mui/icons-material/Code';
import LanguageIcon from '@mui/icons-material/Language';
import QueryStatsIcon from '@mui/icons-material/QueryStats';
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
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
                <AppNavLink to="/permissions" name="Permissions" icon={<CheckBoxIcon />} />
                <ContractNavLink />
                <AppNavLink to="/participants" name="Participants" icon={<PeopleIcon />} />
            </List>
            <AppNavLink to="#" name="Statistics" icon={<QueryStatsIcon />} />
            <Divider />
            <List>
                <AppNavLink to="/api" name="Api" icon={<CodeIcon />} />
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

function ContractNavLink(): JSX.Element {
    const [expanded, setExpanded] = useState<boolean>(false);
    function handleClick() { setExpanded(!expanded) }
    return (
        <div key={"Contracts"}>
            <ListItem
                button
                key={"Contracts"}
                onClick={handleClick}
            >
                <ListItemIcon>
                    <GavelIcon />
                </ListItemIcon>
                <ListItemText primary={"Contracts"} />
                {expanded ? <ExpandLess /> : <ExpandMore />}
            </ListItem>
            <Collapse in={expanded}>
                <List>
                    <ListItem key={"en"}><AppNavLink to="/contracts/en" name="en" icon={<LanguageIcon />} /></ListItem>
                    <ListItem key={"sv-se"}><AppNavLink to="/contracts/sv-se" name="sv-SE" icon={<LanguageIcon />} /></ListItem>
                </List>
            </Collapse>
        </div>
    );
}