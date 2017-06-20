namespace CheckupExecApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ConfigAnalysisTab = new System.Windows.Forms.TabPage();
            this.SettingsProgressBar = new System.Windows.Forms.ProgressBar();
            this.GlobalSettingsTextBox = new System.Windows.Forms.TextBox();
            this.HelpButton = new System.Windows.Forms.Button();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.FolderPathBrowseButton = new System.Windows.Forms.Button();
            this.ProgressBarLabel = new System.Windows.Forms.Label();
            this.FolderPathLabel = new System.Windows.Forms.Label();
            this.FolderPathTextBox = new System.Windows.Forms.TextBox();
            this.GlobalSettingsLabel = new System.Windows.Forms.Label();
            this.BackupJobStorageAnalysisTab = new System.Windows.Forms.TabPage();
            this.DiskInfoLabel = new System.Windows.Forms.Label();
            this.SettingsProgressBar2 = new System.Windows.Forms.ProgressBar();
            this.HelpButton2 = new System.Windows.Forms.Button();
            this.GenerateButton2 = new System.Windows.Forms.Button();
            this.FolderPathBrowseButton2 = new System.Windows.Forms.Button();
            this.ProgressBarLabel2 = new System.Windows.Forms.Label();
            this.FolderPathLabel2 = new System.Windows.Forms.Label();
            this.FolderPathTextBox2 = new System.Windows.Forms.TextBox();
            this.DriveUsageChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ElapsedTimeLabel = new System.Windows.Forms.Label();
            this.EndTimeLabel = new System.Windows.Forms.Label();
            this.StartTimeLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.FreeSpaceLabel = new System.Windows.Forms.Label();
            this.UsedSpaceLabel = new System.Windows.Forms.Label();
            this.BackupJobSizeLabel = new System.Windows.Forms.Label();
            this.BackupJobDriveLocLabel = new System.Windows.Forms.Label();
            this.BackupJobNameLabel = new System.Windows.Forms.Label();
            this.FilePathBrowseButton = new System.Windows.Forms.Button();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.FilePathTextBox = new System.Windows.Forms.TextBox();
            this.ErrorAnalysisTab = new System.Windows.Forms.TabPage();
            this.EndDateTimePickerLabel = new System.Windows.Forms.Label();
            this.StartDateTimePickerLabel = new System.Windows.Forms.Label();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SelectAllAlertTypesCheckBox = new System.Windows.Forms.CheckBox();
            this.AlertTypeCheckedListBoxLabel = new System.Windows.Forms.Label();
            this.AlertTypesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.SettingsProgressBar3 = new System.Windows.Forms.ProgressBar();
            this.HelpButton3 = new System.Windows.Forms.Button();
            this.GenerateButton3 = new System.Windows.Forms.Button();
            this.FolderPathBrowseButton3 = new System.Windows.Forms.Button();
            this.ProgressBarLabel3 = new System.Windows.Forms.Label();
            this.FolderPathLabel3 = new System.Windows.Forms.Label();
            this.FolderPathTextBox3 = new System.Windows.Forms.TextBox();
            this.SelectAllBackupJobsCheckBox = new System.Windows.Forms.CheckBox();
            this.SelectAllStorageDevicesCheckBox = new System.Windows.Forms.CheckBox();
            this.BackupJobCheckedListBoxLabel = new System.Windows.Forms.Label();
            this.StorageDevicesCheckedListBoxLabel = new System.Windows.Forms.Label();
            this.BackupJobsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.StorageDevicesCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.tabControl1.SuspendLayout();
            this.ConfigAnalysisTab.SuspendLayout();
            this.BackupJobStorageAnalysisTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DriveUsageChart)).BeginInit();
            this.ErrorAnalysisTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ConfigAnalysisTab);
            this.tabControl1.Controls.Add(this.BackupJobStorageAnalysisTab);
            this.tabControl1.Controls.Add(this.ErrorAnalysisTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1254, 880);
            this.tabControl1.TabIndex = 0;
            // 
            // ConfigAnalysisTab
            // 
            this.ConfigAnalysisTab.Controls.Add(this.SettingsProgressBar);
            this.ConfigAnalysisTab.Controls.Add(this.GlobalSettingsTextBox);
            this.ConfigAnalysisTab.Controls.Add(this.HelpButton);
            this.ConfigAnalysisTab.Controls.Add(this.GenerateButton);
            this.ConfigAnalysisTab.Controls.Add(this.FolderPathBrowseButton);
            this.ConfigAnalysisTab.Controls.Add(this.ProgressBarLabel);
            this.ConfigAnalysisTab.Controls.Add(this.FolderPathLabel);
            this.ConfigAnalysisTab.Controls.Add(this.FolderPathTextBox);
            this.ConfigAnalysisTab.Controls.Add(this.GlobalSettingsLabel);
            this.ConfigAnalysisTab.Location = new System.Drawing.Point(8, 39);
            this.ConfigAnalysisTab.Name = "ConfigAnalysisTab";
            this.ConfigAnalysisTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigAnalysisTab.Size = new System.Drawing.Size(1238, 833);
            this.ConfigAnalysisTab.TabIndex = 0;
            this.ConfigAnalysisTab.Text = "Configuration Settings Overview";
            this.ConfigAnalysisTab.UseVisualStyleBackColor = true;
            // 
            // SettingsProgressBar
            // 
            this.SettingsProgressBar.Location = new System.Drawing.Point(150, 757);
            this.SettingsProgressBar.Name = "SettingsProgressBar";
            this.SettingsProgressBar.Size = new System.Drawing.Size(703, 42);
            this.SettingsProgressBar.TabIndex = 12;
            // 
            // GlobalSettingsTextBox
            // 
            this.GlobalSettingsTextBox.Font = new System.Drawing.Font("Lucida Console", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GlobalSettingsTextBox.Location = new System.Drawing.Point(45, 64);
            this.GlobalSettingsTextBox.MaxLength = 0;
            this.GlobalSettingsTextBox.Multiline = true;
            this.GlobalSettingsTextBox.Name = "GlobalSettingsTextBox";
            this.GlobalSettingsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.GlobalSettingsTextBox.Size = new System.Drawing.Size(1150, 578);
            this.GlobalSettingsTextBox.TabIndex = 11;
            this.GlobalSettingsTextBox.WordWrap = false;
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(1045, 757);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(150, 42);
            this.HelpButton.TabIndex = 10;
            this.HelpButton.Text = "Help";
            this.HelpButton.UseVisualStyleBackColor = true;
            // 
            // GenerateButton
            // 
            this.GenerateButton.Location = new System.Drawing.Point(879, 757);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(150, 42);
            this.GenerateButton.TabIndex = 10;
            this.GenerateButton.Text = "Generate";
            this.GenerateButton.UseVisualStyleBackColor = true;
            // 
            // FolderPathBrowseButton
            // 
            this.FolderPathBrowseButton.Location = new System.Drawing.Point(1045, 687);
            this.FolderPathBrowseButton.Name = "FolderPathBrowseButton";
            this.FolderPathBrowseButton.Size = new System.Drawing.Size(150, 42);
            this.FolderPathBrowseButton.TabIndex = 9;
            this.FolderPathBrowseButton.Text = "Browse";
            this.FolderPathBrowseButton.UseVisualStyleBackColor = true;
            this.FolderPathBrowseButton.Click += new System.EventHandler(this.FolderPathBrowseButton_Click);
            // 
            // ProgressBarLabel
            // 
            this.ProgressBarLabel.AutoSize = true;
            this.ProgressBarLabel.Location = new System.Drawing.Point(40, 766);
            this.ProgressBarLabel.Name = "ProgressBarLabel";
            this.ProgressBarLabel.Size = new System.Drawing.Size(104, 25);
            this.ProgressBarLabel.TabIndex = 8;
            this.ProgressBarLabel.Text = "Progress:";
            // 
            // FolderPathLabel
            // 
            this.FolderPathLabel.AutoSize = true;
            this.FolderPathLabel.Location = new System.Drawing.Point(40, 696);
            this.FolderPathLabel.Name = "FolderPathLabel";
            this.FolderPathLabel.Size = new System.Drawing.Size(408, 25);
            this.FolderPathLabel.TabIndex = 8;
            this.FolderPathLabel.Text = "Destination Folder For Generated Report:";
            // 
            // FolderPathTextBox
            // 
            this.FolderPathTextBox.Location = new System.Drawing.Point(454, 688);
            this.FolderPathTextBox.Name = "FolderPathTextBox";
            this.FolderPathTextBox.ReadOnly = true;
            this.FolderPathTextBox.Size = new System.Drawing.Size(575, 31);
            this.FolderPathTextBox.TabIndex = 7;
            // 
            // GlobalSettingsLabel
            // 
            this.GlobalSettingsLabel.AutoSize = true;
            this.GlobalSettingsLabel.Location = new System.Drawing.Point(40, 24);
            this.GlobalSettingsLabel.Name = "GlobalSettingsLabel";
            this.GlobalSettingsLabel.Size = new System.Drawing.Size(290, 25);
            this.GlobalSettingsLabel.TabIndex = 1;
            this.GlobalSettingsLabel.Text = "Global Backup Exec Settings";
            // 
            // BackupJobStorageAnalysisTab
            // 
            this.BackupJobStorageAnalysisTab.Controls.Add(this.DiskInfoLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.SettingsProgressBar2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.HelpButton2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.GenerateButton2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FolderPathBrowseButton2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.ProgressBarLabel2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FolderPathLabel2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FolderPathTextBox2);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.DriveUsageChart);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.ElapsedTimeLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.EndTimeLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.StartTimeLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.StatusLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FreeSpaceLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.UsedSpaceLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.BackupJobSizeLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.BackupJobDriveLocLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.BackupJobNameLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FilePathBrowseButton);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FilePathLabel);
            this.BackupJobStorageAnalysisTab.Controls.Add(this.FilePathTextBox);
            this.BackupJobStorageAnalysisTab.Location = new System.Drawing.Point(8, 39);
            this.BackupJobStorageAnalysisTab.Name = "BackupJobStorageAnalysisTab";
            this.BackupJobStorageAnalysisTab.Padding = new System.Windows.Forms.Padding(3);
            this.BackupJobStorageAnalysisTab.Size = new System.Drawing.Size(1238, 833);
            this.BackupJobStorageAnalysisTab.TabIndex = 1;
            this.BackupJobStorageAnalysisTab.Text = "Backup Job/Storage Analysis";
            this.BackupJobStorageAnalysisTab.UseVisualStyleBackColor = true;
            // 
            // DiskInfoLabel
            // 
            this.DiskInfoLabel.AutoSize = true;
            this.DiskInfoLabel.Location = new System.Drawing.Point(650, 115);
            this.DiskInfoLabel.Name = "DiskInfoLabel";
            this.DiskInfoLabel.Size = new System.Drawing.Size(0, 25);
            this.DiskInfoLabel.TabIndex = 22;
            // 
            // SettingsProgressBar2
            // 
            this.SettingsProgressBar2.Location = new System.Drawing.Point(150, 757);
            this.SettingsProgressBar2.Name = "SettingsProgressBar2";
            this.SettingsProgressBar2.Size = new System.Drawing.Size(703, 42);
            this.SettingsProgressBar2.TabIndex = 21;
            // 
            // HelpButton2
            // 
            this.HelpButton2.Location = new System.Drawing.Point(1045, 757);
            this.HelpButton2.Name = "HelpButton2";
            this.HelpButton2.Size = new System.Drawing.Size(150, 42);
            this.HelpButton2.TabIndex = 19;
            this.HelpButton2.Text = "Help";
            this.HelpButton2.UseVisualStyleBackColor = true;
            // 
            // GenerateButton2
            // 
            this.GenerateButton2.Location = new System.Drawing.Point(879, 757);
            this.GenerateButton2.Name = "GenerateButton2";
            this.GenerateButton2.Size = new System.Drawing.Size(150, 42);
            this.GenerateButton2.TabIndex = 20;
            this.GenerateButton2.Text = "Generate";
            this.GenerateButton2.UseVisualStyleBackColor = true;
            // 
            // FolderPathBrowseButton2
            // 
            this.FolderPathBrowseButton2.Location = new System.Drawing.Point(1045, 687);
            this.FolderPathBrowseButton2.Name = "FolderPathBrowseButton2";
            this.FolderPathBrowseButton2.Size = new System.Drawing.Size(150, 42);
            this.FolderPathBrowseButton2.TabIndex = 18;
            this.FolderPathBrowseButton2.Text = "Browse";
            this.FolderPathBrowseButton2.UseVisualStyleBackColor = true;
            this.FolderPathBrowseButton2.Click += new System.EventHandler(this.FolderPathBrowseButton2_Click);
            // 
            // ProgressBarLabel2
            // 
            this.ProgressBarLabel2.AutoSize = true;
            this.ProgressBarLabel2.Location = new System.Drawing.Point(40, 766);
            this.ProgressBarLabel2.Name = "ProgressBarLabel2";
            this.ProgressBarLabel2.Size = new System.Drawing.Size(104, 25);
            this.ProgressBarLabel2.TabIndex = 16;
            this.ProgressBarLabel2.Text = "Progress:";
            // 
            // FolderPathLabel2
            // 
            this.FolderPathLabel2.AutoSize = true;
            this.FolderPathLabel2.Location = new System.Drawing.Point(40, 696);
            this.FolderPathLabel2.Name = "FolderPathLabel2";
            this.FolderPathLabel2.Size = new System.Drawing.Size(408, 25);
            this.FolderPathLabel2.TabIndex = 17;
            this.FolderPathLabel2.Text = "Destination Folder For Generated Report:";
            // 
            // FolderPathTextBox2
            // 
            this.FolderPathTextBox2.Location = new System.Drawing.Point(454, 688);
            this.FolderPathTextBox2.Name = "FolderPathTextBox2";
            this.FolderPathTextBox2.ReadOnly = true;
            this.FolderPathTextBox2.Size = new System.Drawing.Size(575, 31);
            this.FolderPathTextBox2.TabIndex = 15;
            // 
            // DriveUsageChart
            // 
            chartArea6.Name = "ChartArea1";
            this.DriveUsageChart.ChartAreas.Add(chartArea6);
            this.DriveUsageChart.Location = new System.Drawing.Point(484, 143);
            this.DriveUsageChart.Name = "DriveUsageChart";
            this.DriveUsageChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.DriveUsageChart.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(160)))), ((int)(((byte)(218))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))))};
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series6.Name = "DriveUsage";
            this.DriveUsageChart.Series.Add(series6);
            this.DriveUsageChart.Size = new System.Drawing.Size(762, 539);
            this.DriveUsageChart.TabIndex = 14;
            this.DriveUsageChart.Text = "Drive Usage";
            // 
            // ElapsedTimeLabel
            // 
            this.ElapsedTimeLabel.AutoSize = true;
            this.ElapsedTimeLabel.Location = new System.Drawing.Point(40, 608);
            this.ElapsedTimeLabel.Name = "ElapsedTimeLabel";
            this.ElapsedTimeLabel.Size = new System.Drawing.Size(149, 25);
            this.ElapsedTimeLabel.TabIndex = 13;
            this.ElapsedTimeLabel.Text = "Elapsed Time:";
            // 
            // EndTimeLabel
            // 
            this.EndTimeLabel.AutoSize = true;
            this.EndTimeLabel.Location = new System.Drawing.Point(40, 547);
            this.EndTimeLabel.Name = "EndTimeLabel";
            this.EndTimeLabel.Size = new System.Drawing.Size(109, 25);
            this.EndTimeLabel.TabIndex = 13;
            this.EndTimeLabel.Text = "End Time:";
            // 
            // StartTimeLabel
            // 
            this.StartTimeLabel.AutoSize = true;
            this.StartTimeLabel.Location = new System.Drawing.Point(40, 485);
            this.StartTimeLabel.Name = "StartTimeLabel";
            this.StartTimeLabel.Size = new System.Drawing.Size(116, 25);
            this.StartTimeLabel.TabIndex = 13;
            this.StartTimeLabel.Text = "Start Time:";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(40, 418);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(79, 25);
            this.StatusLabel.TabIndex = 13;
            this.StatusLabel.Text = "Status:";
            // 
            // FreeSpaceLabel
            // 
            this.FreeSpaceLabel.AutoSize = true;
            this.FreeSpaceLabel.Location = new System.Drawing.Point(40, 357);
            this.FreeSpaceLabel.Name = "FreeSpaceLabel";
            this.FreeSpaceLabel.Size = new System.Drawing.Size(229, 25);
            this.FreeSpaceLabel.TabIndex = 13;
            this.FreeSpaceLabel.Text = "Available Drive Space:";
            // 
            // UsedSpaceLabel
            // 
            this.UsedSpaceLabel.AutoSize = true;
            this.UsedSpaceLabel.Location = new System.Drawing.Point(40, 294);
            this.UsedSpaceLabel.Name = "UsedSpaceLabel";
            this.UsedSpaceLabel.Size = new System.Drawing.Size(185, 25);
            this.UsedSpaceLabel.TabIndex = 13;
            this.UsedSpaceLabel.Text = "Used Drive Space";
            // 
            // BackupJobSizeLabel
            // 
            this.BackupJobSizeLabel.AutoSize = true;
            this.BackupJobSizeLabel.Location = new System.Drawing.Point(40, 234);
            this.BackupJobSizeLabel.Name = "BackupJobSizeLabel";
            this.BackupJobSizeLabel.Size = new System.Drawing.Size(60, 25);
            this.BackupJobSizeLabel.TabIndex = 13;
            this.BackupJobSizeLabel.Text = "Size:";
            // 
            // BackupJobDriveLocLabel
            // 
            this.BackupJobDriveLocLabel.AutoSize = true;
            this.BackupJobDriveLocLabel.Location = new System.Drawing.Point(40, 174);
            this.BackupJobDriveLocLabel.Name = "BackupJobDriveLocLabel";
            this.BackupJobDriveLocLabel.Size = new System.Drawing.Size(156, 25);
            this.BackupJobDriveLocLabel.TabIndex = 13;
            this.BackupJobDriveLocLabel.Text = "Drive Location:";
            // 
            // BackupJobNameLabel
            // 
            this.BackupJobNameLabel.AutoSize = true;
            this.BackupJobNameLabel.Location = new System.Drawing.Point(40, 115);
            this.BackupJobNameLabel.Name = "BackupJobNameLabel";
            this.BackupJobNameLabel.Size = new System.Drawing.Size(74, 25);
            this.BackupJobNameLabel.TabIndex = 13;
            this.BackupJobNameLabel.Text = "Name:";
            // 
            // FilePathBrowseButton
            // 
            this.FilePathBrowseButton.Location = new System.Drawing.Point(1045, 15);
            this.FilePathBrowseButton.Name = "FilePathBrowseButton";
            this.FilePathBrowseButton.Size = new System.Drawing.Size(150, 42);
            this.FilePathBrowseButton.TabIndex = 12;
            this.FilePathBrowseButton.Text = "Browse";
            this.FilePathBrowseButton.UseVisualStyleBackColor = true;
            this.FilePathBrowseButton.Click += new System.EventHandler(this.FilePathBrowseButton_Click);
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Location = new System.Drawing.Point(40, 24);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(131, 25);
            this.FilePathLabel.TabIndex = 11;
            this.FilePathLabel.Text = "Backup File:";
            // 
            // FilePathTextBox
            // 
            this.FilePathTextBox.Location = new System.Drawing.Point(177, 21);
            this.FilePathTextBox.Name = "FilePathTextBox";
            this.FilePathTextBox.ReadOnly = true;
            this.FilePathTextBox.Size = new System.Drawing.Size(846, 31);
            this.FilePathTextBox.TabIndex = 10;
            // 
            // ErrorAnalysisTab
            // 
            this.ErrorAnalysisTab.Controls.Add(this.EndDateTimePickerLabel);
            this.ErrorAnalysisTab.Controls.Add(this.StartDateTimePickerLabel);
            this.ErrorAnalysisTab.Controls.Add(this.EndDateTimePicker);
            this.ErrorAnalysisTab.Controls.Add(this.StartDateTimePicker);
            this.ErrorAnalysisTab.Controls.Add(this.SelectAllAlertTypesCheckBox);
            this.ErrorAnalysisTab.Controls.Add(this.AlertTypeCheckedListBoxLabel);
            this.ErrorAnalysisTab.Controls.Add(this.AlertTypesCheckedListBox);
            this.ErrorAnalysisTab.Controls.Add(this.SettingsProgressBar3);
            this.ErrorAnalysisTab.Controls.Add(this.HelpButton3);
            this.ErrorAnalysisTab.Controls.Add(this.GenerateButton3);
            this.ErrorAnalysisTab.Controls.Add(this.FolderPathBrowseButton3);
            this.ErrorAnalysisTab.Controls.Add(this.ProgressBarLabel3);
            this.ErrorAnalysisTab.Controls.Add(this.FolderPathLabel3);
            this.ErrorAnalysisTab.Controls.Add(this.FolderPathTextBox3);
            this.ErrorAnalysisTab.Controls.Add(this.SelectAllBackupJobsCheckBox);
            this.ErrorAnalysisTab.Controls.Add(this.SelectAllStorageDevicesCheckBox);
            this.ErrorAnalysisTab.Controls.Add(this.BackupJobCheckedListBoxLabel);
            this.ErrorAnalysisTab.Controls.Add(this.StorageDevicesCheckedListBoxLabel);
            this.ErrorAnalysisTab.Controls.Add(this.BackupJobsCheckedListBox);
            this.ErrorAnalysisTab.Controls.Add(this.StorageDevicesCheckedListBox);
            this.ErrorAnalysisTab.Location = new System.Drawing.Point(8, 39);
            this.ErrorAnalysisTab.Name = "ErrorAnalysisTab";
            this.ErrorAnalysisTab.Size = new System.Drawing.Size(1238, 833);
            this.ErrorAnalysisTab.TabIndex = 2;
            this.ErrorAnalysisTab.Text = "Error Analysis";
            this.ErrorAnalysisTab.UseVisualStyleBackColor = true;
            // 
            // EndDateTimePickerLabel
            // 
            this.EndDateTimePickerLabel.AutoSize = true;
            this.EndDateTimePickerLabel.Location = new System.Drawing.Point(840, 465);
            this.EndDateTimePickerLabel.Name = "EndDateTimePickerLabel";
            this.EndDateTimePickerLabel.Size = new System.Drawing.Size(107, 25);
            this.EndDateTimePickerLabel.TabIndex = 36;
            this.EndDateTimePickerLabel.Text = "End Date:";
            // 
            // StartDateTimePickerLabel
            // 
            this.StartDateTimePickerLabel.AutoSize = true;
            this.StartDateTimePickerLabel.Location = new System.Drawing.Point(40, 465);
            this.StartDateTimePickerLabel.Name = "StartDateTimePickerLabel";
            this.StartDateTimePickerLabel.Size = new System.Drawing.Size(114, 25);
            this.StartDateTimePickerLabel.TabIndex = 35;
            this.StartDateTimePickerLabel.Text = "Start Date:";
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            this.EndDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndDateTimePicker.Location = new System.Drawing.Point(845, 493);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(350, 31);
            this.EndDateTimePicker.TabIndex = 34;
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.CustomFormat = "MM/dd/yyyy hh:mm:ss tt";
            this.StartDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDateTimePicker.Location = new System.Drawing.Point(45, 493);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(350, 31);
            this.StartDateTimePicker.TabIndex = 33;
            // 
            // SelectAllAlertTypesCheckBox
            // 
            this.SelectAllAlertTypesCheckBox.AutoSize = true;
            this.SelectAllAlertTypesCheckBox.Location = new System.Drawing.Point(845, 85);
            this.SelectAllAlertTypesCheckBox.Name = "SelectAllAlertTypesCheckBox";
            this.SelectAllAlertTypesCheckBox.Size = new System.Drawing.Size(134, 29);
            this.SelectAllAlertTypesCheckBox.TabIndex = 32;
            this.SelectAllAlertTypesCheckBox.Text = "Select All";
            this.SelectAllAlertTypesCheckBox.UseVisualStyleBackColor = true;
            this.SelectAllAlertTypesCheckBox.CheckedChanged += new System.EventHandler(this.SelectAllAlertTypesCheckBox_CheckedChanged);
            // 
            // AlertTypeCheckedListBoxLabel
            // 
            this.AlertTypeCheckedListBoxLabel.AutoSize = true;
            this.AlertTypeCheckedListBoxLabel.Location = new System.Drawing.Point(840, 24);
            this.AlertTypeCheckedListBoxLabel.Name = "AlertTypeCheckedListBoxLabel";
            this.AlertTypeCheckedListBoxLabel.Size = new System.Drawing.Size(116, 25);
            this.AlertTypeCheckedListBoxLabel.TabIndex = 31;
            this.AlertTypeCheckedListBoxLabel.Text = "Alert Type:";
            // 
            // AlertTypesCheckedListBox
            // 
            this.AlertTypesCheckedListBox.FormattingEnabled = true;
            this.AlertTypesCheckedListBox.HorizontalScrollbar = true;
            this.AlertTypesCheckedListBox.Items.AddRange(new object[] {
            "General Information",
            "Job Start",
            "Job Completed with Exceptions",
            "Media Information",
            "Media Warning",
            "Media Error",
            "Media Intervention",
            "Storage Information",
            "Storage Warning",
            "Storage Error",
            "Storage Intervention",
            "Tape Alert Information",
            "Tape Alert Warning",
            "Tape Alert Error",
            "Job Success",
            "Job Failure",
            "Job Cancelation"});
            this.AlertTypesCheckedListBox.Location = new System.Drawing.Point(845, 120);
            this.AlertTypesCheckedListBox.Name = "AlertTypesCheckedListBox";
            this.AlertTypesCheckedListBox.Size = new System.Drawing.Size(350, 342);
            this.AlertTypesCheckedListBox.TabIndex = 30;
            // 
            // SettingsProgressBar3
            // 
            this.SettingsProgressBar3.Location = new System.Drawing.Point(150, 757);
            this.SettingsProgressBar3.Name = "SettingsProgressBar3";
            this.SettingsProgressBar3.Size = new System.Drawing.Size(703, 42);
            this.SettingsProgressBar3.TabIndex = 28;
            // 
            // HelpButton3
            // 
            this.HelpButton3.Location = new System.Drawing.Point(1045, 757);
            this.HelpButton3.Name = "HelpButton3";
            this.HelpButton3.Size = new System.Drawing.Size(150, 42);
            this.HelpButton3.TabIndex = 26;
            this.HelpButton3.Text = "Help";
            this.HelpButton3.UseVisualStyleBackColor = true;
            // 
            // GenerateButton3
            // 
            this.GenerateButton3.Location = new System.Drawing.Point(879, 757);
            this.GenerateButton3.Name = "GenerateButton3";
            this.GenerateButton3.Size = new System.Drawing.Size(150, 42);
            this.GenerateButton3.TabIndex = 27;
            this.GenerateButton3.Text = "Generate";
            this.GenerateButton3.UseVisualStyleBackColor = true;
            // 
            // FolderPathBrowseButton3
            // 
            this.FolderPathBrowseButton3.Location = new System.Drawing.Point(1045, 687);
            this.FolderPathBrowseButton3.Name = "FolderPathBrowseButton3";
            this.FolderPathBrowseButton3.Size = new System.Drawing.Size(150, 42);
            this.FolderPathBrowseButton3.TabIndex = 25;
            this.FolderPathBrowseButton3.Text = "Browse";
            this.FolderPathBrowseButton3.UseVisualStyleBackColor = true;
            this.FolderPathBrowseButton3.Click += new System.EventHandler(this.FolderPathBrowseButton3_Click);
            // 
            // ProgressBarLabel3
            // 
            this.ProgressBarLabel3.AutoSize = true;
            this.ProgressBarLabel3.Location = new System.Drawing.Point(40, 766);
            this.ProgressBarLabel3.Name = "ProgressBarLabel3";
            this.ProgressBarLabel3.Size = new System.Drawing.Size(104, 25);
            this.ProgressBarLabel3.TabIndex = 23;
            this.ProgressBarLabel3.Text = "Progress:";
            // 
            // FolderPathLabel3
            // 
            this.FolderPathLabel3.AutoSize = true;
            this.FolderPathLabel3.Location = new System.Drawing.Point(40, 696);
            this.FolderPathLabel3.Name = "FolderPathLabel3";
            this.FolderPathLabel3.Size = new System.Drawing.Size(408, 25);
            this.FolderPathLabel3.TabIndex = 24;
            this.FolderPathLabel3.Text = "Destination Folder For Generated Report:";
            // 
            // FolderPathTextBox3
            // 
            this.FolderPathTextBox3.Location = new System.Drawing.Point(454, 688);
            this.FolderPathTextBox3.Name = "FolderPathTextBox3";
            this.FolderPathTextBox3.ReadOnly = true;
            this.FolderPathTextBox3.Size = new System.Drawing.Size(575, 31);
            this.FolderPathTextBox3.TabIndex = 22;
            // 
            // SelectAllBackupJobsCheckBox
            // 
            this.SelectAllBackupJobsCheckBox.AutoSize = true;
            this.SelectAllBackupJobsCheckBox.Location = new System.Drawing.Point(445, 85);
            this.SelectAllBackupJobsCheckBox.Name = "SelectAllBackupJobsCheckBox";
            this.SelectAllBackupJobsCheckBox.Size = new System.Drawing.Size(134, 29);
            this.SelectAllBackupJobsCheckBox.TabIndex = 3;
            this.SelectAllBackupJobsCheckBox.Text = "Select All";
            this.SelectAllBackupJobsCheckBox.UseVisualStyleBackColor = true;
            this.SelectAllBackupJobsCheckBox.CheckedChanged += new System.EventHandler(this.SelectAllBackupJobsCheckBox_CheckedChanged);
            // 
            // SelectAllStorageDevicesCheckBox
            // 
            this.SelectAllStorageDevicesCheckBox.AutoSize = true;
            this.SelectAllStorageDevicesCheckBox.Location = new System.Drawing.Point(45, 85);
            this.SelectAllStorageDevicesCheckBox.Name = "SelectAllStorageDevicesCheckBox";
            this.SelectAllStorageDevicesCheckBox.Size = new System.Drawing.Size(134, 29);
            this.SelectAllStorageDevicesCheckBox.TabIndex = 3;
            this.SelectAllStorageDevicesCheckBox.Text = "Select All";
            this.SelectAllStorageDevicesCheckBox.UseVisualStyleBackColor = true;
            this.SelectAllStorageDevicesCheckBox.CheckedChanged += new System.EventHandler(this.SelectAllStorageDevicesCheckBox_CheckedChanged);
            // 
            // BackupJobCheckedListBoxLabel
            // 
            this.BackupJobCheckedListBoxLabel.AutoSize = true;
            this.BackupJobCheckedListBoxLabel.Location = new System.Drawing.Point(440, 24);
            this.BackupJobCheckedListBoxLabel.Name = "BackupJobCheckedListBoxLabel";
            this.BackupJobCheckedListBoxLabel.Size = new System.Drawing.Size(131, 25);
            this.BackupJobCheckedListBoxLabel.TabIndex = 1;
            this.BackupJobCheckedListBoxLabel.Text = "Backup Job:";
            // 
            // StorageDevicesCheckedListBoxLabel
            // 
            this.StorageDevicesCheckedListBoxLabel.AutoSize = true;
            this.StorageDevicesCheckedListBoxLabel.Location = new System.Drawing.Point(40, 24);
            this.StorageDevicesCheckedListBoxLabel.Name = "StorageDevicesCheckedListBoxLabel";
            this.StorageDevicesCheckedListBoxLabel.Size = new System.Drawing.Size(165, 25);
            this.StorageDevicesCheckedListBoxLabel.TabIndex = 1;
            this.StorageDevicesCheckedListBoxLabel.Text = "Storage Device:";
            // 
            // BackupJobsCheckedListBox
            // 
            this.BackupJobsCheckedListBox.FormattingEnabled = true;
            this.BackupJobsCheckedListBox.HorizontalScrollbar = true;
            this.BackupJobsCheckedListBox.Items.AddRange(new object[] {
            "WIN-JSJ00524NOP Backup 00001-Full",
            "WIN-JSJ00524NOP Backup 00001-Incremental",
            "WIN-JSJ00524NOP Backup 00002-Full",
            "WIN-JSJ00524NOP Backup 00002-Incremental"});
            this.BackupJobsCheckedListBox.Location = new System.Drawing.Point(445, 120);
            this.BackupJobsCheckedListBox.Name = "BackupJobsCheckedListBox";
            this.BackupJobsCheckedListBox.Size = new System.Drawing.Size(350, 342);
            this.BackupJobsCheckedListBox.TabIndex = 0;
            // 
            // StorageDevicesCheckedListBox
            // 
            this.StorageDevicesCheckedListBox.FormattingEnabled = true;
            this.StorageDevicesCheckedListBox.Location = new System.Drawing.Point(45, 120);
            this.StorageDevicesCheckedListBox.Name = "StorageDevicesCheckedListBox";
            this.StorageDevicesCheckedListBox.Size = new System.Drawing.Size(350, 342);
            this.StorageDevicesCheckedListBox.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1254, 880);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainForm";
            this.Text = "Checkup Exec";
            this.tabControl1.ResumeLayout(false);
            this.ConfigAnalysisTab.ResumeLayout(false);
            this.ConfigAnalysisTab.PerformLayout();
            this.BackupJobStorageAnalysisTab.ResumeLayout(false);
            this.BackupJobStorageAnalysisTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DriveUsageChart)).EndInit();
            this.ErrorAnalysisTab.ResumeLayout(false);
            this.ErrorAnalysisTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ConfigAnalysisTab;
        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.Button FolderPathBrowseButton;
        private System.Windows.Forms.Label FolderPathLabel;
        private System.Windows.Forms.TextBox FolderPathTextBox;
        private System.Windows.Forms.Label GlobalSettingsLabel;
        private System.Windows.Forms.TabPage BackupJobStorageAnalysisTab;
        private System.Windows.Forms.TabPage ErrorAnalysisTab;
        private System.Windows.Forms.TextBox GlobalSettingsTextBox;
        private System.Windows.Forms.ProgressBar SettingsProgressBar;
        private System.Windows.Forms.Label ProgressBarLabel;
        private System.Windows.Forms.Label ElapsedTimeLabel;
        private System.Windows.Forms.Label EndTimeLabel;
        private System.Windows.Forms.Label StartTimeLabel;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Label FreeSpaceLabel;
        private System.Windows.Forms.Label UsedSpaceLabel;
        private System.Windows.Forms.Label BackupJobDriveLocLabel;
        private System.Windows.Forms.Label BackupJobNameLabel;
        private System.Windows.Forms.Button FilePathBrowseButton;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.TextBox FilePathTextBox;
        private System.Windows.Forms.Label BackupJobSizeLabel;
        private System.Windows.Forms.Label StorageDevicesCheckedListBoxLabel;
        private System.Windows.Forms.CheckedListBox BackupJobsCheckedListBox;
        private System.Windows.Forms.CheckedListBox StorageDevicesCheckedListBox;
        private System.Windows.Forms.Label BackupJobCheckedListBoxLabel;
        private System.Windows.Forms.CheckBox SelectAllBackupJobsCheckBox;
        private System.Windows.Forms.CheckBox SelectAllStorageDevicesCheckBox;
        private System.Windows.Forms.DataVisualization.Charting.Chart DriveUsageChart;
        private System.Windows.Forms.ProgressBar SettingsProgressBar2;
        private System.Windows.Forms.Button HelpButton2;
        private System.Windows.Forms.Button GenerateButton2;
        private System.Windows.Forms.Button FolderPathBrowseButton2;
        private System.Windows.Forms.Label ProgressBarLabel2;
        private System.Windows.Forms.Label FolderPathLabel2;
        private System.Windows.Forms.TextBox FolderPathTextBox2;
        private System.Windows.Forms.Label DiskInfoLabel;
        private System.Windows.Forms.ProgressBar SettingsProgressBar3;
        private System.Windows.Forms.Button HelpButton3;
        private System.Windows.Forms.Button GenerateButton3;
        private System.Windows.Forms.Button FolderPathBrowseButton3;
        private System.Windows.Forms.Label ProgressBarLabel3;
        private System.Windows.Forms.Label FolderPathLabel3;
        private System.Windows.Forms.TextBox FolderPathTextBox3;
        private System.Windows.Forms.CheckBox SelectAllAlertTypesCheckBox;
        private System.Windows.Forms.Label AlertTypeCheckedListBoxLabel;
        private System.Windows.Forms.CheckedListBox AlertTypesCheckedListBox;
        private System.Windows.Forms.Label EndDateTimePickerLabel;
        private System.Windows.Forms.Label StartDateTimePickerLabel;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
    }
}

