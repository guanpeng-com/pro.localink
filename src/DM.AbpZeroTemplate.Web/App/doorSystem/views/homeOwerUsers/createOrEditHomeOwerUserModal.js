(function () {
    appModule.controller('doorSystem.views.homeOwerUsers.createOrEditHomeOwerUserModal', ['$scope', '$uibModalInstance', 'abp.services.app.homeOwerUser', 'homeOwerUser', 'appSession',
    function ($scope, $uibModalInstance, homeOwerUserService, homeOwerUser, $appSession) {
        var vm = this;
        vm.homeOwerUser = homeOwerUser;
        vm.saving = false;

        vm.save = function () {
            if (vm.homeOwerUser.id) {
                homeOwerUserService.updateHomeOwerUser(vm.homeOwerUser)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                homeOwerUserService.createHomeOwerUser(vm.homeOwerUser)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };

        function init() {
            if (vm.homeOwerUser.id) {
                homeOwerUserService.getHomeOwerUser({ id: vm.homeOwerUser.id })
                .success(function (result) {
                    vm.homeOwerUser = result;
                });
            }
        }

        init();
    }]);
})();
