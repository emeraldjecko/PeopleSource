var app = angular.module("app");
app.controller('OrdersController', ordersController);
ordersController.$inject = ['$scope', 'orderService', 'orderByFilter'];
function ordersController($scope, orderService, orderBy) {

    var od = this;
    od.getOrders = getOrders;
    od.Retracking = Retracking;
    od.orders = []
    od.isLoding = false;
    $scope.orderByField = "date";
    $scope.reverseSort = true;
    od.selectAll = true;
    od.updateOrderDetails = updateOrderDetails;
    od.Show = Show;
    od.hide = true;
    od.ClosedTracking = ClosedTracking;

    function ClosedTracking(item) {
        
        orderService.getClosedTracking(item).then(function (data) {

            var result = data.data;
            //Here we are checking the status of the request
            if (result.status === "Success") {
              
                getOrders();
            }
        }, function (error) {
            alert(error);
        });
    }

    function Show() {       
        od.hide = !od.hide;        
    }

    function getOrders() {


        //Firstly i have start the loader to run before start fetching record
        od.isLoding = true;
        orderService.fetchOrders().then(function (data) {

            var result = data.data;
            //Here we are checking the status of the request
            if (result.status === "Success") {

                od.orders = result.orders;
                //Show msg if no Order is found
                if (od.orders.length === 0) {
                    alert("No Orders found ");
                    od.isLoding = false;
                    return;
                }
                od.CheckedList = "";
                od.isLoding = false;
                //od.orderByField = "date";              
                //od.reverseSort = true;
                updateOrderDetails();
                //od.orders = orderBy(od.orders, od.orderByField, od.reverseSort);
            }
        }, function (error) {

        });
    }

    function updateOrderDetails() {

        if (od.selectAll) {
            od.orders.forEach(function (item) {
                item.selected = true
            });
        }
        else {
            od.orders.forEach(function (item) {
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

    function Retracking() {

        od.isLoding = true;
        var SelectedOrders = od.orders.filter(getSelectedProducts);
        if (SelectedOrders.length > 0) {
          
            
            SelectedOrders.forEach(function (item) {
                if (od.CheckedList == "" || od.CheckedList == undefined)
                    od.CheckedList = item.id.toString();
                else
                    od.CheckedList += "," + item.id.toString();
            });
            orderService.getTrackerDetails(od.CheckedList).then(function (data) {

                var result = data.data;
                //Here we are checking the status of the request
                if (result.status === "Success") {
                    od.orders = result.orders;


                    //Show msg if no Order is found
                    if (od.orders.length === 0) {
                        alert("No Orders found ");
                        od.isLoding = false;
                        return;
                    }
                  
                    od.isLoding = false;
                    od.orderByField = "date";
                    od.reverseSort = true;
                    updateOrderDetails();
                    od.orders = orderBy(od.orders, od.orderByField, od.reverseSort);
                }
            }, function (error) {

            });
        }
        else {
            alert("Please select at least one item!")
        }
    }

    $('a[href^="#"]').click(function (e) {
        e.preventDefault();
    });
}
