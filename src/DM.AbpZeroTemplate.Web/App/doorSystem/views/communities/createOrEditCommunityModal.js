(function () {
    appModule.controller('doorSystem.views.communities.createOrEditCommunityModal', ['$scope', '$uibModalInstance', 'abp.services.app.community', 'community', 'appSession', 'abp.services.app.door',
    function ($scope, $uibModalInstance, communityService, community, $appSession, doorService) {
        var vm = this;
        vm.community = community;
        vm.saving = false;

        vm.doorTypes = [];

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

        function init() {
            doorService.getDoorTypes({ id: vm.community.id })
            .success(function (result) {
                vm.doorTypes = result;
            });

            if (vm.community.id) {
                communityService.getCommunity({ id: vm.community.id })
                .success(function (result) {
                    vm.community = result;
                });
            }
        }

        init();
    }]);
})();