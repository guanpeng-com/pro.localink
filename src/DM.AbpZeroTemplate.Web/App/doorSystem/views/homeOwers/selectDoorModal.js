(function () {
    appModule.controller('doorSystem.views.homeOwers.selectDoorModal', [
        '$scope', '$uibModalInstance', 'homeOwerDoor', 'abp.services.app.door', 'lookupModal', 'abp.services.app.commonLookup',
        function ($scope, $uibModalInstance, homeOwerDoor, doorService, lookupModal, commonLookupService) {
            var vm = this;
            vm.homeOwerDoor = homeOwerDoor;
            vm.saving = false;
            vm.doorTypes = [];

            if (!vm.homeOwerDoor.communityId) {
                $uibModalInstance.dismiss();
            }
            if (!vm.homeOwerDoor.homeOwerId) {
                $uibModalInstance.dismiss();
            }

            vm.save = function () {
                doorService.addHomeOwerDoor(vm.homeOwerDoor)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.selectDoor = function () {
                lookupModal.open({
                    title: app.localize('SelectADoor'),
                    serviceMethod: commonLookupService.findDoors,
                    extraFilters: {
                        communityId: vm.homeOwerDoor.communityId,
                        doorType: vm.homeOwerDoor.doorType
                    },
                    callback: function (selectedItem) {
                        vm.homeOwerDoor.doorId = selectedItem.value;
                        vm.homeOwerDoor.doorName = selectedItem.name;
                    }
                });
            };

            function init() {
                doorService.getDoorTypes({ id: vm.communityId })
                .success(function (result) {
                    vm.doorTypes = result;
                    setTimeout(function () {
                        $("#doorTypeSelectedCombox").selectpicker('refresh');
                    }, 0);
                });
            }

            init();
        }
    ]);
})();
