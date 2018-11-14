//(function () {
    var connection = $.hubConnection();
    var contoHubProxy = connection.createHubProxy('MyHub');

    var vm = new Gameplay();

    connection.start().done(function () { vm.startConnection();});
      

    function Gameplay() {
        var self = this;
        self.message = ko.observable();
        self.messages = ko.observableArray();
        self.users = ko.observableArray();
        self.send = function () {
            console.log("click");
            contoHubProxy.invoke('SendMessage', self.message());
            self.message("");
        };
        self.startConnection = function () {
            console.log("Started connection.");
            contoHubProxy.invoke('SendActiveUsers');
        };
    }

    ko.applyBindings(vm);

    contoHubProxy.on('newMessage', function (message) {
        console.log("New message");
        vm.messages.push(message);
    });
    contoHubProxy.on('receiveUsers', function (user) {
        console.log("New player connected!");
        var _users = JSON.parse(user);
        vm.users.removeAll();
        for (var i in _users) {
            vm.users.push(_users[i].name);
        }
    });
//})();