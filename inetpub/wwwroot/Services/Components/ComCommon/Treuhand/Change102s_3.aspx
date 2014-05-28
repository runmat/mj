<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change102s_3.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change102s_3" 
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
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
                                        <telerik:RadGrid ID="rgGrid1" runat="server" AllowPaging="True" PageSize="15" AllowSorting="True" 
                                            AutoGenerateColumns="False" GridLines="None" Culture="de-DE" 
                                            OnNeedDataSource="rgGrid1_NeedDataSource"  
                                            OnExcelMLExportRowCreated="rgGrid1_ExcelMLExportRowCreated" 
                                            OnExcelMLExportStylesCreated="rgGrid1_ExcelMLExportStylesCreated" 
                                            OnItemCommand="rgGrid1_ItemCommand" OnItemDataBound="rgGrid1_ItemDataBound" >
                                            <ExportSettings HideStructureColumns="true">
                                                <Excel Format="ExcelML" />
                                            </ExportSettings>
                                            <ClientSettings AllowKeyboardNavigation="true" >
                                                <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" />
                                            </ClientSettings>
                                            <MasterTableView Width="100%" TableLayout="Auto" CommandItemDisplay="Top" AllowMultiColumnSorting="True">
                                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false"
                                                    ExportToExcelText="Excel herunterladen">
                                                </CommandItemSettings>
                                                <PagerStyle Mode="NextPrevAndNumeric" ></PagerStyle>
                                                <HeaderStyle ForeColor="White" />
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="ID" SortExpression="ID" UniqueName="ID" Visible="false" >
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridButtonColumn SortExpression="ID" UniqueName="Loeschen" CommandName="Del" HeaderText="Löschen" ButtonType="ImageButton" 
                                                        ImageUrl="../../../Images/Papierkorb_01.gif" ButtonCssClass="RadGridImageButton">
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </telerik:GridButtonColumn>
                                                    <telerik:GridTemplateColumn DataField="ZZREFERENZ2" SortExpression="ZZREFERENZ2" UniqueName="ZZREFERENZ2" >
                                                        <HeaderStyle Width="115px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtZZREFERENZ2" Text='<%# Eval("ZZREFERENZ2") %>' Width="100px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="EQUI_KEY" SortExpression="EQUI_KEY" UniqueName="EQUI_KEY" >
                                                        <HeaderStyle Width="180px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtEQUI_KEY" Text='<%# Eval("EQUI_KEY") %>' Width="165px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="SPERRDAT" SortExpression="SPERRDAT" UniqueName="SPERRDAT" >
                                                        <HeaderStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtSPERRDAT" Text='<%# Eval("SPERRDAT") %>' Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="VERTR_BEGINN" SortExpression="VERTR_BEGINN" UniqueName="VERTR_BEGINN" >
                                                        <HeaderStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtVERTR_BEGINN" Text='<%# Eval("VERTR_BEGINN") %>' Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="VERTR_ENDE" SortExpression="VERTR_ENDE" UniqueName="VERTR_ENDE" >
                                                        <HeaderStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtVERTR_ENDE" Text='<%# Eval("VERTR_ENDE") %>' Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Versanddatum" SortExpression="Versanddatum" UniqueName="Versanddatum" >
                                                        <HeaderStyle Width="90px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtVersanddatum" Text='<%# Eval("Versanddatum") %>' Width="75px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Haendlernummer" SortExpression="Haendlernummer" UniqueName="Haendlernummer" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtHaendlernummer" Text='<%# Eval("Haendlernummer") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Haendlername" SortExpression="Haendlername" UniqueName="Haendlername" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtHaendlername" Text='<%# Eval("Haendlername") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Name2" SortExpression="Name2" UniqueName="Name2" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtName2" Text='<%# Eval("Name2") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Strasse" SortExpression="Strasse" UniqueName="Strasse" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtStrasse" Text='<%# Eval("Strasse") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="PLZ" SortExpression="PLZ" UniqueName="PLZ" >
                                                        <HeaderStyle Width="60px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtPLZ" Text='<%# Eval("PLZ") %>' Width="45px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Ort" SortExpression="Ort" UniqueName="Ort" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtOrt" Text='<%# Eval("Ort") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn DataField="Postfach" SortExpression="Postfach" UniqueName="Postfach" >
                                                        <HeaderStyle Width="165px" />
                                                        <ItemTemplate>
                                                            <asp:TextBox runat="server" ID="txtPostfach" Text='<%# Eval("Postfach") %>' Width="150px"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="ERNAM" SortExpression="ERNAM" UniqueName="ERNAM" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERDAT" SortExpression="ERDAT" UniqueName="ERDAT" DataFormatString="{0:d}" >
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
                                                    <telerik:GridBoundColumn DataField="MESSAGE" SortExpression="MESSAGE" UniqueName="MESSAGE" >
                                                        <HeaderStyle Width="100px" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="ERROR" SortExpression="ERROR" UniqueName="ERROR" Visible="False" >
                                                        <HeaderStyle Width="300px" />
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
                                    <div id="dataQueryFooter" runat="server" >
                                        <asp:LinkButton ID="cmdCheck" runat="server" CssClass="Tablebutton" Width="78px">» Prüfen</asp:LinkButton>
                                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                            Visible="False" Width="78px">» Absenden</asp:LinkButton>
                                    </div> 
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
