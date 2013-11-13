<%@ Page Title="" Language="VB" MasterPageFile="~/LeftWOMainMasterPage.master" AutoEventWireup="false"
    CodeFile="MISview.aspx.vb" Inherits="MISview" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="Custom" Namespace="ENTech.WebControls" Assembly="AutoSuggestMenu" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControl/Division.ascx" TagName="DIVv" TagPrefix="UC1" %>
<%@ Register Src="../UserControl/Branch.ascx" TagName="Branchv" TagPrefix="ACT1" %>
<%@ Register Src="../UserControl/UsersID.ascx" TagName="UsersIDv" TagPrefix="CIT1" %>
<%@ Register Src="../UserControl/UsersIDD.ascx" TagName="UsersIDDv" TagPrefix="USJR1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">

    <script type="text/javascript" language="javascript">
        function doPostback() 
        {
            $get('<%= Button1.ClientID %>').click();
        }
//        function ValidateItem(sender,arguments)
//        {
//            var cpu = document.getElementById("ddlCPU");
//            if (null!= cpu)
//            {
//                var iCpu = new Number(cpu[cpu.selectedIndex].value);            
//                arguments.isvalid=(iCpu>0);            
//            }
//        }
//       function NumberDOT(evt)
//       {
////          var charCode = (evt.which) ? evt.which : event.keyCode;          
////          var d = document.getElementById('<%=txtIP.ClientID%>').value.split(".").length - 1;          
////          if(charCode == 46)
////          {
////            if(d > 2)
////            {              
////               return false
////            }                
////          }                  
////          if (charCode != 46 && charCode > 31
////          && (charCode < 48 || charCode > 57))
////          {       
////                return false;              
////          }
////          return true;
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
//                return false;                       
//          return true;
//       }
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
    
    

    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel ID="UpdateFace" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    View Computer Info</h1>
                <asp:Panel ID="PanelCategory" runat="server" GroupingText="Computer Assignment" Width="631px"
                    TabIndex="1">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbDivision" runat="server" Text="Division" AutoPostBack="True"
                                    Visible="False" />
                                &nbsp;<asp:RadioButton ID="rbBranch" runat="server" Text="Branch" AutoPostBack="True"
                                    Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBCCode" runat="server" Text="Division Code"></asp:Label>
                                &nbsp;<asp:Label ID="Label16" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCode" runat="server" Width="40px" ReadOnly="True"></asp:TextBox>
                                <asp:ImageButton ID="lbASearchCode" CssClass="ButtonSearch1" 
                                    runat="server" ImageUrl="~/Images/SearchButton.JPG" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="lblBCName" runat="server" Text="Division Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label14" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCodeName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
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
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label18" runat="server" Text="Employee Name"></asp:Label>
                                &nbsp;<asp:Label ID="Label19" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIDName" runat="server" ReadOnly="True" Width="220px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label40" runat="server" Text="Asset Inventory No. :"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="AssetInventory" runat="server" Width="224px" 
                                    AutoPostBack="True" Enabled="False" CssClass="WriteBackColor">
                                    <asp:ListItem Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Button ID="Button1" runat="server" Style="display: none" />
                </asp:Panel>
                <asp:Panel ID="PanelInformation" runat="server" GroupingText="Asset Information"
                    Width="631px">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label4" runat="server" Text="Computer Type"></asp:Label>
                                &nbsp;<asp:Label ID="Label6" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompType" CssClass="ReadOnlyArea" runat="server" AutoPostBack="True"
                                    Width="224px">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>DESKTOP</asp:ListItem>
                                    <asp:ListItem>NOTEBOOK</asp:ListItem>
                                    <asp:ListItem>SERVER</asp:ListItem>
                                </asp:DropDownList>                                
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label5" runat="server" Text="Asset Inventory No."></asp:Label>
                                &nbsp;<asp:Label ID="Label7" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAsstInvNo" CssClass="txtUppercase ReadOnlyBackColor" runat="server"
                                    Width="144px" MaxLength="18"></asp:TextBox>
                               <%-- <ajaxToolkit:FilteredTextBoxExtender ID="txtAsstInvNo_FilteredTextBoxExtender1" runat="server"
                                    Enabled="True" TargetControlID="txtAsstInvNo" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="-">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                                <asp:TextBox ID="txtPTPNo" CssClass="ReadOnlyArea" runat="server" Width="100px" MaxLength="10"></asp:TextBox>
                                <%--<ajaxToolkit:FilteredTextBoxExtender ID="txtPTPNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtPTPNo" FilterType="Numbers" >
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label20" runat="server" Text="Description"></asp:Label>
                                &nbsp;<asp:Label ID="Label21" runat="server" Text=":"></asp:Label>
                            </td>
                            <td rowspan="6">
                                <asp:TextBox ID="txtAstDescription" runat="server" Height="136px" TextMode="MultiLine"
                                    Width="440px" MaxLength="500"></asp:TextBox>
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAstDescription_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtAstDescription" FilterType="LowercaseLetters,UppercaseLetters,Custom,Numbers"
                                    ValidChars="=/- .">
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
                            <td align="right" style="width: 143px">
                                &nbsp;
                            </td>
                            <td align="left" >
                                <asp:RequiredFieldValidator ID="reqAsetDescription" ControlToValidate="txtAstDescription"
                                     ValidationGroup="updated" ErrorMessage=" Asset Description is Required" runat="server" 
                                     SetFocusOnError="true"/>                                                                                                   
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblCountWord" runat="server" ForeColor="Blue" Text="500/500"></asp:Label>                                
                                <%--<asp:Label ID="lblCountWord" runat="server" ForeColor="Blue" Text="500/500"></asp:Label>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label22" runat="server" Text="CPU"></asp:Label>
                                &nbsp;<asp:Label ID="Label23" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCPU" runat="server" CssClass="WriteBackColor" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>                                                                                               
                                <asp:TextBox ID="txtCPUOthers" runat="server" CssClass="txtUppercase WriteBackColor"
                                    MaxLength="10" Width="160px" AutoCompleteType="None" ></asp:TextBox>                                
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtCPUOthers_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    TargetControlID="txtCPUOthers" ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label24" runat="server" Text="Memory Size"></asp:Label>
                                &nbsp;<asp:Label ID="Label29" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlMemoSize" CssClass="WriteBackColor" runat="server" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtMemoSizeOthers" CssClass="txtUppercase WriteBackColor"
                                    runat="server" Width="160px" MaxLength="7" AutoCompleteType="None" Visible ="false"></asp:TextBox>                                
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtMemoSizeOthers_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtMemoSizeOthers" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label25" runat="server" Text="Hard Disk"></asp:Label>
                                &nbsp;<asp:Label ID="Label30" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlHardDisk" CssClass="WriteBackColor" runat="server" Width="103px"
                                    AutoPostBack="True">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtHardDiskOthers" CssClass="txtUppercase WriteBackColor"
                                    runat="server" Width="160px" MaxLength="7" AutoCompleteType="None"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtHardDiskOthers_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtHardDiskOthers" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars=" ">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label26" runat="server" Text="Serial No."></asp:Label>
                                &nbsp;<asp:Label ID="Label31" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSerialNo" AutoCompleteType="None" CssClass="txtUppercase WriteBackColor" runat="server"
                                    Width="220px" MaxLength="30"></asp:TextBox>
                                <%--                                <ajaxToolkit:FilteredTextBoxExtender ID="txtSerialNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtSerialNo" FilterType="UppercaseLetters,LowercaseLetters,Custom,Numbers"
                                    ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px; height: 26px;">
                                IP :
                            </td>
                            <td style="height: 26px">
                             <%--onkeypress="return isIP(this.ID);"--%>
                                <asp:TextBox ID="txtIP" AutoCompleteType="None"  runat="server" 
                                    CssClass="txtUppercase WriteBackColor" MaxLength="15" ></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtSerialNo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" TargetControlID="txtSerialNo" FilterType="Custom,Numbers"
                                    ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="regExIP" runat="server" 
                                    ControlToValidate="txtIP" 
                                    ErrorMessage="Invalid IP Address" 
                                    ValidationExpression="^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$"
                                    SetFocusOnError="true"  
                                />
                      <%--          <asp:RegularExpressionValidator ID = "RegularExpressionValidation" runat = "server" 
                                ControlToValidate ="txtIP" SetFocusOnError = "true"
                                validationExpression="^[0-9.\s]{1,1}$" ErrorMessage=" "></asp:RegularExpressionValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px;">
                                IP Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIPType" runat="server" CssClass="WriteBackColor" 
                                    AutoPostBack="True">
                                    <asp:ListItem Selected="True">
                                    </asp:ListItem>
                                    <asp:ListItem Value="STATIC">STATIC</asp:ListItem>
                                    <asp:ListItem Value="DYNAMIC">DYNAMIC</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 143px; text-align: right; vertical-align: top;">
                                <asp:Label ID="Label27" runat="server" Text="Attachment" Visible="False"></asp:Label>
                                &nbsp;<asp:Label ID="Label32" runat="server" Text=":" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="rbScan" runat="server" AutoPostBack="True" Text="Scan File"
                                    Visible="False" />
                                &nbsp;<asp:RadioButton ID="rbAttach" runat="server" AutoPostBack="True" Text="Attach File"
                                    Visible="False" />
                                <asp:Repeater ID="attList" runat="server" EnableTheming="False" Visible="False">
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
                            <td align="right" style="width: 143px">
                                <asp:Label ID="Label28" runat="server" Text="P.O Number"></asp:Label>
                                &nbsp;<asp:Label ID="Label33" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPONo" CssClass="WriteBackColor" runat="server" Width="100px"
                                    MaxLength="8" AutoCompleteType="None"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtPONo_FilteredTextBoxExtender" runat="server"
                                    Enabled="True" FilterType="Custom,Numbers" TargetControlID="txtPONo" ValidChars="">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="ReqPONo" ControlToValidate="txtPONo" ValidationGroup="updated"
                                    ErrorMessage="P.O # is required" runat="server" SetFocusOnError="true"/>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px" align="right">
                                <asp:Label ID="Label36" runat="server" Text="Delivery Date"></asp:Label>
                                &nbsp;<asp:Label ID="Label39" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDelDate" runat="server" CssClass="ReadOnlyBackColor" MaxLength="10"
                                    ReadOnly="True" Width="100px"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="txtDelDate_CalendarExtender" runat="server" Enabled="True"
                                    Format="yyyy-MM-dd" PopupPosition="Right" TargetControlID="txtDelDate">
                                </ajaxToolkit:CalendarExtender>
                                <asp:HiddenField ID="HFReqNeedDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px" align="right">
                                <asp:Label ID="Label1" runat="server" Text="Active"></asp:Label>
                                &nbsp;<asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="cbActive" runat="server" AutoPostBack="True" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                                <asp:Label ID="lblErrorSaving" runat="server" Font-Italic="True" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="False" CausesValidation="true" ValidationGroup="updated" />
                                &nbsp;<asp:Button ID="btnNew" runat="server" Text="OK" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp; 
                               <asp:UpdatePanel ID="UpdateMsg" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <ajaxToolkit:ModalPopupExtender ID="MPEMsg" runat="server" PopupControlID="PanelMsg" 
                                            TargetControlID="none" PopupDragHandleControlID = "PanelMsg"
                                            RepositionMode="RepositionOnWindowResize" BackgroundCssClass="ModalStyle"
                                            CancelControlID="btnOK"/>                                    
                                        <div id = "MsgDiv" style="display:none">
                                            <asp:Panel ID="PanelMsg" runat="server" CssClass="Box">
                                                <div style="margin: auto;">                                                    
                                                    <asp:Label ID="lblMsg" runat="server" Text="Label"></asp:Label>
                                                </div>
                                                <div style="margin: auto;">
                                                    
                                                    <asp:Button ID="btnOK" Text="OK" runat="server" />
                                                    <asp:Button ID="none" runat="server" Visible="False" />
                                                </div>                
                                            </asp:Panel>  
                                        </div>
                                    </ContentTemplate>                                
                                </asp:UpdatePanel>                                                                                      
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 143px">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>                        
            <ajaxToolkit:ModalPopupExtender ID="MPEDivison2" runat="server" PopupControlID="divPopUpDiv"
                TargetControlID="btnDivision"  PopupDragHandleControlID="panelDragHandleDiv"
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
            <asp:Button ID="btnDivision"  runat="server" Text="Button" UseSubmitBehavior="true"
                Style="display: none;" />
            <asp:Button ID="btnBranch" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />
            <asp:Button ID="btnUsersID" UseSubmitBehavior="false" runat="server" Text="Button"
                Style="display: none;" />
            <asp:Button ID="btnUsersIDD" runat="server" Text="Button"
                Style="display: none;" />
            <div id="divPopUpDiv" style="display: none;">
                <asp:Panel runat="Server" ID="panelDragHandleDiv" CssClass="drag">
                    <div class="gridContainer">
                        <UC1:DIVv ID="DIV" runat="server" />
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
                        <ACT1:Branchv ID="Branch" runat="server" />
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
                        <CIT1:UsersIDv ID="UsersID" runat="server" />
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
                        <USJR1:UsersIDDv ID="UsersIDD" runat="server" />
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
