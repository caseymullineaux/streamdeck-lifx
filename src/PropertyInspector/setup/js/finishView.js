// Load the save view
function loadFinishView() {
  // Init loadSaveView
  var instance = this;

  // Set the status bar
  //setStatusBar("finish");

  // Fill the title
  document.getElementById("title").innerHTML = localization["Finish"]["Title"];

  // Fill the content area
  var content =
    "<p>" +
    localization["Finish"]["Description"] +
    "</p> \
                   <img class='image' src='images/paired.png'> \
                   <div class='button' id='close'>" +
    localization["Finish"]["Close"] +
    "</div>";
  document.getElementById("content").innerHTML = content;

  // Add event listener
  document.getElementById("close").addEventListener("click", close);
  document.addEventListener("enterPressed", close);

  // // Safe the bridge
  // var detail = {
  //               'detail': {
  //                   'id': bridge.getID(),
  //                   'username': bridge.getUsername()
  //                 }
  //             };
  // var event = new CustomEvent('saveBridge', detail);
  // window.opener.document.dispatchEvent(event);

  // Close this window
  function close() {
    window.close();
  }
}
