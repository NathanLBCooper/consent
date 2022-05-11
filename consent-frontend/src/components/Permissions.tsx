import React, { useContext, useEffect, useState } from 'react';
import { DIContext } from '../dependencies';
import { Button, CircularProgress, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material'
import { Permission } from "../api/permission";

export default function Permissions(): JSX.Element {
    const { permissionEndpoint } = useContext(DIContext);

    const [permissions, setPermissions] = useState<Permission[]>([]);
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            try {
                const result = await permissionEndpoint.getAll();
                setPermissions(result);
            }
            finally {
                setLoaded(true);
            }
        })();
    }, []);

    function onSaveButtonClicked(e: any) {
        console.warn("onSaveButtonClicked")
    }

    if (!loaded) {
        return <CircularProgress />
    }

    return (
        <>
            <h1>Permissions</h1>
            <p>Some specific idea that can be agreed to
                This is motivated by your usecase.
                When you ask yourself &quot;can I do X with this user&quot;, or &quot;can I do X and/or Y with this user&quot;,
                X and Y are the permissions.
                It may be possible to collect permssion to do X or Y in multiple different ways, but the meaning of X and Y never changes.
                If the meaning does look like it should change, that means a new permission is needed.
            </p>
            <Divider />
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Key</TableCell>
                            <TableCell align="left">Description</TableCell>
                            <TableCell align="left"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {permissions.map((permission) => (
                            <PermissionsRow key={permission.id} permission={permission} />
                        ))}
                        <TableRow>
                            <TableCell><strong>Add</strong></TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );

    function PermissionsRow({ permission }: { permission: Permission }) {
        return (
            <TableRow
                key={permission.id}
                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
                <TableCell component="th" scope="row" style={{ opacity: permission.active ? 1 : 0.4 }}>
                    {permission.key}
                </TableCell>
                <TableCell align="left" style={{ opacity: permission.active ? 1 : 0.4 }}>{permission.description}</TableCell>
                <TableCell align="left">
                    <Button> {permission.active ? "Disable" : "Enable"} </Button>
                </TableCell>
            </TableRow>
        );
    }
}
