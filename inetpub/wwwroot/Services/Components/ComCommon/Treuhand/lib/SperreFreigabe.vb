Option Explicit On
Option Strict On

Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports CKG.Base.Common

Namespace Treuhand
    <Serializable()> Public Class SperreFreigabe
        Inherits Base.Business.BankBase

#Region " Declarations"
        Private mE_SUBRC As String
        Private mE_MESSAGE As String
        Private m_strSperrEnsperr As String
        Private m_tblUpload As DataTable
        Private m_tblFahrzeuge As DataTable
        Private m_tblBestand As DataTable
        Private m_strTreunehmer As String
        Private m_strFilename2 As String
        Private m_strReferenceforAut As String
        Private m_strErdatvon As String
        Private m_strErdatbis As String
        Private m_Treugeber As String
        Private m_alleTreugeber As String
        Private m_IsSperren As Boolean

        Private m_AG As String
        Private m_TREU As String
        Private m_ZSelect As String

        Private m_tblAuthorization As DataTable

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

        Public Property alleTreugeber() As String
            Get
                Return m_alleTreugeber
            End Get
            Set(ByVal value As String)
                m_alleTreugeber = value
            End Set
        End Property

        Public Property IsSperren() As Boolean
            Get
                Return m_IsSperren
            End Get
            Set(ByVal value As Boolean)
                m_IsSperren = value
            End Set
        End Property

        Public Property TREU() As String
            Get
                Return m_TREU
            End Get
            Set(ByVal value As String)
                m_TREU = value
            End Set
        End Property

        Public Property AG() As String
            Get
                Return m_AG
            End Get
            Set(ByVal value As String)
                m_AG = value
            End Set
        End Property

        Public Property ZSelect() As String
            Get
                Return m_ZSelect
            End Get
            Set(ByVal value As String)
                m_ZSelect = value
            End Set
        End Property

        Public Property tblAuthorization() As DataTable
            Get
                Return m_tblAuthorization
            End Get
            Set(ByVal value As DataTable)
                m_tblAuthorization = value
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

        Public Sub GetCustomer(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_TH_GET_TREUH_AG", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_EQTYP", "B")
                myProxy.setImportParameter("I_ALL_AG", m_alleTreugeber)
                myProxy.callBapi()

                Result = myProxy.getExportTable("GT_EXP")

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
        Public Sub GiveCars(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal kombiUpload As Boolean = False)

            m_strClassAndMethod = "SperreFreigabe.GiveCars"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_TH_INS_VORGANG", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_TREU", Treugeber.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_EQTYP", "B")
                If kombiUpload Then
                    myProxy.setImportParameter("I_KOMBI", "X")
                End If

                Dim tbltemp As DataTable = myProxy.getImportTable("GT_IN")

                For Each uploadRow As DataRow In tblUpload.Rows
                    If uploadRow("SELECT").ToString = "99" Then
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
                        If Not String.IsNullOrEmpty(uploadRow("VERTR_BEGINN").ToString()) Then
                            NewDatarow("VERTR_BEGINN") = uploadRow("VERTR_BEGINN")
                        End If
                        If Not String.IsNullOrEmpty(uploadRow("VERTR_ENDE").ToString()) Then
                            NewDatarow("VERTR_ENDE") = uploadRow("VERTR_ENDE")
                        End If
                        tbltemp.Rows.Add(NewDatarow)
                    End If

                Next

                myProxy.callBapi()

                m_tblFahrzeuge = myProxy.getExportTable("GT_IN")
                m_tblFahrzeuge.Columns.Add("ID", GetType(System.String))
                m_tblFahrzeuge.Columns.Add("SELECT", GetType(System.String))
                m_tblFahrzeuge.Columns.Add("ERROR", GetType(System.String))

                For index = 0 To m_tblFahrzeuge.Rows.Count - 1
                    '  m_tblFahrzeuge.Select("ID='1")(index)("ID") = index + 1
                    m_tblFahrzeuge.Rows(index).SetField("ID", index + 1)

                Next

                E_SUBRC = myProxy.getExportParameter("E_SUBRC")
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        E_MESSAGE = "Fehler beim Lesen der Daten: " & ex.Message
                        E_SUBRC = "-9999"
                End Select
            End Try
        End Sub
        Public Sub AutorizeCar(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.GiveCars"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_TH_INS_VORGANG", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_TREU", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_EQTYP", "B")

                Dim tbltemp As DataTable = myProxy.getImportTable("GT_IN")

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

                myProxy.callBapi()

                m_tblFahrzeuge = myProxy.getExportTable("GT_IN")
                m_tblFahrzeuge.Columns.Add("SELECT", GetType(System.String))
                E_SUBRC = myProxy.getExportParameter("E_SUBRC")
                E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        E_MESSAGE = "Fehler beim Lesen der Daten: " & ex.Message
                        E_SUBRC = "-9999"
                End Select
            End Try
        End Sub

        Public Sub SaveAddressData(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String)

            m_strClassAndMethod = "SperreFreigabe.SaveAddressData"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                If m_tblFahrzeuge IsNot Nothing AndAlso m_tblFahrzeuge.Rows.Count > 0 Then
                    Dim rowsOk As DataRow() = m_tblFahrzeuge.Select("SUBRC = 0")

                    If rowsOk.Count > 0 Then
                        Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_FILL_VERSAUFTR", m_objApp, m_objUser, page)

                        myProxy.setImportParameter("KUNNR_AG", tblUpload.Rows(0)("AG").ToString().PadLeft(10, "0"c))

                        Dim tbltemp As DataTable = myProxy.getImportTable("GT_IN")

                        For Each uploadRow As DataRow In tblUpload.Rows

                            'Adressdaten für das Fahrzeug nur speichern, wenn dafür der vorangegangene Bapi-Aufruf geklappt hat (SUBRC=0, s.o.)
                            If rowsOk.FirstOrDefault(Function(f) f("EQUI_KEY").ToString() = uploadRow("EQUI_KEY").ToString()) IsNot Nothing Then
                                Dim NewDatarow As DataRow = tbltemp.NewRow

                                NewDatarow("CHASSIS_NUM") = uploadRow("EQUI_KEY")
                                If Not String.IsNullOrEmpty(uploadRow("Versanddatum").ToString()) Then
                                    NewDatarow("VERSDAT_MIN") = uploadRow("Versanddatum")
                                End If
                                NewDatarow("ZZKONZS_ZS") = uploadRow("Haendlernummer")
                                NewDatarow("ZZNAME1_ZS") = uploadRow("Haendlername")
                                NewDatarow("ZZNAME2_ZS") = uploadRow("Name2")
                                NewDatarow("ZZPSTLZ_ZS") = uploadRow("PLZ")
                                NewDatarow("ZZORT01_ZS") = uploadRow("Ort")
                                If Not String.IsNullOrEmpty(uploadRow("Postfach").ToString()) Then
                                    'Postfach
                                    NewDatarow("ZZSTRAS_ZS") = uploadRow("Postfach")
                                Else
                                    'Normale Adresse
                                    NewDatarow("ZZSTRAS_ZS") = uploadRow("Strasse")
                                End If
                                NewDatarow("ZZKUNNR_AG") = uploadRow("AG").ToString().PadLeft(10, "0"c)
                                NewDatarow("MATNR") = "1391".PadLeft(18, "0"c)
                                NewDatarow("ZZBRFVERS") = "1"
                                NewDatarow("ABCKZ") = "2"
                                NewDatarow("ZKUNNR_TG") = m_objUser.KUNNR.PadLeft(10, "0"c)
                                NewDatarow("TREUGEBER_VERS") = m_objUser.KUNNR.PadLeft(10, "0"c)

                                tbltemp.Rows.Add(NewDatarow)
                            End If

                        Next

                        myProxy.callBapi()

                        Dim tblErrors As DataTable = myProxy.getExportTable("GT_ERR")

                        If tblErrors.Rows.Count > 0 Then
                            For Each errorRow As DataRow In tblErrors.Rows
                                Dim rows As DataRow() = tblUpload.Select("EQUI_KEY = '" & errorRow("CHASSIS_NUM").ToString() & "'")
                                If rows.Length > 0 Then
                                    rows(0)("SUBRC") = "-9999"
                                    rows(0)("MESSAGE") = errorRow("BEMERKUNG")
                                End If
                            Next
                            tblUpload.AcceptChanges()
                        End If               

                        E_SUBRC = myProxy.getExportParameter("E_SUBRC")
                        E_MESSAGE = myProxy.getExportParameter("E_MESSAGE")
                    End If
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case Else
                        E_MESSAGE = "Fehler beim Speichern der Adressdaten: " & ex.Message
                        E_SUBRC = "-9999"
                End Select
            End Try
        End Sub

        Public Overloads Sub FILLAuthorization(ByVal page As Page, ByVal strAppID As String, ByVal strSessionID As String, ByVal strKunnrTG As String, ByVal strKunnrTN As String)
            m_strClassAndMethod = "SperreFreigabe.FILLAuthorization"
            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TREUHAND_AUTHORITY", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_TG", Right("0000000000" & strKunnrTG, 10))
                myProxy.setImportParameter("I_KUNNR_TN", Right("0000000000" & strKunnrTN, 10))
                myProxy.setImportParameter("I_NAME", m_objUser.LastName)
                myProxy.setImportParameter("I_VORNA", m_objUser.FirstName)
                myProxy.setImportParameter("I_EMAIL", m_objUser.Email)

                myProxy.callBapi()

                Dim tblTemp As New DataTable
                tblTemp.Columns.Add("Freigeben", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Sperren", System.Type.GetType("System.String"))
                tblTemp.Columns.Add("Entsperren", System.Type.GetType("System.String"))

                Dim tmpRow As DataRow = tblTemp.NewRow
                tmpRow("Freigeben") = myProxy.getExportParameter("E_FREIGABE")
                tmpRow("Sperren") = myProxy.getExportParameter("E_SPERREN")
                tmpRow("Entsperren") = myProxy.getExportParameter("E_ENTSPERREN")
                tblTemp.Rows.Add(tmpRow)

                m_tblAuthorization = tblTemp

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

        Public Function GiveResultStructure() As DataTable
            If tblUpload Is Nothing Then
                tblUpload = New DataTable()
                With tblUpload.Columns
                    .Add("ID", GetType(System.String))
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
                    .Add("ERROR", GetType(System.String))
                    .Add("VERTR_BEGINN", GetType(System.String))
                    .Add("VERTR_ENDE", GetType(System.String))
                    'Versanddaten
                    .Add("Versanddatum", GetType(System.String))
                    .Add("Haendlernummer", GetType(System.String))
                    .Add("Haendlername", GetType(System.String))
                    .Add("Name2", GetType(System.String))
                    .Add("Strasse", GetType(System.String))
                    .Add("PLZ", GetType(System.String))
                    .Add("Ort", GetType(System.String))
                    .Add("Postfach", GetType(System.String))
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
                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_TREUHANDBESTAND", m_objApp, m_objUser, page)

                myProxy.setImportParameter("KUNNR_AG", m_strTreunehmer.PadLeft(10, "0"c))
                myProxy.setImportParameter("ERDAT_VON", m_strErdatvon)
                myProxy.setImportParameter("ERDAT_BIS", m_strErdatbis)

                myProxy.callBapi()

                Dim tblTemp As DataTable = myProxy.getExportTable("GT_WEB")

                mE_SUBRC = myProxy.getExportParameter("E_SUBRC")
                mE_MESSAGE = myProxy.getExportParameter("E_MESSAGE")

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