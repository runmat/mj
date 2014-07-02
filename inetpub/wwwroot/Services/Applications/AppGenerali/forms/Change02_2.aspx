<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change02_2.aspx.vb" Inherits="AppGenerali.Change02_2"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton runat="server" ID="lb_zurueck" Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="pagination">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td>
                                     <asp:Label ID="lblError" runat="server" CssClass="TextError" EnableViewState="False"></asp:Label>
                                        &nbsp;
                                                                   <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                       
                                        &nbsp;
                                        <asp:Label ID="lblInfo" runat="server" Visible="true"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="data">
                        <table  cellpadding="0" cellspacing="0"  style="border-right-width: 1px; border-left-width: 1px; border-left-style: solid; border-right-style: solid; border-right-color: #DFDFDF; border-left-color: #DFDFDF" >
                            <tfoot>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr>
                                    <td>
                                        <table id="Tablex1" runat="server" cellspacing="0" cellpadding="5" width="100%" bgcolor="white"
                                            border="0">
                                            <tr id="tr_Partneradresse">
                                                <td align="left" nowrap="nowrap">
                                                    <asp:RadioButton Text="rb_Partneradresse" ID="rb_Partneradresse" runat="server" AutoPostBack="true"
                                                        GroupName="AdressArt" />
                                                </td>
                                                <td align="left" width="100%">
                                                    <asp:DropDownList ID="ddlPartneradressen" runat="server" CssClass="DropDownXLarge">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="tr_Geschaeftsstelle">
                                                <td align="left" nowrap="nowrap">
                                                    <asp:RadioButton Text="rb_Geschaeftsstelle" ID="rb_Geschaeftsstelle" runat="server"
                                                        AutoPostBack="true" GroupName="AdressArt" Checked="True" />
                                                </td>
                                                <td align="left" width="100%">
                                                    <asp:DropDownList ID="ddlGeschaeftsstelle" runat="server" AutoPostBack="True" CssClass="DropDownXLarge">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr id="tr_Versandanschrift">
                                                <td align="left" nowrap="nowrap">
                                                    <asp:RadioButton Text="rb_Versandanschrift" ID="rb_Versandanschrift" runat="server"
                                                        AutoPostBack="true" GroupName="AdressArt" />
                                                </td>
                                                <td align="left" width="100%">
                                                </td>
                                            </tr>
                                            <tr id="trVersandanschriftValue"  >
                                                <td align="left">
                                                </td>
                                                <td align="left" width="100%">
                                                    <table id="tblVersandanschrift" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                                        bgcolor="white" border="0">
                                                        <tbody>
                                                            <tr >
                                                                <td  class="firstLeft active" >
                                                                    <asp:Label ID="lbl_FirmaName" Text="lbl_FirmaName" runat="server"></asp:Label>
                                                                </td>
                                                                <td >
                                                                    <asp:TextBox ID="txtName1" CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="active" >
                                                                    <asp:Label ID="lbl_AnsprechpartnerZusatz" Text="lbl_AnsprechpartnerZusatz" runat="server"></asp:Label>
                                                                </td>
                                                                <td >
                                                                    <asp:TextBox ID="txtName2"  CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr >
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lbl_Strasse" Text="lbl_Strasse" runat="server"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtStrasse"  CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:Label ID="lbl_Hausnummer" Text="lbl_Hausnummer" runat="server"></asp:Label>
                                                                </td>
                                                                <td >
                                                                    <asp:TextBox ID="txtHausnummer"  runat="server"  CssClass="TextBoxShort"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lbl_Postleitzahl" Text="lbl_Postleitzahl" runat="server" CssClass="TextLarge"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtPostleitzahl"   runat="server" CssClass="TextBoxShort"></asp:TextBox>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:Label ID="lbl_Ort" Text="lbl_Ort" runat="server" CssClass="TextLarge"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:TextBox ID="txtOrt"  runat="server" CssClass="TextBoxNormal"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr >
                                                                <td class="firstLeft active">
                                                                    <asp:Label ID="lbl_Land" Text="lbl_Land" runat="server" CssClass="TextLarge"></asp:Label>
                                                                </td>
                                                                <td class="active">
                                                                    <asp:DropDownList ID="ddlLand" runat="server" CssClass="DropDownNormal">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="active">
                                                                    &nbsp;
                                                                </td>
                                                                <td class="active">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="tr_Bemerkung" >
                                                <td align="left">
                                                    <asp:Label ID="lbl_Bemerkung" Text="lbl_Bemerkung" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" width="100%">
                                                    <asp:TextBox ID="txtBemerkung" runat="server"  CssClass="TextBoxXLarge"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trVersandgrund">
                                                <td align="left">
                                                    <asp:Label ID="lbl_Versandgrund" Text="lbl_Versandgrund" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" width="100%">
                                                    <table id="tblVersandgrund" cellspacing="0" cellpadding="0" width="100%" bgcolor="white"
                                                        border="0">
                                                        <tr>
                                                            <td align="left" width="50%">
                                                                <asp:DropDownList ID="ddlVersandgrund" runat="server" AutoPostBack="True" CssClass="DropDownXLarge">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblVersandGrundZusatzBemerkung" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="left" width="50%">
                                                                <asp:TextBox ID="txtVersandGrundZusatzEingabe" runat="server" CssClass="TextBoxNormal"
                                                                    Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trVersandart">
                                                <td align="left">
                                                    <asp:Label ID="lbl_Versandart" Text="lbl_Versandart" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" width="100%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                                <td align="left" width="100%">
                                                    <table id="tblVersandart" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                                        bgcolor="white" border="0">
                                                        <tr>
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lbl_Normal" Text="lbl_Normal" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton ID="rb_1391" Text="rb_1391" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" Checked="True" />
                                                            </td>
                                                            <td class="firstLeft active">
                                                                <asp:Label ID="lbl_Express" Text="lbl_Express" runat="server"></asp:Label>
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton Text="rb_1385" ID="rb_1385" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active">
                                                                &nbsp;
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton Text="rb_5530" ID="rb_5530" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" />
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                &nbsp;
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton Text="rb_1389" ID="rb_1389" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="firstLeft active">
                                                                &nbsp;
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton Text="rb_Selbstabholung" ID="rb_Selbstabholung" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" />
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                &nbsp;
                                                            </td>
                                                            <td class="cellBorderGray active">
                                                                <asp:RadioButton ID="rb_1390" Text="rb_1390" runat="server" AutoPostBack="false"
                                                                    GroupName="Versandart" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trZusatzanschreiben">
                                                <td align="left">
                                                    <asp:Label Text="lbl_Zusatzanschreiben" ID="lbl_Zusatzanschreiben" runat="server"></asp:Label>
                                                </td>
                                                <td align="left" width="100%">
                                                </td>
                                            </tr>
                                            <tr id="trZusatzanschreibenValue">
                                                <td align="left">
                                                </td>
                                                <td align="left" width="100%">
                                                    <table id="tblZusatzanschreiben" runat="server" cellspacing="0" cellpadding="5" width="100%"
                                                        bgcolor="white" border="0">
                                                        <tr id="trParameterTemp1" visible="false">
                                                            <td align="left" colspan="2">
                                                                <asp:CheckBox ID="chbEingentumsvorbehaltEintragen" Checked="true" Text="Eigentumsvorbehalt durch KLS eintragen"
                                                                    runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trParameterTemp2" visible="false">
                                                            <td align="left" colspan="2">
                                                                <asp:CheckBox ID="chbBenutzerueberlassung" Text="Benutzungsüberlassung mitschicken"
                                                                    runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trParameterEndg1" visible="false">
                                                            <td align="left" colspan="2">
                                                                <asp:CheckBox ID="chbEingentumsvorbehaltLoeschen" Text="Eingentumsvorbehalt durch KLS löschen"
                                                                    runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trParameterEndg2" visible="false">
                                                            <td align="left" colspan="2">
                                                                <asp:CheckBox ID="chbDevinkulierungsschreiben" Text="Devinkulierungsschreiben mitschicken"
                                                                    runat="server" Checked="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="lb_Weiter" Text="weiter" Height="16px" Width="78px" runat="server"
                            CssClass="Tablebutton"></asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
