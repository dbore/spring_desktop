'Copyright (C) 2014 Damian Borecki
Public Class Form1

    Dim mousex As Integer
    Dim mousey As Integer

    'graphic
    Dim z As New System.Drawing.Drawing2D.GraphicsPath
    Dim g As Graphics

    Private Declare Function GetDC Lib "user32.dll" _
(ByVal hwnd As IntPtr) As IntPtr
    Private Declare Function ReleaseDC Lib "user32.dll" _
    (ByVal hwnd As IntPtr, ByVal hdc As IntPtr) As IntPtr

    Dim hdc As IntPtr

    'move spring
    Dim x, y As Integer

    'direction
    Dim timetochange As Integer = 0
    Dim direction As Integer = 0

    'color
    Dim alpha = 0, red = 0, blue = 0, green = 0 'color1
    Dim alpha1 = 0, red1 = 0, blue1 = 0, green1 = 0 'color2

    Dim clear As Integer = 0 ' clear screen
    Dim check = 0

    Private Declare Function InvalidateRect Lib "user32" (ByVal hwnd As Long, ByVal lpRect As Long, ByVal bErase As Long) As Long ' call to refresh desktop

    Dim tempdir As Integer = -1

    Dim draw As Boolean = True


    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        clearscreen() 'clear hdc
    End Sub




    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Timer1.Enabled = False
        clearscreen()
        Me.Close()

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load


        'full screen form 
        Me.Location = New Point(0, 0)
        Me.Width = My.Computer.Screen.WorkingArea.Width + 50
        Me.Height = My.Computer.Screen.WorkingArea.Height + 50

        'hide cursor + get cursor position
        Windows.Forms.Cursor.Hide()
        mousex = Windows.Forms.Cursor.Position.X
        mousey = Windows.Forms.Cursor.Position.Y

        'starting pos for spring
        x = 0
        y = Me.Height - 120

        'draw spring
        color_change()
        draw_spring()


    End Sub

    Sub checkmouse_pos()
        'much better
        If mousex > Windows.Forms.Cursor.Position.X Or mousex < Windows.Forms.Cursor.Position.X Then
            Timer1.Enabled = False
            clearscreen()
            Me.Close()
        End If

    End Sub

    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Timer1.Enabled = False
        clearscreen()
        Me.Close()
    End Sub

    Sub draw_spring()

        'brush for spring
        If check = 0 Then
            color_change()
            check = 1
        End If

        Dim linGrBrush As New Drawing2D.LinearGradientBrush( _
           New Point(0, 10), _
           New Point(200, 10), _
           Color.FromArgb(255, red, green, blue), _
           Color.FromArgb(255, red1, green1, blue1))


        Dim p As New Pen(Color.Blue, 3)
        p.Width = 3
        p.Brush = linGrBrush


        hdc = GetDC(IntPtr.Zero)

        Try

            g = Graphics.FromHdc(hdc)

            Try
                g.DrawEllipse(p, x, y, 100, 100)

            Finally
                g.Dispose()

            End Try
        Finally
            ReleaseDC(IntPtr.Zero, hdc)


        End Try


    End Sub
    Sub clearscreen()
        Timer1.Enabled = False
        InvalidateRect(0&, 0&, False)
        draw = True
        Timer1.Enabled = True


    End Sub
    Sub color_change()
        'change the color each time
        Dim colortochoose As New Random

        alpha = colortochoose.Next(0, 255)
        red = colortochoose.Next(0, 255)
        green = colortochoose.Next(0, 255)
        blue = colortochoose.Next(0, 255)

        alpha1 = colortochoose.Next(0, 255)
        red1 = colortochoose.Next(0, 255)
        green1 = colortochoose.Next(0, 255)
        blue1 = colortochoose.Next(0, 255)
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        checkmouse_pos() ' mouse pos

        timetochange = timetochange + 1 'for direction delay

        'move
        If direction = 0 Then
            x = x + 10
            y = y - 10
        ElseIf direction = 1 Then
            x = x + 10
            y = y + 10
        ElseIf direction = 2 Then
            y = y - 10
            x = x - 10
        ElseIf direction = 3 Then
            y = y + 10
            x = x - 10
        End If

        'change direction
        If timetochange >= 20 Then

            Dim r As New Random
            Dim r1 As Integer

            r1 = r.Next(0, 4)

            If r1 = 0 Then
                direction = 0
                tempdir = 0
            ElseIf r1 = 1 Then
                direction = 1
                tempdir = 1
            ElseIf r1 = 2 Then
                direction = 2
                tempdir = 2
            ElseIf r1 = 3 Then
                direction = 3
                tempdir = 3
            End If

            'reset counter
            timetochange = 0

            color_change()

        End If

        'bounce
        Dim dec As New Random
        Dim dec1 As Integer = dec.Next(0, 2)

        If x <= 0 And y >= (Me.Height) Then
            'choose  2
            direction = 0
            timetochange = 0

        ElseIf x <= 0 And y <= 0 Then
            'choose direction 1
            direction = 1
            timetochange = 0

        ElseIf x >= (Me.Width) And y <= 0 Then
            'choose direction 4
            direction = 3
            timetochange = 0

        ElseIf x >= (Me.Width) And y >= (Me.Height) Then
            'choose direction 3 
            direction = 2
            timetochange = 0

        ElseIf y > (Me.Height - 100) Then
            direction = 0
            timetochange = 0

        ElseIf x < 0 Then
            direction = 1
            timetochange = 0

        ElseIf x > (Me.Width - 100) Then

            direction = 2
            timetochange = 0

        ElseIf y < 0 Then
            direction = 3
            timetochange = 0


        End If



        'clear screen

        clear = clear + 1

        If clear > 2000 Then
            draw = False
            Timer1.Enabled = False
            clearscreen()
            clear = 0
        End If

        If draw = True Then
            draw_spring()
        End If







    End Sub
End Class

