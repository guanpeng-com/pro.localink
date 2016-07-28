(function () {
    appModule.controller('doorSystem.views.openAttemps.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.openAttemp', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, openAttempService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageOpenAttemps: abp.auth.hasPermission('Pages.DoorSystem.OpenAttemps'),
                deleteOpenAttemps: abp.auth.hasPermission('Pages.DoorSystem.OpenAttemps.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.openAttemps = {

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
                                '<div class=\"ui-grid-cell-openAttemps\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteOpenAttemps" ng-click="grid.appScope.openAttemps.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'homeOwerName',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-openAttemps\" title="{{row.entity.homeOwerName}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('CreationTime'),
                            field: 'creationTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
                            minWidth: 200
                        },
                        {
                            name: app.localize('BrowerInfo'),
                            field: 'browerInfo',
                            minWidth: 200
                        },
                        {
                            name: app.localize('ClientName'),
                            field: 'clientName',
                            minWidth: 150
                        },
                        {
                            name: app.localize('ClientIpAddress'),
                            field: 'clientIpAddress',
                            minWidth: 150
                        },
                        {
                            name: app.localize('IsSuccess'),
                            field: 'isSuccess',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents\">' +
                                '  <span ng-show="row.entity.isSuccess" class="label label-success">' + app.localize('Yes') + '</span>' +
                                '  <span ng-show="!row.entity.isSuccess" class="label label-default">' + app.localize('No') + '</span>' +
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

                            vm.openAttemps.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.openAttemps.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    openAttempService.getOpenAttemps($.extend(vm.requestParams, {})).success(function (result) {
                        vm.openAttemps.gridOptions.totalItems = result.totalCount;
                        vm.openAttemps.gridOptions.data = result.items;
                    });
                },

                remove: function (openAttemp) {
                    abp.message.confirm(
                        app.localize('RemoveOpenAttempWarningMessage', openAttemp.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                openAttempService.deleteOpenAttemp({
                                    id: openAttemp.id
                                }).success(function () {
                                    vm.openAttemps.load();
                                });
                            }
                        }
                    );
                },

                init: function () {
                    if (!vm.permissions.manageOpenAttemps) {
                        vm.openAttemps.gridOptions.columnDefs.shift();
                    }

                    vm.openAttemps.load();
                }
            }

            vm.openAttemps.init();
        }
    ]);
})();
