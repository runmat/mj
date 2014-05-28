Option Explicit On
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Inanspruchnahme

    Inherits Base.Business.BankBase

#Region " Declarations"
    Private mEquis As DataTable
    Private m_strHaendler As String
    Private m_strEQUNR As String
    Private m_strVBELN As String
    Private m_strStornoHaendler As String
    Private m_strFahrgestellnummer As String
    Private mGTWeb As DataTable

#End Region

#Region " Properties"

    Public ReadOnly Property Equipments() As DataTable
        Get
            Return mEquis
        End Get
    End Property

    Public ReadOnly Property GTWeb() As DataTable
        Get
            Return mGTWeb
        End Get
    End Property

    Public Property Haendler() As String
        Get
            Return m_strHaendler
        End Get
        Set(ByVal Value As String)
            m_strHaendler = Value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return m_strFahrgestellnummer
        End Get
        Set(ByVal Value As String)
            m_strFahrgestellnummer = Value
        End Set
    End Property

    Public Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
        Set(ByVal Value As String)
            m_strEQUNR = Value
        End Set
    End Property

    Public Property VBELN() As String
        Get
            Return m_strVBELN
        End Get
        Set(ByVal Value As String)
            m_strVBELN = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        '----------------------------------------------------------------------
        ' Methode: Show
        ' Autor: JJU
        ' Beschreibung: gibt die INanspruchnahme für einen oder alle Händler zurück
        ' Erstellt am: 12.03.2009
        ' ITA: 2688
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_GET_INANSPRUCHNAHME_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", Haendler)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_GET_INANSPRUCHNAHME_STD", "I_AG,I_HAENDLER_EX", m_strKUNNR, Haendler)

            mEquis = S.AP.GetExportTable("GT_WEB") 'myProxy.getExportTable("GT_WEB")
            mEquis.Columns.Add("ZZKKBERAnzeige", String.Empty.GetType)

            For Each tmpRow As DataRow In mEquis.Rows
                With tmpRow
                    Select Case .Item("ZZKKBERAnzeige").ToString
                        Case "0001"
                            .Item("ZZKKBERAnzeige") = "temporär"
                        Case "0002"
                            .Item("ZZKKBERAnzeige") = "endgültig"
                        Case Else
                            .Item("ZZKKBERAnzeige") = "unbekannt"
                    End Select
                End With
            Next

            mEquis = CreateOutPut(mEquis, AppID)

        Catch ex As Exception
            mEquis = Nothing
            m_intStatus = -9999

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try

    End Sub

    Public Overrides Sub Change()

    End Sub


#End Region

End Class
' ************************************************
' $History: Inanspruchnahme.vb $
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 23.03.09   Time: 17:09
' Updated in $/CKAG/Applications/AppF1/lib
' ita 2668 nachbesserungen
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 17.03.09   Time: 9:20
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2688, 2668 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 12.03.09   Time: 14:56
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2688/2668 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 12.03.09   Time: 13:09
' Created in $/CKAG/Applications/AppF1/lib
' ITA 2668, 2688
' 
' ************************************************