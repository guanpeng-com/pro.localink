(function () {
    appModule.controller('doorSystem.views.reports.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.report', 'appSession',
        function ($scope, $uibModal, $q, uiGridConstants, reportService, $appSession) {
            var vm = this;

            vm.appSession = $appSession;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.permissions = {
                manageReports: abp.auth.hasPermission('Pages.DoorSystem.Reports'),
                createReports: abp.auth.hasPermission('Pages.DoorSystem.Reports.Create'),
                editReports: abp.auth.hasPermission('Pages.DoorSystem.Reports.Edit'),
                deleteReports: abp.auth.hasPermission('Pages.DoorSystem.Reports.Delete')
            };

            vm.requestParams = {
                id: 0,
                skipCount: 0,
                maxResultCount: app.consts.grid.defaultPageSize,
                sorting: null
            };

            vm.reports = {

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
                                '<div class=\"ui-grid-cell-reports\">' +
                                '  <div class="btn-group dropdown" uib-dropdown="">' +
                                '    <button class="btn btn-xs btn-primary blue" uib-dropdown-toggle="" aria-haspopup="true" aria-expanded="false"><i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span></button>' +
                                '    <ul uib-dropdown-menu>' +
                                '      <li><a ng-if="grid.appScope.permissions.editReports" ng-click="grid.appScope.reports.editReport(row.entity)">' + app.localize('Edit') + '</a></li>' +
                                '      <li><a ng-if="grid.appScope.permissions.deleteReports" ng-click="grid.appScope.reports.remove(row.entity)" >' + app.localize('Delete') + '</a></li>' +
                                '      <li><a ng-click="grid.appScope.reports.showDetails(row.entity)">' + app.localize('Detail') + '</a></li>' +
                                '    </ul>' +
                                '  </div>' +
                                '</div>'
                        },
                        {
                            name: app.localize('ReportTitle'),
                            field: 'title',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-reports\" title="{{row.entity.title}}"> {{COL_FIELD CUSTOM_FILTERS}} &nbsp;</div>',
                            minWidth: 100
                        },
                        {
                            name: app.localize('HomeOwerName'),
                            field: 'homeOwerName',
                            minWidth: 200
                        },
                        {
                            name: app.localize('ReportStatus'),
                            field: 'status',
                            cellTemplate:
                                '<div class=\"ui-grid-cell-contents\">' +
                                '  <span class="label label-default">{{grid.appScope.reports.localize(row.entity.status)}}</span>' +
                                '</div>'
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

                            vm.reports.load();
                        });
                        gridApi.pagination.on.paginationChanged($scope, function (pageNumber, pageSize) {
                            vm.requestParams.skipCount = (pageNumber - 1) * pageSize;
                            vm.requestParams.maxResultCount = pageSize;

                            vm.reports.load();
                        });
                    },
                    data: []
                },

                load: function () {

                    reportService.getReports($.extend(vm.requestParams, {})).success(function (result) {
                        vm.reports.gridOptions.totalItems = result.totalCount;
                        vm.reports.gridOptions.data = result.items;
                    });
                },

                localize: function (name) {
                    return app.localize(name);
                },

                remove: function (report) {
                    abp.message.confirm(
                        app.localize('RemoveReportWarningMessage', report.name),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                reportService.deleteReport({
                                    id: report.id
                                }).success(function () {
                                    vm.reports.load();
                                });
                            }
                        }
                    );
                },

                addReport: function (report) {
                    vm.reports.openCreateOrEditReportModal({

                    }, function (newReport) {
                        vm.reports.load();
                    });
                },

                editReport: function (report) {

                    vm.reports.openCreateOrEditReportModal(report, function (newReport) {
                        vm.reports.load();
                    });
                },

                openCreateOrEditReportModal: function (report, closeCallback) {
                    var modalInstance = null;

                    modalInstance = $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/reports/createOrEditReportModal.cshtml',
                        controller: 'doorSystem.views.reports.createOrEditReportModal as vm',
                        backdrop: 'static',
                        resolve: {
                            report: function () {
                                return report;
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
                    if (!vm.permissions.manageReports) {
                        vm.reports.gridOptions.columnDefs.shift();
                    }

                    vm.reports.load();
                },

                showDetails: function (report) {
                    $uibModal.open({
                        templateUrl: '~/App/doorSystem/views/reports/detailModal.cshtml',
                        controller: 'doorSystem.views.reports.detailModal as vm',
                        backdrop: 'static',
                        resolve: {
                            report: function () {
                                return report;
                            }
                        }
                    });
                }
            }

            vm.reports.init();
        }
    ]);
})();
