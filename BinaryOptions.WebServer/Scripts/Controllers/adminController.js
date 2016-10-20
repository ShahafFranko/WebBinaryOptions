
app.controller('adminController', ['$scope', 'hubProxy', '$http', function ($scope, hubProxy, $http) {

    $scope.accounts = [];
    $scope.positions = [];

    //account creation
    $scope.username;
    $scope.password;
    $scope.balance;

    // deposit
    $scope.depositAccount;
    $scope.depositBalance;

    // search
    $scope.searchParams =
    {
        descending: true,
        wins: false
    };

    $('#dates-container input').datepicker({
        orientation: "bottom auto",
        autoclose: true
    });

    var hub = hubProxy('http://localhost:2641/', 'tradingHub');

    $scope.$on('connectionEstablished', function () {
        // success notification
        alertify.success("Admin Connected to server.");
        $scope.getAccounts();
        $scope.getWinLoseData();
    });

    $scope.createAccount = function () {
        $http({
            method: 'POST',
            url: '/admin/create',
            data:
            {
                username: $scope.username,
                password: $scope.password,
                balance: $scope.balance,
            }
        }).then(function successCallback(response) {
            alertify.success(response.data.Username + " created successfuly");
            $scope.accounts.push(response.data);
        }, function errorCallback(response) {
            alertify.error("Oh crap, failed to create an account.");
        });
    };

    $scope.getAccounts = function () {
        $http({
            method: 'GET',
            url: '/admin/accounts',
        }).then(function successCallback(response) {
            $scope.accounts = response.data;
        }, function errorCallback(response) {
            alertify.error("Oh crap, failed to fetch accounts.");
        });
    };

    $scope.search = function () {
        $scope.positions = [];

        $http({
            method: 'GET',
            url: '/admin/Search',
            params: {
                openTime: $('#from').val(),
                expireTime: $('#to').val(),
                descending: $scope.searchParams.descending,
                wins: $scope.searchParams.wins,
            }
        }).then(function successCallback(response) {
            for (var i = 0; i < response.data.length; i++) {
                $scope.positions.push({
                    openTime: moment(parseInt(response.data[i].OpenTime.substr(6))).format("DD/MM/YYYY HH:mm:ss"),
                    instrumentName: response.data[i].InstrumentName,
                    direction: response.data[i].Direction,
                    amount: response.data[i].Amount,
                    openPrice: response.data[i].OpenPrice,
                    expireTime: moment(parseInt(response.data[i].ExpireTime.substr(6))).format("DD/MM/YYYY HH:mm:ss"),
                    closePrice: response.data[i].ClosePrice
                });
            }
        }, function errorCallback(response) {
            alertify.error("Oh crap, search failed.");
        });
    };

    $scope.getWinLoseData = function () {
        $http({
            method: 'GET',
            url: '/admin/TradingData'
        }).then(function successCallback(response) {
            $scope.tradingRatio = response.data;
            $scope.setupPieChart($scope.tradingRatio.WinLoseRatio);
            $scope.setupHighLowPieChart($scope.tradingRatio.HighLowRatio);
        }, function errorCallback(response) {
            console.log('failed to retrieve pie chart data');
        });
    };

    $scope.deleteUser = function (accountId) {
        $http({
            method: 'POST',
            url: '/admin/Delete',
            data:
            {
                accountId: accountId
            }
        }).then(function successCallback(response) {
            $scope.getAccounts();
            alertify.success("User delete successfuly.");
        }, function errorCallback(response) {
            console.log('failed to retrieve pie chart data');
        });
    };

    $scope.deposit = function () {
        $http({
            method: 'POST',
            url: '/admin/Deposit',
            data:
            {
                username: $scope.depositAccount,
                amount: $scope.depositBalance,
            }
        }).then(function successCallback(response) {
            $scope.getAccounts();
            alertify.success("balance updated successfuly.");
        }, function errorCallback(response) {
            console.log('failed to retrieve pie chart data');
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

    $scope.setupPieChart = function (data) {
        var width = 360;
        var height = 360;
        var radius = Math.min(width, height) / 2;

        var svg = d3.select('#pie')
          .append('svg')
          .attr('width', width)
          .attr('height', height)
          .append('g')
          .attr('transform', 'translate(' + (width / 2) +
            ',' + (height / 2) + ')');

        var arc = d3.svg.arc()
          .outerRadius(radius);

        var pie = d3.layout.pie()
          .value(function (d) {
              return d.Value;
          })
          .sort(null);

        var path = svg.selectAll('path')
            .data(pie(data))
            .enter()
            .append('path')
            .attr('d', arc) 
            .attr('fill', function (d, i) {
                return d.data.Key == "Win" ? '#5CB811' : '#FE1A00';
        });
    };

    $scope.setupHighLowPieChart = function (data) {
        var width = 360;
        var height = 360;
        var radius = Math.min(width, height) / 2;

        var svg = d3.select('#highLowPie')
          .append('svg')
          .attr('width', width)
          .attr('height', height)
          .append('g')
          .attr('transform', 'translate(' + (width / 2) +
            ',' + (height / 2) + ')');

        var arc = d3.svg.arc()
          .outerRadius(radius);

        var pie = d3.layout.pie()
          .value(function (d) {
              return d.Value;
          })
          .sort(null);

        var path = svg.selectAll('path')
            .data(pie(data))
            .enter()
            .append('path')
            .attr('d', arc)
            .attr('fill', function (d, i) {
                return d.data.Key == "High" ? '#735DCB' : '#F75403';
            });
    };

    //var width = 360;
    //var height = 360;
    //var radius = Math.min(width, height) / 2;

    //var svg = d3.select('#pie')
    //  .append('svg')
    //  .attr('width', width)
    //  .attr('height', height)
    //  .append('g')
    //  .attr('transform', 'translate(' + (width / 2) +
    //    ',' + (height / 2) + ')');

    //var arc = d3.svg.arc()
    //  .outerRadius(radius);

    //var pie = d3.layout.pie()
    //  .value(function (d) {
    //      return d.Value;
    //  })
    //  .sort(null);
}]);