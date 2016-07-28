(function () {
    appModule.controller('doorSystem.views.homeOwerUsers.detailModal', [
        '$scope', '$uibModalInstance', 'homeOwerUser',
        function ($scope, $uibModalInstance, homeOwerUser) {
            var vm = this;

            vm.homeOwerUser = homeOwerUser;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.homeOwerUser.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.homeOwerUser.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
