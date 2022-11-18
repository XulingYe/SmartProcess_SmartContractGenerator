
namespace Graphical2SmartContact_SCG
{
    partial class AddAction
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btn_addAction = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label_TaskID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(116, 111);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 11;
            // 
            // btn_addAction
            // 
            this.btn_addAction.Location = new System.Drawing.Point(159, 246);
            this.btn_addAction.Name = "btn_addAction";
            this.btn_addAction.Size = new System.Drawing.Size(91, 23);
            this.btn_addAction.TabIndex = 12;
            this.btn_addAction.Text = "Add Action";
            this.btn_addAction.UseVisualStyleBackColor = true;
            this.btn_addAction.Click += new System.EventHandler(this.btn_addAction_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Selected task ID:";
            // 
            // label_TaskID
            // 
            this.label_TaskID.AutoSize = true;
            this.label_TaskID.Location = new System.Drawing.Point(156, 43);
            this.label_TaskID.Name = "label_TaskID";
            this.label_TaskID.Size = new System.Drawing.Size(46, 17);
            this.label_TaskID.TabIndex = 14;
            this.label_TaskID.Text = "empty";
            // 
            // AddAction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_TaskID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_addAction);
            this.Controls.Add(this.comboBox1);
            this.Name = "AddAction";
            this.Text = "Add Action";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btn_addAction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_TaskID;
    }
}