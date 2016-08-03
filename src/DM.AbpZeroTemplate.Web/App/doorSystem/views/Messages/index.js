(function () {
    appModule.controller('doorSystem.views.messages.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.message', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, messageService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageMessages: abp.auth.hasPermission('Pages.DoorSystem.Messages'),
                createMessages: abp.auth.hasPermission('Pages.DoorSystem.Messages.Create'),
                editMessages: abp.auth.hasPermission('Pages.DoorSystem.Messages.Edit'),
                deleteMessages: abp.auth.hasPermission('Pages.DoorSystem.Messages.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null,
                isPublic: null,
            };

            vm.messages = {

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
                                '<div class=\"ui-grid-cell-messages\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editMessages" ng-click="grid.appScope.messages.editMessage(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteMessages" ng-click="grid.appScope.messages.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.messages.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('MessageTitle'),
                            field: 'title',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-messages\" title="{{row.entity.title}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'homeOwerName',
                            minWidth: 200
                        },
                        {
                            name: app.localize('CreationTime'),
                            field: 'creationTime',
                            cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm:ss\'',
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

                            vm.messages.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.messages.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    messageService.getMessages($.extend(vm.requestParams, {})).success(function (result) {
                        vm.messages.gridOptions.totalItems = result.totalCount;
                        vm.messages.gridOptions.data = result.items;
                    });
                },

                remove: function (message) {
                    abp.message.confirm(
                        app.localize('RemoveMessageWarningMessage', message.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                messageService.deleteMessage({
                                    id: message.id
                                }).success(function () {
                                    vm.messages.load();
                                });
                            }
                        }
                    );
                },

                addMessage: function (message) {
                    vm.messages.openCreateOrEditMessageModal({

                    }, function (newMessage) {
                        vm.messages.load();
                    });
                },

                editMessage: function (message) {

                    vm.messages.openCreateOrEditMessageModal(message, function (newMessage) {
                        vm.messages.load();
                    });
                },

                openCreateOrEditMessageModal: function (message, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/messages/createOrEditMessageModal.cshtml',
                        controller: 'doorSystem.views.messages.createOrEditMessageModal as vm',
                        backdrop: 'static',
                        resolve: {
                            message: function () {
                                return message;
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
                    if (!vm.permissions.manageMessages) {
                        vm.messages.gridOptions.columnDefs.shift();
                    }

                    vm.messages.load();
                },

                showDetails: function (message) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/messages/detailModal.cshtml',
                        controller: 'doorSystem.views.messages.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            message: function () {
                                return message;
                            }
                        }
                    });
                }
            }

            vm.messages.init();
        }
    ]);
})();
