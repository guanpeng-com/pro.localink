(function () {
    appModule.controller('doorSystem.views.homeOwers.createOrEditHomeOwerModal', ['$scope', '$uibModalInstance', 'abp.services.app.homeOwer', 'homeOwer', 'appSession', 'abp.services.app.community',
    function ($scope, $uibModalInstance, homeOwerService, homeOwer, $appSession, communityService) {
        var vm = this;
        vm.homeOwer = homeOwer;
        vm.saving = false;
        vm.communityies = [];

        vm.save = function () {
            if (vm.homeOwer.id) {
                homeOwerService.updateHomeOwer(vm.homeOwer)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                homeOwerService.createHomeOwer(vm.homeOwer)
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
            communityService.getUserCommunities()
            .success(function (result) {
                vm.communityies = result;
                setTimeout(function () {
                    $('#communitySelectedCombox').selectpicker('refresh');
                    $('#genderSelectedCombox').selectpicker('refresh');
                }, 0);
            });

            if (vm.homeOwer.id) {
                homeOwerService.getHomeOwer({ id: vm.homeOwer.id })
                .success(function (result) {
                    vm.homeOwer = result;
                });
            }
        }

        init();
    }]);
})();
