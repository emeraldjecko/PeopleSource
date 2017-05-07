    var app = angular.module('app');
        app.factory('orderService', ['$http', '$q', orderServiceFn]);
    function orderServiceFn($http, $q)
    {
       var service = {
            fetchOrders: fetchOrders,
            getTrackerDetails: getTrackerDetails,
            getClosedTracking: getClosedTracking
        };
        var baseUrl = '/';
       
        
        function fetchOrders() {
           
            return $http.get('/Orders/GetOrders')
            .success(function (data) {
               
                
                return data.data;
            }).error(function (data) {
              
                return;
            });
        }
        function getTrackerDetails(ordersId) {
            
            return $http.post('/Orders/GetReturnTracking?ordersId='+ordersId)
          .success(function (data) {
              return data.data;
          }).error(function (data) {
              return;
          });
        }
        function getClosedTracking(orders) {
         
            return $http.post('/Orders/ClosedTracking', orders)
          .success(function (data) {
              return data.data;
          }).error(function (data) {
              return;
          });
        }
        return service;
    }
