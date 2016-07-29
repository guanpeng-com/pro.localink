(function () {
    appModule.controller('doorSystem.views.messages.createOrEditMessageModal', ['$scope', '$uibModalInstance', 'abp.services.app.message', 'message', 'appSession', 'lookupModal', 'abp.services.app.commonLookup',
    function ($scope, $uibModalInstance, messageService, message, $appSession, lookupModal, commonLookupService) {
        var vm = this;
        vm.message = message;
        vm.saving = false;

        vm.save = function () {
            if (vm.message.id) {
                messageService.updateMessage(vm.message)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                messageService.createMessage(vm.message)
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
                    vm.message.homeOwerId = selectedItem.value;
                    vm.message.homeOwerName = selectedItem.name;
                }
            });
        };

        function init() {
            if (vm.message.id) {
                messageService.getMessage({ id: vm.message.id })
                .success(function (result) {
                    vm.message = result;
                });
            }
        }

        init();
    }]);
})();
