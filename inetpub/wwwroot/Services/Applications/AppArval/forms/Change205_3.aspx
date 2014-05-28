<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Change205_3.aspx.vb"
    Inherits="AppArval.Change205_3" MasterPageFile="../../../MasterPage/Services.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript">										
							<!--
        function ShowHide() {
            o = document.getElementById("ctl00_ContentPlaceHolder1_tdInput1");
            p = document.getElementById("ctl00_ContentPlaceHolder1_tdInput");

            o.style.display = "none";
            p.style.display = "none";
            document.forms[0].ctl00$ContentPlaceHolder1$txtGrundBemerkung.value = "";

            if (document.forms[0].ctl00$ContentPlaceHolder1$ddlGrund.value == "001") {
                o.style.display = "";
                p.style.display = "";
                document.forms[0].ctl00$ContentPlaceHolder1$txtGrundBemerkung.focus();
            }
            if (document.forms[0].ctl00$ContentPlaceHolder1$ddlGrund.value == "005") {
                o.style.display = "";
                p.style.display = "";
                document.forms[0].ctl00$ContentPlaceHolder1$txtGrundBemerkung.focus();
            }
        }								
							-->
    </script>

    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:HyperLink ID="lnkFahrzeugsuche" runat="server" NavigateUrl="Change205.aspx">Fahrzeugsuche</asp:HyperLink>&nbsp;|&nbsp;<asp:HyperLink
                        ID="lnkFahrzeugAuswahl" runat="server" NavigateUrl="Change205_2.aspx">Fahrzeugauswahl</asp:HyperLink>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                                <asp:Label ID="lblPageTitle" runat="server"> (Adressauswahl)</asp:Label>
                            </h1>
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
                                    <tr>
                                        <td colspan="5">
                                            <table cellpadding="0"  style="border-style:none" cellspacing="0">
                                                <tr class="formquery">
                                                    <td colspan="5" class="firstLeft active">
                                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrTemp" runat="server">
                                                    <td id="Td1" class="firstLeft active" runat="server">
                                                        <asp:Label ID="lblVersandAdresse" runat="server">Zulassungsstelle:</asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlZulDienst"  runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrEnd1" runat="server">
                                                    <td id="Td2" class="firstLeft active" runat="server">
                                                        Name:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName1" runat="server" TabIndex="1" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        Name2:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtName2" TabIndex="1" runat="server" MaxLength="35"></asp:TextBox>
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrEnd2" runat="server">
                                                    <td id="Td3" class="firstLeft active" runat="server">
                                                        Str.:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtStr" runat="server" TabIndex="2" MaxLength="60"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        Nr.:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtNr"  runat="server" MaxLength="10" TabIndex="3"></asp:TextBox>
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrEnd3" runat="server">
                                                    <td id="Td4" class="firstLeft active" runat="server">
                                                        Plz.:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPlz"  runat="server" TabIndex="4" MaxLength="10"></asp:TextBox>
                                                    </td>
                                                    <td class="firstLeft active">
                                                        Ort:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtOrt"  runat="server" TabIndex="5" MaxLength="40"></asp:TextBox>
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrEnd6" runat="server">
                                                    <td id="Td5" class="firstLeft active" runat="server">
                                                        Land:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlLand"  runat="server" AutoPostBack="True" TabIndex="10">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtLand" TabIndex="4" runat="server" MaxLength="3" Visible="False"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td width="100%">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr class="formquery" id="trVersandAdrEnd4" runat="server">
                                        <td id="Td6" class="firstLeft active" runat="server">
                                            <asp:Label ID="txtGrund" runat="server">Versandgrund:</asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlGrund" CssClass="DropDownXLarge"  runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    
                                    <tr class="formquery" runat="server">
                                        <td id="tdInput1" class="firstLeft active" runat="server">
                                            <asp:Label ID="lblMit" runat="server">auf:</asp:Label>
                                        </td>
                                        <td id="tdInput" runat="server" class="active">
                                            <asp:TextBox ID="txtGrundBemerkung" TabIndex="4" runat="server" MaxLength="60"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery" id="trVersandAdrEnd5" runat="server">
                                        <td colspan="5" class="firstLeft active">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trZeit" runat="server">
                                        <td class="firstLeft active" runat="server">
                                            <asp:Label ID="lblVersandart" runat="server">Versandart:</asp:Label>
                                        </td>
                                        <td   nowrap="nowrap"  runat="server" colspan="3">
                                       <ul>
                                           
                                            <asp:RadioButton CssClass="radio" ID="chkVersandStandard" runat="server" Text="innerhalb von 24 bis 48 h"
                                                Checked="True" GroupName="Versandart"></asp:RadioButton>
                                            <asp:RadioButton ID="chk0900" runat="server" Text="vor 9:00 Uhr*" GroupName="Versandart">
                                            </asp:RadioButton>
                                            <asp:RadioButton ID="chk1000" runat="server" Text="vor 10:00 Uhr*" GroupName="Versandart">
                                            </asp:RadioButton>
                                            <asp:RadioButton ID="chk1200" runat="server" Text="vor 12:00 Uhr*" GroupName="Versandart">
                                            </asp:RadioButton>
                                            <asp:RadioButton ID="chkZweigstellen" runat="server" Visible="False" Text="Zweigstellen:"
                                                Checked="True" GroupName="grpVersand"></asp:RadioButton>
                                            <asp:RadioButton ID="chkZulassungsstellen" runat="server" Visible="False" Text="Zulassungsstellen:"
                                                GroupName="grpVersand"></asp:RadioButton>
                                              </ul>  
                                        </td>
                                        <td class="active" width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td colspan="5" class="firstLeft active">
                                            *Diese Versandarten sind mit zusätzlichen Kosten verbunden.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <asp:Literal ID="litScript" runat="server"></asp:Literal>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdSave" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                            <asp:LinkButton ID="cmdSearch" Text="Suchen" Visible="false" Height="16px" Width="78px"
                                runat="server" CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
