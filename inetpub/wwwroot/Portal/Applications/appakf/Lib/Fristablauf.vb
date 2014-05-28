Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Common
Imports CKG.Base.Kernel

Public Class Fristablauf
    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    
    Private m_strFilename2 As String
    Private m_strEQUNR As String
    Private m_strMemo As String
    Private mE_SUBRC As String
    Private mE_MESSAGE As String
    
#End Region

#Region " Properties"

    Public ReadOnly Property FileName() As String
        Get
            Return m_strFilename2
        End Get
    End Property

    Public Property Memo() As String
        Get
            Return m_strMemo
        End Get
        Set(ByVal value As String)
            m_strMemo = value
        End Set
    End Property

    Public Property EQUNR() As String
        Get
            Return m_strEQUNR
        End Get
        Set(ByVal value As String)
            m_strEQUNR = value
        End Set
    End Property

    Public Property E_SUBRC() As String
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As String)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
        m_strFilename2 = strFilename
    End Sub

    Public Overloads Overrides Sub Fill()

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Fristablauf.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Try
            S.AP.InitExecute("Z_M_BRIEF_1MAL_GEMAHNT", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_BRIEF_1MAL_GEMAHNT", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            'myProxy.callBapi()

            Dim tblTemp As DataTable = S.AP.getExportTable("EXP_BRIEFE") 'myProxy.getExportTable("EXP_BRIEFE")
            Dim tblTexte As DataTable = S.AP.getExportTable("GT_TEXT") 'myProxy.getExportTable("GT_TEXT")

            tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))
            tblTemp.Columns.Add("Memo", System.Type.GetType("System.String"))
            tblTemp.Columns.Add("User", System.Type.GetType("System.String"))

            Dim tmpRow As DataRow
            Dim strTemp As String

            For Each tmpRow In tblTemp.Rows
                strTemp = CStr(tmpRow("Name1"))
                If Not TypeOf tmpRow("Name2") Is System.DBNull Then
                    strTemp &= " " & CStr(tmpRow("Name2"))
                End If
                If Not TypeOf tmpRow("Name3") Is System.DBNull Then
                    strTemp &= " " & CStr(tmpRow("Name3"))
                End If
                If Not TypeOf tmpRow("Street") Is System.DBNull Then
                    strTemp &= ", " & CStr(tmpRow("Street"))
                End If
                If Not TypeOf tmpRow("House_Num1") Is System.DBNull Then
                    strTemp &= " " & CStr(tmpRow("House_Num1"))
                End If
                If Not TypeOf tmpRow("Post_Code1") Is System.DBNull Then
                    strTemp &= ", " & CStr(tmpRow("Post_Code1"))
                End If
                If Not TypeOf tmpRow("City1") Is System.DBNull Then
                    strTemp &= " " & CStr(tmpRow("City1"))
                End If

                tmpRow("Versandadresse") = strTemp

                If Not tblTexte Is Nothing Then
                    Dim drows As DataRow() = tblTexte.Select("EQUNR='" & tmpRow("EQUNR").ToString & "'")

                    If drows.Length = 1 Then
                        tmpRow("User") = ""
                        tmpRow("Memo") = drows(0)("TDLINE").ToString
                    ElseIf drows.Length = 2 Then
                        tmpRow("User") = drows(0)("TDLINE").ToString
                        tmpRow("Memo") = drows(1)("TDLINE").ToString
                    End If

                End If
            Next

            CreateOutPut(tblTemp, m_strAppID)
            ResultTable = Result

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "")
                Case "NOT_FOUND"
                    m_intStatus = -1111
                    m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
                Case "NO_DATA"
                    m_intStatus = -12
                    m_strMessage = "Keine Mahnungen gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

        End Try
    End Sub

    Public Sub SaveMemo(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal bloesch As Boolean = False)
        m_strClassAndMethod = "fin_03.SaveMemo"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        E_MESSAGE = ""
        E_SUBRC = ""

        Try
            S.AP.Init("Z_M_EQUI_CHANGE_LTEXT_001", "I_AG,I_EQUNR,I_USER,I_TIMESTAMP",
                             Right("0000000000" & m_objUser.KUNNR, 10),
                             m_strEQUNR,
                             Right(m_objUser.UserName, 12),
                             Now.ToString)

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_EQUI_CHANGE_LTEXT_001", m_objApp, m_objUser, Page)

            'myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            'myProxy.setImportParameter("I_EQUNR", m_strEQUNR)
            'myProxy.setImportParameter("I_USER", Right(m_objUser.UserName, 12))
            'myProxy.setImportParameter("I_TIMESTAMP", Now.ToString)
            If bloesch = False Then
                'myProxy.setImportParameter("I_AKTION", "A")
                'Dim tblTemp As DataTable = myProxy.getImportTable("GT_TEXT")
                S.AP.SetImportParameter("I_AKTION", "A")
                Dim tblTemp As DataTable = S.AP.GetImportTable("GT_TEXT")

                Dim NewRow As DataRow
                NewRow = tblTemp.NewRow
                NewRow("TDLINE") = m_strMemo
                tblTemp.Rows.Add(NewRow)
            Else
                S.AP.SetImportParameter("I_AKTION", "L")
            End If


            'myProxy.callBapi()
            S.AP.Execute()

            mE_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            mE_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case Else
                    m_intStatus = -9999
                    mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: Fristablauf.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 26.02.10   Time: 13:57
' Updated in $/CKAG/Applications/appakf/Lib
' 
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 25.02.10   Time: 14:51
' Updated in $/CKAG/Applications/appakf/Lib
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 18.11.08   Time: 9:12
' Updated in $/CKAG/Applications/appakf/Lib
' ITA 2384 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 13.11.08   Time: 15:29
' Created in $/CKAG/Applications/appakf/Lib
' ITA 2384
' 
' ************************************************
