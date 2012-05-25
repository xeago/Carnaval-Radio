<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AudioService.aspx.cs" Inherits="AudioService_AudioService" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
    <title></title>
</head>
<body>
    <div>
        <button id="Button1" onclick="GetStuff()">
            DERP</button>
    </div>
    <script type="text/javascript">
    function GetStuff() {
        alert("doing stuff");


        $.ajax({ type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "http://localhost:52457/BlogEngine.NET/api/WebService.asmx/GetAudioStreams",
            data: "{}",
            dataType: "json",
            succes: function (msg) 
            {
                // Hide the fake progress indicator graphic.
                alert(msg);    
            }
        });

            

        }
  
    </script>
</body>
</html>
