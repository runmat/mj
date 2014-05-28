<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report55s.aspx.vb" Inherits="CKG.Components.ComCommon.Report55s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                            </h1>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            
                            <ContentTemplate>
                                <div id="paginationQuery">
                                    <table cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td class="active">
                                                    <asp:Label ID="lblNewSearch" runat="server" Text="Neue Abfrage" Visible="False"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <div id="queryImage">
                                                        <asp:ImageButton ID="ibtNewSearch" runat="server" ImageUrl="/Services/Images/queryArrow.gif" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <asp:Panel ID="divTrenn" runat="server" Visible="false">
                                        <div id="PlaceHolderDiv">
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div id="TableQuery">
                                    <asp:Panel ID="divSelection" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tfoot>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            <tbody>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" colspan="3">
                                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="tr_datumAB" runat="server">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_abDatum" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div class="NeutralCalendar">
                                                            <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                                        </div>
                                                        <ajaxToolkit:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                            PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtAbDatum">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <ajaxToolkit:MaskedEditExtender ID="meetxtAbdatum" runat="server" TargetControlID="txtAbDatum"
                                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                        </ajaxToolkit:MaskedEditExtender>
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="tr_datumBis" runat="server">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_bisDatum" runat="server"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <div class="NeutralCalendar">
                                                            <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                                        </div>
                                                        <ajaxToolkit:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                            PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtBisDatum">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <ajaxToolkit:MaskedEditExtender ID="meetxtBisDatum" runat="server" TargetControlID="txtBisDatum"
                                                            Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                        </ajaxToolkit:MaskedEditExtender>
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" runat="server" id="tr_nurDadZulassungen">
                                                    <td class="firstLeft active">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                            &nbsp;
                                        </div>
                                    </asp:Panel>
                                    <div id="dataQueryFooter">
                                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen</asp:LinkButton>
                                        <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                            Text="Button" />
                                    </div>
                                </div>
                                <div id="Result" runat="Server" visible="false">
                                    <div class="ExcelDiv">
                                        <div align="right" class="rightPadding">
                                            &nbsp;</div>
                                    </div>
                                    <div id="pagination">
                                        <uc2:GridNavigation ID="GridNavigation1" runat="server"></uc2:GridNavigation>
                                    </div>
                                    <div id="data">
                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Horizontal">
                                            <asp:GridView AutoGenerateColumns="False" BackColor="White" runat="server" Width="1050px"
                                                ID="gvBestand" CssClass="GridView" GridLines="None" PageSize="20" AllowPaging="True"
                                                AllowSorting="True">
                                                <PagerSettings Visible="False" />
                                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle Wrap="false" CssClass="ItemStyle" />
                                                <EditRowStyle Wrap="False"></EditRowStyle>
                                                <Columns>
                                                    <asp:TemplateField SortExpression="NAME1" HeaderText="col_NAME1">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_NAME1" runat="server" CommandArgument="NAME1"
                                                                CommandName="sort">col_NAME1</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'
                                                                ID="lblNAME1" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_ZZHANDELSNAME">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZZHANDELSNAME" runat="server" CommandArgument="ZZHANDELSNAME"
                                                                CommandName="sort">col_ZZHANDELSNAME</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'
                                                                ID="lblZZHANDELSNAME" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="KREIS" HeaderText="col_KREIS">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_KREIS" runat="server" CommandArgument="KREIS" CommandName="sort">col_KREIS</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.KREIS") %>'
                                                                ID="lblKREIS" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="AKZ" HeaderText="col_AKZ">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_AKZ" runat="server" CommandArgument="AKZ"
                                                                CommandName="sort">col_AKZ</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.AKZ") %>'
                                                                ID="lblAKZ" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="FGNU" HeaderText="col_FGNU">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_FGNU" runat="server" CommandArgument="FGNU"
                                                                CommandName="sort">col_FGNU</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FGNU") %>'
                                                                ID="lblFGNU" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="BRNR" HeaderText="col_BRNR">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_BRNR" runat="server" CommandArgument="BRNR"
                                                                CommandName="sort">col_BRNR</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BRNR") %>'
                                                                ID="lblBRNR" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZLOESCH" HeaderText="col_ZZLOESCH">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZZLOESCH" runat="server" CommandArgument="ZZLOESCH" CommandName="sort">col_ZZLOESCH</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLOESCH") %>'
                                                                ID="lblZZLOESCH" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZSTATUSDATUM" HeaderText="col_ZZSTATUSDATUM">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZZSTATUSDATUM" runat="server" CommandArgument="ZZSTATUSDATUM"
                                                                CommandName="sort">col_ZZSTATUSDATUM</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZSTATUSDATUM","{0:d}") %>'
                                                                ID="lblZZSTATUSDATUM" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZSTATUSUHRZEIT" HeaderText="col_ZZSTATUSUHRZEIT">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZZSTATUSUHRZEIT" runat="server" CommandArgument="ZZSTATUSUHRZEIT"
                                                                CommandName="sort">col_ZZSTATUSUHRZEIT</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZSTATUSUHRZEIT") %>'
                                                                ID="lblZZSTATUSUHRZEIT" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField SortExpression="VGSTAT" HeaderText="col_VGSTAT">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_VGSTAT" runat="server" CommandArgument="VGSTAT"
                                                                CommandName="sort">col_VGSTAT</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VGSTAT") %>'
                                                                ID="lblVGSTAT" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZUDA" HeaderText="col_ZUDA">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZUDA" runat="server" CommandArgument="ZUDA" CommandName="sort">col_ZUDA</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZUDA","{0:d}") %>'
                                                                ID="lblZUDA" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </div>
                                    <div id="dataFooter">
                                        &nbsp;
                                    </div>
                                </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
