Imports System.IO.Ports
Imports System.Threading.Thread
Public Class SerialPortSelectForm

    'Try and connect to each COM port to determine which one the Qy@ board is connected to in order to populate the combo box
    Sub GetComPorts()
        SerialComPortsComboBox.Items.Clear()
        SerialComPortsComboBox.Text = ""

        For Each portName In SerialPort.GetPortNames()
            SerialConnect(portName)
            GetSettings()
            Sleep(5)
            Try
                Dim data(SmartHomeControllerForm.SerialPort.BytesToRead) As Byte
                SmartHomeControllerForm.SerialPort.Read(data, 0, SmartHomeControllerForm.SerialPort.BytesToRead)
                'Byte :  58 | HEX: 51 | DEC: 81  | ASCII: Q
                'Byte :  59 | HEX: 79 | DEC: 121 | ASCII: y
                'Byte :  60 | HEX: 40 | DEC: 64  | ASCII: @
                If data.Length >= 64 Then
                    If data(58) = 81 And data(59) = 121 And data(60) = 64 Then
                        'MsgBox($"Qy@ Board COM Confirmed on port: {SerialPort.PortName}")
                        SerialComPortsComboBox.Items.Add(SmartHomeControllerForm.SerialPort.PortName)
                        SerialComPortsComboBox.SelectedItem = SmartHomeControllerForm.SerialPort.PortName

                        'UpdateStatus()
                    End If
                Else
                    'MsgBox($"{SerialPort.PortName} is not a Qy@ board.")
                End If
            Catch ex As Exception

            End Try

        Next
        'PortComboBox.SelectedIndex = 0

        SmartHomeControllerForm.SerialPort.Close()
    End Sub

    'Write to the Qy@ board to see if you get a handshake back
    Function GetSettings() As Byte()
        Dim data(0) As Byte
        data(0) = &B11110000

        Try
            SmartHomeControllerForm.SerialPort.Write(data, 0, 1)
        Catch ex As Exception

        End Try

        Return data
    End Function

    'Try to open the port with the selected portname
    Sub SerialConnect(portName As String)
        Dim data(1) As Byte

        data(0) = &H20
        data(1) = &H0

        Try
            SmartHomeControllerForm.SerialPort.Close()
            SmartHomeControllerForm.SerialPort.PortName = portName
            SmartHomeControllerForm.SerialPort.BaudRate = 9600
            SmartHomeControllerForm.SerialPort.Open()
            SmartHomeControllerForm.SerialPort.Write(data, 0, 2)
        Catch ex As Exception

        End Try

    End Sub

    'upon startup, bring form to front, and then populate the combo box
    Private Sub SerialPortSelectForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SmartHomeControllerForm.WindowState = FormWindowState.Minimized
        Me.TopMost = True
        GetComPorts()
    End Sub

    'Repopulate the combo box
    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        GetComPorts()
    End Sub

    'Try to connect to the Qy@ board, and close this form and bring back the main form
    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        Try
            SmartHomeControllerForm.SerialPort.PortName = SerialComPortsComboBox.Text
            SmartHomeControllerForm.SerialPort.BaudRate = 9600
            SmartHomeControllerForm.SerialPort.Open()
            SmartHomeControllerForm.ReadyToReceiveData(1)
            SmartHomeControllerForm.WindowState = FormWindowState.Normal
            SmartHomeControllerForm.CheckSensorTimer.Enabled = True
            Me.Close()
        Catch ex As Exception
            If SerialComPortsComboBox.Text = "" Then
                MsgBox("Please select a valid COM port and try again.")
            Else
                MsgBox("Attempting to connect to the selected COM port caused an error.")
            End If
        End Try
    End Sub
End Class