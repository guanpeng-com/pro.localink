(function () {
    appModule.controller('tenant.views.settings.index', [
        '$scope', 'abp.services.app.tenantSettings', 'abp.services.app.area',
        function ($scope, tenantSettingsService, areaService) {
            var vm = this;

            $scope.$on('$viewContentLoaded', function () {
                App.initAjax();
            });

            vm.isMultiTenancyEnabled = abp.multiTenancy.isEnabled;
            vm.loading = false;
            vm.settings = null;
            vm.areas = [];

            vm.getSettings = function () {
                vm.loading = true;
                tenantSettingsService.getAllSettings()
                    .success(function (result) {
                        vm.settings = result;
                    }).finally(function () {
                        vm.loading = false;
                    });
            };

            vm.getAreas = function () {
                areaService.getAreasLevel1({}).success(function (result) {
                    vm.areas = result;
                });
            };

            vm.saveAll = function () {
                tenantSettingsService.updateAllSettings(
                    vm.settings
                ).success(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                });
            };

            vm.getSettings();


            vm.getAreas();
        }
    ]);
})();