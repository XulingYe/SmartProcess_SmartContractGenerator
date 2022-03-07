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
using static Graphical2SmartContact_SCG.SCGTranslator;
using static Graphical2SmartContact_SCG.SmartContractComponents;

namespace Graphical2SmartContact_SCG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Auto_Size();
            /*char[] testTX = "532EAABD9574880DBF76B9B8CC00832C20A6EC113D682299550D7A6E0F345E25".ToCharArray();
            string reverse = String.Empty;
            for (int i = testTX.Length - 1; i > -1; i--)
            {
                reverse += testTX[i];
            }
            richTextBox_displayGraphical.Text = reverse;*/
        }
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Auto_Size();
        }

        #region VariablesDefinition
        ProcessComponents processComponents = new ProcessComponents();
        SmartContractComponents contractComponents = new SmartContractComponents();
        SCGParser parser = new SCGParser();
        SCGTranslator translator = new SCGTranslator();
        SCGChecker checker = new SCGChecker();
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
                        string fileName = Path.GetFileNameWithoutExtension(file);
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
                            treeView_displayYAWLRoles.Nodes.Clear();
                        }
                        treeView_SCCs.Nodes.Clear();
                        treeView_SCfileTree.Nodes.Clear();
                        richTextBox_displaySC.Text = "";
                        textBox_SCExportedPath.Text = "";
                        parser.parseGraphical(text,isBPMN,fileName, processComponents);
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
                            parser.parseYawlRoles(text,processComponents);
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

        private void displayYawlRolesTree()
        {
            treeView_displayYAWLRoles.BeginUpdate();
            treeView_displayYAWLRoles.Nodes.Clear();
            //add roles
            var roles_node = treeView_displayYAWLRoles.Nodes.Add("Roles");
            roles_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var role_yawl in processComponents.allRoles)
            {
                //add parameters to modifier
                var role_node = roles_node.Nodes.Add(role_yawl.name);
                role_node.NodeFont = new Font("Arial", 9);
                role_node.Nodes.Add("address: "+ role_yawl.address);
                role_node.Nodes.Add("id: " + role_yawl.id);
                /*foreach(var actype in role_yawl.actionTypes)
                {
                    role_node.Nodes.Add("action: " + actype);
                }*/
                
            }
            treeView_displayYAWLRoles.EndUpdate();
        }
        #endregion

        #region Smart contract language
        string strContractsFolderName = "contracts_";
        private void btn_exportSolidity_Click(object sender, EventArgs e)
        {
            //export the whole folder
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //new a folder name "contracts"
                    var newFolder = fbd.SelectedPath + "\\"+ strContractsFolderName;
                    //MessageBox.Show("fbd.SelectedPath: " + newFolder, "Message");
                    if (!Directory.Exists(newFolder))
                    {
                        Directory.CreateDirectory(newFolder);
                        foreach (var file in contractComponents.allSolidityFiles)
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
            /*if (treeView_SCfileTree.SelectedNode != null)
            {
                if(!treeView_SCfileTree.SelectedNode.Text.Contains("contracts_"))
                {
                    foreach (var scFile in solidityTranslator.allSolidityFiles)
                    {
                        if (treeView_SCfileTree.SelectedNode.Text == scFile.contractName)
                        {
                            saveFileDiag("Save as an Solidity File", "Solidity File | *.sol", scFile);
                        }
                    }
                }
                else
                {
                } 
            }*/

        }
        private void treeView_SCfileTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Node.Text.Contains("contracts_"))
            {
                richTextBox_displaySC.Text = "You can export the contracts folder with:\n";
                foreach(TreeNode childNode in e.Node.Nodes)
                {
                    richTextBox_displaySC.Text += " -" + childNode.Text + ".sol\n";
                }
            }
            else
            {
                foreach(var fileNode in contractComponents.allSolidityFiles)
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

        #region AutoSize
        private void Auto_Size()
        {
            //graphical process model group box
            int groupBoxGraphical_width = (7 * this.ClientSize.Width - 700) / 12;
            int groupBoxGraphical_height = (this.ClientSize.Height - 50) / 2;
            this.groupBox_graphicalPM.Location = new Point(5, 5);
            this.groupBox_graphicalPM.Size = new Size(groupBoxGraphical_width, groupBoxGraphical_height);
            AutosizeGraphical(groupBoxGraphical_width, groupBoxGraphical_height);

            //smart contract codes group box
            int groupBoxSC_width = groupBoxGraphical_width;
            int groupBoxSC_height = groupBoxGraphical_height;
            this.groupBox_SCcodes.Location = new Point(5, groupBoxGraphical_height + 10);
            this.groupBox_SCcodes.Size = new Size(groupBoxSC_width, groupBoxSC_height);
            AutosizeSC(groupBoxSC_width, groupBoxSC_height);

            //btn & textBox: export smart contract
            this.btn_exportSC.Location = new Point(5, this.ClientSize.Height-35);
            this.btn_exportSC.Size = new Size(180, 30);
            this.textBox_SCExportedPath.Location = new Point(190, this.ClientSize.Height - 31);
            this.textBox_SCExportedPath.Size = new Size(groupBoxGraphical_width - 190, 22);

            //btn: from graphical to process components
            int btn_parseXML2PC_x = groupBoxSC_width + 10;
            int btn_parseXML2PC_y = groupBoxGraphical_height/2+5;
            this.btn_parseXML2PC.Location = new Point(btn_parseXML2PC_x, btn_parseXML2PC_y);
            this.btn_parseXML2PC.Size = new Size(80, 60);

            //btn: from SCCs to SC codes
            int btn_translateSCC2SC_x = groupBoxSC_width + 10;
            int btn_translateSCC2SC_y = groupBoxGraphical_height + groupBoxSC_height/2 -50;
            this.btn_translateSCC2SC.Location = new Point(btn_translateSCC2SC_x, btn_translateSCC2SC_y);
            this.btn_translateSCC2SC.Size = new Size(80, 60);

            //generated components group box
            int groupBox_GCs_width = (5* groupBoxGraphical_width)/7;
            int groupBox_GCs_height = this.ClientSize.Height - 165;
            int groupBox_GCs_x = this.ClientSize.Width - groupBox_GCs_width - 5;
            this.groupBox_generatedComponents.Location = new Point(groupBox_GCs_x, 5);
            this.groupBox_generatedComponents.Size = new Size(groupBox_GCs_width, groupBox_GCs_height);
            AutosizeGeneratedComponents(groupBox_GCs_width, groupBox_GCs_height);

            //checking results group box
            this.groupBox_CheckingResults.Location = new Point(groupBox_GCs_x, groupBox_GCs_height + 10);
            this.groupBox_CheckingResults.Size = new Size(groupBox_GCs_width, 150);
            this.richTextBox_errorMassage.Location = new Point(5, 20);
            this.richTextBox_errorMassage.Size = new Size(groupBox_GCs_width - 10, 125);

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

        private void AutosizeGraphical(int width_yawl, int height_yawl)
        {
            int treeViewYAWLRoles_width = 0;
            int richBoxDisplayYAWL_width = width_yawl - 10;
            if (isBPMN)
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

        private void AutosizeSC(int width_solidity, int height_solidity)
        {
            int treeViewSCfolder_width = width_solidity / 7;
            this.treeView_SCfileTree.Location = new Point(5, 25);
            this.treeView_SCfileTree.Size = new Size(treeViewSCfolder_width, height_solidity - 30);

            this.richTextBox_displaySC.Location = new Point(10 + treeViewSCfolder_width, 25);
            this.richTextBox_displaySC.Size = new Size(width_solidity - treeViewSCfolder_width - 15, height_solidity - 30);
        }

        private void AutosizeGeneratedComponents(int width_GCs, int height_GCs)
        {
            this.btn_translatePC2SCC.Size = new Size(130, 35);
            this.btn_translatePC2SCC.Location = new Point(width_GCs / 2 - 65, 20);

            int treeviewPCs_width = (width_GCs - 15) / 2;//(2*width_table) / 3 - 10;
            this.treeView_PCs.Location = new Point(5, 60);
            this.treeView_PCs.Size = new Size(treeviewPCs_width, height_GCs - 65);

            this.treeView_SCCs.Location = new Point(treeviewPCs_width + 10, 60);
            this.treeView_SCCs.Size = this.treeView_PCs.Size;


            //this.richTextBox_displayTable.Location = new Point(treeviewTable_width + 15, 25);
            //this.richTextBox_displayTable.Size = new Size(width_table - treeviewTable_width - 20, height_table - 30);
        }



        #endregion






        private void btn_parseXML2PC_Click(object sender, EventArgs e)
        {
            if (treeView_displayYAWLRoles.Nodes.Count > 0 || (isBPMN && richTextBox_displayGraphical.Text!=null))
            {
                // from xml to process components

                treeView_PCs.BeginUpdate();
                treeView_PCs.Nodes.Clear();
                //Step 1: generate PC tree table
                var PCs_node = treeView_PCs.Nodes.Add("Process components");
                PCs_node.NodeFont = new Font("Arial", 9.5f, FontStyle.Bold);
                
                var fileName_node = PCs_node.Nodes.Add("File name");
                fileName_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                fileName_node.Nodes.Add(processComponents.fileName);
                
                //defined enums
                if (processComponents.allDefinedEnums.Count > 0)
                {
                    var definedEnums_node = PCs_node.Nodes.Add("Defined enums");
                    definedEnums_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var definedEnum in processComponents.allDefinedEnums)
                    {
                        var definedEnum_node = definedEnums_node.Nodes.Add("name: "+definedEnum.enumName);
                        foreach(var definedEnumValue in definedEnum.enumValues)
                        {
                            definedEnum_node.Nodes.Add("value: "+ definedEnumValue);
                        }
                        
                    }
                }
                //local variables
                if (processComponents.allLocalVariables.Count > 0)
                {
                    var variables_node = PCs_node.Nodes.Add("Local variables");
                    variables_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var variableTemp in processComponents.allLocalVariables)
                    {
                        var variable_node = variables_node.Nodes.Add(variableTemp.name);
                        variable_node.Nodes.Add("type: " + variableTemp.type);
                        if (variableTemp.value != null)
                        {
                            variable_node.Nodes.Add("value: " + variableTemp.value);
                        }
                    }
                }

                //roles or participants
                if (processComponents.allRoles.Count > 0)
                {
                    var roles_node = PCs_node.Nodes.Add("Roles");
                    roles_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var eachRole in processComponents.allRoles)
                    {
                        var role_node = roles_node.Nodes.Add(eachRole.name);
                        if (eachRole.id != null) { role_node.Nodes.Add("id: " + eachRole.id); }
                        if (eachRole.address != null) { role_node.Nodes.Add("address: " + eachRole.address); }
                        if (eachRole.bpmnProcessName != null) { role_node.Nodes.Add("Process name: " + eachRole.bpmnProcessName); }

                        if (eachRole.actionTypes.Count > 0)
                        {
                            var actionTypes_node = role_node.Nodes.Add("Action types");
                            foreach (var actionType in eachRole.actionTypes)
                            {
                                actionTypes_node.Nodes.Add(actionType);
                            }
                        }
                        if (eachRole.TaskIDs.Count > 0)
                        {
                            var functionNames_node = role_node.Nodes.Add("Task names");
                            foreach (var functionName in eachRole.TaskIDs)
                            {
                                functionNames_node.Nodes.Add(functionName);
                            }
                        }
                    }
                }
                //Task´or activity
                if (processComponents.allTasks.Count > 0)
                {
                    var tasks_node = PCs_node.Nodes.Add("Tasks");
                    tasks_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var eachTask in processComponents.allTasks)
                    {
                        var task_node = tasks_node.Nodes.Add(eachTask.taskID);
                        if(eachTask.taskName!=null)
                        {
                            task_node.Nodes.Add("name: " + eachTask.taskName);
                        }
                        if (eachTask.operateRoles.Count > 0)
                        {
                            var currentRoles_node = task_node.Nodes.Add("Role(s)");
                            foreach (var processRole in eachTask.operateRoles)
                            {
                                currentRoles_node.Nodes.Add(processRole.name);
                            }
                        }
                        if (eachTask.actionType!=null)
                        {
                            task_node.Nodes.Add("Action type: " + eachTask.actionType);
                        }
                        else
                        {
                            //TODO: Checker! Print out error message!!!!
                        }

                        if (eachTask.inputVariables.Count > 0)
                        {
                            var inputVari_node = task_node.Nodes.Add("Input variables");
                            foreach (var inputVari in eachTask.inputVariables)
                            {
                                inputVari_node.Nodes.Add(inputVari.type + " " + inputVari.name);
                            }
                        }
                        if (eachTask.outputVariables.Count > 0)
                        {
                            var outputVari_node = task_node.Nodes.Add("Output variables");
                            foreach (var outputVari in eachTask.outputVariables)
                            {
                                outputVari_node.Nodes.Add(outputVari.type + " " + outputVari.name);
                            }
                        }
                    }
                }
                //process flow
                if (processComponents.allFlows.Count > 0)
                {
                    var flows_node = PCs_node.Nodes.Add("Flows");
                    flows_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var eachFlow in processComponents.allFlows)
                    {
                        var flow_node = flows_node.Nodes.Add(eachFlow.currentTaskID);
                        if(eachFlow.nextTasks.Count>0)
                        {
                            var nextProcesses_node = flow_node.Nodes.Add("Next task(s)");
                            foreach(var nextProcess in eachFlow.nextTasks)
                            {
                                var nextProcess_node = nextProcesses_node.Nodes.Add(nextProcess.taskID);
                                if(nextProcess.condition!=null)
                                {
                                    nextProcess_node.Nodes.Add("condition: " + nextProcess.condition);
                                }
                                
                            }
                        }
                    }
                }
                treeView_PCs.EndUpdate();

                //step 2: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                checker.errorMessages = "";
            }

            
        }

        private void btn_translatePC2SCC_Click(object sender, EventArgs e)
        {

            //TODO: check if process components are exist!

            if (treeView_PCs.Nodes.Count > 0)
            {
                // from process components to smart contract components

                treeView_SCCs.BeginUpdate();
                treeView_SCCs.Nodes.Clear();
                //Step 1: translate form PCs to SCCs
                translator.generateSolidityText(processComponents, contractComponents, checker);
                //Step 2: generate SC tree table
                var contracts_node = treeView_SCCs.Nodes.Add("contracts");
                contracts_node.NodeFont = new Font("Arial", 9);
                foreach (var contractTemp in contractComponents.allSolidityFiles)
                {
                    var contract_node = contracts_node.Nodes.Add(contractTemp.contractName);
                    contract_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
                    //parent contracts
                    if (contractTemp.parentContracts.Count > 0)
                    {
                        var parentContracts_node = contract_node.Nodes.Add("parent contract(s)");
                        parentContracts_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var parentC in contractTemp.parentContracts)
                        {
                            parentContracts_node.Nodes.Add(parentC);
                        }
                    }
                    //state variables
                    if (contractTemp.stateVariables.Count > 0)
                    {
                        var variables_node = contract_node.Nodes.Add("state variables");
                        variables_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var variableTemp in contractTemp.stateVariables)
                        {
                            var variable_node = variables_node.Nodes.Add(variableTemp.name);
                            variable_node.Nodes.Add("type:" + variableTemp.type);
                            if (variableTemp.value != null)
                            {
                                variable_node.Nodes.Add("value:" + variableTemp.value);
                            }
                        }
                    }
                    //modifiers
                    if (contractTemp.modifiers.Count > 0)
                    {
                        var modifiers_node = contract_node.Nodes.Add("modifiers");
                        modifiers_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var modifierTemp in contractTemp.modifiers)
                        {
                            var modifier_node = modifiers_node.Nodes.Add(modifierTemp.name);
                            if (modifierTemp.inputParam.Count > 0)
                            {
                                var modPara_node = modifier_node.Nodes.Add("input parameters");
                                foreach (var modPara in modifierTemp.inputParam)
                                {
                                    modPara_node.Nodes.Add(modPara.type + " " + modPara.name);
                                }
                            }
                            if (modifierTemp.statementsText != null)
                            {
                                var modstate = modifier_node.Nodes.Add("statements");
                                modstate.Nodes.Add(modifierTemp.statementsText);
                            }
                        }
                    }
                    //functions
                    if (contractTemp.functions.Count > 0)
                    {
                        var functions_node = contract_node.Nodes.Add("functions");
                        functions_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var functionTemp in contractTemp.functions)
                        {
                            var function_node = functions_node.Nodes.Add(functionTemp.name);
                            if (functionTemp.inputParam.Count > 0)
                            {
                                var funPara_node = function_node.Nodes.Add("input parameters");
                                foreach (var funPara in functionTemp.inputParam)
                                {
                                    funPara_node.Nodes.Add(funPara.type + " " + funPara.name);
                                }
                            }
                            if (functionTemp.calledModifiers.Count > 0)
                            {
                                var funmodi_node = function_node.Nodes.Add("called modifiers");
                                foreach (var funmodi in functionTemp.calledModifiers)
                                {
                                    funmodi_node.Nodes.Add(funmodi);
                                }
                            }
                            if (functionTemp.keywords.Count > 0)
                            {
                                var funkey_node = function_node.Nodes.Add("keywords");
                                foreach (var funkey in functionTemp.keywords)
                                {
                                    funkey_node.Nodes.Add(funkey);
                                }
                            }
                            if (functionTemp.returnVaris.Count > 0)
                            {
                                var funretV_node = function_node.Nodes.Add("return variables");
                                foreach (var funretV in functionTemp.returnVaris)
                                {
                                    funretV_node.Nodes.Add(funretV.type + " " + funretV.name);
                                }
                            }
                            if (functionTemp.statementsText != null)
                            {
                                var funstate = function_node.Nodes.Add("statements");
                                funstate.Nodes.Add(functionTemp.statementsText);
                            }
                        }
                    }
                    //enums
                    if (contractTemp.enums.Count > 0)
                    {
                        var enums_node = contract_node.Nodes.Add("enums");
                        enums_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var enumTemp in contractTemp.enums)
                        {
                            var enum_node = enums_node.Nodes.Add(enumTemp.enumName);
                            foreach (var enumValueTemp in enumTemp.enumValues)
                            {
                                enum_node.Nodes.Add(enumValueTemp);
                            }
                        }
                    }
                }
                treeView_SCCs.EndUpdate();


                //step 3: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                checker.errorMessages = "";
            }
        }

        private void btn_translateSCC2SC_Click(object sender, EventArgs e)
        {
            treeView_SCfileTree.BeginUpdate();
            treeView_SCfileTree.Nodes.Clear();
            if(treeView_SCCs.Nodes.Count>0)
            { 
                //Automated generate contract folder name
                if (strContractsFolderName == "contracts_")
                {
                    strContractsFolderName += Guid.NewGuid().ToString().GetHashCode().ToString("x");
                }
                var contracts_node = treeView_SCfileTree.Nodes.Add(strContractsFolderName);

                foreach (var scFile in contractComponents.allSolidityFiles)
                {
                    contracts_node.Nodes.Add(scFile.contractName);
                }
                
                //step 2: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                checker.errorMessages = "";
            }
            treeView_SCfileTree.EndUpdate();

            
        }
    }
}
