<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change10.aspx.vb" Inherits="KBS.Change10"
    MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script language="JavaScript" type="text/javascript" src="../Java/JScript.js"></script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Wechselgeldzählung</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="4">
                                            <div class="paginationQuery">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tbody>
                                                        <tr>
                                                            <td style="font-size: 12px">
                                                                <asp:Label ID="lblNoData" runat="server" Text="Bitte füllen Sie alle Pflichtfelder(*) aus!"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4">
                                            <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="width: 100%; height: 41px;">
                                            <table cellpadding="0" cellspacing="0" style="border: none">
                                                <tr class="formquery">
                                                    <td class="firstLeft active">
                                                        Datum:
                                                    </td>
                                                    <td class="active" style="width: 100%;">
                                                        <div id="NeutralCalendar">
                                                            <asp:TextBox ID="txtDatum" CssClass="TextBoxShort" runat="server" TabIndex="1" Width="78px"></asp:TextBox>
                                                        </div>
                                                        <cc1:CalendarExtender ID="txtDatum_CalendarExtender" runat="server" Format="dd.MM.yyyy"
                                                            PopupPosition="BottomLeft" Animated="true" Enabled="True" TargetControlID="txtDatum">
                                                        </cc1:CalendarExtender>
                                                        <cc1:MaskedEditExtender ID="meeDatum" runat="server" TargetControlID="txtDatum" Mask="99/99/9999"
                                                            MaskType="Date" InputDirection="LeftToRight">
                                                        </cc1:MaskedEditExtender>
                                                        <cc1:MaskedEditValidator ID="mevDatum" runat="server" ControlToValidate="txtDatum"
                                                            ControlExtender="meeDatum" Display="none" IsValidEmpty="true" Enabled="true"
                                                            EmptyValueMessage="Bitte geben Sie ein gültiges Datum ein" InvalidValueMessage="Bitte geben Sie ein gültiges Datum ein">
                                                                                                                   
                                                        </cc1:MaskedEditValidator>
                                                        <cc1:ValidatorCalloutExtender Enabled="true" ID="vceDatum" Width="350px" runat="server"
                                                            HighlightCssClass="validatorCalloutHighlight" TargetControlID="mevDatum">
                                                        </cc1:ValidatorCalloutExtender>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="width: 15%">
                                            Bezeichnung
                                        </td>
                                        <td class="firstLeft active" style="width: 15%">
                                            Einheit
                                        </td>
                                        <td class="firstLeft active" style="width: 15%">
                                            Menge
                                        </td>
                                        <td class="firstLeft active" style="width: 50%">
                                            Euro-Betrag
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit500" runat="server" Text="500,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein500" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();" TabIndex="2"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblSchein500"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>      
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit200" runat="server" Text="200,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein200"  TabIndex="3" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();">
                                            </asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblSchein200" 
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit100" runat="server" Text="100,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein100" TabIndex="4" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblSchein100"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit50" runat="server" Text="50,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein50" TabIndex="5" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">

                                            <asp:TextBox ID="lblSchein50"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit20" runat="server" Text="20,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein20" TabIndex="7" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblSchein20"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit10" runat="server" Text="10,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein10" TabIndex="8" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">

                                               <asp:TextBox ID="lblSchein10"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold;
                                                text-align:right; 
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Schein
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit5" runat="server" Text="5,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtSchein5" TabIndex="9" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblSchein5"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit2" runat="server" Text="2,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck2" TabIndex="10" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                           <asp:TextBox ID="lblStueck2"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit1" runat="server" Text="1,00"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck1" TabIndex="11" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                                
                                        </td>
                                        <td class="firstLeft active">
                                                                                        <asp:TextBox ID="lblStueck1"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit050" runat="server" Text="0,50"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck050" TabIndex="12" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblStueck050"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit020" runat="server" Text="0,20"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck020" TabIndex="13" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                                                                        <asp:TextBox ID="lblStueck020"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit010" runat="server" Text="0,10"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck010" TabIndex="14" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                              <asp:TextBox ID="lblStueck010"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit005" runat="server" Text="0,05"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck005" TabIndex="15" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblStueck005"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit002" runat="server" Text="0,02"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck002" TabIndex="16" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblStueck002"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            Stück
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblEinheit001" runat="server" Text="0,01"></asp:Label>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="txtStueck001" TabIndex="17" runat="server" MaxLength="5" Width="55px" onKeyPress="return numbersonly(event, false)"
                                                onFocus="Javascript:this.select();"></asp:TextBox>
                                        </td>
                                        <td class="firstLeft active">
                                            <asp:TextBox ID="lblStueck001"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>                                            
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="firstLeft active">
                                            &nbsp;
                                        </td>
                                        <td class="firstLeft active">
                                            Gesamt:
                                        </td>
                                        <td class="firstLeft active">
                                           <asp:TextBox ID="lblGesamt"  
                                                style="font-size:10px; 
                                                font-family:Verdana,Sans-Serif;
                                                font-weight:bold; 
                                                text-align:right;
                                                border:none" runat="server" Width="65px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="dataFooter">
                        <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                            CssClass="Tablebutton" TabIndex="27"></asp:LinkButton>&nbsp;<asp:LinkButton ID="lbNachdruck"
                                Text="Nachdruck" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"
                                Visible="false" TabIndex="27"></asp:LinkButton></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
