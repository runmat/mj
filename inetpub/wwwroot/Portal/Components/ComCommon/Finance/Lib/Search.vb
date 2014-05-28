Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business

Namespace Finance
    <Serializable()> Public Class Search
        Implements Base.Common.ISapError

        REM § Lese-/Schreibfunktion, Kunde: Übergreifend
        REM § LeseHaendlerSAP_Einzeln, LeseHaendlerSAP - BAPI: Z_M_Adressdaten_001/Z_M_Adressdaten_002

#Region " Definitions"
        Private m_strHaendlerReferenzNummer As String
        Private m_strHaendlerName As String
        Private m_strHaendlerOrt As String
        Private m_tblHaendler As DataTable
        Private m_strErrorMessage As String
        Protected m_intStatus As Int32
        Protected m_blnErrorOccured As Boolean

        Private m_objApp As Base.Kernel.Security.App
        Private m_objUser As Base.Kernel.Security.User

        Private m_strCUSTOMER As String
        Private m_strNAME As String
        Private m_strNAME_2 As String
        Private m_strCOUNTRYISO As String
        Private m_strPOSTL_CODE As String
        Private m_strCITY As String
        Private m_strSTREET As String

        Private m_strREFERENZ As String

        Private m_blnGestartet As Boolean
        Private m_tblSearchResult As DataTable

        'für sucheHaendler Control
        '---------------------------------
        Private aStrHaendlernummer() As String
        Private aStrName1() As String
        Private aStrName2() As String
        Private aStrOrt() As String
        Private aStrPLZ() As String
        Private aStrStrasse() As String
        Private inthaendlerTreffer As Int32
        Private strZeigeAlle As String = ""
        Private strSucheHaendlerNr As String
        Private strSuchePLZ As String
        Private strSucheOrt As String
        Private strSucheName1 As String
        Private strSucheName2 As String
        '---------------------------------


        <NonSerialized()> Private m_strSessionID As String
        <NonSerialized()> Private m_strAppID As String
        <NonSerialized()> Private m_vwHaendler As DataView
        <NonSerialized()> Protected m_objLogApp As Base.Kernel.Logging.Trace
        <NonSerialized()> Protected m_intIDSAP As Int32
#End Region

#Region " Public Properties"

        Public Property SessionID() As String
            Get
                Return m_strSessionID
            End Get
            Set(ByVal Value As String)
                m_strSessionID = Value
            End Set
        End Property

        Public ReadOnly Property aHaendlernummer() As String()
            Get
                Return aStrHaendlernummer
            End Get
        End Property

        Public ReadOnly Property aName1() As String()
            Get
                Return aStrName1
            End Get
        End Property

        Public ReadOnly Property aName2() As String()
            Get
                Return aStrName2
            End Get
        End Property
        
        Public ReadOnly Property aOrt() As String()
            Get
                Return aStrOrt
            End Get
        End Property
        
        Public ReadOnly Property aPLZ() As String()
            Get
                Return aStrPLZ
            End Get
        End Property

        Public ReadOnly Property aStrasse() As String()
            Get
                Return aStrStrasse
            End Get
        End Property

        Public ReadOnly Property anzahlHaendlerTreffer() As Integer
            Get
                Return inthaendlerTreffer
            End Get
        End Property

        Public ReadOnly Property IDSAP() As Int32
            Get
                Return m_intIDSAP
            End Get
        End Property

        Public ReadOnly Property SearchResult() As DataTable
            Get
                Return m_tblSearchResult
            End Get
        End Property

        Public ReadOnly Property Gestartet() As Boolean
            Get
                Return m_blnGestartet
            End Get
        End Property

        Public Property HaendlerReferenzNummer() As String
            Get
                Return m_strHaendlerReferenzNummer
            End Get
            Set(ByVal Value As String)
                m_strHaendlerReferenzNummer = Value
            End Set
        End Property
        
        Public Property sucheHaendlerNr() As String
            Get
                Return strSucheHaendlerNr
            End Get
            Set(ByVal Value As String)
                strSucheHaendlerNr = Value
            End Set
        End Property

        Public Property suchePLZ() As String
            Get
                Return strSuchePLZ
            End Get
            Set(ByVal Value As String)
                strSuchePLZ = Value
            End Set
        End Property

        Public Property sucheOrt() As String
            Get
                Return strSucheOrt
            End Get
            Set(ByVal Value As String)
                strSucheOrt = Value
            End Set
        End Property
        
        Public Property sucheName1() As String
            Get
                Return strSucheName1
            End Get
            Set(ByVal Value As String)
                strSucheName1 = Value
            End Set
        End Property

        Public Property sucheName2() As String
            Get
                Return strSucheName2
            End Get
            Set(ByVal Value As String)
                strSucheName2 = Value
            End Set
        End Property

        Public Property HaendlerName() As String
            Get
                Return m_strHaendlerName
            End Get
            Set(ByVal Value As String)
                m_strHaendlerName = Value
            End Set
        End Property
        
        Public Property zeigeAlle() As Boolean
            Get
                If strZeigeAlle = "X" Then
                    Return True
                Else
                    Return False
                End If

            End Get
            Set(ByVal Value As Boolean)

                If Value Then
                    strZeigeAlle = "X"
                Else
                    strZeigeAlle = ""
                End If
            End Set
        End Property

        Public Property HaendlerOrt() As String
            Get
                Return m_strHaendlerOrt
            End Get
            Set(ByVal Value As String)
                m_strHaendlerOrt = Value
            End Set
        End Property

        Public ReadOnly Property Haendler() As DataView
            Get
                Return m_vwHaendler
            End Get
        End Property
        
        Public ReadOnly Property CUSTOMER() As String
            Get
                Return m_strCUSTOMER.TrimStart("0"c)
            End Get
        End Property

        Public ReadOnly Property NAME() As String
            Get
                Return m_strNAME
            End Get
        End Property

        Public ReadOnly Property NAME_2() As String
            Get
                Return m_strNAME_2
            End Get
        End Property

        Public ReadOnly Property COUNTRYISO() As String
            Get
                Return m_strCOUNTRYISO
            End Get
        End Property

        Public ReadOnly Property POSTL_CODE() As String
            Get
                Return m_strPOSTL_CODE
            End Get
        End Property

        Public ReadOnly Property CITY() As String
            Get
                Return m_strCITY
            End Get
        End Property

        Public ReadOnly Property STREET() As String
            Get
                Return m_strSTREET
            End Get
        End Property

        Public ReadOnly Property REFERENZ() As String
            Get
                Return m_strREFERENZ
            End Get
        End Property

        Public ReadOnly Property Errorcode() As String Implements Base.Common.ISapError.ErrorCode
            Get
                Return m_intStatus.ToString
            End Get
        End Property

        Public ReadOnly Property ErrorMessage() As String Implements Base.Common.ISapError.ErrorMessage
            Get
                Return m_strErrorMessage
            End Get
        End Property

        Public ReadOnly Property ErrorOccured() As Boolean Implements Base.Common.ISapError.ErrorOccured
            Get
                Return m_blnErrorOccured
            End Get
        End Property
#End Region

#Region " Public Methods"
        Public Sub New(ByRef objApp As Base.Kernel.Security.App, ByRef objUser As Base.Kernel.Security.User, ByVal strSessionID As String, ByVal strAppID As String)
            m_objApp = objApp
            m_objUser = objUser

            m_strHaendlerReferenzNummer = ""
            m_strHaendlerName = ""
            m_strHaendlerOrt = ""

            m_strSessionID = strSessionID
            m_strAppID = strAppID
        End Sub

        Public Sub ReNewSAPDestination(ByVal strSessionID As String, ByVal strAppID As String)
            m_strSessionID = strSessionID
            m_strAppID = strAppID
        End Sub

        Public Function LeseHaendlerSAP_Einzeln(ByVal strAppID As String, ByVal strSessionID As String, ByVal InputReferenz As String) As Boolean
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            m_strErrorMessage = ""
            Dim blnReturn As Boolean

            Try
                S.AP.InitExecute("Z_M_Adressdaten_001", "I_KUNNR, I_KONZS, I_VKORG",
                                 Right("0000000000" & InputReferenz, 10), Right("0000000000" & m_objUser.KUNNR, 10), "1510")

                m_strCUSTOMER = ""
                m_strNAME = ""
                m_strNAME_2 = ""
                m_strCOUNTRYISO = ""
                m_strPOSTL_CODE = ""
                m_strCITY = ""
                m_strSTREET = ""
                m_strREFERENZ = ""

                Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                Select Case SAPReturnTable.Rows.Count
                    Case 0
                        m_strErrorMessage = "Kein Suchergebnis."

                        blnReturn = False
                    Case 1

                        Dim SAPReturnTableRow As DataRow = SAPReturnTable.Rows(0)

                        m_strCUSTOMER = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)
                        m_strREFERENZ = SAPReturnTableRow("Kunnr").ToString
                        m_strNAME = SAPReturnTableRow("Name1").ToString
                        m_strNAME_2 = SAPReturnTableRow("Name2").ToString
                        m_strCITY = SAPReturnTableRow("Ort01").ToString
                        m_strPOSTL_CODE = SAPReturnTableRow("Pstlz").ToString
                        m_strSTREET = SAPReturnTableRow("Stras").ToString
                        m_strCOUNTRYISO = SAPReturnTableRow("Land1").ToString

                        blnReturn = True
                    Case Else
                        m_strErrorMessage = "Suchergebnis nicht eindeutig."

                        blnReturn = False
                End Select

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler """ & InputReferenz & """ erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                End If
            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                    Case Else
                        m_strErrorMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                End Select
                blnReturn = False
                WriteLogEntry(False, "Suche nach Händler """ & InputReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try
            Return blnReturn

        End Function

        Public Function LeseHaendlerSAP(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "") As Int32
            ResetHaendlerTabelle()
            Dim strTempReferenz As String = InputReferenz
            Dim intReturn As Int32

            ClearError()

            m_strAppID = strAppID
            m_strSessionID = strSessionID

            Try
                If InputReferenz.Length = 0 Then
                    strTempReferenz = m_strHaendlerReferenzNummer
                End If

                S.AP.InitExecute("Z_M_Adressdaten_001", "I_KUNNR, I_KONZS, I_VKORG",
                                 Right("0000000000" & InputReferenz, 10), Right("0000000000" & m_objUser.Customer.KUNNR, 10), "1510")

                Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")

                intReturn = SAPReturnTable.Rows.Count
                If SAPReturnTable.Rows.Count = 0 Then
                    m_strErrorMessage = "Kein Suchergebnis."
                Else

                    Dim newDealerDetailRow As DataRow

                    newDealerDetailRow = m_tblHaendler.NewRow()
                    newDealerDetailRow("REFERENZ") = "000000"
                    newDealerDetailRow("DISPLAY") = " - keine Auswahl - "
                    m_tblHaendler.Rows.Add(newDealerDetailRow)

                    Dim SAPReturnTableRow As DataRow

                    For I As Integer = 0 To SAPReturnTable.Rows.Count - 1
                        SAPReturnTableRow = SAPReturnTable.Rows(I)

                        newDealerDetailRow = m_tblHaendler.NewRow()
                        newDealerDetailRow("CUSTOMER") = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)

                        newDealerDetailRow("REFERENZ") = SAPReturnTableRow("Kunnr").ToString.TrimStart("0"c)

                        newDealerDetailRow("NAME") = SAPReturnTableRow("Name1")
                        newDealerDetailRow("NAME_2") = SAPReturnTableRow("Name2")
                        newDealerDetailRow("CITY") = SAPReturnTableRow("Ort01")
                        newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow("Pstlz")
                        newDealerDetailRow("STREET") = SAPReturnTableRow("Stras")
                        newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow("Land1")
                        '-------------------------------------------------------
                        'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
                        'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
                        'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
                        'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
                        '-------------------------------------------------------
                        newDealerDetailRow("DISPLAY") = SAPReturnTableRow("Name1").ToString & " " & SAPReturnTableRow("Name2").ToString & "  -  " & SAPReturnTableRow("Stras").ToString & ", " & SAPReturnTableRow("Ort01").ToString

                        newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow("Name1")
                        If Not SAPReturnTableRow("Name2").ToString.Trim(" "c).Length = 0 Then
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Name2").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        Else
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        End If

                        m_tblHaendler.Rows.Add(newDealerDetailRow)
                    Next

                    m_vwHaendler = m_tblHaendler.DefaultView

                    If SAPReturnTable.Rows.Count > 1 Then
                        'Weitere Auswahl entspechend Name und/oder Ort
                        Dim filterExp As String = ""

                        If m_strHaendlerReferenzNummer.Trim(" "c).Length = 0 Then
                            If (Len(Trim(m_strHaendlerName)) > 0) And (Len(Trim(m_strHaendlerOrt)) > 0) Then
                                filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "' AND CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                            ElseIf Len(Trim(m_strHaendlerName)) > 0 Then
                                filterExp = "NAME like '" & Replace(m_strHaendlerName, "'", "''") & "'"
                            ElseIf Len(Trim(m_strHaendlerOrt)) > 0 Then
                                filterExp = "CITY like '" & Replace(m_strHaendlerOrt, "'", "''") & "'"
                            End If
                        End If

                        If filterExp.Length > 0 Then
                            filterExp = Replace(filterExp, "*", "%")
                            m_vwHaendler.RowFilter = filterExp
                            intReturn = m_vwHaendler.Count
                        End If
                        'Suche mit mehreren Ergebnissen
                    End If
                    m_vwHaendler.Sort = "NAME"
                End If

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler """ & strTempReferenz & """ erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message).Replace(" Execution failed", "").TrimEnd()
                    Case "NO_WEB"
                        intReturn = 0
                        RaiseError("0000", "Keine Web-Tabelle erstellt.")
                    Case "NO_DATA"
                        intReturn = 0
                        RaiseError("0000", "Keine Eingabedaten gefunden.")
                    Case "HAENDLER_NOT_FOUND"
                        RaiseError("0001", "Keine Händler gefunden!")
                    Case Else
                        RaiseError("9999", HelpProcedures.CastSapBizTalkErrorMessage(ex.Message))
                        intReturn = 0
                End Select
                WriteLogEntry(False, "Suche nach Händler """ & strTempReferenz & """ nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try

            Return intReturn
        End Function

        Public Function LeseHaendlerForSucheHaendlerControl(ByVal strAppID As String, ByVal strSessionID As String, Optional ByVal InputReferenz As String = "") As Int32
            Dim I As Int32
            Dim intReturn As Int32 = 0

            ClearError()

            Try
                ResetHaendlerTabelle()

                S.AP.Init("Z_M_Adressdaten_002")
                S.AP.SetImportParameter("I_KUNNR", sucheHaendlerNr)
                S.AP.SetImportParameter("I_KONZS", Right("0000000000" & m_objUser.Customer.KUNNR, 10))
                S.AP.SetImportParameter("I_VKORG", "1510")
                S.AP.SetImportParameter("I_ALL", strZeigeAlle)
                S.AP.SetImportParameter("I_MAX", "200")
                S.AP.SetImportParameter("I_NAME1", sucheName1)
                S.AP.SetImportParameter("I_NAME2", sucheName2)
                S.AP.SetImportParameter("I_ORT", sucheOrt)
                S.AP.SetImportParameter("I_PSTLZ", suchePLZ)
                S.AP.Execute()

                If IsNumeric(S.AP.GetExportParameter("E_REC_ANZ")) Then
                    inthaendlerTreffer = CInt(S.AP.GetExportParameter("E_REC_ANZ"))
                End If
                Dim SAPReturnTable As DataTable = S.AP.GetExportTable("GT_WEB")
                Dim SAPReturnTableRow As DataRow

                If SAPReturnTable.Rows.Count = 0 Then
                    m_strErrorMessage = "Kein Suchergebnis."
                Else
                    Dim newDealerDetailRow As DataRow

                    'arrays Dimensionieren
                    ReDim aStrHaendlernummer(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrName1(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrName2(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrOrt(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrPLZ(SAPReturnTable.Rows.Count - 1)
                    ReDim aStrStrasse(SAPReturnTable.Rows.Count - 1)

                    'nur für testzwecke!! 
                    For I = 0 To SAPReturnTable.Rows.Count - 1

                        SAPReturnTableRow = SAPReturnTable.Rows(I)

                        'ArraysBefüllen
                        '-------------------------------------
                        aStrHaendlernummer(I) = stringZeichenEntfernen(SAPReturnTableRow("Kunnr").ToString)
                        aStrName1(I) = stringZeichenEntfernen(SAPReturnTableRow("Name1").ToString)
                        aStrName2(I) = stringZeichenEntfernen(SAPReturnTableRow("Name2").ToString)
                        aStrOrt(I) = stringZeichenEntfernen(SAPReturnTableRow("Ort01").ToString)
                        aStrPLZ(I) = stringZeichenEntfernen(SAPReturnTableRow("Pstlz").ToString)
                        aStrStrasse(I) = stringZeichenEntfernen(SAPReturnTableRow("Stras").ToString)
                        '-------------------------------------

                        newDealerDetailRow = m_tblHaendler.NewRow()
                        newDealerDetailRow("CUSTOMER") = SAPReturnTableRow("Konzs").ToString.TrimStart("0"c)

                        newDealerDetailRow("REFERENZ") = SAPReturnTableRow("Kunnr").ToString.TrimStart("0"c)

                        newDealerDetailRow("NAME") = SAPReturnTableRow("Name1").ToString
                        newDealerDetailRow("NAME_2") = SAPReturnTableRow("Name2").ToString
                        newDealerDetailRow("CITY") = SAPReturnTableRow("Ort01").ToString
                        newDealerDetailRow("POSTL_CODE") = SAPReturnTableRow("Pstlz").ToString
                        newDealerDetailRow("STREET") = SAPReturnTableRow("Stras").ToString
                        newDealerDetailRow("COUNTRYISO") = SAPReturnTableRow("Land1").ToString
                        '-------------------------------------------------------
                        'newDealerDetailRow("DISPLAY") = SAPReturnTableRow.Kunnr.TrimStart("0"c) & " - " & SAPReturnTableRow.Name1 & ", " & SAPReturnTableRow.Ort01
                        'es soll mit tastendruck ein händler aus der dll ausgewählt werden, das geht nur wenn der 1. wert in der DDL das suchkriterium ist, hier der name des Händlers-
                        'Händler nummer soll in anzeige komplett verschwinden> JJU//Rothe 2008.03.03
                        'Neuer Aufbau laut Rothe 2008.03.04: Name1,Name2,Str, Ort,.
                        '-------------------------------------------------------
                        newDealerDetailRow("DISPLAY") = SAPReturnTableRow("Name1").ToString & " " & SAPReturnTableRow("Name2").ToString & "  -  " & SAPReturnTableRow("Stras").ToString & ", " & SAPReturnTableRow("Ort01").ToString

                        newDealerDetailRow("DISPLAY_ADDRESS") = SAPReturnTableRow("Name1")
                        If Not SAPReturnTableRow("Name2").ToString.Trim(" "c).Length = 0 Then
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Name2").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        Else
                            newDealerDetailRow("DISPLAY_ADDRESS") = newDealerDetailRow("DISPLAY_ADDRESS").ToString & ", " & SAPReturnTableRow("Land1").ToString & "-" & SAPReturnTableRow("Pstlz").ToString & " " & SAPReturnTableRow("Ort01").ToString & ", " & SAPReturnTableRow("Stras").ToString
                        End If

                        m_tblHaendler.Rows.Add(newDealerDetailRow)
                    Next
                    m_vwHaendler = m_tblHaendler.DefaultView

                    m_vwHaendler.Sort = "NAME"
                End If

                If m_strErrorMessage.Length = 0 Then
                    WriteLogEntry(True, "Suche nach Händler  erfolgreich.")
                Else
                    WriteLogEntry(False, "Suche nach Händler nicht erfolgreich.  (" & m_strErrorMessage & ")")
                End If

            Catch ex As Exception
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_WEB"
                        m_strErrorMessage = "Keine Web-Tabelle erstellt."
                        intReturn = -1
                    Case "NO_DATA"
                        m_strErrorMessage = "Keine Eingabedaten gefunden."
                        intReturn = -2
                    Case Else
                        m_strErrorMessage = HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                        intReturn = -2
                End Select
                WriteLogEntry(False, "Suche nach Händler nicht erfolgreich. (" & m_strErrorMessage & ")")
            End Try
            Return intReturn
        End Function

        Private Function stringZeichenEntfernen(ByVal zeichenkette As String) As String
            If zeichenkette Is Nothing OrElse zeichenkette Is String.Empty Then
                zeichenkette = "-"
            Else
                If Not zeichenkette.IndexOf("'") = -1 Then
                    zeichenkette = zeichenkette.Replace("'", "")
                End If
                If Not zeichenkette.IndexOf("""") = -1 Then
                    zeichenkette = zeichenkette.Replace("""", "")
                End If
            End If

            Return zeichenkette
        End Function

        Public Sub WriteLogEntry(ByVal blnSuccess As Boolean, ByVal strComment As String)
            Try
                If m_objLogApp Is Nothing Then
                    m_objLogApp = New Base.Kernel.Logging.Trace(m_objApp.Connectionstring, m_objApp.SaveLogAccessSAP, m_objApp.LogLevel)
                End If

                Dim p_strType As String = "ERR"
                Dim p_strComment As String = strComment
                If blnSuccess Then
                    p_strType = "DBG"
                End If
                m_objLogApp.WriteEntry(p_strType, m_objUser.UserName, m_strSessionID, CInt(m_strAppID), m_objUser.Applications.Select("AppID = '" & m_strAppID & "'")(0)("AppFriendlyName").ToString, "Report", p_strComment, m_objUser.CustomerName, m_objUser.Customer.CustomerId, m_objUser.IsTestUser, 0)
            Catch ex As Exception
                m_objApp.WriteErrorText(1, m_objUser.UserName, "DADReports", "WriteLogEntry", ex.ToString)
            End Try
        End Sub

        Private Sub ResetHaendlerTabelle()
            m_tblHaendler = New DataTable()
            m_vwHaendler = Nothing

            m_tblHaendler.Columns.Add("CUSTOMER", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("REFERENZ", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("NAME", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("NAME_2", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("CITY", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("STREET", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("DISPLAY", System.Type.GetType("System.String"))
            m_tblHaendler.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))
        End Sub

        Public Function LeseAdressenSAP(ByVal strAppID As String, ByVal strSessionID As String, ByVal strParentNode As String, Optional ByVal adressart As String = "A", Optional ByVal nodelevel As String = "99") As Int32 'HEZ: "B" oder "C"

            Try
                m_tblSearchResult = New DataTable()
                m_tblSearchResult.Columns.Add("ADDRESSNUMBER", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("DISPLAY_ADDRESS", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("POSTL_CODE", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("STREET", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("COUNTRYISO", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("CITY", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("NAME", System.Type.GetType("System.String"))
                m_tblSearchResult.Columns.Add("NAME_2", System.Type.GetType("System.String"))

                S.AP.Init("Z_M_Customer_Get_Children")
                S.AP.SetImportParameter("AG", Right("0000000000" & m_objUser.KUNNR, 10))
                S.AP.SetImportParameter("VALID_ON", Now.ToShortDateString)
                S.AP.SetImportParameter("CUSTHITYP", adressart)
                S.AP.SetImportParameter("NODE_LEVEL", nodelevel)
                S.AP.SetImportParameter("CUSTOMERNO", Right("0000000000" & strParentNode, 10))
                S.AP.SetImportParameter("WEB_GID", m_objUser.GroupID.ToString)
                S.AP.SetImportParameter("WEB_OID", m_objUser.Organization.OrganizationId.ToString)
                S.AP.Execute()

                'Dim SAPSalesTable As DataTable = S.AP.GetExportTable("SALES_AREA")
                'Dim SAPReturnTable As DataTable = S.AP.GetExportTable("RETURN")
                Dim SAPAdressenTable As DataTable = S.AP.GetExportTable("GT_ADRESSEN")
                Dim SAPTableRow As DataRow

                Dim newDealerDetailRow As DataRow
                'Dim firstStep As Boolean = True

                For Each SAPTableRow In SAPAdressenTable.Rows
                    If SAPAdressenTable.Rows.Count > 0 Then

                        newDealerDetailRow = m_tblSearchResult.NewRow

                        Dim strTemp As String = SAPTableRow("Name1").ToString
                        If SAPTableRow("Name2").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name2").ToString
                        End If
                        If SAPTableRow("Name3").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name3").ToString
                        End If
                        If SAPTableRow("Name4").ToString.Length > 0 Then
                            strTemp &= ", " & SAPTableRow("Name4").ToString
                        End If

                        newDealerDetailRow("DISPLAY_ADDRESS") = strTemp & ", " & SAPTableRow("Countryiso").ToString & " - " & SAPTableRow("Post_Code1").ToString & " " & SAPTableRow("City1").ToString & ", " & SAPTableRow("Street").ToString
                        newDealerDetailRow("ADDRESSNUMBER") = SAPTableRow("Haendler").ToString
                        If SAPTableRow("Post_Code1").ToString.Length > 0 Then
                            newDealerDetailRow("POSTL_CODE") = SAPTableRow("Post_Code1")
                        End If
                        If SAPTableRow("Street").ToString.Length > 0 Then
                            newDealerDetailRow("STREET") = SAPTableRow("Street")
                        End If
                        If SAPTableRow("Countryiso").ToString.Length > 0 Then
                            newDealerDetailRow("COUNTRYISO") = SAPTableRow("Countryiso")
                        End If
                        If SAPTableRow("City1").ToString.Length > 0 Then
                            newDealerDetailRow("CITY") = SAPTableRow("City1")
                        End If
                        If SAPTableRow("Name1").ToString.Length > 0 Then
                            newDealerDetailRow("NAME") = SAPTableRow("Name1")
                        End If
                        If SAPTableRow("Name2").ToString.Length > 0 Then
                            newDealerDetailRow("NAME_2") = SAPTableRow("Name2")
                        End If

                        m_tblSearchResult.Rows.Add(newDealerDetailRow)
                    End If

                Next
                Return m_tblSearchResult.Rows.Count
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        '''<summary>
        ''' Setzt den Fehlerzustand der Klasse zurück
        '''</summary>
        Public Sub ClearError() Implements Base.Common.ISapError.ClearError
            m_blnErrorOccured = False
            m_intStatus = 0
            m_strErrorMessage = ""
        End Sub

        '''<summary>
        ''' Löst ein Fehlerereignis mit Fehlercode und Fehlermeldung aus
        '''</summary>
        Public Sub RaiseError(errorcode As String, message As String) Implements Base.Common.ISapError.RaiseError
            m_blnErrorOccured = True
            m_intStatus = CInt(errorcode)
            m_strErrorMessage = message
        End Sub

#End Region
        
    End Class
End Namespace

' ************************************************
' $History: Search.vb $
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 24.06.09   Time: 16:00
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 finalisierung
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 23.06.09   Time: 17:50
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 2918 Z_M_Customer_Get_Children
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 19.06.09   Time: 10:10
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITa 2918 testfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 14.05.08   Time: 17:03
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1855
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.05.08    Time: 14:01
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1855
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 30.04.08   Time: 12:32
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1855
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 29.04.08   Time: 16:24
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' ITA 1855
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 24.04.08   Time: 8:44
' Updated in $/CKAG/Components/ComCommon/Finance/Lib
' Migration AKF-Entwicklungen
' 
' *****************  Version 15  *****************
' User: Jungj        Date: 2.04.08    Time: 13:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1758
' 
' *****************  Version 14  *****************
' User: Jungj        Date: 20.03.08   Time: 16:07
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1758
' 
' *****************  Version 13  *****************
' User: Jungj        Date: 20.03.08   Time: 8:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 12  *****************
' User: Jungj        Date: 11.03.08   Time: 8:15
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 11  *****************
' User: Jungj        Date: 6.03.08    Time: 15:39
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Eigener Finance Proxy Fertigstellung
' 
' *****************  Version 10  *****************
' User: Jungj        Date: 4.03.08    Time: 18:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' AKF ANPASSUNGEN ITA 1733
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 4.03.08    Time: 10:03
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1733, 1667, 1738 
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 1.03.08    Time: 13:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Änderung der Händlernummer auf 10 stellen mit führenden 0 
' 
' *****************  Version 7  *****************
' User: Uha          Date: 7.01.08    Time: 18:47
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 4.01.08    Time: 16:01
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Bugfix RTFS
' 
' *****************  Version 5  *****************
' User: Uha          Date: 19.12.07   Time: 16:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 4  *****************
' User: Uha          Date: 13.12.07   Time: 13:58
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1481/1509 (Änderung / Sperrung Händlerkontingent) Testversion
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 13.12.07   Time: 11:14
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.12.07   Time: 15:56
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' ITA 1473/1497 (Mahnstufe 3) als Testversion; ITA 1481/1509
' (Änderung/Sperrung Händlerkontingent) komplierfähig
' 
' *****************  Version 1  *****************
' User: Uha          Date: 12.12.07   Time: 13:23
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Finance/Lib
' Übernahme der Suchfunktion aus FFD (ohne Anpassung)
' 
' ************************************************
