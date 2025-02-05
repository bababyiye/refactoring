using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace task1
{
    public partial class programma : Form
    {
        private BackgroundWorker worker = new BackgroundWorker();
        private Stopwatch stopwatch = new Stopwatch();

        public programma()
        {
            InitializeComponent();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            StartWorker(true);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            StartWorker(false);
        }

        private void StartWorker(bool isEncryption)
        {
            if (ValidateInputs())
            {
                worker.RunWorkerAsync(new WorkerArgs
                {
                    FilePath = txtFilePath.Text,
                    Key = txtKey.Text,
                    IsEncryption = isEncryption
                });
                UpdateProgressLabel(isEncryption ? "Encrypting..." : "Decrypting...");
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (WorkerArgs)e.Argument;
            var encryptor = new FileEncryptor();
            try
            {
                var result = encryptor.ProcessFile(args.FilePath, args.Key, args.IsEncryption, worker);
                e.Result = result;
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            UpdateProgressLabel("Operation completed.");
            if (e.Result is WorkerResult result)
            {
                MessageBox.Show($"File: {Path.GetFileName(result.FilePath)}\n" +
                                $"Size: {result.FileSize} bytes\n" +
                                $"Time: {result.ElapsedTime}", "Operation Info");
            }
            else
            {
                MessageBox.Show($"Error: {e.Result}", "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text) || !File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("Please select a valid file.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtKey.Text) || txtKey.Text.Length < 8)
            {
                MessageBox.Show("Key must be at least 8 characters long.");
                return false;
            }

            return true;
        }

        private void UpdateProgressLabel(string text)
        {
            progress.Text = text;
            progressBar.Value = 0;
            stopwatch.Restart();
        }
    }

    public class WorkerArgs
    {
        public string FilePath { get; set; }
        public string Key { get; set; }
        public bool IsEncryption { get; set; }
    }

    public class WorkerResult
    {
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public TimeSpan ElapsedTime { get; set; }
    }
}
