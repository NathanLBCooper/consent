import React from 'react';
import { Box, Divider, Button, Chip, TextField, FormGroup, FormControlLabel, Checkbox, Tabs, Tab } from '@mui/material'

export default function Contracts(): JSX.Element {
    return (
        <>
            <h1>Data protection contract &#40;en&#41;</h1>
            <Divider />
            <p>User groups: <Chip onDelete={doNothing} label="all-users" /> <Chip label="+" /></p>
            <Divider />
            <p>Draft Versions &#40;Can still be edited&#41; <Chip label="Version 6" /></p>
            <p>Active &#40;Being shown to new users&#41; <Chip label="Version 5" /></p>
            <p>Legacy &#40;Still valid, but not shown to new users&#41; <Chip label="Version 4" /><Chip label="Version 3" /></p>
            <p>Deprecated &#40;Still valid, but users are being prompted to update to a newer Contract&#41; <Chip label="Version 2" /></p>
            <p>Removed  &#40;Is no longer valid and no longer grants permissions&#41; <Chip label="Version 1" /> </p>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Tabs aria-label="basic tabs example">
                    <Tab label="Version 1" />
                    <Tab label="Version 2" />
                    <Tab label="Version 3" />
                    <Tab label="Version 4" />
                    <Tab label="Version 5" />
                    <Tab style={{ backgroundColor: "whitesmoke" }} label="Version 6" />
                    <Tab label="+" />
                </Tabs>
            </Box>
            <Divider />
            <div style={{
                width: "70%", backgroundColor: "whitesmoke", margin: "auto",
                display: "flex", flexDirection: "column", alignItems: "center",
            }}>
                <br/>
                <TextField
                    id="outlined-multiline"
                    label="Multiline"
                    multiline
                    rows={4}
                    defaultValue="This is a piece of descriptive text"
                />
                <FormGroup>
                    <p>
                        <FormControlLabel disabled control={<Checkbox />} label="I agree for my data and location data to be collected" />
                        <br/>
                        <Chip onDelete={doNothing} label="collect_data" />
                        <Chip onDelete={doNothing} label="collect_location_data" />
                    </p>
                </FormGroup>
                <br/>
                <TextField
                    id="outlined-multiline"
                    label="Multiline"
                    multiline
                    rows={4}
                    defaultValue="This is a piece of descriptive text"
                />
                <FormGroup>
                    <p><FormControlLabel disabled control={<Checkbox />} label="I agree to share data with initrode" />
                    <Chip onDelete={doNothing} label="share_data_initrode" /></p>
                </FormGroup>
                <FormGroup>
                    <p><FormControlLabel disabled control={<Checkbox />} label="I agree to share data with initech" />
                    <Chip onDelete={doNothing} label="share_data_initech" /></p>
                </FormGroup>
                <Button variant="contained">Add part</Button>
            </div>
            <br />
            <p>Publishing will set this Contract to Active and further editing will not be allowed: <Button variant="contained">Publish</Button></p>
        </>
    );
}

function doNothing() {
    // nothing
}
