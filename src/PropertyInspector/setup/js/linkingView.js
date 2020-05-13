// Load the pairing view
function loadLinkView() {
  // Time used to automatically pair bridges
  // var autoPairingTimeout = 5;

  // // Define local timer
  // var timer = null;

  // Set the status bar
  setStatusBar("linking");

  // Fill the title
  document.getElementById("title").innerHTML = localization["Linking"]["Title"];

  // Fill the content area
  var content =
    "<div class='leftAlign'><p class='leftAlign'>" +
    localization["Linking"]["Description"] +
    "</p><p class='leftAlign'><span class='linkspan' onclick='window.opener.openLIFXLogin()'>" +
    localization["Linking"]["ClickHere"] +
    "</span> " +
    localization["Linking"]["DescriptionPart2"] +
    "</p<<hr/><br/></div> \
        <div id = 'controls' ></div> ";
  document.getElementById("content").innerHTML = content;

  // // Start the pairing
  // autoPairing();

  // // For n seconds try to connect to the bridge automatically
  // function autoPairing() {
  // Show manual user controls instead
  var controls =
    "<div class='inputTitle'>" +
    localization["Linking"]["ApprovalCodeTitle"] +
    ":</div><input type='textarea' class='approvalCode' placeholder='" +
    localization["Linking"]["ApprovalPlaceholder"] +
    "' value='' id='authToken'><br/>\
                               <p class='small leftAlign'>" +
    localization["Linking"]["NotePopup"] +
    "</p><br/><div class='button' id='submit'>" +
    localization["Linking"]["Submit"] +
    "</div> \
                               <div class='button-transparent' id='close'>" +
    localization["Linking"]["Close"] +
    "</div>";
  document.getElementById("controls").innerHTML = controls;

  // Add event listener
  document.getElementById("submit").addEventListener("click", submit);
  document.addEventListener("enterPressed", submit);

  document.getElementById("close").addEventListener("click", close);
  document.addEventListener("escPressed", close);
  // }

  // Retry pairing by reloading the view
  function submit() {
    var authToken = document.getElementById("authToken");
    window.opener.updateAuthToken(authToken.value);
    unloadPairingView();
    loadValidatingView();
    //loadFinishView();
  }

  // Close the window
  function close() {
    window.close();
  }

  // Unload view
  function unloadPairingView() {
    // Stop the timer
    // clearInterval(timer);
    // timer = null;

    // Remove event listener
    document.removeEventListener("escPressed", submit);
    document.removeEventListener("enterPressed", close);
  }
}
