'Alex Wheelock
'RCET 3371
'Fall 2024
'Smart Home Controller Final Program
'https://github.com/AlexWheelock/SmartHomeController.git

Option Explicit On
Option Strict On

Public Class SmartHomeControllerForm

    Dim settingsList As New List(Of String)

    'Used to only try and receive data when the port has been opened properly
    Function ReadyToReceiveData(update As Integer) As Boolean
        Static currentState As Boolean

        If update <> -1 Then
            If update = 0 Then
                currentState = False
            ElseIf update = 1 Then
                currentState = True
            End If
        End If

        Return currentState
    End Function

    Function ReadSettings() As Boolean
        Dim temp() As String
        Dim temp1() As String
        Dim currentLine As String
        Dim connected As Boolean = True

        Try
            FileOpen(1, "..\..\HVAC Settings.txt", OpenMode.Input)
            Do Until EOF(1)
                currentLine = LineInput(1)
                temp = Split(currentLine, ",")
                temp1 = Split(temp(1), " ")
                settingsList.Add(temp1(0))
            Loop
            FileClose()
            UpdateMaxTemp(CInt(settingsList.Item(0)))
            UpdateMinTemp(CInt(settingsList.Item(1)))
            SerialPort.PortName = settingsList.Item(2)
            SerialPort.Open()
            ReadyToReceiveData(1)
        Catch ex As Exception
            UpdateMaxTemp(90)
            UpdateMinTemp(50)
            ReadyToReceiveData(0)
            connected = False
        End Try

        Return connected
    End Function

    Sub Quit()
        FileOpen(1, "..\..\HVAC Settings.txt", OpenMode.Output)
        WriteLine(1, $"max_temp,{MaxTempTextBox.Text} ")
        WriteLine(1, $"min_temp,{MinTempTextBox.Text} ")
        WriteLine(1, $"port_name,{SerialPort.PortName} ")
        FileClose(1)
        Me.Close()
    End Sub

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

    Sub ReadDigitalInputs()
        Dim data(0) As Byte
        data(0) = &B110000

        SerialPort.Write(data, 0, 1)

    End Sub

    Function GetBitStatus(data As Byte, index As Integer) As Boolean
        Dim bits As New BitArray({data})
        If bits(index) Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub ReceiveData()
        Dim data(SerialPort.BytesToRead) As Byte
        Dim writeData(1) As Byte
        writeData(0) = &H20


        SerialPort.Read(data, 0, SerialPort.BytesToRead)
        Console.WriteLine($"Bytes Receieved: {SerialPort.BytesToRead}")
        Console.WriteLine($"Digital Inputs: {Hex(data(0))}")

        'Dim bitData As New BitArray(CByte(data(0)))
        If GetBitStatus(data(0), 0) Then
            writeData(1) = &H1
            SerialPort.Write(writeData, 0, 2)
            Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
        Else
            writeData(1) = &H0
            SerialPort.Write(writeData, 0, 2)
            Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
        End If

    End Sub

    Sub InhibitFunctions()
        Static enabled As Boolean
        Dim writeToDigitalOutputs() As Byte

        If enabled Then
            enabled = False
            writeToDigitalOutputs(0) = &H20
            writeToDigitalOutputs(1) = &H0
            SerialPort.Write(writeToDigitalOutputs, 0, 2)
        Else
            enabled = True
            writeToDigitalOutputs(0) = &H20
            writeToDigitalOutputs(1) = &H1
            SerialPort.Write(writeToDigitalOutputs, 0, 2)
        End If

    End Sub

    '================================================================================
    'Event Handlers Below Here
    '================================================================================

    Private Sub SmartHomeControllerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ReadSettings() = False Then
            SerialPortSelectForm.Show()
        End If
        'ReadyToReceiveData(0)
        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")
    End Sub

    Private Sub SerialPort_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort.DataReceived
        If ReadyToReceiveData(-1) Then
            ReceiveData()
        End If
    End Sub

    Private Sub ConnectToQyBoardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToQyBoardToolStripMenuItem.Click
        SerialPortSelectForm.Show()
    End Sub

    Private Sub UpdateCurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles UpdateCurrentTimeTimer.Tick
        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")
        ReadDigitalInputs()
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

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Quit()
    End Sub

End Class
