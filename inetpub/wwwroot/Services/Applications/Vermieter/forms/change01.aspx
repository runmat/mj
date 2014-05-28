<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="change01.aspx.cs" Inherits="Vermieter.forms.change01"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Neue Abfrage starten
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            <asp:ImageButton ID="NewSearch" runat="server" 
                                                ImageUrl="/Services/Images/queryArrow.gif" onclick="NewSearch_Click" />
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
                                                    Text="Einzelauswahl" AutoPostBack="True" TextAlign="Left" 
                                                oncheckedchanged="rdbEinzel_CheckedChanged" />
                                                <asp:RadioButton ID="rdbUpload" runat="server" GroupName="rdbGroup" Text="Upload"
                                                    AutoPostBack="True" TextAlign="Left" 
                                                oncheckedchanged="rdbEinzel_CheckedChanged" />
                                            </span>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table id="tblEinzel" runat="server" cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr class="formquery" id="tr_Fahrgestellnummer" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrgestellnummer" runat="server">lbl_Fahrgestellnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px;width:100%">
                                            <asp:TextBox ID="txtFahrgestellnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Kennzeichen" runat="server">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Kennzeichen" runat="server">lbl_Kennzeichen</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Vertragsnummer">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Vertragsnummer" runat="server">lbl_Vertragsnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:TextBox ID="txtVertragsnummer" runat="server" CssClass="TextBoxNormal" MaxLength="17"
                                                Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_Fahrzeugnummer">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lbl_Fahrzeugnummer" runat="server">lbl_Fahrzeugnummer</asp:Label>
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:TextBox ID="txtFahrzeugnummer" runat="server" CssClass="TextBoxNormal" Width="150px"
                                                MaxLength="20"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="tr_BankTreuhand" runat="server">
                                        <td class="firstLeft active">
                                            Bank Treuhand:
                                        </td>
                                        <td class="firstLeft active" style="padding-left:4px">
                                            <asp:DropDownList ID="ddlCustomer" Style="width: auto" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr ID="tr_BriefeOhneFzgNr" runat="server" class="formquery">
                                        <td class="firstLeft active">
                                            Briefe ohne Fahrzeugnummer:</td>
                                        <td>
                                            <span>
                                            <asp:CheckBox ID="chkBriefeOhneFzgNr" runat="server" 
                                                oncheckedchanged="chkBriefeOhneFzgNr_CheckedChanged" AutoPostBack="true" />
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="2">
                                            <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="/Services/images/empty.gif"
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
                                            <asp:ImageButton ID="ibtInfo" runat="server" ImageUrl="/Services/Images/Info01_10.jpg"
                                                Height="16px" Style="padding-left: 7px" onclick="ibtInfo_Click" />
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
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" 
                            Width="78px" onclick="lbCreate_Click">» Suchen </asp:LinkButton>
                    </div>
                    <div id="Result" runat="Server" visible="false">
                        <div class="ExcelDiv">
                            <div align="right" class="rightPadding">
                                <img src="/Services/Images/iconXLS.gif" alt="Excel herunterladen" />
                                <span class="ExcelSpan">
                                    <asp:LinkButton ID="lnkCreateExcel" runat="server" 
                                    onclick="lnkCreateExcel_Click">Excel herunterladen</asp:LinkButton>
                                </span>
                            </div>
                        </div>
                        <div id="pagination">
                            <uc2:GridNavigation ID="GridNavigation1" runat="server">
                            </uc2:GridNavigation>
                           
                            
                        </div>
                        <div id="data">
                            <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:GridView  AutoGenerateColumns="False" BackColor="White" runat="server" ID="gvAusgabe"
                                            CssClass="GridView" GridLines="None"  PageSize="20" AllowPaging="True" 
                                            AllowSorting="True" onsorting="gvAusgabe_Sorting" Width="1400px" 
                                            EnableModelValidation="True" onrowdeleting="gvAusgabe_RowDeleting">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" 
                                                             DeleteImageUrl="/Services/images/Papierkorb_01.gif" >
                                                         <ControlStyle Width="12px" Height="14px" />
                                                       <HeaderStyle Width="25px" />
                                                         </asp:CommandField>
                                                <asp:TemplateField HeaderText="Status" HeaderStyle-ForeColor="White"  >
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" Text='<%# Bind("Status") %>' ForeColor="Red" runat="server"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle ForeColor="White"></HeaderStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="EQUNR" Visible="False">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("EQUNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEqui" runat="server" Text='<%# Bind("EQUNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="col_Fahrgestellnummer" SortExpression="CHASSIS_NUM">
                                                     <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Fahrgestellnummer" runat="server" CommandName="Sort" CommandArgument="CHASSIS_NUM">col_Fahrgestellnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("CHASSIS_NUM") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("CHASSIS_NUM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                                <asp:TemplateField HeaderText="col_FahrzeugnummerAlt" SortExpression="FZG_NR">
                                                
                                                <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FahrzeugnummerAlt" runat="server" CommandName="Sort" CommandArgument="FZG_NR">col_FahrzeugnummerAlt</asp:LinkButton>
                                                    </HeaderTemplate>
                                                
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("FZG_NR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("FZG_NR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_FahrzeugnummerNeu" SortExpression="BLOCK_NR_NEU">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FahrzeugnummerNeu" runat="server" CommandName="Sort" CommandArgument="BLOCK_NR_NEU">col_FahrzeugnummerNeu</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBlocknummerNeu" runat="server" Width="80px" MaxLength="24" Text='<%# Bind("BLOCK_NR_NEU") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_FzgNummerLoeschen" SortExpression="BLOCK_ALT_LOE">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_FzgNummerLoeschen" runat="server" CommandName="Sort" CommandArgument="BLOCK_ALT_LOE">col_FzgNummerLoeschen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkLoeschen" runat="server" Checked='<%# (DataBinder.Eval(Container, "DataItem.BLOCK_ALT_LOE")=="X") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Vertragsnummer" SortExpression="LIZNR">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Vertragsnummer" runat="server" CommandName="Sort" CommandArgument="LIZNR">col_Vertragsnummer</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("LIZNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("LIZNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="90px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Kennzeichen" SortExpression="LICENSE_NUM">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="LICENSE_NUM">col_Kennzeichen</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("LICENSE_NUM") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("LICENSE_NUM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_ZBII" SortExpression="TIDNR">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_ZBII" runat="server" CommandName="Sort" CommandArgument="TIDNR">col_ZBII</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("TIDNR") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("TIDNR") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Hersteller" SortExpression="ZZHERSTELLER_SCH">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Hersteller" runat="server" CommandName="Sort" CommandArgument="ZZHERSTELLER_SCH">col_Hersteller</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("ZZHERSTELLER_SCH") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("ZZHERSTELLER_SCH") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Typ" SortExpression="ZZTYP_SCHL">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Typ" runat="server" CommandName="Sort" CommandArgument="ZZTYP_SCHL">col_Typ</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("ZZTYP_SCHL") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("ZZTYP_SCHL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_Ausfuehrung" SortExpression="ZZVVS_SCHLUESSEL">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_Ausfuehrung" runat="server" CommandName="Sort" CommandArgument="ZZVVS_SCHLUESSEL">col_Ausfuehrung</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("ZZVVS_SCHLUESSEL") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("ZZVVS_SCHLUESSEL") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="col_BankTreuhand" SortExpression="NAME1_BANK_TH">
                                                    <HeaderTemplate>
                                                        <asp:LinkButton ID="col_BankTreuhand" runat="server" CommandName="Sort" CommandArgument="NAME1_BANK_TH">col_BankTreuhand</asp:LinkButton>
                                                    </HeaderTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("NAME1_BANK_TH") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("NAME1_BANK_TH") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="120px" />
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
                        <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" Width="78px" 
                            Visible="false" onclick="cmdSave_Click">» Speichern</asp:LinkButton>
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
                                <u>Fahrgestellnummer</u>, <u>Fahrzeugnummer Alt</u>, <u>Fahrzeugnummer Neu</u>
                                <br />
                                Neue Fahrzeugnummer = Pflichtfeld<br />
                                <b>Mit Spaltenüberschriften</b><br />
                                <u>Beispiel:</u>
                            </div>
                            <table>
                                <tr>
                                    
                                    <td style="padding-left: 5px">
                                        <b>Fahrgestellnummer</b>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <b>Fahrzeugnummer Alt</b>
                                    </td>
                                    <td>
                                        <b>Fahrzeugnummer Neu</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        WACCCC1TZAW062777
                                    </td>
                                    <td style="padding-left: 5px">
                                        5465465478
                                    </td>
                                    <td style="padding-left: 5px">
                                        5465465490
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        WACCCC1TZAW062546
                                    </td>
                                    <td style="padding-left: 5px">
                                        123456855
                                    </td>
                                    <td style="padding-left: 5px">
                                        123456860
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
