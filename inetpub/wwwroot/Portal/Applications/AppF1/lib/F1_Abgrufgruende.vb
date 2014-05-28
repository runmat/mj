Option Explicit On
Option Strict On
Imports CKG
Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel
Imports CKG.Base.Common

Public Class F1_Abgrufgruende
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_datAbDatum As String
    Private m_datBisDatum As String
    Private m_strSAPWert As String = ""
    Private m_strKunnr As String = ""
    Private mBezahltKennzeichen As String = ""
    Private tblGruende As DataTable
    Private mPage As System.Web.UI.Page
#End Region

#Region " Properties"

    Public Property Abrufgruende() As DataTable
        Get
            Return tblGruende
        End Get
        Set(ByVal Value As DataTable)
            tblGruende = Value
        End Set
    End Property

    Public Property datVON() As String
        Get
            Return m_datAbDatum
        End Get
        Set(ByVal Value As String)
            m_datAbDatum = Value
        End Set
    End Property

    Public Property Bezahlt() As String
        Get
            Return mBezahltKennzeichen
        End Get
        Set(ByVal Value As String)
            mBezahltKennzeichen = Value
        End Set
    End Property

    Public Property datBIS() As String
        Get
            Return m_datBisDatum
        End Get
        Set(ByVal Value As String)
            m_datBisDatum = Value
        End Set
    End Property


    Public Property SAPWert() As String
        Get
            Return m_strSAPWert
        End Get
        Set(ByVal Value As String)
            m_strSAPWert = Value
        End Set
    End Property
    Public Property Kunnr() As String
        Get
            Return m_strKunnr
        End Get
        Set(ByVal Value As String)
            m_strKunnr = Value
        End Set
    End Property
#End Region


    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: FILL
        ' Autor: JJU
        ' Beschreibung: ruft das BAPI Z_M_VERSAND_JE_ABRUFGRUND_STD auf
        ' Erstellt am: 12.03.2009
        ' ITA: 2683
        '----------------------------------------------------------------------

        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_VERSAND_JE_ABRUFGRUND_STD", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_HAENDLER_EX", m_strKunnr)
            'myProxy.setImportParameter("I_DAT_VON", m_datAbDatum)
            'myProxy.setImportParameter("I_DAT_BIS", m_datBisDatum)
            'myProxy.setImportParameter("I_VSGRUND", m_strSAPWert)
            'myProxy.setImportParameter("I_BEZAHLT", mBezahltKennzeichen)

            'myProxy.callBapi()

            'CreateOutPut(myProxy.getExportTable("GT_WEB"), strAppID)

            S.AP.InitExecute("Z_M_VERSAND_JE_ABRUFGRUND_STD", "I_AG,I_HAENDLER_EX,I_DAT_VON,I_DAT_BIS,I_VSGRUND,I_BEZAHLT", Right("0000000000" & m_objUser.KUNNR, 10),
                             m_strKunnr, m_datAbDatum, m_datBisDatum, m_strSAPWert, mBezahltKennzeichen)

            CreateOutPut(S.AP.GetExportTable("GT_WEB"), strAppID)

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -1402
                    m_strMessage = "Keine Daten vorhanden."
                Case Else
                    m_intStatus = -5555
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub

    Public Sub DBAbrufgrund(ByVal strGrundTyp As String)
        Dim cn As New SqlClient.SqlConnection


        Try
            m_intStatus = 0
            cn.ConnectionString = ConfigurationManager.AppSettings("Connectionstring")
            cn.Open()

            Dim da As New SqlClient.SqlDataAdapter("SELECT  TOP 100 PERCENT SapWert, WebBezeichnung " & _
                                            " FROM dbo.CustomerAbrufgruende " & _
                                            " WHERE (CustomerID = " & m_objUser.Customer.CustomerId & ") " & _
                                            " AND (SapWert <> '000') " & _
                                            " GROUP BY SapWert, WebBezeichnung, AbrufTyp " & _
                                            " HAVING (AbrufTyp = '" & strGrundTyp & "')" & _
                                            " ORDER BY WebBezeichnung", cn)
            tblGruende = New DataTable
            da.Fill(tblGruende)

        Catch ex As Exception
            m_intStatus = -99
            m_strMessage = ex.Message
        Finally
            cn.Close()
        End Try
    End Sub

End Class
' ************************************************
' $History: F1_Abgrufgruende.vb $
' 
' *****************  Version 5  *****************
' User: Fassbenders  Date: 11.03.10   Time: 13:51
' Updated in $/CKAG/Applications/AppF1/lib
' ITA: 2918
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 30.04.09   Time: 11:39
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2837
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 12.03.09   Time: 13:09
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2668, 2688
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 10.03.09   Time: 16:12
' Updated in $/CKAG/Applications/AppF1/lib
' ITA 2666 unfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 10.03.09   Time: 14:36
' Created in $/CKAG/Applications/AppF1/lib
' ITa 2666 rohversion
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 30.07.08   Time: 14:34
' Updated in $/CKAG/Applications/AppFFE/lib
' ITA:2091
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 7.07.08    Time: 10:49
' Updated in $/CKAG/Applications/AppFFE/lib
' Historie hinzugefgt
' 
' ************************************************