using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckupExecApp
{
    class Helpers
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

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

        // Populates the StorageDevicesCheckedListBox with the list of storage devices
        public static void LoadStorageDevicesCheckedListBox(CheckedListBox StorageDevicesCheckedListBox)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach(DriveInfo drive in drives)
            {
                StorageDevicesCheckedListBox.Items.Add(drive.Name);
            }
        }

        // Populates the BackupJobsCheckedListBox with the list of Backup Jobs for the selected storage device
        public static void LoadBackupJobsCheckedListBox()
        {

        }

        // Populates the AlertTypesCheckedListBox with the list of Backup Jobs for the selected storage device
        public static void LoadAlertTypesCheckedListBox()
        {

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
