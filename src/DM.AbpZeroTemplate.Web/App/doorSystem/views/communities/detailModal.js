(function () {
    appModule.controller('doorSystem.views.communities.detailModal', [
        '$scope', '$uibModalInstance', 'community',
        function ($scope, $uibModalInstance, community) {
            var vm = this;

            vm.community = community;

            vm.getDurationAsMs = function() {
                return app.localize('Xms', vm.community.executionDuration);
            };

            vm.getFormattedParameters = function() {
                var json = JSON.parse(vm.community.parameters);
                return JSON.stringify(json, null, 4);
            }

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();