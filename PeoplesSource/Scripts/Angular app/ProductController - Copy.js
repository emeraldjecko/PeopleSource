(function () {

    "use strict";
    angular
        .module('app')
        .controller('ProductController', ProductController);

    ProductController.$inject = ['$scope', 'productService'];
    /* @ngInject */
    function ProductController($scope, productService) {
        var vm = this;
        vm.getProducts = getProducts;
        vm.getNetoProducts = getNetoProducts;
        vm.updateProducts = updateProducts;
        vm.updateNetoProducts = updateNetoProducts;
        vm.products = [];
        vm.Netoproducts = [];
        vm.key = '';
        vm.Date = '';
        vm.ShippingCost = '2.75';
        vm.updatePrices = updatePrices;
        vm.updateNetoPrice2 = updateNetoPrice2;
        vm.basePrice = "";
        vm.haveProducts = false;
        vm.haveNetoProducts = false;
        vm.updateSelection = updateSelection;
        vm.updateNetoSelection = updateNetoSelection;
        vm.selectAll = true;
        vm.sortByAsc = sortByAsc;
        vm.sortByDesc = sortByDesc;
        vm.sortByNAsc = sortByNAsc;
        vm.sortByNDesc = sortByNDesc;
        vm.isSortByAsc = false;
        vm.isSortByDesc = false;
        vm.isSortByNAsc = false;
        vm.isSortByNDesc = false;
        vm.isLoding = false;
        vm.profitPrices = profitPrices;


        function getProducts() {

            vm.isLoding = true;
            productService.fetchProducts(vm.key).then(function (data) {
                var result = data.data;
                if (result.status === "Success") {
                    vm.products = result.products;


                    if (vm.products.length === 0) {
                        alert("No products found for " + vm.key);
                        vm.isLoding = false;
                        return;
                    }
                    //select all items
                    updateSelection();

                    //Add % symbol for percentage increments
                    for (var i = 0; i < vm.products.length; i++) {

                        var item = vm.products[i];
                        if (item.isPercentage && item.increment !== null) {
                            item.increment = item.increment + "%";
                        }

                        item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.07166666666) - 0.3 - (item.priceDefault * 0.029);

                    }
                    vm.haveProducts = true;
                    vm.isSortByAsc = true;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    vm.isLoding = false;
                    vm.updatePrices(vm.basePrice)
                    vm.profitPrices(vm.ShippingCost)

                }
                else {
                    alert("please check Real SKU!");
                }

            }, function (error) {

            });
        }
        function getNetoProducts() {
            
            vm.isLoding = true;
            productService.fetchNetoProducts(vm.Date).then(function (data) {
                var result = data.data;
                if (result.status === "Success") {
                    vm.netoproducts = result.netoproducts;


                    if (vm.netoproducts.length === 0) {
                        alert("No products found for " + vm.Date);
                        vm.isLoding = false;
                        return;
                    }
                    //select all items
                    //updateNetoSelection();

                    //Add % symbol for percentage increments
                    for (var i = 0; i < vm.netoproducts.length; i++) {

                        var item = vm.netoproducts[i];

                        item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.07166666666) - 0.3 - (item.priceDefault * 0.029);
                        item.price2 = '';
                        item.profit2 = '';
                    }
                    vm.haveNetoProducts = true;
                    //vm.isSortByNAsc = true;
                    //vm.isSortByNDesc = false;
                    vm.isLoding = false;
                    //vm.updateNetoPrice2(vm.Price2)
                    vm.sortByNAsc('sellerId');

                }
                else {
                    alert("please check Real SKU!");
                }

            }, function (error) {

            });
        }
        function profitPrices() {



            for (var i = 0; i < vm.products.length; i++) {
                var item = vm.products[i];
                item.firstProfitPrice = item.priceDefault - item.cost - vm.ShippingCost - (item.priceDefault * 0.07166666666) - 0.3 - (item.priceDefault * 0.029);

                if (item.updatedPrice == null || isNaN(item.updatedPrice))
                    item.secondProfitPrice = null
                else

                    item.secondProfitPrice = item.updatedPrice - item.cost - vm.ShippingCost - (item.updatedPrice * 0.07166666666) - 0.3 - (item.updatedPrice * 0.029);

            }

        }
        function updateNetoPrice2(itemnumber, cost, price2) {
          

            for (var i = 0; i < vm.netoproducts.length; i++) {
               
                if (vm.netoproducts[i].itemNumber == itemnumber) {

                    vm.netoproducts[i].profit2 = parseFloat(price2) - parseFloat(cost) - vm.ShippingCost - (parseFloat(price2) * 0.07166666666) - 0.3 - (parseFloat(price2) * 0.029);
                   
                }
                else
                    continue;

            }

        }
        function updatePrices(bPrice) {


            var basePrice = parseFloat(bPrice);

            for (var i = 0; i < vm.products.length; i++) {
                //&& vm.products[i].realSKU.indexOf('KZ') == -1 && vm.products[i].realSKU.indexOf('OHT') == -1
                if (vm.products[i].increment !== null) {
                    vm.products[i].updatedPrice = vm.products[i].isPercentage ? (basePrice + basePrice * parseFloat(vm.products[i].increment) / 100)
                    : basePrice + parseFloat(vm.products[i].increment);
                }
                else {

                    vm.products[i].updatedPrice = basePrice;
                }

                if (vm.products[i].kz !== null && vm.products[i].realSKU.indexOf('KZ') != -1) {

                    vm.products[i].updatedPrice += vm.products[i].kz;
                }
                if (vm.products[i].oht !== null && vm.products[i].realSKU.indexOf('OHT') != -1) {

                    vm.products[i].updatedPrice += vm.products[i].oht;
                }

                if (vm.products[i].updatedPrice == null || isNaN(vm.products[i].updatedPrice))
                    vm.products[i].secondProfitPrice = null
                else

                    vm.products[i].secondProfitPrice = parseFloat(vm.products[i].updatedPrice) - parseFloat(vm.products[i].cost) - vm.ShippingCost - (parseFloat(vm.products[i].updatedPrice) * 0.07166666666) - 0.3 - (parseFloat(vm.products[i].updatedPrice) * 0.029);

            }


        }

        function updateProducts() {

            var selectedProducts = vm.products.filter(getSelectedProducts);
            if (selectedProducts.length > 0) {

                productService.updateProducts(selectedProducts).then(function (data) {
                    var response = data.data;
                    alert(response.discription)
                    window.location.href = "/Products/Index"

                }, function (error) {
                    console.log('unable to load Products' + error);
                });
            }
            else {
                alert("Please select at least one item!")
            }
        }
        function updateNetoProducts(item) {
            debugger;
            alert(item);
            var selectedProducts = [];
            selectedProducts.push(item);
            //for (var i = 0; i < selectedProducts.length; i++) {

            //    var item = selectedProducts[i];
            //    alert(item.price2);
            //    if (item.price2 != null || item.price2 != "") {
            //        t.push(item);
            //        //selectedProducts.splice(6,item.itemNumber);
                    
            //    }
            //}
            //alert(t.length);
            //var selectedProducts = vm.netoproducts.filter(getSelectedProducts);
            if (selectedProducts.length >0) {

                productService.updateNetoProducts(selectedProducts).then(function (data) {
                    var response = data.data;
                    alert(response.discription)
                    window.location.href = "/Products/NetoProduct"

                }, function (error) {
                    console.log('unable to load Products' + error);
                });
            }
            else {
                alert("Please add price2!")
            }
        }
    
        function updateNetoSelection() {

            if (vm.selectAll) {
                vm.netoproducts.forEach(function (item) {
                    item.selected = true
                });
            }
            else {
                vm.netoproducts.forEach(function (item) {
                    item.selected = false;
                });
            }
        }
        function updateSelection() {
            if (vm.selectAll) {
                vm.products.forEach(function (item) {
                    item.selected = true
                });
            }
            else {
                vm.products.forEach(function (item) {
                    item.selected = false;
                });
            }
        }
        function getSelectedProducts(item) {

            if (item.selected) {
                return true;
            }
            else {
                return false;
            }
        }

        function sortByAsc(columnField) {



            var ascProducts = vm.products.sort(function (a, b) {
                if (columnField == "sellerId") {
                    vm.isSortByAsc = true;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.sellerId.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sellerId.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "realSKU") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = true;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.realSKU.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.realSKU.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "name") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = true;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.name.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.name.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "sku") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = true;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.sku.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sku.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "priceDefault") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = true;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.priceDefault; // ignore upper and lowercase
                    var nameB = b.priceDefault; // ignore upper and lowercase
                }
                else if (columnField == "cost") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = true;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.cost; // ignore upper and lowercase
                    var nameB = b.cost; // ignore upper and lowercase
                }
                else if (columnField == "firstProfitPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = true;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                else if (columnField == "increment") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = true;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.increment; // ignore upper and lowercase
                    var nameB = b.increment; // ignore upper and lowercase
                }
                else if (columnField == "kz") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = true;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.kz; // ignore upper and lowercase
                    var nameB = b.kz; // ignore upper and lowercase
                }
                else if (columnField == "oht") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = true;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.oht; // ignore upper and lowercase
                    var nameB = b.oht; // ignore upper and lowercase
                }
                else if (columnField == "updatedPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = true;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                else if (columnField == "secondProfitPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = true;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }

                // names must be equal
                return 0;
            });
            vm.products = ascProducts;

        }

        function sortByDesc(columnField) {


            var descProducts = vm.products.sort(function (a, b) {
                if (columnField == "sellerId") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = true;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.sellerId.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sellerId.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "realSKU") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = true;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.realSKU.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.realSKU.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "name") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = true;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.name.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.name.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "sku") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = true;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.sku.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sku.toUpperCase(); // ignore upper and lowercase
                }
                else if (columnField == "priceDefault") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = true;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.priceDefault; // ignore upper and lowercase
                    var nameB = b.priceDefault; // ignore upper and lowercase
                }
                else if (columnField == "cost") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = true;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.cost; // ignore upper and lowercase
                    var nameB = b.cost; // ignore upper and lowercase
                }
                else if (columnField == "firstProfitPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = true;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                else if (columnField == "increment") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = true;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.increment; // ignore upper and lowercase
                    var nameB = b.increment; // ignore upper and lowercase
                }
                else if (columnField == "kz") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = true;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.kz; // ignore upper and lowercase
                    var nameB = b.kz; // ignore upper and lowercase
                }
                else if (columnField == "oht") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = true;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.oht; // ignore upper and lowercase
                    var nameB = b.oht; // ignore upper and lowercase
                }
                else if (columnField == "updatedPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = true;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = false;
                    var nameA = a.updatedPrice; // ignore upper and lowercase
                    var nameB = b.updatedPrice; // ignore upper and lowercase
                }
                else if (columnField == "secondProfitPrice") {
                    vm.isSortByAsc = false;
                    vm.isSortByDesc = false;
                    vm.isRSortByAsc = false;
                    vm.isRSortByDesc = false;
                    vm.isPNSortByAsc = false;
                    vm.isPNSortByDesc = false;
                    vm.isSKUSortByAsc = false;
                    vm.isSKUSortByDesc = false;
                    vm.isPDSortByAsc = false;
                    vm.isPDSortByDesc = false;
                    vm.isCSortByAsc = false;
                    vm.isCSortByDesc = false;
                    vm.isNPPSortByAsc = false;
                    vm.isNPPSortByDesc = false;
                    vm.isISortByDesc = false;
                    vm.isISortByAsc = false;
                    vm.isKZSortByAsc = false;
                    vm.isKZSortByDesc = false;
                    vm.isOHTSortByAsc = false;
                    vm.isOHTSortByDesc = false;
                    vm.isUPSortByAsc = false;
                    vm.isUPSortByDesc = false;
                    vm.isSPPSortByAsc = false;
                    vm.isSPPSortByDesc = true;
                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                if (nameA > nameB) {
                    return -1;
                }
                if (nameA < nameB) {
                    return 1;
                }

                // names must be equal
                return 0;
            });
            vm.products = descProducts;

        }
        function sortByNAsc(columnField) {
        
           

            var ascProducts = vm.netoproducts.sort(function (a, b) {
                if (columnField == "sellerId") {
                    vm.isSortByNAsc = true;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.sellerId.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sellerId.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "realSKU") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = true;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.realSKU.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.realSKU.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "title") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = true;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.title.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.title.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "position") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = true;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA =  parseInt( a.position); // ignore upper and lowercase
                    var nameB =  parseInt( b.position); // ignore upper and lowercase
                } 
                if (columnField == "totalListings") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = true;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = parseInt(a.totalListings); // ignore upper and lowercase
                    var nameB = parseInt(b.totalListings); // ignore upper and lowercase
                  
                }
                if (columnField == "netoSKU") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = true;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.netoSKU; // ignore upper and lowercase
                    var nameB = b.netoSKU; // ignore upper and lowercase
                }
                if (columnField == "itemNumber") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = true;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.itemNumber; // ignore upper and lowercase
                    var nameB = b.itemNumber; // ignore upper and lowercase
                }
                if (columnField == "cost") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = true;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.cost; // ignore upper and lowercase
                    var nameB = b.cost; // ignore upper and lowercase
                }
                if (columnField == "priceDefault") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = true;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.priceDefault; // ignore upper and lowercase
                    var nameB = b.priceDefault; // ignore upper and lowercase
                }
                if (columnField == "firstProfitPrice") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = true;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                if (columnField == "profit2") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = true;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.profit2; // ignore upper and lowercase
                    var nameB = b.profit2; // ignore upper and lowercase
                }
                
                if (nameA < nameB) {
                    return -1;
                }
                if (nameA > nameB) {
                    return 1;
                }

                // names must be equal
                return 0;
            });
            vm.netoproducts = ascProducts;

        }

        function sortByNDesc(columnField) {
         
            
            var descProducts = vm.netoproducts.sort(function (a, b) {
                if (columnField == "sellerId") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc =  true;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.sellerId.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.sellerId.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "realSKU") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = true;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.realSKU.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.realSKU.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "title") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = true;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.title.toUpperCase(); // ignore upper and lowercase
                    var nameB = b.title.toUpperCase(); // ignore upper and lowercase
                }
                if (columnField == "position") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = true;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA =  parseInt( a.position); // ignore upper and lowercase
                    var nameB =  parseInt( b.position); // ignore upper and lowercase
                }
                if (columnField == "totalListings") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = true;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA =  parseInt(a.totalListings); // ignore upper and lowercase
                    var nameB =  parseInt(b.totalListings); // ignore upper and lowercase
                   
                }
                if (columnField == "netoSKU") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = true;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.netoSKU; // ignore upper and lowercase
                    var nameB = b.netoSKU; // ignore upper and lowercase
                }
                if (columnField == "itemNumber") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = true;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.itemNumber; // ignore upper and lowercase
                    var nameB = b.itemNumber; // ignore upper and lowercase
                }
                if (columnField == "cost") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = true;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.cost; // ignore upper and lowercase
                    var nameB = b.cost; // ignore upper and lowercase
                }
                if (columnField == "priceDefault") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = true;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.priceDefault; // ignore upper and lowercase
                    var nameB = b.priceDefault; // ignore upper and lowercase
                }
                if (columnField == "firstProfitPrice") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = true;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = false;

                    var nameA = a.firstProfitPrice; // ignore upper and lowercase
                    var nameB = b.firstProfitPrice; // ignore upper and lowercase
                }
                if (columnField == "profit2") {
                    vm.isSortByNAsc = false;
                    vm.isSortByNDesc = false;

                    vm.isRSortByNAsc = false;
                    vm.isRSortByNDesc = false;

                    vm.isTitlesortByNAsc = false;
                    vm.isTitlesortByNDesc = false;

                    vm.ispositionsortByNAsc = false;
                    vm.ispositionsortByNDesc = false;

                    vm.istotalListingssortByNAsc = false;
                    vm.istotalListingssortByNDesc = false;

                    vm.isNetoSKUsortByNAsc = false;
                    vm.isNetoSKUsortByNDesc = false;

                    vm.isItemNumbersortByNAsc = false;
                    vm.isItemNumbersortByNDesc = false;

                    vm.isCsortByNAsc = false;
                    vm.isCsortByNDesc = false;

                    vm.isPDsortByNAsc = false;
                    vm.isPDsortByNDesc = false;

                    vm.isNPPsortByNAsc = false;
                    vm.isNPPsortByNDesc = false;

                    vm.isprofit2sortByNAsc = false;
                    vm.isprofit2sortByNDesc = true;

                    var nameA = a.profit2; // ignore upper and lowercase
                    var nameB = b.profit2; // ignore upper and lowercase
                }
              
                if (nameA > nameB) {
                    return -1;
                }
                if (nameA < nameB) {
                    return 1;
                }

                // names must be equal
                return 0;
            });
            vm.netoproducts = descProducts;

        }

    }

})();
