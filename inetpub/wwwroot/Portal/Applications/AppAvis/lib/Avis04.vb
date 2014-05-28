Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common.Common
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Imports SapORM.Contracts


Public Class Avis04
 

    Inherits Base.Business.DatenimportBase

#Region " Declarations"
    Private m_tblHistory As DataTable
    Private m_strCHASSIS_NUM As String
    Private m_strEQUNR As String
    Private m_blnUnvollstaendigeTuete As Boolean
#End Region

#Region " Properties"
    Public Property UnvollstaendigeTuete() As Boolean
        Get
            Return m_blnUnvollstaendigeTuete
        End Get
        Set(ByVal Value As Boolean)
            m_blnUnvollstaendigeTuete = Value
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

    Public Property CHASSIS_NUM() As String
        Get
            Return m_strCHASSIS_NUM
        End Get
        Set(ByVal Value As String)
            m_strCHASSIS_NUM = Value
        End Set
    End Property

    Public ReadOnly Property History() As DataTable
        Get
            Return m_tblHistory
        End Get
    End Property
#End Region

#Region " Methods"
    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
        Dim tblTemp2 As DataTable
        Dim tblHerst As DataTable
        Dim intCounter As Integer

        m_strClassAndMethod = "Avis04.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try

                S.AP.InitExecute("Z_M_SCHLUESSELDIFFERENZEN", "I_KUNNR", m_objUser.KUNNR.ToSapKunnr())

                tblTemp2 = S.AP.GetExportTable("GT_WEB_OUT_BRIEFE")
                tblHerst = S.AP.GetExportTable("GT_WEB_OUT_HERST")

                Dim row As DataRow()

                tblTemp2.Columns.Add("Hersteller", GetType(System.String))  '§§§ JVE 27.09.2006: Hersteller.
                tblTemp2.Columns.Add("Delete", GetType(System.Boolean))     '§§§ JVE 16.10.2006: Löschen.
                tblTemp2.Columns.Add("Status", GetType(System.String))     '§§§ JVE 16.10.2006: Status.

                For intCounter = tblTemp2.Rows.Count - 1 To 0 Step -1


                    tblTemp2.Rows(intCounter)("Delete") = False             '§§§ JVE 16.10.2006: Löschen.
                    tblTemp2.Rows(intCounter)("Status") = String.Empty              '§§§ JVE 16.10.2006: Status.

                    '§§§ JVE 27.09.2006: Hersteller einfügen...
                    row = tblHerst.Select("CHASSIS_NUM='" & CStr(tblTemp2.Rows(intCounter)("CHASSIS_NUM")) & "'")
                    If Not (row.Length = 0) Then
                        tblTemp2.Rows(intCounter)("Hersteller") = CStr(row(0)("HERST_T"))
                    Else
                        tblTemp2.Rows(intCounter)("Hersteller") = String.Empty
                    End If
                Next

                tblTemp2.AcceptChanges()

                CreateOutPut(tblTemp2, strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub Clear(ByVal strAppID As String, ByVal strSessionID As String, ByRef myWebTable As DataTable)
        m_strClassAndMethod = "Avis04.Clear"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                Dim strKUNNR As String = m_objUser.KUNNR.ToSapKunnr()

                Dim SAPTable As DataTable = S.AP.GetImportTableWithInit("Z_M_Schluesselverloren", "I_KUNNR", strKUNNR)

                For Each tmpRow As DataRow In myWebTable.Rows

                    If CBool(tmpRow("Delete")) Then
                        Dim Row As DataRow = SAPTable.NewRow
                        Row("KUNNR") = strKUNNR
                        Row("Chassis_Num") = CStr(tmpRow("Fahrgestellnummer"))
                        Row("Equnr") = CStr(tmpRow("Equipmentnummer"))
                        Row("Flag") = "X"
                        SAPTable.Rows.Add(Row)
                        tmpRow("Status") = "OK"
                    End If

                Next
                myWebTable.AcceptChanges()
                SAPTable.AcceptChanges()

                S.AP.Execute()

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", CHASSIS_NUM=" & m_strCHASSIS_NUM & ", EQUNR=" & m_strEQUNR, m_tblResult, True)
            Catch ex As Exception
                m_intStatus = -5555
                Select Case ex.Message
                    Case Else
                        m_strMessage = ex.Message
                End Select

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR & ", CHASSIS_NUM=" & m_strCHASSIS_NUM & ", EQUNR=" & m_strEQUNR & ", " & Replace(m_strMessage, "<br>", " "), m_tblResult, True)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub
#End Region
End Class

' ************************************************
' $History: Avis04.vb $
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 14.01.10   Time: 12:45
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 7  *****************
' User: Rudolpho     Date: 28.04.09   Time: 17:09
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 28.01.09   Time: 11:24
' Updated in $/CKAG/Applications/AppAvis/lib
' ita 2431 nachbesserung
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 9.12.08    Time: 11:26
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2419 21 Tage-WebFilter entfernt
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 3.12.08    Time: 10:46
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2419 testfertig
'
'
' ************************************************