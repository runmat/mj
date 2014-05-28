<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change102s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change102s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
                    <div style="display: none">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                    <div id="data" style="border-left: solid 1px #dfdfdf; border-right: solid 1px #dfdfdf">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="2">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" ></asp:Label>
                                </td>
                            </tr>    
                            <tr class="formquery">
                                <td class="firstLeft active" style="vertical-align:top; padding-top:24px;width:15%" nowrap="nowrap">
                                    <asp:Label ID="lblKunde" runat="server" Text="Kunde:"></asp:Label>
                                </td>
                                <td style="width: 85%">
                                    <table id="tabInner" border="0" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">                               
                                        <tr>
                                            <td class="active" nowrap="nowrap" style="vertical-align:top; width: 70%" >
                                                <asp:DropDownList runat="server" ID="ddlCustomer" style="padding-left:0; margin-right:10px" CssClass="actives" 
                                                    AutoPostBack="true" Width="440px" >
                                                </asp:DropDownList>
                                            </td>
                                            <td valign="top" style="width: 30%" >
                                                <asp:Panel ID="pnInfo" runat="server" Visible="False" style="height: 68px; margin-right: 20px">
                                                    <div class="new_layout">
                                                        <div id="infopanel" class="infopanel" style="float:none">
                                                            <label>
                                                                <asp:Label ID="InfoHead" runat="server" Text="Information!"></asp:Label>
                                                            </label>
                                                            <div>
                                                                <asp:Label ID="InfoText" runat="server" ></asp:Label> 
                                                            </div>
                                                        </div>
                                                        <div id="excelpanel" style="margin-top:5px">
                                                            <asp:ImageButton runat="server" ID="ibtnExcelVorlage" ImageUrl="/Services/Images/iconXLS.gif" 
                                                                AlternateText="iconXLS.gif" Height="18px" Width="18px" ImageAlign="Middle" />
                                                            <label>
                                                                <asp:Label runat="server" Text="Laden Sie sich hier die Excelvorlage herunter"></asp:Label>
                                                            </label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>                            
                                </td>
                            </tr>
                            <tr class="formquery" id="trAktion" runat="server" visible="false">
                                <td class="firstLeft active" style="width: 15%">
                                    <asp:Label ID="lblAktion" runat="server" Text="Aktion:"></asp:Label>
                                </td>
                                <td class="active" style="width: 85%">
                                    <asp:RadioButton ID="rb_Freigeben" runat="server" GroupName="Vorgang" 
                                        Text="Freigeben" Visible="false" AutoPostBack="true" />
                                    <asp:RadioButton ID="rb_Sperren" runat="server" GroupName="Vorgang" 
                                        Text="Sperren" Visible="false" AutoPostBack="true" />&nbsp;
                                    <asp:RadioButton ID="rb_Entsperren" runat="server" GroupName="Vorgang" 
                                        Text="Entsperren" Visible="false" AutoPostBack="true" />&nbsp;
                                </td>
                            </tr>
                            <tr class="formquery" id="trFreigabeAuswahl" runat="server" visible="false">
                                <td class="firstLeft active" style="width: 15%">
                                    <asp:Label ID="lblFreigabeAuswahl" runat="server" Text="Vorgänge:"></asp:Label>
                                </td>
                                <td class="active" style="width: 85%">
                                    <asp:RadioButton ID="rb_Gesperrte" runat="server" GroupName="AuswahlFreigabe" 
                                        Text="gesperrte" Checked="True" />
                                    <asp:RadioButton ID="rb_Abgelehnte" runat="server" GroupName="AuswahlFreigabe" 
                                        Text="abgelehnte" />&nbsp;
                                </td>
                            </tr>
                            <tr id="trUpload" runat="server" visible="false" class="formquery" >
                                <td class="firstLeft active" style="width: 15%" >
                                    Dateiauswahl: 
                                </td>
                                <td class="active" style="width: 85%">
                                    <input id="upFile" type="file" name="File1" runat="server" size="49" />
                                    <a href="javascript:openinfo('Info02.htm');">
                                        <img src="/Services/Images/Fragezeichen03_10.jpg" alt="Fragezeichen03_10.jpg" border="0" height="16px" width="16px" align="middle" />
                                    </a>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                        <div id="dataQueryFooter" style="margin-right: 10px" >
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                Height="16px" CausesValidation="False" Font-Underline="False" Enabled="false" >» Weiter</asp:LinkButton>
                            &nbsp;
                        </div>
                        <script language="JavaScript" type="text/javascript">										
                            function openinfo(url) {
                                fenster = window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=yes,toolbars=0,location=0,directories=0,status=0,width=1000,height=250");
                                fenster.focus();
                            }
                        </script>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
