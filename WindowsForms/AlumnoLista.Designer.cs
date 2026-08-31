namespace WindowsForms
{
    partial class AlumnoLista
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            agregarButton = new Button();
            actualizarButton = new Button();
            eliminarButton = new Button();
            buscarTextBox = new TextBox();
            buscarButton = new Button();
            refrescarButton = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            //
            // dataGridView1
            //
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 90);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1000, 510);
            dataGridView1.TabIndex = 5;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            //
            // agregarButton
            //
            agregarButton.Location = new Point(20, 20);
            agregarButton.Name = "agregarButton";
            agregarButton.Size = new Size(120, 45);
            agregarButton.TabIndex = 0;
            agregarButton.Text = "Agregar";
            agregarButton.UseVisualStyleBackColor = true;
            agregarButton.Click += agregarButton_Click;
            //
            // actualizarButton
            //
            actualizarButton.Location = new Point(150, 20);
            actualizarButton.Name = "actualizarButton";
            actualizarButton.Size = new Size(120, 45);
            actualizarButton.TabIndex = 1;
            actualizarButton.Text = "Actualizar";
            actualizarButton.UseVisualStyleBackColor = true;
            actualizarButton.Click += actualizarButton_Click;
            //
            // eliminarButton
            //
            eliminarButton.Location = new Point(280, 20);
            eliminarButton.Name = "eliminarButton";
            eliminarButton.Size = new Size(120, 45);
            eliminarButton.TabIndex = 2;
            eliminarButton.Text = "Eliminar";
            eliminarButton.UseVisualStyleBackColor = true;
            eliminarButton.Click += eliminarButton_Click;
            //
            // buscarTextBox
            //
            buscarTextBox.Location = new Point(430, 27);
            buscarTextBox.Name = "buscarTextBox";
            buscarTextBox.PlaceholderText = "Buscar por nombre, apellido, legajo o email...";
            buscarTextBox.Size = new Size(350, 39);
            buscarTextBox.TabIndex = 3;
            //
            // buscarButton
            //
            buscarButton.Location = new Point(790, 20);
            buscarButton.Name = "buscarButton";
            buscarButton.Size = new Size(100, 45);
            buscarButton.TabIndex = 4;
            buscarButton.Text = "Buscar";
            buscarButton.UseVisualStyleBackColor = true;
            buscarButton.Click += buscarButton_Click;
            //
            // refrescarButton
            //
            refrescarButton.Location = new Point(900, 20);
            refrescarButton.Name = "refrescarButton";
            refrescarButton.Size = new Size(90, 45);
            refrescarButton.TabIndex = 6;
            refrescarButton.Text = "Ver todos";
            refrescarButton.UseVisualStyleBackColor = true;
            refrescarButton.Click += refrescarButton_Click;
            //
            // AlumnoLista
            //
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 600);
            Controls.Add(refrescarButton);
            Controls.Add(buscarButton);
            Controls.Add(buscarTextBox);
            Controls.Add(eliminarButton);
            Controls.Add(actualizarButton);
            Controls.Add(agregarButton);
            Controls.Add(dataGridView1);
            Name = "AlumnoLista";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alumnos";
            WindowState = FormWindowState.Maximized;
            Load += AlumnoLista_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Button agregarButton;
        private Button actualizarButton;
        private Button eliminarButton;
        private TextBox buscarTextBox;
        private Button buscarButton;
        private Button refrescarButton;
    }
}
