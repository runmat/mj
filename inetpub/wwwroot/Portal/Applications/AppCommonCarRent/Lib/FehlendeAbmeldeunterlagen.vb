Option Explicit On 
Option Strict On
Imports CKG.Base.Common
Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class FehlendeAbmeldeunterlagen
    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    Private mKennzeichen As String
    Private mFahrzeugUndAufbauart As String
    Private mHersteller As String
    Private mTypUndAusfuehrung As String
    Private mFIN As String
    Private mName As String
    Private mPostleitzahl As String
    Private mOrt As String
    Private mStrasse As String
    Private mAnzahlSchilder As String
    Private mVorderes As String = "Verlust"
    Private mHinteres As String = "Verlust"
   

#End Region

#Region " Properties"

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

    Public Property hinteres() As String
        Get
            Return mHinteres
        End Get
        Set(ByVal value As String)
            mHinteres = value
        End Set
    End Property

    Public Property vorderes() As String
        Get
            Return mVorderes
        End Get
        Set(ByVal value As String)
            mVorderes = value
        End Set
    End Property

    Public Property FahrzeugUndAufbauart() As String
        Get
            Return mFahrzeugUndAufbauart
        End Get
        Set(ByVal value As String)
            mFahrzeugUndAufbauart = value
        End Set
    End Property

    Public Property TypUndAusfuehrung() As String
        Get
            Return mTypUndAusfuehrung
        End Get
        Set(ByVal value As String)
            mTypUndAusfuehrung = value
        End Set
    End Property

    Public Property FIN() As String
        Get
            Return mFIN
        End Get
        Set(ByVal value As String)
            mFIN = value
        End Set
    End Property

    Public Property Hersteller() As String
        Get
            Return mHersteller
        End Get
        Set(ByVal value As String)
            mHersteller = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return mName
        End Get
        Set(ByVal value As String)
            mName = value
        End Set
    End Property

    Public Property Postleitzahl() As String
        Get
            Return mPostleitzahl
        End Get
        Set(ByVal value As String)
            mPostleitzahl = value
        End Set
    End Property

    Public Property Ort() As String
        Get
            Return mOrt
        End Get
        Set(ByVal value As String)
            mOrt = value
        End Set
    End Property

    Public Property Strasse() As String
        Get
            Return mStrasse
        End Get
        Set(ByVal value As String)
            mStrasse = value
        End Set
    End Property

    Public Property AnzahlSchilder() As String
        Get
            Return mAnzahlSchilder
        End Get
        Set(ByVal value As String)
            mAnzahlSchilder = value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Sub FILL_FehlUntl(ByVal strAppID As String, ByVal strSessionID As String, ByRef page As Page)
        '----------------------------------------------------------------------
        ' Methode: FILL_FehlUntl
        ' Autor: JJU
        ' Beschreibung: ruft das bapi Z_M_Abm_Fehlende_Unterl_010 auf
        ' Erstellt am: 16.02.2009
        ' ITA: 2588
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Abm_Fehlende_Unterl_010", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

         
            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_Abm_Fehlende_Unterl_010", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

            CreateOutPut(S.AP.GetExportTable("GT_WEB"), strAppID) 'myProxy.getExportTable("GT_WEB")

        Catch ex As Exception
            m_intStatus = -9999
            Select Case ex.Message
                Case "ERR_NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Keine Fahrzeuge vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: FehlendeAbmeldeunterlagen.vb $
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 19.02.09   Time: 10:18
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2586/ 2588
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 17.02.09   Time: 16:28
' Updated in $/CKAG/Applications/AppCommonCarRent/Lib
' ITA 2588 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 16.02.09   Time: 14:19
' Created in $/CKAG/Applications/AppCommonCarRent/Lib
' ITa 2586/2588 unfertig
' 
' ************************************************
