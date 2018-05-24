<%@ Page Title="" Language="C#" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Cinema.Account.Profile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
    <link rel="stylesheet" type="text/css" href="../Content/orders.css">
    <style>
        .pdf 
        {
            color: #2D6CDF;
            padding: 0px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <asp:label id="login" runat="server" />
            <br />
            <asp:Label ID="orders" runat="server" Text=""></asp:Label>
        </div>
        <br />
    </div>
</asp:Content>
