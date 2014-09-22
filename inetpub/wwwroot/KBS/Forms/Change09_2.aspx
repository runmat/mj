<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Change09_2.aspx.vb" Inherits="KBS.Change09_2"  MasterPageFile="~/KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript"  type="text/javascript" src="../Java/JScript.js"></script>
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
                                       Bitte scannen Sie den zu erfassenden Artikel, geben Sie die Menge ein und drücken Sie hinzufügen.
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="TableQuery">
                        <asp:UpdatePanel runat="server" ID="upEingabe">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnInsert">
                                    <table cellpadding="0" cellspacing="0">
                                        <tfoot>
                                            <tr>
                                                <td colspan="5">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tfoot>
                                        <tbody>
                                            <tr class="formquery">
                                                <td colspan="5" class="firstLeft active">
                                                    <asp:Label ID="lblError" CssClass="TextError" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" colspan="5" style="font-size: 12px">
                                                    <asp:Label ID="lblProdH" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblEANAnzeige" Text="EAN" runat="server"></asp:Label>
                                                </td>
                                                <td class="active" style="padding-right: 100px">
                                                    <asp:Label ID="lblArtikelbezeichnungAnzeige" Text="Artikelbezeichnung" runat="server"></asp:Label>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblMengeAnzeige" Text="Menge" runat="server"></asp:Label>
                                                </td>
                                                <td width="100%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td colspan="5" class="firstLeft active">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtEAN" TabIndex="1" onFocus="Javascript:this.select();" onKeyUp="Javascript:setFocusAfterInput(this);"
                                                        MaxLength="15" AutoPostBack="true" runat="server"></asp:TextBox>
                                                    <asp:TextBox Width="1" ID="txtMaterialnummer" Visible="false" Text="" runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Label ID="lblArtikelbezeichnung" Text="(wird automatisch ausgefüllt)" runat="server"></asp:Label>
                                                </td>
                                                <td class="firstLeft active">
                                                    <asp:TextBox ID="txtMenge" MaxLength="3" onFocus="Javascript:this.select();" Width="50px"
                                                        Text="" runat="server"></asp:TextBox>
                                                    <asp:RangeValidator Enabled="true" SetFocusOnError="true" ControlToValidate="txtMenge"
                                                        ID="rvMenge" MinimumValue="1" MaximumValue="999" runat="server" Display="None"
                                                        ErrorMessage="Menge 1-999"></asp:RangeValidator><cc1:ValidatorCalloutExtender Enabled="True"
                                                            ID="vceMenge" Width="350px" runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                            TargetControlID="rvMenge">
                                                        </cc1:ValidatorCalloutExtender>
                                                    <cc1:FilteredTextBoxExtender Enabled="true" ID="fteMenge" runat="server" TargetControlID="txtMenge"
                                                        FilterType="Numbers">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                                <td nowrap="nowrap">
                                                    <asp:LinkButton ID="lbtnInsert" Text="hinzufügen" Height="16px" Width="78px" runat="server"
                                                        CssClass="Tablebutton"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </asp:Panel>
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
                                                AllowPaging="False" AllowSorting="True" ShowFooter="False" GridLines="None" PageSize="999999">
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
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left"  HeaderStyle-Width="25%" HeaderText="Artikel" DataField="MAKTX" />
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25%" HeaderText="EAN" DataField="EAN11" />
                                                    <asp:BoundField HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="25%" HeaderText="Menge" DataField="ERFMG" />
                                                    <asp:TemplateField HeaderText="Menge" HeaderStyle-HorizontalAlign="left" HeaderStyle-Width="25%" >
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'
                                                                CommandName="minusMenge" ID="imgbMinus" ImageUrl="~/Images/Minus.jpg" Width="15"
                                                                Height="15" runat="server" />
                                                            &nbsp;
                                                            <asp:TextBox  MaxLength="3" runat="server" onKeyPress="return numbersonly(event, false)"
                                                                Width="50px" onFocus="Javascript:this.select();" ID="txtMenge" Text='<%# DataBinder.Eval(Container, "DataItem.ERFMG") %>'></asp:TextBox>
                                                            &nbsp;<asp:ImageButton CommandArgument='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'
                                                                CommandName="plusMenge" ID="imgbPlus" Width="15" Height="15" ImageUrl="~/Images/Plus.jpg"
                                                                runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                    
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="lbDelete" runat="server" Width="32" Height="32" ImageUrl="~/Images/RecycleBin.png"
                                                                CommandArgument='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>' CommandName="entfernen" />
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
                            <asp:LinkButton ID="lbOffeneArtikel" 
                                Text="Offene Artikel" Height="16px" Width="155px" runat="server" 
                                CssClass="TablebuttonXLarge"></asp:LinkButton>
                            <asp:LinkButton ID="lbohneArtikel" 
                                Text="ohne Artikel Abschließen" Height="16px" Width="155px" runat="server" 
                                CssClass="TablebuttonXLarge" Visible="False"></asp:LinkButton>                        
                            <asp:LinkButton ID="lbAbsenden" 
                                Text="Speichern" Height="16px" Width="78px" runat="server" 
                                CssClass="Tablebutton"></asp:LinkButton>
                        </div>
                        <asp:Button ID="MPEDummy" style="display: none" runat="server" />
                         <cc1:ModalPopupExtender runat="server" ID="mpeBestellungsCheck" BackgroundCssClass="divProgress"
                            Enabled="true" PopupControlID="BestellungsCheck" TargetControlID="MPEDummy" 
                            BehaviorID="BestellCheck">
                        </cc1:ModalPopupExtender>                                
                        <asp:Panel ID="BestellungsCheck" runat="server"  style="overflow:auto;height:425px;width:600px;display:none">
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
                                        Für folgende Artikel wurden keine Mengen erfasst.<br /><%--<b>Sollen diese Artikel mit Nullzählung gespeichert werden?</b>--%></td>
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
                                                            <asp:Label ID="lblMatnr" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MATNR") %>'></asp:Label>   
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                
                                                    <asp:BoundField DataField="MAKTX" HeaderText="Artikel" />
                                                    <asp:BoundField DataField="EAN11"  HeaderText="EAN" />
                                                    <asp:BoundField DataField="ERFMG" HeaderText="Menge"   />
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
                                        <asp:LinkButton ID="lbBestellungOk" Text="Schließen" Height="16px" Width="78px" runat="server"
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
                   </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
