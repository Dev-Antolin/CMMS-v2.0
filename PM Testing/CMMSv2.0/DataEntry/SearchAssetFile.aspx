<%@ Page Title="" Language="VB" MasterPageFile="~/LeftDEMainMasterPage.master" AutoEventWireup="false"
    CodeFile="SearchAssetFile.aspx.vb" Inherits="DataEntry_SearchAssetFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CMMSMainContentPlaceHolder" runat="Server">
    <ajaxToolkit:ToolkitScriptManager runat="Server" ID="ScriptManager1" EnablePageMethods="true" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="main-copy">
                <h1 id="introduction">
                    Search Attach file</h1>
                <asp:Panel ID="PanelAttachInformation" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td align="right" style="width: 130px">
                                <asp:Label ID="Label1" runat="server" Text="Asset Inventory No."></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text=":"></asp:Label>
                            </td>
                            <td style="width: 220px">
                                <asp:TextBox ID="txtAttachSearchFile" CssClass="WriteBackColor txtUppercase" runat="server"
                                    Width="220px" MaxLength="18" AutoPostBack="True"></asp:TextBox>
                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtAttachSearchFile_TextBoxWatermarkExtender" 
                                    runat="server" Enabled="True" TargetControlID="txtAttachSearchFile" WatermarkCssClass="watermarked"
                                    WatermarkText="500-000-0000-00000" >
                                </ajaxToolkit:TextBoxWatermarkExtender>
                                <ajaxToolkit:FilteredTextBoxExtender ID="txtAttachSearchFile_FilteredTextBoxExtender"
                                    runat="server" Enabled="True" TargetControlID="txtAttachSearchFile" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                    ValidChars="-">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </td>
                            <td>
                                <asp:ImageButton ID="ibSearch" runat="server" 
                                    ImageUrl="~/Images/SearchButton.JPG" />
                            </td>
                        </tr>
                        <tr>
                            <td align="char" colspan="3">
                                <asp:Label ID="lblErrorSearch" runat="server" Font-Italic="True" 
                                    ForeColor="Red"></asp:Label>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px" align="right">
                                &nbsp;
                                <asp:Label ID="Label3" runat="server" Text="Attached File(s)"></asp:Label>
                                <asp:Label ID="Label4" runat="server" Text=":"></asp:Label>
                            </td>
                            <td align="left" colspan="2" rowspan="4">
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
                            <td style="width: 130px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 130px">
                                &nbsp;
                                <asp:Button ID="btnAttachSearch" runat="server" Height="19px" Text="Search" 
                                    Width="56px" />
                            </td>
                        </tr>
                    </table>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtAttachSearchFile" 
                        ErrorMessage="Incorrect format! AssetCode-BcCode-yyMM-00000" 
                        ValidationExpression="^\d{3}-[0-9]{2}[0-9a-zA-Z]{1}-\d{4}-\d{5}$" 
                        Visible="false" />
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
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton ID="btnDownload" runat="server" OnClick="btnDownload_Click" Text="Download"
        Style="display: none;" />
</asp:Content>
