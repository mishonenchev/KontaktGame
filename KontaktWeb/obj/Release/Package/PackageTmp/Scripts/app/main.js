var displayActivePlayersClicked = false;
function displayActivePlayers() {
    if (!displayActivePlayersClicked) {
        document.getElementsByClassName("activePlayers")[0].style.transform = "translateX(calc(0% - 10vw))";
        document.getElementById("leftArrow").style.transform = "rotate(-180deg)";
        displayActivePlayersClicked = true;
    }
    else {
        document.getElementsByClassName("activePlayers")[0].style.transform = "translateX(calc(-100% - 10vw))";
        document.getElementById("leftArrow").style.transform = "rotate(0deg)";
        displayActivePlayersClicked = false;
    }
}