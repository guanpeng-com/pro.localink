(function () {
    appModule.controller('doorSystem.views.reports.createOrEditReportModal', ['$scope', '$uibModalInstance', 'abp.services.app.report', 'report', 'appSession', 'lookupModal', 'abp.services.app.commonLookup', 'fileUploadModal', 'abp.services.app.community',
    function ($scope, $uibModalInstance, reportService, report, $appSession, lookupModal, commonLookupService, fileUploadModal, communityService) {
        var vm = this;
        vm.report = report;
        vm.saving = false;
        vm.reportStatus = [];
        vm.communityies = [];

        vm.save = function () {
            if (vm.report.id) {
                reportService.updateReport(vm.report)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                reportService.createReport(vm.report)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };


        vm.selectHomeOwer = function () {
            lookupModal.open({
                title: app.localize('SelectAHomeOwer'),
                serviceMethod: commonLookupService.findHomeOwers,
                extraFilters: {
                    communityId: vm.report.communityId
                },
                callback: function (selectedItem) {
                    vm.report.homeOwerId = selectedItem.value;
                    vm.report.homeOwerName = selectedItem.name;
                }
            });
        };

        vm.fileUpload = function () {
            fileUploadModal.open({
                homeOwerId: vm.report.homeOwerId,
                callback: function (result) {
                    vm.report.fileArray = result;
                    vm.report.files = result.join(";");
                }
            });
        };

        function init() {
            communityService.getUserCommunities()
.success(function (result) {
    vm.communityies = result;
    setTimeout(function () {
        $('#communitySelectedCombox').selectpicker('refresh');
        $('#genderSelectedCombox').selectpicker('refresh');
    }, 0);
});
            if (vm.report.id) {
                reportService.getAllReportStatus()
                .success(function (result) {
                    vm.reportStatus = result;
                });


                reportService.getReport({ id: vm.report.id })
                .success(function (result) {
                    vm.report = result;
                });
            }
        }

        init();
    }]);
})();
