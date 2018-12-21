namespace ShaftAppForm
{
    partial class GUI
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.paramGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.threadComboBox = new System.Windows.Forms.ComboBox();
            this.diamHeadLabel = new System.Windows.Forms.Label();
            this.diamHeadTextBox = new System.Windows.Forms.TextBox();
            this.headLabel = new System.Windows.Forms.Label();
            this.lengthBracingTextBox = new System.Windows.Forms.TextBox();
            this.diamBracingTextBox = new System.Windows.Forms.TextBox();
            this.lengthLegTextBox = new System.Windows.Forms.TextBox();
            this.diamLegLabel = new System.Windows.Forms.Label();
            this.diamLegTextBox = new System.Windows.Forms.TextBox();
            this.lengthHeadTextBox = new System.Windows.Forms.TextBox();
            this.legLabel = new System.Windows.Forms.Label();
            this.bracingLabel = new System.Windows.Forms.Label();
            this.diamBracingLabel = new System.Windows.Forms.Label();
            this.buildButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.paramGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // paramGroupBox
            // 
            this.paramGroupBox.Controls.Add(this.label1);
            this.paramGroupBox.Controls.Add(this.threadComboBox);
            this.paramGroupBox.Controls.Add(this.diamHeadLabel);
            this.paramGroupBox.Controls.Add(this.diamHeadTextBox);
            this.paramGroupBox.Controls.Add(this.headLabel);
            this.paramGroupBox.Controls.Add(this.lengthBracingTextBox);
            this.paramGroupBox.Controls.Add(this.diamBracingTextBox);
            this.paramGroupBox.Controls.Add(this.lengthLegTextBox);
            this.paramGroupBox.Controls.Add(this.diamLegLabel);
            this.paramGroupBox.Controls.Add(this.diamLegTextBox);
            this.paramGroupBox.Controls.Add(this.lengthHeadTextBox);
            this.paramGroupBox.Controls.Add(this.legLabel);
            this.paramGroupBox.Controls.Add(this.bracingLabel);
            this.paramGroupBox.Controls.Add(this.diamBracingLabel);
            this.paramGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.paramGroupBox.Location = new System.Drawing.Point(12, 12);
            this.paramGroupBox.Name = "paramGroupBox";
            this.paramGroupBox.Size = new System.Drawing.Size(257, 280);
            this.paramGroupBox.TabIndex = 23;
            this.paramGroupBox.TabStop = false;
            this.paramGroupBox.Text = "Параметры детали";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 253);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Резьба";
            // 
            // threadComboBox
            // 
            this.threadComboBox.FormattingEnabled = true;
            this.threadComboBox.Location = new System.Drawing.Point(146, 248);
            this.threadComboBox.Name = "threadComboBox";
            this.threadComboBox.Size = new System.Drawing.Size(100, 26);
            this.threadComboBox.TabIndex = 7;
           // this.threadComboBox.SelectedIndexChanged += new System.EventHandler(this.threadComboBox_SelectedIndexChanged);
            // 
            // diamHeadLabel
            // 
            this.diamHeadLabel.AutoSize = true;
            this.diamHeadLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.diamHeadLabel.Location = new System.Drawing.Point(6, 26);
            this.diamHeadLabel.Name = "diamHeadLabel";
            this.diamHeadLabel.Size = new System.Drawing.Size(124, 16);
            this.diamHeadLabel.TabIndex = 0;
            this.diamHeadLabel.Text = "Диаметр головки:";
            // 
            // diamHeadTextBox
            // 
            this.diamHeadTextBox.Location = new System.Drawing.Point(146, 24);
            this.diamHeadTextBox.Name = "diamHeadTextBox";
            this.diamHeadTextBox.Size = new System.Drawing.Size(100, 24);
            this.diamHeadTextBox.TabIndex = 1;
            this.diamHeadTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.diamHeadTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // headLabel
            // 
            this.headLabel.AutoSize = true;
            this.headLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.headLabel.Location = new System.Drawing.Point(6, 60);
            this.headLabel.Name = "headLabel";
            this.headLabel.Size = new System.Drawing.Size(108, 16);
            this.headLabel.TabIndex = 1;
            this.headLabel.Text = "Длина головки:";
            // 
            // lengthBracingTextBox
            // 
            this.lengthBracingTextBox.Location = new System.Drawing.Point(146, 211);
            this.lengthBracingTextBox.Name = "lengthBracingTextBox";
            this.lengthBracingTextBox.Size = new System.Drawing.Size(100, 24);
            this.lengthBracingTextBox.TabIndex = 6;
            this.lengthBracingTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.lengthBracingTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // diamBracingTextBox
            // 
            this.diamBracingTextBox.Location = new System.Drawing.Point(146, 175);
            this.diamBracingTextBox.Name = "diamBracingTextBox";
            this.diamBracingTextBox.Size = new System.Drawing.Size(100, 24);
            this.diamBracingTextBox.TabIndex = 5;
            this.diamBracingTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.diamBracingTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lengthLegTextBox
            // 
            this.lengthLegTextBox.Location = new System.Drawing.Point(146, 134);
            this.lengthLegTextBox.Name = "lengthLegTextBox";
            this.lengthLegTextBox.Size = new System.Drawing.Size(100, 24);
            this.lengthLegTextBox.TabIndex = 4;
            this.lengthLegTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.lengthLegTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // diamLegLabel
            // 
            this.diamLegLabel.AutoSize = true;
            this.diamLegLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.diamLegLabel.Location = new System.Drawing.Point(6, 96);
            this.diamLegLabel.Name = "diamLegLabel";
            this.diamLegLabel.Size = new System.Drawing.Size(111, 16);
            this.diamLegLabel.TabIndex = 2;
            this.diamLegLabel.Text = "Диаметр ножки:";
            // 
            // diamLegTextBox
            // 
            this.diamLegTextBox.Location = new System.Drawing.Point(146, 94);
            this.diamLegTextBox.Name = "diamLegTextBox";
            this.diamLegTextBox.Size = new System.Drawing.Size(100, 24);
            this.diamLegTextBox.TabIndex = 3;
            this.diamLegTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.diamLegTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lengthHeadTextBox
            // 
            this.lengthHeadTextBox.Location = new System.Drawing.Point(146, 58);
            this.lengthHeadTextBox.Name = "lengthHeadTextBox";
            this.lengthHeadTextBox.Size = new System.Drawing.Size(100, 24);
            this.lengthHeadTextBox.TabIndex = 2;
            this.lengthHeadTextBox.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            this.lengthHeadTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // legLabel
            // 
            this.legLabel.AutoSize = true;
            this.legLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.legLabel.Location = new System.Drawing.Point(6, 136);
            this.legLabel.Name = "legLabel";
            this.legLabel.Size = new System.Drawing.Size(95, 16);
            this.legLabel.TabIndex = 3;
            this.legLabel.Text = "Длина ножки:";
            // 
            // bracingLabel
            // 
            this.bracingLabel.AutoSize = true;
            this.bracingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bracingLabel.Location = new System.Drawing.Point(6, 177);
            this.bracingLabel.Name = "bracingLabel";
            this.bracingLabel.Size = new System.Drawing.Size(134, 16);
            this.bracingLabel.TabIndex = 4;
            this.bracingLabel.Text = "Диамер крепления:";
            // 
            // diamBracingLabel
            // 
            this.diamBracingLabel.AutoSize = true;
            this.diamBracingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.diamBracingLabel.Location = new System.Drawing.Point(6, 219);
            this.diamBracingLabel.Name = "diamBracingLabel";
            this.diamBracingLabel.Size = new System.Drawing.Size(125, 16);
            this.diamBracingLabel.TabIndex = 5;
            this.diamBracingLabel.Text = "Длина крепления:";
            // 
            // buildButton
            // 
            this.buildButton.Location = new System.Drawing.Point(185, 298);
            this.buildButton.Name = "buildButton";
            this.buildButton.Size = new System.Drawing.Size(84, 28);
            this.buildButton.TabIndex = 8;
            this.buildButton.Text = "Построить";
            this.buildButton.UseVisualStyleBackColor = true;
            this.buildButton.Click += new System.EventHandler(this.buildButton_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 338);
            this.Controls.Add(this.paramGroupBox);
            this.Controls.Add(this.buildButton);
            this.Name = "GUI";
            this.Text = "Form1";
            this.paramGroupBox.ResumeLayout(false);
            this.paramGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox paramGroupBox;
        private System.Windows.Forms.Label diamHeadLabel;
        private System.Windows.Forms.TextBox diamHeadTextBox;
        private System.Windows.Forms.Label headLabel;
        private System.Windows.Forms.TextBox lengthBracingTextBox;
        private System.Windows.Forms.TextBox diamBracingTextBox;
        private System.Windows.Forms.TextBox lengthLegTextBox;
        private System.Windows.Forms.Label diamLegLabel;
        private System.Windows.Forms.TextBox diamLegTextBox;
        private System.Windows.Forms.TextBox lengthHeadTextBox;
        private System.Windows.Forms.Label legLabel;
        private System.Windows.Forms.Label bracingLabel;
        private System.Windows.Forms.Label diamBracingLabel;
        private System.Windows.Forms.Button buildButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox threadComboBox;
    }
}

