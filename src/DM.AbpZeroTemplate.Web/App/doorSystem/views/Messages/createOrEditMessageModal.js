(function () {
    appModule.controller('doorSystem.views.messages.createOrEditMessageModal', ['$scope', '$uibModalInstance', 'abp.services.app.message', 'message', 'appSession', 'lookupModal', 'abp.services.app.commonLookup', 'abp.services.app.community',
    function ($scope, $uibModalInstance, messageService, message, $appSession, lookupModal, commonLookupService, communityService) {
        var vm = this;
        vm.message = message;
        vm.saving = false;
        vm.communityies = [];

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
                    communityId:vm.message.communityId
                },
                callback: function (selectedItem) {
                    vm.message.homeOwerId = selectedItem.value;
                    vm.message.homeOwerName = selectedItem.name;
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
