<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change14.aspx.vb" Inherits="AppSIXT.Change14" %>

<%@ Register Assembly="CKG.Portal" Namespace="CKG.Portal.PageElements" TagPrefix="cc1" %>

<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <meta content="Microsoft Visual Studio.NET 7.0" name="GENERATOR">
    <meta content="Visual Basic 7.0" name="CODE_LANGUAGE">
    <meta content="JavaScript" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie3-2nav3-0" name="vs_targetSchema">
    <uc1:Styles ID="ucStyles" runat="server"></uc1:Styles>
</head>
<body leftmargin="0" topmargin="0" ms_positioning="FlowLayout">
    <form id="Form1" method="post" runat="server">
    <table id="Table4" width="100%" align="center">
        <tr>
            <td>
                <uc1:Header ID="ucHeader" runat="server"></uc1:Header>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table0" cellspacing="0" cellpadding="0" width="100%" border="0">
                    <tr>
                        <td class="PageNavigation" colspan="2">
                            <asp:Label ID="lblHead" runat="server"></asp:Label>&nbsp;
                            <asp:Label ID="lblPageTitle" runat="server"> (Zusammenstellung von Abfragekriterien)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle" width="150">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150" height="24">
                                         &nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="center" width="150">
                                        <asp:LinkButton ID="btnConfirm" runat="server" CssClass="StandardButton"> &#149;&nbsp;Weiter</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="" valign="top">
                                        <asp:Label ID="lblError" runat="server" EnableViewState="False" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="true" ></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table3" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left">
                                        <asp:Panel ID="plSearch" runat="server">
                                         <table id="Table1" class="TableColors" cellspacing="0" cellpadding="5" width="50%" border="0" bgcolor="white">
                                            <tr>
                                                <td class="TextLarge" valign="top" width="130px" style="padding-bottom:20px">
                                                   
                                                        Auswahl:
                                                </td>
                                               <td class="TextLarge" valign="center" style="padding-bottom:20px">
                                                        <asp:RadioButtonList ID="rbAktion" runat="server" Width="100%" 
                                                            AutoPostBack="True">
                                                            <asp:ListItem Value="1">Klärfälle zur Bearbeitung</asp:ListItem>
                                                            <asp:ListItem Value="2">Bearbeitete Klärfälle</asp:ListItem>
                                                            <asp:ListItem Value="3">Abgeschlossene Klärfälle</asp:ListItem>
                                                            <asp:ListItem Value="A">Alle Klärfälle</asp:ListItem>
                                                            <asp:ListItem Value="E">Einzelvorgang</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr id = "trKennzeichen" runat=server visible="false">
                                                <td class="TextLarge" valign="top" width="130px" 
                                                    style="border-top: 1px solid #dfdfdf;">
                                                        Kennzeichen:
                                                </td>
                                                <td class="TextLarge" valign="center" style="border-top: 1px solid #dfdfdf;">
                                                    <asp:TextBox ID="txtKennz" runat="server" MaxLength="15"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id = "trFahrgestellnummer" runat=server visible="false">
                                                <td class="StandardTableAlternate" valign="top" width="130px">
                                                        Fahrgestellnummer:
                                                </td>
                                                <td class="StandardTableAlternate" valign="center">
                                                    <asp:TextBox ID="txtFahrgestellnummer" runat="server" MaxLength="17"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr id = "trLieferscheinnummer" runat=server visible="false">
                                                <td class="TextLarge" valign="top" width="130px">
                                                        Lieferscheinnummer:
                                                </td>
                                                <td class="TextLarge" valign="center">
                                                    <asp:TextBox ID="txtLieferscheinnummer" runat="server" MaxLength="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                                                                                                                         
                                        </table>                                       
                                        </asp:Panel>

                                        <asp:Panel ID="plEdit" Visible="false" runat="server">
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
                                            Lieferscheinnummer:
                                        </td>
                                        <td class="TextLarge" width="100%">
                                            <asp:Label ID="lblLiefnr" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap align="left">
                                            Fahrgestellnummer:
                                        </td>
                                        <td class="StandardTableAlternate" width="100%">
                                            <asp:Label ID="lblFin" runat="server"></asp:Label>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap align="left">
                                            Kennzeichen</td>
                                        <td class="TextLarge" width="100%">
                                           <asp:Label ID="lblKennzeichen" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                   
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap="nowrap" align="left" valign="top">
                                            Klärfalltext DAD:</td>
                                        <td class="StandardTableAlternate">
                                            <asp:Label ID="lblKlarDAD" runat="server"></asp:Label></td>
                                            
                                    </tr>
                                    <tr>
                                        <td class="TextLarge" nowrap="nowrap" align="left" valign="top">
                                             abgeschlossen durch DAD am:</td>
                                        <td class="TextLarge">
                                            <asp:Label ID="lblEndDat" runat="server"></asp:Label></td>
                                    </tr> 
                                    <tr>
                                        <td class="StandardTableAlternate" nowrap="nowrap" align="left" valign="top">
                                             Bearbeitung durch Carport am:</td>
                                        <td class="StandardTableAlternate">
                                            <asp:Label ID="lblEditCarport" runat="server"></asp:Label></td>
                                    </tr>                                                                          
                                            <tr>
                                                <td align="left" class="TextLarge" nowrap="nowrap" valign="top">
                                                    Klärfalltext: </td>
                                                <td class="TextLarge">
                                                    <cc1:TextAreaControl ID="txtKlaertext" runat="server" Height="90px" MaxLength="180" 
                                                         Width="320px" TextMode="MultiLine" Rows="4"></cc1:TextAreaControl>
                                                 </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="TextLarge" nowrap="nowrap" valign="top">
                                                    &nbsp;</td>
                                                <td class="TextLarge">
                                                  <div style="width: 320px;text-align:right">  <asp:LinkButton ID="lb_Weiter" runat="server" CssClass="StandardButtonTable" 
                                                        Width="88px">•&nbsp;Erfassen</asp:LinkButton> &nbsp;
                                                        <asp:LinkButton ID="lblCancel" runat="server" CssClass="StandardButtonTable" 
                                                        Width="88px">•&nbsp;Abbrechen</asp:LinkButton> </div>  
                                                </td>
                                            </tr>
                                </table>
                                        
                                        
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            &nbsp;
                        </td>
                        <td valign="top">
                        
                        
                            <table id="tblResult" style="margin-bottom: 10px" visible="false"
                                cellspacing="0" cellpadding="5" width="100%" border="0" runat="server">
                                <tr id="trPrint" runat="server" visible="false">
                                    <td class="LabelExtraLarge" align="left" height="9">
                                        &nbsp;
                                    </td>
                                    <td align="right" height="9">
                                        <img style="margin-left: 5px" src="../../../Images/iconXLS.gif" alt="Excel" />
                                        <span style="padding-right:10px;height:18px "><asp:LinkButton ID="lnkCreateExcel1" runat="server">Excel</asp:LinkButton></span>
                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" 
                                            Visible="False">
                                        </asp:DropDownList>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" DataKeyNames="LICENSE_NUM"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                            <RowStyle CssClass="GridTableItem"></RowStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="CARPORT_ID" HeaderText="Carport ID" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="LIEFNR" HeaderText="Lieferscheinnr." />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="CHASSIS_NUM" HeaderText="Fahrgestellnr." />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="LICENSE_NUM" HeaderText="Kennzeichen" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}" DataField="DAT_IMP" HeaderText="Meldungsdatum" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}"
                                                    DataField="DAT_DEMONT" HeaderText="Demontagedatum" />
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataField="ANZ_KENNZ_CPL"
                                                    HeaderText="Anzahl Kennzeichen" />
                                                <asp:TemplateField HeaderText="Vorlage ZBI">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVorlage0" runat="server" Text="Nein" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 0 %>'></asp:Label>
                                                        <asp:Label ID="lblVorlage1" runat="server" Text="Ja" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 1 %>'></asp:Label>
                                                        <asp:Label ID="lblVorlage2" runat="server" Text="Kopie" Visible='<%# DataBinder.Eval(Container, "DataItem.VORLAGE_ZB1_CPL")= 2 %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd.MM.yy}"
                                                    DataField="DAT_ANLAGE_ABW" HeaderText="Anlage Abweichung" />                                                
                                                <asp:TemplateField HeaderText="Bearbeiten/Details">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbShow" runat="server" Width="10px" CommandArgument='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'
                                                            CommandName="Show" ToolTip="Bearbeiten/Details" Height="10px">
																<img alt="Bearbeiten/Details"  src="../../../Images/EditTableHS.png" border="0"/>
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
               </table>
            </td>
        </tr>
        <tr>
            <td>
                <!--#include File="../../../PageElements/Footer.html" -->
            </td>
        </tr>
    </table>
  </form>
</body>
</html>
