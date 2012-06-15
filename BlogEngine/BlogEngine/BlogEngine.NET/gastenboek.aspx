<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gastenboek.aspx.cs" Inherits="guestbook" ValidateRequest="false" %>
<%@ Import Namespace="BlogEngine.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphBody" Runat="Server">
  <div id="page">
        <asp:Label ID="lblResponse" runat="server" Width="496px"></asp:Label>
        <br/>
            <asp:Label runat="server" ID="lblVerifyCode" />
            <div style="width: 455px">
                    <div>
                        <div style="width: 195px">
                            <asp:Label ID="lblyourname" runat="server" Font-Bold="True">Your Name:</asp:Label></div>
                        <div style="width: 315px" align="left"><asp:TextBox ID="yourname" runat="server" Width="310px"></asp:TextBox></div>
                    </div>
                    <div>
                        <div style="width: 195px">
                            <asp:Label ID="lblyouremail" runat="server" Font-Bold="True">Your Email:</asp:Label></div>
                        <div style="width: 315px" align="left"><asp:TextBox ID="youremail" runat="server" Width="310px"></asp:TextBox></div>
                    </div>
                    <div>
                        <div style="width: 195px">
                            <asp:Label ID="lblyourmessage" runat="server" Font-Bold="True">Your Message:</asp:Label></div>
                        <div style="width: 315px" align="left"><asp:TextBox ID="yourmessage" runat="server" Height="142px" TextMode="MultiLine" Width="310px"></asp:TextBox></div>
                    </div>
                    <div>
                        <div colspan="2" align="right" style="height: 22px">
                            <asp:Button ID="btnAdd" runat="server" Height="20px" Text="Submit" 
                                Width="100px" BackColor="LightSteelBlue" Font-Bold="True" 
                                onclick="btnAdd_Click" /></div>
                    </div>
            </div>
    
    <div id="divForm" runat="server">
         <div>
                <asp:Label ID="MsgDisplay" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblNavigation" runat="server"></asp:Label>
         </div>
    </div>
  </div>
</asp:Content>