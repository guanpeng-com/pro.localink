(function () {
    appModule.controller('doorSystem.views.keyHoldings.detailModal', [
        '$scope', '$uibModalInstance', 'keyHolding',
        function ($scope, $uibModalInstance, keyHolding) {
            var vm = this;

            vm.keyHolding = keyHolding;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.keyHolding.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.keyHolding.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
