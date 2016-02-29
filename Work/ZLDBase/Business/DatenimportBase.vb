Option Explicit On 
Option Infer On
Option Strict On

Imports System
Imports CKG.Base.Kernel.Common
Imports CKG.Base.Common

Namespace Business
    Public Class DatenimportBase
        REM § Status-Report, Kunde: FFD, BAPI: Z_M_Datenimport_Ohne_Briefe,
        REM § Ausgabetabelle per Zuordnung in Web-DB.
        REM § Klasse enthält Methode zur Erzeugung d.Ausgabetabelle.

        Inherits ReportBase

#Region " Declarations"

        Protected m_strFiliale As String

#End Region

#Region " Properties"

        Public Property Filiale() As String
            Get
                Return m_strFiliale
            End Get
            Set(ByVal value As String)
                m_strFiliale = value
            End Set
        End Property

#End Region

#Region " Methods"

        Public Sub New(ByRef objUser As Kernel.Security.User, ByVal objApp As Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub

        Public Overloads Overrides Sub Fill()
            Fill(m_strAppID, m_strSessionID)
        End Sub

        '§§§ JVE 13.01.2006
        'Neue Methode eingefügt, die nur FILL aufruft.
        'Grund: Immer wieder kam es nach einem Build-Vorgang zu dem unerklärlichen Fehler: "...FILL-Methode nicht gefunden".
        '       Evtl. hilft diese Konstruktion, den Fehler in Zukunft zu vermeiden....

        Public Sub FillData(ByVal strAppID As String, ByVal strSessionID As String)
            Fill(strAppID, strSessionID)
        End Sub
        '-----------------------------------------------------------------------------------------
        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "FFD_Bank_Datenimport.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    Dim proxy = DynSapProxy.getProxy("Z_M_DATENIMPORT_OHNE_BRIEFE", m_objApp, m_objUser, PageHelper.GetCurrentPage())
                    proxy.setImportParameter("I_KNRZE", m_strFiliale)
                    proxy.setImportParameter("I_KONZS", Right("0000000000" & m_objUser.KUNNR, 10))
                    proxy.setImportParameter("I_KUNNR", "")
                    proxy.setImportParameter("I_VKORG", "1510")

                    proxy.callBapi()

                    Dim tblTemp2 As DataTable = proxy.getExportTable("GT_WEB")

                    CreateOutPut(tblTemp2, strAppID)

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
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub

        Public Sub CreateOutPut(ByVal tblTemp As DataTable, ByVal appId As String)
            Dim datatablerow As DataRow

            Dim tbTranslation As DataTable = m_objApp.ColumnTranslation(appId)
            Dim rowsTranslation As DataRow()
            rowsTranslation = tbTranslation.Select("", "DisplayOrder")

            m_tblResult = New DataTable("ResultsSAP")   'm_tblResult kommt aus der Klasse ReportBase...
            For Each datatablerow In rowsTranslation
                Dim datatablecolumn As DataColumn
                For Each datatablecolumn In tblTemp.Columns
                    If datatablecolumn.ColumnName.ToUpper = datatablerow("OrgName").ToString.ToUpper Then
                        If CType(datatablerow("IstDatum"), Boolean) = True Then
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), GetType(DateTime))
                        ElseIf CType(datatablerow("IstZeit"), Boolean) = True Then
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), GetType(String))
                        Else
                            m_tblResult.Columns.Add(Replace(datatablerow("NewName").ToString, ".", ""), datatablecolumn.DataType)
                        End If
                        m_tblResult.Columns(m_tblResult.Columns.Count - 1).ExtendedProperties.Add("Alignment", datatablerow("Alignment").ToString)
                        m_tblResult.Columns(m_tblResult.Columns.Count - 1).ExtendedProperties.Add("HeadLine", datatablerow("NewName").ToString)
                        Exit For
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
