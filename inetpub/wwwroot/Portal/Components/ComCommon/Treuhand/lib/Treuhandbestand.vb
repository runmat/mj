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

        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Treuhandsperre.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.Init("Z_M_TH_BESTAND", "I_EQTYP", "B")

                If m_strAktion = "TG" Then
                    S.AP.SetImportParameter("I_TG", Right("0000000000" & m_objUser.KUNNR, 10))
                Else
                    S.AP.SetImportParameter("I_AG", Right("0000000000" & m_objUser.KUNNR, 10))
                End If

                S.AP.Execute()

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_BESTAND")

                tblTemp.Columns.Add("Versandadresse", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow
                Dim strTemp As String
                For Each tmpRow In tblTemp.Rows

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

                CreateOutPut(tblTemp, m_strAppID)
                ResultTable = Result

                mE_SUBRC = S.AP.ResultCode.ToString()
                mE_MESSAGE = S.AP.ResultMessage

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
End Namespace

