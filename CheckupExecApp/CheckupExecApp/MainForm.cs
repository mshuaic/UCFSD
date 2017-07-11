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

            // Set intial StartDateTimePickers dates
            StartDateTimePicker.Value = DateTimePicker.MinimumDateTime;
            StartDateTimePicker4.Value = DateTimePicker.MinimumDateTime;
            
            // Load Disk Analysis checked list boxes
            Helpers.LoadStorageDevicesCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox5);
            
            // Load Backup Jobs Analysis checked list boxes
            Helpers.LoadStorageDevicesCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox6);
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, dataExtractionInstance.GetStorageDeviceNames(), BackupJobsCheckedListBox6);

            // Load Alerts Analysis checked list boxes
            Helpers.LoadStorageDevicesCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox);
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, dataExtractionInstance.GetStorageDeviceNames(), BackupJobsCheckedListBox);
            Helpers.LoadAlertTypesCheckedListBox(dataExtractionInstance, AlertTypesCheckedListBox);

            // Load Job Errors Analysis checked list boxes
            Helpers.LoadStorageDevicesCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox4);
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, dataExtractionInstance.GetStorageDeviceNames(), BackupJobsCheckedListBox4);
            Helpers.LoadJobErrorTypesCheckedListBox(dataExtractionInstance, AlertTypesCheckedListBox4);
        }

        // Create new DataExtraction instance to handle creation of reports/forecasts
        CheckupExec.DataExtraction dataExtractionInstance = new CheckupExec.DataExtraction(false, null, null, null);

        #region Configuration Settings Overview/Front End Analysis
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

        // Run Frontend analysis
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure a folder path was specified
                if (FolderPathTextBox.Text != "")
                {
                    // Get storage devices
                    dataExtractionInstance.GetStorageDeviceNames();
                    // Run Frontend analysis
                    if (dataExtractionInstance.FrontEndAnalysis(FolderPathTextBox.Text))
                    {
                        log.Info("Success: Front End Analysis");

                    }
                    else
                    {
                        log.Error("Failure: Front End Analysis");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid destination folder.", "Invalid destination folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
        #endregion

        #region Disk Analysis
        private void SelectAllStorageDevicesCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(StorageDevicesCheckedListBox5, SelectAllStorageDevicesCheckBox5);
        }

        // Browse for Backup Job file button is clicked
        private void FilePathBrowseButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Allow user to select a desired Backup Job file to analyze
                OpenFileDialog ofd = new OpenFileDialog();

                // Filter the file extension to only allow user to open .bkf files
                ofd.Filter = "XML|*.xml";

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

            // Percentage of drive space used/availble
            double driveSpaceUsedPercent = Math.Round((100 * ((double)(drive.TotalSize - drive.TotalFreeSpace) / (drive.TotalSize))), 2, MidpointRounding.AwayFromZero);
            double driveSpaceAvailablePercent = Math.Round((100 * ((double)drive.TotalFreeSpace / drive.TotalSize)), 2, MidpointRounding.AwayFromZero);

            // Backup Job name
            BackupJobNameLabel.Text = "Name: " + BackupJobFile.Name;

            // Backup Job file size
            BackupJobSizeLabel.Text = "Backup Job File Size: " + Helpers.GetSizeSuffix(BackupJobFile.Length);
            
            // Backup Job location
            BackupJobDriveLocLabel.Text = "Drive Location: " + drivePath;

            // Drive used space
            UsedSpaceLabel.Text = "Used Drive Space: " + Helpers.GetSizeSuffix((drive.TotalSize - drive.TotalFreeSpace));

            // Drive free space
            FreeSpaceLabel.Text = "Available Drive Space: " + Helpers.GetSizeSuffix(drive.TotalFreeSpace);

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
            DriveUsageChart.Series["DriveUsage"].Points.Clear();
            // Drive space used
            DriveUsageChart.Series["DriveUsage"].Points.AddXY(driveSpaceUsedPercent + "% Used", driveSpaceUsedPercent);
            // Drive space available
            DriveUsageChart.Series["DriveUsage"].Points.AddXY(driveSpaceAvailablePercent + "% Free", driveSpaceAvailablePercent);
        }

        // Destination folder Browse button is clicked
        private void FolderPathBrowseButton2_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox2);
        }

        // Run Disk analysis
        private void GenerateButton2_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure a file path and report destination folder path was specified
                if(FolderPathTextBox2.Text != "" && FilePathTextBox.Text != "")
                {
                    // Get storage devices
                    dataExtractionInstance.GetStorageDeviceNames();
                    // Run Frontend analysis
                    if (dataExtractionInstance.DiskAnalysis(StorageDevicesCheckedListBox5.CheckedItems.Cast<string>().ToList(), FilePathTextBox.Text, FolderPathTextBox2.Text))
                    {
                        log.Info("Success: Disk Analysis");

                    }
                    else
                    {
                        log.Error("Failure: Disk Analysis");
                    }      
                }
                else
                {
                    MessageBox.Show("Please select a valid file and destination folder.", "Invalid file/destination folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                log.Error(ex);
            }
        }
        #endregion

        #region Backup Jobs Analysis
        private void SelectAllStorageDevicesCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(StorageDevicesCheckedListBox6, SelectAllStorageDevicesCheckBox6);
        }

        private void SelectAllBackupJobsCheckBox6_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(BackupJobsCheckedListBox6, SelectAllBackupJobsCheckBox6);
        }

        private void FolderPathBrowseButton6_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox6);
        }

        private void GenerateButton6_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure a folder path was specified
                if (FolderPathTextBox6.Text != "")
                {
                    // Get storage devices
                    dataExtractionInstance.GetStorageDeviceNames();
                    // Run Backup Jobs analysis
                    if (dataExtractionInstance.BackupJobsAnalysis(BackupJobsCheckedListBox6.CheckedItems.Cast<string>().ToList(), FolderPathTextBox6.Text))
                    {
                        log.Info("Success: Backup Jobs Analysis");
                    }
                    else
                    {
                        log.Error("Failure: Backup Jobs Analysis");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid destination folder.", "Invalid destination folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void RefreshJobsButton6_Click(object sender, EventArgs e)
        {
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox6.CheckedItems.Cast<string>().ToList(), BackupJobsCheckedListBox6);
        }
        #endregion

        #region Alerts Analysis
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

        private void GenerateButton3_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure a folder path was specified
                if (FolderPathTextBox3.Text != "")
                {
                    // Get storage devices
                    dataExtractionInstance.GetStorageDeviceNames();
                    // Run Alerts analysis
                    if(dataExtractionInstance.AlertsAnalysis(FolderPathTextBox3.Text, BackupJobsCheckedListBox.CheckedItems.Cast<string>().ToList(), AlertTypesCheckedListBox.CheckedItems.Cast<string>().ToList(), StartDateTimePicker.Value, EndDateTimePicker.Value))
                    {
                        log.Info("Success: Alerts Analysis");
                    }
                    else
                    {
                        log.Error("Failure: Alerts Analysis");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid destination folder.", "Invalid destination folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        // Reload the BackupJobsCheckedListBox based on selected storage devices
        private void RefreshJobsButton_Click(object sender, EventArgs e)
        {
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox.CheckedItems.Cast<string>().ToList(), BackupJobsCheckedListBox);
        }
        #endregion

        #region Job Error Analysis
        private void SelectAllStorageDevicesCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(StorageDevicesCheckedListBox4, SelectAllStorageDevicesCheckBox4);
        }

        private void SelectAllBackupJobsCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(BackupJobsCheckedListBox4, SelectAllBackupJobsCheckBox4);
        }

        private void SelectAllAlertTypesCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            Helpers.CheckAllItems(AlertTypesCheckedListBox4, SelectAllAlertTypesCheckBox4);
        }

        // Destination folder Browse button is clicked
        private void FolderPathBrowseButton4_Click(object sender, EventArgs e)
        {
            Helpers.SelectDestinationFolder(FolderPathTextBox4);
        }

        private void GenerateButton4_Click(object sender, EventArgs e)
        {
            try
            {
                // Make sure a folder path was specified
                if (FolderPathTextBox4.Text != "")
                {
                    // Get storage devices
                    dataExtractionInstance.GetStorageDeviceNames();
                    // Run Frontend analysis
                    if (dataExtractionInstance.JobErrorsAnalysis(FolderPathTextBox4.Text, BackupJobsCheckedListBox4.CheckedItems.Cast<string>().ToList(), AlertTypesCheckedListBox4.CheckedItems.Cast<string>().ToList(), StartDateTimePicker4.Value, EndDateTimePicker4.Value))
                    {
                        log.Info("Success: Job Errors Analysis");
                    }
                    else
                    {
                        log.Error("Failure: Job Errors Analysis");
                    }
                }
                else
                {
                    MessageBox.Show("Please select a valid destination folder.", "Invalid destination folder", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        // Reload the BackupJobsCheckedListBox based on selected storage devices
        private void RefreshJobsButton4_Click(object sender, EventArgs e)
        {
            Helpers.LoadBackupJobsCheckedListBox(dataExtractionInstance, StorageDevicesCheckedListBox4.CheckedItems.Cast<string>().ToList(), BackupJobsCheckedListBox4);
        }
        #endregion

    }   
}
