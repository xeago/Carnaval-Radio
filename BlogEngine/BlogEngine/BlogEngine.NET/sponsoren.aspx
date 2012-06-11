<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sponsoren.aspx.cs" Inherits="sponsoren" ValidateRequest="false" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphHeader" Runat="Server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="page">
      <div style="margin-top:60px;">
        <asp:Literal runat="server" ID="litSponsors" />
        <div></div>
      </div>
  </div>
</asp:Content>