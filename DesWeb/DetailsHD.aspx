<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DetailsHD.aspx.cs" Inherits="DesWeb.DetailsHD" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    
        <div class="container">
            <div class="row">
                <div class="col-lg-8">  
                    <h1 class="text-uppercase text-center" style="font-family: Acme, sans-serif;color: rgb(3,41,79);">Warehouse PROFILE</h1>
                    </div>
                <div class="col-lg-4">
                 
                  <%-- <asp:Button ID="lnkSearch" CssClass="btn btn-primary" runat="server" Text="New Search" onclientclick='redirect(); return false;' />--%>
                    <asp:Button OnClick="btnSearch_Click" runat="server" CssClass="btn btn-primary" ID="btnSearch" Text="New Search" />
                    </div>
            </div>
        </div>
    
    <br />
    <div class="container">
       
        <div class="row">
             
                          <div class="container">
                  
                            <div class="row  srch-res-l">
                                <div class="col-lg-8">  
                                  
                                    <h2><asp:Label runat="server" ID="lblName"></asp:Label></h2>
                                <div>
                                    <asp:Image runat="server" ID="imgCompany" />

                                </div>
                                               <p>
                                                  <b> <asp:Label runat="server" ID="lblContactName"></asp:Label></b><br />
                                                   <asp:Label runat="server" ID="lblAddress1"></asp:Label> <br/> 
                                                  <asp:Label runat="server" ID="lblAddress2"></asp:Label>
                                                   <asp:Label runat="server" ID="lblCityStateZip"></asp:Label> <br />
                          
                                                  <span> <asp:Label runat="server" ID="lblPhone"></asp:Label></span><br />
                                                   
                                                    <asp:HyperLink id="lnkWebSite" NavigateUrl="https://www.iwla.com/" CssClass="website" Text="" Target="_new" runat="server"/> 
                                                   <a  target='_blank' class='website' href=' <%# WebSite %>'> <%# WebSite %></a><br />
                                                   <iframe width="560" height="315" frameborder="0" id="ContentIframe" runat="server"></iframe>
                                                </p>

                                    <div class="clear"></div>	
		 
						<p>
                            <asp:Literal runat="server" ID="ltNote"></asp:Literal>
 </p>

                                </div>
                            
                             
                            </div>
                           
                      

                       </div>

                 
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
 

</script>

    
    <style>

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
