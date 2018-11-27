var displayActivePlayersClicked = false;
function displayActivePlayers() {
    if (!displayActivePlayersClicked) {
        document.getElementsByClassName("activePlayers")[0].style.transform = "translateX(calc(0% - 10vw))";
        document.getElementById("leftArrow").style.transform = "rotate(-180deg)";
        displayActivePlayersClicked = true;
        if (displayUsedWords) {
            setTimeout(function () {
                document.getElementsByClassName("used-words-container")[0].style.transform = "translateX(calc(100% + 10vw))";
                document.getElementById("rightArrow").style.transform = "rotate(0deg)";
                displayUsedWords = false;
            }, 70)
        }
    }
    else {
        document.getElementsByClassName("activePlayers")[0].style.transform = "translateX(calc(-100% - 10vw))";
        document.getElementById("leftArrow").style.transform = "rotate(0deg)";
        displayActivePlayersClicked = false;
    }
}
var displayUsedWords = false;
function displayUsedWord() {
    if (!displayUsedWords) {
        document.getElementsByClassName("used-words-container")[0].style.transform = "translateX(calc(0% + 10vw))";
        document.getElementById("rightArrow").style.transform = "rotate(-180deg)";
        displayUsedWords = true;
        if (displayActivePlayersClicked) {
            setTimeout(function () {
                document.getElementsByClassName("activePlayers")[0].style.transform = "translateX(calc(-100% - 10vw))";
                document.getElementById("leftArrow").style.transform = "rotate(0deg)";
                displayActivePlayersClicked = false;
            }, 70)
        }
    }
    else {
        document.getElementsByClassName("used-words-container")[0].style.transform = "translateX(calc(100% + 10vw))";
        document.getElementById("rightArrow").style.transform = "rotate(0deg)";
        displayUsedWords = false;
    }
}