// Load the intro view
function loadIntroView() {
  // Set the status bar
  setStatusBar("intro");

  // Fill the title
  document.getElementById("title").innerHTML = localization["Intro"]["Title"];

  // Fill the content area
  var content =
    "<p>" +
    localization["Intro"]["Description"] +
    "</p> \
                   <img class='image' src='images/paired.png'> \
                   <div class='button' id='start'>" +
    localization["Intro"]["Start"] +
    "</div> \
                   <div class='button-transparent' id='close'>" +
    localization["Intro"]["Close"] +
    "</div>";
  document.getElementById("content").innerHTML = content;

  // Add event listener
  document.getElementById("start").addEventListener("click", startLinking);
  document.addEventListener("enterPressed", startLinking);

  document.getElementById("close").addEventListener("click", close);
  document.addEventListener("escPressed", close);

  // Load the pairing view
  function startLinking() {
    unloadIntroView();
    loadLinkView();
  }

  // Close the window
  function close() {
    window.close();
  }

  // Unload view
  function unloadIntroView() {
    // Remove event listener
    document.removeEventListener("enterPressed", startLinking);
    document.removeEventListener("escPressed", close);
  }
}
