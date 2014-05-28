Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> _
Public Class Adresspflege
    Inherits Base.Business.BankBase

#Region "Constructor"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String, ByVal strFileName As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, strFileName)
    End Sub

#End Region

#Region "Properties"
    Public Property Auftraggeber As String
    Public Property Kennung As String
    Public Property Referenz As String
    Public Property Name1 As String
    Public Property Name2 As String
    Public Property Strasse As String
    Public Property PLZ As String
    Public Property Ort As String
    Public Property Land As String
    Public Property Telefon As String
    Public Property Mail As String
    Public Property Fax As String
    Public Property KonzernNr As String
    Public Property Loeschkennzeichen As String
    Public Property Verarbeitungskennzeichen As String
    Public Property Ident As String
    Public Property EqTyp As String
    Public Property TableKennung As DataTable
    Public Property TableAdressen As DataTable
#End Region


    Public Overrides Sub Change()

    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Sub ChangeNew(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_PFLEGE_ZDAD_AUFTR_006", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR", m_objUser.KUNNR.PadLeft(10, "0"c))
                myProxy.setImportParameter("I_VERKZ", Verarbeitungskennzeichen)
                

                Dim TempTable As DataTable = myProxy.getImportTable("GT_WEB")

                Dim NewRow As DataRow = TempTable.NewRow

                NewRow("KENNUNG") = Kennung
                NewRow("POS_KURZTEXT") = ""
                NewRow("POS_TEXT") = Referenz
                NewRow("NAME1") = Name1
                NewRow("NAME2") = Name2
                NewRow("STRAS") = Strasse
                NewRow("PSTLZ") = PLZ
                NewRow("ORT01") = Ort
                NewRow("EMAIL") = Mail
                NewRow("LAND1") = Land
                NewRow("TELNR") = Telefon
                NewRow("FAXNR") = Fax
                'NewRow("INTNR") = KonzernNr

                If Not Ident Is Nothing Then
                    NewRow("POS_KURZTEXT") = Ident
                End If


                NewRow("AENDT") = Date.Today.ToShortDateString
                NewRow("AENUS") = m_objUser.UserName

                TempTable.Rows.Add(NewRow)
                TempTable.AcceptChanges()


                myProxy.callBapi()


                m_intStatus = CInt(myProxy.getExportParameter("E_SUBRC"))
                m_strMessage = myProxy.getExportParameter("E_MESSAGE")

            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message

            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub

    Public Sub GetKennung(ByVal page As System.Web.UI.Page)

        If Not m_blnGestartet Then
            m_blnGestartet = True

            Try
                m_intStatus = 0
                m_strMessage = ""

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_STEUERUNG_ADRPOOL", m_objApp, m_objUser, page)

                myProxy.setImportParameter("I_KUNNR_AG", m_objUser.KUNNR.PadLeft(10, "0"c))

                myProxy.callBapi()

                TableKennung = myProxy.getExportTable("GT_WEB")


            Catch ex As Exception
                m_intStatus = -9999
                m_strMessage = ex.Message

            Finally
                m_blnGestartet = False
            End Try
        End If


    End Sub


    Public Sub GetAdressenandZulStellen(ByVal page As Web.UI.Page)
        m_strClassAndMethod = "Briefversand.GetAdressenandZulStellen"
       
        m_intStatus = 0
        m_strMessage = ""
        If Not m_blnGestartet Then
            m_blnGestartet = True
            Dim intID As Int32 = -1

            Dim strKUNNR As String = Right("0000000000" & m_objUser.Customer.KUNNR, 10)

            Try

                Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_DPM_READ_ADRESSPOOL_01", m_objApp, m_objUser, Page)

                myProxy.setImportParameter("I_KUNNR_AG", strKUNNR)
                myProxy.setImportParameter("I_EQTYP", EqTyp)

                myProxy.setImportParameter("I_POS_TEXT", Referenz)
                myProxy.setImportParameter("I_NAME1", Name1)
                myProxy.setImportParameter("I_NAME2", Name2)
                myProxy.setImportParameter("I_STRAS", Strasse)
                myProxy.setImportParameter("I_PSTLZ", PLZ)
                myProxy.setImportParameter("I_ORT01", Ort)



                myProxy.callBapi()


                Dim SapTableAdressen As DataTable = myProxy.getExportTable("GT_ADRS")

                Dim SapRow As DataRow


                TableAdressen = SapTableAdressen.Clone
                TableAdressen.Columns.Add("DISPLAY", GetType(System.String))

                For Each row As DataRow In SapTableAdressen.Rows
                    SapRow = TableAdressen.NewRow
                    SapRow("IDENT") = row("IDENT").ToString
                    SapRow("DISPLAY") = row("NAME1").ToString & " - " & row("STREET").ToString & ", " & row("CITY1").ToString
                    SapRow("KUNNR") = row("KUNNR").ToString
                    SapRow("NAME1") = row("NAME1").ToString
                    SapRow("NAME2") = row("NAME2").ToString
                    SapRow("STREET") = row("STREET").ToString
                    SapRow("HOUSE_NUM1") = row("HOUSE_NUM1").ToString
                    SapRow("POST_CODE1") = row("POST_CODE1").ToString
                    SapRow("CITY1") = row("CITY1").ToString
                    SapRow("COUNTRY") = row("COUNTRY").ToString
                    SapRow("POS_TEXT") = row("POS_TEXT").ToString
                    SapRow("TELEFON") = row("TELEFON").ToString
                    SapRow("EMAIL") = row("EMAIL").ToString
                    SapRow("FAX") = row("FAX").ToString


                    TableAdressen.Rows.Add(SapRow)
                Next


                WriteLogEntry(True, "KUNNR=" & m_objUser.KUNNR, TableAdressen)



            Catch ex As Exception
                m_intStatus = -5555
                Select Case HelpProcedures.CastSapBizTalkErrorMessage(ex.Message)
                    Case "NO_ADRPOOL"
                        m_strMessage = "Keine Daten für die angegebenen Suchparameter gefunden."
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = "Beim Erstellen des Reportes ist ein Fehler aufgetreten.<br>(" & HelpProcedures.CastSapBizTalkErrorMessage(ex.Message) & ")"
                End Select
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub







End Class
