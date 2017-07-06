using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckupExec;

namespace CheckupExecApp
{
    class Helpers
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        static readonly string[] SizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        // Allows user to select a destination folder
        public static void SelectDestinationFolder(TextBox PathTextBox)
        {
            try
            {
                // Allow user to select a desired folder to place the generated report
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select a folder for the generated report";

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    PathTextBox.Text = fbd.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        // Converts bytes to the appropriate suffix (KB, MB, GB, etc.)
        public static string GetSizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + GetSizeSuffix(-value); }
            if (value == 0) { return "0.0 bytes"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }

        // Populates the StorageDevicesCheckedListBox with the list of storage devices
        public static void LoadStorageDevicesCheckedListBox(DataExtraction dataExtractionInstance, CheckedListBox StorageDevicesCheckedListBox)
        {
            // Clear the checked list box
            StorageDevicesCheckedListBox.Items.Clear();

            // Get list of storage devices
            List<string> storageDevices = dataExtractionInstance.GetStorageDeviceNames();
            foreach(string storageDevice in storageDevices)
            {
                StorageDevicesCheckedListBox.Items.Add(storageDevice);
            }
        }

        // Populates the BackupJobsCheckedListBox with the list of Backup Jobs for the selected storage device
        public static void LoadBackupJobsCheckedListBox(DataExtraction dataExtractionInstance, List<string> storageDevices, CheckedListBox BackupJobsCheckedListBox)
        {
            // Clear the checked list box
            BackupJobsCheckedListBox.Items.Clear();

            // Get list of backup jobs
            List<string> backupJobs = dataExtractionInstance.GetJobNames(storageDevices);
            foreach(string backupJob in backupJobs)
            {
                BackupJobsCheckedListBox.Items.Add(backupJob);
            }
        }

        // Populates the AlertTypesCheckedListBox with the list of Alert types
        public static void LoadAlertTypesCheckedListBox(DataExtraction dataExtractionInstance, CheckedListBox AlertTypesCheckedListBox)
        {
            // Clear the checked list box
            AlertTypesCheckedListBox.Items.Clear();

            // Get list of alert types
            List<string> alertTypes = dataExtractionInstance.GetAlertCategoryNames();
            foreach(string alertType in alertTypes)
            {
                AlertTypesCheckedListBox.Items.Add(alertType);
            }
        }

        // Populates the AlertTypesCheckedListBox with the list of Job Error statuses
        public static void LoadJobErrorTypesCheckedListBox(DataExtraction dataExtractionInstane, CheckedListBox AlertTypesCheckedListBox)
        {
            // Clear the checked list box
            AlertTypesCheckedListBox.Items.Clear();

            // Get list of job error statuses
            List<string> jobErrorStatuses = dataExtractionInstane.GetJobErrorStatuses();
            foreach(string jobErrorStatus in jobErrorStatuses)
            {
                AlertTypesCheckedListBox.Items.Add(jobErrorStatus);
            }
        }

        // Checks all the available items in the given checkedListBox
        public static void CheckAllItems(CheckedListBox checkedListBox, CheckBox selectAllCheckBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, selectAllCheckBox.Checked);
            }
        }
    }
}
