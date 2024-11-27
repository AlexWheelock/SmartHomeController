'Alex Wheelock
'RCET 3371
'Fall 2024
'Smart Home Controller Final Program
'https://github.com/AlexWheelock/SmartHomeController.git

Option Explicit On
Option Strict On

Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Thread
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
        Static input1Active As Boolean
        Static input2Active As Boolean
        Static input3Active As Boolean

        SerialPort.Read(data, 0, SerialPort.BytesToRead)
        Console.WriteLine($"Bytes Receieved: {SerialPort.BytesToRead}")
        Console.WriteLine($"Digital Inputs: {Hex(data(0))}")

        'Dim bitData As New BitArray(CByte(data(0)))
        If GetBitStatus(data(0), 0) = False Then
            If input1Active = False Then
                writeData(1) = &H1
                SerialPort.Write(writeData, 0, 2)
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
                FileOpen(1, "..\..\HVAC system log.txt", OpenMode.Append)
                WriteLine(1, $"{DateTime.Now.ToString("G")}; Alert: Safety Interlock Activated")
                FileClose(1)
                If Alert1Label.Visible Then
                    HandleErrors(3)
                Else
                    HandleErrors(1)
                End If
                input1Active = True
                input2Active = False
                input3Active = False
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            Else
                If Alert2Label.Visible Then
                    HandleErrors(2)
                Else
                    HandleErrors(0)
                End If
                input1Active = False
                input2Active = False
                input3Active = False
                writeData(1) = &H0
                SerialPort.Write(writeData, 0, 2)
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            End If

        ElseIf GetBitStatus(data(0), 1) = False Then
            If input2Active = False Then
                writeData(1) = &H8
                SerialPort.Write(writeData, 0, 2)
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
                Sleep(5000)
                writeData(1) = &H2
                SerialPort.Write(writeData, 0, 2)
                input1Active = False
                input2Active = True
                input3Active = False
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            Else
                input1Active = False
                input2Active = False
                input3Active = False
                writeData(1) = &H0
                SerialPort.Write(writeData, 0, 2)
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            End If

        ElseIf GetBitStatus(data(0), 2) = False Then
            If input3Active = False Then
                writeData(1) = &H4
                SerialPort.Write(writeData, 0, 2)
                input1Active = False
                input2Active = False
                input3Active = True
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            Else
                input1Active = False
                input2Active = False
                input3Active = False
                writeData(1) = &H0
                SerialPort.Write(writeData, 0, 2)
                Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
            End If

        ElseIf GetBitStatus(data(0), 3) = False Then
            CheckSensor()

        End If

    End Sub

    Sub CheckSensor(Optional read As Boolean = False)
        Static sensorStatus As Boolean

        If read = False Then
            sensorStatus = True
            If Alert1Label.Text = "System Error: Requires System Maintenance" Then
                HandleErrors(0)
            Else
                If Alert2Label.Visible Then
                    HandleErrors(1)
                Else
                    HandleErrors(0)
                End If
            End If

            If CheckSensorTimer.Enabled = False Then
                CheckSensorTimer.Enabled = True
            End If
        Else
            If sensorStatus Then
                sensorStatus = False
                CheckSensorTimer.Enabled = True
            Else
                If Alert1PictureBox.Visible Then
                    If Alert1Label.Text = "Alert: Safety Interlock Activated" Then
                        HandleErrors(3)
                    End If
                Else
                    HandleErrors(2)
                End If
                FileOpen(1, "..\..\HVAC system log.txt", OpenMode.Append)
                WriteLine(1, $"{DateTime.Now.ToString("G")}; System Error: Requires System Maintenance")
                FileClose(1)
            End If
        End If

    End Sub


    Sub HandleErrors(mode As Integer)
        If mode = 0 Then

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Visible = False))
            Else
                Alert1Label.Visible = False
            End If

            If Me.Alert2Label.InvokeRequired Then
                Me.Alert2Label.Invoke(New MethodInvoker(Sub() Alert2Label.Visible = False))
            Else
                Alert2Label.Visible = False
            End If

            If Me.Alert1PictureBox.InvokeRequired Then
                Me.Alert1PictureBox.Invoke(New MethodInvoker(Sub() Alert1PictureBox.Visible = False))
            Else
                Alert1PictureBox.Visible = False
            End If

            If Me.Alert2PictureBox.InvokeRequired Then
                Me.Alert2PictureBox.Invoke(New MethodInvoker(Sub() Alert2PictureBox.Visible = False))
            Else
                Alert2PictureBox.Visible = False
            End If

        ElseIf mode = 1 Then

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Text = "Alert: Safety Interlock Activated"))
            Else
                Alert1Label.Text = "Alert: Safety Interlock Activated"
            End If

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Visible = True))
            Else
                Alert1Label.Visible = True
            End If

            If Me.Alert2Label.InvokeRequired Then
                Me.Alert2Label.Invoke(New MethodInvoker(Sub() Alert2Label.Visible = False))
            Else
                Alert2Label.Visible = False
            End If

            If Me.Alert1PictureBox.InvokeRequired Then
                Me.Alert1PictureBox.Invoke(New MethodInvoker(Sub() Alert1PictureBox.Visible = True))
            Else
                Alert1PictureBox.Visible = True
            End If

            If Me.Alert2PictureBox.InvokeRequired Then
                Me.Alert2PictureBox.Invoke(New MethodInvoker(Sub() Alert2PictureBox.Visible = False))
            Else
                Alert2PictureBox.Visible = False
            End If

        ElseIf mode = 2 Then

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Text = "System Error: Requires System Maintenance"))
            Else
                Alert1Label.Text = "System Error: Requires System Maintenance"
            End If

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Visible = True))
            Else
                Alert1Label.Visible = True
            End If

            If Me.Alert2Label.InvokeRequired Then
                Me.Alert2Label.Invoke(New MethodInvoker(Sub() Alert2Label.Visible = False))
            Else
                Alert2Label.Visible = False
            End If

            If Me.Alert1PictureBox.InvokeRequired Then
                Me.Alert1PictureBox.Invoke(New MethodInvoker(Sub() Alert1PictureBox.Visible = True))
            Else
                Alert1PictureBox.Visible = True
            End If

            If Me.Alert2PictureBox.InvokeRequired Then
                Me.Alert2PictureBox.Invoke(New MethodInvoker(Sub() Alert2PictureBox.Visible = False))
            Else
                Alert2PictureBox.Visible = False
            End If

        ElseIf mode = 3 Then

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Text = "Alert: Safety Interlock Activated"))
            Else
                Alert1Label.Text = "Alert: Safety Interlock Activated"
            End If

            If Me.Alert2Label.InvokeRequired Then
                Me.Alert2Label.Invoke(New MethodInvoker(Sub() Alert2Label.Text = "System Error: Requires System Maintenance"))
            Else
                Alert2Label.Text = "System Error: Requires System Maintenance"
            End If

            If Me.Alert1Label.InvokeRequired Then
                Me.Alert1Label.Invoke(New MethodInvoker(Sub() Alert1Label.Visible = True))
            Else
                Alert1Label.Visible = True
            End If

            If Me.Alert2Label.InvokeRequired Then
                Me.Alert2Label.Invoke(New MethodInvoker(Sub() Alert2Label.Visible = True))
            Else
                Alert2Label.Visible = True
            End If

            If Me.Alert1PictureBox.InvokeRequired Then
                Me.Alert1PictureBox.Invoke(New MethodInvoker(Sub() Alert1PictureBox.Visible = True))
            Else
                Alert1PictureBox.Visible = True
            End If

            If Me.Alert2PictureBox.InvokeRequired Then
                Me.Alert2PictureBox.Invoke(New MethodInvoker(Sub() Alert2PictureBox.Visible = True))
            Else
                Alert2PictureBox.Visible = True
            End If

        End If

    End Sub


    '================================================================================
    'Event Handlers Below Here
    '================================================================================

    Private Sub SmartHomeControllerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If ReadSettings() = False Then
            Me.WindowState = FormWindowState.Minimized
            SerialPortSelectForm.Show()
        End If
        'ReadyToReceiveData(0)
        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")
        HandleErrors(0)
        CheckSensorTimer.Enabled = True
    End Sub

    Private Sub SerialPort_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort.DataReceived
        If ReadyToReceiveData(-1) Then
            ReceiveData()
        End If
    End Sub

    Private Sub ConnectToQyBoardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToQyBoardToolStripMenuItem.Click
        ReadyToReceiveData(0)
        Me.WindowState = FormWindowState.Minimized
        SerialPortSelectForm.Show()
        SerialPortSelectForm.BringToFront()
    End Sub

    Private Sub UpdateCurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles UpdateCurrentTimeTimer.Tick
        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")
        If ReadyToReceiveData(-1) Then
            ReadDigitalInputs()
        End If

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

    Private Sub CheckSensorTimer_Tick(sender As Object, e As EventArgs) Handles CheckSensorTimer.Tick
        CheckSensor(True)
    End Sub
End Class
