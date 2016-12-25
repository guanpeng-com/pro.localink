(function () {
    appModule.controller('doorSystem.views.keyHoldings.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.keyHolding', 'appSession', 'abp.services.app.community', 'abp.services.app.commonLookup', 'lookupModal',
        function ($scope, $uibModal, $q, uiGridConstants, keyHoldingService, $appSession, communityService, commonLookupService, lookupModal) {
            var vm = this;
            vm.communityies = [];

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageKeyHoldings: abp.auth.hasPermission('Pages.DoorSystem.KeyHoldings'),
                createKeyHoldings: abp.auth.hasPermission('Pages.DoorSystem.KeyHoldings.Create'),
                editKeyHoldings: abp.auth.hasPermission('Pages.DoorSystem.KeyHoldings.Edit'),
                deleteKeyHoldings: abp.auth.hasPermission('Pages.DoorSystem.KeyHoldings.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null,
                communityId: null,
                homeOwerId: null
            };

            vm.selectHomeOwer = function () {
                lookupModal.open({
                    title: app.localize('SelectAHomeOwer'),
                    serviceMethod: commonLookupService.findHomeOwers,
                    extraFilters: {
                        communityId: vm.requestParams.communityId
                    },
                    callback: function (selectedItem) {
                        vm.requestParams.homeOwerId = selectedItem.value;
                        vm.requestParams.homeOwerName = selectedItem.name;
                    }
                });
            };


            vm.keyHoldings = {

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
                                '<div class=\"ui-grid-cell-keyHoldings\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editKeyHoldings" ng-click="grid.appScope.keyHoldings.editKeyHolding(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteKeyHoldings" ng-click="grid.appScope.keyHoldings.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.keyHoldings.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('VisitorName'),
                            field: 'visitorName',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-keyHoldings\" title="{{row.entity.visitorName}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('VisiteStartTime'),
                            field: 'visiteStartTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 180
                        },
                        {
                            name: app.localize('VisiteEndTime'),
                            field: 'visiteEndTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 180
                        },
                        {
                            name: app.localize('CommunityName'),
                            field: 'communityName',
                            minWidth: 180
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'homeOwerName',
                            minWidth: 180
                        },
                        {
                            name: app.localize('IsCollection'),
                            field: 'isCollection',
                            cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span ng-show="row.entity.isCollection" class="label label-success">' + app.localize('Yes') + '</span>' +
                            '  <span ng-show="!row.entity.isCollection" class="label label-default">' + app.localize('No') + '</span>' +
                            '</div>',
                            minWidth: 100
                        },
                       {
                           name: app.localize('CollectionTime'),
                           field: 'collectionTimeString',
                           //cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                           minWidth: 180
                       },
                    ],
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        $scope.gridApi.core.on.sortChanged($scope, function (grid, sortColumns) {
                            if (!sortColumns.length || !sortColumns[0].field) {
                                vm.requestParams.sorting = null;
                            } else {
                                vm.requestParams.sorting = sortColumns[0].field + ' ' + sortColumns[0].sort.direction;
                            }

                            vm.keyHoldings.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.keyHoldings.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    keyHoldingService.getKeyHoldings($.extend(vm.requestParams, {})).success(function (result) {
                        vm.keyHoldings.gridOptions.totalItems = result.totalCount;
                        vm.keyHoldings.gridOptions.data = result.items;
                    });
                },

                remove: function (keyHolding) {
                    abp.message.confirm(
                        app.localize('RemoveKeyHoldingWarningMessage', keyHolding.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                keyHoldingService.deleteKeyHolding({
                                    id: keyHolding.id
                                }).success(function () {
                                    vm.keyHoldings.load();
                                });
                            }
                        }
                    );
                },

                addKeyHolding: function (keyHolding) {
                    vm.keyHoldings.openCreateOrEditKeyHoldingModal({

                    }, function (newKeyHolding) {
                        vm.keyHoldings.load();
                    });
                },

                editKeyHolding: function (keyHolding) {

                    vm.keyHoldings.openCreateOrEditKeyHoldingModal(keyHolding, function (newKeyHolding) {
                        vm.keyHoldings.load();
                    });
                },

                openCreateOrEditKeyHoldingModal: function (keyHolding, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/keyHoldings/createOrEditKeyHoldingModal.cshtml',
                        controller: 'doorSystem.views.keyHoldings.createOrEditKeyHoldingModal as vm',
                        backdrop: 'static',
                        resolve: {
                            keyHolding: function () {
                                return keyHolding;
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

                    communityService.getUserCommunities()
.success(function (result) {
    vm.communityies = result;
    setTimeout(function () {
        $('#communitySelectedCombox').selectpicker('refresh');
        $('#genderSelectedCombox').selectpicker('refresh');
    }, 0);
});

                    if (!vm.permissions.manageKeyHoldings) {
                        vm.keyHoldings.gridOptions.columnDefs.shift();
                    }

                    vm.keyHoldings.load();
                },

                showDetails: function (keyHolding) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/keyHoldings/detailKeyHolding.cshtml',
                        controller: 'doorSystem.views.keyHoldings.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            keyHolding: function () {
                                return keyHolding;
                            }
                        }
                    });
                }
            }

            vm.keyHoldings.init();
        }
    ]);
})();
