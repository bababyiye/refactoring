using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace ProcessManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            LoadProcesses();
        }

        private void LoadProcesses()
        {
            dgvProcesses.Rows.Clear(); 
            foreach (var process in Process.GetProcesses().OrderBy(p => p.ProcessName))
            {
                try
                {
                    dgvProcesses.Rows.Add(process.Id, process.ProcessName, process.WorkingSet64 / 1024 + " KB",
                        process.StartTime.ToString("HH:mm:ss"), process.BasePriority, process.Threads.Count);
                }
                catch { 
                }
            }
        }

        private void btnCalculator_Click(object sender, EventArgs e) => Process.Start("calc.exe");
        private void btnWord_Click(object sender, EventArgs e) => Process.Start("winword.exe");
        private void btnGoogle_Click(object sender, EventArgs e) => Process.Start("https://www.google.com");
        private void btnEdge_Click(object sender, EventArgs e) => Process.Start("msedge"); 
        private void btnRecycleBin_Click(object sender, EventArgs e) => Process.Start("explorer.exe", "shell:RecycleBinFolder");

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedRows.Count > 0)
            {
                int processId = Convert.ToInt32(dgvProcesses.SelectedRows[0].Cells[0].Value);
                try
                {
                    Process.GetProcessById(processId).Kill();
                    MessageBox.Show("Процес завершено.");
                    LoadProcesses(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка завершення процесу: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Будь ласка, виберіть процес.");
            }
        }

        private void btnChangePriority_Click(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedRows.Count > 0 && Enum.TryParse(txtPriority.Text, out ProcessPriorityClass newPriority))
            {
                int processId = Convert.ToInt32(dgvProcesses.SelectedRows[0].Cells[0].Value);
                try
                {
                    Process process = Process.GetProcessById(processId);
                    process.PriorityClass = newPriority;
                    MessageBox.Show($"Пріоритет змінено на {newPriority}.");
                    LoadProcesses(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка зміни пріоритету: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Виберіть процес і введіть правильний пріоритет (Idle, BelowNormal, Normal, AboveNormal, High, RealTime).");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();
    }
}
