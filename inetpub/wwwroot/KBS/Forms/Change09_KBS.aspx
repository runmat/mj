<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09_KBS.aspx.vb"
    Inherits="KBS.Change09_KBS" MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Inventur</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte scannen Sie den zu erfassenden Artikel, geben Sie die Menge ein und drücken
                                        Sie hinzufügen.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger  ControlID="lbtnInsert" />
                                 <asp:PostBackTrigger  ControlID="lbtnOverWrite" />
                                  <asp:PostBackTrigger  ControlID="lbtnAdd" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnInsert">
                                    <table cellpadding="0" cellspacing="0">
                                        <tfoot>
                                            <tr>
                                                <td colspan="7">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tfoot>
                                        <tbody>
                                            <tr class="formquery">
                                                <td colspan="7" class="firstLeft active">
                                                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="7" style="font-size: 12px">
                                                    <asp:Label ID="lblProdH" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblEANAnzeige" Text="EAN" runat="server"></asp:Label>
                                                </td>
                                                <td class="active" style="padding-right: 75px">
                                                    <asp:Label ID="lblArtikelbezeichnungAnzeige" Text="Artikelbezeichnung" runat="server"></asp:Label>
                                                </td>
                                                <td class="firstLeft active" nowrap="nowrap">
                                                    <asp:Label ID="lblErfMenge" Text="bereits erf. Menge" runat="server"></asp:Label>
                                                </td>
                                                <td colspan="3" class="firstLeft active" width="100%">
                                                    <asp:Label ID="lblMengeAnzeige" Text="Menge" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="7" class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtEAN" TabIndex="1" onFocus="Javascript:this.select();" onKeyUp="Javascript:setFocusAfterInput(this);"
                                                        MaxLength="15" AutoPostBack="true" runat="server"></asp:TextBox>
                                                    <asp:TextBox Width="1" ID="txtMaterialnummer" Visible="false" Text="" runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblArtikelbezeichnung" Text="(wird automatisch ausgefüllt)" runat="server"></asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblSapMenge" runat="server"></asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtMenge" MaxLength="3" onFocus="Javascript:this.select();" Width="50px"
                                                        Text="" runat="server"></asp:TextBox>
                                                    <asp:RangeValidator Enabled="true" SetFocusOnError="true" ControlToValidate="txtMenge"
                                                        ID="rvMenge" MinimumValue="0" MaximumValue="999" runat="server" Display="None"
                                                        ErrorMessage="Menge 0-999"></asp:RangeValidator><cc1:ValidatorCalloutExtender Enabled="True"
                                                            ID="vceMenge" Width="350px" runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                            TargetControlID="rvMenge">
                                                        </cc1:ValidatorCalloutExtender>
                                                    <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td nowrap="nowrap" width="100%">
                                                    <asp:LinkButton ID="lbtnInsert" Text="speichern" Visible="false" Height="16px" Width="100px"
                                                        runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnOverWrite" Text="überschreiben" Visible="false" Height="16px"
                                                        Width="100px" runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnAdd" Text="addieren" Visible="false" Height="16px" Width="100px"
                                                        runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                            <asp:GridView CssClass="GridView" ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                                                AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" PageSize="999999">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMATNR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="25%" HeaderText="Artikel"
                                                        DataField="MAKTX" />
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15%" HeaderText="EAN"
                                                        DataField="EAN" />                                                    
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="10%" HeaderText="Gesamtmenge"
                                                        DataField="Menge_Gesamt" />
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="10%" HeaderText="erf. Menge"
                                                        DataField="Menge_erfasst" />
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="15%" HeaderText="Aktion"
                                                        DataField="Status" />
                                                    <asp:TemplateField HeaderStyle-Width="10%">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ibEditInfotext" runat="server" Width="27px" Height="30px" ImageUrl="~/Images/edit_01.gif"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Index") %>' CommandName="bearbeiten" />
                                                            <span style="padding-right: 5px"></span>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.Index") %>' CommandName="entfernen" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>



                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellungsCheck" runat="server" Style="overflow: auto; height: 210px;
                            width: 600px; display: none">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white" cellpadding="0"
                                
                                style="overflow: auto; height: 200px; width: 500px; border: solid 1px #646464">
                                <tr>
                                    <td colspan="6">
                                        <asp:Label ID="lblEditError" CssClass="TextError" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="ItemStyle">
                                    <td colspan="6" class="firstLeft active">
                                        Bitte geben Sie die Menge des gewählten Artikels ein!
                                    </td>
                                </tr>
                                <tr class="GridTableAlternate">
                                    <td class="firstLeft active"  style="width:155px">
                                        <asp:Label ID="lblArtikelEdt" Text="Artikel" runat="server"></asp:Label>
                                        <asp:Label ID="lblMaterial" Visible="false" runat="server"></asp:Label>
                                        <asp:Label ID="lblMengealt" Visible="false" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblEanlEdt" Text="EAN" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblStatusEdt" Text="Status" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active" colspan="2">
                                        <asp:Label ID="lblMengeEdt" Text="Menge" runat="server"></asp:Label>
                                    </td>
                                </tr>

                                <tr  class="ItemStyle">
                                    <td class="firstLeft active" >
                                        <asp:Label ID="lblArtikelbez"  runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblEANShow" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblStatusShow" runat="server"></asp:Label>
                                    </td>
                                    <td class="firstLeft active">
                                        <asp:TextBox ID="txtEditMenge" MaxLength="3" 
                                            Width="50px" Text="" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="right">
                                        <asp:LinkButton ID="lbClose" Text="Schließen" Height="16px" Width="100px"
                                            runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnEditOverWrite" Text="überschreiben" Height="16px"
                                            Width="100px" runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnEditAdd" Text="addieren" Height="16px" Width="100px"
                                            runat="server" CssClass="TablebuttonMiddle"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr >
                                    <td  colspan="7">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
