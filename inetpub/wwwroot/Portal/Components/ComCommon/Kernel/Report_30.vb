Option Explicit On 
Option Strict On

Imports System
Imports CKG.Base.Kernel

Namespace Kernel
    Public Class Report_30
        REM § Status-Report, Kunde: Übergreifend, BAPI: Z_DAD_FEINSTAUB,
        REM § Ausgabetabelle per Zuordnung in Web-DB.
        Inherits Base.Business.DatenimportBase

#Region " Declarations"
        Private m_strFahrgestellnummer As String
        Private m_strKennzeichen As String
        Private m_datZulassungsdatumVon As Date
        Private m_datZulassungsdatumBis As Date
        Private m_datVergabedatumVon As Date
        Private m_datVergabedatumBis As Date
        Private m_blnPlakettenartGruen As Boolean
        Private m_blnPlakettenartGelb As Boolean
        Private m_blnPlakettenartRot As Boolean
#End Region

#Region " Properties"
        Property Fahrgestellnummer() As String
            Get
                Return m_strFahrgestellnummer
            End Get
            Set(ByVal Value As String)
                m_strFahrgestellnummer = Value
            End Set
        End Property

        Property Kennzeichen() As String
            Get
                Return m_strKennzeichen
            End Get
            Set(ByVal Value As String)
                m_strKennzeichen = Value
            End Set
        End Property

        Property ZulassungsdatumVon() As Date
            Get
                Return m_datZulassungsdatumVon
            End Get
            Set(ByVal Value As Date)
                m_datZulassungsdatumVon = Value
            End Set
        End Property

        Property ZulassungsdatumBis() As Date
            Get
                Return m_datZulassungsdatumBis
            End Get
            Set(ByVal Value As Date)
                m_datZulassungsdatumBis = Value
            End Set
        End Property

        Property VergabedatumVon() As Date
            Get
                Return m_datVergabedatumVon
            End Get
            Set(ByVal Value As Date)
                m_datVergabedatumVon = Value
            End Set
        End Property

        Property VergabedatumBis() As Date
            Get
                Return m_datVergabedatumBis
            End Get
            Set(ByVal Value As Date)
                m_datVergabedatumBis = Value
            End Set
        End Property

        Property PlakettenartGruen() As Boolean
            Get
                Return m_blnPlakettenartGruen
            End Get
            Set(ByVal Value As Boolean)
                m_blnPlakettenartGruen = Value
            End Set
        End Property

        Property PlakettenartGelb() As Boolean
            Get
                Return m_blnPlakettenartGelb
            End Get
            Set(ByVal Value As Boolean)
                m_blnPlakettenartGelb = Value
            End Set
        End Property

        Property PlakettenartRot() As Boolean
            Get
                Return m_blnPlakettenartRot
            End Get
            Set(ByVal Value As Boolean)
                m_blnPlakettenartRot = Value
            End Set
        End Property
#End Region

#Region " Methods"
        Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
            MyBase.New(objUser, objApp, strFilename)
        End Sub

        Public Overloads Sub FILL(ByVal strAppID As String, ByVal strSessionID As String)
            m_strClassAndMethod = "Report_030.FILL"
            m_strAppID = strAppID
            m_strSessionID = strSessionID
            If Not m_blnGestartet Then
                m_blnGestartet = True

                Try
                    S.AP.Init("Z_DAD_FEINSTAUB", "I_KUNNR", Right("0000000000" & m_objUser.KUNNR, 10))

                    If Not m_datZulassungsdatumBis = Nothing AndAlso IsDate(m_datZulassungsdatumBis) Then
                        S.AP.SetImportParameter("I_ERDAT_BIS", m_datZulassungsdatumBis.ToShortDateString)
                    End If

                    If Not m_datZulassungsdatumVon = Nothing AndAlso IsDate(m_datZulassungsdatumVon) Then
                        S.AP.SetImportParameter("I_ERDAT_VON", m_datZulassungsdatumVon.ToShortDateString)
                    End If

                    If Not m_datVergabedatumBis = Nothing AndAlso IsDate(m_datVergabedatumBis) Then
                        S.AP.SetImportParameter("I_VERDAT_BIS", m_datVergabedatumBis.ToShortDateString)
                    End If

                    If Not m_datVergabedatumVon = Nothing AndAlso IsDate(m_datVergabedatumVon) Then
                        S.AP.SetImportParameter("I_VERDAT_VON", m_datVergabedatumVon.ToShortDateString)
                    End If

                    Dim strPlakettenart As String = ""
                    If m_blnPlakettenartGruen Then
                        strPlakettenart &= "1"
                    End If
                    If m_blnPlakettenartGelb Then
                        strPlakettenart &= "2"
                    End If
                    If m_blnPlakettenartRot Then
                        strPlakettenart &= "3"
                    End If

                    S.AP.SetImportParameter("I_ZZPLAKART", strPlakettenart)
                    S.AP.SetImportParameter("I_CHASSIS_NUM", m_strFahrgestellnummer)
                    S.AP.SetImportParameter("I_LICENSE_NUM", m_strKennzeichen)

                    S.AP.Execute()

                    Dim SAPTable As DataTable = S.AP.GetExportTable("GT_STAUB")

                    If SAPTable Is Nothing OrElse SAPTable.Rows.Count = 0 Then
                        m_intStatus = -5555
                        m_strMessage = "Keine Daten gefunden."
                    Else
                        Dim tblTemp2 As DataTable = SAPTable
                        m_tableResult = New DataTable()
                        m_tableResult.Columns.Add("Parameter", System.Type.GetType("System.String"))
                        m_tableResult.Columns.Add("Wert", System.Type.GetType("System.String"))
                        m_tableResult.Columns.Add("Suche", System.Type.GetType("System.Boolean"))

                        Dim tmpRow As DataRow
                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Kennzeichen"
                        tmpRow("Wert") = m_strKennzeichen
                        tmpRow("Suche") = True
                        m_tableResult.Rows.Add(tmpRow)

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Fahrgestellnummer"
                        tmpRow("Wert") = m_strFahrgestellnummer
                        tmpRow("Suche") = True
                        m_tableResult.Rows.Add(tmpRow)

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Zulassungsdatum"
                        If ((Not m_datZulassungsdatumVon = Nothing) AndAlso (IsDate(m_datZulassungsdatumVon))) And ((Not m_datZulassungsdatumBis = Nothing) AndAlso (IsDate(m_datZulassungsdatumBis))) Then
                            tmpRow("Wert") = m_datZulassungsdatumVon.ToShortDateString & " - " & m_datZulassungsdatumBis.ToShortDateString
                        Else
                            tmpRow("Wert") = ""
                        End If
                        tmpRow("Suche") = True
                        m_tableResult.Rows.Add(tmpRow)

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Vergabedatum"
                        If ((Not m_datVergabedatumVon = Nothing) AndAlso (IsDate(m_datVergabedatumVon))) And ((Not m_datVergabedatumBis = Nothing) AndAlso (IsDate(m_datVergabedatumBis))) Then
                            tmpRow("Wert") = m_datVergabedatumVon.ToShortDateString & " - " & m_datVergabedatumBis.ToShortDateString
                        Else
                            tmpRow("Wert") = ""
                        End If
                        tmpRow("Suche") = True
                        m_tableResult.Rows.Add(tmpRow)

                        Dim iCountGruen As Integer = 0
                        Dim iCountGelb As Integer = 0
                        Dim iCountRot As Integer = 0

                        For Each tmpRow In tblTemp2.Rows
                            Select Case CStr(tmpRow("Zzplakart"))
                                Case "1"
                                    iCountGruen += 1
                                Case "2"
                                    iCountGelb += 1
                                Case "3"
                                    iCountRot += 1
                            End Select
                        Next

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Anzahl grüne Plaketten"
                        tmpRow("Wert") = iCountGruen.ToString
                        tmpRow("Suche") = False
                        m_tableResult.Rows.Add(tmpRow)

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Anzahl gelbe Plaketten"
                        tmpRow("Wert") = iCountGelb.ToString
                        tmpRow("Suche") = False
                        m_tableResult.Rows.Add(tmpRow)

                        tmpRow = m_tableResult.NewRow
                        tmpRow("Parameter") = "Anzahl rote Plaketten"
                        tmpRow("Wert") = iCountRot.ToString
                        tmpRow("Suche") = False
                        m_tableResult.Rows.Add(tmpRow)

                        CreateOutPut(tblTemp2, strAppID)
                    End If

                    WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                Catch ex As Exception
                    m_intStatus = -9999
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
                    WriteLogEntry(False, "KUNNR=" & m_objUser.KUNNR, m_tblResult, False)
                Finally
                    m_blnGestartet = False
                End Try
            End If
        End Sub
#End Region
    End Class
End Namespace

' ************************************************
' $History: Report_30.vb $
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 13:32
' Created in $/CKAG/Components/ComCommon/Kernel
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 4.04.08    Time: 10:48
' Created in $/CKAG/portal/Components/ComCommon/Kernel
' 
' *****************  Version 3  *****************
' User: Uha          Date: 9.08.07    Time: 13:18
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Oberflächenänderung in "Vergabe von Feinstaubplaketten"
' 
' *****************  Version 2  *****************
' User: Uha          Date: 12.07.07   Time: 13:40
' Updated in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Report "Vergabe von Feinstaubplaketten" als Testversion erzeugt
' 
' *****************  Version 1  *****************
' User: Uha          Date: 11.07.07   Time: 18:22
' Created in $/CKG/Components/ComCommon/ComCommonWeb/Kernel
' Report "Vergabe von Feinstaubplaketten" im Rohbau erzeugt
' 
' ************************************************