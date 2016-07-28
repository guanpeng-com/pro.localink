(function () {
    appModule.controller('doorSystem.views.homeOwers.detailModal', [
        '$scope', '$uibModalInstance', 'homeOwer',
        function ($scope, $uibModalInstance, homeOwer) {
            var vm = this;

            vm.homeOwer = homeOwer;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.homeOwer.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.homeOwer.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
