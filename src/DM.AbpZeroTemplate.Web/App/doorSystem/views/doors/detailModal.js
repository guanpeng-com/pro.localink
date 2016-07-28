(function () {
    appModule.controller('doorSystem.views.doors.detailModal', [
        '$scope', '$uibModalInstance', 'door',
        function ($scope, $uibModalInstance, door) {
            var vm = this;

            vm.door = door;

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
