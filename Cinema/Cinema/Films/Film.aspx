<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Film.aspx.cs" Inherits="Cinema.Films.Film" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/film.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <div id="main">
                <div class="face">
                    <asp:Image class="t-img" ID="poster" runat="server" />
                    <div class="t-row">
                        <div class="r-1">
                            <div class="rl-1">title:</div>
                            <div class="rl-2">
                                <asp:Label ID="title" runat="server" Text="Title"></asp:Label>
                            </div>
                        </div>
                        <div class="r-1">
                            <div class="rl-1">country:</div>
                            <div class="rl-2">
                                <asp:Label ID="country" runat="server" Text="Conutry"></asp:Label>
                            </div>
                        </div>
                        <div class="r-1">
                            <div class="rl-1">genres:</div>
                            <div class="rl-2">
                                <asp:Label ID="genre" runat="server" Text="Genre"></asp:Label>
                            </div>
                        </div>
                        <div class="r-1">
                            <div class="rl-1">durability:</div>
                            <div class="rl-2">
                                <asp:Label ID="durability" runat="server" Text="Durability"></asp:Label>
                            </div>
                        </div>
                        <div class="r-1">
                            <div class="rl-1">producer:</div>
                            <div class="rl-2">
                                <asp:Label ID="producer" runat="server" Text="Producer"></asp:Label>
                            </div>
                        </div>
                        <div class="r-1">
                            <div class="rl-1">year:</div>
                            <div class="rl-2">
                                <asp:Label ID="year" runat="server" Text="Year"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="r-1">
                            <div class="rl-1">description:</div>
                            <div class="rl-2">
                                <asp:Label class="content-wrapper" ID="description" runat="server" Text="Description" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
    </div>
</asp:Content>
