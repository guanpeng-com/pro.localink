(function () {
    appModule.controller('doorSystem.views.reports.detailModal', [
        '$scope', '$uibModalInstance', 'report',
        function ($scope, $uibModalInstance, report) {
            var vm = this;

            vm.report = report;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.report.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.report.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
