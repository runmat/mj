<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change01s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change01s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent"> 
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage starten
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" ImageUrl="../../../Images/queryArrow.gif" />
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="divError" runat="server" enableviewstate="false" style="padding: 10px 0px 10px 15px;
                        margin-top: 10px">
                        <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                        <asp:Label ID="lblNoData" runat="server" ForeColor="Blue" EnableViewState="False"></asp:Label>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            <span>
                                                <asp:RadioButton ID="rdbEinzel" runat="server" Checked="true" GroupName="rdbGroup"
                                                    Text="Einzelauswahl" AutoPostBack="True" TextAlign="Left" />
                                                <asp:RadioButton ID="rdbUpload" runat="server" GroupName="rdbGroup" Text="Upload"
                                                    AutoPostBack="True" TextAlign="Left" />
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table id="tblEinzel" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Fahrgestellnummer:
                                        </td>
                                        <td class="firstLeft active" style="width:100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Kennzeichen:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Leasingvertragsnummer:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtLeasingvertragsnummer" runat="server" CssClass="TextBoxNormal"
                                                MaxLength="17" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Blocknummer:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtBlocknummer" runat="server" CssClass="InputTextbox" Width="150px"
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Bank Treuhand:
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:DropDownList ID="ddlCustomer" style="width:auto" runat="server">
                                            </asp:DropDownList>
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
                            <table id="tblUpload" runat="server" cellpadding="0" cellspacing="0" visible="false"
                                style="padding-bottom: 10px">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Upload:
                                        </td>
                                        <td class="firstLeft active">
                                            <input id="upFile" type="file" size="49" name="File1" runat="server" />
                                            <asp:ImageButton ID="ibtInfo" runat="server" ImageUrl="../../../Images/Info01_10.jpg"
                                                Height="16px" Style="padding-left: 7px" />
                                        </td>
                                        <td>
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
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                              <div class="ExcelDiv">
                                    <div align="right" class="rightPadding">
                                        <img src="../../../Images/iconXLS.gif" alt="Excel herunterladen" />
                                        <span class="ExcelSpan">
                                            <asp:LinkButton ID="lnkCreateExcel" runat="server">Excel herunterladen</asp:LinkButton>
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
                                        <asp:GridView ID="gvAusgabe" runat="server" AutoGenerateColumns="False"
                                            CellPadding="0" GridLines="None" AlternatingRowStyle-BackColor="#DEE1E0" AllowSorting="True"
                                            AllowPaging="True" CssClass="GridView" PageSize="20" Width="100%" >
                                            <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                                            <AlternatingRowStyle CssClass="GridTableAlternate"></AlternatingRowStyle>
                                            <PagerSettings Visible="False" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <EmptyDataRowStyle BackColor="#DFDFDF" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="EQUNR" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqui" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="NAME1_BANK_TH" HeaderText="Bank Treuhand" SortExpression="NAME1_BANK_TH" HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="BLOCK_NR_ALT" HeaderText="Blocknr. ALT" SortExpression="BLOCK_NR_ALT" HeaderStyle-Width="67px" />
                                                <asp:TemplateField HeaderText="Blocknummer NEU" SortExpression="BLOCK_NR_NEU" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBlocknummerNeu" runat="server" Width="80px" MaxLength="24" Text='<%# Bind("BLOCK_NR_NEU") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Blocknr. löschen" SortExpression="BlocknummerAlt" HeaderStyle-Width="100px">
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkLoeschen" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.BLOCK_ALT_LOE")="X") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>       
                                                <asp:BoundField DataField="LIZNR" HeaderText="Leasingvertrag" SortExpression="LIZNR" HeaderStyle-Width="90px" />                                                                                         
                                                <asp:BoundField DataField="CHASSIS_NUM" HeaderText="Fahrgestellnr." SortExpression="CHASSIS_NUM" HeaderStyle-Width="130px" />
                                                <asp:BoundField DataField="LICENSE_NUM" HeaderText="Kennz." SortExpression="LICENSE_NUM" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="TIDNR" HeaderText="ZBII Nummer" SortExpression="TIDNR" HeaderStyle-Width="80px"/>
                                                <asp:BoundField DataField="ZZHERSTELLER_SCH" HeaderText="Hersteller" SortExpression="ZZHERSTELLER_SCH" HeaderStyle-Width="60px" />
                                                <asp:BoundField DataField="ZZTYP_SCHL" HeaderText="Typ" SortExpression="ZZTYP_SCHL" HeaderStyle-Width="30px" />
                                                <asp:BoundField DataField="ZZVVS_SCHLUESSEL" HeaderText="Ausführung" SortExpression="ZZVVS_SCHLUESSEL" HeaderStyle-Width="70px" />

                                                <asp:TemplateField HeaderText="Status" HeaderStyle-ForeColor="White" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" ForeColor="Red" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFound" runat="server" Text='<%# Bind("NO_FOUND") %>'></asp:Label>
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
                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" Visible="false">» Speichern</asp:LinkButton>
                    </div>
                </div>
                <div>
                    <asp:Button ID="btnFake" runat="server" Text="Fake" Style="display: none" />
                    <asp:Button ID="Button1" runat="server" Text="BUTTON" OnClick="Button1_Click" Visible="False" />
                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnFake"
                        PopupControlID="mb" BackgroundCssClass="modalBackground" DropShadow="true" CancelControlID="btnOK">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel ID="mb" runat="server" Width="480px" Height="220px" BackColor="White"
                        Style="display: none">
                        <div style="padding: 20px 20px 20px 20px">
                            <div style="padding-bottom: 5px">
                                Erwarteter Dateityp für den Upload: Excel (.XLS)
                                <br />
                                Erwartetes Dateiformat:
                                <br />
                                <u>Leasingvertragsnummer</u>, <u>Fahrgestellnummer</u>, <u>Neue Blocknummer</u><br />
                                Leasingvertragsnummer und/oder Fahrgestellnummer, <br />
                                Neue Blocknummer = Pflichtfeld<br />
                                <b>Mit Spaltenüberschriften</b><br />
                                <u>Beispiel:</u>
                            </div>
                            <table>
                                <tr>
                                    <td>
                                        <b>Leasingvertragsnummer</b>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <b>Fahrgestellnummer</b>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <b>Neue Blocknummer</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        5465465478
                                    </td>
                                    <td style="padding-left: 5px">
                                        WACCCC1TZAW062777
                                    </td>
                                    <td style="padding-left: 5px">
                                        123485
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        123456855
                                    </td>
                                    <td style="padding-left: 5px">
                                        WACCCC1TZAW062546
                                    </td>
                                    <td style="padding-left: 5px">
                                        325681
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="text-align: center">
                            <asp:Button ID="btnOK" runat="server" CssClass="TablebuttonLarge" Font-Bold="true"
                                Text="Schließen" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
