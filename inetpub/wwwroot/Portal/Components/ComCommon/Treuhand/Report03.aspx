<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report03.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Report03" %>

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
                                        &nbsp;</td>
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
                                                <td  colspan="2">
                                                    &nbsp;
                                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                </td>
                                            </tr>
                                                                                                                                  
                                            <tr>
                                                <td class="LabelExtraLarge" align="left" width="618" height="9">
                                                <img alt="" id="ExcelImg" runat="server" visible="false" src="../../../images/excel.gif" style="width: 16px; height: 16px" /> <asp:LinkButton
                                                                CssClass="ExcelButton" ID="lnkCreateExcel" visible="false" runat="server">Excel herunterladen</asp:LinkButton>                                                        
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
                                                <asp:TemplateField SortExpression="Name_AG" HeaderText="col_Name_AG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Name_AG" runat="server" CommandName="Sort" CommandArgument="Name_AG">col_Name_AG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblName_AG" Text='<%# DataBinder.Eval(Container, "DataItem.Name_AG") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Name_TG" HeaderText="col_Name_TG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Name_TG" runat="server" CommandName="Sort" CommandArgument="Name_TG">col_Name_TG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                     <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblName_TG" Text='<%# DataBinder.Eval(Container, "DataItem.Name_TG") %>'>
                                                       </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>                                                
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
                                                        <asp:Label runat="server" ID="lblNummerZBII" Text='<%# DataBinder.Eval(Container, "DataItem.NummerZBII") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Kennzeichen" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">col_Kennzeichen</asp:LinkButton>
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
                                               
                                               <asp:TemplateField SortExpression="Versandstatus" HeaderText="col_Versandstatus">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandstatus" runat="server" CommandName="Sort" CommandArgument="Versandstatus">col_Versandstatus</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersandstatus" Text='<%# DataBinder.Eval(Container, "DataItem.Versandstatus") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="Versandadresse" HeaderText="col_Versandadresse">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandadresse" runat="server" CommandName="Sort" CommandArgument="Versandadresse">col_Versandadresse</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersandadresse" Text='<%# DataBinder.Eval(Container, "DataItem.Versandadresse") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                           <asp:TemplateField SortExpression="TG" HeaderText="col_TG">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_TG" runat="server" CommandName="Sort" CommandArgument="TG">col_TG</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblTG" Text='<%# DataBinder.Eval(Container, "DataItem.TG") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>   
                                                   <asp:TemplateField SortExpression="Referenznummer" HeaderText="col_Referenznummer">
                                                       <HeaderTemplate>
                                                           <asp:LinkButton ID="col_Referenznummer" runat="server" CommandName="Sort" CommandArgument="Referenznummer">col_Referenznummer</asp:LinkButton>
                                                       </HeaderTemplate>
                                                       <ItemTemplate>
                                                           <asp:Label runat="server" ID="lblReferenznummer" Text='<%# DataBinder.Eval(Container, "DataItem.Referenznummer") %>'>
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
