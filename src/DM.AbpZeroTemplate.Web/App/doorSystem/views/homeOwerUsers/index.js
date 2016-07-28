(function () {
    appModule.controller('doorSystem.views.homeOwerUsers.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.homeOwerUser', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, homeOwerUserService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageHomeOwerUsers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwerUsers'),
                createHomeOwerUsers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwerUsers.Create'),
                editHomeOwerUsers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwerUsers.Edit'),
                deleteHomeOwerUsers: abp.auth.hasPermission('Pages.DoorSystem.HomeOwerUsers.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.homeOwerUsers = {

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
                                '<div class=\"ui-grid-cell-homeOwerUsers\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editHomeOwerUsers" ng-click="grid.appScope.homeOwerUsers.editHomeOwerUser(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteHomeOwerUsers" ng-click="grid.appScope.homeOwerUsers.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.homeOwerUsers.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('UserName'),
                            field: 'userName',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-userNames\" title="{{row.entity.userName}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('HomeOwerId'),
                            field: 'homeOwerId',
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

                            vm.homeOwerUsers.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.homeOwerUsers.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    homeOwerUserService.getHomeOwerUsers($.extend(vm.requestParams, {})).success(function (result) {
                        vm.homeOwerUsers.gridOptions.totalItems = result.totalCount;
                        vm.homeOwerUsers.gridOptions.data = result.items;
                    });
                },

                remove: function (homeOwerUser) {
                    abp.message.confirm(
                        app.localize('RemoveHomeOwerUserWarningMessage', homeOwerUser.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                homeOwerUserService.deleteHomeOwerUser({
                                    id: homeOwerUser.id
                                }).success(function () {
                                    vm.homeOwerUsers.load();
                                });
                            }
                        }
                    );
                },

                addHomeOwerUser: function (homeOwerUser) {
                    vm.homeOwerUsers.openCreateOrEditHomeOwerUserModal({

                    }, function (newHomeOwerUser) {
                        vm.homeOwerUsers.load();
                    });
                },

                editHomeOwerUser: function (homeOwerUser) {

                    vm.homeOwerUsers.openCreateOrEditHomeOwerUserModal(homeOwerUser, function (newHomeOwerUser) {
                        vm.homeOwerUsers.load();
                    });
                },

                openCreateOrEditHomeOwerUserModal: function (homeOwerUser, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/homeOwerUsers/createOrEditHomeOwerUserModal.cshtml',
                        controller: 'doorSystem.views.homeOwerUsers.createOrEditHomeOwerUserModal as vm',
                        backdrop: 'static',
                        resolve: {
                            homeOwerUser: function () {
                                return homeOwerUser;
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
                    if (!vm.permissions.manageHomeOwerUsers) {
                        vm.homeOwerUsers.gridOptions.columnDefs.shift();
                    }

                    vm.homeOwerUsers.load();
                },

                showDetails: function (homeOwerUser) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/homeOwerUsers/detailModal.cshtml',
                        controller: 'doorSystem.views.homeOwerUsers.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            homeOwerUser: function () {
                                return homeOwerUser;
                            }
                        }
                    });
                }
            }

            vm.homeOwerUsers.init();
        }
    ]);
})();
