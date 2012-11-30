namespace task
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SLAEButton = new System.Windows.Forms.Button();
            this.IntegralButton = new System.Windows.Forms.Button();
            this.openFileButton = new System.Windows.Forms.Button();
            this.solveButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.monitor = new System.Windows.Forms.RichTextBox();
            this.result = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // SLAEButton
            // 
            this.SLAEButton.Location = new System.Drawing.Point(108, 46);
            this.SLAEButton.Name = "SLAEButton";
            this.SLAEButton.Size = new System.Drawing.Size(252, 85);
            this.SLAEButton.TabIndex = 0;
            this.SLAEButton.Text = "SLAE";
            this.SLAEButton.UseVisualStyleBackColor = true;
            this.SLAEButton.Click += new System.EventHandler(this.SLAEButton_Click);
            // 
            // IntegralButton
            // 
            this.IntegralButton.Location = new System.Drawing.Point(108, 137);
            this.IntegralButton.Name = "IntegralButton";
            this.IntegralButton.Size = new System.Drawing.Size(252, 85);
            this.IntegralButton.TabIndex = 1;
            this.IntegralButton.Text = "Integral";
            this.IntegralButton.UseVisualStyleBackColor = true;
            this.IntegralButton.Click += new System.EventHandler(this.IntegralButton_Click);
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(12, 12);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(128, 28);
            this.openFileButton.TabIndex = 2;
            this.openFileButton.Text = "Open File";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Visible = false;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // solveButton
            // 
            this.solveButton.Location = new System.Drawing.Point(319, 12);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(133, 28);
            this.solveButton.TabIndex = 3;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = true;
            this.solveButton.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // monitor
            // 
            this.monitor.Location = new System.Drawing.Point(12, 46);
            this.monitor.Name = "monitor";
            this.monitor.ReadOnly = true;
            this.monitor.Size = new System.Drawing.Size(216, 224);
            this.monitor.TabIndex = 4;
            this.monitor.Text = "";
            this.monitor.Visible = false;
            // 
            // result
            // 
            this.result.Location = new System.Drawing.Point(236, 46);
            this.result.Name = "result";
            this.result.ReadOnly = true;
            this.result.Size = new System.Drawing.Size(216, 224);
            this.result.TabIndex = 5;
            this.result.Text = "";
            this.result.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 282);
            this.Controls.Add(this.result);
            this.Controls.Add(this.monitor);
            this.Controls.Add(this.solveButton);
            this.Controls.Add(this.openFileButton);
            this.Controls.Add(this.IntegralButton);
            this.Controls.Add(this.SLAEButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SLAEButton;
        private System.Windows.Forms.Button IntegralButton;
        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox monitor;
        private System.Windows.Forms.RichTextBox result;
    }
}

