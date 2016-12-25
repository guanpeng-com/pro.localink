(function () {
    appModule.controller('doorSystem.views.communities.index', [
        '$scope', '$state', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.community', 'appSession',
        function ($scope, $state, $uibModal, $q, uiGridConstants, communityService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageCommunities: abp.auth.hasPermission('Pages.DoorSystem.Communities'),
                createCommunities: abp.auth.hasPermission('Pages.DoorSystem.Communities.Create'),
                editCommunities: abp.auth.hasPermission('Pages.DoorSystem.Communities.Edit'),
                deleteCommunities: abp.auth.hasPermission('Pages.DoorSystem.Communities.Delete'),
                authCommunities: abp.auth.hasPermission('Pages.DoorSystem.Communities.Auth'),
                manageCms: abp.auth.hasPermission('Pages.DoorSystem.Communities.ManageCms')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.communities = {

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
                                '<div class=\"ui-grid-cell-communities\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editCommunities" ng-click="grid.appScope.communities.editCommunity(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.authCommunities" ng-click="grid.appScope.communities.authCommunity(row.entity)">' + app.localize('CommunityAuth') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteCommunities" ng-click="grid.appScope.communities.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.communities.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.manageCms" ng-click="grid.appScope.communities.manageCms(row.entity)">' + app.localize('ManageCMS') + '</a></li>' +
                                 '      <li><a ng-if="grid.appScope.permissions.manageCms" ng-click="grid.appScope.communities.reCreateCms(row.entity)">' + app.localize('ReCreateCMS') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('CommunityName'),
                            field: 'name',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-communities\" title="{{row.entity.name}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('CommunityAddress'),
                            field: 'address',
                            minWidth: 200
                        },
                        {
                            name: app.localize('CommunityIsAuth'),
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

                            vm.communities.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.communities.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    communityService.getCommunities($.extend(vm.requestParams, {})).success(function (result) {
                        vm.communities.gridOptions.totalItems = result.totalCount;
                        vm.communities.gridOptions.data = result.items;
                    });
                },

                remove: function (community) {
                    abp.message.confirm(
                        app.localize('RemoveCommunityWarningMessage', community.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                communityService.deleteCommunity({
                                    id: community.id
                                }).success(function () {
                                    vm.communities.load();
                                });
                            }
                        }
                    );
                },

                addCommunity: function (community) {
                    vm.communities.openCreateOrEditCommunityModal({

                    }, function (newCommunity) {
                        vm.communities.load();
                    });
                },

                editCommunity: function (community) {

                    vm.communities.openCreateOrEditCommunityModal(community, function (newCommunity) {
                        vm.communities.load();
                    });
                },

                authCommunity: function (community) {
                    abp.message.confirm(
                        app.localize('AuthCommunityWarningMessage', community.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                communityService.authCommunity({ id: community.id })
                                .success(function (result) {
                                    if (result.isAuth) {
                                        abp.message.success(app.localize("AuthCommunitySuccess"));
                                        vm.communities.load();
                                    }
                                    else {
                                        abp.message.error(app.localize("AuthCommunityError"));
                                    }
                                });
                            }
                        }
                        );

                },

                manageCms: function (community) {
                    abp.services.app.session.saveCurrentAppId(community.id).done(function (result) {
                        $state.go('cms.channels');
                    });
                },

                reCreateCms: function (community) {
                    communityService.reCreateCMS({ id: community.id })
                    .success(function () {
                        abp.message.success(app.localize("ReCreateCMSSuccess"));
                    });
                },

                openCreateOrEditCommunityModal: function (community, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/communities/createOrEditCommunityModal.cshtml',
                        controller: 'doorSystem.views.communities.createOrEditCommunityModal as vm',
                        backdrop: 'static',
                        resolve: {
                            community: function () {
                                return community;
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
                    if (!vm.permissions.manageCommunities) {
                        vm.communities.gridOptions.columnDefs.shift();
                    }

                    vm.communities.load();
                },

                showDetails: function (community) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/communities/detailModal.cshtml',
                        controller: 'doorSystem.views.communities.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            community: function () {
                                return community;
                            }
                        }
                    });
                }
            }

            vm.communities.init();
        }
    ]);
})();