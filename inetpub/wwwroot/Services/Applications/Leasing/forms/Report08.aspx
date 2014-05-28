<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report08.aspx.cs" Inherits="Leasing.forms.Report08"  MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active">
                                                Neue Abfrage
                                            </td>
                                            <td align="right">
                                                <div id="queryImage">
                                                    <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                        ToolTip="Abfrage öffnen" Visible="false" onclick="NewSearch_Click" />
                                                    <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen"
                                                        ImageUrl="../../../Images/queryArrowUp.gif" 
                                                        Visible="false" onclick="NewSearchUp_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                    <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Zulassungsdatum von:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Zulassungsdatum bis:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtDatumBis" runat="server" CssClass="TextBoxNormal" Width="200px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                        Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>


                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Leasingvertragsnr.:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal"
                                                        Width="200px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Kundennr. Händler:
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtHaendlernr" runat="server" CssClass="TextBoxNormal" 
                                                        Width="200px"></asp:TextBox>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />&nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>


                            </asp:Panel>
                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                                    OnClick="lbCreate_Click">» Suchen </asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" runat="server" OnClick="lnkCreateExcel1_Click">Excel herunterladen</asp:LinkButton>
                                        </span>
                                    </div>
                                </div>
                                <div id="pagination">
                                    <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                </div>
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" ID="GridView1"
                                                    CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True" AllowSorting="True"
                                                    OnSorting="GridView1_Sorting">
                                                    <PagerSettings Visible="False" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>

                                                        <asp:TemplateField SortExpression="VDATU" HeaderText="col_Zulassungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandName="Sort" CommandArgument="VDATU">col_Zulassungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblZulassungsdatum" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU", "{0:d}") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>




                                                        <asp:TemplateField SortExpression="ZZLVNR" HeaderText="col_Leasingvertragsnummer">
                                                            
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Leasingvertragsnummer" runat="server" CommandName="Sort" CommandArgument="ZZLVNR">col_Leasingvertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl22" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLVNR") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField SortExpression="NAME1_HAENDLER" HeaderText="col_Haendler">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Haendler" runat="server" CommandName="Sort" CommandArgument="NAME1_HAENDLER ">col_Haendler</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl23" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_HAENDLER ") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ORT_HAENDLER" HeaderText="col_Ort">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Ort" runat="server" CommandName="Sort" CommandArgument="ORT_HAENDLER">col_Ort</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblOrt" Text='<%# DataBinder.Eval(Container, "DataItem.ORT_HAENDLER") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="NAME1_ZH" HeaderText="col_Halter">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Halter" runat="server" CommandName="Sort" CommandArgument="NAME1_ZH">col_Halter</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblHalter" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_ZH") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:TemplateField SortExpression="ORT01_ZH" HeaderText="col_HalterOrt">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_HalterOrt" runat="server" CommandName="Sort" CommandArgument="ORT01_ZH">col_HalterOrt</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblHalterOrt" Text='<%# DataBinder.Eval(Container, "DataItem.ORT01_ZH") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div id="dataFooter">
                                &nbsp;<asp:HiddenField ID="hField" runat="server" Value="0" />
                            </div>
                            
                        </ContentTemplate>
                         <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                    </asp:UpdatePanel>
                    
                </div>
                
                
                
            </div>
        </div>
    </div>
   
</asp:Content>
