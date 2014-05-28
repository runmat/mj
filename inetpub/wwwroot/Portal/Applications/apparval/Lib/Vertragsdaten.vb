Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Vertragsdaten
    Inherits DatenimportBase

#Region " Declarations"
    Private mFahrgestellnummer As String
    Private mKennzeichen As String
    Private mVertragsnummer As String
    Private mEquinr As String
    Private mKunnrZL As String
    Private mAdressenTable As DataTable
    Private mVertragTable As DataTable
#End Region

#Region " Properties"

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

    Public Property Vertragsnummer() As String
        Get
            Return mVertragsnummer
        End Get
        Set(ByVal value As String)
            mVertragsnummer = value
        End Set
    End Property
    Public Property Equinr() As String
        Get
            Return mEquinr
        End Get
        Set(ByVal value As String)
            mEquinr = value
        End Set
    End Property
    Public Property KunnrZL() As String
        Get
            Return mKunnrZL
        End Get
        Set(ByVal value As String)
            mKunnrZL = value
        End Set
    End Property

    Public Property Adressen() As DataTable
        Get
            Return mAdressenTable
        End Get
        Set(ByVal value As DataTable)
            mAdressenTable = value
        End Set
    End Property

    Public Property VertragTable() As DataTable
        Get
            Return mVertragTable
        End Get
        Set(ByVal value As DataTable)
            mVertragTable = value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strFileName)
    End Sub

    Public Overloads Sub Fill()
        m_strClassAndMethod = "Vetragsdaten.FILL"
        'm_strAppID = strAppID
        'm_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
            'Dim intID As Int32 = -1

            Dim kunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)


            Try
                Dim errMessage As String

                S.AP.InitExecute("Z_DPM_READ_EQUI_001", "I_KUNNR_AG,I_LIZNR,I_LICENSE_NUM,I_CHASSIS_NUM",
                                 kunnr, CStr(mVertragsnummer), CStr(mKennzeichen), CStr(mFahrgestellnummer))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_EQUI_001", m_objApp, m_objUser, page)

                ''Importparameter
                'myProxy.setImportParameter("I_KUNNR_AG", KUNNR)


                'myProxy.setImportParameter("I_LIZNR", CStr(mVertragsnummer))
                'myProxy.setImportParameter("I_LICENSE_NUM", CStr(mKennzeichen))
                'myProxy.setImportParameter("I_CHASSIS_NUM", CStr(mFahrgestellnummer))


                'myProxy.callBapi()

                VertragTable = S.AP.GetExportTable("GT_OUT") 'myProxy.getExportTable("GT_OUT")


                'Keine Vertragsdaten gefunden - aussteigen.
                If VertragTable.Rows.Count < 1 Then Err.Raise(-9999, , "NO_DATA")


                Adressen = S.AP.GetExportTable("GT_ZL") 'myProxy.getExportTable("GT_ZL")


                For Each row As DataRow In Adressen.Rows
                    row("NAME1") = row("NAME1").ToString & IIf(String.IsNullOrEmpty(row("NAME2").ToString), "", " ").ToString & row("NAME2").ToString & ", "
                    row("NAME1") = row("NAME1").ToString & row("STREET").ToString & IIf(String.IsNullOrEmpty(row("HOUSE_NUM1").ToString), "", " ").ToString & row("HOUSE_NUM1").ToString & ", "
                    row("NAME1") = row("NAME1").ToString & row("POST_CODE1").ToString & " " & row("CITY1").ToString & " ~ " & Right("200000000" & row("KUNNR").ToString, 9)
                Next


                errMessage = S.AP.GetExportParameter("E_MESSAGE") 'myProxy.getExportParameter("E_MESSAGE")

                If errMessage.Length > 0 Then Err.Raise(-9999, , errMessage)

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

    Public Sub Change()
        m_strClassAndMethod = "Vetragsdaten.Change"

        If Not m_blnGestartet Then
            m_blnGestartet = True
            'Dim intID As Int32 = -1

            Dim kunnr As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try
                Dim errMessage As String

                S.AP.InitExecute("Z_DPM_SAVE_EQUI_001", "I_KUNNR_AG,I_LIZNR,I_EQUNR,I_KUNNR_ZL",
                                 kunnr, CStr(mVertragsnummer), CStr(mEquinr), CStr(Right("0200000000" & mKunnrZL, 10)))

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_SAVE_EQUI_001", m_objApp, m_objUser, page)

                ''Importparameter
                'myProxy.setImportParameter("I_KUNNR_AG", kunnr)


                'myProxy.setImportParameter("I_LIZNR", CStr(mVertragsnummer))
                'myProxy.setImportParameter("I_EQUNR", CStr(mEquinr))
                'myProxy.setImportParameter("I_KUNNR_ZL", CStr(Right("0200000000" & mKunnrZL, 10)))


                'myProxy.callBapi()

                errMessage = S.AP.GetExportParameter("E_MESSAGE") 'myProxy.getExportParameter("E_MESSAGE")

                If errMessage.Length > 0 Then Err.Raise(-9999, , errMessage)

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = "Beim Speichern ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class
' ************************************************
' $History: Vertragsdaten.vb $
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 20.01.10   Time: 16:34
' Updated in $/CKAG/Applications/apparval/Lib
' ITA: 3339
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 15.01.10   Time: 16:25
' Created in $/CKAG/Applications/apparval/Lib
' ITA: 3339
' 