<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Preisanlage_3.aspx.cs" Inherits="AppZulassungsdienst.forms.Preisanlage_3"  MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
<script language="javascript" type="text/javascript">

//Cursor bewegen wie in einem Excel-Sheet.
function keyPressed(TB , e) {
    var tblGrid = document.getElementById("ctl00_ContentPlaceHolder1_GridView1");

        var rowcount = tblGrid.rows.length;
        var TBID = document.getElementById(TB);
        var key;

        
        if (window.event) {
            key = window.event.keyCode;
        }
        else if (e) {
            key = e.which;
        }
        else {
            return true;
        }

        if (key == 37 || key == 38 ||
         key == 39 || key == 40) {
            for (Index = 0; Index < rowcount; Index++) {
               
                for (childIndex = 0; childIndex <
              tblGrid.rows[Index].cells.length; childIndex++) {
                    
                    if (tblGrid.rows[Index].cells[childIndex].children[0] != null) {
                        if (tblGrid.rows[Index].cells[
                 childIndex].children[0].id == TBID.id) {
                            if (key == 40) {
                                if (Index + 1 > rowcount) { return false; }
                                if (tblGrid.rows[Index + 1].cells[
                     childIndex].children[0] != null) {
                                    if (tblGrid.rows[Index + 1].cells[
                       childIndex].children[0].type == 'text') {

                                        //downvalue

                                        tblGrid.rows[Index + 1].cells[
                             childIndex].children[0].focus();
                                        return false;
                                    }
                                }
                            }
                            if (key == 38) {
                                if (Index - 1 < 0) { return false; }
                                if (tblGrid.rows[Index - 1].cells[
                     childIndex].children[0] != null) {
                                    if (tblGrid.rows[Index - 1].cells[
                       childIndex].children[0].type == 'text') {
                                        //upvalue

                                        tblGrid.rows[Index - 1].cells[
                             childIndex].children[0].focus();
                                        return false;
                                    }
                                }
                            }

                            if (key == 37 && (childIndex != 0)) {

                                if ((tblGrid.rows[Index].cells[
                      childIndex - 1].children[0]) != null) {

                                    if (tblGrid.rows[Index].cells[
                       childIndex - 1].children[0].type == 'text') {
                                        //left

                                        if (tblGrid.rows[Index].cells[
                         childIndex - 1].children[0].value != '') {
                                            var cPos =
                           getCaretPos(tblGrid.rows[Index].cells[
                                       childIndex - 1].children[0], 'left');
                                            if (cPos) {
                                                tblGrid.rows[Index].cells[
                                 childIndex - 1].children[0].focus();
                                                return false;
                                            }
                                            else {
                                                return false;
                                            }
                                        }
                                        tblGrid.rows[Index].cells[childIndex - 1].children[0].focus();
                                        return false;
                                    }
                                }
                            }

                            if (key == 39) {
                                if (tblGrid.rows[Index].cells[childIndex + 1].children[0] != null) {
                                    if (tblGrid.rows[Index].cells[
                       childIndex + 1].children[0].type == 'text') {
                                        //right

                                        if (tblGrid.rows[Index].cells[
                         childIndex + 1].children[0].value != '') {
                                            var cPosR =
                           getCaretPos(tblGrid.rows[Index].cells[
                                       childIndex + 1].children[0], 'right');
                                            if (cPosR) {
                                                tblGrid.rows[Index].cells[
                                 childIndex + 1].children[0].focus();
                                                return false;
                                            }
                                            else {
                                                return false;
                                            }
                                        }
                                        tblGrid.rows[Index].cells[childIndex + 1].children[0].focus();
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
    }

    function getCaretPos(control, way) {
        var movement;
        if (way == 'left') {
            movement = -1;
        }
        else {
            movement = 1;
        }
        if (control.createTextRange) {
            control.caretPos = document.selection.createRange().duplicate();
            if (control.caretPos.move("character", movement) != '') {
                return false;
            }
            else {
                return true;
            }
        }
    }
</script>

    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>

                            <div id="TableQuery" style="margin-bottom: 10px">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <td 
                                                    style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                                </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active" >
                                                    <asp:Label ID="lblError" runat="server" ></asp:Label>
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#269700"></asp:Label>
                                                   </td>
                                            </tr>
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    <asp:Label ID="lblKunnr" runat="server" Font-Size="10pt" Font-Bold="True" ></asp:Label>
                                                    &nbsp;
                                                    <asp:Label ID="lblKunnname" style="padding-left:15px;" runat="server" Font-Size="10pt" Font-Bold="True" ></asp:Label>
                                                </td>
                                            </tr>                                            
                                            <tr class="formquery">
                                                <td class="firstLeft active">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="GridView1" CellPadding="0" ShowHeader = "True" Width="50%" runat="server" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblMatnr" runat="server" Text="Material"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDienstNr" runat="server" Text='<%# Eval("Matnr") %>' Font-Bold="False"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Width="50px" BorderColor="#dfdfdf" BorderStyle="Solid" BorderWidth="1px"
                                                                    HorizontalAlign="Left" />
                                                                <ItemStyle BorderColor="#dfdfdf" BorderStyle="Solid" BorderWidth="1px" Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="Label1" runat="server" Text="Bezeichnung"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label Width="250px" ID="lblDienst" runat="server" Text='<%# Eval("Maktx") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle BorderColor="#dfdfdf" BorderStyle="Solid" BorderWidth="1px" />
                                                                <HeaderStyle BorderColor="#dfdfdf" BorderStyle="Solid" BorderWidth="1px" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                     <asp:Label ID="lblLabel1" runat="server" Text="Preis"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtInput1" onkeypress="return numbersonly(event, true)" onkeyup="keyPressed(this.id, event);"
                                                                        MaxLength="8" runat="server" Width="60px"  Text='<%# Eval("Stva1") %>' ></asp:TextBox>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="ExcelSheetGreen"  />
                                                                <HeaderStyle CssClass="ExcelSheetGreen" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                           </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                    </div>
                                </asp:Panel>
                            </div>
                            <div id="dataQueryFooter" runat="server" class="dataQueryFooter">
                                <asp:LinkButton ID="cmdCreate" runat="server" CssClass="TablebuttonXLarge" Width="155px"
                                    Height="22px" Style="padding-right: 5px" onclick="cmdCreate_Click">» Absenden </asp:LinkButton>
                            </div>

                    <div id="dataFooter">
                        &nbsp;</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
