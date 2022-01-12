import React, { useContext, useEffect, useState } from 'react';
import { DIContext } from '../dependencies';
import { Button, CircularProgress, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper } from '@mui/material'
import { NavLink } from "react-router-dom";
import { ContractSummary } from "../api/contractSummary";

export default function Contracts(): JSX.Element {
    const { contractEndpoint } = useContext(DIContext);

    const [contractSummaries, setContractSummaries] = useState<ContractSummary[]>([]);
    const [loaded, setLoaded] = useState<boolean>(false);

    useEffect(() => {
        (async () => {
            try {
                const result = await contractEndpoint.getSummaries();
                setContractSummaries(result);
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
            <h1>Contracts - &quot;en&quot;</h1>
            <p>The contracts to be presented to the participants. Completing contracts is the means by which participants consent to Permissions.</p>
            <p>Contracts must independently contain all the language for a participant to make a informed consent for the Permissions contained.
                But it&lsquo;s they should also be small, so that applications can show the small and relevant Contracts to the Participants, rather than all of the Permissions all of the time.
            </p>
            <Divider />
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Key</TableCell>
                            <TableCell align="left">Description</TableCell>
                            <TableCell align="left">Drafts</TableCell>
                            <TableCell align="left">Active</TableCell>
                            <TableCell align="left">Deprecated</TableCell>
                            <TableCell align="left">Deactivated</TableCell>
                            <TableCell align="left"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {contractSummaries.map((summary) => (
                            <TableRow
                                key={summary.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                            >
                                <TableCell component="th" scope="row">
                                    {summary.name}
                                </TableCell>
                                <TableCell align="left">{summary.description}</TableCell>
                                <TableCell align="left">{summary.numberOfDraftVersions}</TableCell>
                                <TableCell align="left">{summary.numberOfActiveVersions}</TableCell>
                                <TableCell align="left">{summary.numberOfDeprecatedVersions}</TableCell>
                                <TableCell align="left">{summary.numberOfDeactivatedVersions}</TableCell>
                                <TableCell align="left">
                                    <Button key={summary.id}
                                        component={NavLink} to={`${summary.id}`}>Edit</Button>
                                </TableCell>
                            </TableRow>
                        ))}
                        <TableRow>
                            <TableCell><strong>Add</strong></TableCell>
                        </TableRow>
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}
