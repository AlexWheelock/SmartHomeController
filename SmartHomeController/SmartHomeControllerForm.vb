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

    'Tries to open normally with previous settings
    'Return boolean indicating whether connection was made properly
    Function ReadSettings() As Boolean
        Dim temp() As String
        Dim temp1() As String
        Dim currentLine As String
        Dim connected As Boolean = True
        Dim data(1) As Byte

        data(0) = &H20
        data(1) = &H0

        Try 'try to open with previous settings/port
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
        Catch ex As Exception 'Error opening with previous settings, set to default
            UpdateMaxTemp(90)
            UpdateMinTemp(50)
            ReadyToReceiveData(0)
            connected = False
        End Try

        Return connected
    End Function

    'Save settings and port name into HVAC Settings.txt
    Sub Quit()
        FileOpen(1, "..\..\HVAC Settings.txt", OpenMode.Output)
        WriteLine(1, $"max_temp,{MaxTempTextBox.Text} ")
        WriteLine(1, $"min_temp,{MinTempTextBox.Text} ")
        WriteLine(1, $"port_name,{SerialPort.PortName} ")
        FileClose(1)
        Me.Close()
    End Sub

    'Used to handle changing the max temp with the increment or decrement buttons, as well as updating the displayed max
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

    'Used to handle changing the min temp with the increment or decrement buttons, as well as updating the displayed min
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

    'Read Qy@ board inputs every 250 ms
    'Read digital inputs, then analog input 1 then 2
    'If connection to the Qy@ board fails, it requires the user to re-connect the Qy@ board
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

    'Used to test individual bits read from the Qy@ board
    'data is the byte containing the bit you want to test
    'index is the bit (0-7) that you want to test
    Function GetBitStatus(data As Byte, index As Integer) As Boolean
        Dim bits As New BitArray({data})
        If bits(index) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Receive and handle data from the Qy@ board
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

        Sleep(5) 'Sleep to ensure all bytes are received
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

        'determine if the unit is functioning properly, throw errors if not when tested every two minutes
        'if heating, the unit should be above air temp
        'if cooling, the unit should be below air temp
        If ControllerMode(True, 0) = 2 Then 'heating
            If unitAirTemp <= 70 Then
                CheckSensor(False, 0) 'unit is not working correctly
            Else
                CheckSensor(False, 1) 'unit is working properly
            End If
        ElseIf ControllerMode(True, 0) = 1 Then 'cooling
            If unitAirTemp >= 70 Then
                CheckSensor(False, 0) 'unit is not working correctly
            Else
                CheckSensor(False, 1) 'unit is working correctly
            End If
        Else
            CheckSensor(False, 1) 'ignore if neither in heating nor cooling modes
        End If

        'determine if the heat or a/c needs to be turned on
        If ControllerMode(True, 0) = 1 Then 'system in cooling mode
            If CoolingIndicatorPictureBox.Visible Then
                If homeTemp <= CDbl(MinTempTextBox.Text) - 2 Then 'if actively cooling, and outside of the 2 degree hysteresis, stop cooling
                    UpdateTempIndicators(0)
                    writeData(1) = &H0
                    SerialPort.Write(writeData, 0, 2)
                End If
            End If
            If homeTemp >= CDbl(MinTempTextBox.Text) + 2 Then 'if not cooling and 2 degrees above min temp, start cooling
                UpdateTempIndicators(2)
                writeData(1) = &H4
                SerialPort.Write(writeData, 0, 2)
            End If
        ElseIf ControllerMode(True, 0) = 2 Then 'system in heating mode
            If HeatIndicatorPictureBox.Visible Then
                If homeTemp >= CDbl(MaxTempTextBox.Text) + 2 Then 'if actively heating, and outside the 2 degree hysteresis, stop heating
                    UpdateTempIndicators(0)
                    writeData(1) = &H2
                    SerialPort.Write(writeData, 0, 2)
                End If
            End If
            If homeTemp <= CDbl(MaxTempTextBox.Text) - 2 Then 'if not heating, and 2 degrees below max temp, start heating
                UpdateTempIndicators(1)
                writeData(1) = &H6
                SerialPort.Write(writeData, 0, 2)
            End If
        End If

        Console.WriteLine($"Air Temp: {homeTemp} | Unit Temp: {unitAirTemp}")

        'read digital inputs
        If input1Active Then    'if safety interlock engaged, ignore all other digital inputs until it is pressed again
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
            If GetBitStatus(data(0), 0) = False Then 'determine if the safety interlock was pressed
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

            ElseIf GetBitStatus(data(0), 1) = False Then 'determine if the heater button is pressed
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

            ElseIf GetBitStatus(data(0), 2) = False Then 'determine if the fan button was pressed
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

            ElseIf GetBitStatus(data(0), 3) = False Then 'if pressed, then indicate the differential sensor is working
                CheckSensor(False)
            End If
        End If

    End Sub

    Sub UpdateTempLabel(homeTemp As Double) 'Update the temperature and date every 1 second
        If Me.CurrentTempLabel.InvokeRequired Then
            Me.CurrentTempLabel.Invoke(New MethodInvoker(Sub() CurrentTempLabel.Text = CStr(homeTemp) & "°F"))
        Else
            CurrentTempLabel.Text = CStr(homeTemp) & "°F"
        End If
    End Sub

    'determine if the pressure sensor and unit temp are okay
    Sub CheckSensor(read As Boolean, Optional unitTempOK As Integer = -1)
        Static sensorStatus As Boolean
        Static unitTempStatus As Boolean

        'ignore the unit temp status if not in either heating or cooling
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
        'if not, throw an error and request system maintenance, and save it along with the time into the system log
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

        'allow instant fixing of errors if the problems go away
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

    'used to update the heating/cooling indicators and make them visible
    'invoke added due to cross-threading errors occurring during testing
    'mode = 0: neither indicator visible
    'mode = 1: heating indicator visible
    'mode = 2: cooling indicator visible
    Sub UpdateTempIndicators(mode As Integer)

        Select Case mode
            'neither indicator visible
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

            'heating indicator visible
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
            'cooling indicator visible
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

    'Set the error/alert messages 
    'invoke added due to cross-threading errors occurring during testing
    'mode = 0: Neither error is visible
    'mode = 1: Safety interlock is visible in alert1
    'mode = 2: System maintenance is visible in alert1
    'mode = 3: Safety interlock is visible in alert1, system maintenance is visible in alert2
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

    'Used to determine if the smart home controller is in heating, cooling, or neither modes
    'update if read = false
    'do not update if read = true
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

    'Try to connect to the Qy@ board, and clear indicators, default to neither heating or cooling
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

    'If ready to receive data, then receive the incoming data bytes
    Private Sub SerialPort_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort.DataReceived
        If ReadyToReceiveData(-1) Then
            ReceiveData()
        End If
    End Sub

    'Handles the Connect to Qy@ board button in the top menu strip
    Private Sub ConnectToQyBoardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectToQyBoardToolStripMenuItem.Click
        ReadyToReceiveData(0)
        Me.WindowState = FormWindowState.Minimized
        SerialPortSelectForm.Show()
        SerialPortSelectForm.BringToFront()
    End Sub

    'Update the current time and date every 1 second
    Private Sub UpdateCurrentTimeTimer_Tick(sender As Object, e As EventArgs) Handles UpdateCurrentTimeTimer.Tick
        CurrentTimeLabel.Text = DateTime.Now.ToString("t")
        CurrentDateLabel.Text = DateTime.Now.ToString("D")
    End Sub

    'Increase the max temp by 0.5 degrees
    Private Sub IncreaseMaxTempButton_Click(sender As Object, e As EventArgs) Handles IncreaseMaxTempButton.Click
        UpdateMaxTemp(1)
    End Sub

    'Decrease the max temp by 0.5 degrees
    Private Sub DecreaseMaxTempButton_Click(sender As Object, e As EventArgs) Handles DecreaseMaxTempButton.Click
        UpdateMaxTemp(0)
    End Sub

    'Increase the min temp by 0.5 degrees
    Private Sub IncreaseMinTempButton_Click(sender As Object, e As EventArgs) Handles IncreaseMinTempButton.Click
        UpdateMinTemp(1)
    End Sub

    'Decrease the min temp by 0.5 degrees
    Private Sub DecreaseMinTempButton_Click(sender As Object, e As EventArgs) Handles DecreaseMinTempButton.Click
        UpdateMinTemp(0)
    End Sub

    'Saves the port name and temperature settings of the program when the exit button is pressed
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Quit()
    End Sub

    'Check the sensor status' every 2 minutes
    Private Sub CheckSensorTimer_Tick(sender As Object, e As EventArgs) Handles CheckSensorTimer.Tick
        CheckSensor(True)
    End Sub

    'Put the system into heating mode when button is pressed
    Private Sub HeatButton_Click(sender As Object, e As EventArgs) Handles HeatButton.Click
        ControllerMode(False, 2)
        UpdateTempIndicators(1)
    End Sub

    'Put system into cooling mode when button is pressed
    Private Sub CoolButton_Click(sender As Object, e As EventArgs) Handles CoolButton.Click
        ControllerMode(False, 1)
        UpdateTempIndicators(2)
    End Sub

    'Read inputs from the Qy@ board every 250 ms if connected
    Private Sub RequestDataTimer_Tick(sender As Object, e As EventArgs) Handles RequestDataTimer.Tick
        If ReadyToReceiveData(-1) Then
            ReadQyInputs()
        End If
    End Sub
End Class
