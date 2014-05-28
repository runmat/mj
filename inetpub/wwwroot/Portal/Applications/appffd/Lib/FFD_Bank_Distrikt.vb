Option Explicit On
Option Strict On

Imports System
Imports CKG.Base.Kernel
Imports CKG.Base.Common
Imports CKG.Base.Business

<Serializable()> Public Class FFD_Bank_Distrikt

    Inherits Base.Business.BankBase

    Dim tblRightsResult As DataTable
    Dim tblDistrictsResult As DataTable
    Dim m_sHaendler As String
    Dim m_sDistrikt As String
    Dim m_sKundNr As String
    Dim m_sKUNNR_HA As String
    Dim m_sDistriktID As String


#Region "Properties"
    Public Property Rights() As DataTable
        Get
            Return tblRightsResult
        End Get
        Set(ByVal Value As DataTable)
            tblRightsResult = Value
        End Set
    End Property

    Public Property Haendler() As String
        Get
            Return m_sHaendler
        End Get
        Set(ByVal Value As String)
            m_sHaendler = Value
        End Set
    End Property
    Public Property Districts() As DataTable
        Get
            Return tblDistrictsResult
        End Get
        Set(ByVal Value As DataTable)
            tblDistrictsResult = Value
        End Set
    End Property
    Public Property Distrikt() As String
        Get
            Return m_sDistrikt
        End Get
        Set(ByVal Value As String)
            m_sDistrikt = Value
        End Set
    End Property
    Public Property sDistriktID() As String
        Get
            Return m_sDistriktID
        End Get
        Set(ByVal Value As String)
            m_sDistriktID = Value
        End Set
    End Property
    Public ReadOnly Property KundNR() As String
        Get
            Return m_sKundNr
        End Get
    End Property
    Public ReadOnly Property KundNRHaendler() As String
        Get
            Return m_sKUNNR_HA
        End Get
    End Property
#End Region

#Region " Methods"

    Public Sub New(ByRef objUser As Base.Kernel.Security.User, ByRef objApp As Base.Kernel.Security.App, ByVal strAppID As String, ByVal strSessionID As String)
        MyBase.New(objUser, objApp, strAppID, strSessionID, "")
    End Sub

    Public Overloads Sub Show(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)
        m_strClassAndMethod = "FFD_Bank_Distrikt.Show"
        If Not m_blnGestartet Then
            m_blnGestartet = True

            m_intIDSAP = -1
            Dim sKunnr As String = ""
            Dim sDistriktname As String = ""
            Dim sHaendler As String = Haendler
            Try
                m_intStatus = 0
                m_strMessage = ""

                'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Haendlerzuordnung_Get", m_objApp, m_objUser, page)

                'myProxy.setImportParameter("I_HAENDLER", sHaendler)
                'myProxy.setImportParameter("I_BUKRS", "1510")

                'myProxy.callBapi()

                S.AP.InitExecute("Z_V_Haendlerzuordnung_Get", "I_HAENDLER,I_BUKRS", sHaendler, "1510")

                sKunnr = S.AP.GetExportParameter("O_KUNNR_HA") 'myProxy.getExportParameter("O_KUNNR_HA")
                sDistriktname = S.AP.GetExportParameter("O_NAME1") 'myProxy.getExportParameter("O_NAME1")
                tblDistrictsResult = S.AP.GetExportTable("OT_DISTRIKT") 'myProxy.getExportTable("OT_DISTRIKT")

                Distrikt = sDistriktname


                Dim dRow As DataRow
                For Each dRow In tblDistrictsResult.Rows
                    Dim sID As String
                    sID = dRow("KUNNR_DI").ToString
                    sID = sID.TrimStart("0"c)
                    sID = Right(sID, sID.Length - 1)
                    sID = sID.TrimStart("0"c)
                    dRow("KUNNR_DI") = sID
                    tblDistrictsResult.AcceptChanges()
                Next

                WriteLogEntry(True, "Händlerdistrikt eingelesen", tblRightsResult)
            Catch ex As Exception
                Select Case ex.Message.Replace("Execution failed", "").Trim()
                    Case Else
                        m_intStatus = -9999
                        m_strMessage = ex.Message
                End Select
                If m_intIDSAP > -1 Then
                    m_objLogApp.WriteEndDataAccessSAP(m_intIDSAP, False, m_strMessage)
                End If
                WriteLogEntry(False, "Fehler beim Einlesen des Händlerdistriktes", tblRightsResult)
            Finally
                m_blnGestartet = False
            End Try
        End If
    End Sub

    Public Overrides Sub Show()

    End Sub

    Public Overloads Sub Change(ByVal strAppID As String, ByVal strSessionID As String, ByVal page As Web.UI.Page)

        m_intIDSAP = -1

        Try
            m_intStatus = 0
            m_strMessage = ""
            sDistriktID = Right("000000" & (sDistriktID), 6)
            sDistriktID = Right("0000000000" & ("6" & sDistriktID), 10)


            'Dim myProxy As DynSapProxyObj = DynSapProxy.getProxy("Z_V_Haendlerzuordnung_Change", m_objApp, m_objUser, page)

            'myProxy.setImportParameter("I_DISTRIKT", sDistriktID)
            'myProxy.setImportParameter("I_HAENDLER", Haendler)
            'myProxy.setImportParameter("I_BUKRS", "1510")

            'myProxy.callBapi()

            S.AP.InitExecute("Z_V_Haendlerzuordnung_Change", "I_DISTRIKT,I_HAENDLER,I_BUKRS", sDistriktID, Haendler, "1510")

        Catch ex As Exception
            Select Case ex.Message.Replace("Execution failed", "").Trim()
                Case "SAME_DISTRIKT"
                    m_strMessage = "Distrikt bereits eingetragen."
                Case "DISTRIKT_NOT_FOUND"
                    m_strMessage = "Distrikt nicht vorhanden."
                Case "HAENDLER_NOT_FOUND"
                    m_strMessage = "Händler nicht vorhanden."
                Case Else
                    m_intStatus = -9999
                    m_strMessage = ex.Message
            End Select
        End Try
    End Sub

    Public Overrides Sub Change()

   
    End Sub

#End Region
End Class

' ************************************************
' $History: FFD_Bank_Distrikt.vb $
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 26.06.09   Time: 10:33
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2918
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 20.05.09   Time: 14:30
' Updated in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 2  *****************
' User: Rudolpho     Date: 30.04.09   Time: 14:43
' Updated in $/CKAG/Applications/appffd/Lib
' ITA: 2837
' 
' *****************  Version 1  *****************
' User: Fassbenders  Date: 7.04.08    Time: 13:23
' Created in $/CKAG/Applications/appffd/Lib
' 
' *****************  Version 10  *****************
' User: Rudolpho     Date: 28.02.08   Time: 10:05
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA: 1737
' 
' *****************  Version 9  *****************
' User: Rudolpho     Date: 18.02.08   Time: 13:22
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Ita:1690
' 
' *****************  Version 8  *****************
' User: Rudolpho     Date: 7.11.07    Time: 14:24
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' ITA:1374
' 
' *****************  Version 7  *****************
' User: Uha          Date: 2.07.07    Time: 17:40
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Verbindung ASPX-Logging mit BAPI-Logging
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 19.06.07   Time: 14:27
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 19.06.07   Time: 10:53
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 4  *****************
' User: Rudolpho     Date: 13.06.07   Time: 17:03
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Portal - Startapplication 13.06.2005
' 
' *****************  Version 3  *****************
' User: Rudolpho     Date: 8.06.07    Time: 15:36
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Abgleich Beyond Compare
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 8.06.07    Time: 11:26
' Updated in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' 
' *****************  Version 1  *****************
' User: Uha          Date: 3.05.07    Time: 18:05
' Created in $/CKG/Applications/AppFFD/AppFFDWeb/Lib
' Änderungen aus StartApplication vom 02.05.2007 Mittags übernommen
' 
' ************************************************
