(function () {
    appModule.controller('doorSystem.views.accessKeys.createOrEditAccessKeyModal', ['$scope', '$uibModalInstance', 'abp.services.app.accessKey', 'accessKey', 'appSession', 'abp.services.app.door',
    function ($scope, $uibModalInstance, accessKeyService, accessKey, $appSession, doorService) {
        var vm = this;
        vm.accessKey = accessKey;
        vm.saving = false;

        vm.doors = [];


        vm.save = function () {
            if (vm.accessKey.id) {
                accessKeyService.updateAccessKey(vm.accessKey)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                accessKeyService.createAccessKey(vm.accessKey)
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
            doorService.getAllDoors({ communityId: vm.accessKey.communityId, homeOwerId: vm.accessKey.homeOwerId })
            .success(function (result) {
                vm.doors = result;
                setTimeout(function () {
                    $("#doorSelectedCombox").selectpicker('refresh');
                }, 0);
            });


            if (vm.accessKey.id) {
                accessKeyService.getAccessKey({ id: vm.accessKey.id })
                .success(function (result) {
                    vm.accessKey = result;
                });
            }
        }

        init();
    }]);
})();
