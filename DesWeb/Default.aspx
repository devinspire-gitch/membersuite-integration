<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DesWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <section class="features-icons bg-light text-center" style="color: rgba(255,255,255,0);background-color: rgba(255,255,255,0);filter: brightness(110%);">
        <div class="container">
            <div class="row">
                <div class="col-lg-4" style="margin-right: 98px;margin-left: 122px;">
                    <a href="FAW.aspx">
                        <div class="mx-auto features-icons-item mb-5 mb-lg-0 mb-lg-3">
                            <div class="d-flex features-icons-icon"><i class="fas fa-warehouse m-auto text-primary" data-bs-hover-animate="bounce"></i></div>
                            <h3 style="font-family: Acme, sans-serif;">Find A Warehouse</h3>
                            <p class="lead mb-0" style="font-family: 'Abril Fatface', cursive;">Looking for warehouse or logistics solutions?</p>
                        </div>
                    </a>
                </div>
                <div class="col-lg-4">
                    <a href="FAP.aspx">
                        <div class="mx-auto features-icons-item mb-5 mb-lg-0 mb-lg-3">
                            <div class="d-flex features-icons-icon"><i class="fa fa-building m-auto text-primary" data-bs-hover-animate="pulse"></i></div>
                            <h3 style="font-family: Acme, sans-serif;">Find A Partner</h3>
                            <p class="lead mb-0" style="font-family: 'Abril Fatface', cursive;">Need racking, WMS, staffing, and more?</p>
                        </div>
                    </a>
                </div>
                <div class="col-lg-4">
                    <div class="mx-auto features-icons-item mb-5 mb-lg-0 mb-lg-3"></div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>
