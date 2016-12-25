(function () {
    appModule.controller('doorSystem.views.communities.createOrEditCommunityModal', ['$scope', '$uibModalInstance', 'abp.services.app.community', 'community', 'appSession', 'abp.services.app.door', 'selectAreaModal', 'gmapModal', 'fileUploadModal',
    function ($scope, $uibModalInstance, communityService, community, $appSession, doorService, selectAreaModal, gmapModal, fileUploadModal) {
        var vm = this;
        vm.community = community;
        vm.saving = false;

        vm.doorTypes = [];

        vm.fileUpload = function () {
            fileUploadModal.open({
                communityId: vm.community.id,
                homeOwerId: 0,
                callback: function (result) {

                    vm.community.images = result;
                }
            });
        };

        vm.save = function () {

            var selectedTypes = _.map(
                _.where(vm.doorTypes, { isSelected: true }),
                function (type) {
                    return type.key;
                });

            vm.community.doorTypes = selectedTypes;

            if (vm.community.id) {
                communityService.updateCommunity(vm.community)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                communityService.createCommunity(vm.community)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };

        vm.selectArea = function () {
            selectAreaModal.open({
                callback: function (text) {
                    vm.community.address = text;
                }
            });
        };

        vm.selectMarker = function () {
            gmapModal.open({
                filterText: vm.community.name,
                address: vm.community.address,
                callback: function (marker) {
                    vm.community.latLng = marker.latitude + "," + marker.longitude;
                }
            });
        };

        function init() {
            doorService.getDoorTypes({ id: vm.community.id })
            .success(function (result) {
                vm.doorTypes = result;
            });

            if (vm.community.id) {
                communityService.getCommunity({ id: vm.community.id })
                .success(function (result) {
                    vm.community = result;
                    vm.community.latLng = vm.community.lat + "," + vm.community.lng;
                });
            }
        }

        init();
    }]);
})();