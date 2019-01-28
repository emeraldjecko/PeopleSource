(function () {

    "use strict";
    angular
        .module('app')
        .controller('ProductController', ProductController);

    ProductController.$inject = ['$scope', 'productService', 'orderByFilter', ];
    /* @ngInject */
    function ProductController($scope, productService, orderBy) {
        var vm = this;
        vm.getProducts = getProducts;
        vm.getNetoProducts = getNetoProducts;
        vm.getNetoInDepthProducts = getNetoInDepthProducts;
        vm.updateProducts = updateProducts;
        vm.updateNetoProducts = updateNetoProducts;
        vm.products = [];
        vm.Netoproducts = [];
        vm.key = '';
        vm.PName = '';
        vm.SellerId = '';
        vm.OperandProfit1 = '';
        vm.ValueProfit1 = '';
        vm.OperandSRP30 = '';
        vm.ValueSRP30 = '';
        vm.OperandTUS30 = '';
        vm.ValueTUS30 = '';
        vm.OperandSRRS = '';
        vm.ValueSRRS = '';
        vm.OperandTURS = '';
        vm.ValueTURS = '';
        vm.Profit1Percent = '0';
        vm.Profit2Percent = '0';
        vm.Date = '';
        vm.ShippingCost = '2.75';
        vm.updatePrices = updatePrices;
        vm.updateIncrementalPrices = updateIncrementalPrices;
        vm.updateNetoPrice2 = updateNetoPrice2;
        vm.basePrice = "";
        vm.Incremental = "";
        vm.haveProducts = false;
        vm.haveNetoProducts = false;
        vm.updateSelection = updateSelection;
        vm.updateNetoSelection = updateNetoSelection;
        vm.selectAll = true;
        vm.sortByAsc = sortByAsc;
        vm.sortByDesc = sortByDesc;
        vm.netProductsCount = 0;
        vm.isSortByAsc = false;
        vm.isSortByDesc = false;
        vm.isLoding = false;
        vm.profitPrices = profitPrices;
        vm.updatePricesThenPublish = updatePricesThenPublish;
        vm.updateIncrementalPricesThenPublish = updateIncrementalPricesThenPublish;
        $scope.orderByField = 'sellerId';
        $scope.reverseSort = false;
      
        function getProducts() {

            vm.isLoding = true;
            productService.fetchProducts(vm.key, vm.PName, vm.SellerId, vm.OperandProfit1, vm.ValueProfit1, vm.ShippingCost, vm.OperandSRP30, vm.ValueSRP30, vm.OperandTUS30, vm.ValueTUS30, vm.OperandSRRS, vm.ValueSRRS, vm.Profit1Percent, vm.Profit2Percent, vm.OperandTURS, vm.ValueTURS).then(function (data) {
                var result = data.data;
                if (result.status === "Success") {
                    vm.products = result.products;


                    if (vm.products.length === 0) {
                        alert("No products found for " + vm.key + ' ' + vm.PName);
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

                        //var profit1percent = parseFloat(vm.Profit1Percent);
                        alert("profit1percent parseFloat " + vm.Profit1Percent);

                        if (vm.Profit1Percent > 0) {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029) - (item.priceDefault * (vm.Profit1Percent / 100));
                            alert("Get With percent1: " + item.firstProfitPrice);
                        }
                        else {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029);
                            alert("Get No percent1: " + item.firstProfitPrice);
                        }
                        
                        item.diff = item.priceDefault - item.updatedPrice;
                    }

                    vm.haveProducts = true;
                    //vm.isSortByAsc = true;
                    //vm.isSortByDesc = false;
                    //vm.isRSortByAsc = false;
                    //vm.isRSortByDesc = false;
                    //vm.isPNSortByAsc = false;
                    //vm.isPNSortByDesc = false;
                    //vm.isSKUSortByAsc = false;
                    //vm.isSKUSortByDesc = false;
                    //vm.isPDSortByAsc = false;
                    //vm.isPDSortByDesc = false;
                    //vm.isCSortByAsc = false;
                    //vm.isCSortByDesc = false;
                    //vm.isNPPSortByAsc = false;
                    //vm.isNPPSortByDesc = false;
                    //vm.isISortByDesc = false;
                    //vm.isISortByAsc = false;
                    //vm.isKZSortByAsc = false;
                    //vm.isKZSortByDesc = false;
                    //vm.isOHTSortByAsc = false;
                    //vm.isOHTSortByDesc = false;
                    //vm.isUPSortByAsc = false;
                    //vm.isUPSortByDesc = false;
                    //vm.isSPPSortByAsc = false;
                    //vm.isSPPSortByDesc = false;
                    vm.isLoding = false;
                    vm.updatePrices(vm.basePrice)
                    vm.profitPrices(vm.ShippingCost)
                    vm.orderByField = 'priceDefault';
                    vm.reverseSort = false;
                    $scope.orderByField = 'priceDefault';
                    $scope.reverseSort = false;
                    vm.products = orderBy(vm.products, vm.orderByField, vm.reverseSort);
                }
                else {
                    //alert("please check Real SKU!");
                    alert("please enter atleast 1 valid field value to search!");
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
                        item.totalListings = parseInt(item.totalListings);

                        var profit1percent = parseFloat(vm.Profit1Percent);

                        if (profit1percent > 0) {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029) - (item.priceDefault * (profit1percent / 100));
                            alert("Get With percent 1 Neto: " + item.firstProfitPrice);
                        }
                        else {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029);
                            alert("Get No percent1 Neto: " + item.firstProfitPrice);
                        }
                        
                        item.price2 = '';
                        item.profit2 = '';
                    }
                    vm.haveNetoProducts = true;
                    vm.orderByField = 'sellerId';
                    vm.reverseSort = false;
                    vm.netoproducts = orderBy(vm.netoproducts, vm.orderByField, vm.reverseSort);
                    vm.isLoding = false;
                }
                else {
                    alert("please check Real SKU!");
                }

            }, function (error) {

            });
        }
        function getNetoInDepthProducts(realSKU, Date, section) {

            vm.isLoding = true;
            productService.fetchNetoInDepthProducts(realSKU, Date, section).then(function (data) {
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

                        //item.totalListings = parseInt(item.totalListings);

                        var profit1percent = parseFloat(vm.Profit1Percent);

                        if (profit1percent > 0) {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029) - (item.priceDefault * (profit1percent / 100));
                            alert("getNetoInDepthProducts With percent 1 Neto: " + item.firstProfitPrice);
                        }
                        else {
                            item.firstProfitPrice = item.priceDefault - item.cost - parseFloat(vm.ShippingCost) - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029);
                            alert("getNetoInDepthProducts No percent 1 Neto: " + item.firstProfitPrice);
                        }
                        
                        if (section == "Section1") {
                            item.price2 = '';
                            item.profit2 = '';
                            //$scope.orderByField = 'sellerId';
                            //vm.netoproducts = orderBy(vm.netoproducts, $scope.orderByField, $scope.reverseSort);
                        } else {
                            item.price2 = parseFloat(item.price2);
                            //$scope.orderByField = 'position';
                            //vm.netoproducts = orderBy(vm.netoproducts, $scope.orderByField, $scope.reverseSort);
                            $("#netproductscount").html(vm.netoproducts.length);

                        }

                    }
                    if (section == "Section1") {

                        $scope.orderByField = 'sellerId';
                        vm.netoproducts = orderBy(vm.netoproducts, $scope.orderByField, $scope.reverseSort);
                    } else {

                        $scope.orderByField = 'position';
                        vm.netoproducts = orderBy(vm.netoproducts, $scope.orderByField, $scope.reverseSort);
                        $("#netproductscount").html(vm.netoproducts.length);

                    }
                    vm.haveNetoProducts = true;

                    vm.isLoding = false;
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


                //var profit1percent = parseFloat(vm.Profit1Percent);
                //var profit2percent = parseFloat(vm.Profit2Percent);

                if (vm.Profit1Percent > 0) {
                    item.firstProfitPrice = item.priceDefault - item.cost - vm.ShippingCost - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029) - (item.priceDefault * (vm.Profit1Percent / 100));
                    alert("With percent1 from ProfitPrices method edit: " + item.firstProfitPrice);
                }
                else {
                    item.firstProfitPrice = item.priceDefault - item.cost - vm.ShippingCost - (item.priceDefault * 0.0915) - 0.3 - (item.priceDefault * 0.029);
                    alert("No percent1 from ProfitPrices method: " + item.firstProfitPrice);
                }
                

                if (item.updatedPrice == null || isNaN(item.updatedPrice))
                    item.secondProfitPrice = null
                else {
                    if (vm.Profit2Percent > 0) {
                        item.secondProfitPrice = item.updatedPrice - item.cost - vm.ShippingCost - (item.updatedPrice * 0.0915) - 0.3 - (item.updatedPrice * 0.029) - (item.updatedPrice * (vm.Profit2Percent / 100));
                        alert("With percent2 from ProfitPrices method: " + item.secondProfitPrice);
                    }
                    else {
                        item.secondProfitPrice = item.updatedPrice - item.cost - vm.ShippingCost - (item.updatedPrice * 0.0915) - 0.3 - (item.updatedPrice * 0.029);
                        alert("No percent2 from ProfitPrices method: " + item.secondProfitPrice);
                    }
                    
                }

                    

            }

        }
        function updateNetoPrice2(itemnumber, cost, price2) {


            for (var i = 0; i < vm.netoproducts.length; i++) {

                if (vm.netoproducts[i].itemNumber == itemnumber) {

                    if (vm.Profit2Percent > 0) {
                        vm.netoproducts[i].profit2 = parseFloat(price2) - parseFloat(cost) - vm.ShippingCost - (parseFloat(price2) * 0.0915) - 0.3 - (parseFloat(price2) * 0.029) - (price2 * (vm.Profit2Percent / 100));
                        alert("With percent2 from updateNetoPrice2 method: " + vm.netoproducts[i].profit2);
                    }
                    else {
                        vm.netoproducts[i].profit2 = parseFloat(price2) - parseFloat(cost) - vm.ShippingCost - (parseFloat(price2) * 0.0915) - 0.3 - (parseFloat(price2) * 0.029);
                        alert("No percent2 from updateNetoPrice2 method: " + vm.netoproducts[i].profit2);
                    }
                }
                else
                    continue;

            }

        }
        function updatePrices(bPrice) {

         

            var basePrice = parseFloat(bPrice);

            for (var i = 0; i < vm.products.length; i++) {
                //&& vm.products[i].realSKU.indexOf('KZ') == -1 && vm.products[i].realSKU.indexOf('OHT') == -1
                if (vm.products[i].increment !== null ) {
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
                {
                    if (vm.Profit2Percent > 0) {
                        vm.products[i].secondProfitPrice = parseFloat(vm.products[i].updatedPrice) - parseFloat(vm.products[i].cost) - vm.ShippingCost - (parseFloat(vm.products[i].updatedPrice) * 0.0915) - 0.3 - (parseFloat(vm.products[i].updatedPrice) * 0.029) - (vm.products[i].updatedPrice * (vm.Profit2Percent / 100));
                        alert("With percent2 from updatePrices for Base method: " + vm.products[i].secondProfitPrice);
                    }
                    else {
                        vm.products[i].secondProfitPrice = parseFloat(vm.products[i].updatedPrice) - parseFloat(vm.products[i].cost) - vm.ShippingCost - (parseFloat(vm.products[i].updatedPrice) * 0.0915) - 0.3 - (parseFloat(vm.products[i].updatedPrice) * 0.029);
                        alert("No percent2 from updatePrices for Base method: " + vm.products[i].secondProfitPrice);
                    }
                }
                vm.products[i].diff = vm.products[i].priceDefault - vm.products[i].updatedPrice;
            }


        }

        function updatePricesThenPublish(bPrice) {
            vm.updatePrices(bPrice);
            vm.updateProducts();
        }
        function updateIncrementalPrices(IPrice)
        {
            var iPrice = parseFloat(IPrice);
            vm.products.forEach(function (item) {
                if (item.selected)
                {
                    item.updatedPrice = item.priceDefault + iPrice;
                    //--No need to use kz and oht  in incremental price section
                    //if (item.kz !== null && item.realSKU.indexOf('KZ') != -1) {

                    //    item.updatedPrice += item.kz;
                    //}
                    //if (item.oht !== null && item.realSKU.indexOf('OHT') != -1) {

                    //    item.updatedPrice += item.oht;
                    //}

                    if (item == null || isNaN(item.updatedPrice))
                        item.secondProfitPrice = null
                    else {
                        if (vm.Profit2Percent > 0) {
                            item.secondProfitPrice = parseFloat(item.updatedPrice) - parseFloat(item.cost) - vm.ShippingCost - (parseFloat(item.updatedPrice) * 0.0915) - 0.3 - (parseFloat(item.updatedPrice) * 0.029) - (parseFloat(item.updatedPrice) * (vm.Profit2Percent / 100));
                            alert("With percent2 from updateIncrementalPrices method: " + item.secondProfitPrice);
                        }
                        else {
                            item.secondProfitPrice = parseFloat(item.updatedPrice) - parseFloat(item.cost) - vm.ShippingCost - (parseFloat(item.updatedPrice) * 0.0915) - 0.3 - (parseFloat(item.updatedPrice) * 0.029);
                            alert("No percent2 from updateIncrementalPrices method: " + item.secondProfitPrice);
                        }
                        
                    }

                        
                    item.diff = item.priceDefault - item.updatedPrice;
                }
            });
        }

        function updateIncrementalPricesThenPublish(IPrice) {
            vm.updateIncrementalPrices(IPrice);
            vm.updateProducts();
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
            if (selectedProducts.length > 0) {

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



    }

})();
