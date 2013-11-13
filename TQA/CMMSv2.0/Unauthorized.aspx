<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Unauthorized.aspx.vb" Inherits="Unauthorized" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CMMS || Login</title>
    <meta http-equiv="CACHE-CONTROL" content="NO-STORE" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <meta http-equiv="PRAGMA" content="NO-CACHE" />
    <link rel="shortcut icon" href="Images/application.ico" />
    <link href="CSS/CMMSStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div class="header">
            <div class="header_interior">
                <div class="head_interior-left_column">
                    <img src="Images/MLBig.gif" alt="Logo" style="margin-right: 10px;" />
                </div>
            </div>
        </div>
        <div class=" content_body" style="height: 353px">
            <div id="content_interior2" style="height: 334px">
                <div class="menu-horizontal">
                    <div class="versionLogin">
                        CMMS Web Version 1.0
                    </div>
                </div>
                <div style="float: left; margin-top: 20px;">
                    <h2>
                        Unauthorized Access</h2>
                    <p style="margin-left: 50px;">
                        You have attempted to access a page that you are not authorized to view.
                    </p>
                    <p style="margin-left: 50px;">
                        Please contact the site administrator. 
                        <asp:LinkButton ID="lbLogin" runat="server">Login</asp:LinkButton>
                    </p>
                    <p style="margin-left: 50px;">
                        &nbsp;</p>
                </div>
            </div>
            <div id="footer">
                <div class="left">
                    &nbsp; M. Lhuillier Financial Services Inc.
                </div>
                <br class="doNotDisplay doNotPrint" />
                <div class="right">
                    All Rights Reserved. &nbsp;
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
