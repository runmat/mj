<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01.aspx.vb" Inherits="KBS.Change01"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript"  type="text/javascript" src="../Java/JScript.js"></script>
   
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung EAN-Ware/Handelsware</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        bitte geben Sie hier den gewünschten Artikel + Menge ein und drücken Sie hinzufügen
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tfoot>
                                <tr>
                                    <td colspan="5">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr class="formquery">
                                    <td colspan="5" class="firstLeft active">
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblEANAnzeige" Text="EAN" runat="server"></asp:Label>
                                    </td>
                                    <td class="active" style="padding-right: 100px">
                                        <asp:Label ID="lblArtikelbezeichnungAnzeige" Text="Artikelbezeichnung" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblMengeAnzeige" Text="Menge" runat="server"></asp:Label>
                                    </td>
                                    <td width="100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="5" class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtEAN" TabIndex="1" onFocus="Javascript:this.select();" onKeyUp="Javascript:setFocusAfterInput(this);"
                                            MaxLength="15" AutoPostBack="true" runat="server"></asp:TextBox>
                                        <asp:TextBox Width="1" ID="txtMaterialnummer" Visible="false" Text="" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblArtikelbezeichnung" Text="(wird automatisch ausgefüllt)" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtMenge" MaxLength="4" onFocus="Javascript:this.select();" Width="50px"
                                            Text="" runat="server"></asp:TextBox>
                                        <asp:RangeValidator Enabled="true" SetFocusOnError="true" ControlToValidate="txtMenge"
                                            ID="rvMenge" MinimumValue="1" MaximumValue="9999" runat="server" Display="None"
                                            ErrorMessage="Menge 1-9999"></asp:RangeValidator><cc1:ValidatorCalloutExtender Enabled="True"
                                                ID="vceMenge" Width="350px" runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                TargetControlID="rvMenge">
                                            </cc1:ValidatorCalloutExtender>
                                        <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                            FilterType="Numbers">
                                        </cc1:FilteredTextBoxExtender>
                                    </td>
                                    <td nowrap="nowrap">
                                    <asp:LinkButton ID="lbtnInsert" Text="hinzufügen" Height="16px" Width="78px"
                                        runat="server" CssClass="Tablebutton"></asp:LinkButton>                                                     
                                        <asp:Button ID="defaultButtonInsert" Height="0" Width="0" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="data" style="display:block">
                        <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="rgGrid1" runat="server" AllowSorting="False" AllowPaging="False"
                                        AutoGenerateColumns="False" GridLines="None" Culture="de-DE" Skin="Default">
                                        <ClientSettings AllowKeyboardNavigation="true" >
                                            <Scrolling ScrollHeight="480px" AllowScroll="True" UseStaticHeaders="True" FrozenColumnsCount="1" />
                                        </ClientSettings>
                                        <GroupingSettings RetainGroupFootersVisibility="true" />
                                        <ItemStyle CssClass="ItemStyle" />
                                        <AlternatingItemStyle CssClass="ItemStyle" />
                                        <MasterTableView Width="100%" GroupLoadMode="Client" TableLayout="Auto" AllowPaging="False" 
                                            ShowGroupFooter="True" ShowFooter="True">
                                            <SortExpressions>
                                                <telerik:GridSortExpression FieldName="NAME1" SortOrder="Ascending" />
                                                <telerik:GridSortExpression FieldName="EAN11" SortOrder="Ascending" />
                                            </SortExpressions>
                                            <GroupByExpressions>
                                                <telerik:GridGroupByExpression>
                                                    <SelectFields>
                                                        <telerik:GridGroupByField FieldName="LIFNR"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField FieldName="NAME1"></telerik:GridGroupByField>
                                                        <telerik:GridGroupByField FieldName="MINBW"></telerik:GridGroupByField>
                                                    </SelectFields>
                                                    <GroupByFields>
                                                        <telerik:GridGroupByField FieldName="LIFNR"></telerik:GridGroupByField>
                                                    </GroupByFields>
                                                </telerik:GridGroupByExpression>
                                            </GroupByExpressions>
                                            <HeaderStyle ForeColor="#595959" />
                                            <FooterStyle BackColor="#FFB27F" HorizontalAlign="Right" Wrap="false" />
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="EAN11" SortExpression="EAN11" HeaderText="EAN" FooterText="Gesamtsumme:" >
                                                    <HeaderStyle Width="90px" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="MAKTX" SortExpression="MAKTX" HeaderText="Artikelbezeichnung" >
                                                    <HeaderStyle Width="150px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="KBETR" SortExpression="KBETR" HeaderText="Preis" DataFormatString="{0:c}" >
                                                    <HeaderStyle Width="60px" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="BPRME" SortExpression="BPRME" HeaderText="je" >
                                                    <HeaderStyle Width="40px" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Menge" UniqueName="Menge" >
                                                    <HeaderStyle Width="90px" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandName="minusMenge" ID="imgbMinus" Width="15" Height="15"
                                                            ImageUrl="~/Images/Minus.jpg" runat="server" ToolTip="Anzahl verringern" >
                                                        </asp:ImageButton>
                                                        &nbsp;
                                                        <asp:TextBox  MaxLength="3" runat="server" AutoPostBack="true" OnTextChanged="txtMenge_TextChanged"
                                                            Width="50px" onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# Eval("BSTMG") %>' style="text-align: right">
                                                        </asp:TextBox>
                                                        &nbsp;
                                                        <asp:ImageButton CommandName="plusMenge" ID="imgbPlus" Width="15" Height="15" 
                                                            ImageUrl="~/Images/Plus.jpg" runat="server" ToolTip="Anzahl erhöhen" >
                                                        </asp:ImageButton>
                                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMenge" Display="None"
                                                            ErrorMessage="Bitte Menge eingeben" SetFocusOnError="true" ID="rfvMenge" >
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RangeValidator SetFocusOnError="true" ControlToValidate="txtMenge" Enabled="True"
                                                            ID="RangeValidator1" MinimumValue="1" MaximumValue="999" runat="server" Display="None"
                                                            ErrorMessage="Menge 1-999">
                                                        </asp:RangeValidator>
                                                        <cc1:ValidatorCalloutExtender Enabled="True"
                                                            ID="ValidatorCalloutExtender1" Width="350px" runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                            TargetControlID="rvMenge">
                                                        </cc1:ValidatorCalloutExtender>
                                                        <cc1:ValidatorCalloutExtender Enabled="True" ID="vceMenge2" Width="350px" runat="server"
                                                            HighlightCssClass="validatorCalloutHighlight" TargetControlID="rfvMenge">
                                                        </cc1:ValidatorCalloutExtender>
                                                        <cc1:FilteredTextBoxExtender Enabled="True" ID="fteMenge2" runat="server" TargetControlID="txtMenge"
                                                            FilterType="Numbers">
                                                        </cc1:FilteredTextBoxExtender>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="MINBW" SortExpression="MINBW" Aggregate="Max" Visible="false" UniqueName="MINBW" >
                                                </telerik:GridBoundColumn>
                                                <telerik:GridCalculatedColumn DataFields="BSTMG,KBETR,KPEIN,UMRECH" Expression="{0}*{1}/{2}/{3}" HeaderText="Gesamt" DataFormatString="{0:c}" DataType="System.Double" Aggregate="Sum" UniqueName="Gesamt" >
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridCalculatedColumn>
                                                <telerik:GridTemplateColumn>
                                                    <HeaderStyle Width="25px" />
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lbDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                            CommandName="entfernen" ToolTip="löschen" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Hinweis">
                                                    <HeaderStyle Width="20px" />
                                                    <ItemTemplate>
                                                        <asp:Image runat="server" ID="imgFehler" ImageUrl="~/Images/redwarn.jpg" Visible="false"/>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>

                        <div id="dataFooter">
                            <asp:LinkButton ID="lbSpeichern" Text="Speichern" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Panel ID="BestellungsCheck" runat="server" style="display:none;overflow:auto;width:460px;height:425px;">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white"
                                cellpadding="0" border="0" style="border: solid 1px #646464">
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        Bitte überprüfen Sie Ihre Bestellung, ungewöhnliche Werte sind Rot markiert
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView Width="50%" HorizontalAlign="Center" BackColor="White" CssClass="GridView"
                                            ID="GridView2" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                            AllowSorting="True" ShowFooter="False" GridLines="None">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:BoundField HeaderText="EAN" DataField="EAN11" />
                                                <asp:BoundField HeaderText="Artikelbezeichnung" DataField="MAKTX" />
                                                <asp:BoundField HeaderText="Menge" DataField="BSTMG" />
                                            </Columns>
                                        </asp:GridView>
                                    </td> 
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbBestellungOk" Text="Absenden" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" CancelControlID="lbBestellungKorrektur">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellResultat" HorizontalAlign="Center" runat="server" style="display:none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                border="0">
                                <tr>
                                    <td class="firstLeft active">
                                        Bestellstatus:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblBestellMeldung" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbBestellFinalize" Text="ok" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender runat="server" ID="MPEBestellResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellResultat" TargetControlID="MPEDummy">
                        </cc1:ModalPopupExtender>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
