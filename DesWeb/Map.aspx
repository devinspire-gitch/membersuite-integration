<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Map.aspx.cs" Inherits="DesWeb.Map" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     
    <!--The div element for the map -->
    
       
        <div id="map_canvas" ></div>
   
 
    <script>
        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }
// Initialize and add the map
        function initialize() {
            var markers = JSON.parse('<%=ConvertDataTabletoString() %>');
            $('#footerSlideContainer').hide();
              var mapOptions = {
                  center: new google.maps.LatLng(markers[0].Lat, markers[0].Lng),
                  zoom:10,
                  mapTypeId: google.maps.MapTypeId.ROADMAP
                  //  marker:true
              };
            var infoWindow = new google.maps.InfoWindow();
           $("#map_canvas").css({ height: "100%", width: "100%"});
              var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
              for (i = 0; i < markers.length; i++) {
                  var data = markers[i];
                 
                  var myLatlng = new google.maps.LatLng(data.Lat, data.Lng);
                  var marker = new google.maps.Marker({
                      position: myLatlng,
                      map: map,
                      title: data.Address
                  });
                  (function (marker, data) {

                      // Attaching a click event to the current marker
                      google.maps.event.addListener(marker, "click", function (e) {
                          infoWindow.setContent("<b>" + data.Address + "</b> <br/>" + data.Phone);
                          infoWindow.open(map, marker);
                      });
                  })(marker, data);
              }
          }
    </script>
    <!--Load the API from the specified URL
    * The async attribute allows the browser to render the page while the API loads
    * The key parameter will contain your own API key (which is not needed for this tutorial)
    * The callback parameter executes the initMap() function
    -->
    <script async defer
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCr-UbKwgCZVWUUoiV-rOfh5L6W7fN0uGo&callback=initialize">
    </script>
    <style>

 #map_canvas {
   width:100% !important;
   height:100% !important;
   top:100px !important;
   left:0px !important; 
   position: fixed !important; 
 } 
</style>

</asp:Content>
