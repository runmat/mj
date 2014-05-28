
Public Class Einzahlungsbelege
    Private mE_SUBRC As Integer
    Private mE_MESSAGE As String

    Private mstrKostStelle As String
    Private mstrMenge As String
    Dim SAPExc As SAPExecutor.SAPExecutor

    Public Property E_SUBRC() As Integer
        Get
            Return mE_SUBRC
        End Get
        Set(ByVal Value As Integer)
            mE_SUBRC = Value
        End Set
    End Property

    Public Property E_MESSAGE() As String
        Get
            Return mE_MESSAGE
        End Get
        Set(ByVal Value As String)
            mE_MESSAGE = Value
        End Set
    End Property
    Public Property KostStelle() As String
        Get
            KostStelle = mstrKostStelle
        End Get
        Set(ByVal value As String)
            mstrKostStelle = value
        End Set
    End Property
    Public Property Menge() As String
        Get
            Menge = mstrMenge
        End Get
        Set(ByVal value As String)
            mstrMenge = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub ChangeERP()

        E_MESSAGE = ""
        E_SUBRC = 0

        Dim tblSAP As New DataTable()
        tblSAP.Columns.Add("MATNR", String.Empty.GetType)
        tblSAP.Columns.Add("MENGE", String.Empty.GetType)
        tblSAP.Columns.Add("VKP", String.Empty.GetType)

        SAPExc = New SAPExecutor.SAPExecutor(KBS_BASE.SAPConnectionString)

        Try

            Dim dt As DataTable = SAPExecutor.SAPExecutor.getSAPExecutorTable()

            dt.Rows.Add(New Object() {"I_KOSTL", False, Right("0000000000" & mstrKostStelle, 10), 10})
            dt.Rows.Add(New Object() {"I_MENGE", False, mstrMenge})

            SAPExc.ExecuteERP("Z_FIL_EFA_PO_EINZAHLUNGSBELEGE", dt)

            If (SAPExc.ErrorOccured) Then
                E_SUBRC = SAPExc.E_SUBRC
                E_MESSAGE = SAPExc.E_MESSAGE
            End If

        Catch ex As Exception
            E_MESSAGE = ex.Message
        Finally

        End Try

    End Sub
End Class
