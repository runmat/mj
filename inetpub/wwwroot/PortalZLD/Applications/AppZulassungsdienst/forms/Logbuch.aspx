<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="Logbuch.aspx.cs"
    Inherits="AppZulassungsdienst.forms.Logbuch" MasterPageFile="../MasterPage/App.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/helper.js?22042016"></script>
    <style type="text/css">
        .DistanceText
        {
            padding: 3px 0px 0px 0px;
        }
        .DistanceButton
        {
            margin: 3px 0px 3px 0px;
        }
        .NoDistance td
        {
            margin: 0px;
            padding: 0px;
            border: none 0px white;
        }
    </style>
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lb_zurueck" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="Zurück" OnClick="lb_zurueck_Click"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Logbuch"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        &nbsp;
                    </div>
                    <div id="TableQuery" style="margin-bottom: 0px">
                        <table id="tab1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td class="firstLeft active" colspan="2" width="100%">
                                        <asp:Label ID="lblError" runat="server" CssClass="TextError"></asp:Label>
                                        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Style="color: Green;"></asp:Label>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div>
                        &nbsp;
                    </div>
                    <div id="tblHeaderTabs" runat="server" visible="true">
                        <table id="Table1" runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton ID="lbAufgaben" runat="server" CssClass="TabButtonBig" Width="136px" OnClick="lbAufgaben_Click">Aufgaben</asp:LinkButton>
                                        <asp:LinkButton ID="lbProtokoll" runat="server" CssClass="TabButtonBig Active" Width="136px"
                                            Style="margin-left: 5px;" OnClick="lbProtokoll_Click">Protokoll</asp:LinkButton>
                                    </td>
                                    <td class="firstLeft active" width="100%">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataHeader" style="background-image: url(/PortalZLD/Images/overflow.png); color: #ffffff;
                        font-weight: bold; float: left; height: 22px; line-height: 22px; width: 892px;
                        white-space: nowrap; background-color: #2B4C91; padding-left: 15px;">
                        <table cellpadding="0px" cellspacing="0px" style="padding-right:10px;">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblUser" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;" width="100%">
                                    KST.:
                                    <asp:Label ID="lblKostenstelle" runat="server"></asp:Label>
                                    / GL-Gebiet:
                                    <asp:Label ID="lblGL" runat="server"></asp:Label>
                                </td>
                            </tr>
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
                                    <div id="divTimeSpan" style="text-align: left; padding: 3px 0px 8px 5px; border-right: solid 1px gray;
                                        border-left: solid 1px #595959; border-bottom: solid 1px #595959;" runat="server">
                                        <asp:Label ID="Label1" runat="server">Von:</asp:Label>
                                        <asp:TextBox ID="txtDatumVon" runat="server" Width="80px" Style="padding-left: 3px;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CeTxtDatumVon" runat="server" TargetControlID="txtDatumVon">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MeeTxtDatumVon" runat="server" TargetControlID="txtDatumVon"
                                            MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="MevTxtDatumVon" runat="server" ControlExtender="MeeTxtDatumVon"
                                            ControlToValidate="txtDatumVon" MaximumValue="31.12.2050" MinimumValue="01.01.2012"
                                            MaximumValueMessage="Datum liegt zu weit in der Zukunft!" MinimumValueMessage="Datum liegt in der Vergangenheit!"
                                            InvalidValueMessage="ung&uuml;ltiger Wert" Display="Dynamic" EmptyValueMessage="Geben Sie ein Datum ein!"
                                            ValidationExpression="(([0-2]\d)|(3[0,1]))\.((0\d)|(1[0-2]))\.(2\d{3})"></ajaxToolkit:MaskedEditValidator>
                                        <asp:Label ID="Label2" runat="server" Style="padding-left: 5px;">Bis:</asp:Label>
                                        <asp:TextBox ID="txtDatumBis" runat="server" Width="80px" Style="padding-left: 3px;"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="cetxtDatumBis" runat="server" TargetControlID="txtDatumBis">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="MeetxtDatumBis" runat="server" TargetControlID="txtDatumBis"
                                            MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="mevtxtDatumBis" runat="server" ControlExtender="MeeTxtDatumBis"
                                            ControlToValidate="txtDatumBis" MaximumValue="31.12.2050" MinimumValue="01.01.2012"
                                            MaximumValueMessage="Datum liegt zu weit in der Zukunft!" MinimumValueMessage="Datum liegt in der Vergangenheit!"
                                            InvalidValueMessage="ung&uuml;ltiger Wert" Display="Dynamic" EmptyValueMessage="Geben Sie ein Datum ein!"
                                            ValidationExpression="(([0-2]\d)|(3[0,1]))\.((0\d)|(1[0-2]))\.(2\d{3})"></ajaxToolkit:MaskedEditValidator>
                                        <asp:Label ID="Label3" runat="server" Text="Filter:"></asp:Label>
                                        <asp:DropDownList ID="ddlFilter" runat="server">
                                            <asp:ListItem Selected="True" Text="-- Alle --" Value="all"></asp:ListItem>
                                            <asp:ListItem Text="Beantwortet" Value="E3"></asp:ListItem>
                                            <asp:ListItem Text="Erledigt" Value="E4"></asp:ListItem>
                                            <asp:ListItem Text="Gelesen" Value="E1"></asp:ListItem>
                                            <asp:ListItem Text="Geschlossen" Value="3"></asp:ListItem>
                                            <asp:ListItem Text="Gesendet" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Neu" Value="E0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlFilterFiliale" runat="server">
                                            <asp:ListItem Selected="True" Text="-- Alle --" Value="all"></asp:ListItem>
                                            <asp:ListItem Text="Beantwortet" Value="E3"></asp:ListItem>
                                            <asp:ListItem Text="Erledigt" Value="E4"></asp:ListItem>
                                            <asp:ListItem Text="Gelesen" Value="E1"></asp:ListItem>
                                            <asp:ListItem Text="Neu" Value="E0"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="lbtnRefresh" runat="server" Style="margin-left: 5px; width: 16px;
                                            height: 16px;" OnClick="lbtnRefresh_Click">
                                            <img src="/PortalZLD/Images/arrow_refresh.png" style="width: 16px; height: 16px; vertical-align:middle;" alt="Refresh" title="Aktualisieren" />
                                        </asp:LinkButton>
                                    </div>
                                    <asp:GridView ID="gvAufgaben" runat="server" AutoGenerateColumns="false" 
                                        AllowPaging="false" AllowSorting="true" 
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" 
                                        Style="border-collapse: collapse ! important;" 
                                        onrowcommand="gvAufgaben_RowCommand" onsorting="gvAufgaben_Sorting">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle DetailTable" />
                                        <Columns>
                                            <asp:BoundField DataField="Rowindex" Visible="false" />
                                            <asp:BoundField DataField="I_VORGID" Visible="false" />
                                            <asp:BoundField DataField="I_LFDNR" Visible="false" />
                                            <asp:TemplateField HeaderText="Datum">
                                               <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("I_DATUM","{0:dd.MM.yyyy HH:mm:ss}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton2" runat="server" Text="Von" CommandName="SortVon"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Eval("I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang">
                                                <ItemTemplate>                                                    
                                                    <table class="NoDistance">
                                                        <tr>
                                                            <td style="white-space:nowrap;">
                                                                <img id="Img2" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                                    onprerender="img_prerender" value='<%# Eval("I_STATUS")%>'
                                                                    visible='<%# Eval("DEBUG")%>'/>
                                                                <img id="Img1" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                                    onprerender="img2_prerender" value='<%# Eval("I_STATUSE")%>' />
                                                            </td>
                                                            <td width="100%">
                                                                <asp:Label ID="Label6" runat="server" Text='<%# Eval("I_BETREFF")%>'
                                                                    style="white-space:normal;" Visible='<%# (bool)Eval("I_HASLANGTEXT") == false %>'></asp:Label>
                                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# Eval("Rowindex") %>'
                                                                    Text='<%# Eval("I_BETREFF")%>' Visible='<%# Eval("I_HASLANGTEXT") %>' 
                                                                    style="white-space:normal; color: #595959;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div id="Div1" runat="server" onprerender="DivRender" value='<%# Eval("I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# Eval("I_ANTW")%>'>
                                                                        <asp:Image ID="Image1"  ImageUrl="/PortalZLD/Images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton5" runat="server" CommandName="ReadAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# Eval("I_READ")%>'>
                                                                        <asp:Image ID="Image2" runat="server" ImageUrl="/PortalZLD/Images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton6" runat="server" CommandName="ErlAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# Eval("I_ERL")%>'>
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="/PortalZLD/Images/haken_gruen.gif" Width="16px"
                                                                            Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label ID="Label7" runat="server" Text='<%# "Bis: " + Eval("I_ZERLDAT") %>'
                                                                        Visible='<%# (Eval("I_ZERLDAT") != DBNull.Value) && (Eval("I_ZERLDAT").ToString().Length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvProtokollFiliale" runat="server" 
                                        AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" 
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" 
                                        Style="border-collapse: collapse ! important;" 
                                        onrowcommand="gvProtokollFiliale_RowCommand" 
                                        onsorting="gvProtokollFiliale_Sorting">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle DetailTable" />
                                        <Columns>
                                            <asp:BoundField DataField="Rowindex" Visible="false" />
                                            <asp:BoundField DataField="I_VORGID" Visible="false" />
                                            <asp:BoundField DataField="I_LFDNR" Visible="false" />
                                            <asp:TemplateField HeaderText="Datum">
                                               <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton7" runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("I_DATUM","{0:dd.MM.yyyy HH:mm:ss}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton8" runat="server" Text="Von" CommandName="SortVon"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label9" runat="server" Text='<%# Eval("I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang">
                                                <ItemTemplate>  
                                                    <img id="Img2" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px; text-align:center;" runat="server"
                                                        onprerender="img_prerender" value='<%# Eval("I_STATUS")%>' 
                                                        visible='<%# Eval("DEBUG")%>'/>                                            
                                                    <img id="Img3" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# Eval("I_STATUSE")%>' />
                                                    <asp:Label ID="Label10" runat="server" Text='<%# Eval("I_BETREFF")%>'
                                                        Visible='<%# (bool)Eval("I_HASLANGTEXT") == false %>' style="white-space:normal;"></asp:Label>
                                                    <asp:LinkButton ID="LinkButton9" runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# Eval("Rowindex") %>'
                                                        Text='<%# Eval("I_BETREFF")%>' Visible='<%# Eval("I_HASLANGTEXT") %>' style="white-space:normal; color: #595959;" />
                                                    <div id="Div2" runat="server" onprerender="DivRender" value='<%# Eval("I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton ID="LinkButton10" runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# Eval("I_ANTW")%>'>
                                                             <asp:Image ID="Image3" ImageUrl="/PortalZLD/Images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton11" runat="server" CommandName="ReadAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# Eval("I_READ")%>'>
                                                            <asp:Image ID="Image4" runat="server" ImageUrl="/PortalZLD/Images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton12" runat="server" CommandName="ErlAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# Eval("I_ERL")%>'>
                                                            <asp:Image ID="Image5" runat="server" ImageUrl="/PortalZLD/Images/haken_gruen.gif" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label ID="Label11" runat="server" Text='<%# "Bis: " + Eval("I_ZERLDAT") %>'
                                                                        Visible='<%# (Eval("I_ZERLDAT") != DBNull.Value) && (Eval("I_ZERLDAT").ToString().Length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ausgang/Antwort">
                                                <ItemTemplate>
                                                    <img id="Img4" alt="O_STATUS" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# Eval("O_STATUS")%>' 
                                                        visible='<%# Eval("DEBUG")%>'/>
                                                    <img id="Img5" alt="O_STATUSE" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;"
                                                        runat="server" onprerender="img2_prerender" value='<%# Eval("O_STATUSE")%>' />
                                                    <asp:Label ID="Label12" runat="server" Text='<%# Eval("O_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# (bool)Eval("O_HASLANGTEXT") == false %>'></asp:Label>
                                                    <asp:LinkButton ID="LinkButton13" runat="server" CommandName="ReadAnswerText" CommandArgument='<%# Eval("Rowindex")%>' style="white-space:normal; color: #595959;"
                                                        Text='<%# Eval("O_BETREFF")%>' Visible='<%# Eval("O_HASLANGTEXT")%>'></asp:LinkButton>
                                                    <div id="Div3" runat="server" onprerender="DivRender" value='<%# Eval("O_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label ID="Label13" runat="server" Text='<%# "Bis: " + Eval("O_ZERLDAT") %>'
                                                                        Visible='<%# (Eval("O_ZERLDAT") != DBNull.Value) && (Eval("O_ZERLDAT").ToString().Length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton14"  runat="server" Text="An" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label14"  runat="server" Text='<%# Eval("O_AN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Datum">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton15" runat="server" Text="Datum" CommandName="DatumAusgangSort"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label15" runat="server" Text='<%# Eval("O_DATUM","{0:dd.MM.yyyy HH:mm:ss}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvProtokollGL" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" 
                                        Style="border-collapse: collapse ! important;" 
                                        onrowcommand="gvProtokollGL_RowCommand" onsorting="gvProtokollGL_Sorting">
                                        <PagerSettings Visible="false" />
                                        <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                        <AlternatingRowStyle CssClass="GridTableAlternate" />
                                        <RowStyle CssClass="ItemStyle DetailTable" />
                                        <Columns>
                                            <asp:BoundField DataField="Rowindex" Visible="false" />
                                            <asp:BoundField DataField="I_VORGID" Visible="false" />
                                            <asp:BoundField DataField="I_LFDNR" Visible="false" />
                                            <asp:TemplateField HeaderText="Datum">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton16" runat="server" Text="Datum" CommandName="DatumAusgangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label16"  runat="server" Text='<%# Eval("O_DATUM","{0:dd.MM.yyyy HH:mm:ss}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton17" runat="server" Text="An" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="Label17" runat="server" Text='<%# Eval("O_AN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="LinkButton18" runat="server" Text="Status" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <img id="Img6" alt="" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# Eval("O_STATUS")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Ausgang">
                                                <ItemTemplate>
                                                    <img id="Img7" alt="" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# Eval("O_STATUS")%>' 
                                                        visible='<%# Eval("DEBUG")%>'/>
                                                    <img id="Img8" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# Eval("O_STATUSE")%>' />
                                                    <asp:Label ID="Label18" runat="server" Text='<%# Eval("O_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# (bool)Eval("O_HASLANGTEXT") == false %>'></asp:Label>
                                                    <asp:LinkButton ID="LinkButton19" runat="server" CommandName="ReadAnswerText" CommandArgument='<%# Eval("Rowindex")%>' style="white-space:normal; color: #595959;"
                                                        Text='<%# Eval("O_BETREFF")%>' Visible='<%# Eval("O_HASLANGTEXT")%>'></asp:LinkButton>
                                                    <div id="Div4" runat="server" onprerender="DivRender" value='<%# Eval("O_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton ID="LinkButton20" runat="server" CommandName="LoeAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="löschen" Visible='<%# Eval("O_LOE")%>' title="löschen">
                                                                <asp:Image ID="Image6" runat="server" ImageUrl="/PortalZLD/Images/bin_closed.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton21" runat="server" CommandName="CloseAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="Vorgang schließen" Visible='<%# Eval("O_ClO")%>' title="schließen">
                                                            <asp:Image ID="Image7" runat="server" ImageUrl="/PortalZLD/Images/Lock.png" Width="16px"
                                                                Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label ID="Label19" runat="server" Text='<%# "Bis: " + Eval("O_ZERLDAT") %>'
                                                                        Visible='<%# (Eval("O_ZERLDAT") != DBNull.Value) && (Eval("O_ZERLDAT").ToString().Length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang/Antwort">
                                                <ItemTemplate>
                                                    <img id="Img9" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img_prerender" value='<%# Eval("I_STATUS")%>' 
                                                        visible='<%# Eval("DEBUG")%>'/>
                                                    <img id="Img10" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# Eval("I_STATUSE")%>' />
                                                    <asp:Label ID="Label20" runat="server" Text='<%# Eval("I_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# (bool)Eval("I_HASLANGTEXT") == false %>'></asp:Label>
                                                    <asp:LinkButton ID="LinkButton22" runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# Eval("Rowindex") %>' style="white-space:normal; color: #595959;"
                                                        Text='<%# Eval("I_BETREFF")%>' Visible='<%# Eval("I_HASLANGTEXT") %>' />
                                                    <div id="Div5" runat="server" onprerender="DivRender" value='<%# Eval("I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton ID="LinkButton23" runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# Eval("I_ANTW")%>'>
                                                             <asp:Image ID="Image8"  ImageUrl="/PortalZLD/Images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# Eval("I_READ")%>'>
                                                            <asp:Image runat="server" ImageUrl="/PortalZLD/Images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="ErlAufgabe" CommandArgument='<%# Eval("Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# Eval("I_ERL")%>'>
                                                            <asp:Image runat="server" ImageUrl="/PortalZLD/Images/haken_gruen.gif" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " + Eval("I_ZERLDAT") %>'
                                                                        Visible='<%# (Eval("I_ZERLDAT") != DBNull.Value) && (Eval("I_ZERLDAT").ToString().Length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Von" CommandName="SortVon"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# Eval("I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Datum">
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label  runat="server" Text='<%# Eval("I_DATUM","{0:dd.MM.yyyy HH:mm:ss}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <div id="EditAufgabe2" runat="server" class="dataQueryFooter" style="text-align: right;">
                                        <asp:LinkButton ID="lbtAdd" runat="server" Text="hinzufügen" CssClass="Tablebutton"
                                            Height="16px" Width="78px" OnClick="lbtAdd_Click"/>
                                    </div>                                    
                                    <div>
                                        &nbsp;
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="dataFooter" class="dataQueryFooter">
                    </div>
                    <div>
                        <asp:Button runat="server" ID="btnTarget" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeLangtext" runat="server" PopupControlID="PanelLangtext"
                            TargetControlID="btnTarget" CancelControlID="ibtnCancel" RepositionMode="None"
                            BackgroundCssClass="divProgress">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel runat="server" ID="PanelLangtext" Width="400px" Height="300px" Style="border: solid 1px #eeeeee;
                            background-color: white;">
                            <div style="margin: 5px 3px 10px 3px; font-size: 12px; font-weight: bold; height: 12px;">
                                <asp:Label runat="server" ID="lblBetreff" style="color:#0066cc;"></asp:Label>
                                <asp:TextBox ID="txtBetreff" runat="server" Width="388px" TextMode="SingleLine" Wrap="false" MaxLength="50"
                                    Visible="false"></asp:TextBox>
                            </div>
                            <div id="divText" runat="server" style="margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 230px; border: solid 1px #dddddd;">
                                <asp:Label runat="server" ID="lblText"></asp:Label>
                                <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Visible="false" Width="388px"
                                    Height="230px" style="border:solid 1px #dddddd; color:#595959;"></asp:TextBox>
                            </div>
                            <div style="text-align: right; height: 20px; margin: 10px 10px 5px 3px; white-space: nowrap;">
                                <table>
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:Label ID="lblErrorLangtext" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="ibtnOK" Text="OK" ImageAlign="Middle" ImageUrl="/PortalZLD/Images/haken_gruen.gif"
                                                Style="margin-right: 5px; margin-left: 5px; width: 16px; height: 16px;" OnClick="ibtnOK_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="ibtnCancel" Text="Abbruch" ImageAlign="Middle"
                                                ImageUrl="/PortalZLD/Images/cancel.png" Style="width: 16px; height: 16px;" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label ID="lblVorgangsID" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblLFDNR" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblAn" runat="server" Visible="false"></asp:Label>
                                <asp:Label ID="lblRowIndex" runat="server" Visible="false"></asp:Label>
                                <asp:CheckBox ID="chkEdit" runat="server" Visible="false"/>
                                <asp:CheckBox ID="chkIsRückfrage" runat="server" Visible="false"/>
                            </div>
                        </asp:Panel>
                        <asp:Button runat="server" ID="btnTarget2" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeNeuerText" runat="server" PopupControlID="PanelNeuerText"
                            TargetControlID="btnTarget2" CancelControlID="ibtnCancelNew" RepositionMode="None"
                            BackgroundCssClass="divProgress">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel runat="server" ID="PanelNeuerText" Width="400px" Height="300px" Style="border: solid 1px #eeeeee;
                            background-color: white;">
                            <div style="margin: 5px 3px 10px 3px; height: 12px;">
                                <asp:Label runat="server" Text="Betreff:" width="40px"  style="margin-left:1px;"></asp:Label>
                                <asp:TextBox ID="txtNewBetreff" runat="server" Width="340px" TextMode="SingleLine" MaxLength="50"
                                    Wrap="false" style="margin-left:3px; border:solid 1px #dddddd;"></asp:TextBox>
                            </div>
                            <div style="margin: 5px 3px 10px 3px; height: 12px;">
                                <asp:Label runat="server" Text="Bis:" width="40px" style="margin-left:1px;"></asp:Label>
                                <asp:TextBox ID="txtZuerledigenBis" runat="server" Width="80px" style="margin-left:3px;border:solid 1px #dddddd;"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="cetxtZuerledigenBis" runat="server" TargetControlID="txtZuerledigenBis">
                                </ajaxToolkit:CalendarExtender>
                                <ajaxToolkit:MaskedEditExtender ID="meetxtZuerledigenBis" runat="server" TargetControlID="txtZuerledigenBis"
                                    MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                </ajaxToolkit:MaskedEditExtender>                                
                                <asp:Label runat="server" Text="Vorgangsart:" style="margin-left:3px;"></asp:Label>
                                <asp:DropDownList ID="ddlVorgangsarten" runat="server" Style="margin-left: 3px; width:180px; border:solid 1px #dddddd;">
                                </asp:DropDownList>
                            </div>
                            <div style="margin: 0px 3px 10px 3px; font-size: 10px; overflow: visible; height: 210px;">
                                <asp:TextBox ID="txtNewText" runat="server" TextMode="MultiLine" Width="388px" Height="210px" style="border: solid 1px #dddddd;"></asp:TextBox>
                            </div>
                            <div style="text-align: right; height: 20px; margin: 10px 10px 5px 3px; white-space: nowrap;">
                                <table>
                                    <tr>
                                        <td style="text-align: left;" width="100%">
                                            <asp:Label ID="lblErrorNewText" runat="server" ForeColor="Red"></asp:Label>
                                            <ajaxToolkit:MaskedEditValidator ID="mevtxtZuerledigenBis" runat="server" ControlExtender="meetxtZuerledigenBis"
                                    ControlToValidate="txtZuerledigenBis" MaximumValue="31.12.2050" MinimumValue="01.01.2012" 
                                    MaximumValueMessage="Datum liegt zu weit in der Zukunft!" MinimumValueMessage="Datum liegt in der Vergangenheit!"
                                    InvalidValueMessage="ung&uuml;ltiger Wert" Display="Dynamic" EmptyValueMessage="Geben Sie ein Datum ein!"
                                    ValidationExpression="(([0-2]\d)|(3[0,1]))\.((0\d)|(1[0-2]))\.(2\d{3})"></ajaxToolkit:MaskedEditValidator>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="ibtnOkNew" Text="OK" ImageAlign="Middle" ImageUrl="/PortalZLD/Images/haken_gruen.gif"
                                                Style="margin-right: 5px; margin-left: 5px; width: 16px; height: 16px;" OnClick="ibtnOkNew_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="ibtnCancelNew" Text="Abbruch" ImageAlign="Middle"
                                                ImageUrl="/PortalZLD/Images/cancel.png" Style="width: 16px; height: 16px;" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
