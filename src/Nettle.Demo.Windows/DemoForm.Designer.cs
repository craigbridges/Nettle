namespace Nettle.Demo.Windows
{
    partial class DemoForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DemoForm));
            this.label1 = new System.Windows.Forms.Label();
            this.templateTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.outputTextBox = new System.Windows.Forms.TextBox();
            this.renderButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Template Code";
            // 
            // templateTextBox
            // 
            this.templateTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.templateTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateTextBox.ForeColor = System.Drawing.Color.White;
            this.templateTextBox.Location = new System.Drawing.Point(10, 27);
            this.templateTextBox.Multiline = true;
            this.templateTextBox.Name = "templateTextBox";
            this.templateTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.templateTextBox.Size = new System.Drawing.Size(900, 300);
            this.templateTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 355);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Output";
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.outputTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.ForeColor = System.Drawing.Color.White;
            this.outputTextBox.Location = new System.Drawing.Point(10, 373);
            this.outputTextBox.Multiline = true;
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.outputTextBox.Size = new System.Drawing.Size(900, 300);
            this.outputTextBox.TabIndex = 3;
            // 
            // renderButton
            // 
            this.renderButton.Location = new System.Drawing.Point(813, 335);
            this.renderButton.Name = "renderButton";
            this.renderButton.Size = new System.Drawing.Size(97, 26);
            this.renderButton.TabIndex = 4;
            this.renderButton.Text = "Render Output";
            this.renderButton.UseVisualStyleBackColor = true;
            this.renderButton.Click += new System.EventHandler(this.renderButton_Click);
            // 
            // DemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(921, 685);
            this.Controls.Add(this.renderButton);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.templateTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DemoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nettle Windows Demo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox templateTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button renderButton;
    }
}

