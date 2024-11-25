'Alex Wheelock
'RCET 3371
'Fall 2024
'Smart Home Controller Final Program
'https://github.com/AlexWheelock/SmartHomeController.git

Option Explicit On
Option Strict On

Public Class SmartHomeControllerForm

    Function UpdateMaxTemp(update As Integer, Optional read As Boolean = False) As Double
        Static currentMaxTemp As Double

        If read = False Then
            If update = 1 Then
                'increment 0.5 degrees when "+" button is pressed
                If currentMaxTemp <> 90 Then
                    currentMaxTemp += 0.5
                End If
            ElseIf update = 0 Then
                'decrement 0.5 degrees when "-" button is pressed
                If currentMaxTemp <> CDbl(MinTempTextBox.Text) Then
                    currentMaxTemp -= 0.5
                End If
            Else
                'used to initalize the max temp to the previously saved max on startup
                currentMaxTemp = update
            End If
        End If

        MaxTempTextBox.Text = $"{currentMaxTemp}"
        Return currentMaxTemp
    End Function

    Function UpdateMinTemp(update As Integer, Optional read As Boolean = False) As Double
        Static currentMinTemp As Double

        If read = False Then
            If update = 1 Then
                'increment 0.5 degrees when "+" button is pressed
                If currentMinTemp <> CDbl(MaxTempTextBox.Text) Then
                    currentMinTemp += 0.5
                End If
            ElseIf update = 0 Then
                'decrement 0.5 degrees when "-" button is pressed
                If currentMinTemp <> 50 Then
                    currentMinTemp -= 0.5
                End If
            Else
                'used to initalize the max temp to the previously saved max on startup
                currentMinTemp = update
            End If
        End If

        MinTempTextBox.Text = $"{currentMinTemp}"
        Return currentMinTemp
    End Function

    '================================================================================
    'Event Handlers Below Here
    '================================================================================

    Private Sub SmartHomeControllerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateMaxTemp(90)
        UpdateMinTemp(50)
    End Sub

    Private Sub ConnectToQyBoardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToQyBoardToolStripMenuItem.Click
        SerialPortSelectForm.Show()
    End Sub

    Private Sub UpdateCurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles UpdateCurrentTimeTimer.Tick

        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")

    End Sub

    Private Sub IncreaseMaxTempButton_Click(sender As Object, e As EventArgs) Handles IncreaseMaxTempButton.Click
        UpdateMaxTemp(1)
    End Sub

    Private Sub DecreaseMaxTempButton_Click(sender As Object, e As EventArgs) Handles DecreaseMaxTempButton.Click
        UpdateMaxTemp(0)
    End Sub

    Private Sub IncreaseMinTempButton_Click(sender As Object, e As EventArgs) Handles IncreaseMinTempButton.Click
        UpdateMinTemp(1)
    End Sub

    Private Sub DecreaseMinTempButton_Click(sender As Object, e As EventArgs) Handles DecreaseMinTempButton.Click
        UpdateMinTemp(0)
    End Sub
End Class
