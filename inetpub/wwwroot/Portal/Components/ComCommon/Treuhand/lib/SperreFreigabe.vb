Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Business

Namespace Treuhand
    <Serializable()> Public Class SperreFreigabe
        Inherits Base.Business.BankBase

#Region " Declarations"

        Private m_strFilename2 As String
        Private mE_SUBRC As String
        Private mE_MESSAGE As String
        Private m_strSperrEnsperr As String
        Private m_tblUpload As DataTable
        Private m_tblFahrzeuge As DataTable
        Private m_tblBestand As DataTable
        Private m_strTreunehmer As String
        Private m_strReferenceforAut As String
        Private m_strErdatvon As String
        Private m_strErdatbis As String
        Private m_Treugeber As String
#End Region

#Region " Properties "
        Public Property tblUpload() As DataTable
            Get
                Return m_tblUpload
            End Get
            Set(ByVal value As DataTable)
                m_tblUpload = value
            End Set
        End Property
        Public Property Fahrzeuge() As DataTable
            Get
                Return m_tblFahrzeuge
            End Get
            Set(ByVal value As DataTable)
                m_tblFahrzeuge = value
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
        Public Property SperrEnsperr() As String
            Get
                Return m_strSperrEnsperr
            End Get
            Set(ByVal Value As String)
                m_strSperrEnsperr = Value
            End Set
        End Property

        Public Property Bestand() As DataTable
            Get
                Return m_tblBestand
            End Get
            Set(ByVal value As DataTable)
                m_tblBestand = value
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
        Public Property ReferenceforAut() As String
            Get
                Return m_strReferenceforAut
            End Get
            Set(ByVal value As String)
                m_strReferenceforAut = value
            End Set
        End Property
        Public Property Erdatvon() As String
            Get
                Return m_strErdatvon
            End Get
            Set(ByVal value As String)
                m_strErdatvon = value
            End Set
        End Property
        Public Property Erdatbis() As String
            Get
                Return m_strErdatbis
            End Get
            Set(ByVal value As String)
                m_strErdatbis = value
            End Set
        End Property
        Public Property Treugeber() As String
            Get
                Return m_Treugeber
            End Get
            Set(ByVal value As String)
                m_Treugeber = value
            End Set
        End Property
#End Region

#Region "Methods"

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

        Public Sub GetCustomer(ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.GetCustomer"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.InitExecute("Z_M_TH_GET_TREUH_AG", "I_TREU, I_EQTYP", m_objUser.KUNNR.PadLeft(10, "0"c), "B")

                Result = S.AP.GetExportTable("GT_EXP")

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

        Public Sub GiveCars(ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.GiveCars"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.Init("Z_M_TH_INS_VORGANG", "I_TREU, I_EQTYP", Treugeber.PadLeft(10, "0"c), "B")

                Dim tbltemp As DataTable = S.AP.GetImportTable("GT_IN")

                For Each uploadRow As DataRow In tblUpload.Rows
                    If uploadRow("SELECT").ToString = "99" Then
                        Dim NewDatarow As DataRow = tbltemp.NewRow
                        NewDatarow("AG") = uploadRow("AG")
                        NewDatarow("EQUI_KEY") = uploadRow("EQUI_KEY")
                        NewDatarow("ERNAM") = uploadRow("ERNAM")
                        NewDatarow("ERDAT") = uploadRow("ERDAT")
                        NewDatarow("SPERRDAT") = uploadRow("SPERRDAT")
                        NewDatarow("TREUH_VGA") = uploadRow("TREUH_VGA")
                        NewDatarow("SUBRC") = ""
                        NewDatarow("MESSAGE") = ""
                        NewDatarow("ZZREFERENZ2") = uploadRow("ZZREFERENZ2")

                        tbltemp.Rows.Add(NewDatarow)
                    End If

                Next

                S.AP.Execute()

                m_tblFahrzeuge = S.AP.GetExportTable("GT_IN")
                m_tblFahrzeuge.Columns.Add("SELECT", GetType(System.String))
                HelpProcedures.killAllDBNullValuesInDataTable(m_tblFahrzeuge)
                E_SUBRC = S.AP.ResultCode.ToString()
                E_MESSAGE = S.AP.ResultMessage

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        E_MESSAGE = "Fehler beim Lesen der Daten."
                        E_SUBRC = "-9999"
                End Select
            End Try
        End Sub

        Public Sub AutorizeCar(ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.GiveCars"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                S.AP.Init("Z_M_TH_INS_VORGANG", "I_TREU, I_EQTYP", m_objUser.KUNNR.PadLeft(10, "0"c), "B")

                Dim tbltemp As DataTable = S.AP.GetImportTable("GT_IN")

                For Each uploadRow As DataRow In tblUpload.Rows
                    If uploadRow("SELECT").ToString = "99" AndAlso uploadRow("EQUI_KEY").ToString = m_strReferenceforAut Then
                        Dim NewDatarow As DataRow = tbltemp.NewRow
                        NewDatarow("AG") = uploadRow("AG")
                        NewDatarow("EQUI_KEY") = uploadRow("EQUI_KEY")
                        NewDatarow("ERNAM") = Left(uploadRow("ERNAM").ToString, 12)
                        NewDatarow("ERDAT") = uploadRow("ERDAT")
                        NewDatarow("SPERRDAT") = uploadRow("SPERRDAT")
                        NewDatarow("TREUH_VGA") = uploadRow("TREUH_VGA")
                        NewDatarow("SUBRC") = ""
                        NewDatarow("MESSAGE") = ""
                        NewDatarow("ZZREFERENZ2") = uploadRow("ZZREFERENZ2")

                        tbltemp.Rows.Add(NewDatarow)
                    End If

                Next

                S.AP.Execute()

                m_tblFahrzeuge = S.AP.GetExportTable("GT_IN")
                m_tblFahrzeuge.Columns.Add("SELECT", GetType(System.String))
                HelpProcedures.killAllDBNullValuesInDataTable(m_tblFahrzeuge)
                E_SUBRC = S.AP.ResultCode.ToString()
                E_MESSAGE = S.AP.ResultMessage

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        E_MESSAGE = "Fehler beim Lesen der Daten."
                        E_SUBRC = "-9999"
                End Select
            End Try
        End Sub

        Public Function GiveResultStructure() As DataTable
            If tblUpload Is Nothing Then
                tblUpload = New DataTable()
                With tblUpload.Columns
                    .Add("AG", GetType(System.String))
                    .Add("EQUI_KEY", GetType(System.String))
                    .Add("ERNAM", GetType(System.String))
                    .Add("ERDAT", GetType(System.String))
                    .Add("SPERRDAT", GetType(System.String))
                    .Add("TREUH_VGA", GetType(System.String))
                    .Add("SUBRC", GetType(System.String))
                    .Add("MESSAGE", GetType(System.String))
                    .Add("SELECT", GetType(System.String))
                    .Add("ZZREFERENZ2", GetType(System.String))
                End With
            End If
            Return Fahrzeuge
        End Function

        Public Sub GetTreuhandBestand(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "SperreFreigabe.GetTreuhandBestand"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            mE_SUBRC = ""
            mE_MESSAGE = ""

            Try
                S.AP.InitExecute("Z_DPM_TREUHANDBESTAND", "KUNNR_AG, ERDAT_VON, ERDAT_BIS",
                                 m_strTreunehmer.PadLeft(10, "0"c), m_strErdatvon, m_strErdatbis)

                Dim tblTemp As DataTable = S.AP.GetExportTable("GT_WEB")

                mE_SUBRC = S.AP.ResultCode.ToString()
                mE_MESSAGE = S.AP.ResultMessage

                m_tblBestand = CreateOutPut(tblTemp, m_strAppID)

                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblBestand)

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        m_intStatus = -9999
                        mE_MESSAGE = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select

                WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), m_tblBestand)

            End Try

        End Sub
#End Region

    End Class
End Namespace