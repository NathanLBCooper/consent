import React, { useContext, useEffect, useState } from 'react';
import { DIContext } from '../dependencies';
import { Button, CircularProgress, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material'
import { Permission } from "../api/permission";


export default function Permissions(): JSX.Element {
    const { permissionEndpoint } = useContext(DIContext);

    const [permissions, setPermissions] = useState<Permission[]>([]);
    const [loaded, setLoaded] = useState<boolean>(false);

    const [underEdit, setUnderEdit] = useState<number>(-1);

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

    function onEditButtonClicked(id: number) {
        setUnderEdit(id);
    }

    function onCancelButtonClicked(id: number) {
        setUnderEdit(-1);
    }

    function onSaveButtonClicked(e: any) {
        console.warn("hello")
        console.error(e);
        // const permission = permissions.filter(p => p.id === id);
        // console.log(permission)
    }

    if (!loaded) {
        return <CircularProgress />
    }

    return (
        <>
            <h1>Permissions</h1>
            <p>Some specific idea that can be agreed to.
                When writing an application and you ask the question &quot;Can I do X&quot; with this user, &quot;X&quot; is a permission.</p>
            <p>Being able to do &quot;X&quot; always means the same thing regardless of in what Contract that Permission was obtained.
                If the meaning changes, create a new permission.
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
                            <PermissionsRow key={permission.id} permission={permission} thisRowEditing={permission.id == underEdit} someRowEditing={underEdit > 0} />
                        ))}
                        <TableRow>
                            <TableCell><strong>Add</strong></TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );

    function PermissionsRow({ permission, onUpdate }: { permission: Permission, onUpdate: () => void }) {
        return (
            <TableRow
                key={permission.id}
                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
                <TableCell component="th" scope="row">
                    {permission.key}
                </TableCell>
                <TableCell align="left">{permission.description}</TableCell>
                <TableCell align="left">
                    <Button onClick={() => onEditButtonClicked(permission.id)} disabled={someRowEditing}>Edit</Button> <Button disabled={someRowEditing}>Disable</Button>
                </TableCell>
            </TableRow>
        );
    }

    function EditablePermissionsRow({ permission }: { permission: Permission }) {
        const [value, setValue] = React.useState(permission)

        return (
            <TableRow
                key={value.id}
                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
            >
                <TableCell component="th" scope="row">
                    {value.key}
                </TableCell>
                <TableCell align="left" contentEditable={true} onChange={e => setValue(e.target.value)}>{value.description}</TableCell>
                <TableCell align="left">
                    <Button onClick={onSaveButtonClicked}>Save</Button> <Button onClick={() => onCancelButtonClicked(permission.id)}>Cancel</Button>
                </TableCell>
            </TableRow>
        );
    }
}
