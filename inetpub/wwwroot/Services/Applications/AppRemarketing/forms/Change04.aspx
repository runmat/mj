<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Change04.aspx.cs" Inherits="AppRemarketing.forms.Change04"
    MasterPageFile="../Master/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" onclick="lbBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <table id="Table1" runat="server"  cellpadding="0" cellspacing="0" width="100%" style="width: 100%;border-bottom: none">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" nowrap="nowrap">
                                        Selektionsauswahl:
                                    </td>
                                    <td class="active" style="width: 88%">
                                        <span>
                                            <asp:RadioButton ID="rb_Haendler" Text="Selektion Händler" GroupName="Auswahl" runat="server"
                                                Checked="True" AutoPostBack="True" OnCheckedChanged="rb_Vermieter_CheckedChanged" />
                                            &nbsp;
                                            <asp:RadioButton ID="rbFin" Text="Upload Fahrgestellnummern" GroupName="Auswahl"
                                                runat="server" AutoPostBack="True" OnCheckedChanged="rbFin_CheckedChanged" />  
                                            &nbsp;<asp:RadioButton ID="rbDetail" Text="Detailsuche" GroupName="Auswahl"
                                                runat="server" AutoPostBack="True" 
                                            OnCheckedChanged="rbDetail_CheckedChanged" />                                                
                                                </span>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2">

                                    </td>
                                </tr>                                
                            </tbody>
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
                                <asp:Label ID="lblError" runat="server" CssClass="TextError" style="margin-left: 15px"></asp:Label>
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%;border-bottom: none">
                                    <tbody>
                                        <tr id="trNummerDetail" class="formquery" runat="server">
                                            <td class="firstLeft active" style="height: 25px">
                                                <asp:Label ID="lbl_NummerDetail" runat="server">Händlernr.:</asp:Label>
                                            </td>
                                            <td class="active" style="width: 100%; height: 25px;">
                                                <asp:TextBox ID="txtNummerDetail" runat="server" MaxLength="10" AutoPostBack="True"
                                                    OnTextChanged="txtNummerDetail_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trName1"  runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtName1_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trPLz" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True" OnTextChanged="txtPLZ_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trOrt"  runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"
                                                    OnTextChanged="txtOrt_TextChanged"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trSelectionButton"  runat="server" class="formquery">
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
                                        <tr id="trHaendlerAuswahl" runat="server">
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

                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right" style="width: 100%">
                                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                                    &nbsp;
                                                </div>
                                            </td>
                                        </tr>
                                </table>
                                <div id="divSearchButtons" runat="server" class="dataQueryFooter" >

                                                <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonMiddle" Width="100px"
                                                    Height="16px" OnClick="cmdSearch_Click">» Suchen</asp:LinkButton>
                                                <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" CssClass="TablebuttonMiddle"
                                                    Width="100px" Height="16px" OnClick="lbSelektionZurueckSetzen_Click1">» Neue Suche</asp:LinkButton>
                                                <asp:LinkButton ID="cmdWeiter" Visible="false" runat="server" CssClass="TablebuttonMiddle"
                                                    Width="100px" Height="16px" OnClick="cmdWeiter_Click">» Weiter</asp:LinkButton>
                                                       
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                          <table id="Table2" runat="server" visible="false" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_Upload" runat="server">lbl_Upload</asp:Label>
                                </td>
                                <td class="active" style="width: 100%">
                                    <input id="upFile1" type="file" size="35" name="File1" runat="server" style="background-color: #FFFFFF" />
                                    &nbsp;
                                        <a href="javascript:openinfo('InfoFin.htm');"><img src="/Services/Images/info.gif" border="0" height="16px" width="16px" 
                                     alt="Struktur Uploaddatei" title="Struktur Uploaddatei Fahrgestllnummern" /></a> 
                                    &nbsp; * max. 900 Datensätze
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" class="rightPadding" align="right">
                                    <asp:LinkButton ID="cmdUpload" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" OnClick="cmdUpload_Click">» Senden</asp:LinkButton>
                                </td>
                            </tr>
                            <tr class="formquery"  runat="server">
                                <td colspan="2" >
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery" >
                                <td colspan="2"  style="background-color: #dfdfdf; width: 100%">
                                     &nbsp;
                                </td>
                            </tr>                                                        
                        </table>
                        <table id="Table3" runat="server" visible="false" cellpadding="0" cellspacing="0"
                            style="width: 100%">
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td class="active" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    <asp:Label ID="lbl_DetailFIN" runat="server">lbl_DetailFIN</asp:Label>
                                </td>
                                <td class="active" style="width: 100%">
                                    <asp:TextBox ID="txtFinDetail" runat="server" MaxLength="17" Width="200px" OnTextChanged="txtOrt_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" class="rightPadding" align="right">
                                    <asp:LinkButton ID="lbtSendDetail" runat="server" CssClass="Tablebutton" Width="78px"
                                        Height="16px" onclick="lbtSendDetail_Click">» Senden</asp:LinkButton>
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td class="firstLeft active">
                                    &nbsp;
                                </td>
                                <td class="active" style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr class="formquery">
                                <td colspan="2" style="background-color: #dfdfdf; width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>

                  <div id="dataFooter">
                        &nbsp;</div>
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
