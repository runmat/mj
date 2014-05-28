<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change100s.aspx.vb" Inherits="CKG.Components.ComCommon.Treuhand.Change100s"
    MasterPageFile="../../../MasterPage/Services.Master" %>

<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
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
                    <div id="data" style="border-left: solid 1px #dfdfdf; border-right: solid 1px #dfdfdf">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                    <asp:Label ID="lblError" runat="server" ForeColor="#FF3300" textcolor="red" Visible="false"></asp:Label>
                                    <asp:Label ID="lblNoData" runat="server" Visible="False"></asp:Label>&nbsp
                                </td>
                            </tr>
                            
                            <tr class="formquery">
                           
                                <td class="firstLeft active" style="vertical-align:top; padding-top:5px;width:10%" nowrap="nowrap">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label>
                                </td>
                                
                               <td nowrap="nowrap" style="width: 90%"  colspan="2">

                                 <table id="tabInner" border="0" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                
                                 <tr>

                                  <td width="500" class="active" nowrap="nowrap" >
                                        
                                        <asp:RadioButtonList style="padding-left: 0" class="actives"  ID="rdbCustomer" runat="server" 
                                        AutoPostBack="True" Width="450px">
                                        </asp:RadioButtonList>
                                  
                                </td>
                                 <td width="300" valign="top" >
                                    
                                    <asp:Panel ID="pnInfo" runat="server" Visible="False" Height="68px">
                                    <div class="new_layout">
                                    <div id="infopanel" class="infopanel">
                                    <label><asp.label id="InfoHead" runat="server">Information!</asp.label></label>
                                    <div><asp:Label ID="InfoText" runat="server" ></asp:Label> 
                                    </div></div></div>
                                    </asp:Panel>

                                </td>
                                <td width="30">
                                </td>

                                </tr>
                                </table>
                            
                                </td>
                  
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" nowrap="nowrap" colspan="3" style="height: 22px">
                                    &nbsp
                                </td>
                            </tr>
                            <tr class="formquery" id="trAktion" runat="server" visible="false">
                                <td class="firstLeft active" style="width: 15%">
                                    <asp:Label ID="lblAktion" runat="server" Text="Aktion:"></asp:Label>
                                </td>
                                <td class="active">
                                    <span >
                                    <asp:RadioButton ID="rb_Sperren" GroupName="Vorgang" runat="server" Text="sperren"
                                        Checked="True" /></span>
                                    <span ><asp:RadioButton ID="rb_Entsperren" runat="server" GroupName="Vorgang" 
                                        Text="entsperren" /></span>
                                </td>
                            </tr>
                            <tr id="trUpload" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Dateiauswahl: 
                                </td>
                                <td class="active" style="width: 88%" colspan="2">
                                    <input id="upFile" type="file" size="49" name="File1" runat="server" /> <a href="javascript:openinfo('Info01.htm');"><img src="/Services/Images/Fragezeichen03_10.jpg" border="0" height="16px" width="16px" /></a>
                                </td>
                            </tr>
                           <%-- <tr id="trTabName" runat="server" visible="false" class="formquery">
                                <td class="firstLeft active" nowrap="nowrap">
                                    Tabellenname:</td>
                                <td class="active" style="width: 88%" colspan="2">
                                    <asp:TextBox ID="txtTabName" CssClass="InputTextbox" Width="150px" runat="server">Tabelle1</asp:TextBox>
                                    <img src="/Services/Images/Fragezeichen03_10.jpg" alt="Information" onclick="alert('Der Name des Tabellenblattes (Standard: Tabelle1)')" border="0" height="16px" width="16px" />
                                </td>
                            </tr>--%>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>

                              <tr class="formquery"><td colspan="3" align="right" class="rightPadding" 
                                    style="width: 100%">
                            
                        <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                            &nbsp;
                        </div>
                            
                            </td></tr> 

                            <tr class="formquery">
                                <td colspan="3" align="right" class="rightPadding" 
                                    style="width: 100%; height: 70px;">
                                    <div id="dataQueryFooter" >
                                        <asp:LinkButton ID="cmdSearch" runat="server" CssClass="Tablebutton" Width="78px"
                                            Height="16px" CausesValidation="False" Font-Underline="False">» Weiter</asp:LinkButton>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active" colspan="3" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>

                        <script language="JavaScript" type="text/javascript">										
				                                <!--
                            function openinfo(url) {
                                fenster = window.open(url, "Zulassungsdatum", "menubar=0,scrollbars=0,toolbars=0,location=0,directories=0,status=0,width=650,height=250");
                                fenster.focus();
                            }
				                                -->
                        </script>

                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
