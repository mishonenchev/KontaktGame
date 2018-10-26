var connection = $.hubConnection();
var contosoChatHubProxy = connection.createHubProxy('Hub');
function ViewModel() {
    var self = this;
    self.message = ko.observable();
    self.send = function () {
        console.log("hey");
        contosoChatHubProxy.invoke('SendMessage', self.message());
        self.message("");
    };
    self.messages = ko.observableArray();


}
var vm = new ViewModel();
contosoChatHubProxy.on('newMessage', function (message) {
    vm.messages.push(message);
});
connection.start().done(function () {
    ko.applyBindings(vm);
});