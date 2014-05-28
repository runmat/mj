<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change01.aspx.cs" Inherits="Leasing.forms.Change01"  MasterPageFile="../Master/App.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" OnClick="lbBack_Click" CausesValidation="False"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Leasingnummer" runat="server">lbl_Leasingnummer</asp:Label></td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtLeasingnummer" CssClass="TextBoxNormal" runat="server" 
                                        ></asp:TextBox></td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    <asp:Label ID="lbl_Kunde" runat="server">lbl_Kunde</asp:Label></td>
                                <td class="active" style="width: 88%">
                                    <asp:TextBox ID="txtKunde" CssClass="TextBoxNormal" runat="server" 
                                        ></asp:TextBox></td>
                            </tr>                                                        
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Liefertermine:
                                </td>
                                <td class="active" style="width: 88%">
                                    <span>
                                    <asp:radiobuttonlist id="rbAktion" runat="server" Width="292px" RepeatDirection="Horizontal">
									<asp:ListItem Value="A" Selected="True">alle</asp:ListItem>
									<asp:ListItem Value="U">unbearbeitet</asp:ListItem>
									<asp:ListItem Value="B">bearbeitet</asp:ListItem>
									</asp:radiobuttonlist></span>
                                </td>
                                 
                            </tr>

                                                    
                            
                            <tr class="formquery">
                                <td colspan="2" align="right" style="width: 100%">
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="dataQueryFooter">
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" OnClick="cmdSearch_Click">» 
                            Weiter</asp:LinkButton>
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function openinfo(url) {
            fenster = window.open(url, "Uploadstruktur", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=750,height=350");
            fenster.focus();
        }
 
    </script>
</asp:Content>
