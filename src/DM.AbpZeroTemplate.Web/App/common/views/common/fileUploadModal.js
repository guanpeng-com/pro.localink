(function () {
    appModule.controller('common.views.common.fileUploadModal', [
        '$scope', '$uibModalInstance', 'uiGridConstants', 'FileUploader', 'fileUploadOptions', 'abp.services.app.commonFileUpload',
    function ($scope, $uibModalInstance, uiGridConstants, fileUploader, fileUploadOptions, commonFileUploadService) {
        var vm = this;

        vm.loading = false;

        vm.files = [];

        //Options
        vm.options = angular.extend({
            url: abp.appPath + 'api/services/app/commonFileUpload/FileUpload', //Required
            title: app.localize('FileUpload'),
            allowExtension: '',
            maxFileLength: 2 * 5242880,
            fileUploadType: 'Common',
            homeOwerId: null,
            communityId: null,
            callback: function (selectedItem) {
                //This method is used to get selected item
            }
        }, fileUploadOptions);

        //Check required parameters
        if (!vm.options.url) {
            $uibModalInstance.dismiss();
            return;
        }

        vm.cancel = function () {
            $uibModalInstance.dismiss();
        };

        var uploader = vm.uploader = new fileUploader({
            url: vm.options.url
        });

        // FILTERS
        uploader.filters.push({
            name: 'customFilter',
            fn: function (item /*{File|FileLikeObject}*/, options) {
                return filter(item, options, vm.options.allowExtension, vm.options.maxFileLength);
            }
        });

        //formData
        uploader.formData.push({
            data: JSON.stringify({
                maxFileLength: vm.options.maxFileLength,
                allowFileExtension: vm.options.allowExtension,
                fileUploadType: vm.options.fileUploadType,
                homeOwerId: vm.options.homeOwerId,
                communityId: vm.options.communityId
            })
        });


        // CALLBACKS

        uploader.onWhenAddingFileFailed = function (item /*{File|FileLikeObject}*/, filter, options) {
            //console.info('onWhenAddingFileFailed', item, filter, options);
        };
        uploader.onAfterAddingFile = function (fileItem) {
            //console.info('onAfterAddingFile', fileItem);
        };
        uploader.onAfterAddingAll = function (addedFileItems) {
            //console.info('onAfterAddingAll', addedFileItems);
        };
        uploader.onBeforeUploadItem = function (item) {
            //console.info('onBeforeUploadItem', item);
        };
        uploader.onProgressItem = function (fileItem, progress) {
            //console.info('onProgressItem', fileItem, progress);
        };
        uploader.onProgressAll = function (progress) {
            //console.info('onProgressAll', progress);
        };
        uploader.onSuccessItem = function (fileItem, response, status, headers) {
            vm.files.push(response);
        };
        uploader.onErrorItem = function (fileItem, response, status, headers) {
            //console.info('onErrorItem', fileItem, response, status, headers);
        };
        uploader.onCancelItem = function (fileItem, response, status, headers) {
            //console.info('onCancelItem', fileItem, response, status, headers);
        };
        uploader.onCompleteItem = function (fileItem, response, status, headers) {
            console.info('onCompleteItem', fileItem, response, status, headers);
        };
        uploader.onCompleteAll = function () {
            if (vm.files.length > 0){
                vm.options.callback && vm.options.callback(vm.files);
                $uibModalInstance.close(vm.files);
                return;
            }
        };


        function filter(item, options, fileTypes, fileSize) {

            //File type check
            var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
            if (type.indexOf(type) === -1) {
                abp.message.warn(app.localize('FIle_Upload_Warn_ExtensionLimit', fileTypes));
                return false;
            }

            if (fileSize) {
                //File size check
                if (item.size > fileSize) //1MB
                {
                    abp.message.warn(app.localize('FIle_Upload_Warn_SizeLimit', fileSize / 5242880));
                    return false;
                }
            }

            return true;
        }
    }
    ]);

    //fileUploadModal service
    appModule.factory('fileUploadModal', [
        '$uibModal',
        function ($uibModal) {
            function open(fileUploadOptions) {
                $uibModal.open({
                    templateUrl: '~/App/common/views/common/fileUploadModal.cshtml',
                    controller: 'common.views.common.fileUploadModal as vm',
                    backdrop: 'static',
                    size: 'lg',
                    resolve: {
                        fileUploadOptions: function () {
                            return fileUploadOptions;
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