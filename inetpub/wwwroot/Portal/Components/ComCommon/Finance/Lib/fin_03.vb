Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Public Class fin_03
    REM § Status-Report, Kunde: Übergreifend, BAPI: Z_M_Brief_3mal_Gemahnt,
    REM § Ausgabetabelle per Zuordnung in Web-DB.

    Inherits Base.Business.DatenimportBase

#Region " Declarations"

    'Private dtSAPHersteller As DataTable
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

    'Public Overrides Sub Fill()

    'Fill(m_strAppID, m_strSessionID)

    'End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "fin_03.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID

        Try
            S.AP.InitExecute("Z_M_Brief_3mal_Gemahnt", "I_AG", Right("0000000000" & m_objUser.KUNNR, 10))

            Dim tblTemp As DataTable = S.AP.GetExportTable("EXP_BRIEFE")
            Dim tblTexte As DataTable = S.AP.GetExportTable("GT_TEXT")

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
                    Dim drows() As DataRow
                    drows = tblTexte.Select("EQUNR='" & tmpRow("EQUNR").ToString & "'")

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

            Dim errormessage As String = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)

            If errormessage.Contains("NOT_FOUND") Then
                m_intStatus = -1111
                m_strMessage = "Keine Ergebnisse zu den Kriterien gefunden."
            ElseIf errormessage.Contains("NO_DATA") Then
                m_intStatus = -12
                m_strMessage = "Keine Mahnungen gefunden."
            Else
                m_intStatus = -9999
                m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & errormessage & ")"
            End If

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
            S.AP.Init("Z_M_EQUI_CHANGE_LTEXT_001", "I_AG, I_EQUNR, I_USER, I_TIMESTAMP",
                      Right("0000000000" & m_objUser.KUNNR, 10), m_strEQUNR, Right(m_objUser.UserName, 12), Now.ToString)

            If bloesch = False Then
                S.AP.SetImportParameter("I_AKTION", "A")
                Dim tblTemp As DataTable = S.AP.GetImportTable("GT_TEXT")
                Dim NewRow As DataRow
                NewRow = tblTemp.NewRow
                NewRow("TDLINE") = m_strMemo
                tblTemp.Rows.Add(NewRow)
            Else
                S.AP.SetImportParameter("I_AKTION", "L")
            End If

            S.AP.Execute()

            mE_SUBRC = S.AP.GetExportParameter("E_SUBRC")
            mE_MESSAGE = S.AP.GetExportParameter("E_MESSAGE")

            WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
        Catch ex As Exception
            m_intStatus = -9999
            mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

        End Try
    End Sub

#End Region

End Class

' ************************************************
' $History: fin_03.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 23.02.10   Time: 9:56
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA: 3509
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 19.02.10   Time: 16:04
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 14:29
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - .Net Connector Umstellung
' 
' Bapis:
' Z_M_Abweich_abrufgrund
' Z_M_Save_ZABWVERGRUND
' 
' *****************  Version 3  *****************
' User: Dittbernerc  Date: 19.06.09   Time: 10:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 2  *****************
' User: Dittbernerc  Date: 18.06.09   Time: 17:18
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 - .net Connector umstellen
' 
' Bapis:
' Z_M_Brief_3mal_Gemahnt
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Finance/Lib
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 3  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 10:20
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Kosmetik im Bereich Finance
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.12.07   Time: 15:47
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1470 bzw. 1473/1497 ASPX-Seite und Lib hinzugefügt
' 
' ************************************************
