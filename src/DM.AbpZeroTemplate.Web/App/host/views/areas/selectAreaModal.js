(function () {
    appModule.controller('host.views.areas.selectAreaModal', [
        '$scope', '$uibModalInstance', 'selectOptions', 'abp.services.app.area',
        function ($scope, $uibModalInstance, selectOptions, areaServices) {
            var vm = this;
            vm.loading = false;
            vm.areas = [];
            vm.area1 = null;
            vm.area2 = null;
            vm.area3 = null;

            //Options
            vm.options = angular.extend({
                title: app.localize('SelectArea'),
                callback: function (text) {
                    //This method is used to get selected item
                }
            }, selectOptions);


            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.save = function () {
                var text = "";
                if (vm.area1) {
                    text += vm.area1.name;
                }
                if (vm.area2) {
                    text += vm.area2.name;
                }
                if (vm.area3) {
                    text += vm.area3.name;
                }
                vm.options.callback(text);
                $uibModalInstance.close(text);
            };

            function init() {
                areaServices.getAllAreas()
                .success(function (result) {
                    vm.areas = result;
                });
            }

            init();
        }
    ]);

    //lookupModal service
    appModule.factory('selectAreaModal', [
        '$uibModal',
        function ($uibModal) {
            function open(selectOptions) {
                $uibModal.open({
                    templateUrl: '~/App/host/views/areas/selectAreaModal.cshtml',
                    controller: 'host.views.areas.selectAreaModal as vm',
                    backdrop: 'static',
                    resolve: {
                        selectOptions: function () {
                            return selectOptions;
                        }
                    }
                });
            }

            return {
                open: open
            };
        }
    ]);
})();