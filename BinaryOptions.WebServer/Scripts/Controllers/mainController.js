
app.controller('mainController', ['$scope', 'hubProxy', '$http', '$cookies', '$interval', function ($scope, hubProxy, $http, $cookies, $interval) {
    
    $scope.instruments = [];
    $scope.account = null;

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

    hub.on('onAccountUpdated', function (account) {
        $scope.account = account;
    });

    $scope.$on('connectionEstablished', function () {
        // success notification
        alertify.success("Connected to server.");

        // get accounts details during load.
        $scope.getAccountDetails();

        //poll for account changes every 10 seconds.
        $interval($scope.getAccountDetails, 5000);

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

    $scope.mapAccountResponse = function (response) {
        $scope.account = {
            id: response.Id,
            username: response.Username,
            balance: response.Balance,
            positions: $scope.mapPositionsResponse(response.Positions)
        };
    };

    $scope.mapPositionsResponse = function (positionsResponse) {
        var positions = [];

        for (var i = 0; i < positionsResponse.length; i++) {
            positions.push({
                openTime: moment(parseInt(positionsResponse[i].OpenTime.substr(6))).format("DD/MM/YYYY HH:mm:ss"),
                instrumentName: positionsResponse[i].InstrumentName,
                direction: positionsResponse[i].Direction,
                amount: positionsResponse[i].Amount,
                openPrice: positionsResponse[i].OpenPrice,
                expireTime: moment(parseInt(positionsResponse[i].ExpireTime.substr(6))).format("DD/MM/YYYY HH:mm:ss"),
                closePrice: positionsResponse[i].ClosePrice
            });
        }

        return positions;
    };

    $scope.openHighPosition = function(username, instrumentName, amount) {
        $scope.openPosition(username, "High", instrumentName, amount);
    };

    $scope.openLowPosition = function(username, instrumentName, amount) {
        $scope.openPosition(username, "Low", instrumentName, amount);
    };

    $scope.openPosition = function(username, direction, instrumentName, amount) {
        $http({
            method: 'POST',
            url: '/Index/OpenPosition',
            data:
            {
                AccountId: $scope.account.id,
                Direction: direction,
                InstrumentName: instrumentName,
                Amount: amount,
            }
        }).then(function successCallback(response) {
            alertify.success(direction + " position opened successfuly");
        }, function errorCallback(response) {
            alertify.error("Oh crap, failed to open a position.");
        });
    };

    $scope.logOut = function () {
        $http({
            method: 'POST',
            url: '/Login/LogOut',
        }).then(function successCallback(response) {
            window.location.href = 'http://localhost:2641/login';
        }, function errorCallback(response) {
            alertify.error("Failed to log out.");
        });
    };

    $scope.getAccountDetails = function () {
        $http({
            method: 'POST',
            url: '/Index/GetAccount',
            data:
            {
                AccountId: $cookies.get('accountId'),
            }
        }).then(function successCallback(response) {
            $scope.mapAccountResponse(response.data);
        }, function errorCallback(response) {
            alertify.error("Failed to get account details.");
            $scope.logOut();
        });
    };
}]);