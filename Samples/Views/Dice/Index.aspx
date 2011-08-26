<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Rolagem de dados
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Rolagem de dados</h2>

<% if (ViewBag.DiceResult != null){ %>
    Resultado dos dados: <%= ViewBag.DiceResult%>
<% } %>

<p>
    <% using(Html.BeginForm("Show", "Dice", FormMethod.Post, null)){ %>
        1D<input type="text" name="faces" value="6" />
        <input type="submit" value="Rolar dados"/>
    <% } %>
</p>

</asp:Content>
