<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SmartHomeControllerForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.IncreaseMaxTempButton = New System.Windows.Forms.Button()
        Me.DecreaseMaxTempButton = New System.Windows.Forms.Button()
        Me.CurrentTempLabel = New System.Windows.Forms.Label()
        Me.MaxTempTextBox = New System.Windows.Forms.TextBox()
        Me.MinTempTextBox = New System.Windows.Forms.TextBox()
        Me.MaxTempLabel = New System.Windows.Forms.Label()
        Me.DecreaseMinTempButton = New System.Windows.Forms.Button()
        Me.IncreaseMinTempButton = New System.Windows.Forms.Button()
        Me.MinTempLabel = New System.Windows.Forms.Label()
        Me.CurrentTimeLabel = New System.Windows.Forms.Label()
        Me.SerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.TopMenuStrip = New System.Windows.Forms.MenuStrip()
        Me.SetupToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConnectToQyBoardToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateCurrentTimeTimer = New System.Windows.Forms.Timer(Me.components)
        Me.CurrentDateLabel = New System.Windows.Forms.Label()
        Me.CheckSensorTimer = New System.Windows.Forms.Timer(Me.components)
        Me.Alert1Label = New System.Windows.Forms.Label()
        Me.Alert2Label = New System.Windows.Forms.Label()
        Me.HeatButton = New System.Windows.Forms.Button()
        Me.CoolButton = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.CoolingIndicatorPictureBox = New System.Windows.Forms.PictureBox()
        Me.HeatIndicatorPictureBox = New System.Windows.Forms.PictureBox()
        Me.Alert2PictureBox = New System.Windows.Forms.PictureBox()
        Me.Alert1PictureBox = New System.Windows.Forms.PictureBox()
        Me.RequestDataTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TopMenuStrip.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CoolingIndicatorPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.HeatIndicatorPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Alert2PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Alert1PictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'IncreaseMaxTempButton
        '
        Me.IncreaseMaxTempButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.IncreaseMaxTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.IncreaseMaxTempButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IncreaseMaxTempButton.ForeColor = System.Drawing.Color.Black
        Me.IncreaseMaxTempButton.Location = New System.Drawing.Point(110, 186)
        Me.IncreaseMaxTempButton.Name = "IncreaseMaxTempButton"
        Me.IncreaseMaxTempButton.Size = New System.Drawing.Size(38, 38)
        Me.IncreaseMaxTempButton.TabIndex = 0
        Me.IncreaseMaxTempButton.Text = "+"
        Me.IncreaseMaxTempButton.UseVisualStyleBackColor = True
        '
        'DecreaseMaxTempButton
        '
        Me.DecreaseMaxTempButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.DecreaseMaxTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.DecreaseMaxTempButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DecreaseMaxTempButton.ForeColor = System.Drawing.Color.Black
        Me.DecreaseMaxTempButton.Location = New System.Drawing.Point(154, 186)
        Me.DecreaseMaxTempButton.Name = "DecreaseMaxTempButton"
        Me.DecreaseMaxTempButton.Size = New System.Drawing.Size(38, 38)
        Me.DecreaseMaxTempButton.TabIndex = 1
        Me.DecreaseMaxTempButton.Text = "-"
        Me.DecreaseMaxTempButton.UseVisualStyleBackColor = True
        '
        'CurrentTempLabel
        '
        Me.CurrentTempLabel.AutoSize = True
        Me.CurrentTempLabel.Font = New System.Drawing.Font("Roboto Slab", 28.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentTempLabel.Location = New System.Drawing.Point(8, 130)
        Me.CurrentTempLabel.Name = "CurrentTempLabel"
        Me.CurrentTempLabel.Size = New System.Drawing.Size(125, 51)
        Me.CurrentTempLabel.TabIndex = 2
        Me.CurrentTempLabel.Text = "Temp"
        '
        'MaxTempTextBox
        '
        Me.MaxTempTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.MaxTempTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MaxTempTextBox.Font = New System.Drawing.Font("Roboto Slab", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaxTempTextBox.Location = New System.Drawing.Point(68, 193)
        Me.MaxTempTextBox.Name = "MaxTempTextBox"
        Me.MaxTempTextBox.Size = New System.Drawing.Size(36, 25)
        Me.MaxTempTextBox.TabIndex = 3
        Me.MaxTempTextBox.Text = "70"
        '
        'MinTempTextBox
        '
        Me.MinTempTextBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.MinTempTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.MinTempTextBox.Font = New System.Drawing.Font("Roboto Slab", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinTempTextBox.Location = New System.Drawing.Point(68, 237)
        Me.MinTempTextBox.Name = "MinTempTextBox"
        Me.MinTempTextBox.Size = New System.Drawing.Size(36, 25)
        Me.MinTempTextBox.TabIndex = 4
        Me.MinTempTextBox.Text = "50"
        '
        'MaxTempLabel
        '
        Me.MaxTempLabel.AutoSize = True
        Me.MaxTempLabel.Font = New System.Drawing.Font("Roboto Slab", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaxTempLabel.Location = New System.Drawing.Point(13, 193)
        Me.MaxTempLabel.Name = "MaxTempLabel"
        Me.MaxTempLabel.Size = New System.Drawing.Size(56, 26)
        Me.MaxTempLabel.TabIndex = 5
        Me.MaxTempLabel.Text = "Max:"
        '
        'DecreaseMinTempButton
        '
        Me.DecreaseMinTempButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.DecreaseMinTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.DecreaseMinTempButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DecreaseMinTempButton.ForeColor = System.Drawing.Color.Black
        Me.DecreaseMinTempButton.Location = New System.Drawing.Point(154, 230)
        Me.DecreaseMinTempButton.Name = "DecreaseMinTempButton"
        Me.DecreaseMinTempButton.Size = New System.Drawing.Size(38, 38)
        Me.DecreaseMinTempButton.TabIndex = 7
        Me.DecreaseMinTempButton.Text = "-"
        Me.DecreaseMinTempButton.UseVisualStyleBackColor = True
        '
        'IncreaseMinTempButton
        '
        Me.IncreaseMinTempButton.BackColor = System.Drawing.Color.FromArgb(CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer), CType(CType(130, Byte), Integer))
        Me.IncreaseMinTempButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.IncreaseMinTempButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.IncreaseMinTempButton.ForeColor = System.Drawing.Color.Black
        Me.IncreaseMinTempButton.Location = New System.Drawing.Point(110, 230)
        Me.IncreaseMinTempButton.Name = "IncreaseMinTempButton"
        Me.IncreaseMinTempButton.Size = New System.Drawing.Size(38, 38)
        Me.IncreaseMinTempButton.TabIndex = 6
        Me.IncreaseMinTempButton.Text = "+"
        Me.IncreaseMinTempButton.UseVisualStyleBackColor = True
        '
        'MinTempLabel
        '
        Me.MinTempLabel.AutoSize = True
        Me.MinTempLabel.Font = New System.Drawing.Font("Roboto Slab", 13.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinTempLabel.Location = New System.Drawing.Point(13, 237)
        Me.MinTempLabel.Name = "MinTempLabel"
        Me.MinTempLabel.Size = New System.Drawing.Size(52, 26)
        Me.MinTempLabel.TabIndex = 8
        Me.MinTempLabel.Text = "Min:"
        '
        'CurrentTimeLabel
        '
        Me.CurrentTimeLabel.AutoSize = True
        Me.CurrentTimeLabel.Font = New System.Drawing.Font("Roboto Slab", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentTimeLabel.Location = New System.Drawing.Point(12, 43)
        Me.CurrentTimeLabel.Name = "CurrentTimeLabel"
        Me.CurrentTimeLabel.Size = New System.Drawing.Size(72, 32)
        Me.CurrentTimeLabel.TabIndex = 9
        Me.CurrentTimeLabel.Text = "Time"
        '
        'SerialPort
        '
        '
        'TopMenuStrip
        '
        Me.TopMenuStrip.Font = New System.Drawing.Font("Roboto Slab", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TopMenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.TopMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SetupToolStripMenuItem})
        Me.TopMenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.TopMenuStrip.Name = "TopMenuStrip"
        Me.TopMenuStrip.Size = New System.Drawing.Size(357, 25)
        Me.TopMenuStrip.TabIndex = 10
        Me.TopMenuStrip.Text = "MenuStrip1"
        '
        'SetupToolStripMenuItem
        '
        Me.SetupToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConnectToQyBoardToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.SetupToolStripMenuItem.Name = "SetupToolStripMenuItem"
        Me.SetupToolStripMenuItem.Size = New System.Drawing.Size(51, 21)
        Me.SetupToolStripMenuItem.Text = "&Setup"
        '
        'ConnectToQyBoardToolStripMenuItem
        '
        Me.ConnectToQyBoardToolStripMenuItem.Name = "ConnectToQyBoardToolStripMenuItem"
        Me.ConnectToQyBoardToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ConnectToQyBoardToolStripMenuItem.Text = "&Connect to Qy@ board"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(202, 22)
        Me.ExitToolStripMenuItem.Text = "E&xit"
        '
        'UpdateCurrentTimeTimer
        '
        Me.UpdateCurrentTimeTimer.Enabled = True
        Me.UpdateCurrentTimeTimer.Interval = 1000
        '
        'CurrentDateLabel
        '
        Me.CurrentDateLabel.AutoSize = True
        Me.CurrentDateLabel.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CurrentDateLabel.Location = New System.Drawing.Point(14, 74)
        Me.CurrentDateLabel.Name = "CurrentDateLabel"
        Me.CurrentDateLabel.Size = New System.Drawing.Size(44, 22)
        Me.CurrentDateLabel.TabIndex = 11
        Me.CurrentDateLabel.Text = "Date"
        '
        'CheckSensorTimer
        '
        Me.CheckSensorTimer.Interval = 120000
        '
        'Alert1Label
        '
        Me.Alert1Label.AutoSize = True
        Me.Alert1Label.Location = New System.Drawing.Point(41, 284)
        Me.Alert1Label.Name = "Alert1Label"
        Me.Alert1Label.Size = New System.Drawing.Size(177, 15)
        Me.Alert1Label.TabIndex = 13
        Me.Alert1Label.Text = "Alert: Safety Interlock Activated"
        '
        'Alert2Label
        '
        Me.Alert2Label.AutoSize = True
        Me.Alert2Label.Location = New System.Drawing.Point(41, 309)
        Me.Alert2Label.Name = "Alert2Label"
        Me.Alert2Label.Size = New System.Drawing.Size(177, 15)
        Me.Alert2Label.TabIndex = 15
        Me.Alert2Label.Text = "Alert: Safety Interlock Activated"
        '
        'HeatButton
        '
        Me.HeatButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HeatButton.Location = New System.Drawing.Point(235, 186)
        Me.HeatButton.Name = "HeatButton"
        Me.HeatButton.Size = New System.Drawing.Size(82, 38)
        Me.HeatButton.TabIndex = 19
        Me.HeatButton.Text = "Heat"
        Me.HeatButton.UseVisualStyleBackColor = True
        '
        'CoolButton
        '
        Me.CoolButton.Font = New System.Drawing.Font("Roboto Slab", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CoolButton.Location = New System.Drawing.Point(235, 230)
        Me.CoolButton.Name = "CoolButton"
        Me.CoolButton.Size = New System.Drawing.Size(82, 38)
        Me.CoolButton.TabIndex = 20
        Me.CoolButton.Text = "A/C"
        Me.CoolButton.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = Global.SmartHomeController.My.Resources.Resources.Gear2020
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.Location = New System.Drawing.Point(268, 41)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(75, 75)
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'CoolingIndicatorPictureBox
        '
        Me.CoolingIndicatorPictureBox.BackgroundImage = Global.SmartHomeController.My.Resources.Resources.snowflake1
        Me.CoolingIndicatorPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CoolingIndicatorPictureBox.Location = New System.Drawing.Point(142, 134)
        Me.CoolingIndicatorPictureBox.Name = "CoolingIndicatorPictureBox"
        Me.CoolingIndicatorPictureBox.Size = New System.Drawing.Size(43, 43)
        Me.CoolingIndicatorPictureBox.TabIndex = 17
        Me.CoolingIndicatorPictureBox.TabStop = False
        '
        'HeatIndicatorPictureBox
        '
        Me.HeatIndicatorPictureBox.BackgroundImage = Global.SmartHomeController.My.Resources.Resources.fire
        Me.HeatIndicatorPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.HeatIndicatorPictureBox.Location = New System.Drawing.Point(142, 134)
        Me.HeatIndicatorPictureBox.Name = "HeatIndicatorPictureBox"
        Me.HeatIndicatorPictureBox.Size = New System.Drawing.Size(43, 43)
        Me.HeatIndicatorPictureBox.TabIndex = 16
        Me.HeatIndicatorPictureBox.TabStop = False
        '
        'Alert2PictureBox
        '
        Me.Alert2PictureBox.BackgroundImage = Global.SmartHomeController.My.Resources.Resources._381599_error_icon
        Me.Alert2PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Alert2PictureBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.Alert2PictureBox.Location = New System.Drawing.Point(19, 306)
        Me.Alert2PictureBox.Name = "Alert2PictureBox"
        Me.Alert2PictureBox.Size = New System.Drawing.Size(20, 20)
        Me.Alert2PictureBox.TabIndex = 14
        Me.Alert2PictureBox.TabStop = False
        '
        'Alert1PictureBox
        '
        Me.Alert1PictureBox.BackgroundImage = Global.SmartHomeController.My.Resources.Resources._381599_error_icon
        Me.Alert1PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Alert1PictureBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.Alert1PictureBox.Location = New System.Drawing.Point(19, 281)
        Me.Alert1PictureBox.Name = "Alert1PictureBox"
        Me.Alert1PictureBox.Size = New System.Drawing.Size(20, 20)
        Me.Alert1PictureBox.TabIndex = 12
        Me.Alert1PictureBox.TabStop = False
        '
        'RequestDataTimer
        '
        Me.RequestDataTimer.Enabled = True
        Me.RequestDataTimer.Interval = 250
        '
        'SmartHomeControllerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(230, Byte), Integer), CType(CType(231, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(357, 340)
        Me.Controls.Add(Me.CoolButton)
        Me.Controls.Add(Me.HeatButton)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.CoolingIndicatorPictureBox)
        Me.Controls.Add(Me.HeatIndicatorPictureBox)
        Me.Controls.Add(Me.Alert2Label)
        Me.Controls.Add(Me.Alert2PictureBox)
        Me.Controls.Add(Me.Alert1Label)
        Me.Controls.Add(Me.Alert1PictureBox)
        Me.Controls.Add(Me.CurrentDateLabel)
        Me.Controls.Add(Me.CurrentTimeLabel)
        Me.Controls.Add(Me.MinTempLabel)
        Me.Controls.Add(Me.DecreaseMinTempButton)
        Me.Controls.Add(Me.IncreaseMinTempButton)
        Me.Controls.Add(Me.MaxTempLabel)
        Me.Controls.Add(Me.MinTempTextBox)
        Me.Controls.Add(Me.MaxTempTextBox)
        Me.Controls.Add(Me.CurrentTempLabel)
        Me.Controls.Add(Me.DecreaseMaxTempButton)
        Me.Controls.Add(Me.IncreaseMaxTempButton)
        Me.Controls.Add(Me.TopMenuStrip)
        Me.Font = New System.Drawing.Font("Roboto Slab", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.TopMenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4, 3, 4, 3)
        Me.Name = "SmartHomeControllerForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Smart Home Controller"
        Me.TopMenuStrip.ResumeLayout(False)
        Me.TopMenuStrip.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CoolingIndicatorPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.HeatIndicatorPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Alert2PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Alert1PictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents IncreaseMaxTempButton As Button
    Friend WithEvents DecreaseMaxTempButton As Button
    Friend WithEvents CurrentTempLabel As Label
    Friend WithEvents MaxTempTextBox As TextBox
    Friend WithEvents MinTempTextBox As TextBox
    Friend WithEvents MaxTempLabel As Label
    Friend WithEvents DecreaseMinTempButton As Button
    Friend WithEvents IncreaseMinTempButton As Button
    Friend WithEvents MinTempLabel As Label
    Friend WithEvents CurrentTimeLabel As Label
    Friend WithEvents SerialPort As IO.Ports.SerialPort
    Friend WithEvents TopMenuStrip As MenuStrip
    Friend WithEvents SetupToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConnectToQyBoardToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UpdateCurrentTimeTimer As Timer
    Friend WithEvents CurrentDateLabel As Label
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CheckSensorTimer As Timer
    Friend WithEvents Alert1PictureBox As PictureBox
    Friend WithEvents Alert1Label As Label
    Friend WithEvents Alert2Label As Label
    Friend WithEvents Alert2PictureBox As PictureBox
    Friend WithEvents HeatIndicatorPictureBox As PictureBox
    Friend WithEvents CoolingIndicatorPictureBox As PictureBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents HeatButton As Button
    Friend WithEvents CoolButton As Button
    Friend WithEvents RequestDataTimer As Timer
End Class
