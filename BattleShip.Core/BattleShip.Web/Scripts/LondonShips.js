var LondonShips = {
    Map: null,
    Circle: null,
    HitRadius: 150,
    TargetZone: null,
    Init: function () {

        // Get target zone
        $.ajax({
            url: "/Game/GetTargetZone",
            type: "get",
            dataType: "json",
            success: function (response, textStatus, jqXHR) {

                // When done initialize global variable
                LondonShips.TargetZone = {
                    center: new google.maps.LatLng(response.Latitude, response.LongLongitude),
                    radius: response.Radius
                };

                // Draw map
                LondonShips.DrawMap();

                // Add listener to the click event of the circle
                google.maps.event.addListener(LondonShips.Circle, 'click', function (event) {
                    LondonShips.Shoot(event.latLng);
                });
            }
        });

    },
    DrawMap: function () {


        // Define map
        var mapOptions = {
            zoom: 15,
            center: LondonShips.TargetZone.center,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        // Add map
        LondonShips.Map = new google.maps.Map(document.getElementById('game-map'), mapOptions);


        // Define circle
        LondonShips.Circle = new google.maps.Circle({
            strokeColor: "#4169e1",
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: "#1e90ff",
            fillOpacity: 0.35,
            map: LondonShips.Map,
            center: LondonShips.TargetZone.center,
            radius: LondonShips.TargetZone.radius
        });

        // Define player marker
        var playerImage = new google.maps.Marker({
            position: new google.maps.LatLng(LondonShips.Circle.getBounds().ca.f, LondonShips.TargetZone.center.lng()),
            map: LondonShips.Map,
            icon: new google.maps.MarkerImage(
                        '/Content/Images/image.png',
                        new google.maps.Size(48, 48),
                        new google.maps.Point(0, 0),
                        new google.maps.Point(24, 24)),
            shadow: new google.maps.MarkerImage(
                        '/Content/Images/shadow.png',
                        new google.maps.Size(76, 48),
                        new google.maps.Point(0, 0),
                        new google.maps.Point(24, 24)
                        ),
            shape: {
                coord: [0, 0, 48, 48],
                type: 'rect'
            }
        });
    },
    Shoot: function (latLng) {
        var coords = "{Lat: " + latLng.lat() + ", Long: " + latLng.lng() + "}";
        $.ajax({
            url: "/Game/Shoot",
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: coords,
            success: function (response, textStatus, jqXHR) {
                // check if failed or not and inform the user
                if (response.InTheTarget) {
                    alert("Congrats.That was a hit!");
                    LondonShips.DrawShapesWhenHit(latLng);
                } else {
                    alert("You missed!");
                    LondonShips.DrawShapesWhenMiss(latLng);
                }
            }
        });
    },
    DrawShapesWhenMiss: function (latLng) {

        // Define miss marker
        var missMarker = new google.maps.Marker({
            position: latLng,
            map: LondonShips.Map,
            icon: new google.maps.MarkerImage('/Content/Images/miss_original_64x64.png',
                        new google.maps.Size(64, 64),
                        new google.maps.Point(0, 0),
                        new google.maps.Point(32, 32)),
            shape: {
                coord: [0, 0, 64, 64],
                type: 'rect'
            }
        });

        // Define circle
        LondonShips.Circle = new google.maps.Circle({
            strokeColor: "#000000",
            strokeOpacity: 0.8,
            strokeWeight: 1,
            fillColor: "#ffffff",
            fillOpacity: 0.40,
            map: LondonShips.Map,
            center: latLng,
            radius: LondonShips.HitRadius
        });

    },
    DrawShapesWhenHit: function (latLng) {

        // Define miss marker
        var hitMarker = new google.maps.Marker({
            position: latLng,
            map: LondonShips.Map,
            icon: new google.maps.MarkerImage('/Content/Images/hit_original_64x64.png',
                        new google.maps.Size(64, 64),
                        new google.maps.Point(0, 0),
                        new google.maps.Point(32, 32)),
            shape: {
                coord: [0, 0, 64, 64],
                type: 'rect'
            }
        });

        // Define circle
        LondonShips.Circle = new google.maps.Circle({
            strokeColor: "#000000",
            strokeOpacity: 0.8,
            strokeWeight: 0,
            fillColor: "#ffffff",
            fillOpacity: 0.70,
            map: LondonShips.Map,
            center: latLng,
            radius: LondonShips.HitRadius
        });

    }
};