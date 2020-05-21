document.addEventListener("websocketCreate", function () {
  console.log("Websocket created!");
  //checkSettings(actionInfo.payload.settings);
    window.setTimeout(updateBrightnessLabel, 500);
    updateSelectorRadio();
    updateSelectors();

  websocket.addEventListener("message", function (event) {
    console.log("[SetColor.js] Got message event!");

    // Received message from Stream Deck
    var jsonObj = JSON.parse(event.data);
    var payload = jsonObj.payload;
      if (jsonObj.event === "sendToPropertyInspector") {
        //updateSelectors();
    } else if (jsonObj.event === "didReceiveSettings") {
          if (!payload.settings.selectorList) {
              updateSelectors();
          }
    }
    //updateSelectorRadio();
    window.setTimeout(updateBrightnessLabel, 500);
  });
});

document.addEventListener("settingsUpdated", function (event) {
  console.log("Got settingsUpdated event!");
    window.setTimeout(updateBrightnessLabel, 500);
    //updateSelectors();
});

function updateBrightnessLabel() {
  var brightnessTitle = document.getElementById("brightnessTitle");
  var brightness = document.getElementById("brightness");

  brightnessTitle.innerText = "Brightness: " + brightness.value + " %";
}
