<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Input_002_02.aspx.cs" Inherits="Leasing.forms.Input_002_02" EnableEventValidation="false"
    MasterPageFile="../../../MasterPage/Services.Master"%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
               <div id="navigationSubmenu">
                    <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                        Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="(Zusammenstellung von Abfragekriterien)" runat="server"></asp:Label>
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
                                        <td colspan="4" class="firstLeft active">                                             
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Amtl. Kennzeichen (von,bis)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKennzeichenVon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtKennzeichenBis" runat="server" ></asp:TextBox>&nbsp;(M-X1000)&nbsp;
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Fahrgestellnummer (von,bis)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFahrgestellVon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtFahrgestellBis" runat="server"></asp:TextBox>&nbsp;(WDX11111111111111)
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Leasingvertragsnr. (von,bis)
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLeasVVon" runat="server"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtLeasVBis" runat="server"></asp:TextBox>&nbsp;(1000000012)
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Kundennummer
                                        </td>
                                        <td class="active" nowrap="nowrap">
                                        
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtKundennr" runat="server"></asp:TextBox>&nbsp;(23632)
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                      <tr>
                                        <td colspan="4">
                                        &nbsp;
                                        </td>
                                        </tr>
                                    <tr >
                                        <td colspan="3">
                                        <ul>
                                            <asp:RadioButtonList  BorderWidth="0" BorderStyle="None" ID="rbStatus" runat="server">
                                                <asp:ListItem Value="ALL" Selected="True">Alle</asp:ListItem>
                                                <asp:ListItem Value="DAD1">Angelegt beim DAD</asp:ListItem>
                                                <asp:ListItem Value="LEA1">Beim LN</asp:ListItem>
                                                <asp:ListItem Value="DAD2">Zur&#252;ck vom LN</asp:ListItem>
                                                <asp:ListItem Value="VER1">Bei Versicherung</asp:ListItem>
                                                <asp:ListItem Value="DAD3">Zur&#252;ck von Versicherung</asp:ListItem>
                                                <asp:ListItem Value="DAD4">Leasingende &#252;berschritten</asp:ListItem>
                                            </asp:RadioButtonList>
                                            </ul>
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr >
                                        <td class="firstLeft active" colspan="3">
                                         
                                          <ul>
                                         
                                            <asp:RadioButtonList BorderWidth="0" BorderStyle="None" ID="rbMahnung" runat="server" Visible="False">
                                                <asp:ListItem Value="MALL" Selected="True">Alle</asp:ListItem>
                                                <asp:ListItem Value="M1LN">Stufe 1 LN</asp:ListItem>
                                                <asp:ListItem Value="M2LN">Stufe 2 LN</asp:ListItem>
                                                <asp:ListItem Value="M3LN">Stufe 3 LN</asp:ListItem>
                                                <asp:ListItem Value="M4LN">Stufe 4 LN</asp:ListItem>
                                                <asp:ListItem Value="M1VG">Stufe 1 VG</asp:ListItem>
                                                <asp:ListItem Value="M2VG">Stufe 2 VG</asp:ListItem>
                                                <asp:ListItem Value="M3VG">Stufe 3 VG</asp:ListItem>
                                                <asp:ListItem Value="M4VG">Stufe 4 VG</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:RadioButtonList CssClass="radio" ID="rbSelect" runat="server" Visible="False" Height="49px">
                                                <asp:ListItem Value="H" Selected="True">Historie</asp:ListItem>
                                                <asp:ListItem Value="M">Mahnstufen</asp:ListItem>
                                            </asp:RadioButtonList>
                                            <asp:CheckBox ID="lblKF" runat="server" Visible="False" Text="nur Klärfälle"></asp:CheckBox>
                                        </ul>
                                       
                                        </td>
                                        <td width="100%">
                                            &nbsp;
                                        </td>
                                        
                                       
                                        
                                    </tr>
                                   
                                </tbody>
                            </table>
                        </div>
                        <div id="dataFooter">
                            <asp:LinkButton ID="cmdCreate" Text="Report erstellen" Height="16px" Width="128px"
                                runat="server" CssClass="TablebuttonLarge" OnClick="cmdCreate_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
