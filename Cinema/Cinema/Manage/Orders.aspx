<%@ Page Title="" Language="C#" ClientIDMode="Static" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="Cinema.Manage.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
    <link rel="stylesheet" type="text/css" href="../Content/orders.css">
    <script>
        function approve(e)
        {
            var myHidden = document.getElementById('<%= HiddenField1.ClientID %>');
            myHidden.value = e.id; 
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
             <asp:Label ID="orders" runat="server" Text=""></asp:Label>
        </div><br />
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" OnValueChanged="HiddenField1_ValueChanged" />
</asp:Content>