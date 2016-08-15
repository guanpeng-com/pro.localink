(function () {
    appModule.controller('host.views.areas.index', [
        '$scope', '$uibModal', '$q', 'uiGridConstants', 'abp.services.app.area', 'abp.services.app.commonLookup', 'lookupModal', 'appSession',
    function ($scope, $uiModal, $q, uiGridConstants, areaService, commonLookupService, lookupModal, $appSession) {
        var vm = this;

        $scope.$on('$viewContentLoaded', function () {
            App.initAjax();
        });

        vm.permissions = {
            manageAreaTree: abp.auth.hasPermission('Pages.Areas'),
            createAreaTree: abp.auth.hasPermission('Pages.Areas.Create'),
            editAreaTree: abp.auth.hasPermission('Pages.Areas.Edit'),
            deleteAreaTree: abp.auth.hasPermission('Pages.Areas.Delete')
        };

        vm.areaTree = {
            $tree: null,
            areaCount: 0,

            setAreaCount: function (areaCount) {
                $scope.safeApply(function () {
                    vm.areaTree.areaCount = areaCount;
                });
            },

            refreshAreaCount: function () {
                vm.areaTree.setAreaCount(vm.areaTree.$tree.jstree('get_json').length)
            },

            selectedArea: {
                id: null,
                name: null,

                set: function (ouInTree) {
                    if (!ouInTree) {
                        vm.areaTree.selectedArea.id = null;
                        vm.areaTree.selectedArea.name = null;
                    } else {
                        vm.areaTree.selectedArea.id = ouInTree.id;
                        vm.areaTree.selectedArea.name = ouInTree.name;
                    }
                    // vm.members.load();
                }
            },

            contextMenu: function (node) {
                var items = {
                    editArea: {
                        label: app.localize('Edit'),
                        _disabled: !vm.permissions.editAreaTree,
                        action: function (data) {
                            var instance = $.jstree.reference(data.reference);

                            vm.areaTree.openCreateOrEditAreaModal(
                                {
                                    id: node.id,
                                    parentId: node.original.parent,
                                    name: node.original.name
                                },
                                function (updatedArea) {
                                    node.original.name = updatedArea.name;
                                    instance.rename_node(node, vm.areaTree.generateTextOnTree(updatedArea));
                                });
                        }
                    }
                    ,
                    addSubArea: {
                        label: app.localize('Add'),
                        _disabled: !vm.permissions.createAreaTree,
                        action: function () {
                            vm.areaTree.addArea(node.id);
                        }
                    }
                    ,
                    'delete': {
                        label: app.localize('Delete'),
                        _disabled: !vm.permissions.deleteAreaTree,
                        action: function (data) {
                            var instance = $.jstree.reference(data.reference);

                            abp.message.confirm(
                                app.localize('AreaDeleteWarningMessage',
                                node.original.name),
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        areaService.deleteArea({
                                            id: node.id
                                        }).success(function () {
                                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                                            instance.delete_node(node);
                                            vm.areaTree.refreshAreaCount();
                                        });
                                    }
                                });
                        }
                    }
                }

                return items;
            },

            addArea: function (parentId) {
                var instance = $.jstree.reference(vm.areaTree.$tree);

                vm.areaTree.openCreateOrEditAreaModal({
                    parentId: parentId
                }, function (newArea) {
                    instance.create_node(
                        parentId ? instance.get_node(parentId) : '#',
                        {
                            id: newArea.id,
                            parent: newArea.parentId ? newArea.parentId : '#',
                            name: newArea.name,
                            text: vm.areaTree.generateTextOnTree(newArea),
                            state: {
                                opened: true
                            }
                        });
                    vm.areaTree.refreshAreaCount();
                });
            },

            openCreateOrEditAreaModal: function (area, closeCallback) {
                var modalInstance = $uiModal.open({
                    templateUrl: '~/App/host/views/areas/createOrEditAreaModal.cshtml',
                    controller: 'host.views.areas.createOrEditAreaModal as vm',
                    backdrop: 'static',
                    resolve: {
                        area: function () {
                            return area;
                        }
                    }
                });

                modalInstance.result.then(function (result) {
                    closeCallback && closeCallback(result);
                });
            },

            generateTextOnTree: function (area) {
                return '<span title="' + area.name + '" class="ou-text" data-ou-id="' + area.id + '">' + area.name + '<i class="fa fa-caret-down text-muted"></i></span>';
            },

            getTreeDataFromServer: function (callback) {
                areaService.getAreas({ }).success(function (result) {
                    var treeData = _.map(result.items, function (item) {
                        return {
                            id: item.id,
                            parent: item.parentId ? item.parentId : '#',
                            name: item.name,
                            text: vm.areaTree.generateTextOnTree(item),
                            state: {
                                opened: true
                            }
                        };
                    });

                    callback && callback(treeData);
                });
            },

            init: function () {
                vm.areaTree.getTreeDataFromServer(function (treeData) {
                    vm.areaTree.setAreaCount(treeData.length);
                    vm.areaTree.$tree = $('#AreaEditTree');

                    var jsTreePlugins = [
                            'types',
                            'contextmenu',
                            'wholerow',
                            'sort'
                    ];

                    if (vm.permissions.manageAreaTree) {
                        jsTreePlugins.push('dnd');
                    }

                    vm.areaTree.$tree
                    .on('changed.jstree', function (e, data) {
                        $scope.safeApply(function () {
                            if (data.selected.length != 1) {
                                vm.areaTree.selectedArea.set(null);
                            }
                            else {
                                var selectedArea = data.instance.get_node(data.selected[0]);
                                vm.areaTree.selectedArea.set(selectedArea);
                            }
                        })
                    })
                        .jstree({
                            'core': {
                                data: treeData,
                                multilpe: false,
                                check_callback: function (operation, node, node_parent, node_position, more) {
                                    return true;
                                }
                            },
                            types: {
                                "default": {
                                    "icon": "fa fa-folder tree-item-icon-color icon-lg"
                                },
                                "file": {
                                    "icon": "fa fa-file tree-item-icon-color icon-lg"
                                }
                            },
                            contextmenu: {
                                items: vm.areaTree.contextMenu
                            },
                            sort: function (node1, node2) {
                                if (this.get_node(node2).original.name < this.get_node(node1).original.name) {
                                    return 1;
                                }
                                return -1;
                            },
                            plugins: jsTreePlugins
                        });

                    vm.areaTree.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                        e.preventDefault();

                        var areaId = $(this).closest('.ou-text').attr('data-ou-id');
                        setTimeout(function () {
                            vm.areaTree.$tree.jstree('show_contextmenu', areaId);
                        }, 100);
                    });
                });
            },

            reload: function () {
                vm.areaTree.getTreeDataFromServer(function (treeData) {
                    vm.areaTree.setAreaCount(treeData.length);
                    vm.areaTree.$tree.jstree(true).settings.core.data = treeData;
                    vm.areaTree.$tree.jstree('refresh');
                });
            }
        };

        vm.areaTree.init();
    }
    ]);
})();