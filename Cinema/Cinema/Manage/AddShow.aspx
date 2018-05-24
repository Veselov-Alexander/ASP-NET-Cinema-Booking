<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="AddShow.aspx.cs" Inherits="Cinema.Manage.AddShow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/addfilm.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <h1 class="header-text">Add new session</h1>
            <br />
            <p class="bold-header">Film</p>
            <asp:DropDownList class="input-form" ID="films" runat="server" Width="100%">
            </asp:DropDownList><br /><br />
            <p class="bold-header">Date</p>
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Calendar class ="cal" ID="Calendar1" runat="server" OnSelectionChanged="Check" Width="100%" Height="250px"></asp:Calendar>
                    <br />
                    <p class="bold-header">Time</p>
                    <asp:DropDownList class="input-form" ID="time" runat="server" Width="100%">
                    </asp:DropDownList>
                    <br /><br />
                    <p class="bold-header">Cost</p>
                    <asp:DropDownList class="input-form" ID="cost" runat="server" Width="100%">
                        <asp:ListItem>60</asp:ListItem>
                        <asp:ListItem>80</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                        <asp:ListItem>120</asp:ListItem>
                        <asp:ListItem>150</asp:ListItem>
                    </asp:DropDownList>
                    <br /><br />
                    <asp:Button class="sub-button" Text="Add" ID="submit" runat="server" OnClick="submit_Click" />
                    <br /><br /><br />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
