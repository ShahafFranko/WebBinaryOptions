
app.controller('loginController', ['$scope', '$http', function ($scope, $http) {

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
            window.location.href = 'http://localhost:2641/';
        }, function errorCallback(response) {
            alertify.error("Login Failed, please try again.");
        });
    };

}]);