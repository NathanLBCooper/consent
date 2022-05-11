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
            <h1>Contracts &#40;en&#41;</h1>
            <p>Text containing informed consent for the Permissions, which is presented to participants in the selected user groups.
            </p>
            <Divider />
            <TableContainer component={Paper}>
                <Table sx={{ minWidth: 650 }} aria-label="simple table">
                    <TableHead>
                        <TableRow>
                            <TableCell>Key</TableCell>
                            <TableCell align="left">Drafts</TableCell>
                            <TableCell align="left">Active</TableCell>
                            <TableCell align="left">Legacy</TableCell>
                            <TableCell align="left">Deprecated</TableCell>
                            <TableCell align="left">Removed</TableCell>
                            <TableCell align="left">User Groups</TableCell>
                            <TableCell align="left"></TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {contractSummaries.map((summary) => (
                            <TableRow
                                key={summary.id}
                                sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                                style={{ opacity: IsOld(summary) ? 0.4 : 1 }}
                            >
                                <TableCell component="th" scope="row">
                                    {summary.name}
                                </TableCell>
                                <TableCell align="left">{summary.stateCounts.draft}</TableCell>
                                <TableCell align="left">{summary.stateCounts.active}</TableCell>
                                <TableCell align="left">{summary.stateCounts.legacy}</TableCell>
                                <TableCell align="left">{summary.stateCounts.deprecated}</TableCell>
                                <TableCell align="left">{summary.stateCounts.removed}</TableCell>
                                <TableCell align="left">{summary.userGroups.join()}</TableCell>
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

function IsOld(summary: ContractSummary) {
    const counts = summary.stateCounts;
    return counts.draft + counts.active + counts.legacy + counts.deprecated == 0;
}