<%@ Page Title="" Language="C#" async="true" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Cinema.Account.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <h1 class="header-text">Create your personal account</h1>
            <br />
            <p class="bold-header">Username</p>
            <input class="input-form" type="text" id="loginBox" runat="server" />
            <br />
            <asp:label class="lower-text" id="loginError" runat="server">This will be your username. </asp:label>
            <br /><br />
            <p class="bold-header">Email address</p>
            <input class="input-form" type="email" id="emalBox" runat="server" />
            <br />
            <asp:label class="lower-text" id="emailError" runat="server">We'll occasionally send updates about your account to this inbox. </asp:label>
            <br /><br />
            <p class="bold-header">Password</p>
            <input class="input-form" type="password" id="passwordBox" runat="server" />
            <asp:label class="lower-text" id="passwordError" runat="server">Use at least one lowercase letter, one numeral, and seven characters. </asp:label>
            <br /><br /><br /><br />
            <asp:button class="sub-button" text="Create an account" id="submit" runat="server" onclick="submit_Click" />
        </div>
    </div>
</asp:Content>
