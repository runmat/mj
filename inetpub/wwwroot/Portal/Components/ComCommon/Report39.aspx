<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report39.aspx.vb" Inherits="CKG.Components.ComCommon.Report39" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="/Portal/PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="/Portal/PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR" />
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE" />
    <meta content="JavaScript" name="vs_defaultClientScript" />
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema" />
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table1" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Ergebnisanzeige)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 140px">
                            <table id="Table2" cellspacing="0" cellpadding="0" style="width: 140px" border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="StandardButton" ToolTip="Vorgänge Suchen"> &#149;&nbsp;Suchen</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="middle">
                                        <asp:LinkButton ID="cmdLoeschen" Visible="false" runat="server" CssClass="StandardButton"
                                            ToolTip="zur Kenntnis genommen"> •&nbsp;Neue Suche</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr id="trSuche" runat="server">
                                    <td valign="top">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap">
                                                    Datum von:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CE_Freigabevon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtAbDatum">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MEE_Freigabevon" runat="server" TargetControlID="txtAbDatum"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Datum bis:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="CE_FreigabeBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtBisDatum">
                                                    </cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="MEE_FreigabeBis" runat="server" TargetControlID="txtBisDatum"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </cc1:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Kennzeichen:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server"></asp:TextBox>                                            
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Fahrgestellnummer:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtChassisNum" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Name1:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtName1" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Name2
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtName2" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Straße:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtStrasse" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    PLZ:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtPLZ" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TextLarge" nowrap="nowrap" style="width: 8%">
                                                    Ort:
                                                </td>
                                                <td style="width: 90%">
                                                    <asp:TextBox ID="txtOrt" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="'Datum von' größer als 'Datum bis'!"
                                                        Type="Date" ControlToValidate="txtAbDatum" ControlToCompare="txtBisDatum" Operator="LessThanEqual"
                                                        Font-Bold="True" CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LabelExtraLarge" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trExcel" visible="false">
                                                <td class="LabelExtraLarge">
                                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" Visible="False">Excelformat</asp:LinkButton>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="100%"
                                            ID="GridView1" AllowPaging="True" AllowSorting="true" PageSize="20" Visible="false">
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <HeaderStyle Wrap="False" ForeColor="White" CssClass="GridTableHead" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="50px">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibtnEdit" runat="server" SortExpression="ibtnEdit" CommandName="Info"
                                                            ImageUrl="/services/images/shipping.gif" ToolTip="Versandstatus anzeigen." CommandArgument='<%# DataBinder.Eval(Container, "DataItem.POOLNR") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZLSDAT" HeaderText="col_Datum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Datum" runat="server" CommandName="Sort" CommandArgument="ZZLSDAT">col_Datum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblDatum" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSDAT","{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZLSTIM" HeaderText="col_Uhrzeit">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Uhrzeit" runat="server" CommandName="Sort" CommandArgument="ZZLSTIM">col_Uhrzeit</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblUhrzeit" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLSTIM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZDIEN1" HeaderText="col_Versandart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="ZZDIEN1">col_Versandart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersandart" Text='<%# DataBinder.Eval(Container, "DataItem.ZZDIEN1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="FRACHTFUEHRER" HeaderText="col_Frachtfuehrer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Frachtfuehrer" runat="server" CommandName="Sort" CommandArgument="FRACHTFUEHRER">col_Frachtfuehrer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFrachtfuehrer" Text='<%# DataBinder.Eval(Container, "DataItem.FRACHTFUEHRER") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Adresse" HeaderText="col_Adresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Adresse" runat="server" CommandName="Sort" CommandArgument="Adresse">col_Adresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lbAdresse" Text='<%# DataBinder.Eval(Container, "DataItem.Adresse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr id="trVersand" runat="server" visible="false">
                                    <td>
                                        <asp:Panel ID="mb" runat="server" Width="600px" Height="670px" BackColor="White"
                                            Style="margin-left: 200px; margin-top: 20px;">
                                            <div style="text-align: center; background-color: #DCDCDC; height: 40px; vertical-align: middle">
                                                <div style="padding-top: 10px">
                                                    <asp:Label ID="lblVersandstatus" runat="server" Text="Versandstatus" Font-Bold="True"
                                                        Font-Size="14px"></asp:Label>
                                                </div>
                                            </div>
                                            <div>
                                                <table cellpadding="5" cellspacing="0" style="width: 100%;" border="1">
                                                    <tr>
                                                        <td style="font-weight: bold; padding-left: 20px">
                                                            <b>Status </b>&nbsp;
                                                        </td>
                                                        <td>
                                                            <b>Datum </b>&nbsp;
                                                        </td>
                                                        <td>
                                                            <b>Uhrzeit </b>&nbsp;
                                                        </td>
                                                        <td>
                                                            <b>Meldung </b>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trStatus0" runat="server">
                                                        <td>
                                                            <asp:Image ID="Image1" runat="server" ImageUrl="/services/images/vlog/1.png" Width="80px"
                                                                Height="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat0" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime0" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld0" runat="server" Text="Ihr Auftrag wurde bearbeitet."></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trStatus1" runat="server">
                                                        <td>
                                                            <asp:Image ID="Image6" runat="server" Height="80px" ImageUrl="/services/images/vlog/6.png"
                                                                Width="80px" Visible="False" />
                                                            <asp:Image ID="Image2" runat="server" Height="80px" ImageUrl="/services/images/vlog/2.png"
                                                                Width="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat1" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime1" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld1" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trStatus2" runat="server">
                                                        <td>
                                                            <asp:Image ID="Image7" runat="server" Height="80px" ImageUrl="/services/images/vlog/7.png"
                                                                Visible="False" Width="80px" />
                                                            <asp:Image ID="Image3" runat="server" Height="80px" ImageUrl="/services/images/vlog/3.png"
                                                                Width="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat2" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime2" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld2" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trStatus3" runat="server">
                                                        <td>
                                                            <asp:Image ID="Image8" runat="server" Height="80px" ImageUrl="/services/images/vlog/8.png"
                                                                Visible="False" Width="80px" />
                                                            <asp:Image ID="Image4" runat="server" Height="80px" ImageUrl="/services/images/vlog/4.png"
                                                                Width="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat3" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime3" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld3" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trStatus4" runat="server">
                                                        <td>
                                                            <asp:Image ID="Image9" runat="server" Height="80px" ImageUrl="/services/images/vlog/9.png"
                                                                Visible="False" Width="80px" />
                                                            <asp:Image ID="Image5" runat="server" Height="80px" ImageUrl="/services/images/vlog/5.png"
                                                                Width="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat4" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime4" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld4" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr id="trFehler" runat="server" visible="false">
                                                        <td>
                                                            <asp:Image ID="ImageFehler" runat="server" Height="80px" ImageUrl="/services/images/vlog/Fehler.png"
                                                                Width="80px" />&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblDat5" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblTime5" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblMeld5" runat="server"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" style="text-align: center">
                                                            <asp:Button ID="btnCancel" runat="server" Text="OK" CssClass="TablebuttonLarge" Font-Bold="true"
                                                                Width="90px" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div style="height: 10px">
                                                &nbsp;
                                            </div>  
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <!--#include File="/Portal/PageElements/Footer.html" -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
