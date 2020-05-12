var authWindow = null;

document.addEventListener("websocketCreate", function () {
  console.log("Websocket created!");
  checkAuthToken(actionInfo.payload.settings);
  window.setTimeout(updatePeakLabel, 500);

  websocket.addEventListener("message", function (event) {
    console.log("Got message event!");

    // Received message from Stream Deck
    var jsonObj = JSON.parse(event.data);

    if (jsonObj.event === "sendToPropertyInspector") {
      var payload = jsonObj.payload;
      checkAuthToken(payload);
    } else if (jsonObj.event === "didReceiveSettings") {
      var payload = jsonObj.payload;
      checkAuthToken(payload.settings);
    }
    window.setTimeout(updatePeakLabel, 500);
  });
});

document.addEventListener("settingsUpdated", function (event) {
  console.log("Got settingsUpdated event!");
  window.setTimeout(updatePeakLabel, 500);
});

function checkAuthToken(payload) {
  console.log("Checking auth token ...");
  var authToken = document.getElementById("authToken");
  authToken.value = payload["authToken"];

  // check if the auth token has a value
  if (payload["authToken"]) {
    setSettingsWrapper("");
    var event = new Event("authToken");
    document.dispatchEvent(event);

    if (authWindow) {
      authWindow.loadSuccessView();
    }
  } else {
    setSettingsWrapper("none");

    if (authWindow) {
      authWindow.loadFailedView();
    } else {
      authWindow = window.open("setup/setup.html");
    }
  }
}

function setSettingsWrapper(displayValue) {
  var sdWrapper = document.getElementById("sdWrapper");
  // sdWrapper.style.display = displayValue;
}

function resetPlugin() {
  var payload = {};
  payload.property_inspector = "resetPlugin";
  sendPayloadToPlugin(payload);
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
  console.log("Received from form: " + authToken);

  var payload = {};
  payload.property_inspector = "updateApproval";
  payload.authToken = authToken;
  console.log(payload);
  //console.log("PAT:" + payload.authToken);
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
