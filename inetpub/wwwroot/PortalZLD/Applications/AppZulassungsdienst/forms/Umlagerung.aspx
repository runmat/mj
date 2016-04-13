<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Umlagerung.aspx.cs" Inherits="AppZulassungsdienst.forms.Umlagerung"
    MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte geben Sie hier die empfangende Kostenstelle und den gewünschten Artikel/EAN
                                        + Menge ein und drücken Sie hinzufügen.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lbtnInsert" />
                            </Triggers>
                            <ContentTemplate>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                empfangende KST:
                                            </td>
                                            <td colspan="6" class="firstLeft active" style="width: 100%">
                                                <asp:TextBox ID="txtKST" runat="server" AutoPostBack="true" Width="100px" OnTextChanged="txtKST_TextChanged"
                                                    CssClass="TextBoxNormal"></asp:TextBox>
                                                &nbsp;
                                                <div style="margin-left: 10px; margin-top: 5px; position: absolute;">
                                                    <asp:Label ID="lblKSTText" runat="server" TabIndex="0" Visible="false"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trPlaceHolderArtikel" runat="server" visible="false">
                                            <td colspan="7" style="border-bottom: solid 1px #dfdfdf" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="trArtikel" runat="server" visible="false">
                                            <td class="firstLeft active">
                                                Artikel:
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlArtikel" runat="server" Style="width: auto" TabIndex="1"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlArtikel_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active" id="tdKennzForm" runat="server" visible="false">
                                                Größe:
                                            </td>
                                            <td class="firstLeft active" id="tdKennzFormShow" runat="server" visible="false">
                                                <asp:DropDownList ID="ddlKennzform" runat="server" Style="width: auto" TabIndex="2">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active">
                                                Menge: &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMenge" CssClass="TextBoxNormal" runat="server" TabIndex="2" Width="50px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="width: 100%">
                                                <asp:LinkButton ID="lbtnInsert" runat="server" CssClass="Tablebutton" Height="16px"
                                                    Style="margin-left: 0px" TabIndex="3" Width="78px" OnClick="lbtnInsert_Click">hinzuf&uuml;gen</asp:LinkButton>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="7" style="width: 100%; border-bottom: solid 1px #dfdfdf">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="7" class="firstLeft active">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="7" class="firstLeft active">
                                                <asp:Label ID="lblMessage" CssClass="TextError" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="7" class="firstLeft active">
                                                &nbsp;
                                                <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender Enabled="true" ID="FTBExtender1" runat="server" TargetControlID="txtKST"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                                <cc1:FilteredTextBoxExtender Enabled="true" ID="FilteredTextBoxExtender1" runat="server"
                                                    TargetControlID="txtMenge" FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
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
                                        AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" OnRowCommand="GridView1_RowCommand">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle" />
                                        <Columns>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("MATNR") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" HeaderText="an Kostenstelle" DataField="UMLGO"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Artikel" DataField="MAKTX" />
                                            <asp:BoundField HeaderStyle-Width="10%" HeaderText="Kennz.-Größe" DataField="KENNZFORM" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Eval("MATNR") %>'
                                                        CommandName="minusMenge" ID="imgbMinus" ImageUrl="~/Images/Minus.jpg" Width="15px"
                                                        Height="15px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="10%" HeaderText="Menge" DataField="Menge" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Eval("MATNR") %>'
                                                        CommandName="plusMenge" ID="imgbPlus" ImageUrl="~/Images/Plus.jpg" Width="15px"
                                                        Height="15px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" HeaderText="Infotext" DataField="LText" Visible="true" />
                                            <asp:BoundField DataField="LTEXT_NR" Visible="false" />
                                            <asp:TemplateField Visible="true">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibEditInfotext" runat="server" Width="27px" Height="30px" ImageUrl="~/Images/edit_01.gif"
                                                        CommandArgument='<%# Eval("MATNR") %>' CommandName="bearbeiten" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="lbDelete" runat="server" Width="32px" Height="32px" ImageUrl="~/Images/RecycleBin.png"
                                                        TabIndex="-1" CommandArgument='<%# Eval("MATNR") %>' CommandName="entfernen" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" OnClick="lbAbsenden_Click"></asp:LinkButton>
                        </div>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" Style="display: none" />
                        <asp:Button ID="MPEDummy1" Width="0" Height="0" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellungsCheck" runat="server" Style="overflow: auto; height: 425px;
                            width: 600px; display: none;">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white" cellpadding="0"
                                style="overflow: auto; height: auto; width: auto; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        Bitte überprüfen Sie Ihre Umlagerung. Bitte korrigieren Sie gegebenenfalls!<br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 10px 5px 10px 5px">
                                        <asp:GridView ID="GridView2" runat="server" AllowPaging="False" AllowSorting="True"
                                            AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                            HorizontalAlign="Center" ShowFooter="False" Width="100%">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMatnr" runat="server" Text='<%# Eval("MATNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="UMLGO" HeaderStyle-Width="15%" HeaderText="an Kostenstelle" />
                                                <asp:BoundField DataField="MAKTX" HeaderStyle-Width="35%" HeaderText="Artikel" />
                                                <asp:BoundField HeaderStyle-Width="10%" HeaderText="Kennz.-Größe" DataField="KENNZFORM" />
                                                <asp:BoundField DataField="Menge" HeaderStyle-Width="25%" HeaderText="Menge" />
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbBestellungOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton" OnClick="lbBestellungOk_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender runat="server" ID="MPEBestellResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellResultat" TargetControlID="MPEDummy1">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellResultat" HorizontalAlign="Center" runat="server" Style="display: none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width: 50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        Umlagerungsstatus:
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
                                        <asp:LinkButton ID="lbCreatePDF" Text="Drucken" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton" OnClick="lbCreatePDF_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="MPEInfotext" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="Infotext" TargetControlID="MPEDummy2">
                        </cc1:ModalPopupExtender>
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
                                        <asp:Label ID="lblArtikelbezeichnungInfo" runat="server" Visible="false"></asp:Label>
                                        <asp:Label ID="lblEAN" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblKennzForm" runat="server" Visible="false"></asp:Label>
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
    </div>
</asp:Content>
