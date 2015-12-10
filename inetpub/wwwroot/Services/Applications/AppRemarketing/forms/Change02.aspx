<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change02.aspx.cs" Inherits="AppRemarketing.forms.Change02"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                &nbsp;<asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text=""></asp:Label>
                        </h1>
                    </div>
                    
                    <asp:Panel ID="Panel2" DefaultButton="btndefault" runat="server">
                        <div id="TableQuery">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr id="tr_Message" runat="server" class="formquery" >
                                    <td colspan="2" class="firstLeft active" style="width: 100%">
                                        <p>
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label></p>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active">
                                        Selektionsauswahl:</td>
                                    <td class="active" style="width: 100%">
                                        <span>
                                        <asp:RadioButton ID="rbFin" runat="server" AutoPostBack="True" Checked="True" 
                                            GroupName="Auswahl" OnCheckedChanged="rbFin_CheckedChanged" 
                                            Text="Upload/Einzelauswahl" />
                                        <asp:RadioButton ID="rb_Haendler" runat="server" AutoPostBack="True" 
                                            GroupName="Auswahl" 
                                            Text="Selektion Händler" oncheckedchanged="rb_Haendler_CheckedChanged" />
                                        </span>
                                    </td>
                                </tr>
                                <tr ID="tr_Fin" runat="server" class="formquery">
                                    <td class="firstLeft active">
                                        Fahrgestellnummer</td>
                                    <td class="active" style="width: 100%">
                                        <asp:TextBox ID="txtFin" runat="server" MaxLength="17" Width="220px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr class="formquery" ID="tr_upload" runat="server" style="padding-bottom:10px" >
                                    <td class="firstLeft active">
                                        <asp:Label ID="lbl_Upload" runat="server">lbl_Upload</asp:Label>
                                    </td>
                                    <td class="active" style="width: 100%">
                                        <input ID="upFile1" runat="server" name="File1" size="35" type="file" /> &nbsp;
                                        <a href="javascript:openinfo('InfoFin.htm');">
                                        <img alt="Struktur Uploaddatei" border="0" height="16px" 
                                            src="/Services/Images/info.gif" title="Struktur Uploaddatei Fahrgestllnummern" 
                                            width="16px" /></a> &nbsp; * max. 900 Datensätze
                                    </td>
                                </tr>
                                                               
                                
                                <tr  class="formquery" style="padding-bottom:10px">
                                    <td class="firstLeft active" colspan="2">
                                        &nbsp;</td>
                                </tr>
                                                               
                                
                            </table>
                            
                             <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lbHaendler" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtNummerDetail" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtName1" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtPLZ" EventName="TextChanged" />
                                <asp:AsyncPostBackTrigger ControlID="txtOrt" EventName="TextChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <table id="tblSuche" runat="server" visible="false" cellpadding="0" cellspacing="0" width="100%" style="width: 100%;border-bottom: none">
                                        <tr id="trNummerDetail" visible="false" class="formquery" runat="server">
                                            <td class="firstLeft active" style="height: 25px">
                                                <asp:Label ID="lbl_NummerDetail" runat="server">Händlernr.:</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%; height: 25px;">
                                                <asp:TextBox ID="txtNummerDetail" runat="server" MaxLength="10" AutoPostBack="True"
                                                    OnTextChanged="txtNummerDetail_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trName1" runat="server" class="formquery" Visible="False">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtName1_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trPLz" runat="server" class="formquery" Visible="False">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtPLZ_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trOrt"  runat="server" class="formquery" Visible="False">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"
                                                    OnTextChanged="txtOrt_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trSelectionButton" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active" style="height: 57px">
                                                Anzahl Treffer:
                                                <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trHaendlerAuswahl" runat="server" visible="false">
                                            <td colspan="2" class="firstLeft active">
                                                <div style="float: left">
                                                    <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="126px" AutoPostBack="True"
                                                        OnSelectedIndexChanged="lbHaendler_SelectedIndexChanged"></asp:ListBox>
                                                </div>
                                                <div>
                                                    <b>&nbsp;&nbsp;
                                                        <asp:Label ID="lblHalter" runat="server" Font-Size="11pt"></asp:Label></b><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerName1" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerName2" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerStrasse" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    <br />
                                                    <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerPLZ" runat="server" Font-Size="10pt"></asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblHaendlerOrt" runat="server" Font-Size="10pt"></asp:Label>
                                                    </b>
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr class="formquery" id="trHaendlerCommands" runat="server">
                                            <td colspan="2" class="rightPadding" align="right" style="padding-top:10px">
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        
                                </table>
                                
                            </ContentTemplate>
                        </asp:UpdatePanel>
                            
                            
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr class="formquery" style="background-color: #dfdfdf; width: 100%">
                                    <td>
                                        &nbsp;</td>
                                </tr>
                            </table>
                            
                            
                        </div>
                        <div id="dataFooter">
                            <asp:Button Style="display: none" UseSubmitBehavior="false" ID="btndefault" runat="server"
                                Text="Button" />
                            <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonMiddle" 
                                Height="16px" OnClick="cmdSearch_Click" Width="100px" Visible="False">» Suchen</asp:LinkButton>
                            <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" 
                                CssClass="TablebuttonMiddle" Height="16px" 
                                OnClick="lbSelektionZurueckSetzen_Click1" Width="100px" Visible="False">» Neue Suche</asp:LinkButton>
                            <asp:LinkButton ID="lbCreate" runat="server" CssClass="Tablebutton" 
                                onclick="lbCreate_Click" style="margin-bottom: 0px" Width="78px">» Weiter</asp:LinkButton>
                        </div>
                    </asp:Panel>
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
