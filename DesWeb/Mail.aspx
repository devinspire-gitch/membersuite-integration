<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Mail.aspx.cs" Inherits="DesWeb.Mail" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
    <h2 class="text-center">Contac Form</h2>
	<div class="row justify-content-center">
		<div class="col-12 col-md-8 col-lg-6 pb-5">


                    <!--Form with header-->

                   
                        <div class="card border-primary rounded-0">
                            <div class="card-header p-0">
                                <div class="bg-info text-white text-center py-2">
                                    <h3><i class="fa fa-envelope"></i> Contact IWLA</h3>
                                   <%-- <p class="m-0">email</p>--%>
                                </div>
                            </div>
                            <div class="card-body p-3">

                                <!--Body-->
                                <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-user text-info"></i></div>
                                        </div>
                                        <asp:TextBox class="form-control" runat="server" ID="txtName" placeholder="Name" required></asp:TextBox>
            
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-user text-info"></i></div>
                                        </div>
                                        <asp:TextBox class="form-control" runat="server" ID="txtCompany" placeholder="Company" required></asp:TextBox>
            
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-phone text-info"></i></div>
                                        </div>
                                        <asp:TextBox class="form-control" runat="server" ID="txtPhone" placeholder="Phone" required></asp:TextBox>
                                        
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-envelope text-info"></i></div>
                                        </div>
                                        <asp:TextBox type="email" class="form-control" runat="server" ID="txtEmail" placeholder="example@gmail.com" required></asp:TextBox>
                                       <%-- <input type="email" class="form-control" id="nombre" name="email" placeholder="ejemplo@gmail.com" required>--%>
                                    </div>
                                </div>

                                  <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-header text-info"></i></div>
                                        </div>
                                        <asp:TextBox  class="form-control" runat="server" ID="txtSubject" placeholder="Subject" required></asp:TextBox>
                                       <%-- <input type="email" class="form-control" id="nombre" name="email" placeholder="ejemplo@gmail.com" required>--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="input-group mb-2">
                                        <div class="input-group-prepend">
                                            <div class="input-group-text"><i class="fa fa-comment text-info"></i></div>
                                        </div>
                                        <asp:TextBox  TextMode="MultiLine"  Rows="4" class="form-control" runat="server" ID="txtNote" placeholder="Note" required></asp:TextBox>
                                        <%--<textarea class="form-control" placeholder="Envianos tu Mensaje" required></textarea>--%>
                                    </div>
                                </div>

                                <div class="text-center">
                                    <asp:Button runat="server" OnClick="btnSubmit_Click" ID="btnSubmit" class="btn btn-info btn-block rounded-0 py-2" Text="Submit" /> 
                   <%--                 <input type="submit" value="Submit" class="btn btn-info btn-block rounded-0 py-2">--%>
                                     <asp:Label ID="Label1" runat="server" Text=""/>
                                </div>
                            </div>

                        </div>
                   


                </div>
	</div>
</div>
</asp:Content>
