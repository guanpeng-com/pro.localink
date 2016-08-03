(function () {
    appModule.controller('doorSystem.views.deliverys.createOrEditDeliveryModal', ['$scope', '$uibModalInstance', 'abp.services.app.delivery', 'delivery', 'appSession', 'lookupModal', 'abp.services.app.commonLookup',
    function ($scope, $uibModalInstance, deliveryService, delivery, $appSession, lookupModal, commonLookupService) {
        var vm = this;
        vm.delivery = delivery;
        vm.saving = false;

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
                },
                callback: function (selectedItem) {
                    vm.delivery.homeOwerId = selectedItem.value;
                    vm.delivery.homeOwerName = selectedItem.name;
                }
            });
        };

        function init() {
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
