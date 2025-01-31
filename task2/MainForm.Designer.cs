using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProcessManager
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Елементи інтерфейсу
        private DataGridView dgvProcesses;
        private Button btnCalculator;
        private Button btnWord;
        private Button btnGoogle;
        private Button btnEdge;
        private Button btnRecycleBin;
        private Button btnKillProcess;
        private Button btnChangePriority;
        private Button btnRefresh;
        private TextBox txtPriority;

        /// <summary>
        /// Метод ініціалізації елементів.
        /// </summary>
        private void InitializeComponent()
        {
            // Створення елементів
            this.dgvProcesses = new DataGridView();
            this.btnCalculator = new Button();
            this.btnWord = new Button();
            this.btnGoogle = new Button();
            this.btnEdge = new Button();
            this.btnRecycleBin = new Button();
            this.btnKillProcess = new Button();
            this.btnChangePriority = new Button();
            this.txtPriority = new TextBox();
            this.btnRefresh = new Button();

            // Налаштування DataGridView
            this.dgvProcesses.Columns.Add("ID", "ID процесу");
            this.dgvProcesses.Columns.Add("Name", "Ім'я процесу");
            this.dgvProcesses.Columns.Add("Memory", "Оперативна пам'ять");
            this.dgvProcesses.Columns.Add("StartTime", "Час запуску");
            this.dgvProcesses.Columns.Add("Priority", "Пріоритет");
            this.dgvProcesses.Columns.Add("Threads", "Кількість потоків");
            this.dgvProcesses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProcesses.Location = new Point(10, 10);
            this.dgvProcesses.Size = new Size(600, 300);

            // Налаштування кнопок
            this.btnCalculator.Text = "Калькулятор";
            this.btnCalculator.Location = new Point(10, 320);
            this.btnCalculator.Size = new Size(100, 30);
            this.btnCalculator.Click += new EventHandler(this.btnCalculator_Click);

            this.btnWord.Text = "Microsoft Word";
            this.btnWord.Location = new Point(120, 320);
            this.btnWord.Size = new Size(120, 30);
            this.btnWord.Click += new EventHandler(this.btnWord_Click);

            this.btnGoogle.Text = "Google";
            this.btnGoogle.Location = new Point(250, 320);
            this.btnGoogle.Size = new Size(100, 30);
            this.btnGoogle.Click += new EventHandler(this.btnGoogle_Click);

            this.btnEdge = new Button();
            this.btnEdge.Text = "Microsoft Edge";
            this.btnEdge.Location = new Point(340, 320);
            this.btnEdge.Size = new Size(120, 30); // Задайте розмір
            this.btnEdge.Click += new EventHandler(this.btnEdge_Click);

            this.btnRecycleBin.Text = "Кошик";
            this.btnRecycleBin.Location = new Point(470, 320);
            this.btnRecycleBin.Size = new Size(100, 30);
            this.btnRecycleBin.Click += new EventHandler(this.btnRecycleBin_Click);

            this.btnKillProcess.Text = "Зупинити процес";
            this.btnKillProcess.Location = new Point(10, 360);
            this.btnKillProcess.Size = new Size(150, 30);
            this.btnKillProcess.Click += new EventHandler(this.btnKillProcess_Click);

            this.txtPriority.Location = new Point(170, 360);
            this.txtPriority.Size = new Size(100, 20);

            this.btnChangePriority.Text = "Змінити пріоритет";
            this.btnChangePriority.Location = new Point(280, 360);
            this.btnChangePriority.Size = new Size(150, 30);
            this.btnChangePriority.Click += new EventHandler(this.btnChangePriority_Click);

            this.btnRefresh.Text = "Оновити список";
            this.btnRefresh.Location = new Point(440, 360);
            this.btnRefresh.Size = new Size(120, 30);
            this.btnRefresh.Click += new EventHandler(this.btnRefresh_Click);

            // Додавання елементів на форму
            this.Controls.Add(this.dgvProcesses);
            this.Controls.Add(this.btnCalculator);
            this.Controls.Add(this.btnWord);
            this.Controls.Add(this.btnGoogle);
            this.Controls.Add(this.btnEdge);
            this.Controls.Add(this.btnRecycleBin);
            this.Controls.Add(this.btnKillProcess);
            this.Controls.Add(this.txtPriority);
            this.Controls.Add(this.btnChangePriority);
            this.Controls.Add(this.btnRefresh);

            // Налаштування форми
            this.Text = "Менеджер процесів";
            this.Size = new Size(640, 480);
        }


        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
    }
}
