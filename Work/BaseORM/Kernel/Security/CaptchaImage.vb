Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text

Public Class CaptchaImage
#Region "Members"

    Private _text As String
    Private _width As Integer
    Private _height As Integer
    Private _familyName As String
    Private _image As Bitmap
    Private _random As New Random()

#End Region

#Region "Properties"

    Public ReadOnly Property Text() As String
        Get
            Return _text
        End Get
    End Property

    Public ReadOnly Property Image() As Bitmap
        Get
            Return _image
        End Get
    End Property
    Public ReadOnly Property Width() As Integer
        Get
            Return _width
        End Get
    End Property
    Public ReadOnly Property Height() As Integer
        Get
            Return _height
        End Get
    End Property

#End Region


    Public Sub New(ByVal s As String, ByVal width As Integer, ByVal height As Integer)
        _text = s
        SetDimensions(width, height)
        GenerateImage()

    End Sub
    Public Sub New(ByVal s As String, ByVal width As Integer, ByVal height As Integer, ByVal familyName As String)
        Me.New(s, width, height)
        SetFamilyName(familyName)
        GenerateImage()
    End Sub

    Public Sub Dispose()
        GC.SuppressFinalize(Me)
        Me.Dispose(True)
    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            ' Dispose of the bitmap.
            Me._image.Dispose()
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Try
            Dispose(False)
        Finally
            MyBase.Finalize()
        End Try
    End Sub


    Private Sub SetDimensions(ByVal width As Integer, ByVal height As Integer)
        If width <= 0 Then
            Throw New ArgumentOutOfRangeException("width", width, "Argument out of range, must be greater than zero.")
        End If
        If height <= 0 Then
            Throw New ArgumentOutOfRangeException("height", height, "Argument out of range, must be greater than zero.")
        End If
        Me._width = width
        Me._height = height
    End Sub
    Private Sub SetFamilyName(ByVal familyName As String)
        Try
            Dim font As New Font(Me._familyName, 12.0F)
            Me._familyName = familyName
            font.Dispose()
        Catch ex As Exception
            Me._familyName = System.Drawing.FontFamily.GenericSerif.Name
        End Try
    End Sub
    Private Sub GenerateImage()
        ' Create a new 32-bit bitmap image.
        Dim bitmap As New Bitmap(Me._width, Me._height, PixelFormat.Format32bppArgb)

        ' Create a graphics object for drawing.
        Dim g As Graphics = Graphics.FromImage(bitmap)
        g.SmoothingMode = SmoothingMode.AntiAlias
        Dim rect As New Rectangle(0, 0, Me._width, Me._height)

        ' Fill in the background.
        Dim hatchBrush As New HatchBrush(HatchStyle.SmallConfetti, Color.LightGray, Color.White)
        g.FillRectangle(hatchBrush, rect)

        ' Set up the text font.
        Dim size As SizeF
        Dim fontSize As Single = rect.Height + 1
        Dim font As Font
        ' Adjust the font size until the text fits within the image.
        Do
            fontSize -= 1
            font = New Font(Me._familyName, fontSize, FontStyle.Bold)
            size = g.MeasureString(Me._text, font)
        Loop While size.Width > rect.Width

        ' Set up the text format.
        Dim format As New StringFormat()
        format.Alignment = StringAlignment.Center
        format.LineAlignment = StringAlignment.Center

        ' Create a path using the text and warp it randomly.
        Dim path As New GraphicsPath()
        path.AddString(Me._text, font.FontFamily, CInt(font.Style), font.Size, rect, format)
        Dim v As Single = 4.0F
        Dim points As PointF() = {New PointF(Me._random.[Next](rect.Width) / v, Me._random.[Next](rect.Height) / v), New PointF(rect.Width - Me._random.[Next](rect.Width) / v, Me._random.[Next](rect.Height) / v), New PointF(Me._random.[Next](rect.Width) / v, rect.Height - Me._random.[Next](rect.Height) / v), New PointF(rect.Width - Me._random.[Next](rect.Width) / v, rect.Height - Me._random.[Next](rect.Height) / v)}
        Dim matrix As New Matrix()
        matrix.Translate(0.0F, 0.0F)
        path.Warp(points, rect, matrix, WarpMode.Perspective, 0.0F)

        ' Draw the text.
        hatchBrush = New HatchBrush(HatchStyle.LargeConfetti, Color.LightGray, Color.DarkGray)
        g.FillPath(hatchBrush, path)

        ' Add some random noise.
        Dim m As Integer = Math.Max(rect.Width, rect.Height)
        For i As Integer = 0 To CInt(Math.Truncate(rect.Width * rect.Height / 30.0F)) - 1
            Dim x As Integer = Me._random.[Next](rect.Width)
            Dim y As Integer = Me._random.[Next](rect.Height)
            Dim w As Integer = Me._random.[Next](m / 50)
            Dim h As Integer = Me._random.[Next](m / 50)
            g.FillEllipse(hatchBrush, x, y, w, h)
        Next

        ' Clean up.
        font.Dispose()
        hatchBrush.Dispose()
        g.Dispose()

        ' Set the image.
        Me._image = bitmap
    End Sub

End Class
