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
using BPMN;
using System.Drawing.Imaging;

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
        string allText = "";
        string fileName = "";
        ProcessComponents processComponents = new ProcessComponents();
        SmartContractComponents contractComponents = new SmartContractComponents();
        SCGParser parser = new SCGParser();
        SCGTranslator translator = new SCGTranslator();
        SCGChecker checker = new SCGChecker();

        //Model bpmn_model;
        //private UserControl _bpmnControl;
        #endregion

        #region Graphical representation

        private void btn_importGraphical_Click(object sender, EventArgs e)
        {
            String file = openFileDiag("Browse Graphical Representation Files", "YAWL files (*.yawl)|*.yawl|BPMN files (*.bpmn)|*.bpmn");

            if (file != "")
            {
                Cursor = Cursors.WaitCursor;
                textBox_GraphicalImportedPath.Text = file;
                try
                {
                    allText = File.ReadAllText(file);
                    if (allText != "")
                    {
                        richTextBox_displayGraphical.Text = allText;
                        fileName = Path.GetFileNameWithoutExtension(file);
                        string graphicalExtension = Path.GetExtension(file);
                        
                        //if (graphicalExtension == ".bpmn")
                        //{
                        Auto_Size();
                        //
                        
                        treeView_SCCs.Nodes.Clear();
                        treeView_SCfileTree.Nodes.Clear();
                        richTextBox_displaySC.Text = "";
                        textBox_SCExportedPath.Text = "";

                        //bpmnImageShow(fileName);
                    }
                }
                catch (IOException)
                {
                }
                Cursor = Cursors.Default;
            }
        }
        /*private void bpmnImageShow(string fileName)
        {
            MessageBox.Show("start:", "image",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            bpmn_model = Model.Read(fileName);
            MessageBox.Show("read:", "image",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            //bpmn_viewer = bpmn_model.Diagrams.FirstOrDefault();
            Image img = bpmn_model.GetImage(0, 2.0f);
            MessageBox.Show("get image:", "image",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            bpmn_image.Image = img;
            MessageBox.Show("Display:", "image",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            img.Save("B.2.0.png", ImageFormat.Png);
            MessageBox.Show("save:", "image",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
        }*/

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
                        foreach (var file in contractComponents.allSmartContracts)
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
                    foreach (var scFile in solidityTranslator.allSmartContracts)
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
                foreach(var fileNode in contractComponents.allSmartContracts)
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

        private void saveFileDiag(string title, string Filter, SmartContract solidityFile)
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

        private void writeFile(string filePath, SmartContract solidityFile)
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

        private void AutosizeGraphical(int width_bpmn, int height_bpmn)
        {
            int richBoxDisplayBPMN_width = ((width_bpmn-10)*4)/5;
            int action_group_width = width_bpmn - richBoxDisplayBPMN_width - 13;

            this.btn_importGraphical.Location = new Point(5, 25);
            this.btn_importGraphical.Size = new Size(100, 30);

            this.textBox_GraphicalImportedPath.Location = new Point(110, 30);
            this.textBox_GraphicalImportedPath.Size = new Size(width_bpmn - 115, 22);

            this.richTextBox_displayGraphical.Location = new Point(5, 60);
            this.richTextBox_displayGraphical.Size = new Size(richBoxDisplayBPMN_width, height_bpmn - 65);
            
            this.groupBox_Action.Location = new Point(8+ richBoxDisplayBPMN_width, 55);
            this.groupBox_Action.Size = new Size(action_group_width, height_bpmn - 60);

            this.listBox_CurrentAction.Size = new Size(action_group_width-10, height_bpmn - 255);
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
            if (richTextBox_displayGraphical.Text!=null)
            {
                // from xml to process components

                treeView_PCs.BeginUpdate();
                treeView_PCs.Nodes.Clear();
                //Step 1: parse
                parser.parseGraphical(allText, fileName, processComponents, checker);


                //generate PC tree table
                var PCs_node = treeView_PCs.Nodes.Add("Process components");
                PCs_node.NodeFont = new Font("Arial", 9.5f, FontStyle.Bold);
                
                var fileName_node = PCs_node.Nodes.Add("File name");
                fileName_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                fileName_node.Nodes.Add(processComponents.fileName);
                
                //defined enums
                /*if (processComponents.allDefinedEnums.Count > 0)
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
                }*/

                //participants
                if (processComponents.allParticipants.Count > 0)
                {
                    var participants_node = PCs_node.Nodes.Add("Participants");
                    participants_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach (var eachParticipant in processComponents.allParticipants)
                    {
                        var participant_node = participants_node.Nodes.Add(eachParticipant.name);
                        if (eachParticipant.id != null) { participant_node.Nodes.Add("id: " + eachParticipant.id); }
                        foreach(var info in eachParticipant.allInfo)
                        {
                            participant_node.Nodes.Add(info.name + ": " + info.value);
                        }
                        if (eachParticipant.bpmnProcessName != null) { participant_node.Nodes.Add("Process name: " + eachParticipant.bpmnProcessName); }
                        
                        /*if (eachParticipant.actionTypes.Count > 0)
                        {
                            var actionTypes_node = participant_node.Nodes.Add("Action types");
                            foreach (var actionType in eachParticipant.actionTypes)
                            {
                                actionTypes_node.Nodes.Add(actionType);
                            }
                        }*/
                        if (eachParticipant.TaskIDs.Count > 0)
                        {
                            var functionNames_node = participant_node.Nodes.Add("Task names");
                            foreach (var functionName in eachParticipant.TaskIDs)
                            {
                                functionNames_node.Nodes.Add(functionName);
                            }
                        }
                    }
                }
                //Tasks
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
                        if (eachTask.operateParticipants.Count > 0)
                        {
                            var currentParticipants_node = task_node.Nodes.Add("participants");
                            foreach (var processParticipant in eachTask.operateParticipants)
                            {
                                currentParticipants_node.Nodes.Add(processParticipant.name);
                            }
                        }
                        if (eachTask.actions.Count > 0)
                        {
                            var actions_node = task_node.Nodes.Add("actions");
                            foreach(var e_action in eachTask.actions)
                            {
                                var eachAction_node = actions_node.Nodes.Add(e_action.actionID);
                                /*if(e_action.addVariables.Count>0)
                                {
                                    //var actionVaris_node = eachAction_node.Nodes.Add("variables");
                                    foreach (var addVari in e_action.addVariables)
                                    {
                                        eachAction_node.Nodes.Add(addVari.type + " " + addVari.name);
                                    }
                                }*/
                                if (e_action.inputVariables.Count > 0)
                                {
                                    var actionVaris_node = eachAction_node.Nodes.Add("input variables");
                                    foreach (var inputVari in e_action.inputVariables)
                                    {
                                        var inputVariRef_node = actionVaris_node.Nodes.Add(inputVari.name);
                                        if(inputVari.refVari!=null && inputVari.refVari != "")
                                        {
                                            inputVariRef_node.Nodes.Add("Equal vari: " + inputVari.refVari);
                                        }
                                        
                                    }
                                }
                                if (e_action.outputVariables.Count > 0)
                                {
                                    var actionVaris_node = eachAction_node.Nodes.Add("output variables");
                                    foreach (var outputVari in e_action.outputVariables)
                                    {
                                        var outputVariRef_node = actionVaris_node.Nodes.Add(outputVari.name);
                                        if (outputVari.refVari != null && outputVari.refVari != "")
                                        {
                                            outputVariRef_node.Nodes.Add("Equal vari: " + outputVari.refVari);
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            //TODO: Checker! Print out error message!!!!
                        }
                    }
                }
                //process flow
                if (processComponents.allFlows.Count > 0)
                {
                    var flows_node = PCs_node.Nodes.Add("Flows");
                    flows_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);

                    //Handle gateways here
                    parser.handleGateways(processComponents);

                    foreach (var eachFlow in processComponents.allFlows)
                    {
                        var flow_node = flows_node.Nodes.Add(eachFlow.sourceRef);
                        if(eachFlow.ProcessID!=null)
                            flow_node.Nodes.Add("Process name: " + eachFlow.ProcessID);
                        if(eachFlow.flowName!=null)
                            flow_node.Nodes.Add("Flow name: " + eachFlow.flowName);
                        flow_node.Nodes.Add("Flow type: " + eachFlow.flowType);
                        if (eachFlow.TargetRef != null)
                            flow_node.Nodes.Add("Next Task: " + eachFlow.TargetRef);
                        if (eachFlow.gateway.gatewayID != null && eachFlow.gateway.gatewayID != "")
                        {
                            var gw_node = flow_node.Nodes.Add("Gateway: "+ eachFlow.gateway.gatewayID);
                            gw_node.Nodes.Add(eachFlow.gateway.gatewayName);
                            gw_node.Nodes.Add("Operation: " + eachFlow.gateway.splitOperation.ToString());
                            var outFlows_node = gw_node.Nodes.Add("Outgoing Task(s)");
                            foreach (var outgoFlow in eachFlow.gateway.outgoingFlows)
                            {
                                outFlows_node.Nodes.Add(outgoFlow.TargetRef);
                            }
                            /*var inFlows_node = gw_node.Nodes.Add("Incoming flow(s)");
                            foreach (var incomeFlow in eachFlow.gateway.incomingFlows)
                            {
                                inFlows_node.Nodes.Add(incomeFlow.flowID);
                            }*/
                        }
                            
                        

                        //TODO: handle gateway operation
                        /*if(eachFlow.splitOperation != null)
                        {
                            flow_node.Nodes.Add("Operation: "+eachFlow.splitOperation);
                        }*/
                        
                    }
                }
                //Gateways
                if(processComponents.allGateways.Count>0)
                {
                    var gateways_node = PCs_node.Nodes.Add("Gateways");
                    gateways_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                    foreach(var eachGateway in processComponents.allGateways)
                    {
                        var gw_node = gateways_node.Nodes.Add(eachGateway.gatewayID);
                        gw_node.Nodes.Add(eachGateway.gatewayName);
                        gw_node.Nodes.Add("Operation: " + eachGateway.splitOperation.ToString());
                        gw_node.Nodes.Add("Process name: " + eachGateway.processID);
                        var outFlows_node = gw_node.Nodes.Add("Outgoing flow(s)");
                        foreach(var outgoFlow in eachGateway.outgoingFlows)
                        {
                            outFlows_node.Nodes.Add(outgoFlow.TargetRef);
                        }

                    }
                }
                treeView_PCs.EndUpdate();

                //step 2: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                //checker.errorMessages = "";
            }

            
        }

        private void btn_translatePC2SCC_Click(object sender, EventArgs e)
        {

            if (treeView_PCs.Nodes.Count > 0)
            {
                // from process components to smart contract components

                treeView_SCCs.BeginUpdate();
                treeView_SCCs.Nodes.Clear();
                //Step 1: translate form PCs to SCCs
                translator.generateSolidityMain(processComponents, contractComponents, checker);
                //Step 2: generate SC tree table
                var contracts_node = treeView_SCCs.Nodes.Add("Contracts");
                contracts_node.NodeFont = new Font("Arial", 9.5f, FontStyle.Bold);
                foreach (var contractTemp in contractComponents.allSmartContracts)
                {
                    var contract_node = contracts_node.Nodes.Add(contractTemp.contractName);
                    contract_node.NodeFont = new Font("Arial", 9);
                    //parent contracts
                    if (contractTemp.parentContracts.Count > 0)
                    {
                        var parentContracts_node = contract_node.Nodes.Add("Parent contract(s)");
                        parentContracts_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var parentC in contractTemp.parentContracts)
                        {
                            parentContracts_node.Nodes.Add(parentC);
                        }
                    }
                    //enums
                    if (contractTemp.enums.Count > 0)
                    {
                        var enums_node = contract_node.Nodes.Add("Enum");
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
                    //structs
                    if(contractTemp.structs.Count > 0)
                    {
                        var structs_node = contract_node.Nodes.Add("Struct");
                        structs_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var structTemp in contractTemp.structs)
                        {
                            var struct_node = structs_node.Nodes.Add(structTemp.structName);
                            
                            foreach (var structParaTemp in structTemp.parameters)
                            {
                                struct_node.Nodes.Add(structParaTemp.type + " " + structParaTemp.name);
                            }
                        }
                    }
                    //state variables
                    if (contractTemp.stateVariables.Count > 0)
                    {
                        var variables_node = contract_node.Nodes.Add("State variables");
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
                        var modifiers_node = contract_node.Nodes.Add("Modifiers");
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
                        var functions_node = contract_node.Nodes.Add("Functions");
                        functions_node.NodeFont = new Font("Arial", 8.5f, FontStyle.Bold);
                        foreach (var functionTemp in contractTemp.functions)
                        {
                            var function_node = functions_node.Nodes.Add(functionTemp.name);
                            if (functionTemp.inputParams.Count > 0)
                            {
                                var funPara_node = function_node.Nodes.Add("input parameters");
                                foreach (var funPara in functionTemp.inputParams)
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
                            if (functionTemp.calledFunctions.Count > 0)
                            {
                                var funkey_node = function_node.Nodes.Add("called functions");
                                foreach (var calledfun in functionTemp.calledFunctions)
                                {
                                    funkey_node.Nodes.Add(calledfun);
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
                }
                treeView_SCCs.EndUpdate();


                //step 3: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                //checker.errorMessages = "";
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

                foreach (var scFile in contractComponents.allSmartContracts)
                {
                    contracts_node.Nodes.Add(scFile.contractName);
                }
                
                //step 2: print out error messages
                richTextBox_errorMassage.Text = checker.checkResults();
                //checker.errorMessages = "";
            }
            treeView_SCfileTree.EndUpdate();

            
        }

    }
}
