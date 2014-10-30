<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Report11.aspx.vb" Inherits="AppF2.Report11"
    MasterPageFile="../MasterPage/AppMaster.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="site">
        <div id="content">
            <div id="navigationSubmenu">
                <asp:LinkButton ID="lbBack" Style="padding-left: 15px" runat="server" class="firstLeft active"
                    Text="zurück"></asp:LinkButton>
            </div>
            <div id="innerContent">
                <div id="innerContentRight" style="width: 100%">
                    <div id="innerContentRightHeading">
                        <h1>
                            <asp:Label ID="lblHead" runat="server" Text="Label"></asp:Label>
                        </h1>
                    </div>
                    <div id="paginationQuery">
                        <table cellpadding="0" cellspacing="0">
                            <tbody>
                                <tr>
                                    <td class="active">
                                        Laden Sie hier Ihre Dateien herunter:
                                    </td>
                                    <td align="right">
                                        <div id="queryImage">
                                            &nbsp;
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <asp:Panel ID="Panel1" runat="server">
                        <div id="TableQuery" style="margin-bottom: 10px">
                            <table id="tab1" runat="server" cellpadding="0" cellspacing="0" width="100%">
                                <tbody>
                                    <tr class="formquery">
                                        <td class="firstLeft active" colspan="3">
                                            <asp:Label ID="lblError" runat="server" CssClass="TextError" Font-Bold="True" EnableViewState="False" />
                                        </td>
                                    </tr>
                                    <tr class="formquery" style="padding-bottom:20px;">
                                        <td class="firstLeft active" style="white-space:nowrap;vertical-align:middle">
                                            <img src="../../../Images/Blatt_08.jpg" alt="CSV-Datei herunterladen" />
                                        </td>
                                        <td style="white-space:nowrap;vertical-align:middle" >
                                            &nbsp; Datei-Download ASF:
                                        </td>
                                        <td class="firstLeft active" style="width: 100%; padding-left: 10px; vertical-align: middle">
                                            <asp:LinkButton ID="lbtDownloadASF" runat="server" Visible="false" 
                                                Text="Controllingdatei ASF" style="font-size:12px;font-family:Verdana;text-decoration:underline;font-weight:bold;" />
                                            <asp:Label ID="lblErrorASF" runat="server" CssClass="TextError" EnableViewState="False" />
                                        </td>
                                    </tr>
                                    <tr class="formquery" style="padding-bottom:20px;">
                                        <td class="firstLeft active" style="white-space:nowrap;vertical-align:middle">
                                            <img src="../../../Images/Blatt_08.jpg" alt="CSV-Datei herunterladen" />
                                        </td>
                                        <td style="white-space:nowrap;vertical-align:middle" >
                                            &nbsp; Datei-Download EKF:
                                        </td>
                                        <td class="firstLeft active" style="width: 100%; padding-left: 10px; vertical-align: middle">
                                            <asp:LinkButton ID="lbtDownloadEKF" runat="server" Visible="false" 
                                                Text="Controllingdatei EKF" style="font-size:12px;font-family:Verdana;text-decoration:underline;font-weight:bold;" />
                                            <asp:Label ID="lblErrorEKF" runat="server" CssClass="TextError" EnableViewState="False" />
                                        </td>
                                    </tr>
                                    <tr class="formquery" style="padding-bottom:20px;">
                                        <td class="firstLeft active" style="white-space:nowrap;vertical-align:middle">
                                            <img src="../../../Images/Blatt_08.jpg" alt="CSV-Datei herunterladen" />
                                        </td>
                                        <td style="white-space:nowrap;vertical-align:middle" >
                                            &nbsp; Datei-Download OAK:
                                        </td>
                                        <td class="firstLeft active" style="width: 100%; padding-left: 10px; vertical-align: middle">
                                            <asp:LinkButton ID="lbtDownloadOAK" runat="server" Visible="false" 
                                                Text="Controllingdatei OAK" style="font-size:12px;font-family:Verdana;text-decoration:underline;font-weight:bold;" />
                                            <asp:Label ID="lblErrorOAK" runat="server" CssClass="TextError" EnableViewState="False" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <div id="Queryfooter" runat="server" style="background-color: #dfdfdf; height: 22px;">
                                &nbsp;
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="dataQueryFooter">
                    </div>
                    <div id="Result" runat="Server" visible="false">
                    </div>
                    <div id="dataFooter">
                        &nbsp;
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
