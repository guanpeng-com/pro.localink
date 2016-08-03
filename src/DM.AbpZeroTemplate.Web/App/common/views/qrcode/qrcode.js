(function () {
    appModule.controller('common.views.qrcode', [
        '$scope', '$uibModalInstance', 'qrcode',
        function ($scope, $uibModalInstance, qrcode) {
            var vm = this;

            vm.qrcode = {
                data: '',
                size: 274,
                version: 5,
                error_correction_level: 'M'
            };

            vm.qrcode = $.extend(vm.qrcode, qrcode);

            vm.close = function () {
                $uibModalInstance.dismiss();
            };
        }
    ]);
})();
