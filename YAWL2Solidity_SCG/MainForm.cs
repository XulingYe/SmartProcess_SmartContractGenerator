using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Graphical2SmartContact_SCG.SmartContractGenerator;

namespace Graphical2SmartContact_SCG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Auto_Size();
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Auto_Size();
        }

        #region VariablesDefinition
        GraphicalParser graphicalParser = new GraphicalParser();
        SmartContractGenerator solidityGenerator = new SmartContractGenerator();
        #endregion

        #region Graphical representation

        bool isBPMN = false;
        private void btn_importGraphical_Click(object sender, EventArgs e)
        {
            String file = openFileDiag("Browse Graphical Representation Files", "YAWL files (*.yawl)|*.yawl|BPMN files (*.bpmn)|*.bpmn");

            if (file != "")
            {
                Cursor = Cursors.WaitCursor;
                textBox_GraphicalImportedPath.Text = file;
                string text = "";
                try
                {
                    text = File.ReadAllText(file);
                    if (text != "")
                    {
                        richTextBox_displayGraphical.Text = text;
                        string graphicalExtension = Path.GetExtension(file);
                        if (graphicalExtension == ".bpmn")
                        {
                            isBPMN = true;
                            Auto_Size();
                        }
                        else if(graphicalExtension == ".yawl")
                        {
                            isBPMN = false;
                            Auto_Size();
                        }
                        graphicalParser.parseGraphical(text,isBPMN);
                    }
                }
                catch (IOException)
                {
                }
                Cursor = Cursors.Default;
            }
        }

        private void btn_importYAWLRoles_Click(object sender, EventArgs e)
        {
            if (!isBPMN && textBox_GraphicalImportedPath.Text != "")
            {
                String file = openFileDiag("Browse YAWL Roles Files", "YAWL Roles files (*.ybkp)|*.ybkp");

                if (file != "")
                {
                    Cursor = Cursors.WaitCursor;
                    string text = "";
                    try
                    {
                        text = File.ReadAllText(file);
                        if (text != "")
                        {
                            graphicalParser.parseYawlRoles(text);
                            displayYawlRolesTree();
                        }
                    }
                    catch (IOException)
                    {
                    }
                    Cursor = Cursors.Default;
                }
            }
            /*else
            {
                MessageBox.Show("You did not import any graphical representation! Please click \"Import\" first.", "Error in importing YAWL Roles",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/

        }

        private void AutosizeGraphical(int width_yawl, int height_yawl)
        {
            int treeViewYAWLRoles_width = 0;
            int richBoxDisplayYAWL_width = width_yawl - 10;
            if(isBPMN)
            {
                this.btn_importYAWLRoles.Hide();
                this.treeView_displayYAWLRoles.Hide();
            }
            else
            {
                this.btn_importYAWLRoles.Show();
                this.treeView_displayYAWLRoles.Show();
                treeViewYAWLRoles_width = (width_yawl - 15) / 4;
                richBoxDisplayYAWL_width -= (treeViewYAWLRoles_width + 5);

                this.btn_importYAWLRoles.Location = new Point(10 + richBoxDisplayYAWL_width, 25);
                this.btn_importYAWLRoles.Size = new Size(treeViewYAWLRoles_width, 30);

                this.treeView_displayYAWLRoles.Location = new Point(10 + richBoxDisplayYAWL_width, 60);
                this.treeView_displayYAWLRoles.Size = new Size(treeViewYAWLRoles_width, height_yawl - 65);
            }

            this.btn_importGraphical.Location = new Point(5, 25);
            this.btn_importGraphical.Size = new Size(100, 30);

            this.textBox_GraphicalImportedPath.Location = new Point(110, 30);
            this.textBox_GraphicalImportedPath.Size = new Size(richBoxDisplayYAWL_width - 105, 22);

            this.richTextBox_displayGraphical.Location = new Point(5, 60);
            this.richTextBox_displayGraphical.Size = new Size(richBoxDisplayYAWL_width, height_yawl - 65);
            

            
        }
        private void displayYawlRolesTree()
        {
            treeView_displayYAWLRoles.BeginUpdate();
            treeView_displayYAWLRoles.Nodes.Clear();
            //add roles
            var roles_node = treeView_displayYAWLRoles.Nodes.Add("Roles");
            roles_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var role_yawl in graphicalParser.allRoles)
            {
                //add parameters to modifier
                var role_node = roles_node.Nodes.Add(role_yawl.name);
                role_node.NodeFont = new Font("Arial", 9);
                role_node.Nodes.Add("address: "+ role_yawl.address);
                role_node.Nodes.Add("id: " + role_yawl.id);
            }
            treeView_displayYAWLRoles.EndUpdate();
        }
        #endregion

        #region Content checking
        private void btn_fromGraphical2Checking_Click(object sender, EventArgs e)
        {
            if (graphicalParser.fileName != "default" & treeView_displayYAWLRoles.Nodes.Count>0)
            {
                treeView_Checking.BeginUpdate();
                treeView_Checking.Nodes.Clear();
                //add data definition
                /*var definedEnums_node = treeView_table.Nodes.Add("Defined Data Structure");
                definedEnums_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var definedenum in graphicalParser.allDefinedEnums)
                {
                    var definedenum_node = definedEnums_node.Nodes.Add(definedenum.name);
                    definedenum_node.NodeFont = new Font("Arial", 9);
                    for (int i = 0; i< definedenum.elements.Count(); i++)
                    {
                        definedenum_node.Nodes.Add(definedenum.elements[i]);
                    }
                }*/
                //add local variables
                var localVaris_node = treeView_Checking.Nodes.Add("Local Variables");
                localVaris_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var localvari_yawl in graphicalParser.allLocalVariables)
                {
                    var localvari_node = localVaris_node.Nodes.Add(localvari_yawl.name);
                    localvari_node.NodeFont = new Font("Arial", 9);
                    localvari_node.Nodes.Add("Type:" + localvari_yawl.type);
                    localvari_node.Nodes.Add("Value:" + localvari_yawl.defaultVaule);
                }
                //add modifiers and flows
                var roles_node = treeView_Checking.Nodes.Add("Roles");
                roles_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var role_yawl in graphicalParser.allRoles)
                {
                    //add parameters to modifier
                    var role_node = roles_node.Nodes.Add(role_yawl.name);
                    role_node.NodeFont = new Font("Arial", 9);
                    role_node.Nodes.Add("address: " + role_yawl.address);
                    role_node.Nodes.Add("id: " + role_yawl.id);
                    var strRoleTasksName = "tasks: ";
                    for(int i = 0; i < role_yawl.functionNames.Count; i++)
                    {
                        if(i > 0) 
                        { 
                            strRoleTasksName += ", "; 
                        }
                        strRoleTasksName += role_yawl.functionNames[i];
                    }
                    role_node.Nodes.Add(strRoleTasksName);
                }
                /*var modifiers_node = treeView_table.Nodes.Add("Modifiers");
                modifiers_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var modifier_yawl in graphicalParser.allModifiers)
                {
                    //add parameters to modifier
                    string temp_modifier_nameWithVariables = modifier_yawl.name+"(";
                    for(int i =0; i< modifier_yawl.inputVaris.Count;i++)
                    {
                        if(i>0)
                        {
                            temp_modifier_nameWithVariables += ",";
                        }
                        temp_modifier_nameWithVariables += modifier_yawl.inputVaris[i].name;
                    }
                    temp_modifier_nameWithVariables += ")";

                    var modifier_node = modifiers_node.Nodes.Add(temp_modifier_nameWithVariables);
                    modifier_node.NodeFont = new Font("Arial", 9);
                    modifier_node.Nodes.Add(modifier_yawl.condition);
                    modifier_node.Nodes.Add(modifier_yawl.errorString);
                }*/
                //add flows
                var flows_node = treeView_Checking.Nodes.Add("Process Flows");
                flows_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var flow in graphicalParser.allFlows)
                {
                    var flow_node = flows_node.Nodes.Add(flow.currentProcessName);
                    flow_node.NodeFont = new Font("Arial", 9);
                    if (flow.nextProcesses.Count > 0)
                    {
                        foreach (var nextProcess in flow.nextProcesses)
                        {
                            var nextProcess_node = flow_node.Nodes.Add("Next process: " + nextProcess.processName);
                            if (nextProcess.condition != null)
                            {
                                nextProcess_node.Nodes.Add(nextProcess.condition);
                            }
                        }
                    }
                    if (flow.splitOperation != null)
                    {
                        flow_node.Nodes.Add(flow.splitOperation);
                    }
                    if(flow.currentProcessRoles.Count>0)
                    {
                        foreach(var role in flow.currentProcessRoles)
                        {
                            var role_node = flow_node.Nodes.Add("Role: " + role.name);
                            role_node.Nodes.Add("address: " + role.address);
                            role_node.Nodes.Add("id: " + role.id);
                        }
                    }
                }

                //add functions
                var functions_node = treeView_Checking.Nodes.Add("Tasks(Functions)");
                functions_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                foreach (var function in graphicalParser.allFunctions)
                {
                    var function_node = functions_node.Nodes.Add(function.name);
                    function_node.NodeFont = new Font("Arial", 9);
                    //modifiers
                    //var funmodifiers_node = function_node.Nodes.Add("Role");
                    /*foreach (var funmodifier in function.modifiers)
                    {
                        string funmodi_values = funmodifier.name + "(";
                        for (int i = 0; i < funmodifier.inputVaris.Count(); i++)
                        {
                            if (i > 0)
                            {
                                funmodi_values += ", ";
                            }
                            funmodi_values += funmodifier.inputVaris[i].defaultVaule;
                        }
                        funmodi_values += ")";
                        funmodifiers_node.Nodes.Add(funmodi_values);
                    }*/
                    //Roles
                    var funRoles_node = function_node.Nodes.Add("Roles"); ;
                    foreach (var funRole in function.nextProcess.currentProcessRoles)
                    {
                        var role_node = funRoles_node.Nodes.Add("Role: " + funRole.name);
                        role_node.Nodes.Add("address: " + funRole.address);
                        role_node.Nodes.Add("id: " + funRole.id);
                    }
                    //inputs
                    var funinputs_node = function_node.Nodes.Add("Input variables");
                    foreach (var funinput in function.inputVariables)
                    {
                        funinputs_node.Nodes.Add(funinput.name);
                    }
                    //outputs
                    var funonputs_node = function_node.Nodes.Add("Output variables");
                    foreach (var funonput in function.outputVariables)
                    {
                        funonputs_node.Nodes.Add(funonput.name);
                    }
                    //inOutputs
                    /*var funinoutputs_node = function_node.Nodes.Add("In/output variables");
                    foreach (var funinoutput in function.inOutVariables)
                    {
                        funinoutputs_node.Nodes.Add(funinoutput.name);
                    }*/
                }
                treeView_Checking.EndUpdate();
            }
        }
        private void btn_fromChecking2SC_Click(object sender, EventArgs e)
        {
            if(graphicalParser.fileName!="default")
            {
                treeView_SCfileTree.BeginUpdate();
                treeView_SCfileTree.Nodes.Clear();
                var contracts_node = treeView_SCfileTree.Nodes.Add("contracts");
                solidityGenerator.generateSolidityText(graphicalParser);
                foreach(var scFile in solidityGenerator.allSolidityFiles)
                {
                    contracts_node.Nodes.Add(scFile.contractName);
                }
                treeView_SCfileTree.EndUpdate();
            }
            

            
        }
        private void AutosizeChecking(int width_table, int height_table)
        {
            int treeviewTable_width = width_table - 10;//(2*width_table) / 3 - 10;
            this.treeView_Checking.Location = new Point(5, 25);
            this.treeView_Checking.Size = new Size(treeviewTable_width, height_table - 30);

            //this.richTextBox_displayTable.Location = new Point(treeviewTable_width + 15, 25);
            //this.richTextBox_displayTable.Size = new Size(width_table - treeviewTable_width - 20, height_table - 30);
        }
        #endregion

        #region Smart contract language
        private void btn_exportSolidity_Click(object sender, EventArgs e)
        {
            if (treeView_SCfileTree.SelectedNode != null)
            {
                if(treeView_SCfileTree.SelectedNode.Text != "contracts")
                {
                    foreach (var scFile in solidityGenerator.allSolidityFiles)
                    {
                        if (treeView_SCfileTree.SelectedNode.Text == scFile.contractName)
                        {
                            saveFileDiag("Save as an Solidity File", "Solidity File | *.sol", scFile);
                        }
                    }
                }
                else
                {
                    using (var fbd = new FolderBrowserDialog())
                    {
                        DialogResult result = fbd.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                        {
                            //new a folder name "contracts"
                            var newFolder = fbd.SelectedPath + "\\contracts";
                            //MessageBox.Show("fbd.SelectedPath: " + newFolder, "Message");
                            if(!Directory.Exists(newFolder))
                            {
                                Directory.CreateDirectory(newFolder);
                                foreach(var file in solidityGenerator.allSolidityFiles)
                                {
                                    var fileName = file.contractName + ".sol";
                                    var filePath = newFolder + "\\" + fileName;
                                    writeFile(filePath, file);
                                }
                                textBox_SCExportedPath.Text = newFolder;
                            }
                            else
                            {
                                MessageBox.Show("The folder: " + newFolder + " exists.", "Error in exporting", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            /*string[] files = Directory.GetFiles();

                            */
                        }
                    }
                } 
            }
            
        }
        private void AutosizeSC(int width_solidity, int height_solidity)
        {
            int treeViewSCfolder_width = width_solidity / 7;
            this.treeView_SCfileTree.Location = new Point(5, 25);
            this.treeView_SCfileTree.Size = new Size(treeViewSCfolder_width, height_solidity - 65);

            this.richTextBox_displaySC.Location = new Point(10 + treeViewSCfolder_width, 25);
            this.richTextBox_displaySC.Size = new Size(width_solidity - treeViewSCfolder_width - 15, height_solidity - 65);

            this.btn_exportSC.Location = new Point(5, height_solidity - 35);
            this.btn_exportSC.Size = new Size(110, 30);

            this.textBox_SCExportedPath.Location = new Point(120, height_solidity - 30);
            this.textBox_SCExportedPath.Size = new Size(width_solidity - 125, 22);
        }

        private void treeView_SCfileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Text == "contracts")
            {
                richTextBox_displaySC.Text = "You can export the contracts folder with:\n";
                foreach(TreeNode childNode in e.Node.Nodes)
                {
                    richTextBox_displaySC.Text += " -" + childNode.Text + ".sol\n";
                }
            }
            else
            {
                foreach(var fileNode in solidityGenerator.allSolidityFiles)
                {
                    if(e.Node.Text==fileNode.contractName)
                    {
                        richTextBox_displaySC.Text = fileNode.fileAllText;
                        return;
                    }
                }
            }
            
        }

        #endregion

        #region generalFunctions
        private string openFileDiag(string title, string Filter)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = title,

                CheckFileExists = true,
                CheckPathExists = true,

                Filter = Filter,
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                return openFileDialog1.FileName;
            }
            else
            {
                return "";
            }
        }

        private void saveFileDiag(string title, string Filter, SolidityFile solidityFile)
        {
            // Displays a SaveFileDialog so the user can save the .sol file
            // assigned to btn_exportSolidity.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = title;// "Save as an Solidity File";
            saveFileDialog1.Filter = Filter;// "Solidity File|*.sol";
            saveFileDialog1.FileName = solidityFile.contractName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                textBox_SCExportedPath.Text = filePath;
                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                writer.Write(solidityFile.fileAllText);
                writer.Dispose();
                writer.Close();
            }
            else
            {
                textBox_SCExportedPath.Text = "";
            }
        }

        private void writeFile(string filePath, SolidityFile solidityFile)
        {
            StreamWriter writer = new StreamWriter(filePath);
            writer.Write(solidityFile.fileAllText);
            writer.Dispose();
            writer.Close();
        }

        private void Auto_Size()
        {
            //int groupBoxYAWL_width = (2*this.ClientSize.Width)/3 - 120;
            int groupBoxGraphical_width = (3 * this.ClientSize.Width) / 4 - 135;
            int groupBoxGraphical_height = (this.ClientSize.Height - 10) / 2;
            this.groupBox_graphical.Location = new Point(5, 0);
            this.groupBox_graphical.Size = new Size(groupBoxGraphical_width, groupBoxGraphical_height);
            AutosizeGraphical(groupBoxGraphical_width, groupBoxGraphical_height);

            int groupBoxSolidity_height = this.ClientSize.Height - groupBoxGraphical_height - 10;
            this.groupBox_SC.Location = new Point(5, groupBoxGraphical_height + 10);
            this.groupBox_SC.Size = new Size(groupBoxGraphical_width, groupBoxSolidity_height);
            AutosizeSC(groupBoxGraphical_width, groupBoxSolidity_height);

            int groupBoxTable_width = groupBoxGraphical_width / 3;
            int groupBoxTable_height = this.ClientSize.Height - 10;
            int groupBoxTable_x = this.ClientSize.Width - groupBoxTable_width - 5;
            this.groupBox_checking.Location = new Point(groupBoxTable_x, 5);
            this.groupBox_checking.Size = new Size(groupBoxTable_width, groupBoxTable_height);
            AutosizeChecking(groupBoxTable_width, groupBoxTable_height);

            //left button
            int btnFromYAWL2Table_y = groupBoxGraphical_height / 2 - 15;
            this.btn_fromGraphical2Checking.Location = new Point(groupBoxGraphical_width + 10, btnFromYAWL2Table_y);
            this.btn_fromGraphical2Checking.Size = new Size(160, 80);

            //right button
            int btnFromTable2Solidity = this.ClientSize.Height - (groupBoxSolidity_height / 2) - 85;
            this.btn_fromChecking2SC.Location = new Point(groupBoxGraphical_width + 10, btnFromTable2Solidity);
            this.btn_fromChecking2SC.Size = new Size(160, 80);

            //Arrows
            // (x1,y1)                                              (x6,y1)
            //   |                                                     /\
            //  \/                                                      |
            // (x1,y2)                                              (x6,y2)
            //         (x2,y3) -> (x3,y3)        (x4,y3) -> (x5,y3)
            /*int x1 = btnFromYAWL2Table_x + 75;
            int x2 = btnFromYAWL2Table_x + 150;
            int x3 = groupBoxTable_x;
            int x4 = groupBoxTable_x + groupBoxTable_width;
            int x5 = btnFromTable2Solidity_x;
            int x6 = btnFromTable2Solidity_x + 75;
            int y1 = groupBoxYAWL_height;
            int y2 = btnFromYAWL2Table_y;
            int y3 = btnFromYAWL2Table_y + 40;
            this.Paint += delegate (object s2, PaintEventArgs e2)
            {
                this.MainForm_Paint(s2, e2, x1, x2, x3, x4, x5, x6, y1, y2, y3);
            };*/
        }

        /*private void MainForm_Paint(object sender, PaintEventArgs e, int x1, int x2, int x3, int x4, int x5, int x6,
            int y1, int y2, int y3)
        {
            // (x1,y1)                                              (x6,y1)
            //   |                                                     /\
            //  \/                                                      |
            // (x1,y2)                                              (x6,y2)
            //         (x2,y3) -> (x3,y3)        (x4,y3) -> (x5,y3)
            
            //draw arrows
            Graphics g = CreateGraphics();
            Pen p = new Pen(Brushes.DeepSkyBlue, 20);
            p.StartCap = LineCap.ArrowAnchor;
            g.Clear(Color.WhiteSmoke);
            g.DrawLine(p, x1, y2, x1, y1);
            g.DrawLine(p, x3, y3, x2, y3);
            g.DrawLine(p, x5, y3, x4, y3);
            g.DrawLine(p, x6, y1, x6, y2);
            

            //g.DrawLine(p, 340, 450, 200, 320); //(x2,y2)<-(x1,y1)
            //2*340-200+660
            //g.DrawLine(p, 1140, 320, 1002, 450); //(2*x2-x1+TableWidth,y1)<-(x2+TableWidth,y2)
            //g.DrawLine(System.Drawing.Pens.Red, pictureBox1.Left, pictureBox1.Top, pictureBox1.Right, pictureBox1.Bottom);

        }*/

        #endregion

    }
}
