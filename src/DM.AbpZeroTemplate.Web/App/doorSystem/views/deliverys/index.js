(function () {
    appModule.controller('doorSystem.views.deliverys.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.delivery', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, deliveryService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageDeliverys: abp.auth.hasPermission('Pages.DoorSystem.Deliverys'),
                createDeliverys: abp.auth.hasPermission('Pages.DoorSystem.Deliverys.Create'),
                editDeliverys: abp.auth.hasPermission('Pages.DoorSystem.Deliverys.Edit'),
                deleteDeliverys: abp.auth.hasPermission('Pages.DoorSystem.Deliverys.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null,
                homeOwerName: null
            };

            vm.deliverys = {

                gridOptions: {
                    enableHorizontalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                    enableVerticalScrollbar: uiGridConstants.scrollbars.WHEN_NEEDED,
                    paginationPageSizes: app.consts.grid.defaultPageSizes,
                    paginationPageSize: app.consts.grid.defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: true,
                    appScopeProvider: vm,
                    columnDefs: [
                        {
                            name: app.localize('Actions'),
                            enableSorting: false,
                            width: 100,
                            cellTemplate:
                                '<div class=\"ui-grid-cell-deliverys\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editDeliverys" ng-click="grid.appScope.deliverys.editDelivery(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteDeliverys" ng-click="grid.appScope.deliverys.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.deliverys.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('DeliveryId'),
                            field: 'id',
                            minWidth: 100
                        },
                        {
                            name: app.localize('DeliveryTitle'),
                            field: 'title',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-deliverys\" title="{{row.entity.name}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 150
                        },
                        {
                            name: app.localize('CreationTime'),
                            field: 'creationTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 180
                        },
                        {
                            name: app.localize('DeliveryIsGather'),
                            field: 'isGather',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.isGather" class="label label-success">' + app.localize('Yes') + '</span>' +
                                '  <span ng-show="!row.entity.isGather" class="label label-default">' + app.localize('No') + '</span>' +
                                '</div>'
                        },
                        {
                            name: app.localize('DeliveryGatherTime'),
                            field: 'gatherTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 180
                        },
                        {
                            name: app.localize('DeliveryQRCode'),
                            cellTemplate: '<span ng-click="grid.appScope.deliverys.showQRCode(row.entity)" >QR Code</span>',
                            minWidth: 100
                        }
                    ],
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                            if (!sortColumns.length || !sortColumns[0].field) {
                                vm.requestParams.sorting = null;
                            } else {
                                vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                            }

                            vm.deliverys.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.deliverys.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    deliveryService.getDeliverys($.extend(vm.requestParams, {})).success(function (result) {
                        vm.deliverys.gridOptions.totalItems = result.totalCount;
                        vm.deliverys.gridOptions.data = result.items;
                    });
                },

                remove: function (delivery) {
                    abp.message.confirm(
                        app.localize('RemoveDeliveryWarningMessage', delivery.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                deliveryService.deleteDelivery({
                                    id: delivery.id
                                }).success(function () {
                                    vm.deliverys.load();
                                });
                            }
                        }
                    );
                },

                addDelivery: function (delivery) {
                    vm.deliverys.openCreateOrEditDeliveryModal({

                    }, function (newDelivery) {
                        vm.deliverys.load();
                    });
                },

                editDelivery: function (delivery) {

                    vm.deliverys.openCreateOrEditDeliveryModal(delivery, function (newDelivery) {
                        vm.deliverys.load();
                    });
                },

                openCreateOrEditDeliveryModal: function (delivery, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/deliverys/createOrEditDeliveryModal.cshtml',
                        controller: 'doorSystem.views.deliverys.createOrEditDeliveryModal as vm',
                        backdrop: 'static',
                        resolve: {
                            delivery: function () {
                                return delivery;
                            }
                        }
                    });


                    if (modalInstance) {
                        modalInstance.result.then(function (result) {
                            closeCallback && closeCallback(result);
                        });
                    }
                },

                init: function () {
                    if (!vm.permissions.manageDeliverys) {
                        vm.deliverys.gridOptions.columnDefs.shift();
                    }

                    vm.deliverys.load();
                },

                showDetails: function (delivery) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/deliverys/detailModal.cshtml',
                        controller: 'doorSystem.views.deliverys.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            delivery: function () {
                                return delivery;
                            }
                        }
                    });
                },

                showQRCode: function (delivery) {
                    $uibModal.open({
                        templateUrl: '~/App/common/views/QRCode/QRCode.cshtml',
                        controller: 'common.views.qrcode as vm',
                        backdrop: 'static',
                        resolve: {
                            qrcode: function () {
                                return {
                                    data: delivery.token
                                };
                            }
                        }
                    });
                }
            }

            vm.deliverys.init();
        }
    ]);
})();
