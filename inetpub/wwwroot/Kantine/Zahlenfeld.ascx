<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Zahlenfeld.ascx.cs"
    Inherits="Kantine.Zahlenfeld" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="Styles/Rechner.css" media="screen, projection" type="text/css"
        rel="stylesheet" />

<div runat="server" ID="Rechner" Visible="false" class="RechnerBackground Box">
    <asp:UpdatePanel ID="UPRechner" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click"/>
            <asp:AsyncPostBackTrigger ControlID="btnDelete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPlus" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnMinus" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSeven" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnEight" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnNine" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnFour" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnFive" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSix" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnThree" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnTwo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnOne" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnPoint" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnZero" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:TextBox ID="txtZahlenfeld" runat="server" Wrap="false" ReadOnly="true" CssClass="RechnerTextbox" style="height:30px;"></asp:TextBox>
            <table id="tblRechner" runat="server" visible="false" class="RechnerTabelle">
                <tr>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnClear" runat="server" Text="C" CssClass="ButtonRechner"
                            OnClick="btnClear_Click" />
                    </td>
                    <td colspan="2" width="50%" height="20%">
                        <asp:Button ID="btnDelete" runat="server" Text="&larr;" CssClass="ButtonRechner"
                            OnClick="btnDelete_Click" />
                    </td>
                    <td rowspan="2" width="25%" height="40%">
                        <asp:Button ID="btnPlus" runat="server" Text="+" OnClick="btnPlus_Click" CssClass="ButtonRechner"/>
                    </td>
                </tr>
                <tr>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnSeven" runat="server" Text="7" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnEight" runat="server" Text="8" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnNine" runat="server" Text="9" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                </tr>
                <tr>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnFour" runat="server" Text="4" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnFive" runat="server" Text="5" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnSix" runat="server" Text="6" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td rowspan="2" width="25%" height="40%">
                        <asp:Button ID="btnMinus" runat="server" Text="-" OnClick="btnMinus_Click" CssClass=" ButtonRechner"/>
                    </td>
                </tr>
                <tr>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnOne" runat="server" Text="1" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnTwo" runat="server" Text="2" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnThree" runat="server" Text="3" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                </tr>
                <tr>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnPoint" runat="server" Text="," OnClick="btnPoint_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="25%" height="20%">
                        <asp:Button ID="btnZero" runat="server" Text="0" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td height="25%" width="20%">
                        <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click" Text="Abbr." CssClass=" ButtonCancel" />
                    </td>
                    <td height="25%" width="20%">
                        <asp:Button ID="btnEnter" runat="server" Text="OK" OnClick="btnEnter_Click" CssClass=" ButtonOK" />
                    </td>
                </tr>
            </table>
            <table id="tblZahlen" runat="server" visible="false" class="RechnerTabelle">
                <tr>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button1" runat="server" Text="C" CssClass=" ButtonRechner"
                            OnClick="btnClear_Click" />
                    </td>
                    <td colspan="2" width="66%" height="20%">
                        <asp:Button ID="Button2" runat="server" Text="&larr;" CssClass=" ButtonRechner"
                            OnClick="btnDelete_Click"/>
                    </td>
                </tr>
                <tr>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button4" runat="server" Text="7" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button5" runat="server" Text="8" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button6" runat="server" Text="9" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                </tr>
                <tr>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button7" runat="server" Text="4" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button8" runat="server" Text="5" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button9" runat="server" Text="6" OnClick="btnNumber_Click" CssClass=" ButtonRechner" />
                    </td>
                </tr>
                <tr>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button11" runat="server" Text="1" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button12" runat="server" Text="2" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button13" runat="server" Text="3" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                </tr>
                <tr>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button16" runat="server" OnClick="btnClose_Click" Text="Abbr." CssClass="ButtonCancel" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button15" runat="server" Text="0" OnClick="btnNumber_Click" CssClass="ButtonRechner" />
                    </td>
                    <td width="33%" height="20%">
                        <asp:Button ID="Button17" runat="server" Text="OK" OnClick="btnEnter_Click" CssClass="ButtonOK" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>    
</div>