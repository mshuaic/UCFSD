using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using NLog;

namespace CheckupExecApp
{
    public partial class MainForm : Form
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public MainForm()
        {
            InitializeComponent();
            GlobalSettingsTextBox_Load();
            LoadCheckedListBoxes();
        }

        #region Configuration Analysis
        // Load global Backup Exec Settings
        private void GlobalSettingsTextBox_Load()
        {
            string powershellScript = "import-module bemcli" + "\n" + "Get-BEBackupExecSetting";
            string test = "Get-Process";
            try
            {
                // Clear the textbox
                GlobalSettingsTextBox.Clear();

                // Display the sript results in the textbox
                GlobalSettingsTextBox.Text = RunScript(powershellScript);
            }
            catch (Exception ex)
            {
                GlobalSettingsTextBox.Text = "Error getting configuration settings. Check log for details.";
                log.Error(ex.Message);
            }
        }
        
        // Runs the "Get-BEBackupExecSetting" powershell script and returns the script output (Global Backup Exec Settings).
        private string RunScript(string powershellScript)
        {
            // Create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();

            // Open the runspace
            runspace.Open();

            // Create a pipeline and feed it the script text
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(powershellScript);

            // Transform the script output objects into formatted strings
            pipeline.Commands.Add("Out-String");

            // Execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // Close the runspace
            runspace.Close();

            // Convert the script result into a single string and return the string
            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }

        // Destination folder Browse button is clicked
        private void FolderPathBrowseButton_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox);
        }
        #endregion

        #region Backup Job/Storage Analysis
        // Browse for Backup Job file button is clicked
        private void FilePathBrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Allow user to select a desired Backup Job file to analyze
                OpenFileDialog ofd = new OpenFileDialog();

                // Filter the file extension to only allow user to open .bkf files
                ofd.Filter = "BKF|*.bkf";

                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    FilePathTextBox.Text = ofd.FileName;
                    DisplayBackupJobInfo(ofd.FileName);
                }   
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        // Display the appropriate information regarding the user-selected Backup Job file
        private void DisplayBackupJobInfo(string BackupJobFilePath)
        {
            System.IO.FileInfo BackupJobFile = new System.IO.FileInfo(BackupJobFilePath);
            string drivePath = Path.GetPathRoot(BackupJobFile.FullName);
            DriveInfo drive = new DriveInfo(drivePath);

            // Backup Job name
            BackupJobNameLabel.Text = "Name: " + BackupJobFile.Name;

            // Backup Job file size
            BackupJobSizeLabel.Text = "Backup Job Size: " + BackupJobFile.Length + " (Bytes)";
            
            // Backup Job location
            BackupJobDriveLocLabel.Text = "Drive Location: " + drivePath;

            // Drive used space
            UsedSpaceLabel.Text = "Used Drive Space: " + (drive.TotalSize - drive.TotalFreeSpace) + " (Bytes)";

            // Drive free space
            FreeSpaceLabel.Text = "Available Drive Space: " + drive.TotalFreeSpace + " (Bytes)";

            // Backup Job status
            StatusLabel.Text = "Status: " + 0;

            // Backup Job start time
            StartTimeLabel.Text = "Start Time: " + BackupJobFile.CreationTime;

            // Backup Job end time
            EndTimeLabel.Text = "End Time: " + BackupJobFile.LastAccessTime;

            // Backup Job elapsed time
            ElapsedTimeLabel.Text = "Elapsed Time: " + (BackupJobFile.LastAccessTime - BackupJobFile.CreationTime);

            // Display drive usage pie chart
            DiskInfoLabel.Text = drivePath;
            // Drive space used
            DriveUsageChart.Series["DriveUsage"].Points.AddXY(100 * ((double)(drive.TotalSize - drive.TotalFreeSpace) / (drive.TotalSize)) + " %\nUsed", 100 * ((double)(drive.TotalSize - drive.TotalFreeSpace) / (drive.TotalSize)));
            // Drive space available
            DriveUsageChart.Series["DriveUsage"].Points.AddXY(100 * ((double)drive.TotalFreeSpace / drive.TotalSize) + " %\nFree", 100 * ((double)drive.TotalFreeSpace / drive.TotalSize));
        }

        // Destination folder Browse button is clicked
        private void FolderPathBrowseButton2_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox2);
        }
        #endregion

        #region Error Analysis
        // Loads the Storage Device and Backup Job checked listboxes
        private void LoadCheckedListBoxes()
        {
            // Load the Storage Device checked listbox
            Helpers.LoadStorageDevicesCheckedListBox(StorageDevicesCheckedListBox);

            // Load the Backup Job checked listbox
            Helpers.LoadBackupJobsCheckedListBox();

            // Load the Alert Types checked listbox
            Helpers.LoadAlertTypesCheckedListBox();
        }

        private void SelectAllStorageDevicesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(StorageDevicesCheckedListBox, SelectAllStorageDevicesCheckBox);
        }

        private void SelectAllBackupJobsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(BackupJobsCheckedListBox, SelectAllBackupJobsCheckBox);
        }

        private void SelectAllAlertTypesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(AlertTypesCheckedListBox, SelectAllAlertTypesCheckBox);
        }

        // Destination folder Browse button is clicked
        private void FolderPathBrowseButton3_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox3);
        }
        #endregion 
    }   
}
