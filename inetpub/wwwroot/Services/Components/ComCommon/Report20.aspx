<%@ Page Language="vb" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="Report20.aspx.vb"
    Inherits="CKG.Components.ComCommon.Report20" MasterPageFile="../../MasterPage/Services.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="paginationQuery">
                            <table cellpadding="0" cellspacing="0">
                                <tbody>
                                    <tr>
                                        <td class="firstLeft active">
                                            bitte geben Sie ihre Suchkriterien ein
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
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
                                        <td colspan="3" class="firstLeft active">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="tr_Filialen" class="formquery">
                                        <td class="firstLeft active">
                                            Filiale
                                        </td>
                                        <td>
                                            <asp:Label ID="lblFiliale" runat="server" Visible="False">Label</asp:Label>
                                            <asp:DropDownList ID="ddlFiliale" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr runat="server" id="TRdistrikte" class="formquery">
                                        <td class="firstLeft active">
                                            Distrikte
                                        </td>
                                        <td>
                                            <asp:Label ID="lblDistrikte" runat="server" Visible="False">Label</asp:Label>
                                            <asp:DropDownList ID="ddlDistrikte" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            ab Datum
                                        </td>
                                        <td>
                                            <div class="NeutralCalendar">
                                                <asp:TextBox ID="txtAbDatum" runat="server"></asp:TextBox>
                                            </div>
                                            <cc1:calendarextender id="txtAbDatum_CalendarExtender" runat="server" format="dd.MM.yyyy"
                                                popupposition="BottomLeft" animated="true" enabled="True" targetcontrolid="txtAbDatum">
                                            </cc1:calendarextender>
                                            <cc1:maskededitextender id="meetxtAbdatum" runat="server" targetcontrolid="txtAbDatum"
                                                mask="99/99/9999" masktype="Date" inputdirection="LeftToRight">
                                            </cc1:maskededitextender>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            bis Datum
                                        </td>
                                        <td>
                                            <div class="NeutralCalendar">
                                                <asp:TextBox ID="txtBisDatum" runat="server"></asp:TextBox>
                                            </div>
                                            <cc1:calendarextender id="txtBisDatum_CalendarExtender" runat="server" format="dd.MM.yyyy"
                                                popupposition="BottomLeft" animated="true" enabled="True" targetcontrolid="txtBisDatum">
                                            </cc1:calendarextender>
                                            <cc1:maskededitextender id="meetxtBisDatum" runat="server" targetcontrolid="txtBisDatum"
                                                mask="99/99/9999" masktype="Date" inputdirection="LeftToRight">
                                            </cc1:maskededitextender>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Aufgabe
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="optChange" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdcreate" Text="Erstellen" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton" ></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
