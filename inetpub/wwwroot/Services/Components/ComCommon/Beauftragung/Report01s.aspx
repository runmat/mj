<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report01s.aspx.vb" Inherits="CKG.Components.ComCommon.Report01s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
                    <asp:UpdatePanel ID="Up1" runat="server">
                        <ContentTemplate>
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
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lblKunde" runat="server" Text="Kunde*"></asp:Label>
                                        </td>
                                        <td class="active">
                                            <asp:TextBox ID="txtKunde" runat="server" Width="450px" AutoPostBack="true" TabIndex="1"></asp:TextBox><cc1:AutoCompleteExtender
                                                ID="txtKunde_AutoCompleteExtender" runat="server" DelimiterCharacters="" Enabled="True"
                                                ServicePath="../CommonService.asmx" TargetControlID="txtKunde" UseContextKey="True"
                                                ServiceMethod="GetCustomerList" MinimumPrefixLength="1">
                                            </cc1:AutoCompleteExtender>
                                            <asp:Label ID="lblKundeInfo" runat="server" CssClass="TextError"></asp:Label>
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
                                            <asp:TextBox ID="txtReferenz" runat="server" TabIndex="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" nowrap="nowrap">
                                            <asp:Label ID="lblKennzeichen" runat="server" Text="Kennzeichen"></asp:Label>
                                        </td>
                                        <td class="active" style="width: 100%" nowrap="nowrap">
                                            <asp:TextBox ID="txtKennzeichen" runat="server" TabIndex="3"></asp:TextBox>
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
                                            <asp:TextBox ID="txtZulDatumVon" runat="server" Width="80px" TabIndex="6"></asp:TextBox>
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
                                            <asp:TextBox ID="txtZulDatumBis" runat="server" Width="80px" TabIndex="7"></asp:TextBox>
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
                                                    TabIndex="8" />
                                                <asp:RadioButton ID="rbA" runat="server" GroupName="test" Text="durchgeführte Zulassungen"
                                                    TabIndex="9" />
                                                <asp:RadioButton ID="rbLeer" runat="server" GroupName="test" Text="offene Zulassungen"
                                                    TabIndex="10" />
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
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="txtKunde" EventName="TextChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div id="dataQueryFooter">
                        <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" Width="78px"
                            TabIndex="11">» Suchen</asp:LinkButton>
                    </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
