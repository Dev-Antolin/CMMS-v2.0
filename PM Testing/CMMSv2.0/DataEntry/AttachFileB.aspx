<%@ Page Title="" Language="VB" MasterPageFile="~/LeftDEMainMasterPage.master" AutoEventWireup="false"
    CodeFile="AttachFileB.aspx.vb" Inherits="DataEntry_AttachFileB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="main-copy">
        <h1 id="introduction">
            Basic Data Attachment
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
