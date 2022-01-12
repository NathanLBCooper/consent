import React from 'react';
import { Box, Divider, Button, Chip, TextField, FormGroup, FormControlLabel, Checkbox, Tabs, Tab } from '@mui/material'

export default function Contracts(): JSX.Element {
    return (
        <>
            <h1>User Contract - &quot;en&quot;</h1>
            <Divider />
            <p>User groups: <Chip onDelete={doNothing} label="all-users (en)" /></p>
            <Divider />
            <p><Chip label="Version 4" /> has not been published and can still be edited</p>
            <p><Chip label="Version 3" /> is active and being shown to new users. <Chip label="Version 2" /> is still active, but new responses are not being collected</p>
            <p><Chip label="Version 1" /> has been deprecated and users are being asked to update to <Chip label="Version 3" /> </p>
            <p><Chip label="" /> has been deactivated and consents given are no longer valid</p>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <Tabs aria-label="basic tabs example">
                    <Tab label="Version 1" />
                    <Tab label="Version 2" />
                    <Tab label="Version 3" />
                    <Tab style={{ backgroundColor: "whitesmoke" }} label="Version 4" />
                    <Tab label="ADD" />
                </Tabs>
            </Box>
            <Divider />
            <div style={{
                width: "70%", backgroundColor: "whitesmoke", margin: "auto",
                display: "flex", flexDirection: "column", alignItems: "center",
            }}>
                <TextField
                    id="outlined-multiline"
                    label="Multiline"
                    multiline
                    rows={4}
                    defaultValue="This is a piece of descriptive text"
                />
                <FormGroup>
                    <p>
                        <FormControlLabel disabled control={<Checkbox />} label="I agree to accept these terms and conditions" />
                        <Chip onDelete={doNothing} label="Grants: terms_and_conditions_v2" />
                        <Chip onDelete={doNothing} label="Grants: terms_and_conditions" />
                    </p>
                </FormGroup>
                <TextField
                    id="outlined-multiline"
                    label="Multiline"
                    multiline
                    rows={4}
                    defaultValue="This is a piece of descriptive text"
                />
                <FormGroup>
                    <p><FormControlLabel disabled control={<Checkbox />} label="I agree to this" /> <Chip onDelete={doNothing} label="Grants: share_data_initech" /></p>
                </FormGroup>
                <Button variant="contained">Add part</Button>
            </div>
            <br />
            <Button variant="contained">Lock wording and publish</Button>
        </>
    );
}

function doNothing() {
    // nothing
}
