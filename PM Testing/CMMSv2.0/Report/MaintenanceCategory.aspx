<%@ Page Title="" Language="VB" MasterPageFile="~/LeftRPTMainMasterPage.master" AutoEventWireup="false"
    CodeFile="MaintenanceCategory.aspx.vb" Inherits="Report_MaintenanceCategory" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Maintenance History</h1>
                <br />
                <center>
                    <asp:Panel ID="PanelMAProfile" runat="server" GroupingText="Asset Maintenance Category"
                        Width="485px" TabIndex="7">
                        <br />
                        <br />
                        <asp:Button ID="btnMADivision" runat="server" Text="Division Asset Maintenance History"
                            Width="310px" />
                        <br />
                        <asp:Button ID="btnMABranch" runat="server" Text="Branch Asset Maintenance History"
                            Width="310px" />
                        <br />
                        <br />
                        <br />
                        <br />
                    </asp:Panel>
                </center>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
