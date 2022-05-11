import React, { useContext, useEffect, useState } from 'react';
import { DIContext } from '../dependencies';
import { Button, CircularProgress, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material'
import { NavLink } from "react-router-dom";
import { Participant } from '../api/participant';

export default function Participants(): JSX.Element {
    const { participantEndpoint } = useContext(DIContext);

    const [participants, setParticipants] = useState<Participant[]>([]);
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            try {
                const result = await participantEndpoint.get();
                setParticipants(result);
            }
            finally {
                setLoaded(true);
            }
        })();
    }, []);

    if (!loaded) {
        return <CircularProgress />
    }

    return (
        <>
            <h1>Participants</h1>
            <p>todo</p>
            <Divider />
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Key</TableCell>
                            <TableCell align="left">Language</TableCell>
                            <TableCell align="left">User groups</TableCell>
                            <TableCell align="left">Active Permissions</TableCell>
                            <TableCell align="left"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {participants.map(p => (
                            <TableRow
                                key={p.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    {p.key}
                                </TableCell>
                                <TableCell align="left">{p.language}</TableCell>
                                <TableCell align="left">{p.participantGroups.map(g => g.key).join(", ").toString()}</TableCell>
                                <TableCell align="left">collect_data, collect_location_data</TableCell>
                                <TableCell align="left"><Button key={p.id}
                                    component={NavLink} to={`${p.id}`}>View</Button></TableCell>
                            </TableRow>
                        ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}
