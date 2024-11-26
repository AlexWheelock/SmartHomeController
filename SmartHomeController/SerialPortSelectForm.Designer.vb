<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SerialPortSelectForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ConnectButton = New System.Windows.Forms.Button()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.SerialComPortsComboBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'ConnectButton
        '
        Me.ConnectButton.Location = New System.Drawing.Point(46, 66)
        Me.ConnectButton.Margin = New System.Windows.Forms.Padding(2)
        Me.ConnectButton.Name = "ConnectButton"
        Me.ConnectButton.Size = New System.Drawing.Size(135, 24)
        Me.ConnectButton.TabIndex = 2
        Me.ConnectButton.Text = "Connect"
        Me.ConnectButton.UseVisualStyleBackColor = True
        '
        'RefreshButton
        '
        Me.RefreshButton.Location = New System.Drawing.Point(46, 38)
        Me.RefreshButton.Margin = New System.Windows.Forms.Padding(2)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(135, 24)
        Me.RefreshButton.TabIndex = 1
        Me.RefreshButton.Text = "Refresh"
        Me.RefreshButton.UseVisualStyleBackColor = True
        '
        'SerialComPortsComboBox
        '
        Me.SerialComPortsComboBox.FormattingEnabled = True
        Me.SerialComPortsComboBox.Location = New System.Drawing.Point(46, 11)
        Me.SerialComPortsComboBox.Margin = New System.Windows.Forms.Padding(2)
        Me.SerialComPortsComboBox.Name = "SerialComPortsComboBox"
        Me.SerialComPortsComboBox.Size = New System.Drawing.Size(135, 23)
        Me.SerialComPortsComboBox.TabIndex = 0
        '
        'SerialPortSelectForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(229, 101)
        Me.Controls.Add(Me.ConnectButton)
        Me.Controls.Add(Me.RefreshButton)
        Me.Controls.Add(Me.SerialComPortsComboBox)
        Me.Font = New System.Drawing.Font("Roboto Slab", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "SerialPortSelectForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "COM Port Select"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ConnectButton As Button
    Friend WithEvents RefreshButton As Button
    Friend WithEvents SerialComPortsComboBox As ComboBox
End Class
