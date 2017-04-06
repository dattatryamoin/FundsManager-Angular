var app = angular.module('app', ['toaster', 'ngAnimate', 'ngTouch', 'ui.grid', 'ui.grid.pagination', 'ui.grid.selection']);

app.controller('MainCtrl', ['$scope', '$http', 'toaster', function ($scope, $http, toaster) {
    $scope.gridOptions = {
        paginationPageSizes: [8, 25, 50, 75],
        paginationPageSize: 8,
        enableRowSelection: true,
        enableColumnMenus: false,
        columnDefs: [
                        { name: 'schemeCode', width: 140 },
                        { name: 'schemeName', width: '*' },
                        { name: 'isinDivPayout_ISINGrowth', visible: false },
                        { name: 'isinDivReinvestment', visible: false },
                        { name: 'netAssetValue', cellFilter: 'number:2', width: 150 },
                        { name: 'repurchasePrice', cellFilter: 'number:2', width: 170 },
                        { name: 'salePrice', cellFilter: 'number:2', width: 110 },
                        { name: 'date', cellFilter: 'date:\'yyyy-MM-dd\'', width: 110 },
                        { name: 'fundHouse', visible: false },
                        { name: 'schemeType', visible: false }
        ],
        enableColumnResize: true
    };

    $scope.gridOptions.multiSelect = true;

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;

        // Single row select in Grid
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            var msg = 'row selected ' + row.isSelected;
            console.log(row.entity);
        });

        // Multirow row select in Grid
        gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            var msg = 'rows changed ' + rows.length;
            console.log(rows);
        });

    }


    // Saving funds
    $scope.CustomList = [];

    $scope.saveFunds = function () {

        $scope.CustomList = $scope.gridApi.selection.getSelectedRows();

        if ($scope.CustomList.length < 1) {
                        toaster.pop({
                            type: 'warning',
                            title: 'Warning',
                            body: 'Please select funds to save',
                            timeout: 3000
                        });
        }
        else {

                        $http({
                            method: 'POST',
                            url: 'http://localhost:3242/api/Funds/SaveCustomList',
                            data: angular.toJson($scope.CustomList)
                        }).then(
                        function (response) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Funds Saved Successfully !',
                                timeout: 3000
                            });
            },
            function (reason) {
                        toaster.pop({
                            type: 'error',
                            title: 'Error',
                            body: 'Error while saving funds',
                            timeout: 3000
                        });
                        console.log(reason.data);
            }
            );
        }
    };



    // Funds Api's 

    $http({ method: 'GET', url: 'http://localhost:3242/api/Funds/GetAllFundHouses' }).
       then(
       function successCallback(response) {
           $scope.fundHousesList = response.data;
       },
       function errorCallback(response) {
           console.log(response);
       });




    $http({ method: 'GET', url: 'http://localhost:3242/api/Funds/GetAllSchemeTypes' }).
            then(
            function successCallback(response) {
                $scope.allSchemeTypes = response.data;
            },
            function errorCallback(response) {
                console.log(response);
            });

    $scope.selectedFundHouse = "";
    $scope.selectedSchemeType = "";

    $scope.selectedItemChanged = function (selectedFundHouse, selectedSchemeType) {

        if (selectedFundHouse != "" && selectedSchemeType != "") {

            $scope.gridOptions.data = [];

            var compositeUrl = "http://localhost:3242/api/Funds/GetFundsBySchemeTypeAndFundHouse?schemeType=" + selectedSchemeType + "&fundHouse=" + selectedFundHouse;

            $http({ method: 'GET', url: compositeUrl }).
                   then(
                   function successCallback(response) {
                       $scope.gridOptions.data = response.data;
                   },
                   function errorCallback(response) {
                       console.log(response);
                   });
        }
    };

}]);



//$scope.CustomList = [];
//$scope.AddToCustomList = function (item) {
//    $scope.CustomList.push(item);
//};

//$scope.RemoveFromCustomList = function (index, item) {
//    $scope.CustomList.splice(index, 1);
//};

//$scope.UploadCustomList = function (items) {
//    $log.info(items);
//};