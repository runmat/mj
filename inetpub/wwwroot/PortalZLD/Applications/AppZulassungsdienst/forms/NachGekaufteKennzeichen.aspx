<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NachGekaufteKennzeichen.aspx.cs"
    Inherits="AppZulassungsdienst.forms.NachGekaufteKennzeichen" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2011.3.1305.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>                   
                            <div id="paginationQuery">
                                &nbsp;
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="5" width="100%">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" style="color: Green;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="lblDatum" runat="server">Datum:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:TextBox ID="txtDatum" runat="server" CssClass="TextBoxNormal" Width="78px"></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="MEEtxtDatum" runat="server" TargetControlID="txtDatum"
                                                    Mask="99/99/9999" MaskType="Date">
                                                </ajaxToolkit:MaskedEditExtender>
                                                <ajaxToolkit:CalendarExtender ID="CEtxtDatum" runat="server" TargetControlID="txtDatum">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                            <td class="firstLeft active" style="height: 30px">
                                                <asp:Label ID="lblLieferant" runat="server">Lieferant:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 30px;">
                                                <asp:DropDownList ID="ddlLiefer" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLiefer_SelectedIndexChanged"
                                                    Style="width: 280px">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active" style="height: 30px;">
                                                <asp:Label runat="server">Lieferscheinnr.:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%; height: 30px;">
                                                <asp:TextBox runat="server" ID="txtLieferscheinnummer" Style="width: 170px" CssClass="TextBoxNormal"
                                                    MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="Label1" runat="server">Menge in St&uuml;ck:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:TextBox ID="txtMenge" runat="server" CssClass="TextBoxNormal" Width="78px" 
                                                    MaxLength="3"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FTBEtxtMenge" runat="server" TargetControlID="txtMenge"
                                                    FilterMode="ValidChars" FilterType="Numbers">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="lblArtikel" runat="server">Artikel:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:DropDownList ID="ddlArtikel" runat="server" CssClass="TextBoxNormal" Width="280px" AutoPostBack="true"
                                                    OnSelectedIndexChanged="ddlArtikel_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:Label ID="lblPreis" runat="server">Preis:</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="height: 36px">
                                                <asp:TextBox ID="txtPreis" runat="server" CssClass="TextBoxNormal" Width="75px"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FTBEtxtPreis" runat="server" TargetControlID="txtPreis"
                                                    FilterMode="ValidChars" FilterType="Custom" ValidChars="0,1,2,3,4,5,6,7,8,9,,">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="7">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    &nbsp;
                                </div>
                            </div>
                            <div id="dataQueryFooter" class="dataQueryFooter">
                                <asp:LinkButton ID="lbNewLine" runat="server" Width="78px" CssClass="Tablebutton"
                                    OnClick="lbNewLine_Click">hinzuf&uuml;gen</asp:LinkButton>&nbsp;
                                <asp:LinkButton ID="lbShowBuchungen" runat="server" CssClass="TablebuttonXLarge" Width="155px" Height="17px"
                                                OnClick="lbToggleBuchungen_Click" />
                                <asp:ImageButton ID="ibHideBuchungen" runat="server" Width="16px" Height="16px" AlternateText="Übersicht 'erfasste Käufe' schließen" 
                                                Visible="False" OnClick="lbToggleBuchungen_Click" ImageUrl="~/Images/Delete01.jpg" />
                            </div>                         
                            <div id="data">
                                <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="divKaeufe" runat="server" Visible="False">
                                                <strong>Übersicht erfasste Käufe </strong> für <asp:Label id="lblGewaehlterLieferant" style="font-weight: bold" runat="server" /> <span class="smallHintLightHighlight" style="margin-left: 10px;">(Lieferantenwechsel über Auswahl-Box oben)</span>
                                                <br/><br/>
                                                Bisher erfasst: <asp:Label id="lblAnzahlKaeufe" style="font-weight: bold" Text="-" runat="server"/> Lieferscheine
                                                <br/><br/>
                                                
                                                <telerik:RadGrid ID="rgGrid1" runat="server" Width="99%"
                                                    AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default" AllowPaging="True" AllowSorting="True"  
                                                    OnNeedDataSource="rgGrid1_NeedDataSource" OnDetailTableDataBind="rgGrid1_DetailTableDataBind">
                                                    <ClientSettings AllowKeyboardNavigation="true" >
                                                        <Scrolling ScrollHeight="480px" AllowScroll="True" />
                                                    </ClientSettings>
                                                    <ItemStyle CssClass="ItemStyle" />
                                                    <AlternatingItemStyle CssClass="ItemStyle" />
                                                    <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" DataKeyNames="BSTNR">
                                                        <SortExpressions>
                                                            <telerik:GridSortExpression FieldName="BEDAT" SortOrder="Descending" />
                                                        </SortExpressions>
                                                        <HeaderStyle ForeColor="#595959" />
                                                        <DetailTables>
                                                            <telerik:GridTableView DataKeyNames="BSTNR" Width="100%" runat="server">
                                                                <SortExpressions>
                                                                    <telerik:GridSortExpression FieldName="EBELP" SortOrder="Ascending" />
                                                                </SortExpressions>
                                                                <ParentTableRelation>
                                                                    <telerik:GridRelationFields DetailKeyField="BSTNR" MasterKeyField="BSTNR">
                                                                    </telerik:GridRelationFields>
                                                                </ParentTableRelation>
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Artikel"/>
                                                                    <telerik:GridBoundColumn DataField="BEDAT" SortExpression="BEDAT" HeaderText="Bestelldatum" DataFormatString="{0:dd.MM.yyyy}"/>
                                                                    <telerik:GridBoundColumn DataField="MENGE" SortExpression="MENGE" HeaderText="Menge" DataFormatString="{0:f0}"/>
                                                                    <telerik:GridBoundColumn DataField="PREIS" SortExpression="PREIS" HeaderText="Preis" DataFormatString="{0:c}"/>
                                                                    <telerik:GridBoundColumn DataField="LTEXT" SortExpression="LTEXT" HeaderText="Info-Text"/>
                                                                </Columns>
                                                            </telerik:GridTableView>
                                                        </DetailTables>
                                                        <Columns>
                                                            <telerik:GridBoundColumn DataField="LIEFERSNR" SortExpression="LIEFERSNR" HeaderText="Lieferscheinnummer"/>
                                                            <telerik:GridBoundColumn DataField="EEIND" SortExpression="EEIND" HeaderText="Lieferdatum" DataFormatString="{0:dd.MM.yyyy}"/>
                                                            <telerik:GridBoundColumn DataField="BSTNR" SortExpression="BSTNR" HeaderText="Belegnummer (SAP intern)"/>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvArtikel" runat="server" CssClass="GridView" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" OnRowCommand="gvArtikel_RowCommand"
                                                OnSelectedIndexChanged="gvArtikel_SelectedIndexChanged" OnRowDataBound="gvArtikel_OnRowDataBound">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("ArtikelID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="LTEXT_NR" Visible="false"/>
                                                    <asp:BoundField HeaderStyle-Width="20%" HeaderText="Artikel" DataField="Artikel" />
                                                     <asp:BoundField HeaderStyle-Width="10%" HeaderText="Datum" DataField="Datum" />
                                                    <asp:TemplateField HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# Eval("ArtikelID") %>'
                                                                CommandName="minusMenge" ID="imgbMinus" ImageUrl="~/Images/Minus.jpg" Width="15px"
                                                                Height="15px" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderStyle-Width="8%" HeaderText="Menge" DataField="Menge" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField  HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# Eval("ArtikelID") %>'
                                                                CommandName="plusMenge" ID="imgbPlus" ImageUrl="~/Images/Plus.jpg" Width="15px"
                                                                Height="15px" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderStyle-Width="8%" HeaderText="Preis" DataField="Preis" ItemStyle-HorizontalAlign="Right" />
                                                    <asp:BoundField HeaderStyle-Width="25%" HeaderText="Infotext" DataField="Langtext"
                                                        Visible="true" />
                                                    <asp:TemplateField Visible="true"  HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibEditInfotext" runat="server" Width="27px" Height="30px" ImageUrl="~/Images/edit_01.gif"
                                                                CommandArgument='<%# Eval("ArtikelID") %>' CommandName="bearbeiten" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField  HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="32px" Height="32px" ImageUrl="~/Images/RecycleBin.png"
                                                                TabIndex="-1" CommandArgument='<%# Eval("ArtikelID") %>' CommandName="entfernen" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    </table>
                            </div>                       
                    <div id="dataFooter" class="dataQueryFooter">
                    <asp:LinkButton ID="MPEDummy2" Width="0" Height="0" runat="server" style="border:solid; color:White; border-color:transparent;"/>
                        &nbsp;
                        <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            OnClick="cmdCreate_Click" Visible="false">» Absenden </asp:LinkButton>
                    </div>          
                    <ajaxToolkit:ModalPopupExtender runat="server" ID="MPEInfotext" BackgroundCssClass="divProgress"
                        Enabled="true" PopupControlID="Infotext" TargetControlID="MPEDummy2">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="Infotext" HorizontalAlign="Center" runat="server" Style="display: block">
                        <table cellspacing="0" id="tblInfotext" runat="server" width="50%" bgcolor="white"
                            cellpadding="0" style="width: 50%; border: solid 1px #646464">
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="firstLeft active">
                                    Infotext Artikel:
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="firstLeft active">
                                    <asp:Label ID="lblErrorInfotext" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:Label ID="lblLTextNr" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblMatNr" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblPflicht" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblMenge" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblArtikelbezeichnung" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblDat" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblLieferantenID" runat="server" Visible="false"></asp:Label>
                                     <asp:Label ID="lblLieferscheinnummer" runat="server" Visible="false"></asp:Label>
                                     <asp:Label ID="lblPreisBox" runat="server" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td class="firstLeft active">
                                    <asp:TextBox ID="txtInfotext" runat="server" Width="400px" Height="300px" TextMode="MultiLine"
                                        Wrap="true"></asp:TextBox>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:LinkButton ID="lbInfotextSave" Text="Speichern" Height="16px" Width="78px" runat="server"
                                        CssClass="Tablebutton" OnClick="lbInfotextSave_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lbInfotextClose" Text="Schließen" Height="16px" Width="78px"
                                        runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
