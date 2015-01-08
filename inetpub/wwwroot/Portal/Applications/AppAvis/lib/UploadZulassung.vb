Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports SapORM.Contracts

Public Class UploadZulassung
    Inherits Base.Business.BankBase

#Region " Declarations"
    Private mZulassungsTabelle As DataTable
    Private mSAPTabelle As DataTable
    Private mSAPTabelleZulassung As DataTable
    Private m_DezZul As Boolean
#End Region

#Region " Properties"

    Public Enum Zulassungstyp
        Zulassung = 1
        Planzulassung = 2
    End Enum

    Public ReadOnly Property ZulassungsTabelle() As DataTable
        Get
            If mZulassungsTabelle Is Nothing Then
                mZulassungsTabelle = New DataTable
                With mZulassungsTabelle
                    .Columns.Add("Fahrgestellnummer", Type.GetType("System.String"))
                    .Columns.Add("MVANummer", Type.GetType("System.String"))
                    .Columns.Add("Zulassungsdatum", Type.GetType("System.String"))
                    .Columns.Add("Verarbeitungsdatum", Type.GetType("System.String"))
                    .Columns.Add("Herstellernummer", Type.GetType("System.String"))
                    .Columns.Add("Modell", Type.GetType("System.String"))
                    .Columns.Add("Modellbezeichnung", Type.GetType("System.String"))
                    .Columns.Add("geplanterLiefertermin", Type.GetType("System.String"))
                    .Columns.Add("istbezahlt", Type.GetType("System.String"))
                    .Columns.Add("Sperrdatum", Type.GetType("System.String"))
                    .Columns.Add("Sperrvermerk", Type.GetType("System.String"))
                    .Columns.Add("STATUS", Type.GetType("System.String"))
                End With
                mZulassungsTabelle.AcceptChanges()

            End If

            Return mZulassungsTabelle
        End Get
    End Property

    Public Property DezZul() As Boolean
        Get
            Return m_DezZul
        End Get
        Set(ByVal value As Boolean)
            m_DezZul = value
        End Set
    End Property

    Public Property ArtDerZulassung As Zulassungstyp

#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

    Public Overloads Overrides Sub change()
        m_strClassAndMethod = "UploadZulassung.Change"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                S.AP.Init("Z_M_MASSENZULASSUNG_006", "I_KUNNR_AG, I_WEB_USER", m_objUser.KUNNR.ToSapKunnr(), m_objUser.UserName.Left(40))

                If mSAPTabelleZulassung Is Nothing Then
                    mSAPTabelleZulassung = S.AP.GetImportTable("GT_WEB")
                Else
                    mSAPTabelleZulassung.Clear()
                End If

                Dim tmpRows() As DataRow

                If ArtDerZulassung = Zulassungstyp.Planzulassung Then
                    tmpRows = ZulassungsTabelle.Select()
                Else
                    tmpRows = ZulassungsTabelle.Select("STATUS='Für Zulassung bereit'")
                End If

                For Each tmpRow As DataRow In tmpRows
                    Dim tmpSAPRow As DataRow = mSAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)

                    Dim tmpNewRowZulassung As DataRow = mSAPTabelleZulassung.NewRow

                    tmpNewRowZulassung("CHASSIS_NUM") = tmpRow("Fahrgestellnummer").ToString
                    If ArtDerZulassung = Zulassungstyp.Planzulassung Then
                        tmpNewRowZulassung("PLZULDAT") = tmpRow("Zulassungsdatum")
                        tmpNewRowZulassung("DURCHFD") = tmpRow("Verarbeitungsdatum")
                    Else
                        tmpNewRowZulassung("ZULDAT") = tmpRow("Zulassungsdatum")
                    End If

                    tmpNewRowZulassung("REIFENART") = tmpSAPRow("REIFENART").ToString
                    tmpNewRowZulassung("ZULASSUNGSORT") = tmpSAPRow("ZULASSUNGSORT").ToString
                    tmpNewRowZulassung("VERWENDUNGSZWECK") = tmpSAPRow("VERWENDUNGSZWECK").ToString
                    tmpNewRowZulassung("DAT_SPERRE") = tmpSAPRow("DAT_SPERRE")
                    tmpNewRowZulassung("SPERRVERMERK") = tmpSAPRow("SPERRVERMERK").ToString
                    tmpNewRowZulassung("EQUNR") = tmpSAPRow("EQUNR").ToString
                    tmpNewRowZulassung("QMNUM") = tmpSAPRow("QMNUM").ToString
                    tmpNewRowZulassung("ZZCARPORT") = tmpSAPRow("ZZCARPORT").ToString
                    tmpNewRowZulassung("AKTION") = "Z" 'Zulassung
                    tmpNewRowZulassung("WEB_USER") = Left(m_objUser.UserName, 15)

                    mSAPTabelleZulassung.Rows.Add(tmpNewRowZulassung)
                Next
                mSAPTabelleZulassung.AcceptChanges()

                S.AP.Execute()

                mSAPTabelleZulassung = S.AP.GetExportTable("GT_WEB")

                'nur fehlerhafte sätze wurden zurückgeliefert
                'jetzt müssen alle stadien die wirklich übertragen worden waren, ermal den status ok bekommen
                For Each tmpRow As DataRow In tmpRows
                    tmpRow("STATUS") = "Zulassung erfolgreich beauftragt."
                Next

                'alle fehlerhaften Datensätze werden zurückgeliefert, status dort Ändern
                For Each tmpRow As DataRow In mSAPTabelleZulassung.Rows
                    ZulassungsTabelle.Select("Fahrgestellnummer='" & tmpRow("CHASSIS_NUM").ToString & "'")(0)("STATUS") = "Zulassungsbeauftragung fehlgeschlagen: " & tmpRow("FEHLER").ToString
                Next

                ZulassungsTabelle.AcceptChanges()

            Catch ex As Exception
                mZulassungsTabelle = Nothing
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Sub generateZulassungsTable(ByVal tmpZulassungsTable As DataTable)

        If ZulassungsTabelle IsNot Nothing Then
            ZulassungsTabelle.Clear()
        End If

        Try
            For Each tmpRow As DataRow In tmpZulassungsTable.Rows
                Dim tmpNewRow As DataRow = ZulassungsTabelle.NewRow

                Dim fehlerHafteRow As Boolean = False
                If Not tmpZulassungsTable.Rows(0) Is tmpRow Then
                    'nicht die überschriftsspalte

                    If tmpRow(0).ToString.Length > 0 OrElse tmpRow(1).ToString.Length > 0 Then

                        If tmpRow(0).ToString.Length = 17 Then
                            tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                            tmpNewRow("MVANummer") = tmpRow(1).ToString
                        ElseIf tmpRow(1).ToString.Length = 8 Then
                            tmpNewRow("MVANummer") = tmpRow(1).ToString
                            'muss trotzdem hinzugefügt werden, da auf die fgsn ein select getätigt wird
                            tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                        Else
                            'muss trotzdem hinzugefügt werden, da auf die fgsn ein select getätigt wird
                            tmpNewRow("Fahrgestellnummer") = tmpRow(0).ToString
                            fehlerHafteRow = True
                        End If

                        If tmpRow(2).ToString.Length > 0 AndAlso IsDate(tmpRow(2).ToString) Then
                            tmpNewRow("Zulassungsdatum") = CDate(tmpRow(2).ToString).ToShortDateString
                        Else
                            fehlerHafteRow = True
                        End If

                        If ArtDerZulassung = Zulassungstyp.Planzulassung AndAlso tmpRow(3).ToString.Length > 0 Then
                            If IsDate(tmpRow(3).ToString) Then
                                tmpNewRow("Verarbeitungsdatum") = CDate(tmpRow(3).ToString).ToShortDateString
                            Else
                                fehlerHafteRow = True
                            End If
                        End If

                        If fehlerHafteRow Then
                            tmpNewRow("Status") = "Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert"
                            ZulassungsTabelle.Rows.Add(tmpNewRow)
                        Else
                            'Statustext darf nicht NULL bzw. NOTHING sein, weil sonst die DataTable.Select-Aufrufe schiefgehen
                            tmpNewRow("Status") = ""
                            ZulassungsTabelle.Rows.Add(tmpNewRow)
                        End If
                    Else
                        'nichts tun
                    End If
                End If

            Next
            ZulassungsTabelle.AcceptChanges()
        Catch ex As Exception
            m_intStatus = -111
            m_strMessage = "Die Übergabetabelle konnte nicht generiert werden, überprüfen Sie Ihre Exceldatei. (Fehler: " & ex.Message & ")"
        End Try

    End Sub

    Public Overrides Sub Show()
        m_strClassAndMethod = "UploadZulassung.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intStatus = 0

            Try
                S.AP.Init("Z_M_FAHRGNR_READ_FZGPOOL_006", "I_KUNNR_AG, I_DEZ", m_objUser.KUNNR.ToSapKunnr(), DezZul)

                If mSAPTabelle Is Nothing Then
                    mSAPTabelle = S.AP.GetImportTable("GT_WEB")
                Else
                    mSAPTabelle.Clear()
                End If

                Dim tmpNewRow As DataRow
                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")
                    tmpNewRow = mSAPTabelle.NewRow
                    tmpNewRow("ZULDAT") = tmpRow("Zulassungsdatum")
                    tmpNewRow("CHASSIS_NUM") = tmpRow("Fahrgestellnummer").ToString
                    tmpNewRow("MVA_NUMMER") = tmpRow("MVANummer").ToString
                    mSAPTabelle.Rows.Add(tmpNewRow)
                Next
                mSAPTabelle.AcceptChanges()

                S.AP.Execute()

                mSAPTabelle = S.AP.GetExportTable("GT_WEB")

                Dim tmpRowSAP As DataRow

                For Each tmpRow As DataRow In ZulassungsTabelle.Select("STATUS<>'Fehlerhafte Uploaddatei, Fahrzeug wird ignoriert'")

                    'entweder nach MVA-Nummer oder FAhrgestellnummer zuordnen, je nach mit was sie die Daten ergänzt haben

                    If mSAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'").Length = 1 Then
                        tmpRowSAP = mSAPTabelle.Select("CHASSIS_NUM='" & tmpRow("Fahrgestellnummer").ToString & "'")(0)
                    ElseIf mSAPTabelle.Select("MVA_NUMMER='" & tmpRow("MVANummer").ToString & "'").Length = 1 Then
                        tmpRowSAP = mSAPTabelle.Select("MVA_NUMMER='" & tmpRow("MVANummer").ToString & "'")(0)
                    Else
                        Throw New Exception("eines der Angefragen Fahrzeuge konnte nicht eindeutig zugeordnet werden")
                    End If

                    If tmpRowSAP("BEZAHLTKENNZ").ToString = "X" Then
                        tmpRow("istbezahlt") = "Ja"
                    Else
                        tmpRow("istbezahlt") = "Nein"
                    End If

                    Select Case tmpRowSAP("STATUS").ToString
                        Case "1"
                            tmpRow("STATUS") = "Für Zulassung bereit"
                        Case "2"
                            tmpRow("STATUS") = "Fahrzeug bereits zugelassen"
                        Case "3"
                            tmpRow("STATUS") = "Fahrzeug gesperrt"
                        Case "4"
                            tmpRow("STATUS") = "Fahrzeug nicht zulassungsbereit"
                        Case "5"
                            tmpRow("STATUS") = "Dezentrale Zulassung"
                        Case "6"
                            tmpRow("STATUS") = "Navi CD liegt nicht vor"
                        Case "7"
                            tmpRow("STATUS") = "Zentrale Zulassung"
                        Case Else
                            tmpRow("STATUS") = "Unbekannter Status"
                    End Select

                    tmpRow("Fahrgestellnummer") = tmpRowSAP("CHASSIS_NUM").ToString
                    tmpRow("MVANummer") = tmpRowSAP("MVA_NUMMER").ToString

                    If IsDBNull(tmpRowSAP("DAT_SPERRE")) OrElse Not IsDate(tmpRowSAP("DAT_SPERRE")) OrElse CType(tmpRowSAP("DAT_SPERRE"), DateTime).Year < 1901 Then
                        tmpRow("Sperrdatum") = ""
                    Else
                        tmpRow("Sperrdatum") = CType(tmpRowSAP("DAT_SPERRE"), DateTime).ToShortDateString()
                    End If

                    tmpRow("Herstellernummer") = tmpRowSAP("HERST_NUMMER").ToString
                    tmpRow("Modell") = tmpRowSAP("ZZMODELL").ToString
                    tmpRow("Modellbezeichnung") = tmpRowSAP("ZZBEZEI").ToString
                    tmpRow("geplanterLiefertermin") = tmpRowSAP("GEPL_LIEFTERMIN").ToString
                    If tmpRowSAP("SPERRVERMERK").ToString = "X" Then
                        tmpRow("Sperrvermerk") = "Ja"
                    Else
                        tmpRow("Sperrvermerk") = "Nein"
                    End If
                Next

                ZulassungsTabelle.AcceptChanges()

            Catch ex As Exception
                mZulassungsTabelle = Nothing
                m_intStatus = -9999
                Select Case ex.Message
                    Case "NO_DATA"
                        m_strMessage = "Keine Daten vorhanden. "
                    Case Else
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                End Select

            Finally

                m_blnGestartet = False
            End Try
        End If
    End Sub

#End Region

End Class

' ************************************************
' $History: UploadZulassung.vb $
' 
' *****************  Version 12  *****************
' User: Rudolpho     Date: 6.05.10    Time: 14:15
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA: 3696
' 
' *****************  Version 11  *****************
' User: Rudolpho     Date: 25.11.09   Time: 11:10
' Updated in $/CKAG/Applications/AppAvis/lib
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 30.10.09   Time: 11:14
' Updated in $/CKAG/Applications/AppAvis/lib
' ita´s: 3216, 3155
' 
' *****************  Version 9  *****************
' User: Jungj        Date: 3.04.09    Time: 13:04
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2758 testfertig
' 
' *****************  Version 8  *****************
' User: Jungj        Date: 31.03.09   Time: 11:48
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2758
' 
' *****************  Version 7  *****************
' User: Jungj        Date: 26.03.09   Time: 9:08
' Updated in $/CKAG/Applications/AppAvis/lib
' BugFix
' 
' *****************  Version 6  *****************
' User: Jungj        Date: 23.03.09   Time: 8:34
' Updated in $/CKAG/Applications/AppAvis/lib
' ita 2739
' 
' *****************  Version 5  *****************
' User: Jungj        Date: 20.03.09   Time: 15:33
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2739 unfertig
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 15.01.09   Time: 11:21
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2457
' 
' *****************  Version 3  *****************
' User: Jungj        Date: 21.11.08   Time: 14:54
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2412 testfertig
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 21.11.08   Time: 9:59
' Updated in $/CKAG/Applications/AppAvis/lib
' ITA 2412 testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 20.11.08   Time: 14:08
' Created in $/CKAG/Applications/AppAvis/lib
' ITa 2412 torso
' 
' ************************************************