(function () {
    appModule.controller('doorSystem.views.deliverys.createOrEditDeliveryModal', ['$scope', '$uibModalInstance', 'abp.services.app.delivery', 'delivery', 'appSession', 'lookupModal', 'abp.services.app.commonLookup', 'abp.services.app.community',
    function ($scope, $uibModalInstance, deliveryService, delivery, $appSession, lookupModal, commonLookupService, communityService) {
        var vm = this;
        vm.delivery = delivery;
        vm.saving = false;
        vm.communityies = [];

        vm.save = function () {
            if (vm.delivery.id) {
                deliveryService.updateDelivery(vm.delivery)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                deliveryService.createDelivery(vm.delivery)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };

        vm.selectHomeOwer = function () {
            lookupModal.open({
                title: app.localize('SelectAHomeOwer'),
                serviceMethod: commonLookupService.findHomeOwers,
                extraFilters: {
                    communityId: vm.delivery.communityId
                },
                callback: function (selectedItem) {
                    vm.delivery.homeOwerId = selectedItem.value;
                    vm.delivery.homeOwerName = selectedItem.name;
                }
            });
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

            if (vm.delivery.id) {
                deliveryService.getDelivery({ id: vm.delivery.id })
                .success(function (result) {
                    vm.delivery = result;
                });
            }
        }

        init();
    }]);
})();
