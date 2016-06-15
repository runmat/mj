<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VorgangStatusaenderung.aspx.cs" Inherits="AppZulassungsdienst.forms.VorgangStatusaenderung" MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript" src="../JavaScript/helper.js?22042016"></script>

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
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
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
                                                        ImageUrl="../../../Images/queryArrow.gif" onclick="NewSearch_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div style="margin-top: 25px">
                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblSearchID" runat="server">ID:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtSearchID" runat="server" MaxLength="10" CssClass="TextBoxNormal"></asp:TextBox>
                                                  </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="cmdCreate_Click" />
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
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Suchen </asp:LinkButton>
                            </div>
                            <asp:Panel ID="Panel2" DefaultButton="btnEmpty2" runat="server" Visible="False">
                                <div id="Result" runat="Server">
                                    <div id="data">
                                        <table cellspacing="0" cellpadding="0" width="100%" bgcolor="white" border="0">
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblID" runat="server">ID:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblIDDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblBelegtyp" runat="server">Belegtyp:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBelegtypDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblZulassungsdatum" runat="server">Zulassungsdatum:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblZulassungsdatumDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblKundennummer" runat="server">Kundennummer:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblKundennummerDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblKundeDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblKreis" runat="server">Kreis:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblKreisDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Kennzeichen:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblKennzeichenDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblBEBStatus" runat="server">BEB-Status:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblBEBStatusDisplay" runat="server"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery" style="height: 32px">
                                                <td class="firstLeft active" style="width: 150px">
                                                    <asp:Label ID="lblBEBStatusNeu" runat="server">neuer BEB-Status:</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" ID="ddlBEBStatus"/>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton
                                                        ID="btnEmpty2" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="cmdSave_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </asp:Panel>
                            <div id="dataFooter">
                                <asp:LinkButton ID="cmdSave" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdSave_Click" Visible="False">» Speichern </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
