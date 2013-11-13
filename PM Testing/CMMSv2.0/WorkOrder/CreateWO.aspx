<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWOMainMasterPage.master" AutoEventWireup="false"
    CodeFile="CreateWO.aspx.vb" Inherits="WorkOrder_CreateWO" %>

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

            $('#<%=txtWODesc.ClientID %>').live('keyup', function() {
                var len = $(this).val().length;
                if (len >= limit) {
                    this.value = this.value.substring(0, limit);
                    $("#<%= lblCountWord.ClientID %>").css('color', 'red');
                    $("#<%= lblCountWord.ClientID %>").text("0/500");
                } else {
                    $("#<%= lblCountWord.ClientID %>").text(limit - len + "/500");
                    $("#<%= lblCountWord.ClientID %>").css('color', 'blue');
                }
            });
        });
    </script>

    <script type="text/javascript">
        function disableEnterKey(e) {
            var key;
            if (window.event)
                key = window.event.keyCode;     //IE
            else
                key = e.which;     //firefox

            if (key == 13)
                return false;
            else
                return true;
        }
    </script>

    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Create Work Order</h1>
                <asp:Panel ID="PanelAuthorInfo" runat="server" GroupingText="Division Information"
                    Width="631px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBCodeAuthor" runat="server"></asp:Label>
                                &nbsp;<asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOCode" CssClass="ReadOnlyBackColor" onKeyPress="return disableEnterKey(event)"
                                    runat="server" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBNameAuthor" runat="server"></asp:Label>
                                &nbsp;<asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOName" CssClass="ReadOnlyBackColor" runat="server" onKeyPress="return disableEnterKey(event)"
                                    Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label11" runat="server" Text="ID No."></asp:Label>
                                &nbsp;<asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOIDNo" CssClass="ReadOnlyBackColor" onKeyPress="return disableEnterKey(event)"
                                    runat="server" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label18" runat="server" Text="Employee Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOEmpName" CssClass="ReadOnlyBackColor" onKeyPress="return disableEnterKey(event)"
                                    runat="server" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelWorkOrderInfo" runat="server" GroupingText="Work Order Information"
                    Width="631px">
                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label4" runat="server" Text="Work Order No."></asp:Label>
                                &nbsp;<asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWONo" CssClass="ReadOnlyBackColor" onKeyPress="return disableEnterKey(event)"
                                    runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px;">
                                <asp:Label ID="Label39" runat="server" Text="Escalation"></asp:Label>
                                &nbsp;<asp:Label ID="Label38" runat="server" Text=":"></asp:Label>
                            </td>
                            <td style="height: 22px;">
                                <asp:DropDownList ID="ddlWOEscal" CssClass="WriteBackColor" runat="server" Width="110px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlDesignation" CssClass="WriteBackColor" runat="server" Width="215px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:ImageButton ID="lbWOSrchEscal" CssClass="ButtonSearchRegion" runat="server"
                                    ImageUrl="~/Images/SearchButton2.jpg" TabIndex="1" />
                                <br />
                                <asp:Label ID="lblWOSelectEscal" runat="server" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label40" runat="server" Text="Work Order Type"></asp:Label>
                                &nbsp;<asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlWOType" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                    Width="147px">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtWOTypeOthers" CssClass="txtUppercase WriteBackColor" runat="server"
                                    onKeyPress="return disableEnterKey(event)" Width="270px" MaxLength="30"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtWOTypeOthers_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtWOTypeOthers" FilterType="UppercaseLetters,LowercaseLetters,Custom"
                                    ValidChars=" .">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label5" runat="server" Text="Asset Inventory No."></asp:Label>
                                &nbsp;<asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOAsstInvNo" CssClass="txtUppercase WriteBackColor" runat="server"
                                    onKeyPress="return disableEnterKey(event)" MaxLength="6"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtWOAsstInvNo_TextBoxWatermarkExtender" 
                                    runat="server" Enabled="True" TargetControlID="txtWOAsstInvNo" WatermarkCssClass="watermarked"
                                    WatermarkText="000000">
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbeAssetInvNo" runat="server" TargetControlID="txtWOAsstInvNo"
                                    FilterType="Custom,Numbers" ValidChars="" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Incorrect format! 6 number only. e.g: 000000"
                                    ControlToValidate="txtWOAsstInvNo" ValidationExpression="^\d{6}$" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label34" runat="server" Text="Date"></asp:Label>
                                &nbsp;<asp:Label ID="Label35" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOStartDate" CssClass="ReadOnlyBackColor" runat="server" Width="100px"
                                    ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label36" runat="server" Text="IR No."></asp:Label>
                                &nbsp;<asp:Label ID="Label37" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWOIRNo" CssClass="txtUppercase WriteBackColor" runat="server" onKeyPress="return disableEnterKey(event)"
                                    MaxLength="16"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtWOIRNo_TextBoxWatermarkExtender" 
                                    runat="server" Enabled="True" TargetControlID="txtWOIRNo" WatermarkCssClass="watermarked"
                                    WatermarkText="01-000-0000-0000" >
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <ajaxToolkit:FilteredTextBoxExtender ID="ftbeIRNo" runat="server" TargetControlID="txtWOIRNo"
                                    FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers" ValidChars="-" />
                                <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Incorrect format! RegionCode-BcCode-yyMM-0000"
                                    ControlToValidate="txtWOIRNo" ValidationExpression="^\d{2}-[0-9]{2}[0-9a-zA-Z]{1}-\d{4}-\d{4}$" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label20" runat="server" Text="Description"></asp:Label>
                                &nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="7">
                                <asp:TextBox ID="txtWODesc" runat="server" Height="160px" TextMode="MultiLine" Width="440px"
                                    MaxLength="300"></asp:TextBox>
<%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtWODesc_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtWODesc" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars=" ./-:">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
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
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px; text-align: right; vertical-align: top;">
                                &nbsp;
                            </td>
                            <td align="right">
                                <asp:Label ID="lblCountWord" runat="server" Text="500/500" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label27" runat="server" Text="Attachment"></asp:Label>
                                &nbsp;<asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbAttachFile" runat="server" AutoPostBack="True" Text="Attach File" />
                                <asp:Repeater ID="attList" runat="server">
                                    <ItemTemplate>
                                        <br />
                                        <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "rattchfilename")%>'
                                            OnCommand="RemoveAtt" Text="[remove]" />
                                        <asp:Label ID="attchmnt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "File_Name")%>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--                                <input id="AddFile" runat="server" onclick="AddFileUpload()" type="button" value="Add File(s)" />
                                &nbsp;<div id="FileUploadContainer">
                                    <!--FileUpload Controls will be added here -->
                                </div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblPromtError" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnWOSave" runat="server" Text="Save" />
                                &nbsp;<asp:Button ID="btnWOOk" runat="server" Text="OK" />
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
</asp:Content>
