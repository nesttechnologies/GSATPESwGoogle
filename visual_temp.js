 /* This is Visualization of Estimated Population data with their condifence level
               within a circular range.*/
            function initMap(){
                var map;
                var infoWindow;
                var irad=Number(document.getElementById("rad").value);
                var ilong=Number(document.getElementById("long").value);
                var ilat=Number(document.getElementById("lat").value);
                map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 15,
                    center: {lat: ilat, lng: ilong},
                    mapTypeId: google.maps.MapTypeId.ROADMAP,
                    scaleControl: true
                });
                var iRows = 10000;
                var iCols = 4;
                var i;
                var j;
                var data_g = new Array(iRows);
                /* Generate 10000 coordinates and generate random data of population density 
                 * and confidence level for each grid coordinates*/
                for (i = 0; i < iRows; i++) {
                    ////////test
                    var b = new Array(iCols);
                    data_g[i] = b;
                    ///////////
                    if(i%100 > 0){
                        for (j = 0; j < iCols; j++) {                    
                            if(j===0)
                            {
                                data_g[i][0] = data_g[i-1][0] + 0.00002;
                            }else if(j===1)
                            {
                                data_g[i][1] = data_g[i-1][1] + 0.001152;                 
                            } else if(j===2){
                                data_g[i][2] = Math.floor(Math.random()*1000); 
                            } else if(j===3){
                                data_g[i][3] = Math.floor(Math.random()*100); 
                            }
                        }}else {
                        for (j = 0; j < iCols; j++) {
                            if(i===0 && j===0){
                                data_g[i][j] = ilat-((0.00002*50)+(0.00090*50));
                            }else if(i===0 && j===1)
                            {
                                data_g[i][j] = ilong-((0.001152*50)+(0.000026*50));  
                                data_g[i][2] = Math.floor(Math.random()*1000);
                                data_g[i][3] = Math.floor(Math.random()*100);
                            }else{
                                if(j===0)
                                {
                                    data_g[i][0] = data_g[i-100][0] + 0.0009;
                                }else if(j===1)
                                {
                                    data_g[i][1] = data_g[i-100][1] + 0.000026;
                                } else if(j===2){ 
                                    data_g[i][2] = Math.floor(Math.random()*1000); 
                                } else if(j===3){
                                    data_g[i][3] = Math.floor(Math.random()*100); 
                                }
                            }
                        }
                    }
                }
                // Define the circular range.        
                var Circle = new google.maps.Circle({
                    strokeColor: '#0000FF',
                    strokeOpacity: 0.8,
                    strokeWeight: 2,
                    fillColor: '#0000FF',
                    fillOpacity: 0.0,
                    map: map,
                    center: {lat: ilat, lng: ilong},
                    radius: irad
                });
                Circle.setMap(map); 
                // Construct the UTM Squares.
                for(var i = 0;i < 10000;i++)
                { 
                    var coord1 = new google.maps.LatLng(a[i][0]+(0.00090/2), a[i][1]+(0.001152/2));
                    if(Circle.getBounds().contains(coord1))
                    {
                        if(i%100 === 99)
                        {
                        }
                        else{ 
                            var squareCoords = [
                                {lat: data_g[i][0], lng: data_g[i][1]},
                                {lat: data_g[i+100][0], lng: data_g[i+100][1]},
                                {lat: data_g[i+101][0], lng: data_g[i+101][1]},
                                {lat: data_g[i+1][0], lng: data_g[i+1][1]}
                            ];
                            var color;
                            var opacity;
                            if(data_g[i][2]>1 && data_g[i][2]<10){
                                color = '#00FF00';
                            } 
                            else if(data_g[i][2]>10 && data_g[i][2]<100)
                            {
                                color = '#FFFF00';
                            } else if(data_g[i][2]>100 && data_g[i][2]<1000){
                                color = '#FF7F00';
                            } else {
                                color = '#FF0000';
                            }
                            if(data_g[i][3]>0 && data_g[i][3]<25)
                            {
                                opacity = 0.2;
                            }
                            else if(data_g[i][3]>25 && data_g[i][3]<50)
                            {
                                opacity = 0.3;
                            } 
                            else if(data_g[i][3]>50 && data_g[i][3]<75)
                            {
                                opacity: 0.4;
                            }
                            else
                            {
                                opacity: 0.5;
                            }
                            var UTMsquare = new google.maps.Polygon({
                                paths: squareCoords,
                                strokeColor: '#FFFFFF',
                                strokeOpacity: 0.2,
                                strokeWeight: 1,
                                fillColor: color,
                                fillOpacity: opacity
                            });
                            UTMsquare.setMap(map);
                            // Add a listener for the click event.
                            UTMsquare.addListener('click', showArrays);
                            infoWindow = new google.maps.InfoWindow;
                            
                }
            }
            else
            {
            }
        }
    }
    function showArrays(event) 
    {
        /* Since this polygon has only one path, we can call getPath() to return the
         *          MVCArray of LatLngs.*/
        var vertices = this.getPath();
        var contentString = '<b>UTM Square polygon</b><br>' + 
                'Clicked location: <br>' + event.latLng.lat() + ',' + event.latLng.lng() +
                '<br>';
        // Iterate over the vertices.
        for (var i =0; i < vertices.getLength(); i++) 
        {
            var xy = vertices.getAt(i);
            contentString += '<br>' + 'Coordinate ' + i + ':<br>' + xy.lat() + ',' + xy.lng();
        }
        // Replace the info window's content and position.
        infoWindow.setContent(contentString);
        infoWindow.setPosition(event.latLng);
        infoWindow.open(map);
    }
