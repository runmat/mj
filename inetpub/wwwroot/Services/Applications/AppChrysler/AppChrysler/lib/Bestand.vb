Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Kernel.Common.Common
Imports Microsoft.Data.SAPClient
Imports CKG.Base.Common

Public Class Bestand
    Inherits DatenimportBase

#Region " Declarations"
    Private mDatumVon As String
    Private mDatumBis As String
    Private mDatumVonAus As String
    Private mDatumBisAus As String
    Private mDatumSchuesselerfassungVon As String
    Private mDatumSchuesselerfassungBis As String
    Private mDatumSchuesselversandVon As String
    Private mDatumSchuesselversandBis As String
    Private mFahrgestellnummer As String
    Private mKennzeichen As String
    Private mBestandsart As String
    Private mAutovermieter As String
#End Region

#Region " Properties"
    Public Property DatumVon() As String
        Get
            Return mDatumVon
        End Get
        Set(ByVal value As System.String)
            mDatumVon = value
        End Set
    End Property
    Public Property DatumBis() As String
        Get
            Return mDatumBis
        End Get
        Set(ByVal value As System.String)
            mDatumBis = value
        End Set
    End Property

    Public Property DatumVonAus() As String
        Get
            Return mDatumVonAus
        End Get
        Set(ByVal value As System.String)
            mDatumVonAus = value
        End Set
    End Property
    Public Property DatumBisAus() As String
        Get
            Return mDatumBisAus
        End Get
        Set(ByVal value As System.String)
            mDatumBisAus = value
        End Set
    End Property

    Public Property DatumSchluesselerfVon() As String
        Get
            Return mDatumSchuesselerfassungVon
        End Get
        Set(ByVal value As System.String)
            mDatumSchuesselerfassungVon = value
        End Set
    End Property

    Public Property DatumSchluesselerfBis() As String
        Get
            Return mDatumSchuesselerfassungBis
        End Get
        Set(ByVal value As System.String)
            mDatumSchuesselerfassungBis = value
        End Set
    End Property

    Public Property DatumSchluesselversandVon() As String
        Get
            Return mDatumSchuesselversandVon
        End Get
        Set(ByVal value As System.String)
            mDatumSchuesselversandVon = value
        End Set
    End Property
    Public Property DatumSchluesselversandBis() As String
        Get
            Return mDatumSchuesselversandBis
        End Get
        Set(ByVal value As System.String)
            mDatumSchuesselversandBis = value
        End Set
    End Property

    Public Property Fahrgestellnummer() As String
        Get
            Return mFahrgestellnummer
        End Get
        Set(ByVal value As String)
            mFahrgestellnummer = value
        End Set
    End Property

    Public Property Kennzeichen() As String
        Get
            Return mKennzeichen
        End Get
        Set(ByVal value As String)
            mKennzeichen = value
        End Set
    End Property

    Public Property Bestandsart() As String
        Get
            Return mBestandsart
        End Get
        Set(ByVal value As String)
            mBestandsart = value
        End Set
    End Property
    Public Property Autovermieter() As String
        Get
            Return mAutovermieter
        End Get
        Set(ByVal value As String)
            mAutovermieter = value
        End Set
    End Property


#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Bestand.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim KUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Try
                Dim ErrMessage As String

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_REM_001", m_objApp, m_objUser, page)

                'Importparameter
                myProxy.setImportParameter("I_KUNNR_AG", KUNNR)
                myProxy.setImportParameter("I_BESTANDSART", CStr(mBestandsart))
                myProxy.setImportParameter("I_KENNZEICHEN", CStr(mKennzeichen))
                myProxy.setImportParameter("I_CHASSIS_NUM", CStr(mFahrgestellnummer))
                myProxy.setImportParameter("I_AVNR", CStr(mAutovermieter))
                myProxy.setImportParameter("I_DATIN_VON", mDatumVon)
                myProxy.setImportParameter("I_DATIN_BIS", mDatumBis)
                myProxy.setImportParameter("I_DTOUT_VON", mDatumVonAus)
                myProxy.setImportParameter("I_DTOUT_BIS", mDatumBisAus)
                myProxy.setImportParameter("I_EGZWSL_VON", mDatumSchuesselerfassungVon)
                myProxy.setImportParameter("I_EGZWSL_BIS", mDatumSchuesselerfassungBis)
                myProxy.setImportParameter("I_VSZWSL_VON", mDatumSchuesselversandVon)
                myProxy.setImportParameter("I_VSZWSL_BIS", mDatumSchuesselversandBis)




                myProxy.callBapi()

                Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")

                CreateOutPut(tblTemp, strAppID)

                ErrMessage = myProxy.getExportParameter("E_MESSAGE")

                If ErrMessage.Length > 0 Then Err.Raise(-9999, , ErrMessage)

            Catch ex As Exception
                m_intStatus = -9999

                Select Case ex.Message.ToUpper
                    Case "NO DATA"
                        m_strMessage = "Es wurden keine Daten gefunden."
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region





End Class
' ************************************************
' $History: Bestand.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 9.04.10    Time: 10:27
' Updated in $/CKAG2/Applications/AppChrysler/AppChrysler/lib
' ITA: 3624
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 16.09.09   Time: 11:03
' Created in $/CKAG2/Applications/AppChrysler/AppChrysler/lib
' 