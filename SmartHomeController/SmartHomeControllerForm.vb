'Alex Wheelock
'RCET 3371
'Fall 2024
'Smart Home Controller Final Program
'https://github.com/AlexWheelock/SmartHomeController.git

Option Explicit On
Option Strict On

Imports System.Runtime.InteropServices
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
        Dim data(1) As Byte

        data(0) = &H20
        data(1) = &H0

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
            SerialPort.Write(data, 0, 2)
            ReadyToReceiveData(1)
            CheckSensorTimer.Enabled = True
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

    Sub ReadQyInputs()
        Dim data(0) As Byte
        Try
            'read digital inputs
            data(0) = &B110000
            SerialPort.Write(data, 0, 1)

            'read analog input 1
            data(0) = &B1010001
            SerialPort.Write(data, 0, 1)

            'read analog input 2
            data(0) = &B1010010
            SerialPort.Write(data, 0, 1)
        Catch ex As Exception
            SerialPort.Close()
            ReadyToReceiveData(0)
            MsgBox("There was an error communicating to the Qy@ board." & vbCrLf _
                   & vbCrLf _
                   & "Please reconnect the device.")
            SerialPortSelectForm.Show()
        End Try


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
        Dim data(105) As Byte
        Dim writeData(1) As Byte
        writeData(0) = &H20
        Static input1Active As Boolean
        Static input2Active As Boolean
        Static input3Active As Boolean
        Dim analogInput1High As Double
        Dim analogInput1Low As Double
        Dim homeTemp As Double
        Dim analoginput2High As Double
        Dim analoginput2Low As Double
        Dim unitAirTemp As Double

        Sleep(5)
        Console.WriteLine($"Bytes Receieved: {SerialPort.BytesToRead}")
        SerialPort.Read(data, 0, SerialPort.BytesToRead)
        Console.WriteLine($"Digital Inputs: {Hex(data(0))}")


        'read analog input 1
        analogInput1High = data(1) * 4
        analogInput1Low = data(2) \ 64
        homeTemp = Math.Round((((analogInput1High + analogInput1Low) / 17.05) + 40), 1)
        UpdateTempLabel(homeTemp)

        'read analog input 2
        analoginput2High = data(3) * 4
        analoginput2Low = data(4) \ 64
        unitAirTemp = Math.Round((((analoginput2High + analoginput2Low) / 17.05) + 40), 1)

        If ControllerMode(True, 0) = 2 Then
            If unitAirTemp <= 70 Then
                CheckSensor(False, 0)
            Else
                CheckSensor(False, 1)
            End If
        ElseIf ControllerMode(True, 0) = 1 Then
            If unitAirTemp >= 70 Then
                CheckSensor(False, 0)
            Else
                CheckSensor(False, 1)
            End If
        Else
            CheckSensor(False, 1)
        End If

        'determine if the heat or a/c needs to be turned on
        If ControllerMode(True, 0) = 1 Then 'system in cooling mode
            If CoolingIndicatorPictureBox.Visible Then
                If homeTemp <= CDbl(MinTempTextBox.Text) - 2 Then
                    UpdateTempIndicators(0)
                    writeData(1) = &H0
                    SerialPort.Write(writeData, 0, 2)
                End If
            End If
            If homeTemp >= CDbl(MinTempTextBox.Text) + 2 Then
                UpdateTempIndicators(2)
                writeData(1) = &H4
                SerialPort.Write(writeData, 0, 2)
            End If
        ElseIf ControllerMode(True, 0) = 2 Then 'system in heating mode
            If HeatIndicatorPictureBox.Visible Then
                If homeTemp >= CDbl(MaxTempTextBox.Text) + 2 Then
                    UpdateTempIndicators(0)
                    writeData(1) = &H2
                    SerialPort.Write(writeData, 0, 2)
                End If
            End If
            If homeTemp <= CDbl(MaxTempTextBox.Text) - 2 Then
                UpdateTempIndicators(1)
                writeData(1) = &H6
                SerialPort.Write(writeData, 0, 2)
            End If
        End If

        Console.WriteLine($"Air Temp: {homeTemp} | Unit Temp: {unitAirTemp}")

        'read digital inputs
        If input1Active Then
            If GetBitStatus(data(0), 0) = False Then
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
        Else
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
                    UpdateTempIndicators(0)
                    ControllerMode(False, 0)
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
                    ControllerMode(False, 2)
                    input1Active = False
                    input2Active = True
                    input3Active = False
                    Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
                    UpdateTempIndicators(1)
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
                    UpdateTempIndicators(0)
                    ControllerMode(False, 0)
                Else
                    input1Active = False
                    input2Active = False
                    input3Active = False
                    writeData(1) = &H0
                    SerialPort.Write(writeData, 0, 2)
                    Console.WriteLine($"Command: {Hex(writeData(0))}  | Data Out: {Hex(writeData(1))}")
                End If

            ElseIf GetBitStatus(data(0), 3) = False Then
                CheckSensor(False)
            End If
        End If

    End Sub

    Sub UpdateTempLabel(homeTemp As Double)
        If Me.CurrentTempLabel.InvokeRequired Then
            Me.CurrentTempLabel.Invoke(New MethodInvoker(Sub() CurrentTempLabel.Text = CStr(homeTemp) & "°F"))
        Else
            CurrentTempLabel.Text = CStr(homeTemp) & "°F"
        End If
    End Sub

    Sub CheckSensor(read As Boolean, Optional unitTempOK As Integer = -1)
        Static sensorStatus As Boolean
        Static unitTempStatus As Boolean

        If ControllerMode(True, 0) = 0 Then
            unitTempStatus = True
        End If

        'update the sensor status
        If Not read And unitTempOK = -1 Then
            sensorStatus = True
        End If

        'update the unit temp status
        If unitTempOK <> -1 Then
            If unitTempOK = 1 Then
                unitTempStatus = True
            Else
                unitTempStatus = False
            End If
        End If

        'determine if the sensors are working and in the proper temperature range
        If read Then
            If Not sensorStatus Or Not unitTempStatus Then
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
            Else
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
            End If
            'clear status on a read, because they need to be updated in the next 2 min in order to be functioning properly
            unitTempStatus = False
            sensorStatus = False
        End If

        If unitTempStatus And sensorStatus Then
            If Alert1Label.Visible Then
                If Alert1Label.Text = "System Error: Requires System Maintenance" Then
                    HandleErrors(0)
                Else
                    HandleErrors(1)
                End If
            End If

        End If
    End Sub

    Sub UpdateTempIndicators(mode As Integer)

        Select Case mode
            Case 0
                If Me.HeatIndicatorPictureBox.InvokeRequired Then
                    Me.HeatIndicatorPictureBox.Invoke(New MethodInvoker(Sub() HeatIndicatorPictureBox.Visible = False))
                Else
                    HeatIndicatorPictureBox.Visible = False
                End If

                If Me.CoolingIndicatorPictureBox.InvokeRequired Then
                    Me.CoolingIndicatorPictureBox.Invoke(New MethodInvoker(Sub() CoolingIndicatorPictureBox.Visible = False))
                Else
                    CoolingIndicatorPictureBox.Visible = False
                End If
            Case 1
                If Me.HeatIndicatorPictureBox.InvokeRequired Then
                    Me.HeatIndicatorPictureBox.Invoke(New MethodInvoker(Sub() HeatIndicatorPictureBox.Visible = True))
                Else
                    HeatIndicatorPictureBox.Visible = True
                End If

                If Me.CoolingIndicatorPictureBox.InvokeRequired Then
                    Me.CoolingIndicatorPictureBox.Invoke(New MethodInvoker(Sub() CoolingIndicatorPictureBox.Visible = False))
                Else
                    CoolingIndicatorPictureBox.Visible = False
                End If
            Case 2
                If Me.HeatIndicatorPictureBox.InvokeRequired Then
                    Me.HeatIndicatorPictureBox.Invoke(New MethodInvoker(Sub() HeatIndicatorPictureBox.Visible = False))
                Else
                    HeatIndicatorPictureBox.Visible = False
                End If

                If Me.CoolingIndicatorPictureBox.InvokeRequired Then
                    Me.CoolingIndicatorPictureBox.Invoke(New MethodInvoker(Sub() CoolingIndicatorPictureBox.Visible = True))
                Else
                    CoolingIndicatorPictureBox.Visible = True
                End If
        End Select

    End Sub


    Sub HandleErrors(mode As Integer)
        'Neither error is active
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

            'Only the Safety Interlock warning active
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

            'Only the system maintenance is active
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

            'both warnings active
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

    Function ControllerMode(read As Boolean, heatEnabled As Integer) As Integer
        Static controllerStatus As Integer
        'heat = 2
        'a/c = 1
        'neither = 0

        If Not read Then
            controllerStatus = heatEnabled
        End If

        Return controllerStatus
    End Function

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
        UpdateTempIndicators(0)
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

    Private Sub HeatButton_Click(sender As Object, e As EventArgs) Handles HeatButton.Click
        'Dim writedata(1) As Byte
        'writedata(1) = &H20
        ControllerMode(False, 2)
        UpdateTempIndicators(1)
        'writedata(1) = &H2
        'SerialPort.Write(writeData, 0, 2)
    End Sub

    Private Sub CoolButton_Click(sender As Object, e As EventArgs) Handles CoolButton.Click
        ControllerMode(False, 1)
        UpdateTempIndicators(2)
    End Sub

    Private Sub RequestDataTimer_Tick(sender As Object, e As EventArgs) Handles RequestDataTimer.Tick
        If ReadyToReceiveData(-1) Then
            ReadQyInputs()
        End If
    End Sub
End Class
