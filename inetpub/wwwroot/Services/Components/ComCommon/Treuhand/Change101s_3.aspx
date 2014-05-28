<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change101s_3.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change101s_3" 
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu" style="margin-top: 10px; margin-bottom: 10px">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" >
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="innerContentRight">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="innerContentRight" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <div id="innerContentRight" style="width: 100%; margin-bottom: 10px">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                    <asp:Label ID="lblMessage" runat="server" ></asp:Label>
                    <div id="paginationQuery">
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <table cellspacing="0" cellpadding="0" style="width: 100%" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <div style="width: 100%; max-width: 909px">        
                                        <telerik:RadGrid ID="rgGrid1" runat="server" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE"  
                                            OnNeedDataSource="rgGrid1_NeedDataSource" OnItemDataBound="rgGrid1_ItemDataBound" >
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" AllowPaging="true" >
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <SortExpressions>
                                                    <telerik:GridSortExpression FieldName="ID" SortOrder="Ascending" />
                                                </SortExpressions>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="ID" SortExpression="ID" Visible="false" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="SELECT" SortExpression="SELECT" UniqueName="SELECT" >
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                        <asp:CheckBox ID="chkAnfordern" runat="server" AutoPostBack="True" Enabled='<%# Eval("MESSAGE").ToString() = "" %>' 
                                                            Checked='<%# Eval("SELECT").ToString() = "99" %>' oncheckedchanged="chkAnfordern_CheckedChanged"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="EQUI_KEY" SortExpression="EQUI_KEY" UniqueName="EQUI_KEY" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ZZREFERENZ2" SortExpression="ZZREFERENZ2" >
                                                        <HeaderStyle Width="110px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERNAM" SortExpression="ERNAM" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" DataFormatString="{0:d}" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="SPERRDAT" SortExpression="SPERRDAT" DataFormatString="{0:d}" UniqueName="SPERRDAT" >
                                                        <HeaderStyle Width="85px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn DataField="TREUH_VGA" SortExpression="TREUH_VGA" UniqueName="TREUH_VGA" >
                                                        <HeaderStyle Width="70px" />
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" ID="lblTREUH_VGA" Text="Sperren" Visible='<%# Eval("TREUH_VGA").ToString() = "S" %>'>
                                                            </asp:Label>
                                                            <asp:Label runat="server" ID="lblTREUH_VGA2" Text="Entsperren" Visible='<%# Eval("TREUH_VGA").ToString() = "F" %>'>
                                                            </asp:Label>                                                        
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="MESSAGE" SortExpression="MESSAGE" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERROR" SortExpression="ERROR" UniqueName="ERROR" Visible="false" >
                                                        <HeaderStyle Width="150px" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div>  
                        <table id="tabCrtl" width="100%" border="0" >
                            <tr>
                                <td align="left" width="230">
                                    <asp:Panel ID="pnInfo" runat="server">
                                        <div class="new_layout" >
                                            <div id="infopanel" class="infopanel">
                                                <label>
                                                    <asp:Label id="InfoHead" runat="server" Text="Information!"></asp:Label>
                                                </label>
                                                <div>
                                                    <asp:Label ID="InfoText" runat="server" >
                                                        Vor dem Absenden muss zunächst eine Prüfung erfolgen.
                                                    </asp:Label> 
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>         
                                </td>
                                <td align="right">
                                    <div id="dataQueryFooter" >
                                        <asp:LinkButton ID="cmdCheck" runat="server" CssClass="Tablebutton" Width="78px">» Prüfen</asp:LinkButton>
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Visible="False" Width="78px">» Absenden</asp:LinkButton>
                                    </div> 
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter">
                    </div>
                    <input type="hidden" id="ihLastSendTime" value=""/>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
