
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
            this.btn_importGraphical = new System.Windows.Forms.Button();
            this.btn_fromGraphical2Checking = new System.Windows.Forms.Button();
            this.btn_exportSC = new System.Windows.Forms.Button();
            this.textBox_SCExportedPath = new System.Windows.Forms.TextBox();
            this.textBox_GraphicalImportedPath = new System.Windows.Forms.TextBox();
            this.groupBox_graphical = new System.Windows.Forms.GroupBox();
            this.treeView_displayYAWLRoles = new System.Windows.Forms.TreeView();
            this.btn_importYAWLRoles = new System.Windows.Forms.Button();
            this.richTextBox_displayGraphical = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_SC = new System.Windows.Forms.GroupBox();
            this.treeView_SCfileTree = new System.Windows.Forms.TreeView();
            this.richTextBox_displaySC = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_fromChecking2SC = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_checking = new System.Windows.Forms.GroupBox();
            this.treeView_Checking = new System.Windows.Forms.TreeView();
            this.groupBox_graphical.SuspendLayout();
            this.groupBox_SC.SuspendLayout();
            this.groupBox_checking.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_importGraphical
            // 
            this.btn_importGraphical.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importGraphical.Location = new System.Drawing.Point(6, 26);
            this.btn_importGraphical.Name = "btn_importGraphical";
            this.btn_importGraphical.Size = new System.Drawing.Size(100, 30);
            this.btn_importGraphical.TabIndex = 0;
            this.btn_importGraphical.Text = "Import";
            this.btn_importGraphical.UseVisualStyleBackColor = true;
            this.btn_importGraphical.Click += new System.EventHandler(this.btn_importGraphical_Click);
            // 
            // btn_fromGraphical2Checking
            // 
            this.btn_fromGraphical2Checking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fromGraphical2Checking.Location = new System.Drawing.Point(667, 122);
            this.btn_fromGraphical2Checking.Name = "btn_fromGraphical2Checking";
            this.btn_fromGraphical2Checking.Size = new System.Drawing.Size(160, 75);
            this.btn_fromGraphical2Checking.TabIndex = 2;
            this.btn_fromGraphical2Checking.Text = "From Graphical \r\nto Checking\r\n======>";
            this.btn_fromGraphical2Checking.UseVisualStyleBackColor = true;
            this.btn_fromGraphical2Checking.Click += new System.EventHandler(this.btn_fromGraphical2Checking_Click);
            // 
            // btn_exportSC
            // 
            this.btn_exportSC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_exportSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_exportSC.Location = new System.Drawing.Point(10, 275);
            this.btn_exportSC.Name = "btn_exportSC";
            this.btn_exportSC.Size = new System.Drawing.Size(110, 30);
            this.btn_exportSC.TabIndex = 4;
            this.btn_exportSC.Text = "Export";
            this.btn_exportSC.UseVisualStyleBackColor = true;
            this.btn_exportSC.Click += new System.EventHandler(this.btn_exportSolidity_Click);
            // 
            // textBox_SCExportedPath
            // 
            this.textBox_SCExportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SCExportedPath.Location = new System.Drawing.Point(123, 279);
            this.textBox_SCExportedPath.Name = "textBox_SCExportedPath";
            this.textBox_SCExportedPath.Size = new System.Drawing.Size(527, 22);
            this.textBox_SCExportedPath.TabIndex = 5;
            // 
            // textBox_GraphicalImportedPath
            // 
            this.textBox_GraphicalImportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_GraphicalImportedPath.Location = new System.Drawing.Point(112, 30);
            this.textBox_GraphicalImportedPath.Name = "textBox_GraphicalImportedPath";
            this.textBox_GraphicalImportedPath.Size = new System.Drawing.Size(408, 22);
            this.textBox_GraphicalImportedPath.TabIndex = 6;
            // 
            // groupBox_graphical
            // 
            this.groupBox_graphical.Controls.Add(this.treeView_displayYAWLRoles);
            this.groupBox_graphical.Controls.Add(this.btn_importYAWLRoles);
            this.groupBox_graphical.Controls.Add(this.richTextBox_displayGraphical);
            this.groupBox_graphical.Controls.Add(this.label2);
            this.groupBox_graphical.Controls.Add(this.btn_importGraphical);
            this.groupBox_graphical.Controls.Add(this.textBox_GraphicalImportedPath);
            this.groupBox_graphical.Location = new System.Drawing.Point(0, 0);
            this.groupBox_graphical.Name = "groupBox_graphical";
            this.groupBox_graphical.Size = new System.Drawing.Size(661, 329);
            this.groupBox_graphical.TabIndex = 8;
            this.groupBox_graphical.TabStop = false;
            this.groupBox_graphical.Text = "YAWL";
            // 
            // treeView_displayYAWLRoles
            // 
            this.treeView_displayYAWLRoles.Location = new System.Drawing.Point(526, 66);
            this.treeView_displayYAWLRoles.Name = "treeView_displayYAWLRoles";
            this.treeView_displayYAWLRoles.Size = new System.Drawing.Size(129, 257);
            this.treeView_displayYAWLRoles.TabIndex = 11;
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
            // richTextBox_displayGraphical
            // 
            this.richTextBox_displayGraphical.Location = new System.Drawing.Point(5, 66);
            this.richTextBox_displayGraphical.Name = "richTextBox_displayGraphical";
            this.richTextBox_displayGraphical.Size = new System.Drawing.Size(515, 257);
            this.richTextBox_displayGraphical.TabIndex = 8;
            this.richTextBox_displayGraphical.Text = "";
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
            // groupBox_SC
            // 
            this.groupBox_SC.Controls.Add(this.treeView_SCfileTree);
            this.groupBox_SC.Controls.Add(this.richTextBox_displaySC);
            this.groupBox_SC.Controls.Add(this.label1);
            this.groupBox_SC.Controls.Add(this.btn_exportSC);
            this.groupBox_SC.Controls.Add(this.textBox_SCExportedPath);
            this.groupBox_SC.Location = new System.Drawing.Point(5, 335);
            this.groupBox_SC.Name = "groupBox_SC";
            this.groupBox_SC.Size = new System.Drawing.Size(656, 311);
            this.groupBox_SC.TabIndex = 9;
            this.groupBox_SC.TabStop = false;
            this.groupBox_SC.Text = "Solidity";
            // 
            // treeView_SCfileTree
            // 
            this.treeView_SCfileTree.Location = new System.Drawing.Point(6, 21);
            this.treeView_SCfileTree.Name = "treeView_SCfileTree";
            this.treeView_SCfileTree.Size = new System.Drawing.Size(95, 248);
            this.treeView_SCfileTree.TabIndex = 12;
            this.treeView_SCfileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_SCfileTree_NodeMouseClick);
            // 
            // richTextBox_displaySC
            // 
            this.richTextBox_displaySC.Location = new System.Drawing.Point(107, 20);
            this.richTextBox_displaySC.Name = "richTextBox_displaySC";
            this.richTextBox_displaySC.Size = new System.Drawing.Size(541, 253);
            this.richTextBox_displaySC.TabIndex = 9;
            this.richTextBox_displaySC.Text = "";
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
            // btn_fromChecking2SC
            // 
            this.btn_fromChecking2SC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fromChecking2SC.Location = new System.Drawing.Point(667, 440);
            this.btn_fromChecking2SC.Name = "btn_fromChecking2SC";
            this.btn_fromChecking2SC.Size = new System.Drawing.Size(160, 80);
            this.btn_fromChecking2SC.TabIndex = 10;
            this.btn_fromChecking2SC.Text = "From Checking \r\nto Smart Contract\r\n<======";
            this.btn_fromChecking2SC.UseVisualStyleBackColor = true;
            this.btn_fromChecking2SC.Click += new System.EventHandler(this.btn_fromChecking2SC_Click);
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
            // groupBox_checking
            // 
            this.groupBox_checking.Controls.Add(this.treeView_Checking);
            this.groupBox_checking.Controls.Add(this.label3);
            this.groupBox_checking.Location = new System.Drawing.Point(832, 2);
            this.groupBox_checking.Name = "groupBox_checking";
            this.groupBox_checking.Size = new System.Drawing.Size(370, 644);
            this.groupBox_checking.TabIndex = 9;
            this.groupBox_checking.TabStop = false;
            this.groupBox_checking.Text = "Table";
            // 
            // treeView_Checking
            // 
            this.treeView_Checking.Location = new System.Drawing.Point(14, 28);
            this.treeView_Checking.Name = "treeView_Checking";
            this.treeView_Checking.Size = new System.Drawing.Size(350, 606);
            this.treeView_Checking.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1209, 651);
            this.Controls.Add(this.groupBox_checking);
            this.Controls.Add(this.btn_fromChecking2SC);
            this.Controls.Add(this.btn_fromGraphical2Checking);
            this.Controls.Add(this.groupBox_SC);
            this.Controls.Add(this.groupBox_graphical);
            this.Name = "MainForm";
            this.Text = "Smart Contract Generator";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.groupBox_graphical.ResumeLayout(false);
            this.groupBox_graphical.PerformLayout();
            this.groupBox_SC.ResumeLayout(false);
            this.groupBox_SC.PerformLayout();
            this.groupBox_checking.ResumeLayout(false);
            this.groupBox_checking.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_importGraphical;
        private System.Windows.Forms.Button btn_fromGraphical2Checking;
        private System.Windows.Forms.Button btn_exportSC;
        private System.Windows.Forms.TextBox textBox_SCExportedPath;
        private System.Windows.Forms.TextBox textBox_GraphicalImportedPath;
        private System.Windows.Forms.GroupBox groupBox_graphical;
        private System.Windows.Forms.GroupBox groupBox_SC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fromChecking2SC;
        private System.Windows.Forms.RichTextBox richTextBox_displayGraphical;
        private System.Windows.Forms.RichTextBox richTextBox_displaySC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox_checking;
        private System.Windows.Forms.TreeView treeView_Checking;
        private System.Windows.Forms.Button btn_importYAWLRoles;
        private System.Windows.Forms.TreeView treeView_displayYAWLRoles;
        private System.Windows.Forms.TreeView treeView_SCfileTree;
    }
}

