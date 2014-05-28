Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Fahrzeughistorie
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private tblHistorie As DataTable
    Private tblGrunddaten As DataTable
    Private tblAdressen As DataTable
    Private tblLastchange As DataTable
    Private tblEquidaten As DataTable
#End Region

#Region " Properties"

    Public SapSourceORM As Boolean

    Property Historie() As DataTable
        Get
            Return tblHistorie
        End Get
        Set(ByVal value As DataTable)
            tblHistorie = value
        End Set
    End Property
    Property Grunddaten() As DataTable
        Get
            Return tblGrunddaten
        End Get
        Set(ByVal value As DataTable)
            tblGrunddaten = value
        End Set
    End Property
    Property Adressen() As DataTable
        Get
            Return tblAdressen
        End Get
        Set(ByVal value As DataTable)
            tblAdressen = value
        End Set
    End Property
    Property LastChange() As DataTable
        Get
            Return tblAdressen
        End Get
        Set(ByVal value As DataTable)
            tblAdressen = value
        End Set
    End Property
    Property Equidaten() As DataTable
        Get
            Return tblEquidaten
        End Get
        Set(ByVal value As DataTable)
            tblEquidaten = value
        End Set
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    '----------------------------------------------------------------------
    ' Methode: Fill
    ' Autor: O.Rudolph
    ' Beschreibung: füllen der Ausgabetabelle für Fahrzeughistorie (Report07)
    ' Erstellt am: 30.12.2008
    ' ITA: 2389
    '----------------------------------------------------------------------

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByVal strAmtlKennzeichen As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strBriefnummer As String, ByVal strMVAnummer As String, _
                              ByVal mProduktionskennziffer As String)

        Fill_ORM(strAppID, strSessionID, strAmtlKennzeichen, strFahrgestellnummer, strBriefnummer, strMVAnummer, mProduktionskennziffer)

    End Sub

    Public Sub Fill_ORM(ByVal strAppID As String, ByVal strSessionID As String, _
                              ByVal strAmtlKennzeichen As String, _
                              ByVal strFahrgestellnummer As String, _
                              ByVal strBriefnummer As String, ByVal strMVAnummer As String, _
                              ByVal mProduktionskennziffer As String)

        Dim strKunnr As String = ""
        m_strClassAndMethod = "Fahrzeughistorie.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        strKunnr = Right("0000000000" & m_objUser.KUNNR, 10)

        Try


            S.AP.InitExecute("Z_M_BRIEFLEBENSLAUF_002",
                        "I_KUNNR, I_ZZKENN, I_ZZFAHRG, I_ZZBRIEF, I_PROD_KENNZIFFER, I_ZZREF1, I_EQUNR, I_MVA_NUMMER",
                        strKunnr, strAmtlKennzeichen.ToUpper, strFahrgestellnummer.ToUpper, strBriefnummer.ToUpper, mProduktionskennziffer.ToUpper, "", "", strMVAnummer)

            Dim tblTemp2 As DataTable = S.AP.GetExportTable("GT_WEB")
            If Not tblTemp2 Is Nothing Then
                tblGrunddaten = tblTemp2
            End If

            tblTemp2 = S.AP.GetExportTable("GT_ADDR")
            If Not tblTemp2 Is Nothing Then
                tblAdressen = tblTemp2
            End If

            tblTemp2 = S.AP.GetExportTable("GT_QMEL")
            If Not tblTemp2 Is Nothing Then
                tblHistorie = tblTemp2
            End If

            tblTemp2 = S.AP.GetExportTable("GT_EQUI")
            If Not tblTemp2 Is Nothing Then
                tblEquidaten = tblTemp2
            End If

            tblTemp2 = S.AP.GetExportTable("GT_QMMA")
            If Not tblTemp2 Is Nothing Then
                tblLastchange = tblTemp2
            End If

            Dim row As DataRow

            For Each row In tblLastchange.Rows
                row("AEZEIT") = row("AEZEIT").ToString.ToTimeString()
            Next

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -5555
            Select Case ex.Message
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", I_ZZKENN=" & strAmtlKennzeichen & ", I_ZZFAHRG=" & strFahrgestellnummer & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

#End Region
End Class
' ************************************************
' $History: Fahrzeughistorie.vb $
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 9.06.09    Time: 17:17
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 5.01.09    Time: 17:07
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 2389
' 
' ************************************************
