<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="KassenabrechnungNeu.aspx.cs" Inherits="AppZulassungsdienst.forms.KassenabrechnungNeu" MasterPageFile="../MasterPage/Big.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <script src="/PortalZLD/JScript/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.blockUI.js" type="text/javascript"></script>
        <script src="/PortalZLD/JScript/jquery.ui.datepicker-de.js" type="text/javascript"></script>
    <script src="/PortalZLD/JScript/jquery.scrollTo-1.4.2.js" type="text/javascript"></script>
	<script type="text/javascript">
	    var scroll;
        $(document).ready(function() {
			$("#divEditPos").dialog({
			    autoOpen: false,
			    bgiframe: true,
				modal: true,
				resizable: false,
				closeOnEscape: false,
				height: 300,
				width: 830,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divEditPosDlgContainer");
                    $(this).parent().children().children('.ui-dialog-titlebar-close').hide();

                }
                
            });


        });

        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_txtStartDate").unbind();
            $("#ctl00_ContentPlaceHolder1_txtEndDate").unbind();
            $("#ctl00_ContentPlaceHolder1_txtStartDate").datepicker();
            $("#ctl00_ContentPlaceHolder1_txtEndDate").datepicker();
            $("#ctl00_ContentPlaceHolder1_txtEndDate").keypress();
        }


        function closeDialogandSave() {
			//Could cause an infinite loop because of "on close handling"
		    $("#divEditPos").dialog('close');
		    __doPostBack('ctl00$ContentPlaceHolder1$btnRefreshGrid', '');
		}
		function closeDialog() {
		    //Could cause an infinite loop because of "on close handling"
		    $("#divEditPos").dialog('close');
		}
		function openDialog(title) {

		    $("#divEditPos").dialog('open');
		}
        
		function openDialogAndBlock(title) {
			openDialog(title);

			//block it to clean out the data
			$("#divEditPos").block({
			    message: '<img src="/PortalZLD/images/indicator.gif" />',
				css: { border: '0px' },
				fadeIn: 0,
				overlayCSS: { backgroundColor: '#ffffff', opacity: 1 } 
			});
		}

		
		function unblockDialog() {
		    $("#divEditPos").unblock();
		}

		function datePick (control) {
		    $(control).datepicker();
		}
   
		function Savescroll(control) {

		    scroll= $(control).scrollTop();
		}
		function SetScrollPos() {
		    if (scroll != null)
		    {
		        $("#ctl00_ContentPlaceHolder1_pEingabe").scrollTo(scroll);
		    }
		}

	</script>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" onclick="btnBack_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                        <div id="innerContentRight" style="width: 100%">
                    
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <Triggers>                            
                                </Triggers>
                                <ContentTemplate>
                                <div id="innerContentRightHeading">
                                <h1>
                                    <asp:Label ID="lblHead" runat="server" Text="Kassenabrechnung"></asp:Label>
                                </h1>
                            </div>
                            <div id="TableQuery" style="margin-bottom: 10px;">
                                <asp:Panel ID="Panel1" runat="server">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td colspan="3" style="background-color: #dfdfdf; height: 22px; padding-left: 15px">
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3">
                                                <asp:Label ID="lblErrorMain" runat="server" CssClass="TextError"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align:top; padding:10px 5px 0px 15px;" >
                                            <td class="firstLeft active" valign="top" >                                            
                                                <div style="background-color: #dfdfdf; height: 10px; padding:5px 5px 5px 15px;">
                                                    Anzeigezeitraum</div>
                                                <div style="white-space: nowrap; padding:5px 5px 5px 15px; border:1px solid #dfdfdf;">
                                                    <asp:TextBox ID="txtStartDate" runat="server" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                                    <asp:Label ID="lblStartDate" runat="server" Visible="false"></asp:Label>
                                                    <asp:Label ID="Label1" runat="server">&nbsp;-&nbsp;</asp:Label>
                                                    <asp:TextBox ID="txtEndDate" runat="server" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                                    <asp:Label ID="lblEndDate" runat="server" Visible="false"></asp:Label>
                                                </div>
                                            </td>
                                            <td class="formquery" style="text-align: center; vertical-align: middle; border-style: none; margin-top: 5px;">
                                                <div style="margin-bottom: 5px;">
                                                    <asp:LinkButton ID="lbtnTimeRange" runat="server" Text="gew. Zeitraum" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="20px" Style="margin-bottom: 5px;
                                                        border-style: none; text-align: center;font-size: 9px" 
                                                        onclick="lbtnTimeRange_Click" />
                                                    <asp:LinkButton ID="lbtnToday" runat="server" Text="Heute" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="20px" OnClick="btnToday_Click" Style="margin-bottom: 5px;
                                                        border-style: none; text-align: center;font-size: 9px" />
                                                    <asp:LinkButton ID="lbtnThisWeek" runat="server" Text="Diese Woche" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="20px" OnClick="btnThisWeek_Click" Style="margin-bottom: 5px;
                                                        border-style: none; text-align: center;font-size: 9px" />
                                                    <asp:LinkButton ID="lbtnCurrentPeriod" runat="server" Text="Laufende Periode" CssClass="TablebuttonMiddle"
                                                        Width="100px" Height="20px" OnClick="btnCurrentPeriod_Click" Style="margin-bottom: 5px;
                                                        border-style: none; text-align: center;font-size: 9px" />
                                                </div>
                                            </td>
                                            <td width="100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr class="formquery">
                                            <td class="firstLeft active" colspan="3" align="left">
                                                <asp:Panel ID="pUebersicht" runat="server">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                Anfangsbestand:
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblAnfangsbestand" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &#128;
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                + Summe Einnahmen:
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblSummeEinnahmen" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &#128;
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lbShowEinnahmen" runat="server" CssClass="TablebuttonSmall" OnClick="lbShowEinnahmen_Click"
                                                                    Style="font-size: 12px;" Width="50px">+</asp:LinkButton>&nbsp;
                                                                <asp:Label ID="Label2" runat="server">Zu den Einnahmen</asp:Label>
                                                            </td>
                                                            <td width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                - Summe Ausgaben:
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblSummeAusgaben" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &#128;
                                                            </td>
                                                            <td style="white-space: nowrap;">
                                                                <asp:LinkButton ID="lbShowAusgaben" runat="server" CssClass="TablebuttonSmall" OnClick="lbShowAusgaben_Click"
                                                                    Style="font-size: 12px;" Width="50px">+</asp:LinkButton>&nbsp;
                                                                <asp:Label ID="Label3" runat="server">Zu den Ausgaben</asp:Label>
                                                            </td>
                                                            <td width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <hr width="100%"></hr>
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                = Endbestand:
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblEndbestand" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                &#128;
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td width="100%">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <div id="KassenGridHeader" runat="server"  visible="false" class="KassenGridHeader" >
                                        <table cellpadding="0" cellspacing="0" style="border: 1px solid #dfdfdf; border-bottom: none; font-size:11px" >
                                        <tr>
                                            <td  style="width:20px">

                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col1" >
                                             Geschäftsvorfall
                                            </td>
                                             <td style="font-weight: bold;" class="TablePaddingKasse col2">
                                             Datum
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col3">
                                             Betrag <br />Brutto &#8364;
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col3">
                                             Betrag <br />Netto &#8364;
                                            </td>
                                             <td style="font-weight: bold;" class="TablePaddingKasse col4">
                                             
                                            </td>
                                             <td class="TablePaddingKasse col5" style="font-weight: bold;">
                                             Kst.
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col6">
                                             Zuordnung
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col7">
                                             Text
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse col8" >
                                             Auftrag
                                            </td>
                                            <td style="font-weight: bold" class="TablePaddingKasse col9">
                                             Barcode
                                            </td>
                                            <td style="font-weight: bold" class="TablePaddingKasse col10">
                                             Debitor/ <br /> Kreditor
                                            </td>
                                             <td style="font-weight: bold" class="TablePaddingKasse">
                                             
                                            </td>
                                        </tr>
                                        </table>
                                        </div>
                                    <asp:Panel ID="pEingabe" onscroll="Savescroll(this)" CssClass="KassenGrid" runat="server" 
                                        Style="padding:0px 0px  10px 0px;border: 1px solid #dfdfdf; border-top: none " visible="false"  BorderColor="#dfdfdf">
                                        <asp:HiddenField ID="hfScrollPosition" runat="server" Value="0" />     

                                        <asp:GridView ID="gvDaten" Style="border: none;" runat="server" AutoGenerateColumns="False"
                                            OnRowCommand="gvDaten_RowCommand" onrowdatabound="gvDaten_RowDataBound"  ShowHeader="false">
                                           <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%#Eval("Ampel") %>'
                                                            Height="16px" Width="16px" />
                                                        <asp:Label ID="lblPostNr" runat="server" Visible="false" Text='<%#Eval("POSTING_NUMBER") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" Width="16px" />
                                                    <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px"  Width="16px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Geschäftsvorfall">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlVorfall" CssClass="TextBoxNormal" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlVorfall_SelectedIndexChanged" 
                                                           Font-Size="11px" ></asp:DropDownList>
                                                        <asp:TextBox  Font-Size="11px" ID="txtVorfall" CssClass="TextBoxNormal" runat="server" MaxLength="10" Width="200px" 
                                                            Text='<%# Eval("TRANSACT_NAME") %>' Enabled="false"></asp:TextBox>
                                                        <asp:Label ID="lblStatus" Visible="false" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" Width="150px" CssClass="TablePaddingKasse" />
                                                    <HeaderStyle Font-Size="11px"  CssClass="TablePaddingKasse"  BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Datum">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDatum" CssClass="TextBoxNormal" Font-Size="11px" runat="server" MaxLength="10" Width="70px" 
                                                            Text='<%# Eval("BUDAT", "{0:d}") %>' onKeyPress="return numbersonly(event, true)" ></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse" Width="76px"/>
                                                    <HeaderStyle Font-Size="11px" CssClass="TablePaddingKasse" BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px" Width="76px"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Betrag <br />Brutto &#8364;">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBetragBruttoEinnahmen" Font-Size="11px" Width="60px" onKeyPress="return numbersonly(event, true)"
                                                            Text='<%# Eval("H_RECEIPTS", "{0:f}") %>' CssClass="TextBoxNormalRight"
                                                            runat="server"></asp:TextBox>
                                                        <asp:TextBox ID="txtBetragBruttoAusgaben" Font-Size="11px" Width="60px" onKeyPress="return numbersonly(event, true)"
                                                            Text='<%# Eval("H_PAYMENTS", "{0:f}") %>' CssClass="TextBoxNormalRight"
                                                            runat="server" ></asp:TextBox>                                                    
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse" Width="66px" />
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" HorizontalAlign="Left"  CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px" Width="66px"/>
                                                </asp:TemplateField>                                               
                                                <asp:TemplateField HeaderText="Betrag <br />Netto &#8364;">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBetragNetto" Font-Size="11px" Enabled="false" Width="60px" Text='<%# Eval("H_NET_AMOUNT", "{0:f}") %>'
                                                            CssClass="TextBoxNormalRight" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse"  Width="66px" />
                                                    <HeaderStyle  Font-Size="11px" BorderStyle="None" HorizontalAlign="Left"  CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px"  Width="66px"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgRefresh2" CommandName="Refresh" CommandArgument='<%# Eval("Posting_Number") %>'
                                                            ImageUrl="/PortalZLD/Images/blank.gif" Height="0px" Width="0px" runat="server"  />
                                                        <asp:ImageButton ID="ibtnSplit" CommandName="Split" CommandArgument='<%# Eval("Posting_Number") %>'
                                                             ImageUrl="/PortalZLD/Images/Pfeil202_07.jpg" onKeyPress="return false;" ToolTip="Split"
                                                            Height="16px" Width="16px" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse" Width="16px"/>
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" BorderColor="#ffffff" CssClass="TablePaddingKasse"
                                                        BorderWidth="0px" Width="16px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Kst.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtKST" runat="server" MaxLength="4" Width="30px" Text='<%# Eval("KOSTL","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal"  Font-Size="11px" ></asp:TextBox>                                                      
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None"  CssClass="TablePaddingKasse" Width="36px"/>
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px" Width="36px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zuordnung">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtZuordnung" Font-Size="11px" runat="server" Width="105px" MaxLength="18" Text='<%# Eval("ZUONR") %>'
                                                                     CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse"  Width="115px"/>
                                                    <HeaderStyle Font-Size="11px"  CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px"  Width="115px"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Text">
                                                    <ItemTemplate>
                                                        <asp:TextBox Font-Size="11px" ID="txtFreitext" runat="server" Width="120px" MaxLength="50" Text='<%# Eval("SGTXT") %>'
                                                                     CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse"  Width="128px"/>
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" HorizontalAlign="Left" CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px" Width="128px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Auftrag">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAuftrag" runat="server" Width="90px" MaxLength="12" Font-Size="11px"  Text='<%# Eval("ORDERID") %>'
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse" Width="96px"/>
                                                    <HeaderStyle Font-Size="11px"  BorderStyle="None" HorizontalAlign="Left" CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px" Width="96px" />
                                                </asp:TemplateField>
                                                 <asp:TemplateField HeaderText="Barcode">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBarcode" runat="server" Width="50px" Font-Size="11px" Text='<%# Eval("DOCUMENT_NUMBER") %>' 
                                                                    onKeyPress="return numbersonly(event, false)" MaxLength = "6" CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None"  CssClass="TablePaddingKasse"  Width="56px"/>
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" HorizontalAlign="Left" CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px" Width="56px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField >
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDebKred" Font-Size="11px" runat="server" Text="Debitor/ <br /> Kreditor"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDebitor" runat="server" MaxLength="10" Width="80px" Text='<%# Eval("KUNNR","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" Font-Size="11px"></asp:TextBox>                                                         
                                                        <asp:TextBox ID="txtKreditor" runat="server" MaxLength="10" Width="80px" Text='<%# Eval("LIFNR","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)"  CssClass="TextBoxNormal" Font-Size="11px"></asp:TextBox>                                                   
                                                    </ItemTemplate>                                                    
                                                    <ItemStyle BorderStyle="none" CssClass="TablePaddingKasse"  Width="86px"/>
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left"  CssClass="TablePaddingKasse"
                                                        BorderColor="#ffffff" BorderWidth="0px"  Width="86px"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAuswahl" ToolTip="für das Buchen markieren" runat="server" TabIndex="-1" />

                                                        <asp:ImageButton ID="ImageButton1" CommandName="Confirm" CommandArgument='<%# Eval("Posting_Number") %>'
                                                            TabIndex="-1" ImageUrl="/PortalZLD/Images/haken_gruen.gif" Visible='<%# Eval("ASTATUS").ToString()=="ZA" %>' 
                                                            ToolTip='<%# Eval("TRANSACT_NUMBER").ToString().Trim() == "34" ? "Vorgang bestätigen" : (Eval("TRANSACT_NUMBER").ToString().Trim() == "31" ? "Betrag ausgegeben" : "Betrag erhalten") %>' 
                                                            Height="18px" Width="18px" runat="server" />

                                                        <asp:ImageButton ID="ibtnDel" CommandName="Del"  CommandArgument='<%# Eval("Posting_Number") %>'
                                                             ImageUrl="/PortalZLD/Images/RecycleBin.png"
                                                             TabIndex="-1" Height="16px" Width="16px" runat="server" ToolTip="Löschen"/>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="TablePaddingKasse" Width="100%" />
                                                    <HeaderStyle Font-Size="11px" BorderStyle="None" CssClass="TablePaddingKasse" BorderColor="#ffffff"
                                                        BorderWidth="0px"  Width="100%"/>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>                                        
                                    </asp:Panel>
                                    <asp:Panel ID="pNewLine" runat="server" Visible="false">
                                            <asp:LinkButton ID="lbNewLine" runat="server" CssClass="TablebuttonSmall" OnClick="lbNewLine_Click"
                                                Style="font-size: 12px; margin: 10px 0px 5px 15px;" Width="50px">+</asp:LinkButton>&nbsp;
                                            <asp:Label ID="Label4" runat="server">Neue Zeile</asp:Label>
                                        </asp:Panel>
                                        <asp:Panel ID="plMessage" runat="server">
                                            <asp:Label ID="lblError" Style="margin: 10px 0px 5px 15px;" runat="server" CssClass="TextError"></asp:Label>
                                            <asp:Label ID="lblMessage" Style="margin: 10px 0px 5px 15px;" runat="server" ForeColor="#269700" Font-Bold="True" Visible="True"></asp:Label> 
                                        </asp:Panel>
                                        
                                    <div id="divButtons" runat="server" class="formquery active" style="white-space: nowrap;
                                        text-align: right; margin-right: 10px; margin-top: 10px;">
                                        <asp:LinkButton ID="lbtnBack" runat="server" Text="Zurück" CssClass="Tablebutton" Width="78px"
                                            Style="margin-bottom: 5px; border-style: none; text-align: center;"
                                            OnClick="btnBack_Click" visible="false"/>

                                        <asp:LinkButton ID="cmdMark" runat="server" Text="alle markieren" 
                                            CssClass="TablebuttonMiddle" Width="100px"
                                            Style="margin-bottom: 5px; border-style: none; text-align: center;"
                                            visible="false" onclick="cmdMark_Click"/>

                                        <asp:LinkButton ID="lbtnSave" runat="server" Text="Sichern" CssClass="Tablebutton" Width="78px"
                                            Style="margin-bottom: 5px; border-style: none; text-align: center;"
                                            OnClick="btnSave_Click" visible="false"/>
                                        <asp:LinkButton ID="lbtnBuchen" runat="server" Text="Buchen" CssClass="Tablebutton" Width="78px"
                                            Style="margin-bottom: 5px; border-style: none; text-align: center;"
                                            OnClick="btnBuchen_Click" visible="false"/>
                                            <asp:LinkButton ID="btnRefreshGrid" CausesValidation="false" OnClick="btnRefreshGrid_Click" style="display:none" runat="server"></asp:LinkButton>   
                                    </div>
                                </asp:Panel>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div id="dataFooter">
                        &nbsp;</div>
                    <div id="divEditPosDlgContainer">
                        <div id="divEditPos" title="Belegsplit" style="display: none;">
                            <asp:UpdatePanel ID="upnlEditPos" runat="server">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hfPostingNumber" runat="server" />
                                    <asp:HiddenField ID="hfGesamt" runat="server" />
                                   <asp:HiddenField ID="hfNettoGesamt" runat="server" />
                                    <asp:HiddenField ID="hfStatus" runat="server" />
                                        <asp:GridView ID="GridView1" Style="border: none;" runat="server" 
                                            AutoGenerateColumns="False" onrowcommand="GridView1_RowCommand" >
                                            <Columns>
												<asp:TemplateField Visible="false">
														<ItemTemplate>
														<asp:Label ID="lblPostingNr" runat="server" Text='<%# Eval("POSTING_NUMBER") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField> 
												<asp:TemplateField Visible="false">
														<ItemTemplate>
														<asp:Label ID="lblPositionNr" runat="server" Text='<%# Eval("POSITION_NUMBER") %>'></asp:Label>
													</ItemTemplate>
												</asp:TemplateField> 
                                                <asp:TemplateField HeaderText="Geschäftsvorfall">
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="ddlVorfall" CssClass="TextBoxNormal" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlVorfall_SelectedIndexChanged2" ></asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" Width="150px" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Betrag <br />Brutto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBetragBruttoEinnahmen" Width="60px" onKeyPress="return numbersonly(event, true)"
                                                            Text='<%# Eval("P_RECEIPTS", "{0:f}") %>' CssClass="TextBoxNormal"
                                                            runat="server"></asp:TextBox>
                                                        <asp:TextBox ID="txtBetragBruttoAusgaben" Width="60px" onKeyPress="return numbersonly(event, true)"
                                                            Text='<%# Eval("P_PAYMENTS", "{0:f}") %>' CssClass="TextBoxNormal"
                                                            runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Betrag <br />Netto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtBetragNetto" Enabled="false" Width="60px" Text='<%# Eval("P_NET_AMOUNT", "{0:f}") %>'
                                                            CssClass="TextBoxNormal" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Kst.">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtKST" runat="server" MaxLength="4" Width="40px" Text='<%# Eval("KOSTL","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Zuordnung">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtZuordnung" runat="server" Width="100px" MaxLength="18" Text='<%# Eval("ALLOC_NMBR") %>'
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Text">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtFreitext" runat="server" Width="100px" MaxLength="50" Text='<%# Eval("SGTXT") %>'
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Auftrag">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAuftrag" runat="server" MaxLength="12" Width="100px" Text='<%# Eval("ORDERID") %>'
                                                            CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblDebKred" runat="server" Text="Debitor/ <br /> Kreditor"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtDebitor" runat="server" MaxLength="10" Width="80px" Text='<%# Eval("KUNNR","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal"></asp:TextBox>
                                                        <asp:TextBox ID="txtKreditor" runat="server" MaxLength="10" Width="80px" Text='<%# Eval("LIFNR","{0:d}") %>'
                                                            onKeyPress="return numbersonly(event, false)" CssClass="TextBoxNormal"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="none" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" HorizontalAlign="Left" CssClass="firstLeft active"
                                                        BorderColor="#ffffff" BorderWidth="0px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                            <asp:ImageButton  ID="cmdGridRefresh" runat="server"  Width="0px" Height="0"
                                                ImageUrl="/PortalZLD/images/blank.gif" onclick="cmdRefresh_Click"></asp:ImageButton>
                                                        <asp:ImageButton ID="ibtnDel" CommandName="Del" CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                                            TabIndex="-1" ImageUrl="/PortalZLD/Images/RecycleBin.png" Visible='<%# ShowDel2(((GridViewRow)Container).RowIndex) %>' Height="16px" Width="16px" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle BorderStyle="None" CssClass="firstLeft active" />
                                                    <HeaderStyle BorderStyle="None" CssClass="firstLeft active" BorderColor="#ffffff"
                                                        BorderWidth="0px" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                         
                                        <table cellpadding="3" cellspacing="0" style="color:#595959">
                                        <tr>
                                            <td style="width:25%">
                                                <asp:LinkButton ID="cmdNewPos2"   runat="server" CssClass="TablebuttonSmall" Style="font-size: 12px;
                                                 text-align:center"
                                                 Width="50px" onclick="cmdNewPos_Click"  ToolTip="Neue Zeile">+</asp:LinkButton>&nbsp;
                                           </td>
                                           <td  style="width:75%">

                                            <asp:ImageButton  ID="cmdRefresh" runat="server" style="padding-top:2px"   Width="50px" 
                                                ImageUrl="/PortalZLD/images/refresh.jpg" onclick="cmdRefresh_Click" ToolTip="Aktualisieren" ></asp:ImageButton>
                                           </td>
                                        </tr>
                                        <tr>
                                           <td colspan="2"  style="width:100%">
                                               <asp:Label ID="lblPosError" runat="server" CssClass="TextError" ></asp:Label>
                                           </td>
                                        </tr>
                                     </table>
                                     <table cellpadding="0" cellspacing="0" style="color:#595959">
                                        <tr>
                                            <td style="padding-top:10px;padding-right:10px;font-size: 12px;">
                                                <asp:Label ID="lblGesamt" runat="server" Text="Geamtbetrag:"></asp:Label>
                                            </td>
                                            <td  align="right"  style="padding-top:10px;padding-right:10px;font-size: 12px">
                                                <asp:Label ID="lblGesamtShow" runat="server"></asp:Label>  
                                            </td>
                                               
                                            <td style="padding-top:10px;font-size: 12px">
                                             <asp:Label ID="lblEuro1" runat="server" Text="EUR"></asp:Label>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="padding-top:10px;padding-right:10px;font-size: 12px" nowrap="nowrap">
                                                <asp:Label ID="lblGesamtPos" runat="server" Text="Summe Positionen:"></asp:Label>
                                            </td>
                                            <td class="firstLeft active" align="right" style="padding-top:10px;padding-right:10px;font-size: 12px">
                                                <asp:Label ID="lblGesamtPosShow" runat="server" ></asp:Label>  
                                            </td>
                                               
                                            <td style="padding-top:10px;font-size: 12px">
                                             <asp:Label ID="lblEuro2" runat="server" Text="EUR"></asp:Label>  
                                            </td>
                                        </tr>
                                         <tr>
                                            <td style="padding-top:10px;padding-right:10px;font-size: 12px">
                                                <asp:Label ID="lblDiff" runat="server" Text="Differenz:" ></asp:Label>
                                            </td>
                                            <td align="right" style="padding-top:10px;padding-right:10px;font-size: 12px">
                                                <asp:Label ID="lblDiffShow" runat="server" ></asp:Label>  
                                            </td>
                                               
                                            <td style="width:100%;padding-top:10px;font-size: 12px">
                                             <asp:Label ID="Label10" runat="server" Text="EUR"></asp:Label>  
                                            </td>
                                        </tr>
                                    </table>


 
                                    <div id="div1" class="dataQueryFooter">

                                        <asp:LinkButton ID="cmdCloseDialog" runat="server" Text="Schließen" CssClass="Tablebutton"
                                            Width="78px" onclick="cmdCloseDialog_Click"  />
                                        <asp:LinkButton ID="cmdSavePos" runat="server" Text="Übernehmen" CssClass="Tablebutton"
                                            Width="78px" onclick="cmdSavePos_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
		            <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
			            <ContentTemplate>
				            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
			            </ContentTemplate>
		            </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
