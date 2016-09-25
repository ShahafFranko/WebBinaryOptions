
app.controller('adminController', ['$scope', 'hubProxy', '$http', function ($scope, hubProxy, $http) {
    
    $scope.accounts = [];

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
    });

    $scope.createAccount = function() {
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
            alertify.success("query is gooooooooooooooooooood");
            //$scope.accounts = response.data;
        }, function errorCallback(response) {
            alertify.error("Oh crap, search failed.");
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
}]);