<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangeStatusVersand.aspx.cs" Inherits="AppZulassungsdienst.forms.ChangeStatusVersand"  MasterPageFile="../MasterPage/App.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" PostBackUrl="../../../Start/Selection.aspx"></asp:LinkButton>
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
                                &nbsp;
                            </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="3" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:Label ID="lblKunde" runat="server">Kunde:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:TextBox ID="txtKunnr" onKeyPress="return numbersonly(event, false)" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%; vertical-align:top; margin-top:3px">
                                                    <asp:DropDownList ID="ddlKunnr" runat="server" AutoPostBack="True" EnableViewState="False" 
                                                        OnSelectedIndexChanged="ddlKunnr_SelectedIndexChanged"  Style="width: auto; position:absolute;">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>                                          

                                             <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblStva" runat="server">StVA:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVa" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px" AutoPostBack="True" OnTextChanged="txtStVa_TextChanged"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:DropDownList ID="ddlStVa" runat="server" Style="width: 375px" AutoPostBack="True"
                                                       OnSelectedIndexChanged="ddlStVa_SelectedIndexChanged" EnableViewState="False" >
                                                    </asp:DropDownList>
                                                </td>

                                            </tr>
                                                   <tr class="formquery">
                                                <td class="firstLeft active" style="height: 30px">
                                                    <asp:Label ID="lblDienst" runat="server">Lieferant/ ausf. ZLD:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" style="height: 30px">
                                                       </td>
                                                <td class="firstLeft active" style="width: 100%; height: 30px;">
                                                    <asp:DropDownList ID="ddlLief" runat="server" Style="width: 375px" 
                                                        AutoPostBack="True" 
                                                      >
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 36px">
                                                    <asp:Label ID="lblDatum" onKeyPress="return numbersonly(event, false)" runat="server">Datum der Zulassung von:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="2" style="height: 36px">
                                                    <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="txtZulDateFormate" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:Label ID="Label2" Style="padding-left: 15px; font-weight: bold"
                                                        Height="15px" runat="server">bis:</asp:Label>                                                        
                                                    <asp:TextBox ID="txtZulDateBis" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="lblZulDateFormat2" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>                                                        
                                               </td>
                                            
                                            </tr>                                            
                                            
                                                   <tr class="formquery">
                                                       <td class="firstLeft active" style="height: 36px">
                                                           <asp:Label ID="lblID" runat="server">ID:</asp:Label></td>
                                                       <td class="firstLeft active" colspan="2" style="height: 36px">
                                                        <asp:TextBox ID="txtID" runat="server" onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox></td>
                                            </tr>
                                            
                                                   <tr class="formquery">
                                                       <td class="firstLeft active" style="height: 36px">
                                                           <asp:Label ID="lblAuswahl" runat="server">Auswahl:</asp:Label></td>
                                                       <td class="firstLeft active" colspan="2" style="height: 36px">
                                                           <asp:RadioButton ID="rbAuswahl1"  Text="Offene Versandzulassungen (noch nicht zurück)" Checked="true" GroupName="Auswahl" runat="server" /></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 36px">
                                                    &nbsp;</td>
                                                <td class="firstLeft active" colspan="2" style="height: 36px">
                                                    <asp:RadioButton ID="rbAuswahl2" Text="Offene Versandzulassungen (RE noch nicht erhalten)" GroupName="Auswahl" runat="server" /></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" style="height: 36px">
                                                    &nbsp;</td>
                                                <td class="firstLeft active" colspan="2" style="height: 36px">
                                                    <asp:RadioButton ID="rbAuswahl3" Text="Erledigte Versandzulassungen"  GroupName="Auswahl" runat="server" /></td>
                                            </tr>
                                            
                                                   <tr class="formquery">
                                                <td colspan="3">
                                                    <asp:ImageButton
                                                        ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" onclick="btnEmpty_Click"  />
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
                                    Width="78px" onclick="cmdCreate_Click">» Erstellen </asp:LinkButton>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
