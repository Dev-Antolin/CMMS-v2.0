<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWOMainMasterPage.master" AutoEventWireup="false"
    CodeFile="CloseDetail.aspx.vb" Inherits="WorkOrder_CloseDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Closed Work Order
                </h1>
                <asp:Panel ID="PanelAuthorDetail" runat="server" GroupingText="Branch Information"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBCodeAuthor" runat="server"></asp:Label>
                                &nbsp;<asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBCodeAuthor" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBNameAuthor" runat="server"></asp:Label>
                                &nbsp;<asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBNameAuthor" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label12" runat="server" Text="ID No."></asp:Label>
                                &nbsp;<asp:Label ID="Label13" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAuthorID" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label15" runat="server" Text="Employee Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label22" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAuthorName" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelWorkOrderInfo" runat="server" GroupingText="Work Order Information"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label23" runat="server" Text="Work Order No."></asp:Label>
                                &nbsp;<asp:Label ID="Label24" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkOrderNo" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label42" runat="server" Text="Escalation"></asp:Label>
                                &nbsp;<asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEscalationName" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label43" runat="server" Text="Work Order Type"></asp:Label>
                                &nbsp;<asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkOrderType" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label25" runat="server" Text="Asset Inventory No."></asp:Label>
                                &nbsp;<asp:Label ID="Label26" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAssetInventory" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label28" runat="server" Text="Date"></asp:Label>
                                &nbsp;<asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDate" CssClass="ReadOnlyBackColor" runat="server" Width="100px"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label30" runat="server" Text="IR No."></asp:Label>
                                &nbsp;<asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIRNumber" CssClass="ReadOnlyBackColor" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label33" runat="server" Text="Description"></asp:Label>
                                &nbsp;<asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="7">
                                <asp:TextBox ID="txtWorkOrderDesc" CssClass="ReadOnlyBackColor" runat="server" Height="160px"
                                    TextMode="MultiLine" Width="430px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px" align="right">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label40" runat="server" Text="Attachment"></asp:Label>
                                &nbsp;<asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:Repeater ID="attList" runat="server">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="attchmnt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "File_Name")%>'
                                            CommandArgument='<%#DataBinder.Eval(Container.DataItem, "File_Name")%>' Height="0" />
                                        <br />
                                        <br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelWorkManagement" runat="server" GroupingText="Work Management"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td rowspan="9">
                                <asp:TextBox ID="txtAssessmentHist" CssClass="ReadOnlyBackColor" runat="server" Height="196px"
                                    TextMode="MultiLine" Width="430px" ReadOnly="True"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelServCallTicket" runat="server" GroupingText="Service Call Ticket"
                    Width="600px" Font-Names="Arial">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px" align="right">
                                &nbsp;
                                <asp:Label ID="Label46" runat="server" Text="Corrective Action"></asp:Label>
                                &nbsp;<asp:Label ID="Label47" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="9">
                                <asp:TextBox ID="txtSCTCorect" CssClass="ReadOnlyBackColor" runat="server" Height="196px"
                                    TextMode="MultiLine" Width="430px" ReadOnly="True"></asp:TextBox>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelAssessment" runat="server" GroupingText="Assessment" 
                    Width="600px" Font-Names="Arial">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px" align="right">
                                &nbsp; &nbsp;
                            </td>
                            <td rowspan="8">
                                <asp:TextBox ID="txtAssessment" CssClass="ReadOnlyBackColor" runat="server" Height="196px"
                                    TextMode="MultiLine" Width="430px" ReadOnly="True" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px; height: 25px;">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="btnDownload" runat="server" OnClick="btnDownload_Click" Text="Download"
        Style="display: none;" />
    <div id="CloseContent">
        <a href="javascript:PrintThisPage();" style="font-size: smaller; text-decoration: underline;
            margin-left: 650px">Print</a>
    </div>
</asp:Content>
