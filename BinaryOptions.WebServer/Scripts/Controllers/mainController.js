
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
        if ($scope.instruments != undefined) {
            if ($scope.instruments[0] != NaN) {
                if ($scope.instruments[0].rate != NaN) {
                    tick($scope.instruments[0].rate);
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

    $scope.getInstrumentRates = function (instrumentId) {
        $http({
            method: 'GET',
            url: '/Index/GetInstrumentRates',
            params: { instrumentId: instrumentId }
        }).then(function successCallback(response) {
            alertify.success("Got rates successfuly.");
        }, function errorCallback(response) {
            alertify.error("Oh crap, failed to get rates.");
        });
    };

    var limit = 60 * 1,
            duration = 250,
            now = new Date(Date.now() - duration)

    var width = 500,
        height = 200

    var groups = {
        current: {
            value: 0,
            color: 'orange',
            data: d3.range(limit).map(function () {
                return 0
            })
        }
    }

    var x = d3.time.scale()
        .domain([now - (limit - 2), now - duration])
        .range([0, width])

    var y = d3.scale.linear()
        .domain([0, 5])
        .range([height, 0])

    var line = d3.svg.line()
        .interpolate('basis')
        .x(function (d, i) {
            return x(now - (limit - 1 - i) * duration)
        })
        .y(function (d) {
            return y(d)
        })

    var svg = d3.select('svg')
        .attr('class', 'chart')
        .attr('width', width)
        .attr('height', height + 50)

    var axis = svg.append('g')
        .attr('class', 'x axis')
        .attr('transform', 'translate(0,' + height + ')')
        .call(x.axis = d3.svg.axis().scale(x).orient('bottom'))

    var paths = svg.append('g')

    for (var name in groups) {
        var group = groups[name]
        group.path = paths.append('path')
            .data([group.data])
            .attr('class', name + ' group')
            .style('stroke', group.color)
    }

    function tick(rate) {
        now = new Date()

        // Add new values
        for (var name in groups) {
            var group = groups[name]
            //group.data.push(group.value) // Real values arrive at irregular intervals
            group.data.push(rate)
            group.path.attr('d', line)
        }

        // Shift domain
        x.domain([now - (limit - 2) * duration, now - duration])

        // Slide x-axis left
        axis.transition()
            .duration(duration)
            .ease('linear')
            .call(x.axis)

        // Slide paths left
        paths.attr('transform', 'translate(' + x(now - (limit - 1) * duration) + ')')
            .transition()
            .duration(duration)
            .ease('linear')
            .attr('transform', 'translate(' + x(now - (limit - 1) * duration) + ')');
        //.each('end', tick)

        // Remove oldest data point from each group
        for (var name in groups) {
            var group = groups[name]
            group.data.shift()
        }
    }

    function test(){

    };

}]);