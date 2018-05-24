<%@ Page Title="" Language="C#" ClientIDMode="Static" MasterPageFile="~/Cinema.Master" AutoEventWireup="true" CodeBehind="Book.aspx.cs" Inherits="Cinema.Shows.Book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../../../Content/book.css">
    <link rel="stylesheet" type="text/css" href="../../../Content/form.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="bg">
        <p class="margin"></p>
        <div class="reg-form" runat="server">
            <asp:Label ID="film" runat="server" Text=""></asp:Label>   
            <asp:Label ID="session_date" runat="server" Text="Date: "></asp:Label>
            <asp:Label ID="session_time" runat="server" Text="Time: "></asp:Label>
            <asp:Label ID="film_cost" runat="server" Text=""></asp:Label>
            <div class="plane">
                <div>
                    <h1 class ="header-text" style="font-size: 30px">Please select a seat</h1>
                </div>
                <ol>
                    <asp:Label ID="seats" runat="server" Text=""></asp:Label>
                </ol>
            </div>
            <asp:Label Style="font-size: 30px" ID="cost" runat="server" Text="Cost: 0"></asp:Label>
            <br /><br />
            <asp:Label ID="move" runat="server" Text="Label"></asp:Label>
            <br /><br /><br />
            <asp:HiddenField ID="HiddenField1" runat="server" OnValueChanged="HiddenField1_ValueChanged" />
        </div>
        <br />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">
    <script>
        function get()
        {
            var list = '';
            var isSelected = false;
            for (var i = 0; i < 30; i = i + 1) {
                list = list + document.getElementById(i + 1).checked + ",";
                if (document.getElementById(i + 1).checked) {
                    isSelected = true;
                }
            }
            if (!isSelected) {
                alert('Select at least one place');
            } else {
                var myHidden = document.getElementById('<%= HiddenField1.ClientID %>');
                myHidden.value = list;
            }
        }       
        function update_cost(e)
        {
            var cost_string = document.getElementById('<%= cost.ClientID %>');
            var film_cost = document.getElementById('<%= film_cost.ClientID %>');
            var sum = cost_string.innerHTML.split(':')[1];
            var cost = 0;
            if (e.id >= 1 && e.id <= 6)
            {
                cost = 10;
            }
            if (e.id >= 19 && e.id <= 24)
            {
                cost = 10;
            }
            if (e.id >= 25 && e.id <= 30)
            {
                cost = 20;
            }
            cost += parseInt(film_cost.innerHTML.split(':')[1]);
            if (e.checked)
            {
                sum = parseInt(sum) + cost;
            }
            else
            {
                sum = parseInt(sum) - cost;
            }           
            cost_string.innerHTML = "Cost: " + sum;
        }
    </script>
</asp:Content>
