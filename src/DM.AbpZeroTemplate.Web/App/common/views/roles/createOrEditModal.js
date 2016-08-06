(function () {
    appModule.controller('common.views.roles.createOrEditModal', [
        '$scope', '$uibModalInstance', 'abp.services.app.role', 'roleId',
        function ($scope, $uibModalInstance, roleService, roleId) {
            var vm = this;

            vm.saving = false;
            vm.role = null;
            vm.permissionEditData = null;
            vm.communities = null;
            vm.communityIds = [];

            vm.save = function () {
                vm.saving = true;

                var selectCommunityIds = _.map(
                    _.where(vm.communities, { isSelected: true }),
                    function (community) {
                        return community.id;
                    });

                vm.communityIds = selectCommunityIds;

                roleService.createOrUpdateRole({
                    role: vm.role,
                    grantedPermissionNames: vm.permissionEditData.grantedPermissionNames,
                    communityIds: vm.communityIds
                }).success(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    $uibModalInstance.close();
                }).finally(function () {
                    vm.saving = false;
                });
            };

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            function init() {
                roleService.getRoleForEdit({
                    id: roleId
                }).success(function (result) {
                    vm.role = result.role;
                    vm.permissionEditData = {
                        permissions: result.permissions,
                        grantedPermissionNames: result.grantedPermissionNames
                    };
                    vm.communities = result.communities;

                    for (var i = 0; i < vm.communities.length; i++) {
                        if (vm.role.communityIdArray.indexOf(vm.communities[i].id) >= 0) {
                            vm.communities[i].isSelected = true;
                        }
                    }

                });
            }

            init();
        }
    ]);
})();