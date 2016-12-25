(function () {
    appModule.controller('doorSystem.views.keyHoldings.createOrEditKeyHoldingModal', ['$scope', '$uibModalInstance', 'abp.services.app.keyHolding', 'keyHolding', 'appSession', 'abp.services.app.community', 'abp.services.app.commonLookup', 'lookupModal',
    function ($scope, $uibModalInstance, keyHoldingService, keyHolding, $appSession, communityService, commonLookupService, lookupModal) {
        var vm = this;
        vm.keyHolding = keyHolding;
        vm.saving = false;
        vm.communityies = [];

        vm.save = function () {
            if (vm.keyHolding.id) {
                keyHoldingService.updateKeyHolding(vm.keyHolding)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                keyHoldingService.createKeyHolding(vm.keyHolding)
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
                    communityId: vm.keyHolding.communityId
                },
                callback: function (selectedItem) {
                    vm.keyHolding.homeOwerId = selectedItem.value;
                    vm.keyHolding.homeOwerName = selectedItem.name;
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

            if (vm.keyHolding.id) {
                keyHoldingService.getKeyHolding({ id: vm.keyHolding.id })
                .success(function (result) {
                    
                    result.visiteStartTime = formatTime(result.visiteStartTime);
                    result.visiteEndTime = formatTime(result.visiteEndTime);
                    vm.keyHolding = result;
                });
            }
        }

        function formatTime(time) {
            return time.replace("T", " ").split("+")[0];
        }

        init();
    }]);
})();
