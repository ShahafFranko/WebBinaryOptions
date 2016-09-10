
app.controller('loginController', ['$scope', '$http', '$cookies', function ($scope, $http, $cookies) {

    $scope.username = null;
    $scope.password = null;

    $scope.login = function() {
        $http({
            method: 'POST',
            url: '/Login/Login',
            data:
            {
                Username: $scope.username,
                Password: $scope.password,
            }
        }).then(function successCallback(response) {
            $cookies.put('accountId', response.data);
            window.location.href = 'http://localhost:2641/';
        }, function errorCallback(response) {
            alertify.error("Login Failed, please try again.");
        });
    };

}]);