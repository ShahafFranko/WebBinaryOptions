
app.controller('mainController', ['$scope', 'hubProxy', '$http', function ($scope, hubProxy, $http) {
    
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

    //$scope.openUpPosition = function (amount, instrumentName) {
    //    hub.invoke('openUpPosition', function () {
    //        alertify.success("Position opened successfuly");
    //    });
    //};

    $scope.openHighPosition = function (username, instrumentName, amount) {
        $scope.openPosition(username, "High", instrumentName, amount);
    }

    $scope.openLowPosition = function (username, instrumentName, amount) {
        $scope.openPosition(username, "Low", instrumentName, amount);
    }

    $scope.openPosition = function (username ,direction, instrumentName, amount) {
        $http({
            method: 'POST',
            url: '/Index/OpenPosition',
            data:
            {
                Username: username,
                Direction: direction,
                InstrumentName: instrumentName,
                Amount: amount,
            }
        }).then(function successCallback(response) {
            alertify.success(direction+" position opened successfuly");
            $scope.accounts.push(response.data);
        }, function errorCallback(response) {
            alertify.error("Oh crap, failed to open a position.");
        });
    }

}]);