(function () {
    appModule.controller('host.views.areas.createOrEditAreaModal', ['$scope', '$uibModalInstance', 'abp.services.app.area', 'area', 'FileUploader', 'appSession',
    function ($scope, $uibModalInstance, areaService, area, fileUploader, $appSession) {
        var vm = this;
        vm.saving = false;
        vm.area = area;

        vm.save = function () {
            if (vm.area.id) {
                areaService.updateArea(vm.area)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
            else {
                areaService.createArea(vm.area)
                .success(function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close(result);
                });
            }
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };
    }]);
})();