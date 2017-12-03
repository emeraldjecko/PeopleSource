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
        
        function fetchProducts(key, Pname, SellerId, OperandProfit1, ValueProfit1, ShippingCost, OperandSRP30, ValueSRP30, OperandTUS30, ValueTUS30, OperandSRRS, ValueSRRS, OperandTURS, ValueTURS) {

            return $http.get('/Products/GetProducts?realSKU="' + key + '"&PName="' + Pname.replace("+", "%2B")
                                + '"&SellerId="' + SellerId.replace("+", "%2B") + '"&OperandProfit1="' + OperandProfit1.replace("+", "%2B")
                                + '"&ValueProfit1="' + ValueProfit1.replace("+", "%2B") + '"&ShippingCost="' + ShippingCost.replace("+", "%2B")
                                + '"&OperandSRP30="' + OperandSRP30.replace("+", "%2B") + '"&ValueSRP30="' + ValueSRP30.replace("+", "%2B")
                                + '"&OperandTUS30="' + OperandTUS30.replace("+", "%2B") + '"&ValueTUS30="' + ValueTUS30.replace("+", "%2B")
                                + '"&OperandSRRS="' + OperandSRRS.replace("+", "%2B") + '"&ValueSRRS="' + ValueSRRS.replace("+", "%2B")
                                + '"&OperandTURS="' + OperandTURS.replace("+", "%2B") + '"&ValueTURS="' + ValueTURS.replace("+", "%2B") + '"')
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