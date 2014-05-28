Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Portal.PageElements
Imports CKG.Base.Common

Public Class ec_12
    REM § Status-Report, Kunde: Sixt, BAPI: Z_M_Zugelassene_Fahrzeuge,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase ' FFD_Bank_Datenimport

#Region " Declarations"
    Private m_datEingangsdatumVon As DateTime
    Private m_datEingangsdatumBis As DateTime
    Private m_strPDI As String
#End Region

#Region " Properties"
    Public Property PEingangsdatumVon() As DateTime
        Get
            Return m_datEingangsdatumVon
        End Get
        Set(ByVal Value As DateTime)
            m_datEingangsdatumVon = Value
        End Set
    End Property

    Public Property PEingangsdatumBis() As DateTime
        Get
            Return m_datEingangsdatumBis
        End Get
        Set(ByVal Value As DateTime)
            m_datEingangsdatumBis = Value
        End Set
    End Property

    Public Property PPDI() As String
        Get
            Return m_strPDI
        End Get
        Set(ByVal Value As String)
            m_strPDI = Value
        End Set
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim tblTemp As DataTable
        Dim row As DataRow
        Dim datDate As Date
        Dim vwView As DataView
        Dim intItem As Integer
        Dim intSumme As Integer
        Dim rowFound As DataRow()

        m_strClassAndMethod = "ec_12.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Ec_Avm_Zulassungen", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_PDI", m_strPDI)
            'myProxy.setImportParameter("I_ZULDAT_VON", m_datEingangsdatumVon.ToShortDateString)
            'myProxy.setImportParameter("I_ZULDAT_BIS", m_datEingangsdatumBis.ToShortDateString)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Ec_Avm_Zulassungen", "I_KUNNR,I_PDI,I_ZULDAT_VON,I_ZULDAT_BIS", Right("0000000000" & m_objUser.KUNNR, 10), _
                             m_strPDI, m_datEingangsdatumVon.ToShortDateString, m_datEingangsdatumBis.ToShortDateString)

            tblTemp = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")

            CreateOutPut(tblTemp, strAppID)

            'Datumsfeld enfügen
            Result.Columns.Add("DatumZulassung", GetType(System.DateTime))
            'Flag für Tagessumme
            Result.Columns.Add("Tagessumme", GetType(System.Int16))
            'Flag für Tagessumme 2
            Result.Columns.Add("Flag", GetType(System.Int16))

            For Each row In Result.Rows
                row("Flag") = 0
                Try
                    datDate = CDate(row("Zulassungsdatum"))
                    row("DatumZulassung") = datDate
                Catch ex As Exception
                    row("DatumZulassung") = Nothing
                End Try
            Next

            Result.AcceptChanges()

            tblTemp = Result.Clone                                  'Ausgabetabelle
            vwView = Result.DefaultView
            vwView.Sort = "DatumZulassung ASC"

            datDate = Nothing
            intSumme = 0

            'Summen bilden

            For intItem = vwView.Count - 1 To 0 Step -1
                If CInt(Result.Rows(intItem)("Flag")) = 0 Then
                    intSumme = 0
                    datDate = CDate(vwView.Item(intItem)("DatumZulassung"))
                    rowFound = Result.Select("Zulassungsdatum='" & datDate & "'")   'Alle Zeilen mit dem Datum holen

                    For Each row In rowFound
                        row("Flag") = 1
                        intSumme = intSumme + CInt(row("Anzahl"))
                    Next

                    Result.Rows(intItem)("Tagessumme") = intSumme
                End If
            Next

            WriteLogEntry(True, "VDATU_VON=" & m_datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & m_datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDI, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_strMessage = "Keine Ausgabedaten gefunden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten."
            End Select
            WriteLogEntry(False, "VDATU_VON=" & m_datEingangsdatumVon.ToShortDateString & ", VDATU_BIS=" & m_datEingangsdatumBis.ToShortDateString & ", KUNNR=" & m_objUser.KUNNR & ", ZZCARPORT=" & m_strPDI & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        End Try
    End Sub

#End Region
End Class

' ************************************************
' $History: ec_12.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 26.06.09   Time: 11:29
' Updated in $/CKAG/Applications/appec/Lib
' ITA 2918
' Z_M_EC_AVM_BRIEFLEBENSLAUF,Z_M_Ec_Avm_Fzg_M_Dfs_O_Zul,Z_M_EC_AVM_FZG_OH
' NE_BRIEF,Z_M_Ec_Avm_Fzg_Ohne_Unitnr,Z_M_Ec_Avm_Nur_Brief_Vorh,
' Z_M_EC_AVM_OFFENE_ZAHLUNGEN,  Z_M_EC_AVM_PDI_BESTAND,
' Z_M_EC_AVM_STATUS_EINSTEUERUNG,  Z_M_EC_AVM_STATUS_GREENWAY,
' Z_M_Ec_Avm_Status_Zul, Z_M_EC_AVM_ZULASSUNGEN, Z_M_Ec_Avm_Zulassungen_2
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 15:49
' Created in $/CKAG/Applications/appec/Lib
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 17:23
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Uha          Date: 7.03.07    Time: 11:02
' Updated in $/CKG/Applications/AppEC/AppECWeb/Lib
' 
' ************************************************
