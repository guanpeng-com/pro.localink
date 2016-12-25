(function () {
    appModule.controller('doorSystem.views.homeOwers.index', [
        '$scope', '$state', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.homeOwer', 'appSession',
        function ($scope, $state, $uibModal, $q, uiGridConstants, homeOwerService, $appSession) {
            var vm = this;
            vm.appSession = $appSession;
            vm.homeOwerStatus = [];

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageHomeOwers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwers'),
                createHomeOwers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwers.Create'),
                editHomeOwers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwers.Edit'),
                deleteHomeOwers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwers.Delete'),
                doors: abp.auth.hasPermission('Pages.DoorSystem.Doors'),
                createAccessKey: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Create'),
                authHomeOwer: abp.auth.hasPermission('Pages.DoorSystem.HomeOwers.AuthHomeOwer')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null,
                homeOwerStatus: null
            };

            vm.homeOwers = {

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
                                '<div class=\"ui-grid-cell-homeOwers\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.doors"  ng-click="grid.appScope.homeOwers.toDoors(row.entity)">' + app.localize('ManageHomeOwerDoors') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.editHomeOwers" ng-click="grid.appScope.homeOwers.editHomeOwer(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteHomeOwers" ng-click="grid.appScope.homeOwers.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.homeOwers.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                //'      <li><a ng-if="grid.appScope.permissions.createAccessKey" ng-click="grid.appScope.homeOwers.applyKey(row.entity)">' + app.localize('ApplyKey') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.authHomeOwer" ng-click="grid.appScope.homeOwers.authHomeOwer(row.entity)">' + app.localize('AuthAndApplyKey') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'name',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-homeOwers\" title="{{row.entity.name}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('HomeOwerPhone'),
                            field: 'phone',
                            minWidth: 200
                        },
                        {
                            name: app.localize('HomeOwerEmail'),
                            field: 'email',
                            minWidth: 200
                        },
                        {
                            name: app.localize('HomeOwerStatus'),
                            field: 'status',
                            minWidth: 200
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

                            vm.homeOwers.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.homeOwers.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    homeOwerService.getHomeOwers($.extend(vm.requestParams, {})).success(function (result) {
                        vm.homeOwers.gridOptions.totalItems = result.totalCount;
                        vm.homeOwers.gridOptions.data = result.items;
                    });
                },

                remove: function (homeOwer) {
                    abp.message.confirm(
                        app.localize('RemoveHomeOwerWarningMessage', homeOwer.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                homeOwerService.deleteHomeOwer({
                                    id: homeOwer.id
                                }).success(function () {
                                    vm.homeOwers.load();
                                });
                            }
                        }
                    );
                },

                addHomeOwer: function (homeOwer) {
                    vm.homeOwers.openCreateOrEditHomeOwerModal({

                    }, function (newHomeOwer) {
                        vm.homeOwers.load();
                    });
                },

                editHomeOwer: function (homeOwer) {
                    vm.homeOwers.openCreateOrEditHomeOwerModal(homeOwer, function (newHomeOwer) {
                        vm.homeOwers.load();
                    });
                },

                toDoors: function (homeOwer) {
                    $state.go('doorSystem.homeOwerDoors', { homeOwerId: homeOwer.id, communityId: homeOwer.communityId });
                },

                applyKey: function (homeOwer) {
                    vm.homeOwers.openCreateOrEditAccessKeyModal(homeOwer, function (newKey) {
                        vm.homeOwers.load();
                    });
                },

                //审核业主信息
                authHomeOwer: function (homeOwer) {
                    abp.message.confirm(
                        app.localize('AuthHomeOwerWarningMessage',homeOwer.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                homeOwerService.authHomeOwer({ id: homeOwer.id })
                                .success(function () {
                                    vm.homeOwers.load();
                                });
                            }
                        });
                },

                openCreateOrEditHomeOwerModal: function (homeOwer, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/homeOwers/createOrEditHomeOwerModal.cshtml',
                        controller: 'doorSystem.views.homeOwers.createOrEditHomeOwerModal as vm',
                        backdrop: 'static',
                        resolve: {
                            homeOwer: function () {
                                return homeOwer;
                            }
                        }
                    });


                    if (modalInstance) {
                        modalInstance.result.then(function (result) {
                            closeCallback && closeCallback(result);
                        });
                    }
                },

                openCreateOrEditAccessKeyModal: function (homeOwer, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/accessKeys/createOrEditAccessKeyModal.cshtml',
                        controller: 'doorSystem.views.accessKeys.createOrEditAccessKeyModal as vm',
                        backdrop: 'static',
                        resolve: {
                            accessKey: function () {
                                return { communityId: homeOwer.communityId, homeOwerId: homeOwer.id };
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
                    if (!vm.permissions.manageHomeOwers) {
                        vm.homeOwers.gridOptions.columnDefs.shift();
                    }

                    homeOwerService.getAllHomeOwerStatus()
                    .success(function (result) {
                        vm.homeOwerStatus = result;
                    });

                    vm.homeOwers.load();
                },

                showDetails: function (homeOwer) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/homeOwers/detailModal.cshtml',
                        controller: 'doorSystem.views.homeOwers.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            homeOwer: function () {
                                return homeOwer;
                            }
                        }
                    });
                }
            }

            vm.homeOwers.init();
        }
    ]);
})();
