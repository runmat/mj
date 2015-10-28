<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerbBuchErfassung.aspx.vb"
    Inherits="KBS.VerbBuchErfassung" MasterPageFile="~/KBS.Master" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Verbandbuch"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="(Erfassung)" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                           Bitte füllen sie alle Felder aus.
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <asp:UpdatePanel runat="server" ID="upSelection">
                            <ContentTemplate>
                                <div id="TableQuery">
                                    <table cellpadding="0" cellspacing="0">
                                        <tfoot>
                                            <tr>
                                                <td colspan="3">
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
                                                <td class="firstLeft active" style="width: 180px; height: 38px;">
                                                    <asp:Label ID="lblNameVerl" Text="Name des Verletzten:" runat="server"></asp:Label>
                                                </td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtNameVerl" AutoPostBack="true" runat="server" Text="" 
                                                        Width="262px" MaxLength="80"></asp:TextBox>
                                                </td>
                                                <td style="height: 38px; width: 40px;" class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                                <td style="height: 38px; width: 180px;" class="firstLeft active">
                                                    <asp:Label ID="lblDatumUnf" runat="server" Text="Datum des Unfalls"></asp:Label>
                                                </td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtDatUnfall" runat="server" AutoPostBack="true" Text="" Width="80px" />
                                                    <ajaxToolkit:CalendarExtender ID="CeTxtZeitUnfall" runat="server" TargetControlID="txtDatUnfall" Animated="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MeTxtZeitUnfall" runat="server" TargetControlID="txtDatUnfall"
                                                        MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 38px; width: 180px;" class="firstLeft active">
                                                    &nbsp;</td> 
                                                <td style="height: 38px">
                                                    &nbsp;</td>
                                                <td class="firstLeft active" style="height: 38px; width: 40px;">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="height: 38px; width: 180px;">
                                                    Uhrzeit des Unfalls</td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtZeitUnfall" runat="server" AutoPostBack="true"
                                                        Width="50px" MaxLength="4"  />
                                                        &nbsp;&nbsp; Format: HHmm
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtZeitUnfall" runat="server" 
                                                        ErrorMessage="Nur Zahlen eingeben" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td style="height: 38px" colspan="5">

                                                </td> 
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px">
                                                    <asp:Label ID="lvlVerletzung" Text="Verletzung" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtVerletzung" TextMode="multiline" Columns="50" Rows="5" runat="server"
                                                        Width="262px" MaxLength="400" />
                                                </td>
                                                <td style="width: 40px">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="width: 180px">
                                                    <asp:Label ID="lblHergang" Text="Hergang des Unfalls" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtHergang" TextMode="multiline" Columns="50" Rows="5" runat="server"
                                                        Width="262px" MaxLength="400" />
                                                </td>
                                                <td style="width: 22%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px">
                                                    <asp:Label ID="lblNameZeuge" Text="Name der Zeugen" runat="server"></asp:Label>
                                                </td>
                                                <td height="38px">
                                                    <asp:TextBox ID="txtNameZeug" runat="server" AutoPostBack="true" Text="" 
                                                        Width="262px" MaxLength="80"></asp:TextBox>
                                                </td>
                                                <td style="width: 40px">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="width: 180px">
                                                    <asp:Label ID="lblOrt" Text="Ort des Unfalls" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrt" TextMode="SingleLine" runat="server"
                                                        Width="262px" MaxLength="160" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px; height: 38px;">
                                                    <asp:Label ID="lblNameVerl1" runat="server" Text="Name des Ersthelfers"></asp:Label>
                                                </td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtNameErstHelf" runat="server" AutoPostBack="true" Text="" 
                                                        Width="262px" MaxLength="80"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="height: 38px; width: 40px;">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="width: 180px; height: 38px;">
                                                    <asp:Label ID="lblDatHilfe" runat="server" Text="Datum der ersten Hilfe"></asp:Label>
                                                </td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtDatHilfe" runat="server" AutoPostBack="true" Text="" 
                                                        Width="80px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDatHilfe"
                                                        Animated="True">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" TargetControlID="txtDatHilfe"
                                                        MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                               <td style="height: 38px; width: 180px;" class="firstLeft active">
                                                    &nbsp;</td> 
                                                <td style="height: 38px">
                                                    &nbsp;</td>
                                                <td class="firstLeft active" style="height: 38px; width: 40px;">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active" style="height: 38px; width: 180px;">
                                                    Uhrzeit der ersten Hilfe</td>
                                                <td style="height: 38px">
                                                    <asp:TextBox ID="txtUhrHilfe" runat="server" AutoPostBack="true" Text="" 
                                                        Width="50px" MaxLength="4"/>
                                                    &nbsp;&nbsp; Format: HHmm
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtUhrHilfe" runat="server" 
                                                        ErrorMessage="Nur Zahlen eingeben" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active" style="width: 180px">
                                                    <asp:Label ID="Label1" Text="Erste Hilfe Maßnahmen" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtErsteHilfe" TextMode="multiline" Columns="50" Rows="5" runat="server"
                                                        Width="262px" MaxLength="400" />
                                                </td>
                                                <td colspan="3">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbWeiter" Text="Weiter" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
