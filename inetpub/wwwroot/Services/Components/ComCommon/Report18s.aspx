<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report18s.aspx.vb" Inherits="CKG.Components.ComCommon.Report18s"
    MasterPageFile="../../MasterPage/Services.Master" %>
    <%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkCreateExcel1" />
                        </Triggers>
                            <ContentTemplate>
                                <div id="paginationQuery">
                                <table cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr>
                                            <td class="active" style="width: 25px;">
                                                <asp:ImageButton ID="NewSearch" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                            <td class="active">
                                                Neue Abfrage starten
                                            </td>
                                            <td class="active" style="width: 25px;" align="right">
                                                <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                             <asp:Panel ID="Panel1" runat="server">
                                <div id="TableQuery">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
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
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    Auswahl
                                                </td>
                                                <td nowrap="nowrap">
                                                    <span>
                                                        <asp:RadioButton ID="rbEingang" runat="server" Text="Eingänge" GroupName="Auswahl"
                                                            Checked="True"  AutoPostBack="True" /></span> 
                                                </td>
                                                <td nowrap="nowrap">
                                                    <span>
                                                     <asp:RadioButton ID="rbAusgang" runat="server" Text="Ausgänge" 
                                                        GroupName="Auswahl" AutoPostBack="True" /></span>
                                                </td>
                                            </tr>
                                            <tr id="trAusgangZusatz" runat="server" visible="false" class="formquery">
                                                <td class="firstLeft active">
                                                    
                                                </td>
                                                <td nowrap="nowrap">
                                                    
                                                </td>
                                                <td nowrap="nowrap">
                                                   
                                                        <asp:RadioButtonList ID="rblAusgangZusatz" runat="server" RepeatLayout="Flow">
                                                            <asp:ListItem Value="A">Alle</asp:ListItem>
                                                            <asp:ListItem Value="1">Temporär</asp:ListItem>
                                                            <asp:ListItem Value="2" Selected="True" >Endgültig</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    
                                                </td>
                                            </tr>

                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    ab Datum
                                                </td>
                                                <td>
                                                    <div class="NeutralCalendar">
                                                        <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                                    </div>
                                                    <ajaxToolkit:CalendarExtender ID="txtAbDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtAbDatum">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="meetxtAbdatum" runat="server" TargetControlID="txtAbDatum"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                                <td width="100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    bis Datum
                                                </td>
                                                <td>
                                                    <div class="NeutralCalendar">
                                                        <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                                    </div>
                                                    <ajaxToolkit:CalendarExtender ID="txtBisDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                        PopupPosition="BottomLeft" Animated="false" Enabled="True" TargetControlID="txtBisDatum">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="meetxtBisDatum" runat="server" TargetControlID="txtBisDatum"
                                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                                <td width="100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                <td width="100%">
                                                    &nbsp;</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                 <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </asp:Panel>
                                <div id="dataQueryFooter">
                                    <asp:LinkButton ID="cmdcreate" Text="Erstellen" Height="16px" Width="78px" runat="server"
                                        CssClass="Tablebutton"></asp:LinkButton>
                                </div>

                                 <div id="Result" runat="Server" visible="false">
                                <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton>
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
                                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="0" CellSpacing="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0"
                                                    AllowSorting="true" AllowPaging="True" CssClass="GridView" PageSize="20">
                                                    <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                                    <PagerSettings Visible="False" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <EmptyDataRowStyle BackColor="#DFDFDF" />
                                                    <Columns>
                                                    <asp:TemplateField SortExpression="ZZHERST_TEXT" HeaderText="col_Hersteller">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hersteller" runat="server" CommandArgument="ZZHERST_TEXT"
                                                                    CommandName="sort">col_Hersteller</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHERST_TEXT") %>'
                                                                    ID="lblHersteller" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZHANDELSNAME" HeaderText="col_Handelsname">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Handelsname" runat="server" CommandArgument="ZZHANDELSNAME"
                                                                    CommandName="sort">col_Handelsname</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHANDELSNAME") %>'
                                                                    ID="lblHandelsname" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="LIZNR" HeaderText="col_Vertragsnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandArgument="LIZNR"
                                                                    CommandName="sort">col_Vertragsnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                                    ID="lblVertragsnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="CHASSIS_NUM"
                                                                    CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'
                                                                    ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="IDENT5" HeaderText="col_Ident5">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Ident5" runat="server" CommandArgument="IDENT5"
                                                                    CommandName="sort">col_Ident5</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IDENT5") %>'
                                                                    ID="lblIdent5" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="TIDNR" HeaderText="col_Briefnummer">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Briefnummer" runat="server" CommandArgument="TIDNR" CommandName="sort">col_Briefnummer</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TIDNR") %>'
                                                                    ID="lblBriefnummer" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZKENN" HeaderText="col_Kennzeichen">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="ZZKENN"
                                                                    CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'
                                                                    ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZLABEL" HeaderText="col_Label">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Label" runat="server" CommandArgument="ZZLABEL"
                                                                    CommandName="sort">col_Label</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZLABEL") %>'
                                                                    ID="lblLabel" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ERDAT" HeaderText="col_Erstellungsdatum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Erstellungsdatum" runat="server" CommandArgument="ERDAT"
                                                                    CommandName="sort">col_Erstellungsdatum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT","{0:d}") %>'
                                                                    ID="lblErstellungsdatum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField SortExpression="POST_CODE1" HeaderText="col_POST_CODE1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_POST_CODE1" runat="server" CommandName="Sort" CommandArgument="POST_CODE1">col_POST_CODE1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblAdresse2" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'>
                                                                </asp:Label><br />
                                                                <asp:Label runat="server" ID="lblAdresse3" Text='<%# DataBinder.Eval(Container, "DataItem.NAME2") %>'>
                                                                </asp:Label><br />
                                                                <asp:Label runat="server" ID="lblAdresse4" Text='<%# DataBinder.Eval(Container, "DataItem.STREET") %>'>
                                                                </asp:Label>&nbsp;
                                                                <asp:Label runat="server" ID="lblAdresse5" Text='<%# DataBinder.Eval(Container, "DataItem.HOUSE_NUM1") %>'>
                                                                </asp:Label><br />
                                                                <asp:Label runat="server" ID="lblAdresse6" Text='<%# DataBinder.Eval(Container, "DataItem.POST_CODE1") %>'>
                                                                </asp:Label>&nbsp;
                                                                <asp:Label runat="server" ID="lblAdresse7" Text='<%# DataBinder.Eval(Container, "DataItem.CITY1") %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_Referenz1">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz1" runat="server" CommandArgument="ZZREFERENZ1"
                                                                    CommandName="sort">col_Referenz1</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                                    ID="lblReferenz1" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZREFERENZ2" HeaderText="col_Referenz2">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Referenz2" runat="server" CommandArgument="ZZREFERENZ2"
                                                                    CommandName="sort">col_Referenz2</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ2") %>'
                                                                    ID="lblReferenz2" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZCOCKZ" HeaderText="col_COC">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_COC" runat="server" CommandArgument="ZZCOCKZ"
                                                                    CommandName="sort">col_COC</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'
                                                                    ID="lblCOC" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZVVS_SCHLUESSEL" HeaderText="col_VVSSchluessel">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_VVSSchluessel" runat="server" CommandArgument="ZZVVS_SCHLUESSEL"
                                                                    CommandName="sort">col_VVSSchluessel</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZVVS_SCHLUESSEL") %>'
                                                                    ID="lblVVSSchluessel" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZTYP_SCHL" HeaderText="col_TYPSchluessel">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_TYPSchluessel" runat="server" CommandArgument="ZZTYP_SCHL"
                                                                    CommandName="sort">col_TYPSchluessel</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZTYP_SCHL") %>'
                                                                    ID="lblTYPSchluessel" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZHUBRAUM" HeaderText="col_Hubraum">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Hubraum" runat="server" CommandArgument="ZZHUBRAUM"
                                                                    CommandName="sort">col_Hubraum</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZHUBRAUM") %>'
                                                                    ID="lblHubraum" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZKRAFTSTOFF_TXT" HeaderText="col_Kraftstoffart">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Kraftstoffart" runat="server" CommandArgument="ZZKRAFTSTOFF_TXT"
                                                                    CommandName="sort">col_Kraftstoffart</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKRAFTSTOFF_TXT") %>'
                                                                    ID="lblKraftstoffart" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField SortExpression="ZZNENNLEISTUNG" HeaderText="col_Nennleistung">
                                                            <HeaderTemplate>
                                                                <asp:LinkButton ID="col_Nennleistung" runat="server" CommandArgument="ZZNENNLEISTUNG"
                                                                    CommandName="sort">col_Nennleistung</asp:LinkButton>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZNENNLEISTUNG") %>'
                                                                    ID="lblNennleistung" Visible="true"> </asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                                </div> </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
</asp:Content>
