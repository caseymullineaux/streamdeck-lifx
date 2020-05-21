function validateToken() {
    console.log("[lifx.js] Checking if the token is valid ...");
    payload = actionInfo.payload.settings;
    console.log(payload);


    if (!payload['authTokenIsValid']) {
        console.log("[lifx.js] Token is invalid.");
        console.log("[lifx.js] Displaying invalid message.");
        setSettingsWrapper("block"); // show the token invalid message
        return false;
    }

    return true;
}

function tokenInvalid() {
    // TODO:
    // - set the alert on the PI
    // - set the alert icon on the buttons
}