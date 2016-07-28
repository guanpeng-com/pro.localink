(function () {
    appModule.controller('doorSystem.views.accessKeys.detailModal', [
        '$scope', '$uibModalInstance', 'accessKey',
        function ($scope, $uibModalInstance, accessKey) {
            var vm = this;

            vm.accessKey = accessKey;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.accessKey.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.accessKey.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
