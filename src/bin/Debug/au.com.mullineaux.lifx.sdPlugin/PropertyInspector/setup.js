var authWindow = null;

document.addEventListener("websocketCreate", function () {
  console.log("Websocket created!");
    validateSetupProcess(actionInfo.payload.settings);
    //updateAuthToken(actionInfo.payload.settings["authToken"])
    


  websocket.addEventListener("message", function (event) {
    console.log("Got message event!");

    // Received message from Stream Deck
    var jsonObj = JSON.parse(event.data);

    if (jsonObj.event === "sendToPropertyInspector") {
      var payload = jsonObj.payload;
      //showSetupDialog(payload.settings);
    } else if (jsonObj.event === "didReceiveSettings") {
      var payload = jsonObj.payload;
      validateSetupProcess(payload.settings);
    }
  });
});

//document.addEventListener("settingsUpdated", function (event) {
//});

function validateSetupProcess(payload) {

    // Check if a token has been set.
    // If it is blank or null, run the setup process.

  console.log("[setup.js] Checking if an auth token has been set ...");
   console.log(payload);
  //var authToken = document.getElementById("authToken");
  //authToken.value = payload["authToken"];


  //if (payload["authTokenIsValid"]) {
    if (payload["authToken"]) {
        console.log("[setup.js] authToken has a value.");
        console.log("[setup.js] " + payload["authToken"]);
        // var event = new Event("authTokenIsValid");
        // document.dispatchEvent(event);

        if (payload["authTokenIsValid"]) {
            console.log("[setup.js] authTokenIsValue = True");
            console.log("[setup.js] " + payload["authTokenIsValid"]);
            setSettingsWrapper("none");

            if (authWindow) {
                authWindow.loadSuccessView();
                setSettingsWrapper("none");
                updateSelectors();
            }

        } else {

            // if authWindow variable is not set
            // display the "invalid token" message in the PI
            console.log("[setup.js] authToken has a value but it is invalid.");
            console.log("[setup.js] authTokenIsValue = False");
            console.log("[setup.js] " + payload["authTokenIsValid"]);
            setSettingsWrapper("block");

            if (authWindow) {
                authWindow.loadFailedView();
            }
            //else {
            //    if (authToken.value == null) {
            //        authWindow = window.open("setup/setup.html");
            //    }
            //}
        }
    } else {
        if (authWindow) {
            // if the setup is in progress and
            // a blank value for the token was submitted
            console.log("[setup.js] setup is in progress, but a blank value for the token was submitted");

            authWindow.loadFailedView();
        } else {
            console.log("[setup.js] authToken does NOT have a value");
            console.log("[setup.js] " + payload["authToken"]);
            authWindow = window.open("setup/setup.html");
        }
    }
}

function showSetupDialog() {
    authWindow = window.open("setup/setup.html");
}

function setSettingsWrapper(displayValue) {
  var sdWrapper = document.getElementById("sdWrapper");
  sdWrapper.style.display = displayValue;
}

function resetPlugin() {
  var payload = {};
    payload.property_inspector = "resetPlugin";
    setSettingsWrapper("block");
  sendPayloadToPlugin(payload);
  authWindow = window.open("setup/setup.html");
}

function openLIFXLogin() {
  if (websocket && websocket.readyState === 1) {
    const json = {
      event: "openUrl",
      payload: { url: "https://cloud.lifx.com/settings" },
    };
    websocket.send(JSON.stringify(json));
  }
}

function updateAuthToken(val) {
  var authToken = val;

  var payload = {};
  payload.property_inspector = "validateToken";
  payload.authToken = authToken;
  sendPayloadToPlugin(payload);
  console.log("Updating access token");
}

function sendPayloadToPlugin(payload) {
  if (websocket && websocket.readyState === 1) {
    const json = {
      action: actionInfo["action"],
      event: "sendToPlugin",
      context: uuid,
      payload: payload,
    };

    websocket.send(JSON.stringify(json));
  }
}

