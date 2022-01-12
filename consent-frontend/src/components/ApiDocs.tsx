import React from 'react';

export default function ApiDocs(): JSX.Element {
    return (
        <>
            <div style={{
                width: "80%", margin: "auto",
                display: "flex", flexDirection: "column"
            }}>
                <div style={{ backgroundColor: "#eee", padding: "1em" }}>
                    <h1>Permissions</h1>
                    <br/>
                    <p>What are the permissions for this user?</p>
                    <p>CODE CODE CODE</p>
                    <p>EVENTS EVENTS EVENTS</p>
                    <br/>
                    <p>Does this user have a certain permission?</p>
                    <p>CODE CODE CODE</p>
                    <p>EVENTS EVENTS EVENTS</p>
                </div>
                <br/>
                <div style={{ backgroundColor: "#eee", padding: "1em" }}>
                    <h1>User</h1>
                    <br/>
                    <p>Add a user</p>
                    <p>CODE CODE CODE</p>
                    <p>EVENTS EVENTS EVENTS</p>
                    <br/>
                    <p>Get / modify a users membership in a user group</p>
                    <p>CODE CODE CODE</p>
                    <p>EVENTS EVENTS EVENTS</p>
                </div>
                <br/>
                <div style={{ backgroundColor: "#eee", padding: "1em" }}>
                    <h1>Integrate contract iframe in your application</h1>
                    <br/>
                    <p>Show all relevant contracts to participant</p>
                    <p>CODE CODE CODE</p>
                    <br/>
                    <p>Show all relevant contracts to user, but hide fully accepted</p>
                    <p>CODE CODE CODE</p>
                    <br/>
                    <p>Should only contracts necessary to get a specific permission</p>
                    <p>CODE CODE CODE</p>
                    <br/>
                    <p>Make specific permissions mandatory</p>
                    <p>CODE CODE CODE</p>
                </div>
                <br/>
                <div style={{ backgroundColor: "#eee", padding: "1em" }}>
                    <h1>Have participants see contracts through your frontend and integrate via api</h1>
                    <br/>
                    <p>Get contracts for a user and submit participant response</p>
                    <p>CODE CODE CODE</p>
                    <br/>
                    <br/>
                    <p>Get contracts for specific permission</p>
                    <p>CODE CODE CODE</p>
                </div>
            </div>
        </>
    );
}
