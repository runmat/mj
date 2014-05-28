<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change05.aspx.vb" Inherits="KBS.Change05" MasterPageFile="~/KBS.Master" %>

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
                            <asp:Label ID="lblHead" runat="server" Text="Label">Bestellung Versicherungen</asp:Label>
                            <asp:Label ID="lblPageTitle" runat="server"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="firstLeft active">
                                        Bitte geben Sie hier den gewünschten Artikel + Menge + Preis ein und drücken Sie hinzufügen.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="0">                                   
                                    <tbody>
                                     <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active" width="100%">
                                                <asp:Label ID="lblError" CssClass="TextError" style="white-space:normal;" runat="server"></asp:Label> 
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td  class="firstLeft active">
                                                 Versicherungstyp:
                                            </td>
                          
                                            <td class="firstLeft active">
                                               Menge
                                            </td>
                                            <td colspan="2" class="firstLeft active" width="100%">
                                               Preis
                                            </td>
                                        </tr>                                        
                                        <tr class="formquery">
                                            <td class="firstLeft active">
                                                <asp:DropDownList ID="ddlArtikel" AutoPostBack="true"  runat="server" style="width:auto;" >
                                                </asp:DropDownList>
                                            </td>
                                            <td class="firstLeft active">
                                                <asp:TextBox ID="txtMenge" width="50px" runat="server"></asp:TextBox>
                                                <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                                    FilterType="Numbers">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td class="firstLeft active" style="width:75px">
                                                <asp:TextBox ID="txtPreis" width="75px" runat="server" ></asp:TextBox>
                                                 <cc1:FilteredTextBoxExtender Enabled="true" ID="ftePreis" runat="server" TargetControlID="txtPreis"
                                                    FilterType="Custom" ValidChars="0123456789,">
                                                </cc1:FilteredTextBoxExtender>
                                            <td class="firstLeft active" width="100%">
                                                <asp:LinkButton ID="lbtnInsert" Height="16px" Width="78px" 
                                                    CssClass="Tablebutton" runat="server" style="margin-left: 0px">hinzufügen</asp:LinkButton>
                                            </td>                                                                                     
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active" width="100%">
                                                <asp:Label ID="lblMessage" CssClass="TextError" runat= "server"></asp:Label>
                                             </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td colspan="4" class="firstLeft active" width="100%">
                                               &nbsp;
                                            </td>
                                        </tr>                                        
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="data"> 
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblNoData" runat="server" Visible="False" Font-Bold="True"></asp:Label>
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
                                                            <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'></asp:Label>   
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Versicherung" DataField="MAKTX" />
                                                    <asp:BoundField DataFormatString="{0:C}" HeaderText="Preis" DataField="VKP" />
                                                    <asp:BoundField HeaderText="Menge" DataField="Menge" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.KEY") %>' CommandName="entfernen" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            <asp:LinkButton ID="lbAbsenden" 
                                Text="Absenden" Height="16px" Width="78px" runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                </div>
                        <asp:Button ID="MPEDummy" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy1" Width="0" Height="0" runat="server" />
                        <asp:Button ID="MPEDummy2" Width="0" Height="0" runat="server" />
                         <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" 
                            BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>                                
                        <asp:Panel ID="BestellungsCheck" runat="server" style="overflow:auto;height:425px;width:600px;display:none;">
                            <table cellspacing="0" id="tblBestellungscheck" runat="server" bgcolor="white"
                                cellpadding="0" style="overflow:auto;height:425px;width:583px;border: solid 1px #646464" >
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                       
                                <tr>
                                    <td align="center" class="firstLeft active">
                                        Bitte überprüfen Sie Ihre Bestellung. Bitte korrigieren Sie gegebenenfalls!<br />
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                   
                                <tr>
                                    <td> 
                                            <asp:GridView ID="GridView2" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                                    CssClass="GridView" GridLines="None" HorizontalAlign="Center" 
                                                    ShowFooter="False" Width="75%">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblMatnr" runat="server" 
                                                                    Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="MAKTX" HeaderText="Versicherung" />
                                                        <asp:BoundField DataField="VKP" HeaderText="Preis"  DataFormatString="{0:C}"  />
                                                        <asp:BoundField DataField="Menge" HeaderText="Menge" />
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
                                          <asp:LinkButton ID="lbBestellungKorrektur" Text="Korrektur" Height="16px" Width="78px"
                                            runat="server" CssClass="Tablebutton"></asp:LinkButton>
                                        &nbsp; &nbsp;
                                        <asp:LinkButton ID="lbBestellungOk" Text="Weiter" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton" ></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                
                            </table>
                        </asp:Panel>

                       <cc1:ModalPopupExtender runat="server" ID="MPEBestellResultat" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellResultat" TargetControlID="MPEDummy1">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="BestellResultat" HorizontalAlign="Center" runat="server" style="display:none">
                            <table cellspacing="0" id="Table1" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width:50%; border: solid 1px #646464">
                               <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        Bestellstatus:
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <asp:Label ID="lblBestellMeldung" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td> 
                                            <asp:GridView ID="GridView3" runat="server" AllowPaging="False" 
                                                    AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
                                                    CssClass="GridView" GridLines="None" HorizontalAlign="Center" 
                                                    ShowFooter="False" Width="75%">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead" />
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                        <asp:BoundField ItemStyle-HorizontalAlign="Left" DataField="BSTNR" HeaderText="Bestellnr." />
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
                                    <td>
                                        <asp:LinkButton ID="lbBestellFinalize" Text="ok" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr >
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>                                   
                            </table>
                        </asp:Panel>
                        <cc1:ModalPopupExtender runat="server" ID="MPE_plMenge" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="plMenge" TargetControlID="MPEDummy2" >
                        </cc1:ModalPopupExtender>  
                        <asp:Panel ID="plMenge" HorizontalAlign="Center" runat="server" style="display:none">
                            <table cellspacing="0" id="Table2" runat="server" width="50%" bgcolor="white" cellpadding="0"
                                style="width:50%;border: solid 1px #646464">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="firstLeft active">
                                        <span >Soll diese Menge(größer 50 St.) wirklich bestellt werden?</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lbtnOK" Text="OK" Height="16px" Width="78px" runat="server"
                                            CssClass="Tablebutton"></asp:LinkButton>
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
