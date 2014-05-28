<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AbmeldungAHZul.aspx.cs" Inherits="AutohausPortal.forms.AbmeldungAHZul"  
MasterPageFile="/AutohausPortal/MasterPage/FormBig.Master" %>
<%@ MasterType VirtualPath="/AutohausPortal/MasterPage/FormBig.Master" %>
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
                    Abmelden: function () {
                        $(this).dialog('close');
                        __doPostBack('ctl00$ContentPlaceHolder1$cmdSave', '');

                    }
                }
            });
            $("#dialog2").dialog({
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
                    Löschen: function () {
                        $(this).dialog('close');
                        __doPostBack('ctl00$ContentPlaceHolder1$cmdDelete', '');

                    }
                }
            });
            $('#ctl00_ContentPlaceHolder1_cmdSave').click(function () {
                $('#dialog').dialog('open');
            });
            $('#ctl00_ContentPlaceHolder1_cmdDelete').click(function () {
                $('#dialog2').dialog('open');
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
        <ContentTemplate>

            <asp:GridView ID="gvZuldienst" Width="100%" runat="server" AutoGenerateColumns="False"
                CellPadding="0" CellSpacing="0" GridLines="None" AllowSorting="true" AllowPaging="True"  DataKeyNames="ZULBELN"
                CssClass="GridView" PageSize="300" OnSorting="gvZuldienst_Sorting">
                <HeaderStyle CssClass="GridTableHead" ForeColor="White" />
                <PagerSettings Visible="False" />
                <RowStyle CssClass="ItemStyle" />
                <Columns>
                    <asp:TemplateField SortExpression="ZULBELN" HeaderText="ID">
                        <HeaderTemplate>
                        <div class="formselects">
                            <asp:CheckBox ID="chkAuswahlAll" CssClass="tableCheckbox checkall" runat="server" /></div>
                            <asp:LinkButton ID="col_ID" runat="server" CommandName="Sort" CommandArgument="ZULBELN">ID</asp:LinkButton>
                        </HeaderTemplate>
                        <ItemTemplate><div class="formselects">
                            <asp:CheckBox ID="chkAuswahl" CssClass="tableCheckbox" runat="server" /></div>
                            <asp:Label ID="lblID" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZULBELN") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Auswahl" Visible="false" HeaderText="Status">
                       <ItemTemplate>
                            <asp:Label ID="lblAuswahl" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Auswahl") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="NAME1" HeaderText="Kunde">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_kundenname" runat="server" CommandName="Sort" CommandArgument="NAME1">Kunde</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblkundenname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.NAME1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="MAKTX" HeaderText="Dienstleistung">
                        <HeaderTemplate>
                            <asp:LinkButton ID="colMatbez" runat="server" CommandName="Sort" CommandArgument="MAKTX">Dienstleistung</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMatbez" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.MAKTX") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZZLDAT" HeaderText="urspüngl. Zulassungsdatum">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Zulassungsdatum" runat="server" CommandName="Sort" CommandArgument="ZZZLDAT">urspüngl. Zulassungsdatum</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblZulassungsdatum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZZLDAT", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="HALTE_DAUER" HeaderText="Haltedauer bis">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Haltedauer" runat="server" CommandName="Sort" CommandArgument="HALTE_DAUER">Haltedauer bis</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblHaltedauer" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.HALTE_DAUER", "{0:d}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZREFNR1" HeaderText="Referenz1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz1" runat="server" CommandName="Sort" CommandArgument="ZZREFNR1">Referenz1</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZREFNR2" HeaderText="Referenz1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz2" runat="server" CommandName="Sort" CommandArgument="ZZREFNR2">Referenz2</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz2" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZREFNR3" HeaderText="Referenz1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz3" runat="server" CommandName="Sort" CommandArgument="ZZREFNR3">Referenz3</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz3" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZREFNR4" HeaderText="Referenz1">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Referenz4" runat="server" CommandName="Sort" CommandArgument="ZZREFNR4">Referenz4</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReferenz4" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ZZREFNR4") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ZZKENN" HeaderText="Kennzeichen">
                        <HeaderTemplate>
                            <asp:LinkButton ID="col_Kennzeichen" runat="server" CommandName="Sort" CommandArgument="ZZKENN">Kennzeichen</asp:LinkButton></HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtKennzeichen" CssClass="Textbox110 TextUpperCase" Text='<%# DataBinder.Eval(Container, "DataItem.ZZKENN") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>



        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="formbuttons">
        <asp:Button ID="cmdCancel" runat="server" CssClass="button" Text="Abbrechen" 
        onclick="cmdCancel_Click" />
        <asp:Button ID="cmdSave" runat="server" CssClass="submitbutton" ToolTip = "ausgwählte Aufträge abmelden" Text="Abmelden" OnClick="cmdSave_Click"
            OnClientClick="javascript: return false;" />
        <asp:Button ID="cmdDelete" runat="server" CssClass="submitbutton" 
            ToolTip = "ausgwählte Aufträge löschen" Text="Löschen" 
            OnClientClick="javascript: return false;" onclick="cmdDelete_Click" />
    </div>
    <div id="dialog" title="Abmelden">
        Sollen die markierten Aufträge jetzt abgemeldet werden?
    </div>
    <div id="dialog2" title="Löschen">
        Sollen die markierten Aufträge jetzt gelöscht werden?
    </div>
        <telerik:RadWindowManager ID="RadWindowManager1" ShowContentDuringLoad="false" VisibleStatusbar="false"
        ReloadOnShow="true" runat="server" VisibleOnPageLoad="false" Modal="true" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" Title="Auftragsdaten herunterladen" runat="server"  Behaviors="Resize, Move" OnClientClose="OnClientClose"
                NavigateUrl="PrintDialog.aspx" Width="550" Height="300" Modal="true" >
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
</asp:Content>
