<%@ Page Title="" Language="VB" MasterPageFile="~/LeftDEMainMasterPage.master" AutoEventWireup="false"
    CodeFile="CABasicData.aspx.vb" Inherits="EntryData_CABasicData" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="Custom" Namespace="ENTech.WebControls" Assembly="AutoSuggestMenu" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/Division.ascx" TagName="DIV" TagPrefix="UC" %>
<%@ Register Src="../UserControl/Branch.ascx" TagName="Branch" TagPrefix="ACT" %>
<%@ Register Src="../UserControl/UsersID.ascx" TagName="UsersID" TagPrefix="CIT" %>
<%@ Register Src="../UserControl/UsersIDD.ascx" TagName="UsersIDD" TagPrefix="USJR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">

    <script type="text/javascript" language="javascript">
        function doPostback() {
            $get('<%= Button1.ClientID %>').click();
        }
//        function NumberDOT(evt)
//        {
//          var charCode = (evt.which) ? evt.which : event.keyCode;          
//          var d = document.getElementById('<%=txtIP.ClientID%>').value.split(".").length - 1;          
//          if(charCode == 46)
//          {
//            if(d > 2)
//            {              
//               return false
//            }                 
//          }                  
//          if (charCode != 46 && charCode > 31
//            && (charCode < 48 || charCode > 57))
//          {       
//                return false;              
//          }
//          return true;
//        }
    </script>

    <script type="text/javascript">
        $(function() {

            var limit = 500;

            $('#<%=txtAstDescription.ClientID %>').live('keyup', function() {
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
    
    <script type="text/javascript" language="javascript">

    </script>
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />           
    <asp:UpdatePanel ID="UpdateFace" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <div id="main-copy">
                <h1 id="introduction">
                    Computer Asset Basic Data</h1>
                <asp:Panel ID="PanelCategory" runat="server" GroupingText="Computer Assignment" Width="631px"
                    TabIndex="1">
                    <table style="width: 101%">
                        <tr>
                            <td align="right" style="width: 144px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbDivision" runat="server" Text="Division" 
                                    AutoPostBack="True" Checked="True" />
                                &nbsp;<asp:RadioButton ID="rbBranch" runat="server" Text="Branch" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="lblBCCode" runat="server" Text="Division Code"></asp:Label>
                                &nbsp;<asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="40px" ReadOnly="True"></asp:TextBox>
                                <asp:ImageButton ID="lbASearchCode" CssClass="ButtonSearch1" UseSubmitBehavior="false"
                                    runat="server" ImageUrl="~/Images/SearchButton.JPG" TabIndex="10"/>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="lblBCName" runat="server" Text="Division Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCodeName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label11" runat="server" Text="ID No."></asp:Label>
                                &nbsp;<asp:Label ID="Label17" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtID" runat="server" Width="80px" ReadOnly="True"></asp:TextBox>
                                <asp:ImageButton ID="lbASearchID" CssClass="ButtonSearch2" UseSubmitBehavior="false"
                                    runat="server" ImageUrl="~/Images/SearchButton.JPG" TabIndex="11" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label18" runat="server" Text="Employee Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIDName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                </asp:Panel>
                <asp:Panel ID="PanelInformation" runat="server" GroupingText="Asset Information"
                    Width="634px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label4" runat="server" Text="Computer Type"></asp:Label>
                                &nbsp;<asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                                <br />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompType" CssClass="WriteBackColor" runat="server" AutoPostBack="True"
                                    Width="224px">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>DESKTOP</asp:ListItem>
                                    <asp:ListItem>NOTEBOOK</asp:ListItem>
                                    <asp:ListItem>SERVER</asp:ListItem>
                                </asp:DropDownList>                               
                                <asp:RequiredFieldValidator ID="reqCompType" ControlToValidate="ddlCompType" ValidationGroup="submit"
                                 ErrorMessage="Select Computer Type" runat="server" SetFocusOnError="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label5" runat="server" Text="Asset Inventory No."></asp:Label>
                                &nbsp;<asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAsstInvNo" CssClass="txtUppercase ReadOnlyBackColor" runat="server"
                                    Width="144px" MaxLength="18"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAsstInvNo_FilteredTextBoxExtender1" runat="server"
                                    Enabled="True" TargetControlID="txtAsstInvNo" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="-">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:TextBox ID="txtPTPNo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    MaxLength="10"  AutoCompleteType="None"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPTPNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtPTPNo" FilterType="Custom,Numbers" ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="requiredAssetInvNo" ControlToValidate="txtPTPNo"
                                    ValidationGroup="submit" ErrorMessage="PTP # is Required" runat="server" 
                                    SetFocusOnError="true" Width="176px" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label20" runat="server" Text="Description"></asp:Label>
                                &nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="6" style="width: 246px">
                                <asp:TextBox ID="txtAstDescription" runat="server" Height="122px" TextMode="MultiLine"
                                    Width="440px" ></asp:TextBox>      
                                                              
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAstDescription_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtAstDescription" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="=/- .">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                        
                            <td align="right" style="width: 144px">
                                &nbsp;
                            </td>                            
                            <td align="left">
                                <asp:RequiredFieldValidator ID="reqAsetDescription" ControlToValidate="txtAstDescription"
                                 ValidationGroup="submit" ErrorMessage=" Asset Description is Required" runat="server" 
                                 SetFocusOnError="true"/>                                                                                                   
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCountWord" runat="server" ForeColor="Blue" Text="500/500"></asp:Label>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label22" runat="server" Text="CPU"></asp:Label>
                                &nbsp;<asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCPU" runat="server" CssClass="WriteBackColor" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem  Value="0" Text=""></asp:ListItem>
                                </asp:DropDownList>                                                                                               
                                <asp:TextBox ID="txtCPUOthers" runat="server" CssClass="txtUppercase WriteBackColor"
                                    MaxLength="10" Width="160px"  AutoCompleteType="None" ></asp:TextBox>                                                                                                      
                                                                                                            
                                <asp:CompareValidator ID="cv" runat="server" ControlToValidate="ddlCPU" ErrorMessage="CPU Type is required" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="submit"  />                                                                    
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtCPUOthers_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    TargetControlID="txtCPUOthers" ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label24" runat="server" Text="Memory Size"></asp:Label>
                                &nbsp;<asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMemoSize" CssClass="WriteBackColor" runat="server" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem  Value="0" Text=""></asp:ListItem>
                                </asp:DropDownList>                                                                 
                                <asp:TextBox ID="txtMemoSizeOthers" CssClass="txtUppercase WriteBackColor"
                                    runat="server" Width="160px" MaxLength="7" AutoCompleteType="None" ></asp:TextBox>
                                <asp:CompareValidator ID="cvMemory" runat="server" ControlToValidate="ddlMemoSize" ErrorMessage="Memory Type is required" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="submit" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtMemoSizeOthers_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtMemoSizeOthers" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label25" runat="server" Text="Hard Disk"></asp:Label>
                                &nbsp;<asp:Label ID="Label30" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlHardDisk" CssClass="WriteBackColor" runat="server" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem Value="0" Text=""></asp:ListItem>
                                </asp:DropDownList>                                
                                <asp:TextBox ID="txtHardDiskOthers" CssClass="txtUppercase WriteBackColor"
                                    runat="server" Width="160px" MaxLength="7" AutoCompleteType="None" ></asp:TextBox>
                                <asp:CompareValidator ID="cvHDD" runat="server" ControlToValidate="ddlHardDisk" ErrorMessage="CPU Type is required" Operator="NotEqual"
                                    ValueToCompare="0" ValidationGroup="submit"  />                                    
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtHardDiskOthers_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtHardDiskOthers" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label26" runat="server" Text="Serial No."></asp:Label>
                                &nbsp;<asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSerialNo" CssClass="txtUppercase WriteBackColor" runat="server"
                                    Width="220px" MaxLength="30" AutoCompleteType="None" ></asp:TextBox>
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtSerialNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtSerialNo" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px; height: 26px;">
                                IP :
                            </td>
                            <td>
                             <ajaxToolkit:FilteredTextBoxExtender ID="txtIP_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtIP" FilterType="Custom,Numbers"
                                    ValidChars=".">
                             </ajaxToolkit:FilteredTextBoxExtender>                             
                             <asp:TextBox ID="txtIP" runat="server" CssClass="txtUppercase WriteBackColor" MaxLength="15" AutoCompleteType="None" ></asp:TextBox>                                                             
                             <asp:RegularExpressionValidator ID="regExIP" runat="server" 
                                ControlToValidate="txtIP" 
                                ErrorMessage="Invalid IP Address" 
                                ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"
                                SetFocusOnError="true"  
                             />
                                &nbsp;                              
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px;">
                                IP Type:
                            </td>
                            <td style="width: 246px">
                                <asp:DropDownList ID="ddlIPType" runat="server" CssClass="WriteBackColor">
                                    <asp:ListItem Selected="True">
                                    </asp:ListItem>
                                    <asp:ListItem>STATIC</asp:ListItem>
                                    <asp:ListItem>DYNAMIC</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 144px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label27" runat="server" Text="Attachment"></asp:Label>
                                &nbsp;<asp:Label ID="Label32" runat="server" Text=":"></asp:Label>
                            </td>
                            <td style="width: 246px">
                                <asp:RadioButton ID="rbScan" runat="server" AutoPostBack="True" Text="Scan File" />
                                &nbsp;<asp:RadioButton ID="rbAttach" runat="server" AutoPostBack="True" Text="Attach File" />
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
                            <td align="right" style="width: 144px">
                                <asp:Label ID="Label28" runat="server" Text="P.O Number"></asp:Label>
                                &nbsp;<asp:Label ID="Label33" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPONo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    MaxLength="8" AutoCompleteType="None" ></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPONo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Custom,Numbers" TargetControlID="txtPONo" ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="ReqPONo" ControlToValidate="txtPONo" ValidationGroup="submit"
                                    ErrorMessage=" P.O # is required" runat="server" SetFocusOnError="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px" align="right">
                                <asp:Label ID="Label36" runat="server" Text="Delivery Date"></asp:Label>
                                &nbsp;<asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                                <br />
                            </td>
                            <td>
                                <asp:TextBox ID="txtDelDate" runat="server" CssClass="ReadOnlyBackColor" MaxLength="10"
                                    ReadOnly="True" Width="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqDelDate" ControlToValidate="txtDelDate" ValidationGroup="submit"
                                    ErrorMessage="Delivery Date is required" runat="server" 
                                    SetFocusOnError="true"/>
                                <ajaxToolkit:CalendarExtender ID="txtDelDate_CalendarExtender" runat="server" Enabled="True"
                                    Format="yyyy-MM-dd" PopupPosition="Right" TargetControlID="txtDelDate">
                                </ajaxToolkit:CalendarExtender>                                
                                <asp:HiddenField ID="HFReqNeedDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px" align="right">
                                <asp:Label ID="Label1" runat="server" Text="Active"></asp:Label>
                                &nbsp;<asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                            </td>
                            <td style="width: 246px">
                                <asp:CheckBox ID="cbActive" runat="server" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                            <td style="width: 246px">
                                &nbsp;
                                <asp:Label ID="lblErrorSaving" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                            </td>
                            <td style="width: 246px">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="submit" />
                                &nbsp;<asp:Button ID="btnNew" runat="server" Text="OK" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 144px">
                                &nbsp;
                                </td>
                            <td style="width: 246px">                                
                                &nbsp;
                                &nbsp;                                
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
          
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          
            <ajaxToolkit:ModalPopupExtender ID="MPEDivison2" runat="server" PopupControlID="divPopUpDiv"
                TargetControlID="btnDivision"  PopupDragHandleControlID="panelDragHandleDiv"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <ajaxToolkit:ModalPopupExtender ID="MPEBranch" runat="server" PopupControlID="divPopUpBranch"
                TargetControlID="btnBranch"  PopupDragHandleControlID="panelDragHandleBranch"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <ajaxToolkit:ModalPopupExtender ID="MPEUsersID" runat="server" PopupControlID="divPopUpUserID"
                TargetControlID="btnUsersID"  PopupDragHandleControlID="panelDragHandleUserID"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <ajaxToolkit:ModalPopupExtender ID="MPEUsersIDD" runat="server" PopupControlID="divPopUpUserIDD"
                TargetControlID="btnUsersIDD" PopupDragHandleControlID="panelDragHandleUserIDD"
                BackgroundCssClass="ModalStyle">
            </ajaxToolkit:ModalPopupExtender>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnDivision" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnBranch" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUsersID" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUsersIDD" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />       
            <%--<ajaxToolkit:ModalPopupExtender 
                ID="MPE" 
                runat="server" 
                PopupControlID = "PopUpPanel" CancelControlID="btn" >
            </ajaxToolkit:ModalPopupExtender>    
            <asp:Panel ID="PopUpPanel" runat="server" Height="16px" Width="238px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                    
                </asp:UpdatePanel>                
                <asp:Label ID="lblmsg" runat="server" Text=" "></asp:Label>
                <br />
                <asp:Button ID="btn" runat="server" Text="Modal" />
                <br />
            </asp:Panel>--%><div id="divPopUpDiv" style="display: none;">
                
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
                
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            </div>
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
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
            
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <%--<ajaxToolkit:ModalPopupExtender 
                ID="MPE" 
                runat="server" 
                PopupControlID = "PopUpPanel" CancelControlID="btn" >
            </ajaxToolkit:ModalPopupExtender>    
            <asp:Panel ID="PopUpPanel" runat="server" Height="16px" Width="238px">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">                    
                </asp:UpdatePanel>                
                <asp:Label ID="lblmsg" runat="server" Text=" "></asp:Label>
                <br />
                <asp:Button ID="btn" runat="server" Text="Modal" />
                <br />
            </asp:Panel>--%>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
        </ContentTemplate>
        
    </asp:UpdatePanel>
</asp:Content>
