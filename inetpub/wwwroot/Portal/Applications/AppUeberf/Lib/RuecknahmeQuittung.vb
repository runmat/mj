Option Explicit On
Option Strict On

Imports CKG.Base.Kernel

Public Class RuecknahmeQuittung
    Public VBELN As String
    Public ZZKENN As String
    Public ZZFAHRG As String
    Public ZZREFNR As String
    Public FAHRER As String

    Public ERSCHW_BED As String
    Public VERSCHMUTZT As String
    Public REGEN_NASS As String
    Public DUNKELHEIT As String
    Public PARKHAUS As String
    Public SCHNEE_EIS As String
    Public ZEITDRU_BEI_UEB As String
    Public SONSTIGES As String
    Public UNFALL As String
    Public TECHN_MAENGEL As String
    Public AUSTAUSCH_MOT As String
    Public AUSTAUSCH_GETR As String
    Public AUSTAUSCH_TACHO As String
    Public FOTO_BEI_UEB As String
    Public GENEHMIG_MAEIL As String

    Public EMAIL_ADR As String
    Public FEHLFAHRT As String
    Public WARTEZEIT As String

    Public OELSTAND As String
    Public MOTORLAUF As String
    Public GETRIEBE As String
    Public ANFAHRTEST As String
    Public KUPPLUNG As String
    Public PROBEFAHRT As String
    Public RADLAGER As String
    Public GEBLAESE As String
    Public KLIMAANLAGE As String
    Public RADIO_NAVI As String
    Public FENSTERHEBER As String
    Public VERDECK As String
    Public SCHLUESSEL As String

    Public BETANKUNG As String
    Public FAHRZEUGWAESCHE As String
    Public INNENREINIGUNG As String
    Public MAUT As String
    Public WAERS As String

    Public KENNZ_MONTAGE As String
    Public REIFENHANDLING As String
    Public DISPOKONT_NOTW As String
    Public VORHOL_NACHBR As String

    Public BEMERKUNG As String
    Public SCHAED_BEI_UEB As String
    Public IUN_TIM As String
    Public IUG_DAT As String
    Public IUG_TIM As String

    Public KM_BEI_UEBERG As String

    Public KM_BEI_UEBERNAHM As String
    Public OEL As String
    Public UEBERNACHTUNG As String
    Public SONST_AUSLAGEN As String

    Public ReturnTable As DataTable

    Public ErrMessage As String

    Public Function Fill(ByRef User As Base.Kernel.Security.User, ByRef App As CKG.Base.Kernel.Security.App, ByVal SessionID As String,
                         ByVal Fahrer As String, ByVal VKORG As String) As DataTable

        'Dim myProxy As Base.Common.DynSapProxyObj
        Dim TempTable As New DataTable

        Try
            'myProxy = Base.Common.DynSapProxy.getProxy("Z_DPM_GET_FAHRER_AUFTR_001", App, User, page)
            S.AP.InitExecute("Z_DPM_GET_FAHRER_AUFTR_001", "I_VKORG,I_FAHRER", VKORG, Fahrer.PadLeft(10, "0"c))

            'Importparameter
            'myProxy.setImportParameter("I_VKORG", VKORG)
            'myProxy.setImportParameter("I_FAHRER", Fahrer.PadLeft(10, "0"c))

            'myProxy.callBapi()

            TempTable = S.AP.GetExportTable("GT_ORDER")
            
        Catch ex As Exception

        End Try

        Return TempTable

    End Function

    Public Sub Save(ByRef User As Base.Kernel.Security.User, ByRef App As CKG.Base.Kernel.Security.App, ByVal SessionID As String)

        'Dim myProxy As Base.Common.DynSapProxyObj

        ErrMessage = ""

        Try

            'myProxy = Base.Common.DynSapProxy.getProxy("Z_DPM_SAVE_RUE_QUIT_001", App, User, Page)

            Dim TempTable As DataTable = S.AP.GetImportTableWithInit("Z_DPM_SAVE_RUE_QUIT_001.GT_IN")

            Dim dtRow As DataRow = TempTable.NewRow

            dtRow("VBELN") = VBELN
            dtRow("ZZKENN") = ZZKENN
            dtRow("ZZFAHRG") = ZZFAHRG
            dtRow("ZZREFNR") = ZZREFNR
            dtRow("FAHRER") = FAHRER.PadLeft(10, "0"c)
            dtRow("ERSCHW_BED") = ERSCHW_BED
            dtRow("VERSCHMUTZT") = VERSCHMUTZT
            dtRow("REGEN_NASS") = REGEN_NASS
            dtRow("DUNKELHEIT") = DUNKELHEIT
            dtRow("PARKHAUS") = PARKHAUS
            dtRow("SCHNEE_EIS") = SCHNEE_EIS
            dtRow("ZEITDRU_BEI_UEB") = ZEITDRU_BEI_UEB
            dtRow("SONSTIGES") = SONSTIGES
            dtRow("UNFALL") = UNFALL
            dtRow("TECHN_MAENGEL") = TECHN_MAENGEL
            dtRow("AUSTAUSCH_MOT") = AUSTAUSCH_MOT
            dtRow("AUSTAUSCH_GETR") = AUSTAUSCH_GETR
            dtRow("AUSTAUSCH_TACHO") = AUSTAUSCH_TACHO
            dtRow("FOTO_BEI_UEB") = FOTO_BEI_UEB
            dtRow("GENEHMIG_MAEIL") = GENEHMIG_MAEIL
            dtRow("EMAIL_ADR") = EMAIL_ADR
            dtRow("FEHLFAHRT") = FEHLFAHRT
            dtRow("WARTEZEIT") = WARTEZEIT
            dtRow("OELSTAND") = OELSTAND
            dtRow("MOTORLAUF") = MOTORLAUF
            dtRow("GETRIEBE") = GETRIEBE
            dtRow("ANFAHRTEST") = ANFAHRTEST
            dtRow("KUPPLUNG") = KUPPLUNG
            dtRow("PROBEFAHRT") = PROBEFAHRT
            dtRow("RADLAGER") = RADLAGER
            dtRow("GEBLAESE") = GEBLAESE
            dtRow("KLIMAANLAGE") = KLIMAANLAGE
            dtRow("RADIO_NAVI") = RADIO_NAVI
            dtRow("FENSTERHEBER") = FENSTERHEBER
            dtRow("VERDECK") = VERDECK
            dtRow("SCHLUESSEL") = SCHLUESSEL
            dtRow("BETANKUNG") = Replace(BETANKUNG, ",", ".") ' Currency
            dtRow("FAHRZEUGWAESCHE") = Replace(FAHRZEUGWAESCHE, ",", ".") ' Currency
            dtRow("INNENREINIGUNG") = Replace(INNENREINIGUNG, ",", ".") ' Currency
            dtRow("MAUT") = Replace(MAUT, ",", ".") ' Currency
            dtRow("WAERS") = WAERS
            dtRow("KENNZ_MONTAGE") = KENNZ_MONTAGE
            dtRow("REIFENHANDLING") = REIFENHANDLING
            dtRow("DISPOKONT_NOTW") = DISPOKONT_NOTW
            dtRow("VORHOL_NACHBR") = VORHOL_NACHBR
            dtRow("BEMERKUNG") = BEMERKUNG
            dtRow("SCHAED_BEI_UEB") = SCHAED_BEI_UEB
            dtRow("IUN_TIM") = IUN_TIM
            dtRow("IUG_DAT") = IUG_DAT
            dtRow("IUG_TIM") = IUG_TIM
            dtRow("KM_BEI_UEBERG") = KM_BEI_UEBERG
            dtRow("KM_BEI_UEBERNAHM") = KM_BEI_UEBERNAHM
            dtRow("OEL") = Replace(OEL, ",", ".") ' Currency
            dtRow("UEBERNACHTUNG") = Replace(UEBERNACHTUNG, ",", ".") ' Currency
            dtRow("SONST_AUSLAGEN") = Replace(SONST_AUSLAGEN, ",", ".") ' Currency


            TempTable.Rows.Add(dtRow)

            TempTable.AcceptChanges()

            'myProxy.callBapi()
            S.AP.Execute()

            ReturnTable = S.AP.GetExportTable("GT_OUT")

            If ReturnTable.Rows.Count > 0 Then
                ErrMessage = ReturnTable.Rows(0)("RET_BEM").ToString
            End If


        Catch ex As Exception
            ErrMessage = ex.Message
        End Try
        
    End Sub
    
End Class

' ************************************************
' $History: RuecknahmeQuittung.vb $
' 
' *****************  Version 6  *****************
' User: Rudolpho     Date: 10.11.10   Time: 11:52
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 5  *****************
' User: Rudolpho     Date: 2.11.10    Time: 10:13
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 4  *****************
' User: Fassbenders  Date: 29.10.10   Time: 13:26
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA: 4240
' 
' *****************  Version 3  *****************
' User: Fassbenders  Date: 5.05.10    Time: 10:59
' Updated in $/CKAG/Applications/AppUeberf/Lib
' 
' *****************  Version 2  *****************
' User: Fassbenders  Date: 23.04.10   Time: 14:35
' Updated in $/CKAG/Applications/AppUeberf/Lib
' ITA: 3669
' 
