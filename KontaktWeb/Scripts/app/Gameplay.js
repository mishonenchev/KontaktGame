(function () {
    var connection = $.hubConnection();
    var contosoChatHubProxy = connection.createHubProxy('Hub');

    function chatViewModel() {
        var self = this;
        self.users = ko.observableArray();
    }


    var vm = new chatViewModel();
    ko.applyBindings(vm);
    contosoChatHubProxy.on('receiveUsers', function (message) {
        var _users = JSON.parse(message);
        vm.users.removeAll();
        for (var i in _users) {
            vm.users.push(users[i]);
        }
    });
    connection.start().done(function () {

    });
})();