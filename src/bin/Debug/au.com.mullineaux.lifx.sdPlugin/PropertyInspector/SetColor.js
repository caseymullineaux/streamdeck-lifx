document.addEventListener("websocketCreate", function () {
  console.log("Websocket created!");
  //checkSettings(actionInfo.payload.settings);
    window.setTimeout(updateBrightnessLabel, 500);
    updateSelectors();
    

  websocket.addEventListener("message", function (event) {
    console.log("Got message event!");

    // Received message from Stream Deck
    var jsonObj = JSON.parse(event.data);

    if (jsonObj.event === "sendToPropertyInspector") {
        var payload = jsonObj.payload;
        updateSelectors();
    } else if (jsonObj.event === "didReceiveSettings") {
        var payload = jsonObj.payload;
        //updateSelectors();
    }
    window.setTimeout(updateBrightnessLabel, 500);
  });
});

document.addEventListener("settingsUpdated", function (event) {
  console.log("Got settingsUpdated event!");
    window.setTimeout(updateBrightnessLabel, 500);
    updateSelectors();
});

function checkSettings(payload) {
  console.log("Checking Settings");
}

function updateBrightnessLabel() {
  var brightnessTitle = document.getElementById("brightnessTitle");
  var brightness = document.getElementById("brightness");

  brightnessTitle.innerText = "Brightness: " + brightness.value + " %";
}

function resetCounter() {
  var payload = {};
  payload.property_inspector = "resetCounter";
  sendPayloadToPlugin(payload);
}

function updateSelectors() {
    if (validateToken()) {
        console.log("updateSelectors called")
        var payload = {};
        payload.property_inspector = "updateSelectors";
        payload.type = document.querySelector('input[name="rdSelector"]:checked').value;
        sendPayloadToPlugin(payload);
    }
    
}
