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

namespace YAWL2Solidity_SCG
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Auto_Size();
        }

        #region VariablesDefinition
        YAWLParser yAWLParser = new YAWLParser();
        SolidityGenerator solidityGenerator = new SolidityGenerator();
        #endregion

        private void btn_importYAWL_Click(object sender, EventArgs e)
        {
            String file = openFileDiag("Browse YAWL Files", "YAWL files (*.yawl)|*.yawl");

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
                        richTextBox_displayTable.Text = yAWLParser.parseYAWL(text);
                    }
                }
                catch (IOException)
                {
                }
                Cursor = Cursors.Default;
            }
        }

        private void btn_fromYAWL2Table_Click(object sender, EventArgs e)
        {
            treeView_table.BeginUpdate();
            //add data definition
            var definedEnums_node = treeView_table.Nodes.Add("Defined Data Structure");
            definedEnums_node.NodeFont = new Font("Arial", 9, FontStyle.Bold);
            foreach (var definedenum in yAWLParser.allDefinedEnums)
            {
                var definedenum_node = definedEnums_node.Nodes.Add(definedenum.name);
                definedenum_node.NodeFont = new Font("Arial", 9);
                for (int i = 0; i< definedenum.elements.Count(); i++)
                {
                    definedenum_node.Nodes.Add(definedenum.elements[i]);
                }
            }
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
            //add modifiers
            var modifiers_node = treeView_table.Nodes.Add("Modifiers");
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
            }
            //add functions
            var functions_node = treeView_table.Nodes.Add("Functions");
            functions_node.NodeFont = new Font("Arial", 9 , FontStyle.Bold);


            treeView_table.EndUpdate();
        }

        private void btn_fromTable2Solidity_Click(object sender, EventArgs e)
        {
            richTextBox_displaySolidity.Text = solidityGenerator.generateSolidityText(yAWLParser);
        }

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

        private PictureBox pictureBox1 = new PictureBox();
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
            int groupBoxYAWL_width = (this.ClientSize.Width - 20) / 2;
            int groupBoxYAWL_height = (this.ClientSize.Height - 10) / 2;
            this.groupBox_yawl.Location = new Point(5, 0);
            this.groupBox_yawl.Size = new Size(groupBoxYAWL_width, groupBoxYAWL_height);
            AutosizeYAWL(groupBoxYAWL_width, groupBoxYAWL_height);

            int groupBoxSolidity_width = this.ClientSize.Width - groupBoxYAWL_width - 20;
            this.groupBox_solidity.Location = new Point(this.ClientSize.Width - groupBoxSolidity_width -5, 0);
            this.groupBox_solidity.Size = new Size(groupBoxSolidity_width, groupBoxYAWL_height);
            AutosizeSolidity(groupBoxSolidity_width, groupBoxYAWL_height);

            int groupBoxTable_width = this.ClientSize.Width / 2;
            int groupBoxTable_height = this.ClientSize.Height - groupBoxYAWL_height - 10;
            int groupBoxTable_x = (groupBoxYAWL_width / 2) + 5;
            this.groupBox_table.Location = new Point(groupBoxTable_x, groupBoxYAWL_height + 5);
            this.groupBox_table.Size = new Size(groupBoxTable_width, groupBoxTable_height);
            AutosizeTable(groupBoxTable_width, groupBoxTable_height);

            //left button
            int btnFromYAWL2Table_x = (this.ClientSize.Width/8) - 75;
            int btnFromYAWL2Table_y = ((3 * this.ClientSize.Height) / 4) - 42;
            this.btn_fromYAWL2Table.Location = new Point(btnFromYAWL2Table_x, btnFromYAWL2Table_y);
            this.btn_fromYAWL2Table.Size = new Size(150, 80);

            //right button
            int btnFromTable2Solidity_x = this.ClientSize.Width - 150 - btnFromYAWL2Table_x;
            this.btn_fromTable2Solidity.Location = new Point(btnFromTable2Solidity_x, btnFromYAWL2Table_y);
            this.btn_fromTable2Solidity.Size = new Size(150, 80);

            //Arrows
            // (x1,y1)                                              (x6,y1)
            //   |                                                     /\
            //  \/                                                      |
            // (x1,y2)                                              (x6,y2)
            //         (x2,y3) -> (x3,y3)        (x4,y3) -> (x5,y3)
            int x1 = btnFromYAWL2Table_x + 75;
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
            };
        }

        private void AutosizeYAWL(int width_yawl, int height_yawl)
        {
            this.btn_importYAWL.Location = new Point(5, 20);
            this.btn_importYAWL.Size = new Size(100, 30);

            this.textBox_YAWLImportedPath.Location = new Point(110,25);
            this.textBox_YAWLImportedPath.Size = new Size(width_yawl-115,22);

            this.richTextBox_displayYAWL.Location = new Point(5,55);
            this.richTextBox_displayYAWL.Size = new Size(width_yawl - 10, height_yawl - 60);
        }

        private void AutosizeSolidity(int width_solidity, int height_solidity)
        {
            this.richTextBox_displaySolidity.Location = new Point(5, 25);
            this.richTextBox_displaySolidity.Size = new Size(width_solidity - 10, height_solidity - 65);
            
            this.btn_exportSolidity.Location = new Point(5, height_solidity-35);
            this.btn_exportSolidity.Size = new Size(110, 30);
            
            this.textBox_SolidityExportedPath.Location = new Point(120, height_solidity - 30);
            this.textBox_SolidityExportedPath.Size = new Size(width_solidity - 125, 22);
        }

        private void AutosizeTable(int width_table, int height_table)
        {
            
        }


        private void MainForm_Paint(object sender, PaintEventArgs e, int x1, int x2, int x3, int x4, int x5, int x6,
            int y1, int y2, int y3)
        {
            // (x1,y1)                                              (x6,y1)
            //   |                                                     /\
            //  \/                                                      |
            // (x1,y2)                                              (x6,y2)
            //         (x2,y3) -> (x3,y3)        (x4,y3) -> (x5,y3)
            
            //draw arrows
            Graphics g = CreateGraphics();
            Pen p = new Pen(Brushes.DeepSkyBlue, 30);
            p.StartCap = LineCap.ArrowAnchor;
            g.Clear(Color.LightGray);
            g.DrawLine(p, x1, y2, x1, y1);
            g.DrawLine(p, x3, y3, x2, y3);
            g.DrawLine(p, x5, y3, x4, y3);
            g.DrawLine(p, x6, y1, x6, y2);
            

            //g.DrawLine(p, 340, 450, 200, 320); //(x2,y2)<-(x1,y1)
            //2*340-200+660
            //g.DrawLine(p, 1140, 320, 1002, 450); //(2*x2-x1+TableWidth,y1)<-(x2+TableWidth,y2)
            //g.DrawLine(System.Drawing.Pens.Red, pictureBox1.Left, pictureBox1.Top, pictureBox1.Right, pictureBox1.Bottom);

        }


        #endregion

    }
}
