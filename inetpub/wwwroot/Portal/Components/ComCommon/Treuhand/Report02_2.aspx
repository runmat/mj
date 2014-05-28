<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report02_2.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report02_2" %>

<%@ Register TagPrefix="uc1" TagName="Header" Src="../../../PageElements/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Styles" Src="../../../PageElements/Styles.ascx" %>
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
    <table cellspacing="0" cellpadding="2" width="100%" align="center">
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
                            <asp:Label ID="lblHead" runat="server"></asp:Label><asp:Label ID="lblPageTitle" runat="server"> (Fahrzeugauswahl)</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" width="120">
                            <table id="Table2" bordercolor="#ffffff" cellspacing="0" cellpadding="0" width="120"
                                border="0">
                                <tr>
                                    <td class="TaskTitle">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <table id="Table6" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td class="TaskTitle" valign="top">
                                        <asp:HyperLink ID="lnkKreditlimit" runat="server" CssClass="TaskTitle" NavigateUrl="Report02.aspx">Kundenauswahl</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                            <table id="Table5" cellspacing="0" cellpadding="0" width="100%" border="0">
                                <tr>
                                    <td valign="top" align="left" colspan="3" height="41">
                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                            <tr>
                                                <td  colspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                            <td class="TextLarge" style="width:10%">
                                                                <asp:Label ID="lblAktion" runat="server"  Text="Aktion:"></asp:Label></td>
                                                                   <td class="TextLarge" style="width:88%">
                                                            <asp:RadioButton ID="rbKeineZBII" runat="server" GroupName="Vorgang" 
                                                                Text="Sperr-/Freigabesatz mehrfach vorhanden" AutoPostBack="True" />
                                                            </td>                                               
                                            
                                            </tr>
                                            <tr>
                                                            <td class="TextLarge" style="width:10%">
                                                                </td>
                                                                   <td class="TextLarge" style="width:88%">
                                                            <asp:RadioButton ID="rbohneDokumente" runat="server" GroupName="Vorgang" 
                                                                Text="Daten ohne Dokumente" AutoPostBack="True" /></td>                                               
                                            
                                            </tr>
                                            <tr>
                                                            <td class="TextLarge" style="width:10%">
                                                                &nbsp;</td>
                                                                   <td class="TextLarge" style="width:88%">
                                                                      <asp:RadioButton ID="rbAndererTG" runat="server" GroupName="Vorgang" 
                                                                Text="Durch anderen Treugeber gesperrt" AutoPostBack="True" /></td>                                               
                                            
                                            </tr>
                                            <tr>
                                                <td  colspan="2">
                                                    &nbsp;
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                                                                                                                  
                                            <tr>
                                                <td class="LabelExtraLarge" align="left" width="618" height="9">
                                                <img alt="" id="ExcelImg" runat="server" visible="false" src="../../../images/excel.gif" style="width: 16px; height: 16px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" visible="false" runat="server">Excel</asp:LinkButton>                                                        
                                                    </td>
                                                    
                                                    
                                                    
                                                    <td nowrap="nowrap" align="right" height="9">
                                                        <p align="right">
&nbsp;&nbsp;
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" Height="14px">
                                                            </asp:DropDownList>
                                                        </p>
                                                    </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="left" colspan="3">
                                        <asp:GridView ID="GridView1" runat="server" Width="100%" BackColor="White" PageSize="50"
                                            headerCSS="tableHeader" bodyCSS="tableBody" CssClass="tableMain" bodyHeight="400"
                                            AllowSorting="True" AllowPaging="True" AutoGenerateColumns="False">
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <HeaderStyle Wrap="False" CssClass="GridTableHead"></HeaderStyle>
                                            <Columns>

                                                <asp:TemplateField SortExpression="Fahrgestellnummer" HeaderText="col_Fahrgestellnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="Fahrgestellnummer">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFahrgestellnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Fahrgestellnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NummerZBII" HeaderText="col_NummerZBII">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_NummerZBII" runat="server" CommandName="Sort" CommandArgument="NummerZBII">col_NummerZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTIDNR" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Vertragsnummer" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="Vertragsnummer">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                     <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.Vertragsnummer") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <asp:TemplateField SortExpression="Vorgang" HeaderText="col_Vorgang">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vorgang" runat="server" CommandName="Sort" CommandArgument="Fehlercode">col_Vorgang</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVorgang1" Text="Freigabe" Visible='<%# DataBinder.Eval(Container, "DataItem.Vorgang") = "F" %>'>
                                                        </asp:Label>
                                                        <asp:Label runat="server" ID="lblVorgang2" Text="Sperrung" Visible='<%# DataBinder.Eval(Container, "DataItem.Vorgang") = "S" %>'>
                                                        </asp:Label>                                                        
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
                                               <asp:TemplateField SortExpression="Fehlercode" HeaderText="col_Fehlercode">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fehlercode" runat="server" CommandName="Sort" CommandArgument="Fehlercode">col_Fehlercode</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERROR" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlercode") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Fehlertext" HeaderText="col_Fehlertext">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fehlertext" runat="server" CommandName="Sort" CommandArgument="ERROR_TXT">col_Fehlertext</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblERROR_TXT" Text='<%# DataBinder.Eval(Container, "DataItem.Fehlertext") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="bearbeitet" HeaderText="col_Bearbeitet">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bearbeitet" runat="server" CommandName="Sort" CommandArgument="bearbeitet">col_Bearbeitet</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbearbeitet" Text='<%# DataBinder.Eval(Container, "DataItem.bearbeitet") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="bearbeitetam" HeaderText="col_Bearbeitetam">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Bearbeitetam" runat="server" CommandName="Sort" CommandArgument="bearbeitetam">col_Bearbeitetam</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblbearbeitetam" Text='<%# DataBinder.Eval(Container, "DataItem.bearbeitetam", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Erledigt" HeaderText="col_Erledigt">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Erledigt" runat="server" CommandName="Sort" CommandArgument="bearbeitetam">col_Erledigt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblErledigt" Text='<%# DataBinder.Eval(Container, "DataItem.Erledigt", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                                                                      
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120">
                            &nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
