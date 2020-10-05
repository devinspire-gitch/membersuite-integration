<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAP.aspx.cs" Inherits="DesWeb.FAP" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
      <div class="container">
        <div class="row">
               <div class="col-lg-12"> 
                   <h1 class="text-uppercase text-center" style="font-family: Acme, sans-serif;color: rgb(3,41,79);">Find A Partner</h1>
                   </div>
            </div><br />

               <div class="row rowpadding">
                   <div class="col-lg-2"> 
                       <p style="font-family: Acme, sans-serif;">Show results within:</p>
                   </div>
                   <div class="col-lg-2"> 
         <asp:DropDownList class="form-control" runat="server" ID="ddlMiles">
            <asp:ListItem Value="10">10 Miles</asp:ListItem>
            <asp:ListItem Value="30">30 Miles</asp:ListItem>
            <asp:ListItem Selected="True" Value="50">50 Miles</asp:ListItem>
            <asp:ListItem Value="100">100 Miles</asp:ListItem>
            <asp:ListItem Value="200">200 Miles</asp:ListItem>
        </asp:DropDownList>
               </div>
                      <div class="col-lg-1"> 
                       <p style="font-family: Acme, sans-serif;">Zip Code:</p>
                   </div>
                    <div class="col-lg-2"> 
                        
        
        <asp:TextBox class="form-control bg-white border-primary shadow"  placeholder="zip code ex:60000"  ID="txtZipCode" runat="server"></asp:TextBox>

                        </div>

                     <div class="col-lg-1"> 
                         <p style="font-family: Acme, sans-serif;">city/state:</p>
                   </div>
                    <div class="col-lg-2"> 
                        
        
         <asp:TextBox   placeholder="city/state" class="form-control bg-white border-primary shadow" ID="txtCity" runat="server"></asp:TextBox>

                        </div>
               </div><br />
       <div class="row">
               <div class="col-lg-12">
                    <p style="font-family: Acme, sans-serif;">Search by keyword:</p>
                   <asp:CheckBoxList ID="cbKeyword" runat="server" DataTextField="keyword"   
            DataValueField="keyword" AutoPostBack="False" 
           CellPadding="1"
           CellSpacing="1"
           RepeatColumns="3"
           RepeatDirection="Horizontal"
           RepeatLayout="Table"
           TextAlign="Right" Width="100%">  
 <asp:ListItem>Barcode Labeling & Thermal Printing </asp:ListItem>
<asp:ListItem>Labor Management Software </asp:ListItem>
<asp:ListItem>Renewables & Recyclables </asp:ListItem>
<asp:ListItem>Building Materials </asp:ListItem>
<asp:ListItem>Legal </asp:ListItem>
<%--<asp:ListItem>RFID & Wireless Infrastructure </asp:ListItem>--%>
<asp:ListItem>Business Services & Consulting </asp:ListItem>
<asp:ListItem>Lighting </asp:ListItem>
<asp:ListItem>Robotics </asp:ListItem>
<asp:ListItem>Communication Services </asp:ListItem>
<asp:ListItem>Marketing & Website </asp:ListItem>
<asp:ListItem>Safety Products </asp:ListItem>
<asp:ListItem>Construction Services </asp:ListItem>
<asp:ListItem>Material Handling Equipment </asp:ListItem>
<asp:ListItem>Sanitation & Pest Management </asp:ListItem>
<asp:ListItem>Conveyor Systems </asp:ListItem>
<asp:ListItem>Office Materials </asp:ListItem>
<asp:ListItem>Security </asp:ListItem>
<asp:ListItem>Electronic Data Interchange & Systems Integration </asp:ListItem>
<asp:ListItem>Packaging </asp:ListItem>
<%--<asp:ListItem>Software </asp:ListItem>--%>
<asp:ListItem>Energy Performance Management </asp:ListItem>
<asp:ListItem>Pallets </asp:ListItem>
<asp:ListItem>Solar </asp:ListItem>
<%--<asp:ListItem>Facility Maintenance </asp:ListItem>--%>
<asp:ListItem>Payroll & Accounting </asp:ListItem>
<asp:ListItem>Staffing Provider </asp:ListItem>
<asp:ListItem>Forklifts & Accessories </asp:ListItem>
<asp:ListItem>Plant & Facility Equipment </asp:ListItem>
<asp:ListItem>Transportation </asp:ListItem>
<%--<asp:ListItem>Human Resources </asp:ListItem>--%>
<asp:ListItem>Propane </asp:ListItem>
<asp:ListItem>Transportation Management Software </asp:ListItem>
<asp:ListItem>Information Technology & Hardware </asp:ListItem>
<asp:ListItem>Racking </asp:ListItem>
<asp:ListItem>Utilities </asp:ListItem>
<asp:ListItem>Insurance </asp:ListItem>
<asp:ListItem>Real Estate </asp:ListItem>
<asp:ListItem>Warehouse Management Software </asp:ListItem>
<asp:ListItem>IT Service & Repair </asp:ListItem>

     </asp:CheckBoxList> 

                        </div>
              </div><br />
                  <div class="row">
               <div class="col-lg-2"> 
                       <p style="font-family: Acme, sans-serif;">Search by Company</p>
                   </div>
               <div class="col-lg-6">
                  
                   <asp:TextBox   placeholder="company name" class="form-control bg-white border-primary shadow" ID="txtCompanyName" runat="server"></asp:TextBox>
                   </div>
         </div>
              <div class="row" style="text-align:center; padding-top:10px;">
               <div class="col-lg-12">
                   <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" class="btn btn-primary" Text="Search" />

                   </div>
                 </div>


       </div>
  

    <script type="text/javascript">
        $(".effect1").on('click', function (event) {
            window.location.href = "Map.aspx";
        });
        $(function () {
          
            var table = document.getElementById("partnerTable");
            var rows = table.getElementsByTagName("tr");
            for (i = 0; i < rows.length; i++) {
                var currentRow = table.rows[i];
                var createClickHandler = function (row) {
                  
                    return function () {
                    window.location.href = "Map.aspx";
                    };
                };
                currentRow.onclick = createClickHandler(currentRow);
            } 
        });
    </script>

    <div id="dialog" style="display: none">
        <div id="dvMap" style="height: 380px; width: 580px;">
        </div>
    </div>
   <script async defer
    src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCr-UbKwgCZVWUUoiV-rOfh5L6W7fN0uGo&callback=initMap">
    </script>
    <style>
          .rowpadding { padding:10px; }
       .box {
    
    background:#F2F2F2;
    margin:10px auto;
}
/*==================================================
 * Effect 1
 * ===============================================*/
.effect1{
   border: 1px solid #BFBFBF;
background-color: #F2F2F2;
box-shadow: 5px 4px 11px #aaaaaa;
}
#MainContent_cbKeyword label {
  margin-left: 15px;
}

    </style>
</asp:Content>
