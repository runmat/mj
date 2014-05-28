<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Filialbuch.aspx.vb" Inherits="KBS.Filialbuch"
    MasterPageFile="../KBS.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript" src="/PortalZLD/Applications/AppZulassungsdienst/JavaScript/Helper.js"></script>
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
                    Text="Zur&uuml;ck"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Filial-Logbuch"></asp:Label>
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
                        <table runat="server" cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr class="formquery">
                                    <td style="white-space: nowrap;">
                                        <asp:LinkButton ID="lbAufgaben" runat="server" CssClass="TabButtonBig" Width="136px">Aufgaben</asp:LinkButton>
                                        <asp:LinkButton ID="lbProtkoll" runat="server" CssClass="TabButtonBig Active" Width="136px"
                                            Style="margin-left: 5px;">Protokoll</asp:LinkButton>
                                    </td>
                                    <td class="firstLeft active" width="100%">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div id="dataHeader" style="background-image: url(../Images/overflow.png); color: #ffffff;
                        font-weight: bold; float: left; height: 22px; line-height: 22px; width: 892px;
                        white-space: nowrap; background-color: #2B4C91; padding-left: 15px;">
                        <table cellpadding="0px" cellspacing="0px" style="padding-right:10px;">
                            <tr>
                                <td style="text-align: left;">
                                    <asp:Label ID="lblBedienernummer" runat="server"></asp:Label>
                                    <asp:Label ID="lblUser" runat="server"></asp:Label>
                                </td>
                                <td style="text-align: right;" width="100%">
                                    KST.:
                                    <asp:Label ID="lblKostenstelle" runat="server"></asp:Label>
                                    / LFB-Gebiet:
                                    <asp:Label ID="lblLFB" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="CardScann" style="border-bottom: solid 1px #DFDFDF; border-right: solid 1px #DFDFDF;
                        border-left: solid 1px #DFDFDF;">
                        <asp:UpdatePanel runat="server" ID="upGrid">
                            <ContentTemplate>
                                <table id="tblBedienerkarte" runat="server" cellspacing="0" width="100%" cellpadding="0"
                                    bgcolor="white" border="0">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td align="center" class="firstLeft active">
                                            <asp:Label ID="lblBedienError" runat="server" CssClass="TextError">
                                                    Bitte Scannen Sie ihre Bedienerkarte.</asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td id="Usercard" runat="server" align="center" class="firstLeft" style="padding-top: 10px;
                                            padding-bottom: 5px;">
                                            <asp:TextBox ID="txtBedienerkarte" Width="240px" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="formquery">
                                        <td>
                                            <input id="SendTopSap" type="hidden" runat="server" />&nbsp;
                                        </td>
                                    </tr>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                                    <div id="Timespan" style="text-align: left; padding: 3px 0px 8px 5px; border-right: solid 1px gray;
                                        border-left: solid 1px #595959; border-bottom: solid 1px #595959;" runat="server">
                                        <asp:Label runat="server">Von:</asp:Label>
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
                                        <asp:Label runat="server" Style="padding-left: 5px;">Bis:</asp:Label>
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
                                        <asp:Label runat="server" Text="Filter:"></asp:Label>
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
                                            height: 16px;">
                                                            <img src="../images/arrow_refresh.png" style="width: 16px; height: 16px; vertical-align:middle;" alt="Refresh" title="Aktualisieren" />
                                        </asp:LinkButton>
                                    </div>
                                    <asp:GridView ID="gvAufgabenFiliale" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" 
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" Style="border-collapse: collapse ! important;">
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
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERDAT","{0:d}") %>'></asp:Label><br />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERZEIT","{0:T}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Von" CommandName="SortVon"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang">
                                                <ItemTemplate>                                                    
                                                    <table class="NoDistance">
                                                        <tr>
                                                            <td style="white-space:nowrap;">
                                                                <img id="Img2" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                                    onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUS")%>'
                                                                    visible='<%# DataBinder.Eval(Container.DataItem,"DEBUG")%>'/>
                                                                <img alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                                    onprerender="img2_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUSE")%>' />
                                                            </td>
                                                            <td width="100%">
                                                                <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>'
                                                                    style="white-space:normal;" Visible='<%# Not DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>'></asp:Label>
                                                                <asp:LinkButton runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex") %>'
                                                                    Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>' Visible='<%# DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>' 
                                                                    style="white-space:normal;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <div runat="server" onprerender="DivRender" value='<%# DataBinder.Eval(Container.DataItem,"I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ANTW")%>'>
                                                                        <asp:Image  ImageUrl="../images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# DataBinder.Eval(Container.DataItem,"I_READ")%>'>
                                                                        <asp:Image runat="server" ImageUrl="../images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ErlAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ERL")%>'>
                                                                        <asp:Image ID="Image3" runat="server" ImageUrl="../images/haken_gruen.gif" Width="16px"
                                                                            Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " & Databinder.Eval(Container.DataItem,"I_ZERLDAT") %>'
                                                                        Visible='<%# (Not Typeof Databinder.Eval(Container.DataItem,"I_ZERLDAT") is DBNull) AND (Databinder.Eval(Container.DataItem,"I_ZERLDAT").ToString.length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvAllFiliale" runat="server" AutoGenerateColumns="false" AllowPaging="false" AllowSorting="true" 
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" Style="border-collapse: collapse ! important;">
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
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERDAT","{0:d}") %>'></asp:Label><br />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERZEIT","{0:T}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Von" CommandName="SortVon"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang">
                                                <ItemTemplate>  
                                                    <img id="Img1" alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px; text-align:center;" runat="server"
                                                        onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUS")%>' 
                                                        visible='<%# DataBinder.Eval(Container.DataItem,"DEBUG")%>'/>                                            
                                                    <img alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUSE")%>' />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>'
                                                        Visible='<%# Not DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>' style="white-space:normal;"></asp:Label>
                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex") %>'
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>' Visible='<%# DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>' style="white-space:normal;" />
                                                    <div runat="server" onprerender="DivRender" value='<%# DataBinder.Eval(Container.DataItem,"I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ANTW")%>'>
                                                             <asp:Image ImageUrl="../images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# DataBinder.Eval(Container.DataItem,"I_READ")%>'>
                                                            <asp:Image runat="server" ImageUrl="../images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ErlAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ERL")%>'>
                                                            <asp:Image runat="server" ImageUrl="../images/haken_gruen.gif" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " & Databinder.Eval(Container.DataItem,"I_ZERLDAT") %>'
                                                                        Visible='<%# (Not Typeof Databinder.Eval(Container.DataItem,"I_ZERLDAT") is DBNull) AND (Databinder.Eval(Container.DataItem,"I_ZERLDAT").ToString.length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ausgang/Antwort">
                                                <ItemTemplate>
                                                    <img alt="O_STATUS" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"O_STATUS")%>' 
                                                        visible='<%# DataBinder.Eval(Container.DataItem,"DEBUG")%>'/>
                                                    <img alt="O_STATUSE" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;"
                                                        runat="server" onprerender="img2_prerender" value='<%# DataBinder.Eval(Container.DataItem,"O_STATUSE")%>' />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# Not DataBinder.Eval(Container.DataItem,"O_HASLANGTEXT") %>'></asp:Label>
                                                    <asp:LinkButton runat="server" CommandName="ReadAnswerText" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>' style="white-space:normal;"
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"O_BETREFF")%>' Visible='<%# DataBinder.Eval(Container.DataItem,"O_HASLANGTEXT")%>'></asp:LinkButton>
                                                    <div runat="server" onprerender="DivRender" value='<%# DataBinder.Eval(Container.DataItem,"O_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " & Databinder.Eval(Container.DataItem,"O_ZERLDAT") %>'
                                                                        Visible='<%# (Not Typeof Databinder.Eval(Container.DataItem,"O_ZERLDAT") is DBNull) AND (Databinder.Eval(Container.DataItem,"O_ZERLDAT").ToString.length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton  runat="server" Text="An" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_AN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Datum">
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumAusgangSort"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_ERDAT")%>'></asp:Label><br />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_ERZEIT")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvAllLFB" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                        Width="100%" ShowFooter="False" GridLines="Vertical" Visible="true" Style="border-collapse: collapse ! important;">
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
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumAusgangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_ERDAT")%>'></asp:Label><br />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_ERZEIT")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="An" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_AN") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Status" CommandName="SortAn"></asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <img alt="" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"O_STATUS")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Ausgang">
                                                <ItemTemplate>
                                                    <img alt="" src="" height="16" width="16" runat="server" style="padding: 3px 3px 0px 0px;"
                                                        onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"O_STATUS")%>' 
                                                        visible='<%# DataBinder.Eval(Container.DataItem,"DEBUG")%>'/>
                                                    <img alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# DataBinder.Eval(Container.DataItem,"O_STATUSE")%>' />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"O_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# Not DataBinder.Eval(Container.DataItem,"O_HASLANGTEXT") %>'></asp:Label>
                                                    <asp:LinkButton runat="server" CommandName="ReadAnswerText" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>' style="white-space:normal;"
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"O_BETREFF")%>' Visible='<%# DataBinder.Eval(Container.DataItem,"O_HASLANGTEXT")%>'></asp:LinkButton>
                                                    <div runat="server" onprerender="DivRender" value='<%# DataBinder.Eval(Container.DataItem,"O_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton runat="server" CommandName="LoeAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="löschen" Visible='<%# DataBinder.Eval(Container.DataItem,"O_LOE")%>' title="löschen">
                                                                <asp:Image runat="server" ImageUrl="../images/bin_closed.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="CloseAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="Vorgang schließen" Visible='<%# DataBinder.Eval(Container.DataItem,"O_ClO")%>' title="schließen">
                                                            <asp:Image runat="server" ImageUrl="../images/Lock.png" Width="16px"
                                                                Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " & Databinder.Eval(Container.DataItem,"O_ZERLDAT") %>'
                                                                        Visible='<%# (Not Typeof Databinder.Eval(Container.DataItem,"O_ZERLDAT") is DBNull) AND (Databinder.Eval(Container.DataItem,"O_ZERLDAT").ToString.length > 0) %>'></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eingang/Antwort">
                                                <ItemTemplate>
                                                    <img alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUS")%>' 
                                                        visible='<%# DataBinder.Eval(Container.DataItem,"DEBUG")%>'/>
                                                    <img alt="" src="" height="16" width="16" style="padding: 3px 3px 0px 0px;" runat="server"
                                                        onprerender="img2_prerender" value='<%# DataBinder.Eval(Container.DataItem,"I_STATUSE")%>' />
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>' style="white-space:normal;"
                                                        Visible='<%# Not DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>'></asp:Label>
                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabeText" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex") %>' style="white-space:normal;"
                                                        Text='<%# DataBinder.Eval(Container.DataItem,"I_BETREFF")%>' Visible='<%# DataBinder.Eval(Container.DataItem,"I_HASLANGTEXT") %>' />
                                                    <div runat="server" onprerender="DivRender" value='<%# DataBinder.Eval(Container.DataItem,"I_TRENN") %>'>
                                                        <table class="NoDistance">
                                                            <tr>
                                                                <td style="white-space: nowrap;">
                                                                    <asp:LinkButton runat="server" CommandName="AnswerAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="antworten" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ANTW")%>'>
                                                             <asp:Image  ImageUrl="../images/email.png" Width="16px" Height="16px" runat="server"/>
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton runat="server" CommandName="ReadAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="gelesen" Visible='<%# DataBinder.Eval(Container.DataItem,"I_READ")%>'>
                                                            <asp:Image runat="server" ImageUrl="../images/Eye.png" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandName="ErlAufgabe" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"Rowindex")%>'
                                                                        ToolTip="erledigt" Visible='<%# DataBinder.Eval(Container.DataItem,"I_ERL")%>'>
                                                            <asp:Image runat="server" ImageUrl="../images/haken_gruen.gif" Width="16px" Height="16px" />
                                                                    </asp:LinkButton>
                                                                </td>
                                                                <td width="100%" align="right">
                                                                    <asp:Label runat="server" Text='<%# "Bis: " & Databinder.Eval(Container.DataItem,"I_ZERLDAT") %>'
                                                                        Visible='<%# (Not Typeof Databinder.Eval(Container.DataItem,"I_ZERLDAT") is DBNull) AND (Databinder.Eval(Container.DataItem,"I_ZERLDAT").ToString.length > 0) %>'></asp:Label>
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
                                                    <asp:Label runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_VON") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Datum">
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" Text="Datum" CommandName="DatumEingangSort"></asp:LinkButton>
                                               </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERDAT","{0:d}")%>'></asp:Label><br />
                                                    <asp:Label  runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"I_ERZEIT","{0:T}")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <div>
                                        &nbsp;
                                    </div>
                                    <div id="EditAufgabe2" runat="server" class="dataQueryFooter" style="text-align: right;">
                                        <asp:LinkButton ID="lbtFilialbesuch" runat="server" Text="Filialbesuch" CssClass="Tablebutton"
                                            Height="16px" Width="78px" />
                                        <asp:LinkButton ID="lbtAdd" runat="server" Text="hinzuf&uuml;gen" CssClass="Tablebutton"
                                            Height="16px" Width="78px" />
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
                        <asp:Panel runat="server" ID="PanelLangtext" Width="440px" Height="300px" Style="border: solid 1px #eeeeee;
                            background-color: white;">
                            <div style="margin: 0 3px">
                                <div style="margin: 5px 0; font-size: 12px; font-weight: bold; height: 20px;">
                                    <asp:Label runat="server" ID="lblBetreff" style="color:#0066cc;"></asp:Label>
                                    <asp:TextBox ID="txtBetreff" runat="server" Width="98%" TextMode="SingleLine" Wrap="false" MaxLength="50"
                                        Visible="false"></asp:TextBox>
                                </div>
                                <div id="divText" runat="server" style="margin: 5px 0; font-size: 10px; overflow: visible; height: 230px; border: solid 1px #dddddd;">
                                    <asp:Label runat="server" ID="lblText"></asp:Label>
                                    <asp:TextBox ID="txtText" runat="server" TextMode="MultiLine" Visible="false" Width="99%"
                                        Height="230px" style="border:solid 1px #dddddd; color:#595959;"></asp:TextBox>
                                </div>
                                <div style="text-align: right; height: 20px; margin: 5px 0; white-space: nowrap;">
                                    <table>
                                        <tr>
                                            <td style="text-align: left;" width="100%">
                                                <asp:Label ID="lblErrorLangtext" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="ibtnOK" Text="OK" ImageAlign="Middle" ImageUrl="..\images\haken_gruen.gif"
                                                    Style="margin-right: 5px; margin-left: 5px; width: 16px; height: 16px;" OnClick="ibtnOK_Click" />
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="ibtnCancel" Text="Abbruch" ImageAlign="Middle"
                                                    ImageUrl="..\images\cancel.png" Style="width: 16px; height: 16px;" />
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
                            </div>
                        </asp:Panel>
                        <asp:Button runat="server" ID="btnTarget2" Width="0" Height="0" BackColor="Transparent"
                            BorderStyle="none" />
                        <ajaxToolkit:ModalPopupExtender ID="mpeNeuerText" runat="server" PopupControlID="PanelNeuerText"
                            TargetControlID="btnTarget2" CancelControlID="ibtnCancelNew" RepositionMode="None"
                            BackgroundCssClass="divProgress">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel runat="server" ID="PanelNeuerText" Width="440px" Height="300px" Style="border: solid 1px #eeeeee;
                            background-color: white;">
                            <div style="margin: 0 3px">
                                <table width="100%">
                                    <tr style="margin: 5px 0; height: 20px;">
                                        <td style="width: 45px">
                                            <asp:Label runat="server" Text="Betreff:" Width="100%"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNewBetreff" runat="server" Width="99%" TextMode="SingleLine" MaxLength="50"
                                                Wrap="false" style="border:solid 1px #dddddd;"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr style="margin: 5px 0; height: 20px;">
                                        <td style="width: 45px">
                                            <asp:Label runat="server" Text="Bis:" width="100%"></asp:Label>
                                        </td>
                                        <td style="width: 80px">
                                            <asp:TextBox ID="txtZuerledigenBis" runat="server" Width="100%" style="border:solid 1px #dddddd;"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="cetxtZuerledigenBis" runat="server" TargetControlID="txtZuerledigenBis">
                                            </ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="meetxtZuerledigenBis" runat="server" TargetControlID="txtZuerledigenBis"
                                                MaskType="Date" Mask="99/99/9999" CultureName="de-DE">
                                            </ajaxToolkit:MaskedEditExtender> 
                                        </td>
                                        <td style="width: 80px; text-align: right">
                                            <asp:Label runat="server" Text="Vorgangsart:" Width="100%"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlVorgangsarten" runat="server" Width="100%" Style="border:solid 1px #dddddd;">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <div style="margin: 5px 0; font-size: 10px; overflow: visible; height: 210px;">
                                    <asp:TextBox ID="txtNewText" runat="server" TextMode="MultiLine" Width="99%" Height="210px" style="border: solid 1px #dddddd;"></asp:TextBox>
                                </div>
                                <div style="text-align: right; height: 20px; margin: 5px 0; white-space: nowrap;">
                                    <table width="100%">
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
                                                <asp:ImageButton runat="server" ID="ibtnOkNew" Text="OK" ImageAlign="Middle" ImageUrl="..\images\haken_gruen.gif"
                                                    Style="margin-right: 5px; margin-left: 5px; width: 16px; height: 16px;" OnClick="ibtnOkNew_Click" />
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="ibtnCancelNew" Text="Abbruch" ImageAlign="Middle"
                                                    ImageUrl="..\images\cancel.png" Style="width: 16px; height: 16px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
