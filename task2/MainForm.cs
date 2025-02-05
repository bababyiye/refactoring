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
                    dgvProcesses.Rows.Add(
                        process.Id,
                        process.ProcessName,
                        $"{process.WorkingSet64 / 1024} KB",
                        process.StartTime.ToString("HH:mm:ss"),
                        process.BasePriority,
                        process.Threads.Count
                    );
                }
                catch (Exception) { }
            }
        }

        private void StartProcess(string processName, string arguments = "")
        {
            try
            {
                Process.Start(processName, arguments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не вдалося запустити {processName}: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCalculator_Click(object sender, EventArgs e) => StartProcess("calc.exe");
        private void btnWord_Click(object sender, EventArgs e) => StartProcess("winword.exe");
        private void btnGoogle_Click(object sender, EventArgs e) => StartProcess("https://www.google.com");
        private void btnEdge_Click(object sender, EventArgs e) => StartProcess("msedge");
        private void btnRecycleBin_Click(object sender, EventArgs e) => StartProcess("explorer.exe", "shell:RecycleBinFolder");

        private void btnKillProcess_Click(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedRows.Count == 0)
            {
                MessageBox.Show("Будь ласка, виберіть процес.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int processId = Convert.ToInt32(dgvProcesses.SelectedRows[0].Cells[0].Value);
            try
            {
                Process.GetProcessById(processId).Kill();
                MessageBox.Show("Процес завершено.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка завершення процесу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangePriority_Click(object sender, EventArgs e)
        {
            if (dgvProcesses.SelectedRows.Count == 0 || !Enum.TryParse(txtPriority.Text, out ProcessPriorityClass newPriority))
            {
                MessageBox.Show("Виберіть процес і введіть правильний пріоритет (Idle, BelowNormal, Normal, AboveNormal, High, RealTime).", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int processId = Convert.ToInt32(dgvProcesses.SelectedRows[0].Cells[0].Value);
            try
            {
                Process.GetProcessById(processId).PriorityClass = newPriority;
                MessageBox.Show($"Пріоритет змінено на {newPriority}.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadProcesses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка зміни пріоритету: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e) => LoadProcesses();
    }
}
