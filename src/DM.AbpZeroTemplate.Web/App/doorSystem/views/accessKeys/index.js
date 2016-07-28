(function () {
    appModule.controller('doorSystem.views.accessKeys.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.accessKey', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, accessKeyService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageAccessKeys: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys'),
                createAccessKeys: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Create'),
                editAccessKeys: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Edit'),
                deleteAccessKeys: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Delete'),
                authAccessKeys: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Auth')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.accessKeys = {

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
                                '<div class=\"ui-grid-cell-accessKeys\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editAccessKeys" ng-click="grid.appScope.accessKeys.editAccessKey(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteAccessKeys" ng-click="grid.appScope.accessKeys.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.accessKeys.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.authAccessKeys" ng-click="grid.appScope.accessKeys.authAccessKey(row.entity)">' + app.localize('AccessKeyAuth') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('DoorName'),
                            field: 'doorName',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-accessKeys\" title="{{row.entity.doorName}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'homeOwerName',
                            minWidth: 200
                        },
                        {
                            name: app.localize('Validity'),
                            field: 'validity',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 200
                        },
                        {
                            name: app.localize('AccessKeyIsAuth'),
                            field: 'isAuth',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.isAuth" class="label label-success">' + app.localize('Yes') + '</span>' +
                                '  <span ng-show="!row.entity.isAuth" class="label label-default">' + app.localize('No') + '</span>' +
                                '</div>'
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

                            vm.accessKeys.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.accessKeys.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    accessKeyService.getAccessKeys($.extend(vm.requestParams, {})).success(function (result) {
                        vm.accessKeys.gridOptions.totalItems = result.totalCount;
                        vm.accessKeys.gridOptions.data = result.items;
                    });
                },

                remove: function (accessKey) {
                    abp.message.confirm(
                        app.localize('RemoveAccessKeyWarningMessage', accessKey.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                accessKeyService.deleteAccessKey({
                                    id: accessKey.id
                                }).success(function () {
                                    vm.accessKeys.load();
                                });
                            }
                        }
                    );
                },

                authAccessKey: function (accessKey) {
                    abp.message.confirm(
                        app.localize('AuthAccessKeyWarningMessage', accessKey.doorName, accessKey.homeOwerName),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                accessKeyService.authAccessKey({ id: accessKey.id })
                                .success(function (result) {
                                    if (result.isAuth) {
                                        abp.message.success(app.localize("AuthAccessKeySuccess"));
                                        vm.accessKeys.load();
                                    }
                                    else {
                                        abp.message.error(app.localize("AuthAccessKeyError"));
                                    }
                                });
                            }
                        }
                        );

                },

                addAccessKey: function (accessKey) {
                    vm.accessKeys.openCreateOrEditAccessKeyModal({

                    }, function (newAccessKey) {
                        vm.accessKeys.load();
                    });
                },

                editAccessKey: function (accessKey) {

                    vm.accessKeys.openCreateOrEditAccessKeyModal(accessKey, function (newAccessKey) {
                        vm.accessKeys.load();
                    });
                },

                openCreateOrEditAccessKeyModal: function (accessKey, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/accessKeys/createOrEditAccessKeyModal.cshtml',
                        controller: 'doorSystem.views.accessKeys.createOrEditAccessKeyModal as vm',
                        backdrop: 'static',
                        resolve: {
                            accessKey: function () {
                                return accessKey;
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
                    if (!vm.permissions.manageAccessKeys) {
                        vm.accessKeys.gridOptions.columnDefs.shift();
                    }

                    vm.accessKeys.load();
                },

                showDetails: function (accessKey) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/accessKeys/detailModal.cshtml',
                        controller: 'doorSystem.views.accessKeys.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            accessKey: function () {
                                return accessKey;
                            }
                        }
                    });
                }
            }

            vm.accessKeys.init();
        }
    ]);
})();
