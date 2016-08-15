(function () {
    appModule.controller('common.views.common.gmapModal', [
        '$scope', '$uibModalInstance', 'gmapOptions', 'uiGmapGoogleMapApi',
        function ($scope, $uibModalInstance, gmapOptions, GoogleMapApi) {
            var vm = this;
            vm.loading = false;
            vm.marker = {};


            GoogleMapApi.then(function (maps) {
                maps.visualRefresh = true;

                angular.extend($scope, { map: $scope.map });

                init();
            })

            //点击标点
            vm.onMarkerClicked = function (marker) {
                marker.showWindow = true;
                $scope.$apply();
            };

            //地图
            vm.map = $scope.map = {
                show: true,
                control: {},
                version: "uknown",
                showTraffic: true,
                showBicycling: false,
                showWeather: false,
                showHeat: false,
                center: {
                    latitude: 45,
                    longitude: -73
                },
                searchbox: {
                    template: 'searchbox.tpl.html',
                    parentdiv: 'searchDiv',
                    events: {
                        places_changed: function (searchBox) {
                            var places = searchBox.getPlaces();
                            vm.map.markers.length = 0;
                            if (places) {
                                for (var i = 0; i < places.length; i++) {
                                    var place = places[i];
                                    vm.map.markers.push(vm.marker(place.place_id, place.geometry.location.lat(), place.geometry.location.lng(), place.name));
                                    var map = vm.map.control.getGMap();
                                    map.center.latitude = vm.map.markers[0].latitude;
                                    map.center.longitude = vm.map.markers[0].longitude;
                                    map.panTo({ lat: map.center.latitude, lng: map.center.longitude });
                                }
                            }
                        }
                    }
                },
                bounds: {},
                options: {
                    streetViewControl: false,
                    panControl: false,
                    maxZoom: 20,
                    minZoom: 1
                },
                zoom: 15,
                dragging: false,
                markers: []
            };

            //Options
            vm.options = angular.extend({
                title: app.localize('GoogleMap'),
                showFilter: true,
                address: '',//省市区县
                filterText: '',//名称
                callback: function (marker) {
                    //This method is used to get selected item
                }
            }, gmapOptions);

            vm.cancel = function () {
                $uibModalInstance.dismiss();
            };

            vm.marker = function (id, latitude, longitude, text) {
                return {
                    id: id,
                    icon: '/Common/images/blue_marker.png',
                    latitude: latitude,
                    longitude: longitude,
                    showWindow: false,
                    text: text,
                    callback: vm.options.callback,
                    $uibModalInstance: $uibModalInstance,
                    options: {
                        labelClass: "marker-labels"
                    }
                };
            };

            function init() {
                //通过 address + filterText，查找点
                vm.options.filterText = vm.options.address + vm.options.filterText;
                if (vm.map.markers.length > 0) {
                    var map = $scope.map.control.getGMap();
                    map.center.latitude = vm.map.markers[0].latitude;
                    map.center.longitude = vm.map.markers[0].longitude;
                    map.panTo({ lat: map.center.latitude, lng: map.center.longitude });
                }
            }

        }])
    .controller('common.views.common.gmapModal.InfoController', function ($scope) {

        $scope.clickedButtonInWindow = function (callback) {
            var marker = { latitude: $scope.$parent.model.latitude, longitude: $scope.$parent.model.longitude };
            $scope.$parent.model.callback(marker);
            $scope.$parent.model.$uibModalInstance.close(marker);
        }
    });

    //gmapModal service
    appModule.factory('gmapModal', [
        '$uibModal',
        function ($uibModal) {
            function open(gmapOptions) {
                $uibModal.open({
                    templateUrl: '~/App/common/views/common/gmapModal.cshtml',
                    controller: 'common.views.common.gmapModal as vm',
                    backdrop: 'static',
                    size: "lg",
                    resolve: {
                        gmapOptions: function () {
                            return gmapOptions;
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