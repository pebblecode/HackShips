LondonShips = {
    Map: null,
    Circle: null,
    BlastRadius: null,
    TargetZone: null,
    IsItMyTurn: false,
    Init: function () {

        // Get target zone
        $.ajax({
            url: "/Game/GetTargetZone",
            type: "get",
            dataType: "json",
            success: function (response, textStatus, jqXHR) {

                // When done initialize global variable
                LondonShips.TargetZone = {
                    center: new google.maps.LatLng(response.Latitude, response.Longitude),
                    radius: response.Radius,
                };

                LondonShips.BlastRadius = response.BlastRadius;
                
                // Draw map
                LondonShips.DrawMap();

                // Add listener to the click event of the circle
                google.maps.event.addListener(LondonShips.Circle, 'click', function (event) {
                    if (LondonShips.IsItMyTurn) {
                        LondonShips.Shoot(event.latLng);
                    } else {
                        alert("It's not your turn!");
                    }
                });
            }
        });

        window.setInterval("LondonShips.CheckIfItIsMyTurn();", 5000);
    },
    CheckIfItIsMyTurn: function () {
        $.ajax({
            url: "/Game/IsItMyTurn",
            type: "get",
            dataType: "json",
            success: function (response, textStatus, jqXHR) {
                if (response != LondonShips.IsItMyTurn) {
                    LondonShips.IsItMyTurn = response;
                    if (response == true) {
                        alert("It's your turn!");
                    }
                }
            }
        });
    },
    DrawMap: function () {


        // Define map
        var mapOptions = {
            zoom: 13,
            center: LondonShips.TargetZone.center,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        // Add map
        LondonShips.Map = new google.maps.Map(document.getElementById('game-map'), mapOptions);


        // Define circle
        LondonShips.Circle = new google.maps.Circle({
            strokeColor: "#006400",
            strokeOpacity: 0.8,
            strokeWeight: 2,
            fillColor: "#006400",
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
    DrawCircle: function () {

       

        
    },
    Shoot: function (latLng) {
        var coords = "{longitude: " + latLng.lng() + ", latitude: " + latLng.lat() + "}";
        $.ajax({
            url: "/Game/Shoot",
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: coords,
            success: function (response, textStatus, jqXHR) {
                // check if failed or not and inform the user
                switch (response) {
                    case "Hit":
                        LondonShips.DrawShapesWhenHit(latLng);
                        break;
                    case "Miss":
                        LondonShips.DrawShapesWhenMiss(latLng);
                        break;
                    case "GameAlreadyOver":
                        alert("You lost. Game is already over!");
                        window.location = "/";
                        break;
                        
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
            radius: LondonShips.BlastRadius
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