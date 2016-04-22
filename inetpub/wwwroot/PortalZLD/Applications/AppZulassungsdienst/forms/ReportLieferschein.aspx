<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportLieferschein.aspx.cs" Inherits="AppZulassungsdienst.forms.ReportLieferschein"  MasterPageFile="../MasterPage/App.Master" %>

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

                            <div id="paginationQuery" style="height: 22px">
                                &nbsp;
                            </div>

                                <div id="TableQuery" style="margin-bottom: 10px">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="4" width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblStva" runat="server">StVA von:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtStVavon" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label1" runat="server">bis:</asp:Label></td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:TextBox ID="txtStVaBis" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px"></asp:TextBox></td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblKunde" runat="server">Kunde von:</asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtKunnr" runat="server" CssClass="TextBoxNormal" 
                                                        MaxLength="8" Width="75px"></asp:TextBox>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="Label2" runat="server">bis:</asp:Label></td>
                                                <td class="firstLeft active" style="width: 100%;">
                                                    <asp:TextBox ID="txtKunnrBis" runat="server" CssClass="TextBoxNormal" MaxLength="8" 
                                                        Width="75px"></asp:TextBox></td>
                                            </tr>                                          


                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                   <asp:Label ID="lblGruppe" runat="server">Kundengruppe:</asp:Label></td>
                                                <td class="firstLeft active" colspan="3" >
                                                    <asp:DropDownList ID="ddlGruppe" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr>                                            
                                           

                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblTour" runat="server">Tour:</asp:Label></td>
                                                <td class="firstLeft active" colspan="3" >
                                                   <asp:DropDownList ID="ddlTour" runat="server" Style="width: 375px"></asp:DropDownList>
                                                </td>
                                            </tr>                                            
                                           

                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblDatum" runat="server">Datum der Zulassung von:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3" >
                                                    <asp:TextBox ID="txtZulDate" runat="server" CssClass="TextBoxNormal" 
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
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblDatumbis" runat="server">Datum der Zulassung bis:</asp:Label>
                                                </td>
                                                <td class="firstLeft active" colspan="3" >
                                                    <asp:TextBox ID="txtZulDateBis" runat="server" CssClass="TextBoxNormal" 
                                                        Width="75px" MaxLength="6"></asp:TextBox>
                                                    <asp:Label ID="Label4" Style="padding-left: 2px; font-weight: normal"
                                                        Height="15px" runat="server">(ttmmjj)</asp:Label>
                                                    <asp:LinkButton runat="server" Style="padding-left: 10px; font-weight: normal" Height="15px"
                                                        ID="lbtnGesternbis" Text="Gestern |" Width="60px" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnHeutebis"
                                                        Width="50px" Text="Heute |" />
                                                    <asp:LinkButton runat="server" Style="font-weight: normal" Height="15px" ID="lbtnMorgenbis"
                                                        Width="60px" Text="Morgen" />
                                                </td>
                                            </tr> 
                                                   <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblAuswahl" runat="server">Auswahl Lieferschein:</asp:Label>
                                                </td>
                                                <td class="firstLeft active"  colspan="3">
                                                        <asp:RadioButton id="rbohneGeb" runat="server" Text = "ohne Gebühr" GroupName="Auswahl" Checked="True"></asp:RadioButton>
                                                        <asp:RadioButton  id="rbmitGebHoch" runat="server" Text = "mit Gebühr(Hochformat)" GroupName="Auswahl"></asp:RadioButton>
                                                        <asp:RadioButton  id="rbmitGebQuer" runat="server" Text = "mit Gebühr(Querformat)" GroupName="Auswahl"></asp:RadioButton>
     
                                                </td>
                                                </tr>
                                                   <tr class="formquery">
                                                <td colspan="4">

                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        
                                    </div>
                                </div>

                            <div id="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="Tablebutton" 
                                    Width="78px" onclick="cmdCreate_Click">» Weiter </asp:LinkButton>
                            </div>
                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
