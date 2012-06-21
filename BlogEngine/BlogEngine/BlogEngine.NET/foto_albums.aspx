<%@ Page Language="C#" AutoEventWireup="true" CodeFile="foto_albums.aspx.cs" Inherits="fotoalbums" ValidateRequest="false" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="page">
  <div id="foto">
        <div><h1><%= Resources.labels.fotos %></h1></div>
        <asp:Literal runat="server" ID="litAlbums" />
  </div>
  </div>
</asp:Content>