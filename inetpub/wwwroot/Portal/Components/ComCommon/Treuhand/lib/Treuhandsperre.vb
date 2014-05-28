Option Explicit On

Imports CKG.Base.Kernel
Imports CKG.Base.Business


Namespace Treuhand
    <Serializable()> Public Class Treuhandsperre
        Inherits Base.Business.BankBase

#Region " Declarations"

        Private m_strFilename2 As String
        Private m_strAktion As String
        Private mE_SUBRC As String
        Private mE_MESSAGE As String
        Private m_strFreigabeVon As String
        Private m_strFreigabeBis As String
        Private m_strTreunehmer As String
        Private m_strFinForAut As String

#End Region

#Region " Properties"
        Public ReadOnly Property FileName() As String
            Get
                Return m_strFilename2
            End Get
        End Property
        Public Property Aktion() As String
            Get
                Return m_strAktion
            End Get
            Set(ByVal value As String)
                m_strAktion = value
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

        Public Property FreigabeVon() As String
            Get
                Return m_strFreigabeVon
            End Get
            Set(ByVal value As String)
                m_strFreigabeVon = value
            End Set
        End Property
        Public Property FreigabeBis() As String
            Get
                Return m_strFreigabeBis
            End Get
            Set(ByVal value As String)
                m_strFreigabeBis = value
            End Set
        End Property
        Public Property Treunehmer() As String
            Get
                Return m_strTreunehmer
            End Get
            Set(ByVal value As String)
                m_strTreunehmer = value
            End Set
        End Property
        Public Property FinforAut() As String
            Get
                Return m_strFinforAut
            End Get
            Set(ByVal value As String)
                m_strFinforAut = value
            End Set
        End Property

#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strAppID, strSessionID, strFilename)
            m_strFilename2 = strFilename
        End Sub

        Public Overloads Sub Fill()

        End Sub

        Public Overrides Sub Change()

        End Sub

        Public Overrides Sub Show()

        End Sub

        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Treuhandsperre.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.InitExecute("Z_DPM_READ_VERSAND_SPERR_001", "I_TREU, I_AKTION, I_FREIGABEDAT_VON, I_FREIGABEDAT_BIS",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_strAktion, m_strFreigabeVon, m_strFreigabeBis)

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_OUT")

                tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Ersteller", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("zurFreigabe", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow
                Dim strTemp As String
                For Each tmpRow In tblTemp.Rows
                    strTemp = CStr(tmpRow("NAME1_ZS"))
                    If Not TypeOf tmpRow("NAME2_ZS") Is System.DBNull Then
                        strTemp &= " " & CStr(tmpRow("NAME2_ZS"))
                    End If
                    If Not TypeOf tmpRow("STRASSE_ZS") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("STRASSE_ZS"))
                    End If
                    If Not TypeOf tmpRow("PLZ_ZS") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("PLZ_ZS"))
                    End If
                    If Not TypeOf tmpRow("ORT_ZS") Is System.DBNull Then
                        strTemp &= " " & CStr(tmpRow("ORT_ZS"))
                    End If
                    tmpRow("Versandadresse") = strTemp

                    tmpRow("Ersteller") = tmpRow("ERNAM").ToString

                    If IsDate(tmpRow("ERDAT")) Then
                        tmpRow("Ersteller") = tmpRow("Ersteller").ToString & " " & CDate(tmpRow("ERDAT").ToString).ToShortDateString
                    End If

                    tmpRow("zurFreigabe") = ""
                Next

                Result = CreateOutPut(tblTemp, m_strAppID)

                mE_SUBRC = S.AP.ResultCode.ToString()
                mE_MESSAGE = S.AP.ResultMessage

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)

            End Try
        End Sub

        Public Overloads Sub FILLReport(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Treuhandsperre.FillReport"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.InitExecute("Z_DPM_ZVERSANDSPERR", "KUNNR_AG, SPERRSTATUS, FREIGABE_VON, FREIGABE_BIS",
                                 Right("0000000000" & m_objUser.KUNNR, 10), m_strAktion, m_strFreigabeVon, m_strFreigabeBis)

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

                tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Ersteller", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("zurFreigabe", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow
                Dim strTemp As String
                For Each tmpRow In tblTemp.Rows
                    strTemp = CStr(tmpRow("NAME1_ZS"))
                    If Not TypeOf tmpRow("STRAS_ZS") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("STRAS_ZS"))
                    End If
                    If Not TypeOf tmpRow("PSTLZ_ZS") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("PSTLZ_ZS"))
                    End If
                    If Not TypeOf tmpRow("ORT01_ZS") Is System.DBNull Then
                        strTemp &= " " & CStr(tmpRow("ORT01_ZS"))
                    End If
                    tmpRow("Versandadresse") = strTemp

                    tmpRow("Ersteller") = tmpRow("ERNAM").ToString

                    If IsDate(tmpRow("ERDAT")) Then
                        tmpRow("Ersteller") = tmpRow("Ersteller").ToString & " " & CDate(tmpRow("ERDAT").ToString).ToShortDateString
                    End If

                    tmpRow("zurFreigabe") = ""
                Next

                Result = CreateOutPut(tblTemp, m_strAppID)

                mE_SUBRC = S.AP.ResultCode.ToString()
                mE_MESSAGE = S.AP.ResultMessage

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)

            End Try
        End Sub

        Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Treuhandsperre.Change"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.Init("Z_DPM_FREIG_VERSAND_SPERR_001", "I_FREIGABEUSER", Left(m_objUser.UserName, 30))

                Dim tblTemp As DataTable = S.AP.GetImportTable("GT_WEB")
                Dim tmpRow As DataRow

                For Each tmpRow In Result.Rows
                    If tmpRow("zurFreigabe").ToString = "X" Then
                        Dim RowNew As DataRow
                        RowNew = tblTemp.NewRow
                        RowNew("BELNR") = tmpRow("BELNR").ToString
                        RowNew("SPERRSTATUS") = m_strAktion
                        RowNew("NICHT_FREIG_GRU") = tmpRow("Ablehnungsgrund").ToString
                        RowNew("BEM") = ""
                        tblTemp.Rows.Add(RowNew)
                    End If
                Next

                S.AP.Execute()

                mE_SUBRC = S.AP.ResultCode.ToString()
                mE_MESSAGE = S.AP.ResultMessage

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Speichern der Daten ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)

            End Try
        End Sub

        Public Overloads Sub Loeschen(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Treuhandsperre.Loeschen"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                Dim tmpRow As DataRow
                For Each tmpRow In Result.Rows
                    If tmpRow("zurFreigabe").ToString = "X" Then

                        S.AP.Init("Z_DPM_FREIG_VERSAND_SPERR_001", "I_FREIGABEUSER, I_TREU",
                                  Left(m_objUser.UserName, 30), Right("0000000000" & tmpRow("Kunnr").ToString, 10))

                        Dim tblTemp As DataTable = S.AP.GetImportTable("GT_WEB")

                        Dim RowNew As DataRow
                        RowNew = tblTemp.NewRow

                        RowNew("BELNR") = tmpRow("BELNR").ToString
                        RowNew("SPERRSTATUS") = m_strAktion
                        RowNew("NICHT_FREIG_GRU") = tmpRow("Ablehnungsgrund").ToString
                        RowNew("BEM") = ""
                        tblTemp.Rows.Add(RowNew)

                        S.AP.Execute()

                        mE_SUBRC = S.AP.ResultCode.ToString()
                        mE_MESSAGE = S.AP.ResultMessage

                        WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

                    End If
                Next

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Speichern der Daten ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult)

            End Try
        End Sub

#End Region

    End Class
End Namespace

