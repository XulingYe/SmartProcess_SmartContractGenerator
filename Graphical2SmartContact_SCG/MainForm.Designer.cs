
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
            this.btn_exportSC = new System.Windows.Forms.Button();
            this.textBox_SCExportedPath = new System.Windows.Forms.TextBox();
            this.textBox_GraphicalImportedPath = new System.Windows.Forms.TextBox();
            this.groupBox_graphicalPM = new System.Windows.Forms.GroupBox();
            this.groupBox_Action = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.listBox_CurrentAction = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.richTextBox_displayGraphical = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox_SCcodes = new System.Windows.Forms.GroupBox();
            this.treeView_SCfileTree = new System.Windows.Forms.TreeView();
            this.richTextBox_displaySC = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox_generatedComponents = new System.Windows.Forms.GroupBox();
            this.btn_translatePC2SCC = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.treeView_PCs = new System.Windows.Forms.TreeView();
            this.label4 = new System.Windows.Forms.Label();
            this.treeView_SCCs = new System.Windows.Forms.TreeView();
            this.richTextBox_errorMassage = new System.Windows.Forms.RichTextBox();
            this.btn_parseXML2PC = new System.Windows.Forms.Button();
            this.btn_translateSCC2SC = new System.Windows.Forms.Button();
            this.groupBox_CheckingResults = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox_graphicalPM.SuspendLayout();
            this.groupBox_Action.SuspendLayout();
            this.groupBox_SCcodes.SuspendLayout();
            this.groupBox_generatedComponents.SuspendLayout();
            this.groupBox_CheckingResults.SuspendLayout();
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
            this.textBox_GraphicalImportedPath.Size = new System.Drawing.Size(612, 22);
            this.textBox_GraphicalImportedPath.TabIndex = 6;
            // 
            // groupBox_graphicalPM
            // 
            this.groupBox_graphicalPM.Controls.Add(this.groupBox_Action);
            this.groupBox_graphicalPM.Controls.Add(this.richTextBox_displayGraphical);
            this.groupBox_graphicalPM.Controls.Add(this.label2);
            this.groupBox_graphicalPM.Controls.Add(this.btn_importGraphical);
            this.groupBox_graphicalPM.Controls.Add(this.textBox_GraphicalImportedPath);
            this.groupBox_graphicalPM.Location = new System.Drawing.Point(6, 7);
            this.groupBox_graphicalPM.Name = "groupBox_graphicalPM";
            this.groupBox_graphicalPM.Size = new System.Drawing.Size(730, 329);
            this.groupBox_graphicalPM.TabIndex = 8;
            this.groupBox_graphicalPM.TabStop = false;
            this.groupBox_graphicalPM.Text = "YAWL";
            // 
            // groupBox_Action
            // 
            this.groupBox_Action.Controls.Add(this.button1);
            this.groupBox_Action.Controls.Add(this.label9);
            this.groupBox_Action.Controls.Add(this.listBox_CurrentAction);
            this.groupBox_Action.Controls.Add(this.label8);
            this.groupBox_Action.Controls.Add(this.label7);
            this.groupBox_Action.Location = new System.Drawing.Point(597, 58);
            this.groupBox_Action.Name = "groupBox_Action";
            this.groupBox_Action.Size = new System.Drawing.Size(127, 265);
            this.groupBox_Action.TabIndex = 9;
            this.groupBox_Action.TabStop = false;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(6, 210);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 50);
            this.button1.TabIndex = 4;
            this.button1.Text = "Add more Action(s)";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 17);
            this.label9.TabIndex = 3;
            this.label9.Text = "Current Actions:";
            // 
            // listBox_CurrentAction
            // 
            this.listBox_CurrentAction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_CurrentAction.FormattingEnabled = true;
            this.listBox_CurrentAction.ItemHeight = 16;
            this.listBox_CurrentAction.Items.AddRange(new object[] {
            "add",
            "pay"});
            this.listBox_CurrentAction.Location = new System.Drawing.Point(6, 118);
            this.listBox_CurrentAction.Name = "listBox_CurrentAction";
            this.listBox_CurrentAction.Size = new System.Drawing.Size(115, 68);
            this.listBox_CurrentAction.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(36, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 17);
            this.label8.TabIndex = 1;
            this.label8.Text = "PayBU";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Selected task:";
            // 
            // richTextBox_displayGraphical
            // 
            this.richTextBox_displayGraphical.Location = new System.Drawing.Point(5, 66);
            this.richTextBox_displayGraphical.Name = "richTextBox_displayGraphical";
            this.richTextBox_displayGraphical.Size = new System.Drawing.Size(586, 257);
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
            // groupBox_SCcodes
            // 
            this.groupBox_SCcodes.Controls.Add(this.treeView_SCfileTree);
            this.groupBox_SCcodes.Controls.Add(this.richTextBox_displaySC);
            this.groupBox_SCcodes.Controls.Add(this.label1);
            this.groupBox_SCcodes.Location = new System.Drawing.Point(6, 346);
            this.groupBox_SCcodes.Name = "groupBox_SCcodes";
            this.groupBox_SCcodes.Size = new System.Drawing.Size(730, 311);
            this.groupBox_SCcodes.TabIndex = 9;
            this.groupBox_SCcodes.TabStop = false;
            this.groupBox_SCcodes.Text = "Solidity";
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
            // groupBox_generatedComponents
            // 
            this.groupBox_generatedComponents.Controls.Add(this.btn_translatePC2SCC);
            this.groupBox_generatedComponents.Controls.Add(this.label5);
            this.groupBox_generatedComponents.Controls.Add(this.treeView_PCs);
            this.groupBox_generatedComponents.Controls.Add(this.label4);
            this.groupBox_generatedComponents.Controls.Add(this.treeView_SCCs);
            this.groupBox_generatedComponents.Controls.Add(this.label3);
            this.groupBox_generatedComponents.Location = new System.Drawing.Point(832, 6);
            this.groupBox_generatedComponents.Name = "groupBox_generatedComponents";
            this.groupBox_generatedComponents.Size = new System.Drawing.Size(549, 530);
            this.groupBox_generatedComponents.TabIndex = 9;
            this.groupBox_generatedComponents.TabStop = false;
            this.groupBox_generatedComponents.Text = "Table";
            // 
            // btn_translatePC2SCC
            // 
            this.btn_translatePC2SCC.Location = new System.Drawing.Point(213, 25);
            this.btn_translatePC2SCC.Name = "btn_translatePC2SCC";
            this.btn_translatePC2SCC.Size = new System.Drawing.Size(130, 35);
            this.btn_translatePC2SCC.TabIndex = 13;
            this.btn_translatePC2SCC.Text = "=> Translate =>";
            this.btn_translatePC2SCC.UseVisualStyleBackColor = true;
            this.btn_translatePC2SCC.Click += new System.EventHandler(this.btn_translatePC2SCC_Click);
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
            // treeView_PCs
            // 
            this.treeView_PCs.Location = new System.Drawing.Point(9, 64);
            this.treeView_PCs.Name = "treeView_PCs";
            this.treeView_PCs.Size = new System.Drawing.Size(261, 456);
            this.treeView_PCs.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Smart contract components";
            // 
            // treeView_SCCs
            // 
            this.treeView_SCCs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView_SCCs.Location = new System.Drawing.Point(278, 64);
            this.treeView_SCCs.Name = "treeView_SCCs";
            this.treeView_SCCs.Size = new System.Drawing.Size(265, 456);
            this.treeView_SCCs.TabIndex = 8;
            // 
            // richTextBox_errorMassage
            // 
            this.richTextBox_errorMassage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_errorMassage.Location = new System.Drawing.Point(9, 20);
            this.richTextBox_errorMassage.Name = "richTextBox_errorMassage";
            this.richTextBox_errorMassage.Size = new System.Drawing.Size(534, 125);
            this.richTextBox_errorMassage.TabIndex = 11;
            this.richTextBox_errorMassage.Text = "";
            // 
            // btn_parseXML2PC
            // 
            this.btn_parseXML2PC.Location = new System.Drawing.Point(742, 126);
            this.btn_parseXML2PC.Name = "btn_parseXML2PC";
            this.btn_parseXML2PC.Size = new System.Drawing.Size(83, 60);
            this.btn_parseXML2PC.TabIndex = 12;
            this.btn_parseXML2PC.Text = "Parse \r\n===>";
            this.btn_parseXML2PC.UseVisualStyleBackColor = true;
            this.btn_parseXML2PC.Click += new System.EventHandler(this.btn_parseXML2PC_Click);
            // 
            // btn_translateSCC2SC
            // 
            this.btn_translateSCC2SC.Location = new System.Drawing.Point(742, 453);
            this.btn_translateSCC2SC.Name = "btn_translateSCC2SC";
            this.btn_translateSCC2SC.Size = new System.Drawing.Size(80, 60);
            this.btn_translateSCC2SC.TabIndex = 14;
            this.btn_translateSCC2SC.Text = "Translate <=====";
            this.btn_translateSCC2SC.UseVisualStyleBackColor = true;
            this.btn_translateSCC2SC.Click += new System.EventHandler(this.btn_translateSCC2SC_Click);
            // 
            // groupBox_CheckingResults
            // 
            this.groupBox_CheckingResults.Controls.Add(this.label6);
            this.groupBox_CheckingResults.Controls.Add(this.richTextBox_errorMassage);
            this.groupBox_CheckingResults.Location = new System.Drawing.Point(832, 542);
            this.groupBox_CheckingResults.Name = "groupBox_CheckingResults";
            this.groupBox_CheckingResults.Size = new System.Drawing.Size(549, 150);
            this.groupBox_CheckingResults.TabIndex = 15;
            this.groupBox_CheckingResults.TabStop = false;
            this.groupBox_CheckingResults.Text = "groupBox1";
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
            this.Controls.Add(this.groupBox_CheckingResults);
            this.Controls.Add(this.btn_translateSCC2SC);
            this.Controls.Add(this.btn_parseXML2PC);
            this.Controls.Add(this.groupBox_generatedComponents);
            this.Controls.Add(this.btn_exportSC);
            this.Controls.Add(this.textBox_SCExportedPath);
            this.Controls.Add(this.groupBox_SCcodes);
            this.Controls.Add(this.groupBox_graphicalPM);
            this.Name = "MainForm";
            this.Text = "Smart Contract Generator";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.groupBox_graphicalPM.ResumeLayout(false);
            this.groupBox_graphicalPM.PerformLayout();
            this.groupBox_Action.ResumeLayout(false);
            this.groupBox_Action.PerformLayout();
            this.groupBox_SCcodes.ResumeLayout(false);
            this.groupBox_SCcodes.PerformLayout();
            this.groupBox_generatedComponents.ResumeLayout(false);
            this.groupBox_generatedComponents.PerformLayout();
            this.groupBox_CheckingResults.ResumeLayout(false);
            this.groupBox_CheckingResults.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_importGraphical;
        private System.Windows.Forms.Button btn_exportSC;
        private System.Windows.Forms.TextBox textBox_SCExportedPath;
        private System.Windows.Forms.TextBox textBox_GraphicalImportedPath;
        private System.Windows.Forms.GroupBox groupBox_graphicalPM;
        private System.Windows.Forms.GroupBox groupBox_SCcodes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox_displayGraphical;
        private System.Windows.Forms.RichTextBox richTextBox_displaySC;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox_generatedComponents;
        private System.Windows.Forms.TreeView treeView_SCCs;
        private System.Windows.Forms.TreeView treeView_SCfileTree;
        private System.Windows.Forms.RichTextBox richTextBox_errorMassage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_translatePC2SCC;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TreeView treeView_PCs;
        private System.Windows.Forms.Button btn_parseXML2PC;
        private System.Windows.Forms.Button btn_translateSCC2SC;
        private System.Windows.Forms.GroupBox groupBox_CheckingResults;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox_Action;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox listBox_CurrentAction;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}

