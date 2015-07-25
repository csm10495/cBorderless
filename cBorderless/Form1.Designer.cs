namespace cBorderless
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_make_borderless = new System.Windows.Forms.Button();
            this.label_process = new System.Windows.Forms.Label();
            this.combobox_processes = new System.Windows.Forms.ComboBox();
            this.button_refresh = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // button_make_borderless
            // 
            this.button_make_borderless.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_make_borderless.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_make_borderless.Location = new System.Drawing.Point(76, 39);
            this.button_make_borderless.Name = "button_make_borderless";
            this.button_make_borderless.Size = new System.Drawing.Size(212, 63);
            this.button_make_borderless.TabIndex = 0;
            this.button_make_borderless.Text = "Make Selected Process Windowed Borderless";
            this.toolTip1.SetToolTip(this.button_make_borderless, "Make the selected process windowed borderless");
            this.button_make_borderless.UseVisualStyleBackColor = true;
            this.button_make_borderless.Click += new System.EventHandler(this.button_make_borederless_Click);
            // 
            // label_process
            // 
            this.label_process.AutoSize = true;
            this.label_process.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_process.Location = new System.Drawing.Point(12, 11);
            this.label_process.Name = "label_process";
            this.label_process.Size = new System.Drawing.Size(70, 18);
            this.label_process.TabIndex = 1;
            this.label_process.Text = "Process:";
            // 
            // combobox_processes
            // 
            this.combobox_processes.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.combobox_processes.FormattingEnabled = true;
            this.combobox_processes.Location = new System.Drawing.Point(81, 9);
            this.combobox_processes.Name = "combobox_processes";
            this.combobox_processes.Size = new System.Drawing.Size(207, 24);
            this.combobox_processes.Sorted = true;
            this.combobox_processes.TabIndex = 2;
            // 
            // button_refresh
            // 
            this.button_refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_refresh.Font = new System.Drawing.Font("Arial", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_refresh.Location = new System.Drawing.Point(15, 39);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(55, 63);
            this.button_refresh.TabIndex = 3;
            this.button_refresh.Text = "↺";
            this.toolTip1.SetToolTip(this.button_refresh, "Refresh Process List");
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 109);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.combobox_processes);
            this.Controls.Add(this.label_process);
            this.Controls.Add(this.button_make_borderless);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "cBorderless - csm10495";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_make_borderless;
        private System.Windows.Forms.Label label_process;
        private System.Windows.Forms.ComboBox combobox_processes;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

