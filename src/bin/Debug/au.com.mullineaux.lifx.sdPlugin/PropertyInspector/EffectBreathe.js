document.addEventListener('websocketCreate', function () {
    console.log("Websocket created!");
    checkSettings(actionInfo.payload.settings);
    window.setTimeout(updateBrightnessLabel, 500);

    websocket.addEventListener('message', function (event) {
        console.log("Got message event!");

        // Received message from Stream Deck
        var jsonObj = JSON.parse(event.data);

        if (jsonObj.event === 'sendToPropertyInspector') {
            var payload = jsonObj.payload;
            checkSettings(payload);
        }
        else if (jsonObj.event === 'didReceiveSettings') {
            var payload = jsonObj.payload;
            checkSettings(payload.settings);
        }
        window.setTimeout(updateBrightnessLabel, 500);
    });
});

document.addEventListener('settingsUpdated', function (event) {
    console.log("Got settingsUpdated event!");
    window.setTimeout(updateBrightnessLabel, 500);
})

function checkSettings(payload) {
    console.log("Checking Settings");
}

function updateBrightnessLabel() {
    var peakTitle = document.getElementById('peakTitle');
    var peak = document.getElementById('peak');

    peakTitle.innerText = "Peak: " + peak.value;
}


function resetCounter() {
    var payload = {};
    payload.property_inspector = 'resetCounter';
    sendPayloadToPlugin(payload);
}