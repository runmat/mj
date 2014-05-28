<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change11.aspx.vb" Inherits="AppF2.Change11"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

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
                            <asp:PostBackTrigger ControlID="lbNext" />
                            <asp:PostBackTrigger ControlID="lbReset" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                    <tbody>
                                        <tr class="formquery" id="tr1" runat="server">
                                            <td colspan="2" class="firstLeft active" style="background-color: #DFDFDF;">
                                                <div style="height: 20px; vertical-align: middle;">
                                                    <asp:Label ID="lbl_AnzeigeHaendlerSuche" runat="server">Händlersuche</asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="trHaendlernummer" class="formquery" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_HaendlerNummer" runat="server">Händlernummer:</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%">
                                                <asp:TextBox ID="txtNummer" runat="server" onkeydown="ShowWait();" onkeyup="RefreshSearch(this);" OnTextChanged="searchChanged" MaxLength="10" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_Name1" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name1:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" onkeydown="ShowWait();" onkeyup="RefreshSearch(this);" OnTextChanged="searchChanged" MaxLength="35" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Name2" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Name2:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName2" runat="server" onkeydown="ShowWait();" onkeyup="RefreshSearch(this);" OnTextChanged="searchChanged" MaxLength="35" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="tr_PLz" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" onkeydown="ShowWait();" onkeyup="RefreshSearch(this);" OnTextChanged="searchChanged" MaxLength="35" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_Ort" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" onkeydown="ShowWait();" onkeyup="RefreshSearch(this);" OnTextChanged="searchChanged" MaxLength="35" Width="200px" AutoPostBack="True"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="Tr_SelectionButton" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                Anzahl Treffer:
                                                <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                                <asp:Label ID="lblwait" Style="display: none" runat="server" Font-Bold="True">bitte warten..</asp:Label>
                                            </td>
                                        </tr>
                                        <tr id="tr_HaendlerAuswahl" runat="server" visible="false">
                                            <td colspan="2" class="firstLeft active">
                                                <div style="float: left">
                                                    <asp:ListBox ID="lbHaendler" runat="server" OnSelectedIndexChanged="selected" Width="500px" Height="126px" AutoPostBack="True">
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
                                                <p>
                                                    <asp:Label ID="lbl_Message" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lbl_error" runat="server" CssClass="TextError"></asp:Label></p>
                                                <p>
                                                    &nbsp;&nbsp;
                                                </p>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="2" align="right" class="rightPadding" style="width: 100%">
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="lbNext" runat="server" OnClick="nextClick" Enabled="false" CssClass="TablebuttonLarge" Width="130px"
                                                        Height="16px">» Weiter</asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lbReset" runat="server" OnClick="resetClick" CssClass="TablebuttonLarge"
                                                        Width="130px" Height="16px">» Neue Suche</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">&nbsp;</div>


                    <script type="text/javascript">
                        var refreshTimer;
                        var lastInputId;
                        function RefreshSearch(source) {
                            if (!source) return;

                            lastInputId = source.id;

                            clearTimeout(refreshTimer);
                            refreshTimer = setTimeout(function () {
                                __doPostBack(lastInputId, '');
                            }, 750);
                        };

                        function ShowWait() {
                            var wait = document.getElementById('<%= lblwait.ClientID %>');
                            wait.style.display = 'inline';
                        };

                        function RefreshDone(sender, args) {
                            var wait = document.getElementById('<%= lblwait.ClientId %>');
                            wait.style.display = 'none';

                            var input = document.getElementById(lastInputId);
                            if (input) {
                                input.focus();
                                input.select(input.value.length, 0);
                                lastInputId = null;
                            }
                        };

                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RefreshDone);
                    </script>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
