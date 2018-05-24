<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Cinema.Account.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form">
            <h1 class="header-text">Login to your account</h1>
            <br />
            <p class="bold-header">Username or email address</p>
            <input class="input-form" type="text" name="" id="loginBox" runat="server" text=" " />
            <asp:Label class="error-string" ID="loginError" runat="server"><br /></asp:Label>
            <br /><br />
            <p class="bold-header">Password</p>
            <input class="input-form" type="password" name="" id="passwordBox" runat="server" />
            <asp:Label class="error-string" ID="passwordError" runat="server" Text=" "><br /></asp:Label>
            <br />
            <p class="lower-text">
                New to Cinema?<a href="Register">Create an account.</a>
            <p />
            <br /><br /><br />
            <asp:Button class="sub-button" Text="Login" ID="submit" runat="server" OnClick="submit_Click" />
        </div>
    </div>
</asp:Content>
