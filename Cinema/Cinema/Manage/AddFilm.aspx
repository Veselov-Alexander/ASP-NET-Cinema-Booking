<%@ Page Title="" Language="C#" async="true" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="AddFilm.aspx.cs" Inherits="Cinema.Manage.AddFilm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Content/form.css">
    <link rel="stylesheet" type="text/css" href="../Content/addfilm.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <h1 class="header-text">Add new film</h1><br />
            <p class="bold-header">Film title</p>
            <asp:TextBox class="input-form" type="text" name="" ID="title" runat="server" Text="" />
            <br />
            <asp:Label class="lower-text" ID="title_error" runat="server">Enter film title.</asp:Label>
            <br /><br />
            <p class="bold-header">Enter description</p>
            <asp:TextBox class="input-form-2" ID="description" runat="server" TextMode="MultiLine" Rows="4" MaxLength="1000"></asp:TextBox>
            <br />
            <asp:Label class="lower-text" ID="description_error" runat="server">Fill out a short description of the film.</asp:Label>
            <br /><br />
            <p class="bold-header">Upload poster</p>
            <input class="input-form-picture" type="text" name="" ID="poster_path" runat="server" />
            <label class="custom-file-upload" runat="server">
                <asp:FileUpload ID="poster" runat="server" OnChange="changePathText()" />
                Upload
            </label>
            <br />
            <asp:Label class="lower-text" ID="image_error" runat="server">Select an image for the poster.</asp:Label>
            <br /><br />
            <p class="bold-header">URL</p>
            <asp:TextBox class="input-form" type="text" name="" ID="URL" runat="server" />
            <br />
            <asp:Label class="lower-text" ID="URL_error" runat="server">Enter the URL (must be unique).</asp:Label>
            <br /><br />
            <p class="bold-header">Year</p>
            <asp:TextBox class="input-form" type="text" name="" ID="year" runat="server" />
            <br />
            <asp:Label class="lower-text" ID="year_error" runat="server">Enter year of film.</asp:Label>
            <br /><br />
            <p class="bold-header">Durability</p>
            <asp:TextBox class="input-form" type="text" name="" ID="durability" runat="server" />
            <br />
            <asp:Label class="lower-text" ID="durability_error" runat="server">Enter the length of the movie (in minutes).</asp:Label>
            <br /><br />
            <p class="bold-header">Producer</p>
            <input class="input-form" type="text" name="" id="producer" runat="server" />
            <br />
            <asp:Label class="lower-text" ID="producer_error" runat="server">Producer's name.</asp:Label>
            <br /><br />
            <p class="bold-header">Category</p>
            <asp:CheckBoxList class="input-form-list" ID="category" runat="server" RepeatColumns="4">
            </asp:CheckBoxList>
            <asp:Label class="lower-text" ID="category_error" runat="server">Select at least one category.</asp:Label>
            <br /><br />
            <p class="bold-header">Country</p>
            <asp:DropDownList class="input-form" ID="country" runat="server">
            </asp:DropDownList>
            <br /><br />
            <asp:Button class="sub-button" Text="Add" ID="submit" runat="server" OnClick="submit_Click" />
            <br /><br /><br />
        </div>    
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script>
        function changePathText()
        {
            body_poster_path.value = body_poster.value.split('\\')[2];
        }
    </script>
</asp:Content>

