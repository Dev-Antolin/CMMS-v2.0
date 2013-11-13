<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWORABLRMainMasterPage.master" AutoEventWireup="false"
    CodeFile="AttachFileWORABLR.aspx.vb" Inherits="WorkOrder_AttachFileWORABLR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CMMSRABLRMainContentPlaceHolder" runat="Server">
    <div id="main-copy">
        <h1 id="introduction">
            Work Order Attachment
        </h1>
        <br />
        <asp:Panel ID="PanelAttachFile" runat="server" GroupingText="Attachment" Width="485px"
            TabIndex="7">
            <asp:FileUpload ID="fuAttachFile" runat="server" class="multi" maxlength="5" />
            <table style="width: 100%">
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnAttachFile" runat="server" Text="Attach File(s)" />
                        &nbsp;<asp:Button ID="btnBack" runat="server" Text="Back" />
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
