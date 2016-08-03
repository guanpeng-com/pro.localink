(function () {
    appModule.controller('doorSystem.views.deliverys.detailModal', [
        '$scope', '$uibModalInstance', 'delivery',
        function ($scope, $uibModalInstance, delivery) {
            var vm = this;

            vm.delivery = delivery;

            vm.getDurationAsMs = function () {
                return app.localize('Xms', vm.delivery.executionDuration);
            };

            vm.getFormattedParameters = function () {
                var json = JSON.parse(vm.delivery.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
