<%@ Page Language="C#" AutoEventWireup="true" CodeFile="guestbook.aspx.cs" Inherits="guestbook" ValidateRequest="false" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="page">
        <div id="divForm" runat="server">
         <div>
                <asp:Label ID="MsgDisplay" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblNavigation" runat="server"></asp:Label>
         </div>
    </div>
  </div>
</asp:Content>