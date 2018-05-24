<%@ Page Title="" Language="C#" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Cinema.Shows.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Content/form.css" rel="stylesheet" type="text/css" />
    <link href="../Content/shows.css" rel="stylesheet" type="text/css" />
    <script>
        function openTab(evt, button, tab)
        {
            document.getElementById('<%= selectedTab.ClientID %>').value = button.id.substring(1);
            var tabcontent = document.getElementsByClassName("tabcontent");
            for (var i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }
            document.getElementById(tab).style.display = "block";
            setHeight(tab);
        }
        function selectFirst(date)
        {
            document.getElementById(date).style.display = "block";
            setHeight(date);
        }
        function setHeight(date)
        {
            var filmAmount = document.getElementById(date).getElementsByTagName('ul').length;
            document.getElementById('gWR').style.height = 60 + 40 * filmAmount + "vh";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True" RenderMode="Inline">
                <ContentTemplate>
                    <div class="tab">
                        <asp:Label ID="Tabs" runat="server" Text=""></asp:Label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Label ID="generateInfo" runat="server" Text=""></asp:Label>
        </div>
        <br />
    </div>
    <asp:HiddenField ID="selectedTab" runat="server" Value="0" />
</asp:Content>
