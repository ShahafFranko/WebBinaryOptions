
app.controller('mainController', ['$scope', 'hubProxy', function ($scope, hubProxy) {
    
    $scope.instruments = [];

    var hub = hubProxy('http://localhost:2641/', 'tradingHub');

    hub.on('onInstrumentUpdated', function(instrument) {
        for (var i = 0; i < $scope.instruments.length; i++) {
            if ($scope.instruments) {
                if ($scope.instruments[i].name == instrument.Name) {
                    if ($scope.instruments[i].rate < instrument.Rate) {
                        $scope.instruments[i].rate = instrument.Rate;
                        $scope.instruments[i].trend = 'up';
                        
                    }
                    else {
                        $scope.instruments[i].rate = instrument.Rate;
                        $scope.instruments[i].trend = 'down';
                    }
                }
            }
        }
    });
    $scope.$on('connectionEstablished', function () {
        // success notification
        alertify.success("Connected to server.");

        hub.invoke('getInstruments', function (instruments) {
            $scope.mapInstrumentResponse(instruments);
        });
    });

    $scope.mapInstrumentResponse = function (instrumentNames) {
        for (var i = 0; i < instrumentNames.length; i++) {
            $scope.instruments.push({
                name: instrumentNames[i],
                rate: 0,
                trend: ''
            });
        }
    };
}]);