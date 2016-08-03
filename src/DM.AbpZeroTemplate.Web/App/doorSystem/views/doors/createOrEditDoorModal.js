(function () {
    appModule.controller('doorSystem.views.doors.createOrEditDoorModal', ['$scope', '$uibModalInstance', 'abp.services.app.door', 'door', 'appSession', 'abp.services.app.community',
    function ($scope, $uibModalInstance, doorService, door, $appSession, communityService) {
        var vm = this;
        vm.door = door;
        vm.saving = false;
        vm.doorTypes = [];

        vm.communityies = [];

        vm.save = function () {
            if (vm.door.id) {
                doorService.updateDoor(vm.door)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                doorService.createDoor(vm.door)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };

        vm.loadDoorType = function () {
            doorService.getCommunityDoorTypes({ id: vm.door.communityId })
            .success(function (result) {
            vm.doorTypes = result;
            setTimeout(function () {
                $("#doorTypeSelectedCombox").selectpicker('refresh');
            }, 0);
            });
        };

        function init() {
            communityService.getUserCommunities()
            .success(function (result) {
                vm.communityies = result;
                setTimeout(function () {
                    $('#communitySelectedCombox').selectpicker('refresh');
                }, 0);
            });

            if (vm.door.id) {
                doorService.getDoor({ id: vm.door.id })
                .success(function (result) {
                    vm.door = result;
                });

                doorService.getCommunityDoorTypes({ id: vm.door.communityId })
                    .success(function (result) {
                    vm.doorTypes = result;
                    setTimeout(function () {
                        $("#doorTypeSelectedCombox").selectpicker('refresh');
                    }, 0);
                });
            }
        }

        init();
    }]);
})();
