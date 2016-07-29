(function () {
    appModule.controller('doorSystem.views.messages.detailModal', [
        '$scope', '$uibModalInstance', 'message',
        function ($scope, $uibModalInstance, message) {
            var vm = this;

            vm.message = message;

            vm.getDurationAsMs = function () {
                return app.localize('Xms', vm.message.executionDuration);
            };

            vm.getFormattedParameters = function () {
                var json = JSON.parse(vm.message.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
