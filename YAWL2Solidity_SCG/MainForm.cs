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

namespace Graphical2SmartContact_SCG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Auto_Size();
        }

        #region VariablesDefinition
        GraphicalParser yAWLParser = new GraphicalParser();
        SmartContractGenerator solidityGenerator = new SmartContractGenerator();
        #endregion

        #region Graphical representation
        bool isBPMN = false;
        private void btn_importYAWL_Click(object sender, EventArgs e)
        {
            String file = openFileDiag("Browse YAWL or BPMN Files", "YAWL files (*.yawl)|*.yawl|BPMN files (*.bpmn)|*.bpmn");

            if (file != "")
            {
                Cursor = Cursors.WaitCursor;
                textBox_YAWLImportedPath.Text = file;
                string text = "";
                try
                {
                    text = File.ReadAllText(file);
                    if (text != "")
                    {
                        richTextBox_displayYAWL.Text = text;
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
                        yAWLParser.parseGraphical(text,isBPMN);
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
            if (!isBPMN && textBox_YAWLImportedPath.Text != "")
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
                            yAWLParser.parseYawlRoles(text);
                            displayYawlRolesTree();
                        }
                    }
                    catch (IOException)
                    {
                    }
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                MessageBox.Show("You did not import any graphical representation! Please click \"Import\" first.", "Error in importing YAWL Roles",
                         MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void AutosizeYAWL(int width_yawl, int height_yawl)
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
                treeViewYAWLRoles_width = (width_yawl - 15) / 4;
                richBoxDisplayYAWL_width -= (treeViewYAWLRoles_width + 5);

                this.btn_importYAWLRoles.Location = new Point(10 + richBoxDisplayYAWL_width, 25);
                this.btn_importYAWLRoles.Size = new Size(treeViewYAWLRoles_width, 30);

                this.treeView_displayYAWLRoles.Location = new Point(10 + richBoxDisplayYAWL_width, 60);
                this.treeView_displayYAWLRoles.Size = new Size(treeViewYAWLRoles_width, height_yawl - 65);
            }

            this.btn_importYAWL.Location = new Point(5, 25);
            this.btn_importYAWL.Size = new Size(100, 30);

            this.textBox_YAWLImportedPath.Location = new Point(110, 30);
            this.textBox_YAWLImportedPath.Size = new Size(richBoxDisplayYAWL_width - 105, 22);

            this.richTextBox_displayYAWL.Location = new Point(5, 60);
            this.richTextBox_displayYAWL.Size = new Size(richBoxDisplayYAWL_width, height_yawl - 65);
            

            
        }
        private void displayYawlRolesTree()
        {
            treeView_displayYAWLRoles.BeginUpdate();
            treeView_displayYAWLRoles.Nodes.Clear();
            //add roles
            var roles_node = treeView_displayYAWLRoles.Nodes.Add("Roles");
            roles_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var role_yawl in yAWLParser.allRoles)
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

        #region Table
        private void btn_fromYAWL2Table_Click(object sender, EventArgs e)
        {
            treeView_table.BeginUpdate();
            treeView_table.Nodes.Clear();
            //add data definition
            /*var definedEnums_node = treeView_table.Nodes.Add("Defined Data Structure");
            definedEnums_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var definedenum in yAWLParser.allDefinedEnums)
            {
                var definedenum_node = definedEnums_node.Nodes.Add(definedenum.name);
                definedenum_node.NodeFont = new Font("Arial", 9);
                for (int i = 0; i< definedenum.elements.Count(); i++)
                {
                    definedenum_node.Nodes.Add(definedenum.elements[i]);
                }
            }*/
            //add local variables
            var localVaris_node = treeView_table.Nodes.Add("Local Variables");
            localVaris_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var localvari_yawl in yAWLParser.allLocalVariables)
            {
                var localvari_node = localVaris_node.Nodes.Add(localvari_yawl.name);
                localvari_node.NodeFont = new Font("Arial", 9);
                localvari_node.Nodes.Add("Type:" + localvari_yawl.type);
                localvari_node.Nodes.Add("Value:" + localvari_yawl.defaultVaule);
            }
            //add modifiers and flows
            var roles_node = treeView_table.Nodes.Add("Roles");
            roles_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var role_yawl in yAWLParser.allRoles)
            {
                //add parameters to modifier
                var role_node = roles_node.Nodes.Add(role_yawl.name);
                role_node.NodeFont = new Font("Arial", 9);
                role_node.Nodes.Add("address: " + role_yawl.address);
                role_node.Nodes.Add("id: " + role_yawl.id);
            }
            /*var modifiers_node = treeView_table.Nodes.Add("Modifiers");
            modifiers_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var modifier_yawl in yAWLParser.allModifiers)
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
            var flows_node = treeView_table.Nodes.Add("Process Flows");
            flows_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var flow in yAWLParser.allFlows)
            {
                var flow_node = flows_node.Nodes.Add(flow.currentProcessName);
                flow_node.NodeFont = new Font("Arial", 9);
                if(flow.nextProcesses.Count>0)
                {
                    foreach (var nextProcess in flow.nextProcesses)
                    {
                        var nextProcess_node = flow_node.Nodes.Add("Next process: "+nextProcess.processName);
                        if(nextProcess.condition!=null)
                        {
                            nextProcess_node.Nodes.Add(nextProcess.condition);
                        }
                    }
                }
                if(flow.splitOperation!=null)
                {
                    flow_node.Nodes.Add(flow.splitOperation);
                }
            }

            //add functions
            var functions_node = treeView_table.Nodes.Add("Tasks");
            functions_node.NodeFont = new Font("Arial", 9 , FontStyle.Bold);
            foreach (var function in yAWLParser.allFunctions)
            {
                var function_node = functions_node.Nodes.Add(function.name);
                function_node.NodeFont = new Font("Arial", 9);
                //modifiers
                var funmodifiers_node = function_node.Nodes.Add("Role");
                foreach (var funmodifier in function.modifiers)
                {
                    string funmodi_values = funmodifier.name + "(";
                    for(int i = 0; i< funmodifier.inputVaris.Count(); i++)
                    {
                        if(i>0)
                        {
                            funmodi_values += ", ";
                        }
                        funmodi_values += funmodifier.inputVaris[i].defaultVaule;
                    }
                    funmodi_values += ")";
                    funmodifiers_node.Nodes.Add(funmodi_values);
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

            treeView_table.EndUpdate();
        }
        private void btn_fromTable2Solidity_Click(object sender, EventArgs e)
        {
            richTextBox_displaySolidity.Text = solidityGenerator.generateSolidityText(yAWLParser);
        }
        private void AutosizeTable(int width_table, int height_table)
        {
            int treeviewTable_width = width_table - 10;//(2*width_table) / 3 - 10;
            this.treeView_table.Location = new Point(5, 25);
            this.treeView_table.Size = new Size(treeviewTable_width, height_table - 30);

            //this.richTextBox_displayTable.Location = new Point(treeviewTable_width + 15, 25);
            //this.richTextBox_displayTable.Size = new Size(width_table - treeviewTable_width - 20, height_table - 30);
        }
        #endregion

        #region Smart contract language
        private void btn_exportSolidity_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the .sol file
            // assigned to btn_exportSolidity.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Solidity File|*.sol";
            saveFileDialog1.Title = "Save as an Solidity File";
            saveFileDialog1.FileName = solidityGenerator.solidityFileName;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;
                textBox_SolidityExportedPath.Text = filePath;

                StreamWriter writer = new StreamWriter(saveFileDialog1.OpenFile());
                writer.Write(solidityGenerator.solidityAllText);
                writer.Dispose();
                writer.Close();

            }
            else
            {
                textBox_SolidityExportedPath.Text = "";
            }
        }
        private void AutosizeSolidity(int width_solidity, int height_solidity)
        {
            this.richTextBox_displaySolidity.Location = new Point(5, 25);
            this.richTextBox_displaySolidity.Size = new Size(width_solidity - 10, height_solidity - 65);

            this.btn_exportSolidity.Location = new Point(5, height_solidity - 35);
            this.btn_exportSolidity.Size = new Size(110, 30);

            this.textBox_SolidityExportedPath.Location = new Point(120, height_solidity - 30);
            this.textBox_SolidityExportedPath.Size = new Size(width_solidity - 125, 22);
        }
        #endregion

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            Auto_Size();
        }

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

        private void Auto_Size()
        {
            //int groupBoxYAWL_width = (2*this.ClientSize.Width)/3 - 120;
            int groupBoxYAWL_width = (3 * this.ClientSize.Width) / 4 - 135;
            int groupBoxYAWL_height = (this.ClientSize.Height - 10) / 2;
            this.groupBox_yawl.Location = new Point(5, 0);
            this.groupBox_yawl.Size = new Size(groupBoxYAWL_width, groupBoxYAWL_height);
            AutosizeYAWL(groupBoxYAWL_width, groupBoxYAWL_height);

            int groupBoxSolidity_height = this.ClientSize.Height - groupBoxYAWL_height - 10;
            this.groupBox_solidity.Location = new Point(5, groupBoxYAWL_height+10);
            this.groupBox_solidity.Size = new Size(groupBoxYAWL_width, groupBoxSolidity_height);
            AutosizeSolidity(groupBoxYAWL_width, groupBoxSolidity_height);

            int groupBoxTable_width = groupBoxYAWL_width / 3;
            int groupBoxTable_height = this.ClientSize.Height - 10;
            int groupBoxTable_x = this.ClientSize.Width - groupBoxTable_width - 5;
            this.groupBox_table.Location = new Point(groupBoxTable_x, 5);
            this.groupBox_table.Size = new Size(groupBoxTable_width, groupBoxTable_height);
            AutosizeTable(groupBoxTable_width, groupBoxTable_height);

            //left button
            int btnFromYAWL2Table_y = groupBoxYAWL_height / 2 - 15;
            this.btn_fromYAWL2Table.Location = new Point(groupBoxYAWL_width+10, btnFromYAWL2Table_y);
            this.btn_fromYAWL2Table.Size = new Size(160, 80);

            //right button
            int btnFromTable2Solidity = this.ClientSize.Height - (groupBoxSolidity_height / 2) - 85;
            this.btn_fromTable2Solidity.Location = new Point(groupBoxYAWL_width+10, btnFromTable2Solidity);
            this.btn_fromTable2Solidity.Size = new Size(160, 80);

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
