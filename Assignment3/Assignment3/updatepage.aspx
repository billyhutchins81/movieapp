<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="updatepage.aspx.cs" Inherits="Assignment3.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h4>Billy</h4>
    <p>Add or Delete Movies</p>
    <div class="col3">
        <h3>Add a Movie</h3>
        <label>New Movie ID:</label>
        <input type="text" id ="mid" runat="server"  />
        <br />
        <label>New Movie Title:</label>
        <input type="text" id ="mtitle" runat="server"  />
        <br />
        <label>Release Date:</label>
        <input type="text" id ="rdate" runat="server"  />
        <br />
        <label>Select Director and Genre</label>
        <br />
        <h4>Director List</h4>
        <asp:ListBox ID="dList" runat="server"></asp:ListBox>
        <br /><br />
        <h4>Genre List</h4>
        <asp:ListBox ID="gList" runat="server"></asp:ListBox>
        <br /><br />
        <asp:Button ID="add" runat="server" Text="Add Movie" OnClick="add_Click" />
    </div>
    <div class="col3">
        <asp:DropDownList ID="deleteDD" runat="server" AutoPostBack="True" OnSelectedIndexChanged="deleteDD_SelectedIndexChanged"></asp:DropDownList>
        <br /><br />
         <asp:DetailsView ID="movieDetail" runat="server" Height="50px" Width="125px"></asp:DetailsView>
        <asp:Button ID="delete" runat="server" Text="Delete Movie" OnClick="delete_Click" />
    </div>
</asp:Content>
