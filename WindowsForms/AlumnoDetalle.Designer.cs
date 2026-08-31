namespace WindowsForms
{
    partial class AlumnoDetalle
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
            components = new System.ComponentModel.Container();
            nomAlumnoLabel = new Label();
            nomAlumnoTextBox = new TextBox();
            apeAlumnoLabel = new Label();
            apeAlumnoTextBox = new TextBox();
            emailLabel = new Label();
            emailTextBox = new TextBox();
            legajoLabel = new Label();
            legajoTextBox = new TextBox();
            dniLabel = new Label();
            dniTextBox = new TextBox();
            planLabel = new Label();
            planTextBox = new TextBox();
            anioCursoLabel = new Label();
            anioCursoNumeric = new NumericUpDown();
            cantMatApLabel = new Label();
            cantMatApNumeric = new NumericUpDown();
            promedioLabel = new Label();
            promedioNumeric = new NumericUpDown();
            carreraLabel = new Label();
            carreraComboBox = new ComboBox();
            guardarButton = new Button();
            cancelarButton = new Button();
            errorProvider = new ErrorProvider(components);
            ((System.ComponentModel.ISupportInitialize)anioCursoNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cantMatApNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)promedioNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            //
            // nomAlumnoLabel
            //
            nomAlumnoLabel.AutoSize = true;
            nomAlumnoLabel.Location = new Point(30, 30);
            nomAlumnoLabel.Name = "nomAlumnoLabel";
            nomAlumnoLabel.Size = new Size(100, 32);
            nomAlumnoLabel.Text = "Nombre:";
            //
            // nomAlumnoTextBox
            //
            nomAlumnoTextBox.Location = new Point(220, 27);
            nomAlumnoTextBox.Name = "nomAlumnoTextBox";
            nomAlumnoTextBox.Size = new Size(300, 39);
            //
            // apeAlumnoLabel
            //
            apeAlumnoLabel.AutoSize = true;
            apeAlumnoLabel.Location = new Point(30, 85);
            apeAlumnoLabel.Name = "apeAlumnoLabel";
            apeAlumnoLabel.Size = new Size(100, 32);
            apeAlumnoLabel.Text = "Apellido:";
            //
            // apeAlumnoTextBox
            //
            apeAlumnoTextBox.Location = new Point(220, 82);
            apeAlumnoTextBox.Name = "apeAlumnoTextBox";
            apeAlumnoTextBox.Size = new Size(300, 39);
            //
            // emailLabel
            //
            emailLabel.AutoSize = true;
            emailLabel.Location = new Point(30, 140);
            emailLabel.Name = "emailLabel";
            emailLabel.Size = new Size(80, 32);
            emailLabel.Text = "Email:";
            //
            // emailTextBox
            //
            emailTextBox.Location = new Point(220, 137);
            emailTextBox.Name = "emailTextBox";
            emailTextBox.Size = new Size(300, 39);
            //
            // legajoLabel
            //
            legajoLabel.AutoSize = true;
            legajoLabel.Location = new Point(30, 195);
            legajoLabel.Name = "legajoLabel";
            legajoLabel.Size = new Size(90, 32);
            legajoLabel.Text = "Legajo:";
            //
            // legajoTextBox
            //
            legajoTextBox.Location = new Point(220, 192);
            legajoTextBox.Name = "legajoTextBox";
            legajoTextBox.Size = new Size(300, 39);
            //
            // dniLabel
            //
            dniLabel.AutoSize = true;
            dniLabel.Location = new Point(30, 250);
            dniLabel.Name = "dniLabel";
            dniLabel.Size = new Size(60, 32);
            dniLabel.Text = "DNI:";
            //
            // dniTextBox
            //
            dniTextBox.Location = new Point(220, 247);
            dniTextBox.Name = "dniTextBox";
            dniTextBox.Size = new Size(300, 39);
            //
            // planLabel
            //
            planLabel.AutoSize = true;
            planLabel.Location = new Point(30, 305);
            planLabel.Name = "planLabel";
            planLabel.Size = new Size(60, 32);
            planLabel.Text = "Plan:";
            //
            // planTextBox
            //
            planTextBox.Location = new Point(220, 302);
            planTextBox.Name = "planTextBox";
            planTextBox.Size = new Size(300, 39);
            //
            // anioCursoLabel
            //
            anioCursoLabel.AutoSize = true;
            anioCursoLabel.Location = new Point(30, 360);
            anioCursoLabel.Name = "anioCursoLabel";
            anioCursoLabel.Size = new Size(160, 32);
            anioCursoLabel.Text = "Año en curso:";
            //
            // anioCursoNumeric
            //
            anioCursoNumeric.Location = new Point(220, 357);
            anioCursoNumeric.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            anioCursoNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            anioCursoNumeric.Name = "anioCursoNumeric";
            anioCursoNumeric.Size = new Size(120, 39);
            anioCursoNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            //
            // cantMatApLabel
            //
            cantMatApLabel.AutoSize = true;
            cantMatApLabel.Location = new Point(30, 415);
            cantMatApLabel.Name = "cantMatApLabel";
            cantMatApLabel.Size = new Size(280, 32);
            cantMatApLabel.Text = "Materias aprobadas:";
            //
            // cantMatApNumeric
            //
            cantMatApNumeric.Location = new Point(320, 412);
            cantMatApNumeric.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            cantMatApNumeric.Name = "cantMatApNumeric";
            cantMatApNumeric.Size = new Size(120, 39);
            //
            // promedioLabel
            //
            promedioLabel.AutoSize = true;
            promedioLabel.Location = new Point(30, 470);
            promedioLabel.Name = "promedioLabel";
            promedioLabel.Size = new Size(140, 32);
            promedioLabel.Text = "Promedio:";
            //
            // promedioNumeric
            //
            promedioNumeric.DecimalPlaces = 2;
            promedioNumeric.Location = new Point(220, 467);
            promedioNumeric.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            promedioNumeric.Name = "promedioNumeric";
            promedioNumeric.Size = new Size(120, 39);
            //
            // carreraLabel
            //
            carreraLabel.AutoSize = true;
            carreraLabel.Location = new Point(30, 525);
            carreraLabel.Name = "carreraLabel";
            carreraLabel.Size = new Size(110, 32);
            carreraLabel.Text = "Carrera:";
            //
            // carreraComboBox
            //
            carreraComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            carreraComboBox.Location = new Point(220, 522);
            carreraComboBox.Name = "carreraComboBox";
            carreraComboBox.Size = new Size(400, 40);
            //
            // guardarButton
            //
            guardarButton.Location = new Point(220, 590);
            guardarButton.Name = "guardarButton";
            guardarButton.Size = new Size(140, 48);
            guardarButton.Text = "Guardar";
            guardarButton.UseVisualStyleBackColor = true;
            guardarButton.Click += guardarButton_Click;
            //
            // cancelarButton
            //
            cancelarButton.Location = new Point(380, 590);
            cancelarButton.Name = "cancelarButton";
            cancelarButton.Size = new Size(140, 48);
            cancelarButton.Text = "Cancelar";
            cancelarButton.UseVisualStyleBackColor = true;
            cancelarButton.Click += cancelarButton_Click;
            //
            // errorProvider
            //
            errorProvider.ContainerControl = this;
            //
            // AlumnoDetalle
            //
            AcceptButton = guardarButton;
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = cancelarButton;
            ClientSize = new Size(650, 670);
            Controls.Add(nomAlumnoLabel);
            Controls.Add(nomAlumnoTextBox);
            Controls.Add(apeAlumnoLabel);
            Controls.Add(apeAlumnoTextBox);
            Controls.Add(emailLabel);
            Controls.Add(emailTextBox);
            Controls.Add(legajoLabel);
            Controls.Add(legajoTextBox);
            Controls.Add(dniLabel);
            Controls.Add(dniTextBox);
            Controls.Add(planLabel);
            Controls.Add(planTextBox);
            Controls.Add(anioCursoLabel);
            Controls.Add(anioCursoNumeric);
            Controls.Add(cantMatApLabel);
            Controls.Add(cantMatApNumeric);
            Controls.Add(promedioLabel);
            Controls.Add(promedioNumeric);
            Controls.Add(carreraLabel);
            Controls.Add(carreraComboBox);
            Controls.Add(guardarButton);
            Controls.Add(cancelarButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AlumnoDetalle";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Alumno";
            Load += AlumnoDetalle_Load;
            ((System.ComponentModel.ISupportInitialize)anioCursoNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)cantMatApNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)promedioNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nomAlumnoLabel;
        private TextBox nomAlumnoTextBox;
        private Label apeAlumnoLabel;
        private TextBox apeAlumnoTextBox;
        private Label emailLabel;
        private TextBox emailTextBox;
        private Label legajoLabel;
        private TextBox legajoTextBox;
        private Label dniLabel;
        private TextBox dniTextBox;
        private Label planLabel;
        private TextBox planTextBox;
        private Label anioCursoLabel;
        private NumericUpDown anioCursoNumeric;
        private Label cantMatApLabel;
        private NumericUpDown cantMatApNumeric;
        private Label promedioLabel;
        private NumericUpDown promedioNumeric;
        private Label carreraLabel;
        private ComboBox carreraComboBox;
        private Button guardarButton;
        private Button cancelarButton;
        private ErrorProvider errorProvider;
    }
}
