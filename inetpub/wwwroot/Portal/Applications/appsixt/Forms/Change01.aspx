<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="AppSIXT.Change01" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head id="Head1" runat="server">
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
    <style type="text/css">
        .style1
        {
            height: 32px;
        }
    </style>
</head>

<script language="JavaScript" type="text/javascript">
    function openinfo(url) {
        fenster = window.open(url, "AVIS", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=500,height=200");
        fenster.focus();
    }
</script>

<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <asp:ScriptManager runat="server" ID="Scriptmanager1" EnableScriptLocalization="true"
        EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tbody>
                        <tr>
                            <td class="PageNavigation" colspan="2">
                                <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;<asp:Label ID="lbl_PageTitle"
                                    runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TaskTitle" valign="top" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" width="120">
                                <table id="Table2" cellspacing="0" cellpadding="0" bgcolor="white" border="0" runat="server">
                                    <tr id="trCreate" runat="server">
                                        <td  width="120">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td width="100">
                            </td>
                            <td>
                                <table id="tbl_Query" class="TableColors" cellspacing="0" style="margin-bottom:10px" cellpadding="5" width="50%" runat="server">
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap align="left">
                                            CarportID:
                                        </td>
                                        <td class="StandardTableAlternate" width="100%">
                                            <asp:Label ID="lblCarportID" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap align="left">
                                            Name:
                                        </td>
                                        <td class="TextLarge" width="100%">
                                            <asp:Label ID="lblName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap align="left">
                                            Demontagedatum*:
                                        </td>
                                        <td class="StandardTableAlternate" width="100%">
                                            <asp:TextBox ID="txtDemontagedatum" ToolTip="Demontagedatum" runat="server"></asp:TextBox>
                                            <cc1:CalendarExtender ID="Demontagedatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtDemontagedatum">
                                            </cc1:CalendarExtender>
                                            <asp:CompareValidator ID="CVDemontagedatum" Display="Dynamic" runat="server" ControlToValidate="txtDemontagedatum" ErrorMessage="Bitte geben Sie ein gültiges Datum ein!" Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                            <asp:RequiredFieldValidator ID="RFVDemontagedatum" Display="Dynamic" runat="server" ControlToValidate="txtDemontagedatum" ErrorMessage="Bitte geben Sie ein gültiges Datum ein!"></asp:RequiredFieldValidator>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap align="left">
                                            &nbsp;
                                        </td>
                                        <td class="TextLarge" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap="nowrap" align="left">
                                            Vorlage ZBI
                                        </td>
                                        <td class="StandardTableAlternate">
                                            <asp:RadioButtonList ID="rblZBI" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0">Nein</asp:ListItem>
                                                <asp:ListItem Value="1">Ja</asp:ListItem>
                                                <asp:ListItem Selected="True" Value="2">mit Kopie</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap" align="left">
                                            Kennzeichen*:
                                        </td>
                                        <td class="TextLarge" width="100%">
                                            <asp:TextBox ID="txtKennz" runat="server" MaxLength="15"></asp:TextBox>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap="nowrap" align="left">
                                            Anzahl Kennzeichen*:
                                        </td>
                                        <td class="StandardTableAlternate">
                                            <asp:TextBox ID="txtAnzahlKennz" runat="server" MaxLength="1"></asp:TextBox>
                                            <asp:ImageButton ID="btnEmpty" runat="server" ImageUrl="../../../images/empty.gif"
                                                Height="16px" />
                                            <asp:LinkButton ID="lb_Weiter" runat="server" CssClass="StandardButtonTable" 
                                                Width="88px">•&nbsp;Erfassen</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap align="left" colspan="2">
                                            * Pflichtfelder
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td width="100">
                            </td>
                            <td>
                                <table id="tblResult" class="TableColors" style="margin-bottom:10px" visible="false"  cellspacing="0" cellpadding="5" width="100%" border="0" runat="server">
                                    <tr>
                                        <td class="LabelExtraLarge" align="left" height="9">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td align="right" height="9">
                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" 
                                                Height="14px" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trPrint" runat="server" visible="false"> 
                                        <td class="LabelExtraLarge" align="left" height="9">
                                            &nbsp;</td>
                                        <td align="right" height="9">
                                        <img src="../../../Images/iconPDF.gif" alt="PDF herunterladen" />
 
                                            <asp:LinkButton ID="lnkCreatePDF1" runat="server">Lieferschein</asp:LinkButton>

                                        <img style="margin-left:5px" src="../../../Images/iconXLS.gif" alt="Excel" />

                                              <asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel</asp:LinkButton>
                                      
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="LICENSE_NUM" AllowSorting="True" AllowPaging="True"
                                                AutoGenerateColumns="False">
                                                <RowStyle CssClass="GridTableItem"></RowStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                                <Columns>
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="LICENSE_NUM" HeaderText="Kennzeichen" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnummer" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ZZFABRIKNAME" HeaderText="Hersteller" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}" DataField="DAT_DEMONT" HeaderText="Demontagedatum" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="VORLAGE_ZB1_CPL_Text"
                                                        HeaderText="Vorlage ZBI" />
                                                    <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ANZ_KENNZ_CPL" HeaderText="Anzahl Kennzeichen" />
                                                    <asp:TemplateField HeaderText="Entfernen">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbDelete" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'
                                                                CommandName="Delete" Height="10px">
																<img alt="entfernen" src="../../../Images/loesch.gif" border="0"/>
														    </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                                                           
                                        <td  align="left" height="9">
                                        </td>
                                        <td align="right"  height="9">
                                            <asp:LinkButton ID="cmdGetData" runat="server" CssClass="StandardButton" 
                                                Visible="False">&#149;&nbsp;Daten ergänzen</asp:LinkButton>
                                            <asp:LinkButton ID="cmdEdit" runat="server" CssClass="StandardButton" 
                                                Visible="False">&#149;&nbsp;Bearbeiten</asp:LinkButton>            
                                            <asp:LinkButton ID="cmdSend" runat="server" CssClass="StandardButton" 
                                                Visible="False">&#149;&nbsp;Absenden</asp:LinkButton>      
                                            <asp:LinkButton ID="cmdNewList" runat="server" CssClass="StandardButton" 
                                                Visible="False">&#149;&nbsp;Neue Liste</asp:LinkButton>                                                                                              
                                        </td>
                                    </tr>
                        </tr>
                        <tr style="padding-top:25px">
                            <td valign="top">
                                &nbsp;<asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox>
                            </td>
                            <td>
                                <!--#include File="../../../PageElements/Footer.html" -->
                            </td>
                        </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
