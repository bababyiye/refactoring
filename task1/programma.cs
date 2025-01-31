using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            if (ValidateInputs())
            {
                worker.RunWorkerAsync(new WorkerArgs
                {
                    FilePath = txtFilePath.Text,
                    Key = txtKey.Text,
                    IsEncryption = true
                });
                StartOperation("Encrypting...");
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (ValidateInputs())
            {
                worker.RunWorkerAsync(new WorkerArgs
                {
                    FilePath = txtFilePath.Text,
                    Key = txtKey.Text,
                    IsEncryption = false
                });
                StartOperation("Decrypting...");
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var args = (WorkerArgs)e.Argument;
            string outputFilePath = args.FilePath + (args.IsEncryption ? ".enc" : ".dec");

            using (FileStream inputFileStream = new FileStream(args.FilePath, FileMode.Open, FileAccess.Read))
            using (FileStream outputFileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                var key = Encoding.UTF8.GetBytes(args.Key.PadRight(32).Substring(0, 32));
                var iv = Encoding.UTF8.GetBytes(args.Key.PadRight(16).Substring(0, 16));

                using (Aes aes = Aes.Create())
                {
                    aes.Key = key;
                    aes.IV = iv;

                    CryptoStream cryptoStream;
                    if (args.IsEncryption)
                    {
                        cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                    }
                    else
                    {
                        cryptoStream = new CryptoStream(outputFileStream, aes.CreateDecryptor(), CryptoStreamMode.Write);
                    }

                    byte[] buffer = new byte[8192];
                    int bytesRead;
                    long totalBytes = inputFileStream.Length;
                    long processedBytes = 0;

                    while ((bytesRead = inputFileStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        cryptoStream.Write(buffer, 0, bytesRead);
                        processedBytes += bytesRead;
                        int progress = (int)((double)processedBytes / totalBytes * 100);
                        worker.ReportProgress(progress);
                    }
                }
            }

            e.Result = new WorkerResult
            {
                FilePath = outputFilePath,
                FileSize = new FileInfo(outputFilePath).Length,
                ElapsedTime = stopwatch.Elapsed
            };
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            stopwatch.Stop();
            progress.Text = "Operation completed.";
            var result = (WorkerResult)e.Result;
            MessageBox.Show($"File: {Path.GetFileName(result.FilePath)}\n" +
                            $"Size: {result.FileSize} bytes\n" +
                            $"Time: {result.ElapsedTime}", "Operation Info");
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

        private void StartOperation(string status)
        {
            progress.Text = status;
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
