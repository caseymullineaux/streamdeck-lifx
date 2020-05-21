function updateSelectors() {
    //document.getElementById("dvSelector").style.display = 'none';

    //if (payload['selectorList']) {
    //    document.getElementById("dvSelector").style.display = '';
    //}

    if (validateToken()) {
        console.log("[life.js] Updating selectors ...")
        var payload = {};
        payload.property_inspector = "updateSelectors";
        payload.type = document.querySelector('input[name="rdSelector"]:checked').value;
        console.log("DEBUG: payload.type: " + payload.type);
        sendPayloadToPlugin(payload);
    }

}

function validateToken() {
    console.log("[lifx.js] Checking if the token is valid ...");
    var payload = actionInfo.payload.settings;

    if (!payload['authTokenIsValid']) {
        console.log("[lifx.js] Token is invalid.");
        console.log("[lifx.js] Displaying invalid message.");
        setSettingsWrapper("block"); // show the token invalid message
        return false;
    }

    return true;
}

function updateSelectorRadio() {
    console.log("[lifx.js] Updating the selector radio group ...");
    var payload = actionInfo.payload.settings;
    console.log(payload);

    if (payload['selectorType'] === 'group') {
        document.getElementById("selectorGroup").checked = true;
    } else {
        document.getElementById("selectorSingle").checked = true;
    }

   
}