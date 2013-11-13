<%@ Control Language="VB" AutoEventWireup="false" CodeFile="AttachFile.ascx.vb" Inherits="UserControl_AttachFile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<link href="../CSS/ModalPopUpExtender.css" rel="stylesheet" type="text/css" />
<div class="GVtxtSearch">
    <%--<asp:Button ID="btnSearchBranch" runat="server" Text="Search" Font-Names="Tahoma"
        OnClick="btnSearchBranch_Clicked" />--%>
</div>
<div class="GVData">
<asp:FileUpload ID="fuSample" runat="server" class="multi" maxlength="5" />
</div>
