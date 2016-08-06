﻿(function () {
    appModule.controller('doorSystem.views.doors.homeOwerDoorIndex', [
        '$scope', '$state', '$stateParams', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.door', 'appSession', 'abp.services.app.accessKey',
        function ($scope, $state, $stateParams, $uibModal, $q, uiGridConstants, doorService, $appSession, accessKeyService) {
            var vm = this;

            //homeOwerId如果存在，那么显示业主关联的门禁；
            //homeOwerId如果不存在，那么所有门禁；
            vm.homeOwerId = $stateParams.homeOwerId;
            vm.communityId = $stateParams.communityId;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageDoors: abp.auth.hasPermission('Pages.DoorSystem.Doors'),
                createDoors: abp.auth.hasPermission('Pages.DoorSystem.Doors.Create'),
                deleteDoors: abp.auth.hasPermission('Pages.DoorSystem.Doors.Delete'),
                createAccessKey: abp.auth.hasPermission('Pages.DoorSystem.AccessKeys.Create') && vm.homeOwerId
            };

            vm.requestParams = {
                communityId: vm.communityId,
                homeOwerId: vm.homeOwerId,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.doors = {

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
                                '<div class=\"ui-grid-cell-doors\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteDoors" ng-click="grid.appScope.doors.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.createAccessKey" ng-click="grid.appScope.doors.applyKey(row.entity)">' + app.localize('ApplyKey') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('DoorName'),
                            field: 'name',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-doors\" title="{{row.entity.name}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('DoorType'),
                            field: 'doorType',
                            minWidth: 200
                        },
                        {
                            name: app.localize('DoorIsAuth'),
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

                            vm.doors.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.doors.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    doorService.getDoors($.extend(vm.requestParams, {})).success(function (result) {
                        vm.doors.gridOptions.totalItems = result.totalCount;
                        vm.doors.gridOptions.data = result.items;
                    });
                },

                remove: function (door) {
                    abp.message.confirm(
                        app.localize('RemoveDoorWarningMessage', door.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                doorService.deleteHomeOwerDoor({
                                    doorId: door.id,
                                    homeOwerId: vm.homeOwerId
                                }).success(function () {
                                    vm.doors.load();
                                });
                            }
                        }
                    );
                },

                addDoor: function (door) {
                    vm.doors.openSelectDoorModal({
                        communityId: vm.communityId,
                        homeOwerId: vm.homeOwerId
                    }, function (newDoor) {
                        vm.doors.load();
                    });
                },

                openSelectDoorModal: function (homeOwerDoor, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/homeOwers/selectDoorModal.cshtml',
                        controller: 'doorSystem.views.homeOwers.selectDoorModal as vm',
                        backdrop: 'static',
                        resolve: {
                            homeOwerDoor: homeOwerDoor
                        }
                    });


                    if (modalInstance) {
                        modalInstance.result.then(function (result) {
                            closeCallback && closeCallback(result);
                        });
                    }
                },

                applyKey: function (door) {
                    vm.doors.openCreateOrEditAccessKeyModal(door, function (newKey) {
                        vm.doors.load();
                    });
                },

                openCreateOrEditAccessKeyModal: function (door, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/accessKeys/createOrEditAccessKeyModal.cshtml',
                        controller: 'doorSystem.views.accessKeys.createOrEditAccessKeyModal as vm',
                        backdrop: 'static',
                        resolve: {
                            accessKey: function () {
                                return { communityId: door.communityId, homeOwerId: vm.homeOwerId, doorId: door.id };
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
                    if (!vm.permissions.manageDoors) {
                        vm.doors.gridOptions.columnDefs.shift();
                    }

                    vm.doors.load();
                }
            }

            vm.doors.init();
        }
    ]);
})();