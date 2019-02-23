//(function () {
    var connection = $.hubConnection();
    var contoHubProxy = connection.createHubProxy('MyHub');

    var vm = new Gameplay();

    connection.start().done(function () { vm.startConnection();});

    function Gameplay() {
        var self = this;
        self.word = ko.observable("");
        self.words = ko.observableArray();
        self.users = ko.observableArray();
        self.usedWords = ko.observableArray();
        self.button1 = ko.observable("КОНТАКТ");
        self.button2 = ko.observable("ПОЗНАЙ");
        self.question = ko.observable("");
        self.answer = ko.observable("");
        self.inputWordToGuess = ko.observable("");
        self.sendWordToGuess = function () {
            contoHubProxy.invoke('PromptForWord', self.inputWordToGuess());
            self.inputWordToGuess("");
        };
        self.button1Action = function () {
            contoHubProxy.invoke('FirstButtonAction', self.word());
            self.word("");
        };
        self.button2Action = function () {
            contoHubProxy.invoke('SecondButtonAction', self.word());
            self.word("");
        };
        self.startConnection = function () {
            console.log("Started connection.");
            contoHubProxy.invoke('SendActiveUsers');
            contoHubProxy.invoke('StartGame');
        };
    }

    ko.applyBindings(vm);

contoHubProxy.on('newMessage', function (message) {
    console.log("New message");
    vm.words.push(message);
});
contoHubProxy.on('receiveUsers', function (user) {
    console.log("New player connected!");
    var _users = JSON.parse(user);
    vm.users.removeAll();
    for (var i in _users) {
        vm.users.push(_users[i].name);
    }
});
contoHubProxy.on('buttonState', function (firstButton, secondButton) {
    vm.button1(firstButton);
    vm.button2(secondButton);
});
contoHubProxy.on('displayQuestion', function (question) {
    vm.question(question);
});
contoHubProxy.on('displayAnswer', function (answer) {
    vm.answer(answer);
});
contoHubProxy.on('displayUsedWord', function (usedWord) {
    vm.usedWords.push(usedWord);
});
contoHubProxy.on('promptForWordToGuess', function () {
    $(".modal").modal("show");
});

//})();