'use strict';

app.factory('hubProxy', ['$rootScope', function ($rootScope) {

    function hubProxy(serverUrl, hubName) {
          var connection = $.hubConnection('http://localhost:2641/');
          var proxy = connection.createHubProxy('tradingHub');

          connection.start().done(function() {
              $rootScope.$broadcast('connectionEstablished');
          });

          return {
              on: function (eventName, callback) {
                  proxy.on(eventName, function (result) {
                      $rootScope.$apply(function () {
                          if (callback) {
                              callback(result);
                          }
                      });
                  });
              },
              invoke: function (methodName, callback) {
                  proxy.invoke(methodName)
                  .done(function (result) {
                      $rootScope.$apply(function () {
                          if (callback) {
                              callback(result);
                          }
                      });
                  });
              }
          };
      };

    return hubProxy;
  }]);