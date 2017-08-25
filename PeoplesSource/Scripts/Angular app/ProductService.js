(function () {

    "use strict";
    angular
        .module('app')
        .factory('productService', ['$http', '$q', productServiceFn]);

    function productServiceFn($http, $q) {
        var service = {
            fetchProducts: fetchProducts,
            updateProducts: updateProducts,            
            fetchNetoProducts: fetchNetoProducts,
            updateNetoProducts: updateNetoProducts,
            fetchNetoInDepthProducts: fetchNetoInDepthProducts,
            fetchOrders: fetchOrders

        };
        var baseUrl = '/';
        
        function fetchProducts(key, Pname) {

            return $http.get('/Products/GetProducts?realSKU="' + key + '"&PName="' + Pname.replace("+", "%2B") + '"')
            .success(function (data) {
                return data.data;
            }).error(function (data) {
                return;
            });
        }
        function updateProducts(products) {
            
            return $http.post('/Products/UpdateProducts', products)
                    .success(function (data) {
                        debugger;
                        return data.data;
                    }).error(function (data) {
                        return;
                    });
        }
        function updateNetoProducts(products) {
            return $http.post('/Products/UpdateNetoProducts', products)
                    .success(function (data) {
                        debugger;
                        return data.data;
                    }).error(function (data) {
                        return;
                    });
        }
        
        function fetchNetoProducts(key) {
            
            return $http.get('/Products/GetNetoProducts?date=' + key)
            .success(function (data) {
                return data.data;
            }).error(function (data) {
                return;
            });
        }
        function fetchNetoInDepthProducts(key,date,section) {
            
            return $http.get('/Products/GetNetoInDepthProducts?realSKU=' + key + '&date=' + date+'&section='+section)
            .success(function (data) {
                return data.data;
            }).error(function (data) {
                return;
            });
        }
        function fetchOrders() {
            debugger;
            return $http.get('/Orders/GetOrders')
            .success(function (data) {
                return data.data;
            }).error(function (data) {
                return;
            });
        }
        return service;
    }


})();