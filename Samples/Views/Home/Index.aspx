<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: ViewBag.Message %></h2>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
    </p>


    <% using(Html.BeginForm("Create", "Home", FormMethod.Post, null)){ %>
        <input type="submit" value="Iniciar"/>
    <% } %>

    <% using(Html.BeginForm("Delete", "Home", FormMethod.Post, null)){ %>
        <input type="submit" value="Parar"/>
    <% } %>

    <% using(Html.BeginForm("Schedule", "Home", FormMethod.Post, null)){ %>
        Agendar para <input type="text" name="scheduleDate" value="<%= DateTime.Now.AddMinutes(1) %>"/>
        <input type="submit" value="Iniciar"/>
    <% } %>

</asp:Content>
