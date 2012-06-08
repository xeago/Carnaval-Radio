<%@ Page Language="C#" AutoEventWireup="true" CodeFile="foto_album.aspx.cs" Inherits="fotoalbum" ValidateRequest="false" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:content id="Content2" contentplaceholderid="cphHeader" runat="Server">
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js" type="text/javascript"></script>
</asp:content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="page">
  <div style="margin-top:60px;">
    <div class="BreadCrumb"><a href="foto_albums.aspx">Terug naar albums</a></div>
    <asp:Literal runat="server" ID="litAlbums" />

    <div>
    </div>
  </div>
  </div>
</asp:Content>