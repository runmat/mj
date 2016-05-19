<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeZLDSelect.aspx.cs" Inherits="AppZulassungsdienst.forms.ChangeZLDSelect"     MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?26082013"></script>
    <script language="javascript" type="text/javascript">
        function checkZulassungsdatum() {
            var tb = document.getElementById('<%= txtZulDate.ClientID %>');
            document.getElementById('<%= ihDatumIstWerktag.ClientID %>').value = nurWerktage(tb.value)[0];
            return true;
        }
    </script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="paginationQuery">

                            </div>

                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKunnr" runat="server" onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" AutoPostBack="True" EnableViewState="False" 
                                                        OnSelectedIndexChanged="ddlKunnr_SelectedIndexChanged"  Style="width: auto;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trKundengruppe" runat="server" visible="false">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                   <asp:Label ID="lblGruppe" runat="server">Kundengruppe:</asp:Label></td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:DropDownList ID="ddlGruppe" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trTour" runat="server" visible="false">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblTour" runat="server">Tour:</asp:Label></td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                   <asp:DropDownList ID="ddlTour" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr> 
                                            <tr class="formquery" id="trStva" runat="server">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblStva" runat="server">StVA:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlStVa"  runat="server"  Style="width: 375px" AutoPostBack="True"
                                                       OnSelectedIndexChanged="ddlStVa_SelectedIndexChanged" EnableViewState="False" >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trDienstleistung" runat="server">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblDienst" runat="server">Dienstleistung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtMatnr" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:DropDownList ID="ddlDienst" runat="server" Style="width: 375px"
                                                        AutoPostBack="True" onselectedindexchanged="ddlDienst_SelectedIndexChanged"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblDatum" runat="server">Datum der Zulassung:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:TextBox ID="txtZulDate" onKeyPress="return numbersonly(event, false)" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                        ID="lbtnGestern" Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeute"
                                                        Width="50px" Text="Heute |" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgen"
                                                        Width="60px" Text="Morgen" />
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="lblbID" runat="server">ID:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:TextBox ID="txtID" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr class="formquery" id="trVorgang" runat="server">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="Label1" runat="server">Vorgang:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:RadioButton ID="rbAlle" Checked = "true" GroupName="Vorgang" runat="server" Text="alle" />
                                                    <asp:RadioButton ID="rbNZ" GroupName="Vorgang" runat="server" Text="normale Vorgänge" />
                                                    <asp:RadioButton ID="rbON" GroupName="Vorgang" runat="server" Text="Online" />
                                                    <asp:RadioButton ID="rbAH" GroupName="Vorgang" runat="server" Text="Autohaus" />
                                                    <asp:RadioButton ID="rbAH_NZ" GroupName="Vorgang" runat="server" Text="Normal und Autohaus" />
                                                    <asp:RadioButton ID="rbOK" GroupName="Vorgang" runat="server" Text="Online-Vorgänge Kroschke Homepage" />
                                                </td>
                                            </tr> 
                                            <tr class="formquery" id="trFlieger" runat="server">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="Label2" runat="server">Klärfälle:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:CheckBox ID="chkFlieger" runat="server" />
                                                </td>
                                            </tr> 
                                            <tr class="formquery" id="trNochNichtAbgesendete" runat="server">
                                                <td class="firstLeft active" style="width: 125px; height: 30px">
                                                    <asp:Label ID="Label3" runat="server">Noch nicht abgesendete Vorgänge:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="width: 750px">
                                                    <asp:CheckBox ID="chkNochNichtAbgesendete" runat="server" />
                                                </td>
                                            </tr> 
                                            <tr class="formquery">
                                                <td colspan="3">

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </div>

                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click" OnClientClick="checkZulassungsdatum();">» Erstellen </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                    <input type="hidden" runat="server" id="ihDatumIstWerktag" value="false" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
