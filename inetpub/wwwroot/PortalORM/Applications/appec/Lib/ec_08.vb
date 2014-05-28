Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_08
    REM § Lese-/Schreibfunktion, Kunde: SIXT, 
    REM § Show - BAPI: Z_M_ABMBEREIT_LAUFZEIT,
    REM § Change - BAPI: Z_M_ABMBEREIT_LAUFAEN.

    Inherits Base.Business.BankBase ' FDD_Bank_Base

#Region " Declarations"
    Private m_tblFahrzeuge As DataTable
#End Region

#Region " Properties"
    Public Property Fahrzeuge() As DataTable
        Get
            Return m_tblFahrzeuge
        End Get
        Set(ByVal Value As DataTable)
            m_tblFahrzeuge = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overrides Sub change()

    End Sub


    Public Overrides Sub Show()
        m_strClassAndMethod = "ec_08.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            If m_objLogApp Is Nothing Then
                m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
            End If
            m_intIDSAP = -1

            m_tblFahrzeuge = New DataTable()
            With m_tblFahrzeuge.Columns
                .Add("Pdi", System.Type.GetType("System.String"))
                .Add("Equipmentnummer", System.Type.GetType("System.String"))
                .Add("Fahrgestellnummer", System.Type.GetType("System.String"))
                .Add("Briefnummer", System.Type.GetType("System.String"))
                .Add("Kennzeichen", System.Type.GetType("System.String"))
                .Add("Abmeldedatum", System.Type.GetType("System.DateTime"))
                .Add("ActionNOTHING", System.Type.GetType("System.Boolean"))
                .Add("ActionDELE", System.Type.GetType("System.Boolean"))
                .Add("Bemerkung", System.Type.GetType("System.String"))
                .Add("Action", System.Type.GetType("System.String"))
                .Add("Belegnr", System.Type.GetType("System.String"))
                .Add("Hersteller", System.Type.GetType("System.String"))
                .Add("Laufzeit", System.Type.GetType("System.String"))
                .Add("Zulassungsdatum", System.Type.GetType("System.DateTime"))
                .Add("Modellbezeichnung", String.Empty.GetType)
            End With



            m_intStatus = 0
            m_strMessage = ""
            'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
            'con.Open()
            Try
                Dim CarDetailTable As New DataTable
                'Dim cmd As New SAPCommand()
                'cmd.Connection = con

                'Dim strCom As String

                'strCom = "EXEC Z_M_Abmbereit_Laufzeit "
                'strCom = strCom & "@KUNNR=@pI_KUNNR,@AUSGABE=@pE_AUSGABE OUTPUT OPTION 'disabledatavalidation'"

                'cmd.CommandText = strCom


                'importparameter

                'Dim pI_KUNNR As New SAPParameter("@pI_KUNNR", ParameterDirection.Input)


                'exportParameter
                'Dim pE_AUSGABE As New SAPParameter("@pE_AUSGABE", ParameterDirection.Output)

                'Importparameter hinzufügen
                'cmd.Parameters.Add(pI_KUNNR)

                'exportparameter hinzugfügen
                'cmd.Parameters.Add(pE_AUSGABE)


                'befüllen der Importparameter
                'pI_KUNNR.Value = Right("0000000000" & m_objUser.KUNNR, 10)




                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abmbereit_Laufzeit", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)

                S.AP.InitExecute("Z_M_Abmbereit_Laufzeit", "KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                'cmd.ExecuteNonQuery()

                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, True)
                End If

                'If Not pE_AUSGABE.Value Is Nothing Then
                '    CarDetailTable = DirectCast(pE_AUSGABE.Value, DataTable)
                '    HelpProcedures.killAllDBNullValuesInDataTable(CarDetailTable)
                'End If

                CarDetailTable = S.AP.GetExportTable("AUSGABE")

                If CarDetailTable.Rows.Count > 0 Then
                    Dim rowNew As DataRow
                    For Each tmpRow As DataRow In CarDetailTable.Rows
                        rowNew = m_tblFahrzeuge.NewRow
                        rowNew("Pdi") = tmpRow("Dadpdi")
                        rowNew("Belegnr") = tmpRow("Kunpdi")
                        rowNew("Equipmentnummer") = tmpRow("Equnr")
                        rowNew("Fahrgestellnummer") = tmpRow("Zzfahrg")
                        rowNew("Briefnummer") = tmpRow("Zzbrief")
                        rowNew("Kennzeichen") = tmpRow("Zzkenn")
                        Dim datTemp As DateTime
                        If Not tmpRow("Repla_Date") Is Nothing AndAlso IsNumeric(tmpRow("Repla_Date")) Then
                            datTemp = MakeDateStandard(tmpRow("Repla_Date").ToString)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Abmeldedatum") = datTemp
                            End If
                        End If

                        rowNew("Modellbezeichnung") = tmpRow("Zzbezei")
                        datTemp = Nothing
                        rowNew("Bemerkung") = ""
                        rowNew("Action") = ""
                        rowNew("ActionNOTHING") = True
                        rowNew("ActionDELE") = False
                        rowNew("Hersteller") = tmpRow("Zzherst_Text")

                        If Not (tmpRow("Vdatu") Is Nothing) AndAlso IsNumeric(tmpRow("Vdatu")) Then
                            datTemp = MakeDateStandard(tmpRow("Vdatu").ToString)
                            If datTemp > CDate("01.01.1900") Then
                                rowNew("Zulassungsdatum") = datTemp
                            End If
                        End If
                        rowNew("Laufzeit") = tmpRow("ZZLAUFZEIT")

                        m_tblFahrzeuge.Rows.Add(rowNew)
                    Next
                Else
                    m_intStatus = -2202
                    m_strMessage = "Keine Vorgangsinformationen vorhanden."
                End If

                Dim col As DataColumn
                For Each col In m_tblFahrzeuge.Columns
                    col.ReadOnly = False
                Next

                WriteLogEntry(True, "KUNNR=" & m_strCustomer.TrimStart("0"c), m_tblFahrzeuge)
            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "KUNNR=" & m_strCustomer.TrimStart("0"c) & " , " & m_strMessage, m_tblFahrzeuge)
            Finally
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteStandardDataAccessSAP(m_intIDSAP)
                End If
                'con.Close()
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overloads Sub Change(ByVal page As Page)
        m_intStatus = 0
        m_strMessage = ""

        Dim tmpDataView As DataView
        tmpDataView = m_tblFahrzeuge.DefaultView

        tmpDataView.RowFilter = "ActionNOTHING = 0"

        Dim i As Int32
        If m_objLogApp Is Nothing Then
            m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
        End If

        For i = 0 To tmpDataView.Count - 1
            Dim strBemerkung As String = "Haltefrist ignoriert"
            Dim strActionText As String = "Löschen"

            Try
                m_intIDSAP = m_objLogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Abmbereit_Laufaen", m_strAppID, m_strSessionID, m_objUser.CurrentLogAccessASPXID)
                Dim strKUNNR As String = m_strCustomer
                Dim strKUNPDI As String = CType(tmpDataView.Item(i)("Belegnr"), String)
                Dim strZZFAHRG As String = CType(tmpDataView.Item(i)("Fahrgestellnummer"), String)
                Dim strZZKENN As String = CType(tmpDataView.Item(i)("Kennzeichen"), String)

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abmbereit_Laufaen", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("KUNNR", strKUNNR)
                'myProxy.setImportParameter("ZZKENN", strZZKENN)
                'myProxy.setImportParameter("ZZFAHRG", strZZFAHRG)
                'myProxy.setImportParameter("KUNPDI", strKUNPDI)

                'myProxy.callBapi()

                S.AP.InitExecute("Z_M_Abmbereit_Laufaen", "KUNNR,ZZKENN,ZZFAHRG,KUNPDI", strKUNNR, strZZKENN, strZZFAHRG, strKUNPDI)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Fehler bei der Speicherung der Vorgänge"
                strBemerkung = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Try

            m_tblFahrzeuge.AcceptChanges()
            Dim tmpRows As DataRow()
            tmpRows = m_tblFahrzeuge.Select("Fahrgestellnummer = '" & CType(tmpDataView.Item(i)("Fahrgestellnummer"), String) & "'")
            If tmpRows.Length > 0 Then
                tmpRows(0).BeginEdit()
                tmpRows(0).Item("Action") = strActionText
                tmpRows(0).Item("Bemerkung") = strBemerkung
                tmpRows(0).EndEdit()
                m_tblFahrzeuge.AcceptChanges()
            End If
        Next
    End Sub

#End Region
End Class

' ************************************************
' $History: ec_08.vb $
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.07.09   Time: 14:15
' Updated in $/CKAG/Applications/appec/Lib
' nachbesserung dynproxy umstellung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 26.06.09   Time: 14:53
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918, Z_M_Ec_Avm_Zulauf, Z_V_FAHRZEUG_STATUS_001,
' Z_M_EC_AVM_STATUS_BESTAND, Z_M_ABMBEREIT_LAUFAEN,
' Z_M_ABMBEREIT_LAUFZEIT, Z_M_Brief_Temp_Vers_Mahn_001,
' Z_M_SCHLUE_SET_MAHNSP_001, Z_M_SCHLUESSELDIFFERENZEN,
' Z_M_SCHLUESSELVERLOREN, Z_M_SCHLUE_TEMP_VERS_MAHN_001,
' Z_M_Ec_Avm_Status_Zul,  Z_M_ECA_TAB_BESTAND
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 17.06.09   Time: 8:47
' Updated in $/CKAG/Applications/appec/Lib
' Warnungen entfernt!
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 27.11.08   Time: 15:39
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2414 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 19.11.08   Time: 17:31
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2414
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 15.11.07   Time: 9:24
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
