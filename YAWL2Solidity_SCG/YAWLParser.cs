using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace YAWL2Solidity_SCG
{
    public class YAWLParser
    {
        #region DataTypes
        public class Variable
        {
            public string name;
            public string type;
            public string defaultVaule;
        };
        public class DefineEnum
        {
            public string name;
            public List<string> elements = new List<string>();
        };
        public class Modifier
        {
            public string name;
            public string condition;
            public string errorString;
            public List<Variable> inputVaris = new List<Variable>();
        };
        public class Function
        {
            public string name;
            public List<Variable> inputVariables = new List<Variable>();
            public List<Variable> outputVariables = new List<Variable>();
            public List<Modifier> modifiers = new List<Modifier>();
            public NextProcess nextProcess = new NextProcess();
        };
        public class NextProcess
        {
            public string processName;
            public List<ToNextProcess> nextProcesses = new List<ToNextProcess>();
            public string splitOperation; //XOR, OR, AND //only work when it has more than one process.
        };
        public class ToNextProcess
        {
            public string processname;
            public string condition;
        };
        #endregion

        public List<Variable> allLocalVariables = new List<Variable>();
        public List<DefineEnum> allDefinedEnums = new List<DefineEnum>();
        public List<Modifier> allModifiers = new List<Modifier>();
        public string fileName = "default";

        public string parseYAWL(string text)
        {
            string result = "";
            //clear all previous data
            allLocalVariables.Clear();
            
            //parse YAWL
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            //file name
            XmlNode specification_yawl = doc.GetElementsByTagName("specification").Item(0);
            if (specification_yawl != null && specification_yawl.GetType().Name == "XmlElement")
            {
                //Print detail of BM
                XmlElement e_specification_yawl = (XmlElement)specification_yawl;
                fileName = e_specification_yawl.Attributes.GetNamedItem("uri").InnerXml;

                #region DataDefinitionOfYAWL
                //Data Definition part
                XmlNode data_define = e_specification_yawl.GetElementsByTagName("xs:schema").Item(0);
                if (data_define != null && data_define.GetType().Name == "XmlElement")
                {
                    //Print detail of BM
                    XmlElement eElement_dd = (XmlElement)data_define;
                    //result = "element:\n"+ eElement_dd.InnerXml;
                    foreach (XmlNode node in eElement_dd.ChildNodes)
                    {
                        string attribute = node.Attributes.GetNamedItem("name").InnerXml;
                        if (!attribute.Contains("solidity_"))
                        {
                            XmlNode restriction = node.ChildNodes.Item(0);
                            if (restriction.Name == "xs:restriction" &&
                                restriction.Attributes.GetNamedItem("base").InnerXml == "solidity_enum")
                            {
                                //result += "restriction:" + attribute + "\n";
                                DefineEnum denum = new DefineEnum();
                                denum.name = attribute;
                                //denum.elements = new List<string>();
                                foreach (XmlNode enum_node in restriction.ChildNodes)
                                {
                                    if (enum_node.Name == "xs:enumeration")
                                    {
                                        string enum_value = enum_node.Attributes.GetNamedItem("value").InnerText;
                                        denum.elements.Add(enum_value);
                                        //result += "enum_value:" + enum_value + "\n";
                                    }
                                }
                                allDefinedEnums.Add(denum);
                            }
                        }
                    }

                }
                #endregion

                foreach (XmlNode decomposition_node in e_specification_yawl.ChildNodes)
                {
                    if (decomposition_node.Name == "decomposition")
                    {
                        //RootNet: Local variables and process flow
                        if(decomposition_node.Attributes.GetNamedItem("isRootNet")!=null
                            && decomposition_node.Attributes.GetNamedItem("isRootNet").InnerXml == "true")
                        {
                            foreach(XmlNode nodeInRootNet in decomposition_node.ChildNodes)
                            {
                                if(nodeInRootNet.GetType().Name == "XmlElement")
                                {
                                    XmlElement e_LVinRootNet = (XmlElement)nodeInRootNet;
                                    
                                    if (e_LVinRootNet.Name == "localVariable")
                                    {
                                        XmlNode lvType_node = e_LVinRootNet.GetElementsByTagName("type").Item(0);
                                        if(lvType_node!=null)
                                        {
                                            var originalType = lvType_node.InnerXml;
                                            //modifier
                                            if (originalType == "solidity_modifier")
                                            {
                                                addModifier(e_LVinRootNet);
                                            }
                                            //local variable
                                            else
                                            {
                                                addLocalVariable(e_LVinRootNet, originalType);
                                            }
                                            
                                        }
                                    }
                                    #region ProcessFlow
                                    //process flow, here should initial all the functions
                                    else if (e_LVinRootNet.Name == "processControlElements")
                                    {

                                    }
                                    #endregion
                                }
                                
                            }
                        }
                        #region Functions
                        //others: functions
                        else
                        {

                        }
                        #endregion
                    }
                }
            }
            return result;

        }

        void addLocalVariable(XmlElement e_LVinRootNet, string originalType)
        {
            Variable lv_temp = new Variable();
            //name
            XmlNode lvName_node = e_LVinRootNet.GetElementsByTagName("name").Item(0);
            if (lvName_node != null)
            {
                lv_temp.name = lvName_node.InnerXml;
            }
            //initial value
            XmlNode lvValue_node = e_LVinRootNet.GetElementsByTagName("initialValue").Item(0);
            if (lvValue_node != null)
            {
                lv_temp.defaultVaule = lvValue_node.InnerXml;
            }
            //type
            //if contains "solidity_", delete this part and store the rest.
            if (originalType.Contains("solidity_"))
            {
                lv_temp.type = originalType.Remove(0, 9);
            }
            else if (originalType == "unsignedInt")
            {
                lv_temp.type = "uint";
            }
            else if (originalType == "boolean")
            {
                lv_temp.type = "bool";
            }
            else
            {
                lv_temp.type = originalType;
            }

            allLocalVariables.Add(lv_temp);
        }

        void addModifier(XmlElement e_LVinRootNet)
        {
            Modifier m_temp = new Modifier();
            //name
            XmlNode mName_node = e_LVinRootNet.GetElementsByTagName("name").Item(0);
            if (mName_node != null)
            {
                m_temp.name = mName_node.InnerXml;
            }
            //initial value
            XmlNode mValue_node = e_LVinRootNet.GetElementsByTagName("initialValue").Item(0);
            if (mValue_node != null)
            {
                string[] values = mValue_node.InnerXml.Split(',');
                m_temp.condition = values[0];
                //parse the condition
                //currently, we only consider one condition
                if (!values[0].Contains("&&") && !values[0].Contains("||"))
                {
                    //split the condition into several parts.
                    string[] stringSeparators = new string[] { "==" };
                    string[] conditions = values[0].Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (conditions.Count() == 2)//we define input parameters start with "_"
                    {
                        Variable inputVari = new Variable();
                        foreach (var con in conditions)
                        {
                            if (con.StartsWith("_"))
                            {
                                inputVari.name = con;
                            }
                            else
                            {
                                //search if this variable is in the local variables
                                var findVari = allLocalVariables.Find(x => x.name == con);
                                if (findVari != null)
                                {
                                    inputVari.type = findVari.type;
                                }
                                else
                                {
                                    inputVari.type = "ERROR";
                                }

                            }
                        }
                        if (inputVari.name != null)
                        {
                            m_temp.inputVaris.Add(inputVari);
                        }
                    }

                    m_temp.errorString = values[1];
                }
                else
                {
                    //TODO: when it has more than one condition, "&&" and "||" should also take into account!
                }
            }
            allModifiers.Add(m_temp);
        }
    }
}
