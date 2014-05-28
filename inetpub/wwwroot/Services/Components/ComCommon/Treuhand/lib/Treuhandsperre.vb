Option Explicit On

Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common


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
        Private m_strFinforAut As String
        Private m_strTreugeber As String
        Private m_strTreunehmer As String

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

        Public Property FinforAut() As String
            Get
                Return m_strFinforAut
            End Get
            Set(ByVal value As String)
                m_strFinforAut = value
            End Set
        End Property

        Public Property Treugeber() As String
            Get
                Return m_strTreugeber
            End Get
            Set(ByVal value As String)
                m_strTreugeber = value
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
        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "Treuhandsperre.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_VERSAND_SPERR_001", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_TREU", m_strTreugeber)
                myProxy.setImportParameter("I_AKTION", m_strAktion)
                myProxy.setImportParameter("I_FREIGABEDAT_VON", m_strFreigabeVon)
                myProxy.setImportParameter("I_FREIGABEDAT_BIS", m_strFreigabeBis)
                myProxy.setImportParameter("I_NAME", m_objUser.LastName)
                myProxy.setImportParameter("I_VORNA", m_objUser.FirstName)
                myProxy.setImportParameter("I_EMAIL", m_objUser.Email)
                If Not String.IsNullOrEmpty(m_strTreunehmer) Then
                    myProxy.setImportParameter("I_AG", m_strTreunehmer)
                End If

                myProxy.callBapi()

                Dim tblTemp As DataTable = myProxy.getExportTable("GT_OUT")

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

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

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
        Public Overloads Sub FILLReport(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "Treuhandsperre.FillReport"
            m_strAppID = strAppID
            m_strSessionID = strSessionID


            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_ZVERSANDSPERR", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("SPERRSTATUS", m_strAktion)
                myProxy.setImportParameter("FREIGABE_VON", m_strFreigabeVon)
                myProxy.setImportParameter("FREIGABE_BIS", m_strFreigabeBis)

                myProxy.callBapi()

                Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")

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

                CreateOutPut(tblTemp, m_strAppID)

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

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
        Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "Treuhandsperre.Change"
            m_strAppID = strAppID
            m_strSessionID = strSessionID


            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FREIG_VERSAND_SPERR_001", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_TREU", Right("0000000000" & m_objUser.KUNNR, 10))
                myProxy.setImportParameter("I_FREIGABEUSER", Left(m_objUser.UserName, 30))

                Dim tblTemp As DataTable = myProxy.getImportTable("GT_WEB")
                Dim tmpRow As DataRow

                For Each tmpRow In Result.Rows
                    If tmpRow("zurFreigabe").ToString = "X" Then
                        Dim RowNew As DataRow
                        RowNew = tblTemp.NewRow
                        RowNew("BELNR") = tmpRow("BELNR").ToString
                        RowNew("SPERRSTATUS") = m_strAktion
                        If tmpRow.Table.Columns.Contains("Ablehnungsgrund") Then
                            RowNew("NICHT_FREIG_GRU") = tmpRow("Ablehnungsgrund").ToString
                        Else
                            RowNew("NICHT_FREIG_GRU") = tmpRow("NICHT_FREIG_GRU").ToString
                        End If
                        RowNew("BEM") = ""
                        tblTemp.Rows.Add(RowNew)
                    End If
                Next

                myProxy.callBapi()

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

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
        Public Overloads Sub Loeschen(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Page)
            m_strClassAndMethod = "Treuhandsperre.Loeschen"
            m_strAppID = strAppID
            m_strSessionID = strSessionID


            Try
                Dim tmpRow As DataRow
                For Each tmpRow In Result.Rows
                    If tmpRow("zurFreigabe").ToString = "X" Then
                        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FREIG_VERSAND_SPERR_001", m_objApp, m_objUser, page)

                        myProxy.setImportParameter("I_FREIGABEUSER", Left(m_objUser.UserName, 30))
                        myProxy.setImportParameter("I_TREU", Right("0000000000" & tmpRow("Kunnr").ToString, 10))

                        Dim tblTemp As DataTable = myProxy.getImportTable("GT_WEB")

                        Dim RowNew As DataRow
                        RowNew = tblTemp.NewRow

                        RowNew("BELNR") = tmpRow("BELNR").ToString
                        RowNew("SPERRSTATUS") = m_strAktion
                        If tmpRow.Table.Columns.Contains("Ablehnungsgrund") Then
                            RowNew("NICHT_FREIG_GRU") = tmpRow("Ablehnungsgrund").ToString
                        Else
                            RowNew("NICHT_FREIG_GRU") = tmpRow("NICHT_FREIG_GRU").ToString
                        End If
                        RowNew("BEM") = ""
                        tblTemp.Rows.Add(RowNew)

                        myProxy.callBapi()

                        mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                        mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

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

