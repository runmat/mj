<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerbBuchReport.aspx.vb" Inherits="KBS.VerbBuchReport"
    MasterPageFile="~/KBS.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div id="site">
            <div id="content">
                <div id="navigationSubmenu">
                    <asp:LinkButton ID="lb_zurueck" CausesValidation="false" runat="server" Visible="True">zurück</asp:LinkButton>
                </div>
                <div id="innerContent">
                    <div id="innerContentRight" style="width: 100%;">
                        <div id="innerContentRightHeading">
                            <h1>
                                <asp:Label ID="lblHead" runat="server" Text="Verbandbucheinträge"></asp:Label>&nbsp;
                                <asp:Label ID="lblPageTitle" Text="" runat="server"></asp:Label>
                            </h1>
                        </div>
                        <div id="TableQuery">
                            <div id="statistics"  style="height: 20px; margin-top: 0px; padding-top: 10px">
                                <asp:Label ID="lblVkbur" runat="server" Style="margin-left: 10px"></asp:Label>
                                <asp:ImageButton align="right" ID="ImageButton1" runat="server" ImageUrl="../Images/iconPDF.gif" OnClick="OnClick" style="margin-right: 10px"/>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="upWareneingang">
                            <ContentTemplate>
                                <div id="data">
                                    <table cellspacing="0" width="100%" cellpadding="0" bgcolor="white" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblError" runat="server" Visible="true" CssClass="TextError" EnableViewState="False"></asp:Label>
                                                <asp:Label ID="lblNoData" runat="server" Visible="true" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView CssClass="GridView" ID="GridView1" runat="server" PageSize="50" Width="100%"
                                                    AutoGenerateColumns="False" AllowPaging="false" AllowSorting="True" ShowFooter="False"
                                                    GridLines="None">
                                                    <PagerSettings Visible="false" />
                                                    <HeaderStyle CssClass="GridTableHead"></HeaderStyle>
                                                    <AlternatingRowStyle CssClass="GridTableAlternate" />
                                                    <RowStyle CssClass="ItemStyle" />
                                                    <Columns>
                                                    <asp:TemplateField Visible="false">

                                                    </asp:TemplateField>
                                                     <asp:BoundField HeaderText="ID" DataField="ID" SortExpression="ID" />
                                                     <asp:BoundField HeaderText="Verletzter" DataField="NAME_VERL" SortExpression="NAME_VERL" />
                                                     <asp:BoundField HeaderText="Zeit Unfall" DataField="DATUM_UNF" SortExpression="DATUM_UNF" />
                                                     <asp:BoundField HeaderText="Ort" DataField="NAME_VERL" SortExpression="NAME_VERL" />
                                                     <asp:BoundField HeaderText="Hergang" DataField="HERGANG" SortExpression="HERGANG" />
                                                     <asp:BoundField HeaderText="Zeugen" DataField="NAME_ZEUG" SortExpression="NAME_ZEUG" />
                                                     <asp:BoundField HeaderText="Zeit Erste Hilfe" DataField="DATUM_HILF" SortExpression="DATUM_HILF" />
                                                     <asp:BoundField HeaderText="Hilfeleistung" DataField="ART_HILF" SortExpression="ART_HILF" />
                                                     <asp:BoundField HeaderText="Name der Helfer" DataField="NAME_HELFER" SortExpression="NAME_HELFER" />
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div id="dataFooter">
                            &nbsp;
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
