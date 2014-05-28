Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Business
Imports CKG.Base.Kernel

Public Class Vertragsdaten
    Inherits Base.Business.DatenimportBase

#Region "Declarations"

    Dim mSucheLiznr As String = ""
    Dim mVertragsDaten As DataTable
    Dim mLaender As DataTable
    Dim mTeam As DataTable
    Dim mAdressdaten As DataTable

#End Region

#Region "Properties"
    Public Property SucheLiznr() As String
        Get
            Return mSucheLiznr
        End Get
        Set(ByVal value As String)
            mSucheLiznr = value
        End Set
    End Property

    Public ReadOnly Property Vertragsdaten() As DataTable
        Get
            Return mVertragsDaten
        End Get
    End Property
    Public ReadOnly Property Laender() As DataTable
        Get
            If mLaender Is Nothing Then
                getLaender()
            End If
            Return mLaender
        End Get
    End Property

    Public ReadOnly Property Teams() As DataTable
        Get
            Return mTeam
        End Get
    End Property

    Public ReadOnly Property Adressdaten() As DataTable
        Get
            Return mAdressdaten
        End Get
    End Property



#End Region

#Region "Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overloads Overrides Sub Fill()
        Fill(m_strAppID, m_strSessionID)
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Vertragsdaten.FILL"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

    
        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_M_LESEN_LHS", "I_KUNNR,I_LIZNR", Right("00000000000" & m_objUser.KUNNR, 10), mSucheLiznr)
            
            mVertragsDaten = S.AP.GetExportTable("GT_WEB")
            mTeam = S.AP.GetExportTable("GT_TEAM")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Es wurde keine Daten zu diesem Vertrag gefunden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Es ist ein Fehler aufgetreten:<br>(" & ex.Message & ")"
            End Select
           
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), mVertragsDaten, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub FillTeam()
        m_strClassAndMethod = "Vertragsdaten.FillTeam"
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If

       
        Try
            m_intStatus = 0
            m_strMessage = ""

            S.AP.InitExecute("Z_M_GET_LN_TEAM", "I_AG", Right("00000000000" & m_objUser.KUNNR, 10))

            mTeam = S.AP.GetExportTable("ET_TEAM")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_TEAMS"
                    m_intStatus = -1111
                    m_strMessage = "Keine Teams gefunden."
                Case "NO_AG"
                    m_intStatus = -1111
                    m_strMessage = "Kein Auftraggeber angegeben."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Es ist ein Fehler aufgetreten:<br>(" & ex.Message & ")"
            End Select
           
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), mTeam, False)
        Finally
            m_blnGestartet = False
        End Try
    End Sub

    Public Sub CreateNewVertrag(ByVal liznr As String)
        mVertragsDaten = S.AP.GetImportTableWithInit("Z_M_LESEN_LHS.GT_WEB")
        Dim tmpRow As DataRow = mVertragsDaten.NewRow
        tmpRow("LIZNR") = liznr
        tmpRow("KUNNR") = Right("0000000000" & m_objUser.KUNNR, 10)
        mVertragsDaten.Rows.Add(tmpRow)
    End Sub
    
    Public Sub FillControlElementsOrSAPTable(ByRef alControls As ArrayList, ByVal changeData As Boolean)
        'füllt Formular mit den werten der Angeforderten Row aus der SAPDatenTabelle  oder füllt die SAPDatenTabelle mit werten aus dem FormularJJ2007.11.14
        Dim tmpControl As Control
        Dim tmpRow As DataRow = mVertragsDaten.Rows(0)
        Dim aStrSplittedID() As String
        Dim ex As Exception

        If changeData = True Then

            'wenn Abgearbeitet gekennzeichnet keine Änderungen vornehmen JJU2008.05.21
            For Each tmpControl In alControls

                aStrSplittedID = tmpControl.ID.Split(CChar("_"))
                'aufbau einer Contorl ID ( typ_behandlung_NameßZusammensetzungsnummer)
                'Behandlungskürzel D=Direkt, S=Sonstige, B=Bearbeiten
                'Endung ßZusammensetzungsnummer kommt nur bei Bearbeitungstyp vor
                'JJ2007.11.21
                Select Case aStrSplittedID(1)

                    Case "D"
                        fillDataRowByControl(tmpControl, tmpRow)
                    Case "B"
                        'fillRowByBearbeitetenControls(tmpControl, tmpRow)
                    Case "S"
                        'fillRowbySonstigeControls(tmpControl, tmpRow)
                    Case Else
                        ex = New Exception("Control IDs in der Eingabemaske sind nicht den Konventionen entsprechend: " & tmpControl.ID)
                        Throw ex
                End Select
            Next
        Else 'zweig wenn daten aus der DT in die das Formular gefüllt werden

            For Each tmpControl In alControls

                aStrSplittedID = tmpControl.ID.Split(CChar("_"))
                'aufbau einer Contorl ID ( typ_behandlung_NameßZusammensetzungsnummer)
                'Behandlungskürzel D=Direkt, S=Sonstige, B=Bearbeiten
                'Endung ßZusammensetzungsnummer kommt nur bei Bearbeitungstyp vor
                'JJ2007.11.21
                Select Case aStrSplittedID(1)

                    Case "D"
                        fillControlByDataRow(tmpControl, tmpRow)
                    Case "B"
                        'fillControlsByBearbeitetenRows(tmpControl, tmpRow)
                    Case "S"
                        fillSonstigeControlsByrow(tmpControl, tmpRow)
                    Case Else
                        ex = New Exception("Control IDs in der Eingabemaske sind nicht den Konventionen entsprechend: " & tmpControl.ID)
                        Throw ex
                End Select

            Next
        End If
    End Sub

    Private Sub fillSonstigeControlsByrow(ByVal tmpControl As Control, ByVal tmpRow As DataRow)

        If tmpControl.ID = "ddl_S_Geschaeftsstelle" Then
            'dieses Control wird aus einer anderen tabelle befüllt und die Werte werden in 2 Versteckte felder geschrieben.
            'txt_D_GST_KURZ und txt_D_GST_NAME, die dann automatisch ihren wert in die tabelle GT_WEB übergeben

            Dim tmpDdl As DropDownList = DirectCast(tmpControl, DropDownList)

            If Not Teams Is Nothing Then
                Dim tmpDv As DataView = Teams.DefaultView
                tmpDdl.DataSource = tmpDv
                tmpDdl.DataBind()

                If Not tmpRow("GST_KURZ").ToString = "" Then
                    'ins SAP sollen 6 Stellen geschrieben werden, die Tabelle GT_WEB gibt es mir aber 10 stellig. Anf. MSC JJU2008.09.30
                    tmpDdl.SelectedValue = Right("0000000000" & tmpRow("GST_KURZ").ToString, 10)
                End If
            Else
                tmpDdl.Visible = False
            End If

        End If

    End Sub

    Private Sub fillDataRowByControl(ByRef tmpControl As Control, ByRef tmpRow As DataRow)

        Dim tmpTextbox As New TextBox()
        Dim tmpCheckBox As New CheckBox()
        Dim tmpDropDownList As New DropDownList()

        'Jedes Die Row wird nach der ID jedes Controls selektiert abzüglich des ControlTyp Präfixes (zb txt_D_)JJ2007.11.15
        If tmpControl.GetType Is tmpTextbox.GetType Then
            tmpTextbox = CType(tmpControl, TextBox)
            If tmpTextbox.Enabled = True Then
                tmpRow(tmpTextbox.ID.Substring(tmpTextbox.ID.IndexOf("_", tmpTextbox.ID.IndexOf("_") + 1) + 1)) = tmpTextbox.Text
            End If

        ElseIf tmpControl.GetType Is tmpCheckBox.GetType Then
            tmpCheckBox = CType(tmpControl, CheckBox)
            If tmpCheckBox.Enabled = True Then
                If tmpCheckBox.Checked Then
                    tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1)) = "1"
                Else
                    tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1)) = "0"
                End If
            End If
        ElseIf tmpControl.GetType Is tmpDropDownList.GetType Then
            tmpDropDownList = CType(tmpControl, DropDownList)
            If tmpDropDownList.Enabled = True Then
                tmpRow(tmpDropDownList.ID.Substring(tmpDropDownList.ID.IndexOf("_", tmpDropDownList.ID.IndexOf("_") + 1) + 1)) = tmpDropDownList.SelectedValue
            End If
        End If

    End Sub

    Public Sub SendDataToSap(ByVal strAppID As String, ByVal strSessionID As String)
        m_strClassAndMethod = "Vertragsdaten.sendDataToSap"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        If Not m_blnGestartet Then
            m_blnGestartet = True
        End If
        
        Try

            m_intStatus = 0
            m_strMessage = ""

            S.AP.Init("Z_M_AENDERN_LHS")

            Dim tmpTable As DataTable = S.AP.GetImportTable("I_TAB")
            tmpTable.Merge(mVertragsDaten)

            S.AP.Execute()
           
        Catch ex As Exception
            Dim strError = ex.Message.Replace("Execution failed", "").Trim()
            Select Case strError
                Case "NO_LIZNR"
                    m_intStatus = -9999
                    m_strMessage = "Keine Vertragsnummer angegeben!"
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Es ist ein Fehler beim Speichern aufgetreten:<br>(" & strError & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR & "," & Replace(m_strMessage, "<br>", " "), mVertragsDaten, False)
        Finally
            m_blnGestartet = False
        End Try

    End Sub

    Private Sub getLaender()

        Try

            S.AP.InitExecute("Z_M_Land_Plz_001")

            mLaender = S.AP.GetExportTable("GT_WEB")

            mLaender.Columns.Add("Beschreibung", System.Type.GetType("System.String"))
            mLaender.Columns.Add("FullDesc", System.Type.GetType("System.String"))

            Dim rowTemp As DataRow
            For Each rowTemp In mLaender.Rows
                If CInt(rowTemp("LNPLZ")) > 0 Then
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx")) & " (" & CStr(CInt(rowTemp("LNPLZ"))) & ")"
                Else
                    rowTemp("Beschreibung") = CStr(rowTemp("Landx"))
                End If
                rowTemp("FullDesc") = CStr(rowTemp("Land1")) & " " & CStr(rowTemp("Beschreibung"))
            Next
        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "ERR_INV_PLZ"
                    m_strMessage = "Ungültige Postleitzahl."
                    m_intStatus = -1118
                Case Else
                    m_strMessage = "Unbekannter Fehler."
                    m_intStatus = -9999
            End Select
        End Try

    End Sub

    Private Sub fillControlByDataRow(ByRef tmpControl As Control, ByRef tmpRow As DataRow)
        Dim tmpTextbox As New TextBox()
        Dim tmpCheckBox As New CheckBox()
        Dim tmpDropDownList As New DropDownList


        If tmpControl.GetType Is tmpTextbox.GetType Then
            tmpTextbox = CType(tmpControl, TextBox)

            'ID nach 2. _ abschneiden so das die ID gleich ist mit dem ColumnName der ROW JJ2007.11.21
            tmpTextbox.Text = CStr(tmpRow(tmpTextbox.ID.Substring(tmpTextbox.ID.IndexOf("_", tmpTextbox.ID.IndexOf("_") + 1) + 1)))

        ElseIf tmpControl.GetType Is tmpCheckBox.GetType Then
            tmpCheckBox = CType(tmpControl, CheckBox)

            'ID nach 2. _ abschneiden so das die ID gleich ist mit dem ColumnName der ROW JJ2007.11.21
            If Not CStr(tmpRow(tmpCheckBox.ID.Substring(tmpCheckBox.ID.IndexOf("_", tmpCheckBox.ID.IndexOf("_") + 1) + 1))).Equals("1") Then
                tmpCheckBox.Checked = False
            Else
                tmpCheckBox.Checked = True
            End If

        ElseIf tmpControl.GetType Is tmpDropDownList.GetType Then
            tmpDropDownList = CType(tmpControl, DropDownList)

            'ID nach 2. _ abschneiden so das die ID gleich ist mit dem ColumnName der ROW JJ2007.11.21
            tmpDropDownList.SelectedValue = CStr(tmpRow(tmpDropDownList.ID.Substring(tmpDropDownList.ID.IndexOf("_", tmpDropDownList.ID.IndexOf("_") + 1) + 1)))
        End If

    End Sub

    ''' <summary>
    ''' Beschreibung: Ruft das BAPI Z_M_IMP_AUFTRDAT_007 auf und liefert eine
    '''               Tabelle mit Adressdaten zurück
    ''' Erstellt am:  30.09.2008
    ''' ITA:          2294
    ''' Autor:        JJU
    ''' </summary>
    ''' <param name="Kennung"></param>
    ''' <param name="Name"></param>
    ''' <param name="PLZ"></param>
    ''' <param name="Ort"></param>
    ''' <remarks></remarks>
    Public Sub GetAdresse(ByVal Kennung As String, ByVal Name As String, ByVal PLZ As String, ByVal Ort As String)
        
        m_intStatus = 0

        Try
            S.AP.InitExecute("Z_M_IMP_AUFTRDAT_007", "I_KUNNR,I_KENNUNG,I_NAME1,I_PSTLZ",
                             Right("0000000000" & m_objUser.KUNNR, 10),
                             Kennung,
                             Name,
                             PLZ)

            mAdressdaten = S.AP.GetExportTable("GT_WEB")
            
            With mAdressdaten
                .Columns.Add("Adresse", System.Type.GetType("System.String"))
            End With

            Dim row As DataRow
            Dim adresse As String

            For Each row In mAdressdaten.Rows

                'Adresse für Ausgabe in Dropdown verketten
                adresse = row("Name1").ToString & " "
                adresse = adresse & row("STRAS").ToString & " "
                adresse = adresse & row("PSTLZ").ToString & " "
                adresse = adresse & row("ORT01").ToString

                row("Adresse") = adresse
            Next

            'mAdressdaten = ExportTable
            WriteLogEntry(True, "I_KUNNR=" & m_objUser.KUNNR & ",I_KENNUNG=" & Kennung, m_tblResult)

        Catch ex As Exception
            mAdressdaten = Nothing

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -5555
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."

                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"
            End Select

            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)
        End Try

    End Sub

    Public Sub GetLnKunde(ByVal LnNummer As String)

        m_intStatus = 0
      
        Try
            S.AP.InitExecute("Z_M_GET_LN_KUNDE", "I_AG,I_EXKUNNR_ZL", Right("0000000000" & m_objUser.KUNNR, 10), LnNummer)
            
            mAdressdaten = S.AP.GetExportTable("ES_LN_KUNDE")
            
            WriteLogEntry(True, "I_AG=" & m_objUser.KUNNR & ",I_EXKUNNR_ZL=" & LnNummer, m_tblResult)

        Catch ex As Exception
            m_intStatus = -5555
            mAdressdaten = Nothing

            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_intStatus = -1111
                    m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                Case "NO_AG"
                    m_strMessage = "Kein Auftraggeber angegeben."
                Case "NO_LN"
                    m_strMessage = "Kein Leasingnehmer angegeben."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = "Fehler beim Abfragen der Leasingnehmerdaten.<br>(" & ex.Message & ")"
            End Select

            m_tblResult = Nothing
            
            WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult)

        End Try
    End Sub

#End Region

End Class
' ************************************************
' $History: Vertragsdaten.vb $
' 
' *****************  Version 11  *****************
' User: Fassbenders  Date: 17.12.08   Time: 10:43
' Updated in $/CKAG/Applications/AppAlphabet/lib
' 
' *****************  Version 10  *****************
' User: Fassbenders  Date: 15.12.08   Time: 10:21
' Updated in $/CKAG/Applications/AppAlphabet/lib
' 
' *****************  Version 9  *****************
' User: Fassbenders  Date: 12.12.08   Time: 11:13
' Updated in $/CKAG/Applications/AppAlphabet/lib
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 1.10.08    Time: 13:04
' Updated in $/CKAG/Applications/AppAlphabet/lib
' ITA 2254 testfertig
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 30.09.08   Time: 9:40
' Updated in $/CKAG/Applications/AppAlphabet/lib
' ITA 2254 unfertig
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 6.08.08    Time: 13:34
' Updated in $/CKAG/Applications/AppAlphabet/lib
' ITA 2133 nachbesserungen
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 5.08.08    Time: 18:47
' Updated in $/CKAG/Applications/AppAlphabet/lib
' 2133 testfertig
' 
' *****************  Version 4  *****************
' User: Jungj        Date: 5.08.08    Time: 13:37
' Updated in $/CKAG/Applications/AppAlphabet/lib
' 2133 weiterentwicklung
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 4.08.08    Time: 13:26
' Updated in $/CKAG/Applications/AppAlphabet/lib
' Weiterentwiclung
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 31.07.08   Time: 17:37
' Updated in $/CKAG/Applications/AppAlphabet/lib
' ITA 2133 weiterentwicklung
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 31.07.08   Time: 16:52
' Created in $/CKAG/Applications/AppAlphabet/lib
' ITA 2133 body
' 
' ************************************************
