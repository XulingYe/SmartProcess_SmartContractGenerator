
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
            this.btn_SC2Checking = new System.Windows.Forms.Button();
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
            this.btn_fromGraphical2SC = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_checking = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.treeView_Checking = new System.Windows.Forms.TreeView();
            this.richTextBox_errorMassage = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox_graphical.SuspendLayout();
            this.groupBox_SC.SuspendLayout();
            this.groupBox_checking.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            // btn_SC2Checking
            // 
            this.btn_SC2Checking.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SC2Checking.Location = new System.Drawing.Point(722, 541);
            this.btn_SC2Checking.Name = "btn_SC2Checking";
            this.btn_SC2Checking.Size = new System.Drawing.Size(85, 120);
            this.btn_SC2Checking.TabIndex = 2;
            this.btn_SC2Checking.Text = "Smart contract checking\r\n=====>";
            this.btn_SC2Checking.UseVisualStyleBackColor = true;
            this.btn_SC2Checking.Click += new System.EventHandler(this.btn_SC_Checking_Click);
            // 
            // btn_exportSC
            // 
            this.btn_exportSC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_exportSC.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_exportSC.Location = new System.Drawing.Point(6, 663);
            this.btn_exportSC.Name = "btn_exportSC";
            this.btn_exportSC.Size = new System.Drawing.Size(179, 30);
            this.btn_exportSC.TabIndex = 4;
            this.btn_exportSC.Text = "Export smart contracts";
            this.btn_exportSC.UseVisualStyleBackColor = true;
            this.btn_exportSC.Click += new System.EventHandler(this.btn_exportSolidity_Click);
            // 
            // textBox_SCExportedPath
            // 
            this.textBox_SCExportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_SCExportedPath.Location = new System.Drawing.Point(191, 667);
            this.textBox_SCExportedPath.Name = "textBox_SCExportedPath";
            this.textBox_SCExportedPath.Size = new System.Drawing.Size(545, 22);
            this.textBox_SCExportedPath.TabIndex = 5;
            // 
            // textBox_GraphicalImportedPath
            // 
            this.textBox_GraphicalImportedPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_GraphicalImportedPath.Location = new System.Drawing.Point(112, 30);
            this.textBox_GraphicalImportedPath.Name = "textBox_GraphicalImportedPath";
            this.textBox_GraphicalImportedPath.Size = new System.Drawing.Size(487, 22);
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
            this.groupBox_graphical.Location = new System.Drawing.Point(6, 7);
            this.groupBox_graphical.Name = "groupBox_graphical";
            this.groupBox_graphical.Size = new System.Drawing.Size(746, 329);
            this.groupBox_graphical.TabIndex = 8;
            this.groupBox_graphical.TabStop = false;
            this.groupBox_graphical.Text = "YAWL";
            // 
            // treeView_displayYAWLRoles
            // 
            this.treeView_displayYAWLRoles.Location = new System.Drawing.Point(607, 66);
            this.treeView_displayYAWLRoles.Name = "treeView_displayYAWLRoles";
            this.treeView_displayYAWLRoles.Size = new System.Drawing.Size(129, 257);
            this.treeView_displayYAWLRoles.TabIndex = 11;
            // 
            // btn_importYAWLRoles
            // 
            this.btn_importYAWLRoles.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_importYAWLRoles.Location = new System.Drawing.Point(624, 26);
            this.btn_importYAWLRoles.Name = "btn_importYAWLRoles";
            this.btn_importYAWLRoles.Size = new System.Drawing.Size(100, 30);
            this.btn_importYAWLRoles.TabIndex = 10;
            this.btn_importYAWLRoles.Text = "Import roles";
            this.btn_importYAWLRoles.UseVisualStyleBackColor = true;
            this.btn_importYAWLRoles.Click += new System.EventHandler(this.btn_importYAWLRoles_Click);
            // 
            // richTextBox_displayGraphical
            // 
            this.richTextBox_displayGraphical.Location = new System.Drawing.Point(5, 66);
            this.richTextBox_displayGraphical.Name = "richTextBox_displayGraphical";
            this.richTextBox_displayGraphical.Size = new System.Drawing.Size(594, 257);
            this.richTextBox_displayGraphical.TabIndex = 8;
            this.richTextBox_displayGraphical.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(219, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Graphical process model";
            // 
            // groupBox_SC
            // 
            this.groupBox_SC.Controls.Add(this.treeView_SCfileTree);
            this.groupBox_SC.Controls.Add(this.richTextBox_displaySC);
            this.groupBox_SC.Controls.Add(this.label1);
            this.groupBox_SC.Location = new System.Drawing.Point(6, 346);
            this.groupBox_SC.Name = "groupBox_SC";
            this.groupBox_SC.Size = new System.Drawing.Size(730, 311);
            this.groupBox_SC.TabIndex = 9;
            this.groupBox_SC.TabStop = false;
            this.groupBox_SC.Text = "Solidity";
            // 
            // treeView_SCfileTree
            // 
            this.treeView_SCfileTree.Location = new System.Drawing.Point(6, 21);
            this.treeView_SCfileTree.Name = "treeView_SCfileTree";
            this.treeView_SCfileTree.Size = new System.Drawing.Size(95, 284);
            this.treeView_SCfileTree.TabIndex = 12;
            this.treeView_SCfileTree.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_SCfileTree_NodeMouseClick);
            // 
            // richTextBox_displaySC
            // 
            this.richTextBox_displaySC.Location = new System.Drawing.Point(107, 21);
            this.richTextBox_displaySC.Name = "richTextBox_displaySC";
            this.richTextBox_displaySC.Size = new System.Drawing.Size(617, 284);
            this.richTextBox_displaySC.TabIndex = 9;
            this.richTextBox_displaySC.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Smart contract codes";
            // 
            // btn_fromGraphical2SC
            // 
            this.btn_fromGraphical2SC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fromGraphical2SC.Location = new System.Drawing.Point(768, 580);
            this.btn_fromGraphical2SC.Name = "btn_fromGraphical2SC";
            this.btn_fromGraphical2SC.Size = new System.Drawing.Size(99, 108);
            this.btn_fromGraphical2SC.TabIndex = 10;
            this.btn_fromGraphical2SC.Text = "From graphical to smart contract";
            this.btn_fromGraphical2SC.UseVisualStyleBackColor = true;
            this.btn_fromGraphical2SC.Click += new System.EventHandler(this.btn_fromGraphical2SC_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(203, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Generated components";
            // 
            // groupBox_checking
            // 
            this.groupBox_checking.Controls.Add(this.button2);
            this.groupBox_checking.Controls.Add(this.label5);
            this.groupBox_checking.Controls.Add(this.treeView1);
            this.groupBox_checking.Controls.Add(this.label4);
            this.groupBox_checking.Controls.Add(this.treeView_Checking);
            this.groupBox_checking.Controls.Add(this.label3);
            this.groupBox_checking.Location = new System.Drawing.Point(832, 6);
            this.groupBox_checking.Name = "groupBox_checking";
            this.groupBox_checking.Size = new System.Drawing.Size(549, 530);
            this.groupBox_checking.TabIndex = 9;
            this.groupBox_checking.TabStop = false;
            this.groupBox_checking.Text = "Table";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(213, 25);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 33);
            this.button2.TabIndex = 13;
            this.button2.Text = "=> Translate =>";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 33);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(140, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Process components";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(9, 64);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(261, 456);
            this.treeView1.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Smart contract components";
            // 
            // treeView_Checking
            // 
            this.treeView_Checking.Location = new System.Drawing.Point(278, 64);
            this.treeView_Checking.Name = "treeView_Checking";
            this.treeView_Checking.Size = new System.Drawing.Size(265, 456);
            this.treeView_Checking.TabIndex = 8;
            // 
            // richTextBox_errorMassage
            // 
            this.richTextBox_errorMassage.Location = new System.Drawing.Point(9, 21);
            this.richTextBox_errorMassage.Name = "richTextBox_errorMassage";
            this.richTextBox_errorMassage.Size = new System.Drawing.Size(534, 124);
            this.richTextBox_errorMassage.TabIndex = 11;
            this.richTextBox_errorMassage.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(752, 126);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 60);
            this.button1.TabIndex = 12;
            this.button1.Text = "\nParse ====>";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(749, 453);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(77, 60);
            this.button3.TabIndex = 14;
            this.button3.Text = "\nTranslate <====";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.richTextBox_errorMassage);
            this.groupBox1.Location = new System.Drawing.Point(832, 542);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(549, 151);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, -3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Checking results";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1391, 700);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox_checking);
            this.Controls.Add(this.btn_fromGraphical2SC);
            this.Controls.Add(this.btn_SC2Checking);
            this.Controls.Add(this.btn_exportSC);
            this.Controls.Add(this.textBox_SCExportedPath);
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_importGraphical;
        private System.Windows.Forms.Button btn_SC2Checking;
        private System.Windows.Forms.Button btn_exportSC;
        private System.Windows.Forms.TextBox textBox_SCExportedPath;
        private System.Windows.Forms.TextBox textBox_GraphicalImportedPath;
        private System.Windows.Forms.GroupBox groupBox_graphical;
        private System.Windows.Forms.GroupBox groupBox_SC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_fromGraphical2SC;
        private System.Windows.Forms.RichTextBox richTextBox_displayGraphical;
        private System.Windows.Forms.RichTextBox richTextBox_displaySC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox_checking;
        private System.Windows.Forms.TreeView treeView_Checking;
        private System.Windows.Forms.Button btn_importYAWLRoles;
        private System.Windows.Forms.TreeView treeView_displayYAWLRoles;
        private System.Windows.Forms.TreeView treeView_SCfileTree;
        private System.Windows.Forms.RichTextBox richTextBox_errorMassage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
    }
}

