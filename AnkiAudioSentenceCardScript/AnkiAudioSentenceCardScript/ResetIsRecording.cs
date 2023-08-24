using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnkiAudioSentenceCardScript
{
    public partial class ResetIsRecording : Form
    {
        public static Boolean resetForIsRecordingWasClicked;

        public ResetIsRecording()
        {
            InitializeComponent();
        }

        private void ResetIsRecording_Load(object sender, EventArgs e)
        {
            resetForIsRecordingWasClicked = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            resetForIsRecordingWasClicked = true;
            ResetToDefaultConfirmation.reload("IsRecordingAudio");
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
