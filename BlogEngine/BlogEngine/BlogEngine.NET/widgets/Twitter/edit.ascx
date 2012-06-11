<%@ Control Language="C#" AutoEventWireup="true" CodeFile="edit.ascx.cs" Inherits="Widgets.Twitter.Edit" %>
<%@ Reference Control="~/widgets/Twitter/widget.ascx" %>

<label for="<%=txtAccount %>">Your Twitter account (without @)</label><br />
<asp:TextBox runat="server" ID="txtAccount" Width="300" />
<asp:RequiredFieldValidator runat="Server" ControlToValidate="txtAccount" ErrorMessage="Please enter a username" Display="dynamic" /><br /><br />

<label for="<%=txtHashtags %>">Hashtag (without #)</label><br />
<asp:TextBox runat="server" ID="txtHashtags" Width="300" />
<asp:RequiredFieldValidator runat="Server" ControlToValidate="txtHashtags" ErrorMessage="Please enter a tag" Display="dynamic" /><br /><br />

<label for="<%=txtTwits %>">Number of displayed Tweets</label><br />
<asp:TextBox runat="server" ID="txtTwits" Width="30" />
<asp:RequiredFieldValidator runat="Server" ControlToValidate="txtTwits" ErrorMessage="Please enter a number" Display="dynamic" />
<asp:CompareValidator runat="server" ControlToValidate="txtTwits" Type="Integer" Operator="dataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%=txtPolling %>">Polling Interval (every # minutes)</label><br />
<asp:TextBox ID="txtPolling" runat="server" Width="30" />
<asp:CompareValidator runat="server" ControlToValidate="txtPolling" Type="Integer" Operator="dataTypeCheck" ErrorMessage="A real number please" /><br /><br />

<label for="<%=txtFollowMe %>">Follow me text</label><br />
<asp:TextBox runat="server" ID="txtFollowMe" Width="300" />
<asp:RequiredFieldValidator runat="Server" ControlToValidate="txtFollowMe" ErrorMessage="Please enter a valid string" Display="dynamic" /><br /><br />
