Imports CKG
Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Business
Imports System.IO

'#################################################################
' Klasse für die Auftragsannahme für Fahrer
' Change :  Aufträge (Change03)
'#################################################################


Public Class FahrerAuftraege
    Inherits Base.Business.ReportBase

#Region " Declarations"

    Private mAuftraege As DataTable
    Private mAuswahl As String = ""
   
#End Region

#Region " Properties"

    Public Property Auswahl() As String
        Get
            Return mAuswahl
        End Get
        Set(ByVal value As String)
            mAuswahl = value
        End Set
    End Property
    
    Public ReadOnly Property Auftraege() As DataTable
        Get
            Return mAuftraege
        End Get
    End Property

#End Region

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByVal objApp As Base.Kernel.Security.App, ByVal strFilename As String)
        MyBase.New(objUser, objApp, strFilename)
    End Sub

    Public Overrides Sub Fill()
        
    End Sub

    Public Overloads Sub Fill(ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: Fill
        ' Autor: JJU
        ' Beschreibung: Z_M_GET_FAHRER_AUFTRAEGE
        ' Erstellt am: 1.07.2009
        ' ITA: 2641
        '----------------------------------------------------------------------
        m_strClassAndMethod = "FahrerAuftraege.Fill"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0
        Try

            S.AP.Init("Z_M_GET_FAHRER_AUFTRAEGE")

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_GET_FAHRER_AUFTRAEGE", m_objApp, m_objUser, page)
            S.AP.setImportParameter("I_VKORG", m_objUser.Customer.AccountingArea.ToString)
            S.AP.setImportParameter("I_FAHRER", Right("0000000000" & m_objUser.Reference, 10))
            S.AP.setImportParameter("I_FAHRER_STATUS", Auswahl)

            'myProxy.callBapi()
            S.AP.Execute()

            mAuftraege = S.AP.getExportTable("GT_ORDER")

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "NO_DATA"
                    m_strMessage = "Keine Ergebnisse für die gewählten Kriterien."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub


    Public Overloads Sub Change(ByVal Auftragsnummer As String, ByVal Status As String, ByVal strAppID As String, ByVal strSessionID As String)
        '----------------------------------------------------------------------
        ' Methode: Change
        ' Autor: JJU
        ' Beschreibung: Z_M_GET_FAHRER_AUFTRAEGE
        ' Erstellt am: 1.07.2009
        ' ITA: 2641
        '----------------------------------------------------------------------
        m_strClassAndMethod = "FahrerAuftraege.Change"
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        Try

            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_M_SET_FAHRER_AUFTRAGS_STATUS", m_objApp, m_objUser, page)
            'myProxy.setImportParameter("I_VBELN", Auftragsnummer)
            'myProxy.setImportParameter("I_FAHRER_STATUS", Status)

            'myProxy.callBapi()

            S.AP.InitExecute("Z_M_SET_FAHRER_AUFTRAGS_STATUS", "I_VBELN,I_FAHRER_STATUS", Auftragsnummer, Status)

            Auftraege.Select("VBELN='" & Auftragsnummer & "'")(0)("Fahrer_Status") = Status

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "ORDER_NOT_FOUND"
                    m_strMessage = "Auftrag nicht gefunden. Auftragsnummer: " & Auftragsnummer
                Case "NO_NEW_STATUS"
                    m_strMessage = "Kein neuer Status."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & ex.Message & ")"
            End Select
        End Try
    End Sub
    
    Public Function getPDFBinaryFromSAP(ByVal Auftragsnummer As String, ByVal strAppID As String, ByVal strSessionID As String) As Byte()
        '----------------------------------------------------------------------
        ' Methode: getPDFBinaryFromSAP
        ' Autor: JJU
        ' Beschreibung: Z_M_CRE_FAHRER_AUFTRAG_PDF
        ' Erstellt am: 1.07.2009
        ' ITA: 2641
        '----------------------------------------------------------------------
        m_strAppID = strAppID
        m_strSessionID = strSessionID
        m_intStatus = 0

        'Dim con As New SAPConnection(m_BizTalkSapConnectionString)
        'con.Open()

        Try

            'Dim cmd As New SAPCommand()
            'cmd.Connection = con

            S.AP.InitExecute("Z_M_CRE_FAHRER_AUFTRAG_PDF", "I_VBELN", Auftragsnummer)

            'Dim strCom As String

            'strCom = "EXEC Z_M_CRE_FAHRER_AUFTRAG_PDF @I_VBELN=@pI_VBELN,"
            'strCom = strCom & "@E_XSTRING=@pE_XSTRING OUTPUT OPTION 'disabledatavalidation'"

            'cmd.CommandText = strCom

            ''importparameter
            'Dim pI_VBELN As New SAPParameter("@pI_VBELN", ParameterDirection.Input)
            ''exportParameter
            'Dim pE_XSTRING As New SAPParameter("@pE_XSTRING", ParameterDirection.Output)

            ''Importparameter hinzufügen
            'cmd.Parameters.Add(pI_VBELN)
            ''exportparameter hinzugfügen
            'cmd.Parameters.Add(pE_XSTRING)

            ''befüllen der Importparameter
            ''pI_VBELN.Value = Right("0000000000" & Auftragsnummer, 10)
            'pI_VBELN.Value = Auftragsnummer

            'cmd.ExecuteNonQuery()
            
            Dim ba As Byte() = S.AP.GetExportParameterByte("E_XSTRING")
            If ba IsNot Nothing And ba.Length() > 0 Then
                Return ba
            End If

                'If Not pE_XSTRING.Value Is DBNull.Value Then
                '    Return pE_XSTRING.Value
                'End If
            Return Nothing

        Catch ex As Exception
            m_intStatus = -1111
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "FORM_NOT_FOUND"
                    m_strMessage = "Form nicht gefunden."
                Case "NO_FORMULAR"
                    m_strMessage = "Kein Formular."
                Case "NO_PDF"
                    m_strMessage = "Kein PDF."
                Case Else
                    m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
            End Select
            Return Nothing
            'Finally
            '    con.Close()
        End Try
    End Function

End Class

' ************************************************
' $History: FahrerAuftraege.vb $
' 
' *****************  Version 2  *****************
' User: Jungj        Date: 2.07.09    Time: 15:24
' Updated in $/CKAG/Applications/AppKroschke/Lib
' ITA 2641 Testfertig
' 
' *****************  Version 1  *****************
' User: Jungj        Date: 1.07.09    Time: 16:45
' Created in $/CKAG/Applications/AppKroschke/Lib
' ITA 2641 unfertig
' 
' ************************************************
