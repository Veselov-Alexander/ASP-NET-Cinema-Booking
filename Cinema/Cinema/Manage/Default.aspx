<%@ Page Title="" Language="C#" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Cinema.Manage.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
    <style>
        .reg-form 
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <h1 style="font-size: 40px" class="header-text">Functions</h1><br />
            <asp:HyperLink  class="header-text" ID="add_film" NavigateUrl="~/Manage/AddFilm" Text="Add film" runat="server" />
            <br /><br />
            <asp:HyperLink class="header-text" ID="HyperLink1" NavigateUrl="~/Manage/AddShow" Text="Add show" runat="server" />
            <br /><br />
            <asp:HyperLink class="header-text" ID="HyperLink2" NavigateUrl="~/Manage/Orders" Text="Check orders" runat="server" />
            <br /><br />
        </div>
        <br />
    </div>
</asp:Content>
