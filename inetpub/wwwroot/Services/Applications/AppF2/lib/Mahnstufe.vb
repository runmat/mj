Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Public Class Mahnstufe
    Inherits DatenimportBase

#Region " Declarations"

    Private dtSAPHersteller As DataTable
    Private m_Filename As String
    Private m_Page As System.Web.UI.Page

#End Region

#Region " Properties"
    Public ReadOnly Property FileName() As String
        Get
            Return m_Filename
        End Get
    End Property

#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As CKG.Base.Kernel.Security.User, ByVal objApp As CKG.Base.Kernel.Security.App, ByVal Filename As String)
        MyBase.New(objUser, objApp, Filename)
        m_Filename = Filename
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID, m_Page)

    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
        m_strClassAndMethod = "Mahnstufe.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID


        Try
            Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_Brief_3mal_Gemahnt", m_objApp, m_objUser, page)

            myProxy.setImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
            myProxy.setImportParameter("I_SPRAS", "D")

            myProxy.callBapi()

            Dim tblTemp As DataTable = myProxy.getExportTable("EXP_BRIEFE")
            tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))

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

                'Distriktnummer kürzen
                If Not tmpRow("KNRZE") Is DBNull.Value Then
                    If Len(tmpRow("KNRZE")) > 1 Then
                        tmpRow("KNRZE") = Right(tmpRow("KNRZE").ToString, 2)
                    End If
                End If
            Next



            CreateOutPut(tblTemp, m_strAppID)

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                Case "HAENDLER_NOT_FOUND"
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

#End Region




End Class

' ************************************************
' $History: Mahnstufe.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 10.03.10   Time: 17:10
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 28.08.09   Time: 16:12
' Updated in $/CKAG2/Applications/AppF2/lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 19.08.09   Time: 14:48
' Updated in $/CKAG2/Applications/AppF2/lib
' ITA: 3000
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 19.08.09   Time: 9:07
' Created in $/CKAG2/Applications/AppF2/lib
' ITA: 3000
' 
