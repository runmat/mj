Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Namespace Treuhand
    Public Class Treuhandbestand
        Inherits Base.Business.DatenimportBase

#Region " Declarations"

        Private m_strFilename2 As String
        Private m_strAktion As String
        Private mE_SUBRC As String
        Private mE_MESSAGE As String
        Private m_tblFahrzeuge As DataTable
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

#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
            m_strFilename2 = strFilename
        End Sub

        Public Overloads Overrides Sub Fill()

        End Sub
        Public Overloads Sub Change()


        End Sub

        Public Overloads Sub FILL(ByVal page As Page, _
                                  ByVal preselect_AG_TG As String, ByVal kennz As String, ByVal fin As String, ByVal ref2 As String)
            m_strClassAndMethod = "Treuhandsperre.FILL"
            m_strMessage = String.Empty
            m_intStatus = 0

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_TH_BESTAND", m_objApp, m_objUser, page)

                myProxy.setImportParameter(CStr(IIf(m_strAktion = "TG", "I_TG", "I_AG")), m_objUser.KUNNR.PadLeft(10, "0"c))
                If Not String.IsNullOrEmpty(preselect_AG_TG) Then myProxy.setImportParameter(CStr(IIf(m_strAktion = "TG", "I_AG", "I_TG")), preselect_AG_TG.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_EQTYP", "B")

                myProxy.callBapi()

                Dim tblTemp As DataTable = myProxy.getExportTable("GT_BESTAND")

                tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow
                Dim strTemp As String
                For Each tmpRow In tblTemp.Rows

                    'If tmpRow("TG").ToString <> m_objUser.KUNNR Then
                    '    tmpRow("AG_NAME1") = tmpRow("TG_NAME1")
                    'End If

                    If m_strAktion <> "TG" Then
                        tmpRow("TG") = "X"
                    End If


                    strTemp = CStr(tmpRow("NAME1"))
                    If Not TypeOf tmpRow("STREET") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("STREET"))
                    End If
                    If Not TypeOf tmpRow("POST_CODE1") Is System.DBNull Then
                        strTemp &= "<br/>" & CStr(tmpRow("POST_CODE1"))
                    End If
                    If Not TypeOf tmpRow("CITY1") Is System.DBNull Then
                        strTemp &= " " & CStr(tmpRow("CITY1"))
                    End If
                    tmpRow("Versandadresse") = strTemp
                Next

                Dim filter = New List(Of String)
                If Not String.IsNullOrEmpty(kennz) Then filter.Add("LICENSE_NUM='" & kennz & "'")
                If Not String.IsNullOrEmpty(fin) Then filter.Add("CHASSIS_NUM='" & fin & "'")
                If Not String.IsNullOrEmpty(ref2) Then filter.Add("ZZREFERENZ2='" & ref2 & "'")
                If filter.Count > 0 Then
                    'System.Diagnostics.Trace.WriteLine("About to apply filter on " & tblTemp.Rows.Count & " rows")
                    Dim tblTempCopy = tblTemp.Clone
                    For Each row As DataRow In tblTemp.Select(String.Join(" and ", filter.ToArray))
                        tblTempCopy.ImportRow(row)
                    Next
                    tblTemp = tblTempCopy
                    'System.Diagnostics.Trace.WriteLine("Filter applied - " & tblTemp.Rows.Count & " rows left")
                End If

                CreateOutPut(tblTemp, m_strAppID) ' befüllt m_tblResult (und damit Result)
                ResultTable = Result

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                m_intStatus = Integer.Parse(mE_SUBRC)
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                m_strMessage = mE_MESSAGE

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                        m_strMessage = mE_MESSAGE
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblResult, False)

            End Try
        End Sub
#End Region

    End Class
End Namespace

