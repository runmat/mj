Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Imports SapORM.Contracts

Namespace Business
    Public Class DatenimportBase
        REM § Status-Report, Kunde: FFD, BAPI: Z_M_Datenimport_Ohne_Briefe,
        REM § Ausgabetabelle per Zuordnung in Web-DB.
        REM § Klasse enthält Methode zur Erzeugung d.Ausgabetabelle.

        Inherits ReportBase

#Region " Declarations"
        Private m_blnAll As Boolean
        Protected m_strFiliale As String
#End Region

#Region " Properties"
        Public Property Filiale() As String
            Get
                Return m_strFiliale
            End Get
            Set(ByVal Value As String)
                m_strFiliale = Value
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Kernel.Security.User, ByVal objApp As Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
            m_blnAll = False
        End Sub

        Public Overloads Overrides Sub Fill()
            FILL(m_strAppID, m_strsessionid)
        End Sub

        '§§§ JVE 13.01.2006
        'Neue Methode eingefügt, die nur FILL aufruft.
        'Grund: Immer wieder kam es nach einem Build-Vorgang zu dem unerklärlichen Fehler: "...FILL-Methode nicht gefunden".
        '       Evtl. hilft diese Konstruktion, den Fehler in Zukunft zu vermeiden....

        Public Sub FillData(ByVal strAppID As String, ByVal strSessionID As String)
            FILL(strAppID, strSessionID)
        End Sub
        '-----------------------------------------------------------------------------------------
        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "FFD_Bank_Datenimport.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True

                'Dim objSAP As New SAPProxy_Base.SAPProxy_Base()
                'objSAP.Connection = New SAP.Connector.SAPConnection(m_objSAPDestination)
                'objSAP.Connection.Open()

                Dim intID As Int32 = -1

                Try
                    'Dim SAPTable As New SAPProxy_Base.ZFFDIMPORTTable()
                    Dim strKUNNR As String = ""

                    intID = m_objlogApp.WriteStartDataAccessSAP(m_objUser.UserName, m_objUser.IsTestUser, "Z_M_Datenimport_Ohne_Briefe", strAppID, strSessionID, m_objUser.CurrentLogAccessASPXID)
                    'objSAP.Z_M_Datenimport_Ohne_Briefe(m_strFiliale, Right("0000000000" & m_objUser.KUNNR, 10), strKUNNR, "1510", SAPTable)
                    'objSAP.CommitWork()

                    'Dim tblTemp2 As DataTable = SAPTable.ToADODataTable
                    Dim tblTemp2 As DataTable = S.AP.GetExportTableWithInitExecute("Z_M_Datenimport_Ohne_Briefe.GT_WEB",
                                                                                   "I_KUNNR, I_KNRZE, I_VKORG",
                                                                                   m_objUser.KUNNR.ToSapKunnr(), m_strFiliale, "1510")

                    If intID > -1 Then
                        m_objLogApp.WriteEndDataAccessSAP(intID, True)
                    End If

                    CreateOutPut(tblTemp2, strAppID)
                    WriteLogEntry(True, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR=", m_tblResult, False)
                Catch ex As Exception
                    m_intStatus = -1111
                    Select Case ex.Message
                        Case "NO_DATA"
                            m_strMessage = "Keine Eingabedaten vorhanden."
                        Case "NO_WEB"
                            m_strMessage = "Keine Web-Tabelle erstellt."
                        Case "NO_HAENDLER"
                            m_strMessage = "Keine Ergebnisse gefunden."
                            'm_strMessage = "Händler nicht vorhanden."
                        Case Else
                            m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    End Select
                    If intID > -1 Then
                        m_objlogApp.WriteEndDataAccessSAP(intID, False, m_strMessage)
                    End If
                    WriteLogEntry(False, "KNRZE=" & m_strFiliale & ", KONZS=" & m_objUser.KUNNR & ", KUNNR= , " & Replace(m_strMessage, "<br>", " "), m_tblResult, False)
                Finally
                    If intID > -1 Then
                        m_objlogApp.WriteStandardDataAccessSAP(intID)
                    End If

                    'objSAP.Connection.Close()
                    'objSAP.Dispose()

                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub CreateOutPut(ByVal tblTemp As DataTable, ByVal AppId As String)
            Dim datatablerow As DataRow
            Dim tbTranslation As New DataTable()

            tbTranslation = m_objApp.ColumnTranslation(AppId)
            Dim rowsTranslation As DataRow()
            rowsTranslation = tbTranslation.Select("", "DisplayOrder")

            m_tblResult = New DataTable("ResultsSAP")   'm_tblResult kommt aus der Klasse ReportBase...
            For Each datatablerow In rowsTranslation
                Dim datatablecolumn As DataColumn
                For Each datatablecolumn In tblTemp.Columns
                    If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                        If CType(datatablerow("IstDatum"), System.Boolean) = True Then
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), System.Type.GetType("System.DateTime"))
                        ElseIf CType(datatablerow("IstZeit"), System.Boolean) = True Then
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), GetType(System.String))
                        Else
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), datatablecolumn.DataType)
                        End If
                        m_tblResult.Columns(m_tblResult.Columns.Count - 1).ExtendedProperties.Add("Alignment", datatablerow("Alignment").ToString)
                        m_tblResult.Columns(m_tblResult.Columns.Count - 1).ExtendedProperties.Add("HeadLine", datatablerow("NewName").ToString)
                    End If
                Next
            Next

            Dim rowResult As DataRow
            For Each rowResult In tblTemp.Rows
                Dim rowNew As DataRow
                rowNew = m_tblResult.NewRow

                For Each datatablerow In rowsTranslation
                    Dim datatablecolumn As DataColumn
                    For Each datatablecolumn In tblTemp.Columns
                        If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                            If CType(datatablerow("NullenEntfernen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString).ToString.TrimStart("0"c)
                            ElseIf CType(datatablerow("TextBereinigen"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Replace(rowResult(datatablerow("OrgName").ToString).ToString, "<(>&<)>", "und")
                            ElseIf CType(datatablerow("IstDatum"), System.Boolean) = True Then
                                If Not IsDate(rowResult(datatablerow("OrgName").ToString)) Then
                                    Dim strTempDate As String = Right(rowResult(datatablerow("OrgName").ToString).ToString, 2) & "." & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 5, 2) & "." & Left(rowResult(datatablerow("OrgName").ToString).ToString, 4)
                                    If IsDate(strTempDate) Then
                                        rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = CDate(strTempDate)
                                        'rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Format(CDate(strTempDate), "dd.MM.yyyy")
                                    End If
                                Else
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString)
                                End If

                            ElseIf CType(datatablerow("IstZeit"), System.Boolean) = True Then
                                Dim strTempDate As String = Left(rowResult(datatablerow("OrgName").ToString).ToString, 2) & ":" & Mid(rowResult(datatablerow("OrgName").ToString).ToString, 3, 2) & ":" & Right(rowResult(datatablerow("OrgName").ToString).ToString, 2)
                                If IsDate(strTempDate) Then
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = Format(CDate(strTempDate), "HH:mm:ss").ToString
                                End If
                            ElseIf CType(datatablerow("ABEDaten"), System.Boolean) = True Then
                                rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = "<a href=""../Shared/Change06_3.aspx?EqNr=" & rowResult(datatablerow("OrgName").ToString).ToString & """ Target=""_blank"">Anzeige</a>"
                            Else
                                If TypeOf rowResult(datatablerow("OrgName").ToString) Is String Then
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString).ToString.Trim(" "c)
                                Else
                                    rowNew(Replace(datatablerow("NewName").ToString, ".", "")) = rowResult(datatablerow("OrgName").ToString)
                                End If
                            End If
                        End If
                    Next
                Next
                m_tblResult.Rows.Add(rowNew)
            Next
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: DatenimportBase.vb $
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 14.12.09   Time: 15:02
' Updated in $/CKAG/Base/Business
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 5.12.08    Time: 12:54
' Updated in $/CKAG/Base/Business
' Anpassung ColumnTranslation für DynProxy
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 10.04.08   Time: 17:37
' Updated in $/CKAG/Base/Business
' Migration
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 3.04.08    Time: 16:42
' Created in $/CKAG/Base/Business
' 
' *****************  Version 9  *****************
' User: Uha          Date: 9.08.07    Time: 11:09
' Updated in $/CKG/Base/Base/Business
' Spalte "IstZeit" in Translation übernommen
' 
' *****************  Version 8  *****************
' User: Uha          Date: 2.07.07    Time: 15:39
' Updated in $/CKG/Base/Base/Business
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 7  *****************
' User: Uha          Date: 23.05.07   Time: 15:51
' Updated in $/CKG/Base/Base/Business
' Aspose.Total.lic ist in eingebettete Ressource umgewandelt; Methode
' CreateOutPut in DatenimportBase.vb gibt jetzt wieder Datumswerte zurück
' 
' *****************  Version 6  *****************
' User: Uha          Date: 22.05.07   Time: 9:31
' Updated in $/CKG/Base/Base/Business
' Nacharbeiten + Bereinigungen
' 
' *****************  Version 5  *****************
' User: Uha          Date: 1.03.07    Time: 16:32
' Updated in $/CKG/Base/Base/Business
' 
' ************************************************