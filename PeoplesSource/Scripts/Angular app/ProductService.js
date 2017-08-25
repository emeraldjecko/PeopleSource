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
        
        function fetchProducts(key, Pname, pSellerId, profit1Operand, profit1Value1, profit1Value2, srp30Operand, srp30Value1, srp30Value2, ShippingCost) {
            
            //return $http.get('/Products/GetProducts?realSKU="' + key + '"&PName="' + Pname.replace("+", "%2B") + '"&SellerId="' + pSellerId + '"')
            return $http.get('/Products/GetProducts?realSKU="' + key + '"&pSellerId="' + pSellerId + '"&PName="' + Pname.replace("+", "%2B") + '"&profit1Operand="' + profit1Operand + '"&profit1Value1="' + profit1Value1 + '"&profit1Value2="' + profit1Value2 +
                + '"&srp30Operand="' + srp30Operand + '"&srp30Value1="' + srp30Value1 + '"&srp30Value2="' + srp30Value2 + '"&ShippingCost="' + ShippingCost + '"')
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