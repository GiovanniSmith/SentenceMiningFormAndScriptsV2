using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnkiAudioSentenceCardScript
{
    public partial class ResetToDefaultConfirmation : Form
    {
        public static Boolean resetWasClicked;

        public ResetToDefaultConfirmation()
        {
            InitializeComponent();
        }

        private void ResetToDefaultConfirmation_Load(object sender, EventArgs e)
        {
            resetWasClicked = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetWasClicked = true;
            
            reload("ActivateDitto");
            reload("ActivateWindowsClipboard");
            reload("ClipboardSoftware");
            reload("DelayForRecordingToStart");
            reload("DelayGeneral");
            reload("HotkeyForPasteImageAndAudioWithClipboardSoftware");
            reload("HotkeyForPasteMultipleImagesWithClipboardSoftware");
            reload("HotkeyForTakeScreenshotAndRecordAudioWithShareX");
            reload("IsRecordingAudio");
            reload("PlayPauseVideo");
            reload("TakeScreenshotWithShareX");
            reload("ToggleRecordAudioWithShareX");
            reload("WhenToTakeScreenshotWithShareX");

            Form1.closeAllScripts();
            this.Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static void reload(string variable)
        {
            Form1.replaceContentsOfFileWithContentsOfAnotherFile(Form1.getFilePathForCurrentVariable(variable), Form1.getFilePathForDefaultVariable(variable));
        }
    }
}
