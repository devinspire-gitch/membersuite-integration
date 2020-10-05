<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchFAW.aspx.cs" Inherits="DesWeb.SearchFAW" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
        <div class="container">
            <div class="row">
                <div class="col-lg-8">  
                    <h1 class="text-uppercase text-center" style="font-family: Acme, sans-serif;color: rgb(3,41,79);">SEARCH RESULTS</h1>
                    </div>
                <div class="col-lg-4">
                 
                  <%-- <asp:Button ID="lnkSearch" CssClass="btn btn-primary" runat="server" Text="New Search" u onclientclick='redirect(); return false;' />--%>
                    <asp:Button OnClick="btnSearch_Click" runat="server" CssClass="btn btn-primary" ID="btnSearch" Text="New Search" />
                    </div>
            </div>
        </div>
    
    <br />
    <div class="container">
        <div class="row">
               <div class="col-lg-12 pager">  
              <asp:Repeater ID="rptPager" runat="server">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkPage" runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%# Eval("Value") %>'
                        CssClass='<%# Convert.ToBoolean(Eval("Enabled")) ? "page_enabled" : "page_disabled" %>'
                        OnClick="Page_Changed" OnClientClick='<%# !Convert.ToBoolean(Eval("Enabled")) ? "return false;" : "" %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:Repeater>
             </div>
        </div>
        <div class="row">
              <asp:Repeater ID="Repeater1" runat="server"  >

                    <HeaderTemplate>
                          <div class="container">
                  </HeaderTemplate>

                    <ItemTemplate>
                      

                    
                            <div class="row  srch-res-l">
                                <div class="col-lg-8">  
                                   <h2><a  href=' <%# "DetailsHD.aspx?company=" + DataBinder.Eval(Container.DataItem, "Name")  %>'><%# DataBinder.Eval(Container.DataItem, "Name") %></a></h2>
                                   
                                
                                               <p>
                                                  <b> <%# DataBinder.Eval(Container.DataItem, "PrimaryContactName__RightSide__rt") %></b><br />
                                                    <%# DataBinder.Eval(Container.DataItem, "_Preferred_Address_Line1") %>  <br/> 
                                                   <%# DataBinder.Eval(Container.DataItem, "_Preferred_Address_Line2") %>

                                                   <%# Eval("_Preferred_Address_City") + ", " + Eval("_Preferred_Address_State") + " "+ Eval("_Preferred_Address_PostalCode").ToString().Substring(0,5) %> <br />
                          
                                                  <span> <%# DataBinder.Eval(Container.DataItem, "_Preferred_PhoneNumber") %></span><br />
                                                   

                                                   <a  target='_blank' class='website' href=' <%# DataBinder.Eval(Container.DataItem, "WebSite") %>'> <%# DataBinder.Eval(Container.DataItem, "WebSite") %></a>
                                                </p>

                                </div>
                                <div class="col-lg-4">
                                        <asp:HiddenField ID="hfName" Value='<%# Eval("Name")%>' runat="server" />
                                     <asp:HiddenField ID="hfEmail" Value='<%# DataBinder.Eval(Container.DataItem, "EmailAddress") %>' runat="server" />
                                   <asp:CheckBox ID="cbReqInfo"
     AutoPostBack = "True"
     oncheckedchanged="cbReqInfo_Checked"
    
 runat="server" /><b> Request for Information</b><br /><br />
                                     
                                    
                                   
                                   
                                    <a  href='<%# "DetailsHD.aspx?company=" + DataBinder.Eval(Container.DataItem, "Name")  %>'>
                                        <img src='<%#  DataBinder.Eval(Container.DataItem, "ImagePath")  %>' alt='<%#  DataBinder.Eval(Container.DataItem, "Name")  %>' style='width:150px!important;'/>

                                    </a>
                                     <br />
                                    <br />
                                    <a href='<%# "Mail.aspx?company=" + DataBinder.Eval(Container.DataItem, "Name") +"&email="+ DataBinder.Eval(Container.DataItem, "EmailAddress")  %>' class="website">E-mail</a>
                                                 <%--                                    <a    class='website' href='mailto:christopher.gould@acd-group.com?subject=Inquiry from IWLA Website'>E-mail</a>--%>
                                     <%--<a  target='_blank' class='website' href=' <%# "mailto:" + DataBinder.Eval(Container.DataItem, "EmailAddress") + "?subject=Inquiry from IWLA Website" %>'> E-Mail</a>--%>
                                </div>
                             
                            </div>
                           
                         <hr />
                    </ItemTemplate>

                    <FooterTemplate>

                       </div>

                    </FooterTemplate>

                </asp:Repeater>
        </div>
        <div class="row  top-buffer">
            <div class="col-md-6 text-right"><asp:Button OnClick="btnReqInfo_Click" runat="server" CssClass="btn btn-primary" ID="btnReqInfo" Text="Sumbit Request Info" /></div>
            
        
        </div>


        <div class="row">
            <div id="map_canvas" ></div>
        </div>
    </div>
  
    <br />
   
     
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
    function redirect() {
     
       // location.href = 'javascript:history.go(-1)';
        
    }
    // Initialize and add the map
    function initialize() {
        var markers = JSON.parse('<%=ConvertDataTabletoString() %>');
        $('#footerSlideContainer').hide();
        var mapOptions = {
            center: new google.maps.LatLng(markers[0].Lat, markers[0].Lng),
            zoom: 5,
            mapTypeId: google.maps.MapTypeId.ROADMAP
            //  marker:true
        };
        var infoWindow = new google.maps.InfoWindow();
        $("#map_canvas").css({ height: "100%", width: "100%" });
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

       <script async defer
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCr-UbKwgCZVWUUoiV-rOfh5L6W7fN0uGo&callback=initialize">
    </script>
    <style>
        .top-buffer { padding:20px; }
 #map_canvas {
   width: 100%; height: 400px !important;
  position: relative;
 } 
.srch-res-l h2 {
    margin: 0px 0px 25px 0px;
    color: #282828;
    font-size: 23px;
    font-weight: 600;
}

.page_enabled, .page_disabled
        {
            display: inline-block;
            height: 20px;
            min-width: 20px;
            line-height: 20px;
            text-align: center;
            text-decoration: none;
            border: 1px solid #ccc;
        }
        .page_enabled
        {
            background-color: #eee;
            color: #000;
        }
        .page_disabled
        {
            background-color: #6C6C6C;
            color: #fff !important;
        }
        .pager {
            text-align:center;
        }
</style>
</asp:Content>
