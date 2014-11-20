<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s.aspx.vb" Inherits="CKG.Components.ComCommon.Beauftragung2.Report01s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function FilterItems(value, ddlClientID) {
            var ddl = ddlClientID;
            var found = 0;

            for (i = 0; i < ddl.options.length; i++) {
                if (ddl.options[i].value.substr(0, value.length) == value.toUpperCase()) {
                    ddl.selectedIndex = i;
                    found = 1;
                    break;
                }
            }
            if (found == 0) {
                ddl.selectedIndex = 0;
            }
        }
        function SetItemText(ddlClientID, Textbox) {
            var wert = ddlClientID.options[ddlClientID.selectedIndex].value;
            if (wert != "0") {
                Textbox.value = ddlClientID.options[ddlClientID.selectedIndex].value;
            } else {
                Textbox.value = "";
            }
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                <asp:Panel ID="pnlDummy" runat="server" DefaultButton="btnDummy">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div style="display: none">
                        <asp:Button ID="btnDummy" runat="server" />
                    </div>
                    <div id="TableQuery">

                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red"></asp:Label>
                                    <div>
                                        * = Pflichteingabe / ** = Zeitraum von - bis nicht größer als 60 Tage.
                                    </div>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" style="width: 130px">
                                    <asp:Label ID="lblKunde" runat="server" Text="Kunde*"></asp:Label>
                                </td>
                                <td class="active" align="left" nowrap="nowrap" style="width: 103%">
                                    <asp:TextBox ID="txtKunnr" runat="server" Width="75px" CssClass="InputSolid" TabIndex="1"></asp:TextBox>
                                    <asp:DropDownList runat="server" ID="ddlKunde" CssClass="InputSolid" TabIndex="2" EnableViewState="False"/>
                                    <asp:Image ID="imgKunde" runat="server" ImageUrl="../../../Images/ok_10.jpg" Height="18px"
                                        Width="18px" Visible="False" />
                                    <asp:Label ID="lblKundeInfo" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                    <div style="height: 20px; vertical-align: middle;">
                                        <asp:Label ID="lblSpezifisch" runat="server">Spezieller Datensatz</asp:Label></div>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblReferenz" runat="server" Text="Referenz"></asp:Label>
                                </td>
                                <td class="active" style="width: 100%" nowrap="nowrap">
                                    <asp:TextBox ID="txtReferenz" runat="server" TabIndex="3"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen"></asp:Label>
                                </td>
                                <td class="active" style="width: 100%" nowrap="nowrap">
                                    <asp:TextBox ID="txtKennzeichen" runat="server" TabIndex="4"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                    <div style="height: 20px; vertical-align: middle;">
                                        <asp:Label ID="lblUnspezifisch" runat="server">Unspezifische Suche</asp:Label></div>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblZulDatumVon" runat="server" Text="Datum Zulassung von**"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZulDatumVon" runat="server" Width="80px" TabIndex="5"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtZulDatumVon_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtZulDatumVon">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="meetxtZuldatumVon" runat="server" TargetControlID="txtZulDatumVon"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:Label ID="lblZulDatumVonInfo" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblZulDatumBis" runat="server" Text="Datum Zulassung bis**"></asp:Label>
                                </td>
                                <td class="active">
                                    <asp:TextBox ID="txtZulDatumBis" runat="server" Width="80px" TabIndex="6"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtZulDatumBis_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtZulDatumBis">
                                    </cc1:CalendarExtender>
                                    <cc1:MaskedEditExtender ID="meetxtZulDatumBis" runat="server" TargetControlID="txtZulDatumBis"
                                        Mask="99/99/9999" MaskType="Date" InputDirection="LeftToRight">
                                    </cc1:MaskedEditExtender>
                                    <asp:Label ID="lblZulDatumBisInfo" runat="server" CssClass="TextError"></asp:Label>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lblFilter" runat="server" Text="Filter"></asp:Label>
                                </td>
                                <td class="active" style="width: 100%" nowrap="nowrap">
                                    <span>
                                        <asp:RadioButton ID="rbG" runat="server" Checked="True" GroupName="test" Text="Alle Datensätze"
                                            TabIndex="7" />
                                        <asp:RadioButton ID="rbA" runat="server" GroupName="test" Text="durchgeführte Zulassungen"
                                            TabIndex="8" />
                                        <asp:RadioButton ID="rbLeer" runat="server" GroupName="test" Text="offene Zulassungen"
                                            TabIndex="9" />
                                    </span>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                    </div>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            TabIndex="10">» Suchen</asp:LinkButton>
                    </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
