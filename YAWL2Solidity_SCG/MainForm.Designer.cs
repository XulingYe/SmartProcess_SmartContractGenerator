
namespace Graphical2SmartContact_SCG
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_importYAWL = new System.Windows.Forms.Button();
            this.btn_fromYAWL2Table = new System.Windows.Forms.Button();
            this.btn_exportSolidity = new System.Windows.Forms.Button();
            this.textBox_SolidityExportedPath = new System.Windows.Forms.TextBox();
            this.textBox_YAWLImportedPath = new System.Windows.Forms.TextBox();
            this.groupBox_yawl = new System.Windows.Forms.GroupBox();
            this.btn_importYAWLRoles = new System.Windows.Forms.Button();
            this.richTextBox_displayYAWL = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_solidity = new System.Windows.Forms.GroupBox();
            this.richTextBox_displaySolidity = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fromTable2Solidity = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_table = new System.Windows.Forms.GroupBox();
            this.treeView_table = new System.Windows.Forms.TreeView();
            this.treeView_displayYAWLRoles = new System.Windows.Forms.TreeView();
            this.groupBox_yawl.SuspendLayout();
            this.groupBox_solidity.SuspendLayout();
            this.groupBox_table.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_importYAWL
            // 
            this.btn_importYAWL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importYAWL.Location = new System.Drawing.Point(6, 26);
            this.btn_importYAWL.Name = "btn_importYAWL";
            this.btn_importYAWL.Size = new System.Drawing.Size(100, 30);
            this.btn_importYAWL.TabIndex = 0;
            this.btn_importYAWL.Text = "Import";
            this.btn_importYAWL.UseVisualStyleBackColor = true;
            this.btn_importYAWL.Click += new System.EventHandler(this.btn_importYAWL_Click);
            // 
            // btn_fromYAWL2Table
            // 
            this.btn_fromYAWL2Table.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fromYAWL2Table.Location = new System.Drawing.Point(667, 122);
            this.btn_fromYAWL2Table.Name = "btn_fromYAWL2Table";
            this.btn_fromYAWL2Table.Size = new System.Drawing.Size(160, 75);
            this.btn_fromYAWL2Table.TabIndex = 2;
            this.btn_fromYAWL2Table.Text = "From Graphical \r\nto Checking\r\n======>";
            this.btn_fromYAWL2Table.UseVisualStyleBackColor = true;
            this.btn_fromYAWL2Table.Click += new System.EventHandler(this.btn_fromYAWL2Table_Click);
            // 
            // btn_exportSolidity
            // 
            this.btn_exportSolidity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_exportSolidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_exportSolidity.Location = new System.Drawing.Point(10, 275);
            this.btn_exportSolidity.Name = "btn_exportSolidity";
            this.btn_exportSolidity.Size = new System.Drawing.Size(110, 30);
            this.btn_exportSolidity.TabIndex = 4;
            this.btn_exportSolidity.Text = "Export";
            this.btn_exportSolidity.UseVisualStyleBackColor = true;
            this.btn_exportSolidity.Click += new System.EventHandler(this.btn_exportSolidity_Click);
            // 
            // textBox_SolidityExportedPath
            // 
            this.textBox_SolidityExportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SolidityExportedPath.Location = new System.Drawing.Point(123, 279);
            this.textBox_SolidityExportedPath.Name = "textBox_SolidityExportedPath";
            this.textBox_SolidityExportedPath.Size = new System.Drawing.Size(527, 22);
            this.textBox_SolidityExportedPath.TabIndex = 5;
            // 
            // textBox_YAWLImportedPath
            // 
            this.textBox_YAWLImportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_YAWLImportedPath.Location = new System.Drawing.Point(112, 30);
            this.textBox_YAWLImportedPath.Name = "textBox_YAWLImportedPath";
            this.textBox_YAWLImportedPath.Size = new System.Drawing.Size(408, 22);
            this.textBox_YAWLImportedPath.TabIndex = 6;
            // 
            // groupBox_yawl
            // 
            this.groupBox_yawl.Controls.Add(this.treeView_displayYAWLRoles);
            this.groupBox_yawl.Controls.Add(this.btn_importYAWLRoles);
            this.groupBox_yawl.Controls.Add(this.richTextBox_displayYAWL);
            this.groupBox_yawl.Controls.Add(this.label2);
            this.groupBox_yawl.Controls.Add(this.btn_importYAWL);
            this.groupBox_yawl.Controls.Add(this.textBox_YAWLImportedPath);
            this.groupBox_yawl.Location = new System.Drawing.Point(0, 0);
            this.groupBox_yawl.Name = "groupBox_yawl";
            this.groupBox_yawl.Size = new System.Drawing.Size(661, 329);
            this.groupBox_yawl.TabIndex = 8;
            this.groupBox_yawl.TabStop = false;
            this.groupBox_yawl.Text = "YAWL";
            // 
            // btn_importYAWLRoles
            // 
            this.btn_importYAWLRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importYAWLRoles.Location = new System.Drawing.Point(542, 30);
            this.btn_importYAWLRoles.Name = "btn_importYAWLRoles";
            this.btn_importYAWLRoles.Size = new System.Drawing.Size(100, 30);
            this.btn_importYAWLRoles.TabIndex = 10;
            this.btn_importYAWLRoles.Text = "Import Roles";
            this.btn_importYAWLRoles.UseVisualStyleBackColor = true;
            this.btn_importYAWLRoles.Click += new System.EventHandler(this.btn_importYAWLRoles_Click);
            // 
            // richTextBox_displayYAWL
            // 
            this.richTextBox_displayYAWL.Location = new System.Drawing.Point(5, 66);
            this.richTextBox_displayYAWL.Name = "richTextBox_displayYAWL";
            this.richTextBox_displayYAWL.Size = new System.Drawing.Size(515, 257);
            this.richTextBox_displayYAWL.TabIndex = 8;
            this.richTextBox_displayYAWL.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Graphical representation";
            // 
            // groupBox_solidity
            // 
            this.groupBox_solidity.Controls.Add(this.richTextBox_displaySolidity);
            this.groupBox_solidity.Controls.Add(this.label1);
            this.groupBox_solidity.Controls.Add(this.btn_exportSolidity);
            this.groupBox_solidity.Controls.Add(this.textBox_SolidityExportedPath);
            this.groupBox_solidity.Location = new System.Drawing.Point(5, 335);
            this.groupBox_solidity.Name = "groupBox_solidity";
            this.groupBox_solidity.Size = new System.Drawing.Size(656, 311);
            this.groupBox_solidity.TabIndex = 9;
            this.groupBox_solidity.TabStop = false;
            this.groupBox_solidity.Text = "Solidity";
            // 
            // richTextBox_displaySolidity
            // 
            this.richTextBox_displaySolidity.Location = new System.Drawing.Point(6, 20);
            this.richTextBox_displaySolidity.Name = "richTextBox_displaySolidity";
            this.richTextBox_displaySolidity.Size = new System.Drawing.Size(642, 245);
            this.richTextBox_displaySolidity.TabIndex = 9;
            this.richTextBox_displaySolidity.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Smart Contract Langauge";
            // 
            // btn_fromTable2Solidity
            // 
            this.btn_fromTable2Solidity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fromTable2Solidity.Location = new System.Drawing.Point(667, 440);
            this.btn_fromTable2Solidity.Name = "btn_fromTable2Solidity";
            this.btn_fromTable2Solidity.Size = new System.Drawing.Size(160, 80);
            this.btn_fromTable2Solidity.TabIndex = 10;
            this.btn_fromTable2Solidity.Text = "From Checking \r\nto Smart Contract\r\n<======";
            this.btn_fromTable2Solidity.UseVisualStyleBackColor = true;
            this.btn_fromTable2Solidity.Click += new System.EventHandler(this.btn_fromTable2Solidity_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Content checking";
            // 
            // groupBox_table
            // 
            this.groupBox_table.Controls.Add(this.treeView_table);
            this.groupBox_table.Controls.Add(this.label3);
            this.groupBox_table.Location = new System.Drawing.Point(832, 2);
            this.groupBox_table.Name = "groupBox_table";
            this.groupBox_table.Size = new System.Drawing.Size(370, 644);
            this.groupBox_table.TabIndex = 9;
            this.groupBox_table.TabStop = false;
            this.groupBox_table.Text = "Table";
            // 
            // treeView_table
            // 
            this.treeView_table.Location = new System.Drawing.Point(14, 28);
            this.treeView_table.Name = "treeView_table";
            this.treeView_table.Size = new System.Drawing.Size(350, 606);
            this.treeView_table.TabIndex = 8;
            // 
            // treeView_displayYAWLRoles
            // 
            this.treeView_displayYAWLRoles.Location = new System.Drawing.Point(526, 66);
            this.treeView_displayYAWLRoles.Name = "treeView_displayYAWLRoles";
            this.treeView_displayYAWLRoles.Size = new System.Drawing.Size(129, 257);
            this.treeView_displayYAWLRoles.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1209, 651);
            this.Controls.Add(this.groupBox_table);
            this.Controls.Add(this.btn_fromTable2Solidity);
            this.Controls.Add(this.btn_fromYAWL2Table);
            this.Controls.Add(this.groupBox_solidity);
            this.Controls.Add(this.groupBox_yawl);
            this.Name = "MainForm";
            this.Text = "Smart Contract Generator";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.groupBox_yawl.ResumeLayout(false);
            this.groupBox_yawl.PerformLayout();
            this.groupBox_solidity.ResumeLayout(false);
            this.groupBox_solidity.PerformLayout();
            this.groupBox_table.ResumeLayout(false);
            this.groupBox_table.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_importYAWL;
        private System.Windows.Forms.Button btn_fromYAWL2Table;
        private System.Windows.Forms.Button btn_exportSolidity;
        private System.Windows.Forms.TextBox textBox_SolidityExportedPath;
        private System.Windows.Forms.TextBox textBox_YAWLImportedPath;
        private System.Windows.Forms.GroupBox groupBox_yawl;
        private System.Windows.Forms.GroupBox groupBox_solidity;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fromTable2Solidity;
        private System.Windows.Forms.RichTextBox richTextBox_displayYAWL;
        private System.Windows.Forms.RichTextBox richTextBox_displaySolidity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox_table;
        private System.Windows.Forms.TreeView treeView_table;
        private System.Windows.Forms.Button btn_importYAWLRoles;
        private System.Windows.Forms.TreeView treeView_displayYAWLRoles;
    }
}

