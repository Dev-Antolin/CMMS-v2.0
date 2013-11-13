<%@ Page Title="" Language="VB" MasterPageFile="~/LeftDEMainMasterPage.master" AutoEventWireup="false"
    CodeFile="CAAddDevice.aspx.vb" Inherits="EntryData_CAAddDevice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="Custom" Namespace="ENTech.WebControls" Assembly="AutoSuggestMenu" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/Division.ascx" TagName="DIV" TagPrefix="UC" %>
<%@ Register Src="../UserControl/Branch.ascx" TagName="Branch" TagPrefix="ACT" %>
<%@ Register Src="../UserControl/UsersID.ascx" TagName="UsersID" TagPrefix="CIT" %>
<%@ Register Src="../UserControl/UsersIDD.ascx" TagName="UsersIDD" TagPrefix="USJR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">

    <script type="text/javascript">
        $(function() {

            var limit = 500;

            $('#<%=txtAAstDescription.ClientID %>').live('keyup', function() {
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

    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel runat="server" ID="UpdateFace" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Computer Asset Additional Devices</h1>
                <asp:Panel ID="PanelACategory" runat="server" GroupingText="Computer Assignment"
                    Width="631px">
                    <table width="100%">
                        <tr>
                            <td align="right" style="width: 144px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbADivision" runat="server" Text="Division" AutoPostBack="True" />
                                &nbsp;<asp:RadioButton ID="rbABranch" runat="server" Text="Branch" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblABCCode" runat="server" Text="Division Code"></asp:Label>
                                &nbsp;<asp:Label ID="Label15" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtACode" runat="server" Width="40px" ReadOnly="True"></asp:TextBox>
                                <asp:ImageButton ID="lbASearchCode" CssClass="ButtonSearch1" runat="server" ImageUrl="~/Images/SearchButton.JPG" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblABCName" runat="server" Text="Division Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtACodeName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label13" runat="server" Text="ID No."></asp:Label>
                                &nbsp;<asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAID" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                <asp:ImageButton ID="lbASearchID" CssClass="ButtonSearch2" runat="server" ImageUrl="~/Images/SearchButton.JPG" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label14" runat="server" Text="Employee Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label18" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAIDName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="PanelAInformation" runat="server" GroupingText="Asset Information"
                    Width="631px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label23" runat="server" Text="Device Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label24" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDeviceName" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                    Width="224px">
                                    <asp:ListItem Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtADeviceName" CssClass="txtUppercase WriteBackColor" runat="server"
                                    Width="79px" MaxLength="20"></asp:TextBox>
                                <asp:CompareValidator ID="cvDevName" runat="server" ControlToValidate="ddlDeviceName" ErrorMessage="Device name is required" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="submit" />                                    
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtADeviceName_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtADeviceName" FilterType="UppercaseLetters,LowercaseLetters,Custom"
                                    ValidChars=" .">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label4" runat="server" Text="Asset Inventory No."></asp:Label>
                                &nbsp;<asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAAsstInvNo" CssClass="ReadOnlyBackColor txtUppercase" runat="server"
                                    Width="144px" MaxLength="18"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAAsstInvNo_FilteredTextBoxExtender1"
                                    runat="server" Enabled="True" TargetControlID="txtAAsstInvNo" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="-">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtAPTPNo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    MaxLength="8"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqPTPNo" ControlToValidate="txtAPTPNo"
                                    ValidationGroup="submit" ErrorMessage="PTP # is Required" runat="server" 
                                    SetFocusOnError="true" Width="176px" />                                    
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAPTPNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtAPTPNo" FilterType="Custom,Numbers" ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label30" runat="server" Text="Description"></asp:Label>&nbsp;
                                <asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAAstDescription" runat="server" Height="136px" TextMode="MultiLine"
                                    Width="440px"></asp:TextBox>
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAAstDescription_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtAAstDescription" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars="=/- .">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px;">
                                &nbsp;
                            </td>
                            <td align="left">
                            <asp:RequiredFieldValidator ID="reqAsetDescription" ControlToValidate="txtAAstDescription"
                                 ValidationGroup="submit" ErrorMessage=" Asset Description is Required" runat="server" 
                                 SetFocusOnError="true"/>                                                                                                   
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCountWord" runat="server" Text="500/500" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px;">
                                <asp:Label ID="Label5" runat="server" Text="Serial No."></asp:Label>
                                &nbsp;<asp:Label ID="Label8" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtASerialNo" runat="server" CssClass="WriteBackColor txtUppercase"
                                    MaxLength="30" Width="220px"></asp:TextBox>
<%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtASerialNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtASerialNo" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label6" runat="server" Text="Attachment"></asp:Label>
                                &nbsp;<asp:Label ID="Label9" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbAScan" runat="server" AutoPostBack="True" Text="Scan File" />
                                &nbsp;<asp:RadioButton ID="rbAAttach" runat="server" AutoPostBack="True" Text="Attach File" />
                                <asp:Repeater ID="attList" runat="server">
                                    <ItemTemplate>
                                        <br />
                                        <asp:LinkButton ID="lnkRemove" runat="server" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "rattchfilename")%>'
                                            OnCommand="RemoveAtt" Text="[remove]" />
                                        <asp:Label ID="attchmnt" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "File_Name")%>' />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px" align="right">
                                <asp:Label ID="Label20" runat="server" Text="P.O Number"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPONo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    MaxLength="8"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAPONo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtAPONo" FilterType="Custom,Numbers" ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="ReqPONo" ControlToValidate="txtAPONo" ValidationGroup="submit"
                                    ErrorMessage=" P.O # is required" runat="server" SetFocusOnError="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label25" runat="server" Text="Delivery Date"></asp:Label>
                                &nbsp;<asp:Label ID="Label28" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtADelDate" CssClass="ReadOnlyBackColor" runat="server" Width="100px"
                                    MaxLength="10" ReadOnly="True"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtADelDate_CalendarExtender" runat="server" Enabled="True"
                                    PopupPosition="Right" TargetControlID="txtADelDate" Format="yyyy-MM-dd">
                                </ajaxToolkit:CalendarExtender>
                                <asp:RequiredFieldValidator ID="reqDelDate" ControlToValidate="txtADelDate" ValidationGroup="submit"
                                    ErrorMessage="Delivery Date is required" runat="server" 
                                    SetFocusOnError="true"/>
                                <asp:HiddenField ID="HFReqNeedDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px" align="right">
                                <asp:Label ID="Label31" runat="server" Text="Active"></asp:Label>
                                &nbsp;<asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbAActive" runat="server" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lblErrorSaving" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnASave" runat="server" Text="Save"  CausesValidation="true" ValidationGroup="submit"  />
                                &nbsp;<asp:Button ID="btnANew" runat="server" Text="OK" />
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
            <ajaxToolkit:ModalPopupExtender ID="MPEDivison2" runat="server" PopupControlID="divPopUpDiv"
                TargetControlID="btnDivision" PopupDragHandleControlID="panelDragHandleDiv"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEBranch" runat="server" PopupControlID="divPopUpBranch"
                TargetControlID="btnBranch"  PopupDragHandleControlID="panelDragHandleBranch"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEUsersID" runat="server" PopupControlID="divPopUpUserID"
                TargetControlID="btnUsersID"  PopupDragHandleControlID="panelDragHandleUserID"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <ajaxToolkit:ModalPopupExtender ID="MPEUsersIDD" runat="server" PopupControlID="divPopUpUserIDD"
                TargetControlID="btnUsersIDD"  PopupDragHandleControlID="panelDragHandleUserIDD"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Button ID="btnDivision" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnBranch" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnUsersID" runat="server" Text="Button" Style="display: none;" />
            <asp:Button ID="btnUsersIDD" runat="server" Text="Button" Style="display: none;" />
            <div id="divPopUpDiv" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleDiv" CssClass="drag">
                    <div class="gridContainer">
                        <UC:DIV ID="DIV2" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpBranch" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleBranch" CssClass="drag">
                    <div class="gridContainer">
                        <ACT:Branch ID="Branch" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose2" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpUserID" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleUserID" CssClass="drag">
                    <div class="gridContainer">
                        <CIT:UsersID ID="UsersID" runat="server" />
                    </div>
                    <div class="closeContainer">
                        <div class="closeButton">
                            <asp:Button ID="btnClose3" runat="server" Text="Close" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
            <div id="divPopUpUserIDD" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleUserIDD" CssClass="drag">
                    <div class="gridContainer">
                        <USJR:UsersIDD ID="UsersIDD" runat="server" />
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
