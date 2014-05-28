<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report07.aspx.vb" Inherits="AppF2.Report07"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc2" TagName="GridNavigation" Src="../../../PageElements/GridNavigation.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Kopfdaten" Src="../../../PageElements/Kopfdaten.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück" CausesValidation="false"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="UP1">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lbHaendler" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtNummer" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName1" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtName2" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtPLZ" EventName="TextChanged" />
                            <asp:AsyncPostBackTrigger ControlID="txtOrt" EventName="TextChanged" />
                        </Triggers>
                        <ContentTemplate>
                            <div id="TableQuery">
                                <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                                    <tbody>
                                        <tr class="formquery" id="tr1" runat="server">
                                            <td colspan="2" class="firstLeft active" style="font-weight: bold">
                                                <asp:Label ID="lbl_AnzeigeHaendlerSuche" Font-Underline="True" Font-Size="10pt" runat="server">lbl_AnzeigeHaendlerSuche</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="2">
                                                <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Visible="False"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trHaendlernummer" class="formquery" runat="server">
                                            <td class="firstLeft active">
                                                <asp:Label ID="lbl_HaendlerNummer" runat="server"></asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtNummer" runat="server" MaxLength="10" AutoPostBack="True"></asp:TextBox>
                                                <asp:Label ID="lblSHistoryNR" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="tr_Name1" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                <asp:Label runat="server" ID="lbl_Name1">Name1:</asp:Label>
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName1" runat="server" MaxLength="35" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryName1" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="Tr_Name2" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Name2:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtName2" runat="server" Enabled="False" BackColor=" #c1ccd9" MaxLength="35"
                                                    AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label ID="lblSHistoryName2" runat="server"
                                                        Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="tr_PLz" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                PLZ:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtPLZ" runat="server" MaxLength="35" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryPLZ" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                                &nbsp;
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="Tr_Ort" runat="server" class="formquery">
                                            <td class="firstLeft active">
                                                Ort:
                                            </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtOrt" runat="server" MaxLength="35" Width="200px" AutoPostBack="True"></asp:TextBox>&nbsp;<asp:Label
                                                    ID="lblSHistoryOrt" runat="server" Font-Italic="True" Font-Size="10" ForeColor="#c1ccd9"></asp:Label>
                                            </td>
                                            <td class="active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Datum von:
                                            </td>
                                            
                                            <td class="active">
                                            
                                                <asp:TextBox ID="txtDatumVon" runat="server" CssClass="InputTextbox" Width="150px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtDatumVon_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtDatumVon">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                        <td class="active" style="width: 50%">
                                                <asp:CompareValidator ID="cv_txtDatumVon" runat="server" ErrorMessage="Falsches Datumsformat"
                                                    Type="Date" ControlToValidate="txtDatumVon" ControlToCompare="TextBox1" Operator="DataTypeCheck"
                                                    CssClass="TextError" ForeColor=""></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                Datum bis:
                                                </td>
                                            <td class="active">
                                                <asp:TextBox ID="txtDatumBis" runat="server" CssClass="InputTextbox" 
                                                    Width="150px"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="txtDatumBis_CalendarExtender" runat="server" 
                                                    Enabled="True" TargetControlID="txtDatumBis">
                                                </ajaxToolkit:CalendarExtender>
                                                </td>
                                            <td class="active" style="width: 50%">
                                                <asp:CompareValidator ID="cv_txtDatumBis" runat="server" 
                                                    ControlToCompare="TextBox1" ControlToValidate="txtDatumBis" 
                                                    CssClass="TextError" ErrorMessage="Falsches Datumsformat" ForeColor="" 
                                                    Operator="DataTypeCheck" Type="Date"></asp:CompareValidator>
                                                </td>
                                        </tr>
                                                                               <tr class="formquery">
                                            <td colspan="3">
                                                <asp:TextBox ID="TextBox1" runat="server" Visible="false"></asp:TextBox>
                                                <asp:ImageButton ID="btnEmpty" runat="server" Height="16px" ImageUrl="../../../images/empty.gif"
                                                Width="1px" />                                                
                                            </td>
                                        </tr>
                                        <tr id="Tr_SelectionButton" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lbldirektInput" Style="display: none" runat="server" Width="40" ForeColor="green">
				                                        <u>Direkteingabe</u>
                                                </asp:Label><br />
                                                Anzahl Treffer:
                                                <asp:Label ID="lblErgebnissAnzahl" runat="server" Width="40"></asp:Label><br />
                                                <asp:Label ID="lblwait" Style="display: none" runat="server" ForeColor="red" Font-Bold="True">bitte warten</asp:Label>
                                            </td>
                                            <td class="firstLeft active" style="width: 50%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="tr_HaendlerAuswahl" runat="server" visible="false">
                                            <td colspan="3" class="firstLeft active">
                                                <div style="float: left">
                                                    <asp:ListBox ID="lbHaendler" runat="server" Width="500px" Height="126px" AutoPostBack="True">
                                                    </asp:ListBox>
                                                </div>
                                                <div>
                                                    <b>&nbsp;&nbsp;
                                                        <asp:Label ID="lblHaendlerDetailsNR" runat="server" Font-Size="11pt"></asp:Label></b><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsName1" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsName2" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="lblHaendlerDetailsStrasse" runat="server" Font-Size="10pt"></asp:Label><br />
                                                    <br />
                                                    <b>&nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsPLZ" runat="server" Font-Size="10pt"></asp:Label>
                                                        <br />
                                                        &nbsp;&nbsp;<asp:Label ID="lblHaendlerDetailsOrt" runat="server" Font-Size="10pt"></asp:Label>
                                                    </b>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tr_Message" runat="server" class="formquery">
                                            <td colspan="2" class="firstLeft active">
                                                <asp:Label ID="lbl_Info" runat="server" Width="100%">einfache / mehrfache Platzhaltersuche möglich z.B. 'PLZ= 9*', 'Name1=*Musterma*' </asp:Label>
                                                <p>
                                                    <asp:Label ID="lbl_Message" runat="server" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lbl_error" runat="server" CssClass="TextError"></asp:Label></p>
                                                <p>
                                                    &nbsp;&nbsp;
                                                </p>
                                            </td>
                                            <td class="firstLeft active" style="width: 50%">
                                                &nbsp;
                                                <asp:TextBox ID="txtHaendlerNr" runat="server" MaxLength="35" Visible="False"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                &nbsp;</td>
                                            <td align="right" class="rightPadding" style="width:100%" >
                                                &nbsp;
                                                <div id="dataQueryFooter">
                                                    <asp:LinkButton ID="cmdSearch" runat="server" CssClass="TablebuttonLarge" 
                                                        Height="16px" Width="130px">» Weiter</asp:LinkButton>
                                                    &nbsp;
                                                    <asp:LinkButton ID="lbSelektionZurueckSetzen" runat="server" 
                                                        CssClass="TablebuttonLarge" Height="16px" Width="130px">» Neue Suche</asp:LinkButton>
                                                </div>
                                            </td>
                                        </tr>
                                       </tbody>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>