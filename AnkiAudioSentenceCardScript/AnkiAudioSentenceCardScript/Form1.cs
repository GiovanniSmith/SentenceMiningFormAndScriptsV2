using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;


namespace AnkiAudioSentenceCardScript
{
    public partial class Form1 : Form
    {
        private FileSystemWatcher fileWatcher;
        public static string send = "Send, ";
        
        public Form1()// ChatGPT wrote everything in this method except for InitializeComponent()
        {
            InitializeComponent();
            fileWatcher = new FileSystemWatcher(Path.GetDirectoryName(getFilePathForCurrentVariable("IsRecordingAudio")), Path.GetFileName(getFilePathForCurrentVariable("IsRecordingAudio")));
            fileWatcher.NotifyFilter = NotifyFilters.LastWrite;
            fileWatcher.Changed += fileWatcher_Changed;
            fileWatcher.EnableRaisingEvents = true;
            UpdateLabel();
        }
        private void fileWatcher_Changed(object sender, FileSystemEventArgs e)// ChatGPT wrote this
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
                this.Invoke(new Action(UpdateLabel));
        }

        private void UpdateLabel()// ChatGPT wrote this
        {
            string fileContent = File.ReadAllText(getFilePathForCurrentVariable("IsRecordingAudio"));

            if (fileContent.Trim() == "0")
                txtIsRecording.Text = "False";
            else if (fileContent.Trim() == "1")
                txtIsRecording.Text = "True";
            else
                txtIsRecording.Text = "Error";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load();
            // https://stackoverflow.com/questions/1339524/how-do-i-add-a-tooltip-to-a-control#1339533
            ToolTip toolTip = new ToolTip();
            setToolTipProperties(toolTip);
            setToolTipTexts(toolTip);
        }
        private void setToolTipProperties(ToolTip toolTip)
        {
            toolTip.AutoPopDelay = 30000;
            toolTip.InitialDelay = 250;
            toolTip.ReshowDelay = 250;
            toolTip.ShowAlways = true;
        }
        private void setToolTipTexts(ToolTip toolTip)
        {
            toolTip.SetToolTip(this.linkLblAutoHotkeyWebsite, "https://www.autohotkey.com/docs/v2/Hotkeys.htm#Symbols");
            toolTip.SetToolTip(this.grpPrimaryScriptHotkeys, "Hotkeys that the user presses");
            toolTip.SetToolTip(this.grpSecondaryScriptHotkeys, "Hotkeys that the script presses");
            toolTip.SetToolTip(this.grpDelays, "Programs need delays between actions so that they function correctly");
            toolTip.SetToolTip(this.lblIsRecording, "Pressing the hotkey for \"Take screenshot and record audio with ShareX\" should toggle this");
            toolTip.SetToolTip(this.txtIsRecording, "Pressing the hotkey for \"Take screenshot and record audio with ShareX\" should toggle this");
            toolTip.SetToolTip(this.btnResetIsRecording, "If you are not recording audio with ShareX and \"Is ShareX currently recording audio?\" is \"True,\"\n" +
                "then press this button.");
            toolTip.SetToolTip(this.radWindowsClipboard, "Turn on \"Clipboard history\" in the Windows settings for this to work." +
                "\nMicrosoft sets this hotkey to Windows key + V by default.\nHowever, it could change in the future, which is why you can still edit the hotkey yourself if needed.");
        }

        private void load()
        {
            fillTextBoxesWithInfoFromFiles();
            selectRadioButtonsFromInfoFromFiles();

            if (doesTextInFileEqualValue(getFilePathForCurrentVariable("ClipboardSoftware"), "ditto"))
            {
                radDitto.Checked = true;
                txtActivateClipboardSoftware.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("ActivateDitto"));
            } else
            {
                radWindowsClipboard.Checked = true;
                txtActivateClipboardSoftware.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("ActivateWindowsClipboard"));
            }

            btnSave.Enabled = false;
        }
        private void fillTextBoxesWithInfoFromFiles()
        {
            txtTakeScreenshotAndRecordAudioWithShareX.Text = readFile(getFilePathForCurrentVariable("HotkeyForTakeScreenshotAndRecordAudioWithShareX"));
            txtPasteImageAndAudioWithClipboardSoftware.Text = readFile(getFilePathForCurrentVariable("HotkeyForPasteImageAndAudioWithClipboardSoftware"));
            txtPasteMultipleImagesWithClipboardSoftware.Text = readFile(getFilePathForCurrentVariable("HotkeyForPasteMultipleImagesWithClipboardSoftware"));
            txtGeneralDelay.Text = readFile(getFilePathForCurrentVariable("DelayGeneral"));
            txtDelayForRecordingToStart.Text = readFile(getFilePathForCurrentVariable("DelayForRecordingToStart"));
            txtTakeScreenshot.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("TakeScreenshotWithShareX"));
            txtToggleRecordAudio.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("ToggleRecordAudioWithShareX"));
        }
        
        private void selectRadioButtonsFromInfoFromFiles()
        {
            if (doesTextInFileEqualValue(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "0"))
                radBeginning.Checked = true;
            else if (doesTextInFileEqualValue(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "1"))
                radEnd.Checked = true;
            else if (doesTextInFileEqualValue(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "2"))
                radNoScreenshot.Checked = true;

            if (doesTextInFileEqualValue(getFilePathForCurrentVariable("PlayPauseVideo"), "MouseClick, left"))
                radLeftMouse.Checked = true;
            else if (doesTextInFileEqualValue(getFilePathForCurrentVariable("PlayPauseVideo"), "Send, {Space}"))
                radSpaceBar.Checked = true;
        }

        private string readFile(string filePath)
        {
            return File.ReadAllText(filePath);
        }
        private string readOnlyTheHotkeyOfDotAHKFile(string filePath)
        {
            return readFile(filePath).Substring(send.Length);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            save();
        }
        private void save()
        {
            writeTextBoxInformationIntoFiles();
            writeRadioButtonSelectionsIntoFiles();
            openAllScripts();
            btnSave.Enabled = false;
        }
        private void writeTextBoxInformationIntoFiles()
        {
            updateFile(getFilePathForCurrentVariable("HotkeyForTakeScreenshotAndRecordAudioWithShareX"));
            updateFile(getFilePathForCurrentVariable("HotkeyForPasteImageAndAudioWithClipboardSoftware"));
            updateFile(getFilePathForCurrentVariable("HotkeyForPasteMultipleImagesWithClipboardSoftware"));
            updateFile(getFilePathForCurrentVariable("TakeScreenshotWithShareX"));
            updateFile(getFilePathForCurrentVariable("ToggleRecordAudioWithShareX"));
            updateFile(getFilePathForCurrentVariable("DelayGeneral"));
            updateFile(getFilePathForCurrentVariable("DelayForRecordingToStart"));
        }
        
        private void writeRadioButtonSelectionsIntoFiles()
        {
            updateFile(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"));
            updateFile(getFilePathForCurrentVariable("PlayPauseVideo"));

            if (radDitto.Checked)
                updateFile(getFilePathForCurrentVariable("ActivateDitto"));
            else if (radWindowsClipboard.Checked)
                updateFile(getFilePathForCurrentVariable("ActivateWindowsClipboard"));

            updateFile(getFilePathForCurrentVariable("ClipboardSoftware"));
        }
        private void updateFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (filePath.Equals(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX")))
                {
                    if (radBeginning.Checked)
                        replaceContentsOfFileWithString(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "0");
                    else if (radEnd.Checked)
                        replaceContentsOfFileWithString(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "1");
                    else if (radNoScreenshot.Checked)
                        replaceContentsOfFileWithString(getFilePathForCurrentVariable("WhenToTakeScreenshotWithShareX"), "2");
                }
                else if (filePath.Equals(getFilePathForCurrentVariable("PlayPauseVideo")))
                {
                    if (radLeftMouse.Checked)
                        replaceContentsOfFileWithString(getFilePathForCurrentVariable("PlayPauseVideo"), "MouseClick, left");
                    else if (radSpaceBar.Checked)
                        replaceContentsOfFileWithString(getFilePathForCurrentVariable("PlayPauseVideo"), "Send, {Space}");
                }
                else if (filePath.Equals(getFilePathForCurrentVariable("HotkeyForTakeScreenshotAndRecordAudioWithShareX")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("HotkeyForTakeScreenshotAndRecordAudioWithShareX"), txtTakeScreenshotAndRecordAudioWithShareX.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("HotkeyForPasteImageAndAudioWithClipboardSoftware")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("HotkeyForPasteImageAndAudioWithClipboardSoftware"), txtPasteImageAndAudioWithClipboardSoftware.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("HotkeyForPasteMultipleImagesWithClipboardSoftware")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("HotkeyForPasteMultipleImagesWithClipboardSoftware"), txtPasteMultipleImagesWithClipboardSoftware.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("TakeScreenshotWithShareX")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("TakeScreenshotWithShareX"), send + txtTakeScreenshot.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("ToggleRecordAudioWithShareX")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("ToggleRecordAudioWithShareX"), send + txtToggleRecordAudio.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("DelayGeneral")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("DelayGeneral"), txtGeneralDelay.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("DelayForRecordingToStart")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("DelayForRecordingToStart"), txtDelayForRecordingToStart.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("ActivateDitto")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("ActivateDitto"), send + txtActivateClipboardSoftware.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("ClipboardSoftware")) && radDitto.Checked)
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("ClipboardSoftware"), "ditto");

                else if (filePath.Equals(getFilePathForCurrentVariable("ActivateWindowsClipboard")))
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("ActivateWindowsClipboard"), send + txtActivateClipboardSoftware.Text);

                else if (filePath.Equals(getFilePathForCurrentVariable("ClipboardSoftware")) && radWindowsClipboard.Checked)
                    replaceContentsOfFileWithString(getFilePathForCurrentVariable("ClipboardSoftware"), "windowsClipboard");
            } else
            {
                Console.WriteLine("File path of " + filePath + " does not exist");
            }
        }

        public void startAutohotkeyScript(String s)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C \"" + s + "\"";
            process.StartInfo = startInfo;
            process.Start();
        }
        public static void replaceContentsOfFileWithString(string address, string text)
        {
            File.WriteAllText(address, String.Empty);
            File.WriteAllText(address, text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Form form = new ResetToDefaultConfirmation();
            form.ShowDialog();
            if (ResetToDefaultConfirmation.resetWasClicked)
            {
                load();
                save();
            }
        }
        private void btnResetIsRecording_Click(object sender, EventArgs e)
        {
            Form form = new ResetIsRecording();
            form.ShowDialog();
            if (ResetIsRecording.resetForIsRecordingWasClicked)
            {
                load();
                save();
            }
        }

        private void txtTakeScreenshotAndRecordAudioWithShareX_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtPasteImageAndAudioWithClipboardSoftware_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtPasteMultipleImagesWithClipboardSoftware_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtTakeScreenshot_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtToggleRecordAudio_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void radBeginning_CheckedChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void radEnd_CheckedChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void radNoScreenshot_CheckedChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtActivateClipboardSoftware_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void radSpaceBar_CheckedChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void radLeftMouse_CheckedChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtGeneralDelay_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void txtDelayForRecordingToStart_TextChanged(object sender, EventArgs e) { btnSave.Enabled = true; }
        private void linkLblAutoHotkeyWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.autohotkey.com/docs/v2/Hotkeys.htm#Symbols");
            Process.Start(sInfo);
        }

        private void openAllScripts()
        {
            startAutohotkeyScript(@"TakeScreenshotAndRecordAudioWithShareX.ahk");
            startAutohotkeyScript(@"PasteImageAndAudioWithClipboardSoftware.ahk");
            startAutohotkeyScript(@"PasteMultipleImagesWithClipboardSoftware.ahk");
        }

        public static void closeAllScriptsWindows10()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C taskkill /im \"autohotkey.exe\"";
            process.StartInfo = startInfo;
            process.Start();
        }

        public static void closeAllScriptsWindows11()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C taskkill /im \"autohotkeyu64.exe\"";
            process.StartInfo = startInfo;
            process.Start();
        }
        public static void closeAllScripts()
        {
            
        }
        private void btnOpenAllScripts_Click(object sender, EventArgs e)
        {
            openAllScripts();
        }

        private void btnCloseAllScripts_Click(object sender, EventArgs e)
        {
            closeAllScriptsWindows10();
            closeAllScriptsWindows11();
        }

        private void radDitto_CheckedChanged(object sender, EventArgs e)
        {
            if (radDitto.Checked)
            {
                txtActivateClipboardSoftware.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("ActivateDitto"));
                radEnd.Enabled = true;
            }
        }

        private void radWindowsClipboard_CheckedChanged(object sender, EventArgs e)
        {
            if (radWindowsClipboard.Checked)
            {
                txtActivateClipboardSoftware.Text = readOnlyTheHotkeyOfDotAHKFile(getFilePathForCurrentVariable("ActivateWindowsClipboard"));
                disableTakeScreenshotAtBeginning();
            }
        }

        private void disableTakeScreenshotAtBeginning()
        {
            if (radEnd.Checked)
                radBeginning.Checked = true;

            radEnd.Enabled = false;
        }

        private Boolean doesTextInFileEqualValue(string textFilePath, string value)
        {
            int textFilePathLength = File.ReadAllText(textFilePath).Length;
            return File.ReadAllText(textFilePath).Substring(0, textFilePathLength).Equals(value);
        }

        public static void replaceContentsOfFileWithContentsOfAnotherFile(string filePath1, string filePath2)
        {
            replaceContentsOfFileWithString(filePath1, File.ReadAllText(filePath2) + "");
        }
        public static string getFilePathForCurrentVariable(string variable)
        {
            if (File.Exists(@"helper/" + variable + "/current.txt"))
                return @"helper/" + variable + "/current.txt";
            else
                return @"helper/" + variable + "/current.ahk";
        }
        public static string getFilePathForDefaultVariable(string variable)
        {
            if (File.Exists(@"helper/" + variable + "/default.txt"))
                return @"helper/" + variable + "/default.txt";
            else
                return @"helper/" + variable + "/default.ahk";
        }
    }
}
