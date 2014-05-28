<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report03s.aspx.cs" Inherits="AppMBB.forms.Report03s" MasterPageFile="../Master/AppMaster.Master" %>


<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="/services/PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                       </Triggers>
                        <ContentTemplate>

                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage starten
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif"
                                                ToolTip="Abfrage öffnen" OnClick="NewSearch_Click" Visible="false" />
                                            <asp:ImageButton ID="NewSearchUp" runat="server" ToolTip="Abfrage schließen"
                                                        ImageUrl="../../../Images/queryArrowUp.gif"  Visible="false"
                                                         onclick="NewSearchUp_Click" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="2">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                            <asp:Label ID="lblNoData" runat="server" Font-Bold="True" Visible="False" EnableViewState="False"></asp:Label>
                                        </td>
                                       
                                    </tr>
                                   
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width:100px">
                                            Zeitraum von:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtDatumVon" runat="server" CssClass="TextBoxNormal" Width="100px" ></asp:TextBox>
                                            <cc1:CalendarExtender ID="CE_DatumVon" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                Animated="false" Enabled="True" TargetControlID="txtDatumVon">
                                            </cc1:CalendarExtender>
                                            <cc1:MaskedEditExtender ID="MEE_Datumvon" runat="server" TargetControlID="txtDatumvon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width:100px">
                                            Zeitraum bis:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtDatumBis" Width="100px" runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                            <cc1:CalendarExtender ID="CE_DatumBis" runat="server" Format="dd.MM.yyyy" PopupPosition="BottomLeft"
                                                Animated="false" Enabled="True" TargetControlID="txtDatumBis">
                                            </cc1:CalendarExtender>
                                             <cc1:MaskedEditExtender ID="MEE_DatumBis" runat="server" TargetControlID="txtDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" />
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
                                                <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVertragsnummer" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblKennzeichen" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="DATAB" HeaderText="col_Briefeingang">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Briefeingang" runat="server" CommandName="Sort" CommandArgument="DATAB">col_Briefeingang</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblBriefeingang" Text='<%# DataBinder.Eval(Container, "DataItem.DATAB", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="EXPIRY_DATE" HeaderText="col_Stillegung">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Stillegung" runat="server" CommandName="Sort" CommandArgument="EXPIRY_DATE">col_Stillegung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStillegung" Text='<%# DataBinder.Eval(Container, "DataItem.EXPIRY_DATE", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ZZTMPDT" HeaderText="col_Versanddatum">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versanddatum" runat="server" CommandName="Sort" CommandArgument="ZZTMPDT">col_Versanddatum</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersanddatum" Text='<%# DataBinder.Eval(Container, "DataItem.ZZTMPDT", "{0:d}") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="ABCKZ" HeaderText="col_Versandart">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Versandart" runat="server" CommandName="Sort" CommandArgument="ABCKZ">col_Versandart</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersandart" Text='<%# DataBinder.Eval(Container, "DataItem.ABCKZ") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField SortExpression="NAME1" HeaderText="col_Name">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Name" runat="server" CommandName="Sort" CommandArgument="NAME1">col_Name</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblName" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="POST_CODE1" HeaderText="col_PLZ">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_PLZ" runat="server" CommandName="Sort" CommandArgument="POST_CODE1">col_PLZ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPLZ" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField SortExpression="CITY1" HeaderText="col_Ort">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ort" runat="server" CommandName="Sort" CommandArgument="CITY1">col_Ort</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblOrt" Text='<%# DataBinder.Eval(Container, "DataItem.CITY1") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="hField" runat="server" Value="0" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>

                    </ContentTemplate>
                    </asp:UpdatePanel>

                    <div style="float: right; width: 100%; text-align: right; padding-top: 15px">
                        <asp:LinkButton ID="lbtnBack" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="lbtnBack_Click" Visible="false">» Zurück </asp:LinkButton>
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>