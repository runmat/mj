<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="AppF2.Change05"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
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
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbHaendler" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtNummer" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName1" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName2" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtPLZ" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtOrt" EventName="TextChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                    <tbody>
                                        <tr class="formquery" id="tr1" runat="server">
                                            <td colspan="2" class="firstLeft active" style="background-color: #DFDFDF;">
                                                <div style="height: 20px; vertical-align: middle;">
                                                    <asp:Label ID="lbl_AnzeigeHaendlerSuche" runat="server">lbl_AnzeigeHaendlerSuche</asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trHaendlernummer" class="formquery" runat="server" >
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_HaendlerNummer" runat="server"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txtNummer" runat="server" MaxLength="10" AutoPostBack="True"></asp:TextBox>
                                                <asp:Label ID="lblSHistoryNR" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_Name1" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name1:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryName1" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Name2" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Name2:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName2" runat="server" Enabled="False" BackColor=" #c1ccd9" MaxLength="35"
                                                    AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label ID="lblSHistoryName2" runat="server"
                                                        Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_PLz" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryPLZ" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="Tr_Ort" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryOrt" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="Tr_SelectionButton" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lbldirektInput" Style="display: none" runat="server" Width="40" ForeColor="green">
				                                        <u>Direkteingabe</u>
                                                </asp:Label><br />
                                                Anzahl Treffer:
                                                <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                                <asp:Label ID="lblwait" Style="display: none" runat="server" ForeColor="red" Font-Bold="True">bitte warten</asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_HaendlerAuswahl" runat="server" visible="false">
                                            <td colspan="2" class="firstLeft active">
                                                <div style="float: left">
                                                    <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="126px" AutoPostBack="True">
                                                    </asp:ListBox>
                                                </div>
                                                <div>
                                                    <b>&nbsp;&nbsp;
                                                        <asp:Label ID="lblHaendlerDetailsNR" runat="server" Font-Size="11pt"></asp:Label></b><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsName1" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsName2" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsStrasse" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    <br />
                                                    <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsPLZ" runat="server" Font-Size="10pt"></asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsOrt" runat="server" Font-Size="10pt"></asp:Label>
                                                    </b>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tr_Message" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lbl_Info" runat="server" Width="100%">einfache / mehrfache Platzhaltersuche möglich z.B. 'PLZ= 9*', 'Name1=*Musterma*' </asp:Label>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2" style="background-color: #DFDFDF;">
                                                <div style="height: 20px; vertical-align: middle;">
                                                    <asp:Label ID="lbl_AnzeigeFahrzeugSuche" runat="server">lbl_AnzeigeFahrzeugSuche</asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="tr_FahrgestellNr" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_Fahrgestellnummer" runat="server">Fahrgestellnummer</asp:Label>:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtFahrgestellNr" runat="server" MaxLength="35"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery" id="tr_LIZNR" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_LIZNR" runat="server">lbl_LIZNR</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txt_LIZNR" runat="server" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr  class="formquery" >
                                            <td colspan="2" class="rightPadding" align="right" style="width: 100%">
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">» Weiter</asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" CssClass="TablebuttonLarge"
                                                        Width="130px" Height="16px">» Neue Suche</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
