<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWOMainMasterPage.master" AutoEventWireup="false"
    CodeFile="ReceivedDetail.aspx.vb" Inherits="WorkOrder_ReceivedDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="Custom" Namespace="ENTech.WebControls" Assembly="AutoSuggestMenu" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/DivisionDesignation.ascx" TagName="DIGD" TagPrefix="DIGDIV" %>
<%@ Register Src="../UserControl/RegionDesignation.ascx" TagName="DIGR" TagPrefix="DIGREG" %>
<%@ Register Src="../UserControl/LPTDesignation.ascx" TagName="DIGL" TagPrefix="DIGLPT" %>
<%@ Register Src="../UserControl/RCTDesignation.ascx" TagName="DIGT" TagPrefix="DIGRCT" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">

    <script type="text/javascript">
        $(function() {

            var limit = 500;

            $('#<%=txtAssessment.ClientID %>').live('keyup', function() {
                var len = $(this).val().length;
                if (len >= limit) {
                    this.value = this.value.substring(0, limit);
                    $("#<%= lblCount.ClientID %>").css('color', 'red');
                    $("#<%= lblCount.ClientID %>").text("0/500");
                } else {
                    $("#<%= lblCount.ClientID %>").text(limit - len + "/500");
                    $("#<%= lblCount.ClientID %>").css('color', 'blue');
                }
            });
        });
    </script>

    <script type="text/javascript">
        $(function() {

            var limit = 500;

            $('#<%=txtAssessmentHist.ClientID %>').live('keyup', function() {
                var len = $(this).val().length;
                if (len >= limit) {
                    this.value = this.value.substring(0, limit);
                    $("#<%= lblCountHist.ClientID %>").css('color', 'red');
                    $("#<%= lblCountHist.ClientID %>").text("0/500");
                } else {
                    $("#<%= lblCountHist.ClientID %>").text(limit - len + "/500");
                    $("#<%= lblCountHist.ClientID %>").css('color', 'blue');
                }
            });
        });
    </script>

    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    <asp:LinkButton ID="lbReturn" runat="server" Style="color: #006699;">Received Work Order</asp:LinkButton>
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
                                <asp:DropDownList ID="ddlEscalateTo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlDesignationTo" CssClass="WriteBackColor" runat="server"
                                    Width="215px" AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:ImageButton ID="lbAssignToSrch" CssClass="ButtonSearchRegion" runat="server"
                                    ImageUrl="~/Images/SearchButton2.JPG" />
                                <br />
                                <asp:Label ID="lblWOSelectEscal" runat="server" Font-Bold="True"></asp:Label>
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
                <asp:Panel ID="PanelWorkManageHistory" runat="server" GroupingText="Work Management History"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtWorkManagementHist" CssClass="ReadOnlyBackColor" runat="server"
                                    Height="160px" TextMode="MultiLine" Width="430px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelAssessmentHistory" runat="server" GroupingText="Work Management"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAssessmentHist" CssClass="WriteBackColor" runat="server" Height="160px"
                                    TextMode="MultiLine" Width="430px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td align="right">
                                <asp:Label ID="lblCountHist" runat="server" Text="500/500" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                            </td>
                            <td style="width: 143px">
                                <asp:Button ID="btnSaveAst" runat="server" Text="Escalate" Width="80px" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelAssessment" runat="server" GroupingText="Service Call Ticket"
                    Width="600px">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 143px" align="right">
                                &nbsp;
                                <asp:Label ID="Label46" runat="server" Text="Corrective Action"></asp:Label>
                                &nbsp;<asp:Label ID="Label47" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="7">
                                <asp:TextBox ID="txtAssessment" CssClass="WriteBackColor" runat="server" Height="160px"
                                    TextMode="MultiLine" Width="430px" MaxLength="300">Corrective Action :</asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtAssessment_TextBoxWatermarkExtender"
                                    runat="server" Enabled="True" TargetControlID="txtAssessment" WatermarkCssClass="watermarked"
                                    WatermarkText="Corrective Action :">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAssessment_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtAssessment" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars=". =/">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
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
                            <td align="right">
                                <asp:Label ID="lblCount" runat="server" Text="500/500" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnCloseAst" runat="server" Text="Resolve" Width="80px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblWOError" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnWOOk" runat="server" Text="OK" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="MPEDesignation" runat="server" PopupControlID="divPopUpDesig"
                TargetControlID="btnDesignation" OkControlID="btnClose" PopupDragHandleControlID="panelDragHandleDiv"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEDesignation2" runat="server" PopupControlID="divPopUpDesig2"
                TargetControlID="btnDesignation2" OkControlID="btnClose2" PopupDragHandleControlID="panelDragHandleReg"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEDesignation3" runat="server" PopupControlID="divPopUpDesig3"
                TargetControlID="btnDesignation3" OkControlID="btnClose3" PopupDragHandleControlID="panelDragHandleLPT"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEDesignation4" runat="server" PopupControlID="divPopUpDesig4"
                TargetControlID="btnDesignation4" OkControlID="btnClose4" PopupDragHandleControlID="panelDragHandleRCT"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnDesignation" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnDesignation2" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnDesignation3" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnDesignation4" runat="server" Text="Button" Style="display: none;" />
            <div id="divPopUpDesig" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleDiv" CssClass="drag">
                    <div class="gridContainer">
                        <DIGDIV:DIGD ID="DIG" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpDesig2" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleReg" CssClass="drag">
                    <div class="gridContainer">
                        <DIGREG:DIGR ID="DIG2" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose2" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpDesig3" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleLPT" CssClass="drag">
                    <div class="gridContainer">
                        <DIGLPT:DIGL ID="DIG3" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose3" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpDesig4" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleRCT" CssClass="drag">
                    <div class="gridContainer">
                        <DIGRCT:DIGT ID="DIG4" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose4" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="btnDownload" runat="server" OnClick="btnDownload_Click" Text="Download"
        Style="display: none;" />
</asp:Content>
