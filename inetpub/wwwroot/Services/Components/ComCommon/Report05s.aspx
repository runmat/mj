<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report05s.aspx.vb" Inherits="CKG.Components.ComCommon.Report05s"
    MasterPageFile="../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../PageElements/GridNavigation.ascx" %>
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
                                                <td align="left" class="active" style="vertical-align: middle;">
                                                    Abfrageoptionen
                                                </td>
                                                <td class="active" style="width: 25px;" align="right">
                                                    <asp:ImageButton ID="NewSearch2" runat="server" Width="17px" OnClick="NewSearch_Click" />
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
                                                    <td class="firstLeft active" colspan="3" >
                                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery">
                                                    <td class="firstLeft active" nowrap="nowrap" colspan="3" style="padding-bottom:10px">
                                                        Selektionszeitraum maximal 1 Monat
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
                                                <tr class="formquery" id="tr_datumBis" runat="server">
                                                    <td class="firstLeft active">
                                                        <asp:Label ID="lbl_bisDatum" runat="server"></asp:Label>
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
                                        <img src="../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel1" ForeColor="White" runat="server">Excel herunterladen</asp:LinkButton></span></div>
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
                                                    <asp:BoundField Visible="false" HeaderText="EQUNR" DataField="EQUNR" ReadOnly="true" />
                                                    <asp:TemplateField SortExpression="CHASSIS_NUM" HeaderText="col_Fahrgestellnummer">
                                                        <HeaderTemplate >
                                                            <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandArgument="Fahrgestellnummer"
                                                                CommandName="sort">col_Fahrgestellnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.CHASSIS_NUM") %>'
                                                                ID="lblFahrgestellnummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="REFNR" HeaderText="col_Kundenreferenznummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kundenreferenznummer" runat="server" CommandArgument="REFNR"
                                                                CommandName="sort">col_Kundenreferenznummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.REFNR") %>'
                                                                ID="lblKundenreferenznummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="LIZNR" HeaderText="col_EQUILIZNR">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EQUILIZNR" runat="server" CommandArgument="LIZNR" CommandName="sort">col_EQUILIZNR</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LIZNR") %>'
                                                                ID="lblEQUILIZNR" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZREFERENZ1" HeaderText="col_EQUIZZREFERENZ1">

                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EQUIZZREFERENZ1" runat="server" CommandArgument="ZZREFERENZ1"
                                                                CommandName="sort">col_EQUIZZREFERENZ1</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ1") %>'
                                                                ID="lblEQUIZZREFERENZ1" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZREFERENZ2" HeaderText="col_EQUIZZREFERENZ2">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EQUIZZREFERENZ2" runat="server" CommandArgument="ZZREFERENZ2"
                                                                CommandName="sort">col_EQUIZZREFERENZ2</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFERENZ2") %>'
                                                                ID="lblEQUIZZREFERENZ2" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="NAME1_HAENDLER" HeaderText="col_NAME1Haendler">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_NAME1Haendler" runat="server" CommandArgument="NAME1_HAENDLER"
                                                                CommandName="sort">col_NAME1Haendler</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1_HAENDLER") %>'
                                                                ID="lblNAME1Haendler" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ORT_HAENDLER" HeaderText="col_ORTHAENDLER">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ORTHAENDLER" runat="server" CommandArgument="ORT_HAENDLER"
                                                                CommandName="sort">col_ORTHAENDLER</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ORT_HAENDLER") %>'
                                                                ID="lblORTHAENDLER" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ERDAT" HeaderText="col_ZBRIEFEINGANG">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZBRIEFEINGANG" runat="server" CommandArgument="ERDAT" CommandName="sort">col_ZBRIEFEINGANG</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ERDAT","{0:d}") %>'
                                                                ID="lblZBRIEFEINGANG" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZDATBEAUFTRAGUNG" HeaderText="col_ZDATBEAUFTRAGUNG">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZDATBEAUFTRAGUNG" runat="server" CommandArgument="ZDATBEAUFTRAGUNG"
                                                                CommandName="sort">col_ZDATBEAUFTRAGUNG</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZDATBEAUFTRAGUNG","{0:d}") %>'
                                                                ID="lblZDATBEAUFTRAGUNG" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="VDATU" HeaderText="col_Wunschlieferdatum">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Wunschlieferdatum" runat="server" CommandArgument="VDATU"
                                                                CommandName="sort">col_Wunschlieferdatum</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.VDATU","{0:d}") %>'
                                                                ID="lblWunschlieferdatum" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="REPLA_DATE" HeaderText="col_ZulassungsdatumAusEqui">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZulassungsdatumAusEqui" runat="server" CommandArgument="REPLA_DATE"
                                                                CommandName="sort">col_ZulassungsdatumAusEqui</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.REPLA_DATE","{0:d}") %>'
                                                                ID="lblZulassungsdatumAusEqui" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="LICENSE_NUM" HeaderText="col_Kennzeichen">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandArgument="LICENSE_NUM"
                                                                CommandName="sort">col_Kennzeichen</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.LICENSE_NUM") %>'
                                                                ID="lblKennzeichen" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="BEZEI_KVGR3" HeaderText="col_Zulassungsart">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Zulassungsart" runat="server" CommandArgument="BEZEI_KVGR3" CommandName="sort">col_Zulassungsart</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.BEZEI_KVGR3") %>'
                                                                ID="lblZulassungsart" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZULORT" HeaderText="col_ZULORT">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZULORT" runat="server" CommandArgument="ZULORT" CommandName="sort">col_ZULORT</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZULORT") %>'
                                                                ID="lblZULORT" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="EQUNR" HeaderText="col_Equipmentnummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Equipmentnummer" runat="server" CommandArgument="EQUNR" CommandName="sort">col_Equipmentnummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.EQUNR") %>'
                                                                ID="lblEquipmentnummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Anschrift_ZH" HeaderText="col_Anschrift_ZH">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Anschrift_ZH" runat="server" CommandArgument="Anschrift_ZH"
                                                                CommandName="sort">col_Anschrift_ZH</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anschrift_ZH") %>'
                                                                ID="lblAnschrift_ZH" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Anschrift_ZE" HeaderText="col_Anschrift_ZE">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Anschrift_ZE" runat="server" CommandArgument="Anschrift_ZE"
                                                                CommandName="sort">col_Anschrift_ZE</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anschrift_ZE") %>'
                                                                ID="lblAnschrift_ZE" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZZCOCKZ" HeaderText="col_ZZCOCKZ">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_ZZCOCKZ" runat="server" CommandArgument="ZZCOCKZ" CommandName="sort">col_ZZCOCKZ</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZCOCKZ") %>'
                                                                ID="lblZZCOCKZ" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Anschrift_ZP" HeaderText="col_Anschrift_ZP">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_Anschrift_ZP" runat="server" CommandArgument="Anschrift_ZP"
                                                                CommandName="sort">col_Anschrift_ZP</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Anschrift_ZP") %>'
                                                                ID="lblAnschrift_ZP" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZEVBNR" HeaderText="col_EVBNummer">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EVBNummer" runat="server" CommandArgument="ZEVBNR" CommandName="sort">col_EVBNummer</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZEVBNR") %>'
                                                                ID="lblEVBNummer" Visible="true"> </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="ZEVBTXT" HeaderText="col_EVBNummerBemText">
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="col_EVBNummerBemText" runat="server" CommandArgument="ZEVBTXT"
                                                                CommandName="sort">col_EVBNummerBemText</asp:LinkButton>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZEVBTXT") %>'
                                                                ID="lblEVBNummerBemText" Visible="true"> </asp:Label>
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
</asp:Content>
