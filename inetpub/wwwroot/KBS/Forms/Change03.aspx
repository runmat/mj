<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change03.aspx.vb" Inherits="KBS.Change03"
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
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung Platinen und Zubehör</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tfoot>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            <tbody>
                                <tr class="formquery">
                                    <td colspan="2" class="firstLeft active">
                                        <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td colspan="2" class="firstLeft active">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery">
                                    <td class="firstLeft active" style="white-space:nowrap;">
                                        <asp:Label ID="lblKst" runat="server" Width="80px">Kostenstelle:</asp:Label> 
                                        <asp:TextBox ID="txtKST" runat="server" Enabled="false"  AutoPostBack="true" Width="100px"></asp:TextBox>                                               
                                            <asp:Label ID="lblKSTText" runat="server" TabIndex="0" Visible="false" style="margin-left: 10px; margin-top: 5px;"></asp:Label>                                              
                                    </td>                                           
                                </tr>
                                <tr class="formquery" >
                                    <td class="firstLeft active" style="padding-bottom:5px;">
                                        <table  cellpadding="0" cellspacing="0"  style="border-width: 0px; border-style: none;">
                                            <tr>
                                                <td Width="80px">
                                                    <asp:Label ID="lblLieferant" runat="server" Width="80px">Lieferant:</asp:Label>
                                                </td>
                                                <td class="active">
                                                    <asp:DropDownList ID="ddlLieferant" Style="width: auto" runat="server" AutoPostBack="True">
                                                    </asp:DropDownList>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label runat="server" visible="true">bereits geliefert:</asp:Label> 
                                                </td>
                                                <td class="active">
                                                    <asp:CheckBox ID="chkGeliefert" runat="server" Style="border-style: none;" AutoPostBack="true" visible="true" Checked="false"/>
                                                </td>
                                            </tr>
                                            <tr id="trLieferscheinnummer" runat="server" visible="false" class="formquery">
                                                <td colspan="2">
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    Lieferscheinnummer:
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtLieferscheinnummer" runat="server" MaxLength="10" Width="100px"></asp:TextBox>
                                                </td>
                                            </tr>                                                   
                                        </table>                                                
                                    </td>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr class="formquery" style="background-color:#dfdfdf; padding-bottom:5px;">
                                    <td class="firstLeft active">
                                        <asp:ImageButton id="ibtnOptionalShow" runat="server" ImageUrl="../Images/queryArrow.gif" Width="17px" Height="16px"/> &nbsp;
                                        <asp:Label ID="lblOptional" runat="server" style="vertical-align:middle;">Optional</asp:Label>
                                    </td>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="border-width: 0px; border-style: none;">
                                            <tr class="formquery" id="trLiefertermin" runat="server" visible="false">
                                                <td class="firstLeft active">
                                                    gewünschter <br />
                                                    Liefertermin:                                                           
                                                </td>
                                                <td class="active">
                                                    <asp:TextBox ID="txtLieferdatum" runat="server" Width="70px"></asp:TextBox>
                                                    <cc1:CalendarExtender ID="ceLieferdatum" runat="server" TargetControlID="txtLieferdatum"></cc1:CalendarExtender>
                                                    <cc1:MaskedEditExtender ID="meeLieferdatum" runat="server" TargetControlID="txtLieferdatum" MaskType="Date" Mask="99/99/9999"></cc1:MaskedEditExtender>
                                                </td>
                                                <td style="width: 100%">
                                                    &nbsp;
                                                </td>                                                    
                                            </tr>
                                        </table>
                                </tr>                                       
                                <tr class="formquery">
                                    <td colspan="2" class="firstLeft active">
                                        <asp:Label ID="lblMessage" CssClass="TextError" runat="server"></asp:Label>&nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="data">
                            <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tabContainer" runat="server">
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView CssClass="GridView" ID="GridView1" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Topseller" DataField="ARTBEZ" HeaderStyle-Width="315px" />

                                                <asp:TemplateField HeaderText="Bild" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" Visible='<%# proofFileExist(DataBinder.Eval(Container, "DataItem.ImageUrl").toString)  %>' ImageUrl='<%# DataBinder.Eval(Container, "DataItem.ImageUrl") %>' Height="28px" Width="100px" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stück">
                                                    <ItemTemplate>
                                                        <asp:TextBox MaxLength="6" runat="server" Width="50px" onChange="Javascript:onChangeSetHiddenField();"
                                                            onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# DataBinder.Eval(Container, "DataItem.MENGE") %>'
                                                            OnTextChanged="txtMenge_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                                <asp:TemplateField HeaderText="Beschreibung">
                                                    <ItemTemplate>
                                                        <asp:TextBox MaxLength="30" runat="server" Width="185px" onFocus="Javascript:this.select();"
                                                            Visible='<%# Not DataBinder.Eval(Container, "DataItem.ZUSINFO")= "" %>' ID="txtBeschreibung"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Beschreibung") %>'></asp:TextBox>
                                                        <asp:Label ID="Label1" runat="server" Font-Size="9px" Text="*Plichtfeld, max. 30 Zeichen"
                                                            Visible='<%# Not DataBinder.Eval(Container, "DataItem.ZUSINFO")= "" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView CssClass="GridView" ID="GridView3" runat="server" Width="100%" AutoGenerateColumns="False"
                                            AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None">
                                            <PagerSettings Visible="false" />
                                            <HeaderStyle CssClass="GridTableHead" HorizontalAlign="Left"></HeaderStyle>
                                            <AlternatingRowStyle CssClass="GridTableAlternate" />
                                            <RowStyle CssClass="ItemStyle" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Artikelbezeichnung" DataField="ARTBEZ" HeaderStyle-Width="315px" />
                                                <asp:TemplateField HeaderText="Bild" Visible="false">
                                                    <ItemTemplate>
                                                        <a class="tip" style="text-decoration: none; color: #595959;" href="#">
                                                            <asp:Image ID="Image2" runat="server" Visible='<%# proofFileExist(DataBinder.Eval(Container, "DataItem.ImageUrl").toString)  %>' ImageUrl="~/Images/Lupe_01.gif" Height="18px" Width="16px" />
                                                                    <span style="width: 215px; background-color: WhiteSmoke;
                                                                height: auto">
                                                                <div style="border: none; height: auto; width: 200px;">
                                                                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# DataBinder.Eval(Container, "DataItem.ImageUrl") %>' Height="56px" Width="200px" />
                                                                </div>
                                                            </span></a>
                                                    </ItemTemplate>
                                                    <HeaderStyle  Width = "80px"  />
                                                    <ItemStyle  Width = "80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stück">
                                                    <ItemTemplate>
                                                        <asp:TextBox onChange="Javascript:onChangeSetHiddenField();" MaxLength="6" runat="server"
                                                            Width="50px" onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# DataBinder.Eval(Container, "DataItem.Menge") %>'
                                                            OnTextChanged="txtMenge_TextChanged"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Verpackungseinheit" DataField="VMEINS" />
                                                <asp:TemplateField HeaderText="Beschreibung">
                                                    <ItemTemplate>
                                                        <asp:TextBox MaxLength="30" runat="server" Width="185px" onFocus="Javascript:this.select();"
                                                            Visible='<%# Not DataBinder.Eval(Container, "DataItem.ZUSINFO")= "" %>' ID="txtBeschreibung"
                                                            Text='<%# DataBinder.Eval(Container, "DataItem.Beschreibung") %>'></asp:TextBox>
                                                        <asp:Label ID="Label1" runat="server" Font-Size="9px" Text="*Plichtfeld, max. 30 Zeichen"
                                                            Visible='<%# Not DataBinder.Eval(Container, "DataItem.ZUSINFO")= "" %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <input id="Hidden1" type="hidden" runat="server" />
                            <script type="text/javascript" language="javascript">
                                function onChangeSetHiddenField() {
                                    var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_Hidden1");
                                    hiddenInput.value = 1;
                                }
                                function onSetFocusField(e, control1, control2) {
                                    var e = e ? e : window.event;
                                    var KeyCode = e.which ? e.which : e.keyCode;


                                    if (KeyCode == 40)
                                        control1.focus();
                                    else if (KeyCode == 38)
                                        control2.focus();
                                }
                                function onSetFocusFirstField(control1) {
                                    control1.focus();
                                }                                                                            
                            </script>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" Text="Absenden" Height="16px" Width="78px" runat="server"
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" />
                        <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>
                        <cc1:ModalPopupExtender runat="server" ID="MPE_ChangeLieferant" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="plChangeLief" TargetControlID="MPEDummy2">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellungsCheck" runat="server" Style="overflow: auto; height: 425px;
                            width: 600px; display: none">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white" cellpadding="0"
                                style="overflow: auto; height: 425px; width: 583px; border: solid 1px #646464">
                                <tbody>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="lblBedienError" runat="server" Text="Einscannen der Bedienerkarte!"
                                                CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblStatus" Visible="false" runat="server">Bestellstatus</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" TextMode="Password"></asp:TextBox>
                                            <asp:Label ID="lblBestellMeldung" Visible="false" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr id="trInfo" runat="server">
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="Info" runat="server">Bitte überprüfen Sie Ihre Bestellung, ungewöhnliche Werte sind Rot markiert!<br />
                                        Bitte korrigieren Sie gegebenenfalls!<br /></asp:Label>
                                        </td>
                                    </tr>
                                    <tr id="trGridview">
                                        <td>
                                            <asp:GridView ID="GridView2" runat="server" AllowPaging="False" AllowSorting="True"
                                                AutoGenerateColumns="False" BackColor="White" CssClass="GridView" GridLines="None"
                                                HorizontalAlign="Center" ShowFooter="False" Style="overflow: auto; width: 75%">
                                                <PagerSettings Visible="false" />
                                                <HeaderStyle CssClass="GridTableHead" />
                                                <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                <RowStyle CssClass="ItemStyle" />
                                                <Columns>
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ARTLIF") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ARTBEZ" HeaderText="Artikelbezeichnung" />
                                                    <asp:BoundField DataField="Menge" HeaderText="Stück" />
                                                    <asp:BoundField DataField="Beschreibung" HeaderText="Beschreibung" />
                                                    <asp:TemplateField Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:LinkButton ID="lbBestellFinalize" Visible="false" Text="OK" Height="16px" Width="78px"
                                                runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                            <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                                runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                            &nbsp; &nbsp;
                                            <asp:LinkButton ID="lbBestellungOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                                CssClass="Tablebutton"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input id="SendTopSap" type="hidden" runat="server" />&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <script type="text/javascript" language="javascript">
                                function ControlField(control1) {

                                    if (control1.value.length == 15)
                                        if (control1.value.substring(control1.value.length - 1) == '}') {
                                        theForm.__EVENTTARGET.value = '__Page';
                                        theForm.__EVENTARGUMENT.value = 'MyCustomArgument';
                                        theForm.submit();
                                        var hiddenInput = document.getElementById("ctl00_ContentPlaceHolder1_SendTopSap");
                                        hiddenInput.value = 1;
                                    }
                                    else {
                                        control1.focus();
                                    }
                                    else
                                        control1.focus();

                                }
                            </script>
                        </asp:Panel>
                        <asp:Panel ID="plChangeLief" HorizontalAlign="Center" runat="server" Style="display: none">
                            <table cellspacing="0" id="Table2" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width: 50%; border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <span>Beim Wechsel des Lieferanten werden Ihre Eingaben verloren gehen! Jetzt wechseln!</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbtnOK" Text="OK" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnCancel" Text="Abbrechen" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
