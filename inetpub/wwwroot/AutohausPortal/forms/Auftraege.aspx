<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Auftraege.aspx.cs" Inherits="AutohausPortal.forms.Auftraege"
    MasterPageFile="/AutohausPortal/MasterPage/Form.Master" %>
<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/Form.Master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript">
        $(document).ready(function () {
            $("#dialog").dialog({
                bgiframe: true,
                autoOpen: false,
                height: 150,
                modal: true,
                draggable: false,
                resizable: false,
                buttons: {
                    Abbrechen: function () {
                        $(this).dialog('close');
                    },
                    Absenden: function () {
                        $(this).dialog('close');
                        __doPostBack('ctl00$ContentPlaceHolder1$cmdSave', '');

                    }
                }
            });
            $('#ctl00_ContentPlaceHolder1_cmdSave').click(function () {
                $('#dialog').dialog('open');
            });
            $("#divEditPos").dialog({
                autoOpen: false,
                bgiframe: true,
                modal: true,
                resizable: false,
                closeOnEscape: false,
                height: 150,
                width: 250,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divEditPosDlgContainer");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();

                }

            });
//            var windowName = 'userConsole';
//            var popUp = window.open('PrintDialog.aspx', windowName, 'width=1000, height=700, left=24, top=24, scrollbars, resizable');
//            if (popUp == null || typeof (popUp) == 'undefined') {
//                alert('Please disable your pop-up blocker and click the "Open" link again.');
            //            }
            //-- start of snippet --


            if (PopupBlocked()) {
                alert('Bitte deaktivieren Sie vor dem Absenden Ihren PopUp-Blocker.');
                // Custom code may replace alert()
            }
            else {
                //alert('Test popup was successful.');
                // Custom code may replace alert()
            }
            //-- end of snippet --

        });
        function closeDialog() {
            //Could cause an infinite loop because of "on close handling"
            $("#divEditPos").dialog('close');
        }
        function openDialog(title) {

            $("#divEditPos").dialog('open');
        }
        function unblockDialog() {
            $("#divEditPos").unblock();
        }
        function openDialogAndBlock(title) {
            openDialog(title);

            //block it to clean out the data
            $("#divEditPos").block({
                message: '<img src="../images/indicator.gif" />',
                css: { border: '0px' },
                fadeIn: 0,
                overlayCSS: { backgroundColor: '#ffffff', opacity: 1 }
            });
        }
        function openWin(title) {
            if (title == "Dialog") { var oWnd = radopen("Dialog.aspx", "RadWindow1"); }
        }

        function OnClientClose(oWnd, args) {

        }
        function PopupBlocked() {
            var PUtest = window.open(null, "", "width=1,height=1");
            try { PUtest.close(); return false; }
            catch (e) { return true; }
        }
        function ChangeCheckState(rowindex) {
            var tbl = document.getElementById("<%=gvZuldienst.ClientID%>");

            var items = tbl.rows[rowindex].getElementsByTagName("input");

            for (var i = 0; i < items.length; i++) {
                if (items[i].type.toLowerCase() == "checkbox") {
                    if (items[i].parentNode.className.indexOf("ez-checked") != -1) {
                        items[i].checked = false;
                        items[i].parentNode.className = items[i].parentNode.className.replace("ez-checked", "");
                    } else {
                        items[i].checked = true;
                        items[i].parentNode.className = items[i].parentNode.className + " ez-checked";
                    }
                }
            }
        }
    </script>
    <asp:ScriptManager ID="Scriptmanager1" runat="server" ScriptMode="Release" EnableScriptGlobalization="true"
        EnableScriptLocalization="true" AsyncPostBackTimeout="36000" EnablePageMethods="True">
    </asp:ScriptManager>
            <div>
                <h4>
                    <asp:Label ID="lblError" runat="server" Style="color: #B54D4D" Text="">
                    </asp:Label>
                    <asp:Label ID="lblMessage" runat="server" Style="color: #269700" Text="">
                    </asp:Label>                 
               </h4>
            </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkCreateExcel" />
        </Triggers>
        <ContentTemplate>
            <div id="divExcelExport" runat="server" style="padding-right: 4px" visible="false">
                <div align="right">
                    <span style="float:right">
                        <asp:LinkButton ID="lnkCreateExcel" runat="server" OnClick="lnkCreateExcel_Click" >Excel herunterladen</asp:LinkButton>
                    </span>
                    <img src="../Images/iconXLS.gif" alt="Excel herunterladen" style="float:right"/>
                </div>
            </div>
            <asp:GridView ID="gvZuldienst" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="0" CellSpacing="0" GridLines="None" AllowSorting="true" AllowPaging="True"  DataKeyNames="ID"
                CssClass="GridView" PageSize="300" OnRowCommand="gvZuldienst_RowCommand" OnSorting="gvZuldienst_Sorting" OnRowDataBound="gvZuldienst_RowDataBound">
                <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                <PagerSettings Visible="False" />
                <RowStyle CssClass="ItemStyle" />
                <Columns>
                    <asp:TemplateField SortExpression="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="id_pos" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblid_pos" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.id_pos") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Status" HeaderText="Status" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="id_sap" HeaderText="ID">
                        <HeaderStyle Width="80px" />
                        <ItemStyle Wrap="false" />
                        <HeaderTemplate>
                        <div class="formselects">
                            <asp:CheckBox ID="chkAuswahlAll" CssClass="tableCheckbox checkall" runat="server" /></div>
                            <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="id_sap">ID</asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate><div class="formselects">
                            <asp:CheckBox ID="chkAuswahl" CssClass="tableCheckbox" runat="server" /></div>
                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.id_sap") %>' style="padding-bottom: 9px" CssClass="HighlightOnHover"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="kundenname" HeaderText="Kundennr.">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_kundenname" runat="server" CommandName="Sort" CommandArgument="kundenname">Kundennr.</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblkundenname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.kundenname") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Matbez" HeaderText="Dienstleistung">
                        <HeaderTemplate>
                            <asp:LinkButton ID="colMatbez" runat="server" CommandName="Sort" CommandArgument="Matbez">Dienstleistung</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMatbez" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Matbez") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Zulassungsdatum" HeaderText="Zulassungsdatum">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandName="Sort" CommandArgument="Zulassungsdatum">Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblZulassungsdatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Zulassungsdatum", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Referenz1" HeaderText="Referenz1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="Referenz1">Referenz1</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Referenz2" HeaderText="Referenz2">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="Referenz2">Referenz2</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Referenz2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Kennzeichen" HeaderText="Kennzeichen">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="Kennzeichen">Kennzeichen</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblKennzeichen" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Kennzeichen") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Vorerfasser" HeaderText="Erfasser">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Vorerfasser" runat="server" CommandName="Sort" CommandArgument="Vorerfasser">Erfasser</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVorerfasser" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Vorerfasser") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle Width="75px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnEdit" CssClass="editbutton" CommandName="Bearbeiten" Text="Edit"
                                runat="server" ToolTip="Bearbeiten" Visible='<%# DataBinder.Eval(Container, "DataItem.toDelete").ToString() != "X" %>'
                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'></asp:LinkButton>
                            <asp:LinkButton ID="lbtnDel" CssClass="deletebutton" CommandName="Loeschen"
                                Text="Delete" runat="server" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="formbuttons">
        <asp:Button ID="cmdCancel" runat="server" CssClass="button" Text="Abbrechen" 
        onclick="cmdCancel_Click" />
        <asp:Button ID="cmdSave" runat="server" CssClass="submitbutton" Text="Absenden" OnClick="cmdSave_Click"
            OnClientClick="javascript: return false;" />
        <asp:Button ID="cmdContinue" runat="server" CssClass="submitbutton" 
            Text="Weiter" Visible ="false" onclick="cmdContinue_Click" />
    </div>
    <div id="dialog" title="Absenden">
        Sollen die Aufträge jetzt gesendet werden?
    </div>
    <div id="divEditPosDlgContainer">
        <div id="divEditPos" title="Löschen" style="display: none; ">
            <asp:UpdatePanel ID="upnlEditPos" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="cmdSavePos" />
                </Triggers>
                <ContentTemplate>
                    <asp:HiddenField ID="hfID" runat="server" />
                
                     <div  style="margin-bottom: 20px">Soll dieser Auftrag wirklich gelöscht werden?</div>
                     <div  style="float:right">
                    <asp:Button ID="cmdCloseDialog" runat="server"  Text="Abbrechen" CssClass="dynbutton"
                        Width="78px" onclick="cmdCloseDialog_Click"  />
                    <asp:Button ID="cmdSavePos" runat="server" Text="Löschen" CssClass="dynbutton"
                        Width="78px" onclick="cmdSavePos_Click" /></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
        <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" VisibleOnPageLoad="false" Modal="true" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" Title="Auftragsdaten herunterladen" runat="server"  Behaviors="Resize, Move" OnClientClose="OnClientClose"
                NavigateUrl="PrintDialog.aspx" Width="550" Height="300" Modal="true" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>
