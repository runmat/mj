Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

Public Class OffeneAnforderungen
    Inherits CKG.Base.Business.BankBase

#Region " Declarations"
    Private m_tblAuftraege As DataTable
    Private m_tblRaw As DataTable
    Private m_strHaendler As String
    Private m_strEQUNR As String
    Private m_strVBELN As String
    Private m_strStornoHaendler As String
    Private m_strFahrgestellnummer As String
    Private m_strLiznr As String

#End Region

#Region " Properties"
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return m_tblAuftraege
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


    Public Property StornoHaendler() As String
        Get
            Return m_strStornoHaendler
        End Get
        Set(ByVal Value As String)
            m_strStornoHaendler = Value
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
    Public Property Liznr() As String
        Get
            Return m_strLiznr
        End Get
        Set(ByVal Value As String)
            m_strLiznr = Value
        End Set
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByRef objApp As CKG.Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String, Optional ByVal hez As Boolean = False)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)

    End Sub

    Public Overloads Overrides Sub show()
        'nur wegen bankbase
    End Sub

    Public Overloads Sub Show(ByVal HEZ As String, ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        Dim rowTemp As DataRow

        Try
            m_intStatus = 0
            m_strMessage = ""

            Dim Kunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)

            '****Test mit AKF*****
            'Kunnr = "0000302660"
            '*********************

            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Offene_Anforderungen_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Kunnr)
            myProxy.setImportParameter("I_HAENDLER", m_strHaendler)
            myProxy.setImportParameter("I_VKORG", "1510")
            myProxy.setImportParameter("I_HEZKZ", HEZ)
            myProxy.setImportParameter("I_ZZREFNR", m_strLiznr)

            myProxy.callBapi()


            m_tblRaw = myProxy.getExportTable("GT_WEB")
            m_tblRaw.Columns.Add("Adresse", System.Type.GetType("System.String"))
            m_tblRaw.Columns("BSTZD").MaxLength = 25
            m_tblRaw.AcceptChanges()
            For Each rowTemp In m_tblRaw.Rows
                Select Case rowTemp("BSTZD").ToString
                    Case "0001"
                        rowTemp("BSTZD") = "Standard temporär"
                    Case "0002"
                        rowTemp("BSTZD") = "Standard endgültig"
                    Case "0005"
                        rowTemp("BSTZD") = "Händlereigene Zulassung"
                End Select
                If CStr(rowTemp("CMGST")) = "B" Then
                    rowTemp("CMGST") = "X"
                Else
                    rowTemp("CMGST") = ""
                End If

                rowTemp("Adresse") = CStr(rowTemp("Name1_ZF")) & "<br>" & CStr(rowTemp("Name2_ZF")) & "<br>" & rowTemp("ORT01_ZF").ToString

            Next
            m_tblRaw.AcceptChanges()
            m_tblAuftraege = CreateOutPut(m_tblRaw, m_strAppID)
            m_tblResultExcel = m_tblAuftraege.Copy
            m_tblResultExcel.Columns.Remove("VBELN")
            m_tblResultExcel.Columns.Remove("EQUNR")
            m_tblResultExcel.Columns.Remove("KVGR3")


            If m_tblAuftraege.Rows.Count = 0 Then
                m_intStatus = 0
                m_strMessage = "Keine Daten gefunden."
            End If

            If Not m_strHaendler Is Nothing AndAlso m_strHaendler.Trim.Length > 0 Then
                Haendler = m_strHaendler
            End If
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_DATA"
                    m_intStatus = 0
                    If m_hez = True Then
                        m_intStatus = -2501
                    End If
                    m_strMessage = "Keine Daten gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub


    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)

        Try
            m_intStatus = 0
            m_strMessage = ""


            Dim Kunnr As String = Right("0000000000" & m_objUser.KUNNR, 10)

            '****Test mit AKF*****
            'Kunnr = "0000302660"
            '*********************


            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Offeneanfor_Storno_001", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Kunnr)
            myProxy.setImportParameter("I_HAENDLER", Right("0000000000" & StornoHaendler, 10))
            myProxy.setImportParameter("I_EQUNR", m_strEQUNR)
            myProxy.setImportParameter("I_VBELN", m_strVBELN)
            myProxy.setImportParameter("I_ERNAM", Left(m_objUser.UserName, 12))

            myProxy.callBapi()

        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "NO_UPDATE"
                    m_intStatus = -3501
                    m_strMessage = "Kein EQUI-UPDATE."
                Case "NO_ZDADVERSAND"
                    m_intStatus = -3502
                    m_strMessage = "Kein ZDADVERSAND-STORNO."
                Case "NO_UPDATE_SALESDOCUMENT"
                    m_intStatus = -3503
                    m_strMessage = "Keine Auftragsänderung."
                Case "ZVERSAND_SPERRE"
                    m_intStatus = -3504
                    m_strMessage = "ZVERSAND vom DAD gesperrt."
                Case "NO_PICKLISTE"
                    m_intStatus = -3505
                    m_strMessage = "Kein Picklisteneintrag gefunden."
                Case "NO_ZCREDITCONTROL"
                    m_intStatus = -3506
                    m_strMessage = "Kein Creditcontroleintrag gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
            End Select
        End Try
    End Sub
    Public Overrides Sub Change()
    End Sub

#End Region
End Class