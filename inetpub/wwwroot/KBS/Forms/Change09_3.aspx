<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09_3.aspx.vb" Inherits="KBS.Change09_3" MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%;">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label">Inventur</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery"> 

                                <table cellspacing="0"  cellpadding="0" bgcolor="white" border="0">
                                    <tr class="formquery">
                                        <td class="firstLeft active">
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td class="firstLeft active" style="font-weight:normal; font-size:11pt" align="left">
                                            
                                                Sie haben alle Artikel erfasst und wollen die Inventur abschließen. Wenn Sie 
                                                jetzt den Button &#8222;Abschließen&#8220; klicken, wird<br /> ein Testdruck erzeugt. Diesen 
                                                drucken Sie sich bitte 1 x aus und kontrollieren diesen auf Vollständigkeit. 
                                                Sollten Sie <br />Artikel vergessen oder sich vertippt haben, können Sie oben links 
                                                auf &#8222;zurück&#8220; klicken und Sie gelangen wieder in die <br /> Eingabemaske. Dort können 
                                                Sie Ihre Eingaben entsprechend korrigieren. Nach der Änderung klicken Sie wieder
                                                auf <br />&#8222;Inventur abschließen&#8220; und Sie gelangen erneut  in dieses Fenster. Nach einer 
                                                Änderung wird nochmals ein Testdruck  <br />erzeugt, den Sie ebenfalls ausdrucken und 
                                                überprüfen. Sollte die Inventur nach der Kontrolle vollständig sein, klicken Sie<br /> 
                                                erneut auf &#8222;Abschließen&#8220;. Nachdem Sie in dem dann erscheinenden Fenster 
                                                bestätigt haben, dass Sie die Inventur <br />wirklich abschließen wollen, wird das  
                                                richtige Inventurformular (ohne Aufdruck &#8222;Testdruck&#8220;)erzeugt. 
                                                <span style="font-weight:bold">Bitte drucken Sie  <br />dieses in zweifacher 
                                                Ausfertigung aus und unterschreiben es mit allen an der Inventur teilgenommenen 
                                                 <br />Mitarbeitern/innen!</span>Ein Exemplar ist  
                                                für Ihre Unterlagen und ein Exemplar schicken Sie nach Ahrensburg.
                                           
                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" 
                                Text="Abschließen" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                </div>
                        <asp:Panel ID="PLCheck" runat="server" Width="350px" style="display:none">
                            <table cellspacing="0" id="tblCheck" runat="server" width="100%" bgcolor="#FFFFFF"
                                cellpadding="0" border="0" style="border: solid 1px #000000" > 
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                              
                               <tr>
                                    <td align="center" class="firstLeft active" >
                                       Wollen Sie die Inventur wirklich abschließen?
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbOk" Text="Abschließen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbCancel" Text="Abbrechen" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="PLCheck" TargetControlID="MPEDummy" CancelControlID="lbCancel">
                        </cc1:ModalPopupExtender>
                   </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
