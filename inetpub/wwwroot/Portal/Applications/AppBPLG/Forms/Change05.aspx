<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="AppBPLG.Change05" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
    <title></title>
    <uc1:Styles ID="ucStyles" runat="server" />
    <style type="text/css">
        A.StandardButton:hover
        {
            height:14px;
            
        }
    </style>
 </head>
<body>
    <form id="Form1" method="post" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="server" />
    <table width="100%" border="0">
        <tr>
            <td colspan="2">
                <uc1:Header ID="ucHeader" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="PageNavigation" colspan="2">
                <asp:Label ID="lblHead" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="TaskTitle" valign="top" colspan="2">
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="TABLEX" width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr runat="server" id="searchRow">
            <td style="vertical-align:top;">
                <div style="padding-top:3px;padding-left:4px;width:120px">
                    
                    <asp:LinkButton ID="lb_Suche" runat="server" CssClass="StandardButton" Width="120px" Height="14px" >Suche</asp:LinkButton>
                    
                </div>
            </td>
            <td align="left" style="padding-left:10px" >
                <table id="Table3" cellspacing="0" width="100%" bgcolor="white" border="0">
                    <tr class="TextLarge" id="tr_FIN" runat="server">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_FilterFIN" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilterFIN" Width="200" runat="server" />
                        </td>
                    </tr>
                    <tr class="StandardTableAlternate" id="tr_BRANDING" runat="server">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_FilterBRANDING" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilterBRANDING" Width="200" runat="server" />
                        </td>
                    </tr>
                    <tr class="TextLarge" id="tr_HAENDLER" runat="server">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_FilterHAENDLER" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilterHAENDLER" Width="200" runat="server" />
                        </td>
                    </tr>
                    <tr class="StandardTableAlternate" id="tr_KUNNR" runat="server">
                        <td width="147" class="TextLarge">
                            <asp:Label runat="server" ID="lbl_FilterKUNNR" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtFilterKUNNR" Width="200" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr runat="server" id="resultRow" visible="false">
        
            <td style="vertical-align:top">
            <div style="padding-top:3px;padding-left:4px;width:120px">
                <asp:LinkButton ID="lb_NewSearch" runat="server" CssClass="StandardButton" Width="120" Height="14">Neue Suche</asp:LinkButton>
                 </div>
            </td>
           
            <td style="padding-left:10px;padding-top:10px">
                <div id="ExcelRow" runat="server">
                    <asp:LinkButton ID="lnkCreateExcel" runat="server" style="float:right;padding-right:10px">Excelformat</asp:LinkButton>
                </div>
                <asp:DataGrid runat="server" ID="resultGrid" AllowPaging="True" AllowSorting="True"
                    AutoGenerateColumns="False" BackColor="White" bodyCSS="tableBody" CssClass="tableMain"
                    headerCSS="tableHeader" bodyHeight="400" PageSize="50" Width="100%" DataKeyField="CHASSIS_NUM">
                    <AlternatingItemStyle CssClass="GridTableAlternate" />
                    <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                    <PagerStyle HorizontalAlign="Left" Mode="NumericPages" Position="TopAndBottom" />
                    <Columns>
                        <asp:EditCommandColumn EditText="Bearbeiten" />
                        <asp:BoundColumn DataField="CHASSIS_NUM" HeaderText="col_CHASSIS_NUM" SortExpression="CHASSIS_NUM" />
                        <asp:BoundColumn DataField="HAENDLER" HeaderText="col_HAENDLER" SortExpression="HAENDLER" />
                        <asp:BoundColumn DataField="NAME_ZF" HeaderText="col_NAME_ZF" SortExpression="NAME_ZF" />
                        <asp:BoundColumn DataField="LIZNR" HeaderText="col_LIZNR" SortExpression="LIZNR" />
                        <asp:BoundColumn DataField="TIDNR" HeaderText="col_TIDNR" SortExpression="TIDNR" />
                        <asp:BoundColumn DataField="LICENSE_NUM" HeaderText="col_LICENSE_NUM" SortExpression="LICENSE_NUM" />
                        <asp:BoundColumn DataField="EINGZB2" HeaderText="col_EINGZB2" SortExpression="EINGZB2" dataformatstring="{0:dd.MM.yyyy}" />
                        <asp:BoundColumn DataField="KUNNR" HeaderText="col_KUNNR" SortExpression="KUNNR" />
                        <asp:BoundColumn DataField="NAME_ZS" HeaderText="col_NAME_ZS" SortExpression="NAME_ZS" />
                        <asp:BoundColumn DataField="BRANDING" HeaderText="col_BRANDING" SortExpression="BRANDING" />
                        <asp:BoundColumn DataField="BEZEICHNUNG_1" HeaderText="col_BEZEICHUNG_1" SortExpression="BEZEICHNUNG_1" />
                        <%-- CHASSIS_NUM    HAENDLER    NAME_ZF LIZNR   TIDNR   LICENSE_NUM EINGZB2 KUNNR   NAME_ZS BRANDING    BEZEICHNUNG_1   --%>
                    </Columns>
                </asp:DataGrid>
            </td>
        </tr>
        <tr>
            <td width="80">
                &nbsp;
            </td>
            <td style="padding-left:10px">
                <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False" />
            </td>
        </tr>
        <tr>
            <td width="80">
                &nbsp;
            </td>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
    <ajaxToolkit:ModalPopupExtender runat="server" ID="mpeEditFzg" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground" DropShadow="false" PopupControlID="pnlEditFzg"
        TargetControlID="DUMMY" RepositionMode="RepositionOnWindowResizeAndScroll" X="300"
        Y="200" />
    <asp:Panel runat="server" ID="pnlEditFzg" BackColor="White" CssClass="modalPopup"
        Width="300" Style="display: none;">
        <div style="background: #ddd; border: 1px solid black;padding:10px 10px 10px 10px">
            <table>
                <caption>
                    Fahrzeug bearbeiten</caption>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lbl_DisplayFIN" Font-Bold="True" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_EditHAENDLER" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEditHAENDLER" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_EditLIZNR" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEditLIZNR" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_EditKUNNR" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEditKUNNR" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lbl_EditBRANDING" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtEditBRANDING" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center" style="padding-top:10px">
                        <asp:Button runat="server" ID="btnOK" Text="Übernehmen" />
                        <asp:Button runat="server" ID="btnCancel" Text="Abbrechen" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblEditError" CssClass="TextError" EnableViewState="false" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <asp:Button runat="server" ID="DUMMY" Style="display: none;" />
    </form>
</body>
</html>
