<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="selectionpage.aspx.cs" Inherits="Assignment3.WebForm3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h4>Billy</h4>
    <p>Select genre to view</p>
    <div class="left">
        <h3>Pick a Genre:</h3>
        <asp:RadioButtonList ID="genreList" runat="server" AutoPostBack="true">

        </asp:RadioButtonList>
        <br /><br />
        <asp:GridView ID="genreGrid" runat="server"></asp:GridView>
        <br /><br />
        <asp:Button ID="genrePick" runat="server" Text="Show Movies" OnClick="genrePick_Click" />
    </div>
    <div class="right">
        <asp:DropDownList ID="directorDD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="directorDD_SelectedIndexChanged"></asp:DropDownList>
        <br /><br />
        <asp:GridView ID="directorGrid" runat="server"></asp:GridView>
    </div>
</asp:Content>
