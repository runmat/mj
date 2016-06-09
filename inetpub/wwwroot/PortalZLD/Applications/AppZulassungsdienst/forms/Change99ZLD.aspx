<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change99ZLD.aspx.cs" Inherits="AppZulassungsdienst.forms.Change99ZLD"     MasterPageFile="../MasterPage/Big.Master" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                            <asp:Panel ID="Panel1" DefaultButton="btnEmpty" runat="server">
                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="2" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="#269700" Font-Bold="True" ></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKennzeichen" runat="server">Ortskennzeichen:</asp:Label>
                                                </td>
                                                <td class="active" style="width: 100%">
                                                    <asp:TextBox ID="txtKennzeichen" runat="server" MaxLength="3" CssClass="TextBoxNormal"></asp:TextBox>
                                                    &nbsp;<asp:Label ID="lblEingabe" runat="server" CssClass="TextError" 
                                                        Text="Eingabe erforderlich"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                        Width="1px" />
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
                                    Width="78px" onclick="cmdCreate_Click">» Suchen</asp:LinkButton>
                            </div>
                            <div id="Result" runat="Server" visible="false">
                                <div id="data">
                                    <table cellspacing="0" cellpadding="0" width="100%" style="border: solid 1px #dfdfdf;
                                        margin-bottom: 5px" bgcolor="white" border="0" id="tblData" runat="server">
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="11" style="height: 22px;">
                                               <asp:Label ID="lblBez" runat="server" Font-Size="10pt" Text="Dokumentenanforderung für ZLST "></asp:Label> <asp:Label ID="lblKennz" Font-Size="10pt" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active" height="22">
                                                <u>Kategorie\Dokument</u>**
                                            </td>
                                            <td class="firstLeft active" nowrap="nowrap">
                                                ZB1
                                            </td>
                                            <td class="firstLeft active">
                                                ZB2
                                            </td>
                                            <td class="firstLeft active">
                                                CoC
                                            </td>
                                            <td class="firstLeft active">
                                                eVB
                                            </td>
                                            <td class="firstLeft active">
                                                VM
                                            </td>
                                            <td class="firstLeft active">
                                                PA
                                            </td>
                                            <td class="firstLeft active">
                                                GewA
                                            </td>
                                            <td class="firstLeft active">
                                                HRA
                                            </td>
                                            <td class="firstLeft active">
                                                SEPA
                                            </td>
                                            <td class="firstLeft active" style="width: 100%">
                                                Bemerkung
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="11" style="background-color: #dfdfdf; height: 22px;">
                                                Privat
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Zulassung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPZUL_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtPZUL_BEM" runat="server" 
                                                    style="font-weight: normal; font-size: 8pt" MaxLength="70" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umschreibung</td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" 
                                                  ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMSCHR_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtPUMSCHR_BEM" onKeyPress="return only_O_K_F(event)" 
                                                    runat="server" style="font-weight: normal; font-size: 8pt" MaxLength="70" 
                                                    Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umkennzeichnung</td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPUMK_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtPUMK_BEM" runat="server" style="font-weight: normal; font-size: 8pt" MaxLength="70" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Ersatzfahrzeugschein
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtPERS_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtPERS_BEM" runat="server" MaxLength="70" style="font-weight: normal; font-size: 8pt" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr> 
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="11" style="background-color: #dfdfdf; height: 22px;">
                                                Unternehmen
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Zulassung
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUZUL_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtUZUL_BEM" runat="server" MaxLength="70" style="font-weight: normal; font-size: 8pt" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>                                         
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umschreibung</td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMSCHR_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtUUMSCHR_BEM" runat="server" MaxLength="70" style="font-weight: normal; font-size: 8pt" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>   
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Umkennzeichnung</td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUUMK_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtUUMK_BEM" runat="server" MaxLength="70" style="font-weight: normal; font-size: 8pt" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>  
                                        <tr class="formquery">
                                            <td class="firstLeft active" style="height: 22px;">
                                                Ersatzfahrzeugschein
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_SCHEIN" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_BRIEF" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px" ></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_COC" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_DECK" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_VOLLM" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_AUSW" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_GEWERB" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_HANDEL" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal">
                                                <asp:TextBox ID="txtUERS_LAST" onKeyPress="return only_O_K_F(event)" runat="server" MaxLength="1" Width="40px"></asp:TextBox>
                                            </td>
                                            <td class="firstLeft active" style="font-weight: normal; font-size: 6pt">
                                                <asp:TextBox ID="txtUERS_BEM" runat="server" MaxLength="70" style="font-size: 8pt" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr> 
                                    </table>
                                    <table class="TableLegende" id="Table2" height="107" cellspacing="1" cellpadding="2"
                                        bgcolor="#ffffff" border="0" style="font-size: 6pt">
                                        <tr>
                                            <td>
                                                <u>**Legende:</u>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                O=Original
                                            </td>
                                            <td>
                                                K=Kopie
                                            </td>
                                            <td>
                                                F=Formular Zulassungsstelle
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                ZB1=Fahrzeugschein,
                                            </td>
                                            <td>
                                                ZB2=Fahrzeugbrief,
                                            </td>
                                            <td nowrap>
                                                CoC=Certificate of Conformity,
                                            </td>
                                            <td>
                                                eVB=elektronische Versicherungsbestätigung,
                                            </td>
                                            <td>
                                                VM=Vollmacht,
                                            </td>
                                            <td>
                                                PA=Personalausweis,
                                            </td>
                                            <td>
                                                GewA=Gewerbeanmeldung,
                                            </td>
                                            <td>
                                                HRA=Handelsregister,
                                            </td>
                                            <td>
                                                SEPA=SEPA-Mandat für Kfz-Steuer
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="10">
                                                &nbsp;
                                            </td>
                                             </tr>

                                    </table>
                                    <table class="TableLinks" id="Table1" cellspacing="1" cellpadding="2" align="center"
                                        border="0">
                                        <tr class="formquery" >
                                            <td class="firstLeft active" colspan="2" style="background-color: #dfdfdf; height: 22px;">
                                                Links
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                    <asp:Label ID="lblAmt" runat="server" Width="125"
                                                        >Amt</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%" >
                                                    <asp:TextBox ID="txtAmt" runat="server" Width="700px" ></asp:TextBox>
                                                    <asp:ImageButton ID="ibtnAmt" Width="22px" ToolTip="Prüfen" Height="22px" 
                                                        ImageUrl="/PortalZLD/images/Welt_02.jpg" runat="server" 
                                                        onclick="ibtnAmt_Click" />
                                            </td>                                            
                                        </tr>                                                                                 
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                    <asp:Label ID="lblWunsch" runat="server" 
                                                        >Wunschkennzeichen</asp:Label>
                                            </td>
                                            <td class="active" >
                                                    <asp:TextBox ID="txtWunsch" runat="server" Width="700px"></asp:TextBox>
                                                    <asp:ImageButton ID="ibtnWunsch" ToolTip="Prüfen" Width="22px" Height="22px" 
                                                        ImageUrl="/PortalZLD/images/Welt_02.jpg" runat="server" 
                                                        onclick="ibtnWunsch_Click" />
                                            </td>                                            
                                        </tr>                                                                                 
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                    <asp:Label ID="lblFormulare" runat="server" >Formulare</asp:Label>
                                            </td>
                                            <td class="active" colspan="2" >
                                                    <asp:TextBox ID="txtFormular" runat="server" Width="700px"></asp:TextBox>
                                                    <asp:ImageButton ID="ibtnFormular" ToolTip="Prüfen" Width="22px" Height="22px" 
                                                        ImageUrl="/PortalZLD/images/Welt_02.jpg" runat="server" 
                                                        onclick="ibtnFormular_Click" />
                                            </td>                                            
                                        </tr>                                                                                 
                                        <tr class="formquery">
                                            <td class="firstLeft active" >
                                                <asp:Label ID="lblGebuehr" runat="server" >Gebühren</asp:Label>
                                            </td>
                                            <td class="active" colspan="2" >
                                               <asp:TextBox ID="txtGeb" runat="server"  Width="700px"></asp:TextBox>
                                               <asp:ImageButton ID="ibtnGeb" ToolTip="Prüfen" Width="22px" Height="22px" 
                                                    ImageUrl="/PortalZLD/images/Welt_02.jpg" runat="server" 
                                                    onclick="ibtnGeb_Click" />
                                            </td>                                            
                                        </tr>                                                                                 
                                    </table>
                                </div>
                                                              
                            </div>

                    <div id="dataFooter">
                        <asp:LinkButton ID="cmdSave" Visible="false" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdSave_Click" >» Speichern</asp:LinkButton></div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
