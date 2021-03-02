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
            public List<Variable> inOutVariables = new List<Variable>();
            public List<Modifier> modifiers = new List<Modifier>();
            public Flow nextProcess = new Flow();
        };
        public class Flow
        {
            public string currentProcessName;
            public List<ToNextProcess> nextProcesses = new List<ToNextProcess>();
            public string splitOperation; //XOR, OR, AND //only work when it has more than one process.
        };
        public class ToNextProcess
        {
            public string processName;
            public string condition;
        };
        #endregion

        public List<Variable> allLocalVariables = new List<Variable>();
        public List<DefineEnum> allDefinedEnums = new List<DefineEnum>();
        public List<Modifier> allModifiers = new List<Modifier>();
        public List<Function> allFunctions = new List<Function>();
        public List<Flow> allFlows = new List<Flow>();
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
                                    //process flow, here should initial all the functions
                                    else if (e_LVinRootNet.Name == "processControlElements")
                                    {
                                        foreach(XmlNode flow_node in e_LVinRootNet.ChildNodes)
                                        {
                                            addProcessFlow(flow_node);
                                        }
                                    }
                                }
                                
                            }
                        }
                        //others: functions
                        else
                        {
                            addFunction(decomposition_node);
                        }
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

        void addProcessFlow(XmlNode flow_node)
        {
            Flow flow = new Flow();
            if (flow_node.Name == "inputCondition" || flow_node.Name == "task")
            {
                XmlNode flow_id = flow_node.Attributes.GetNamedItem("id");
                if (flow_id != null)
                {
                    flow.currentProcessName = flow_id.InnerXml;

                    if (flow_node.GetType().Name == "XmlElement")
                    {
                        XmlElement e_flow_input = (XmlElement)flow_node;
                        //flowsInto
                        XmlNodeList next_nodes = e_flow_input.GetElementsByTagName("flowsInto");
                        if (next_nodes != null)
                        {
                            foreach (XmlNode nextNode in next_nodes)
                            {
                                ToNextProcess nextProcess = new ToNextProcess();
                                foreach (XmlNode flowInfo in nextNode.ChildNodes)
                                {
                                    if (flowInfo.Name == "nextElementRef")
                                    {
                                        nextProcess.processName = flowInfo.Attributes.GetNamedItem("id").InnerXml;
                                    }
                                    else if (flowInfo.Name == "predicate")
                                    {
                                        //TODO: this condition should be parse in more detailed.
                                        string[] stringSeparators = new string[] { "/text()" };
                                        string[] conditions = flowInfo.InnerText.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);
                                        if (conditions.Count() == 2)
                                        {
                                            string[] variables = conditions[0].Split('/');
                                            //string[] operValue = conditions[1].Split('\'');
                                            //"&lt;" represents "<", "&gt;" represents ">".
                                            if (conditions[1].Contains("<="))// || conditions[1].Contains(">=")|| conditions[1].Contains("!="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "<=");
                                            }
                                            else if (conditions[1].Contains(">="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], ">=");
                                            }
                                            else if (conditions[1].Contains("!="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "!=");
                                            }
                                            else if (conditions[1].Contains("<"))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "<");
                                            }
                                            else if (conditions[1].Contains(">"))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], ">");
                                            }
                                            else if (conditions[1].Contains("="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "=");
                                            }
                                        }
                                    }
                                    else if (flowInfo.Name == "isDefaultFlow")
                                    {
                                        nextProcess.condition = "otherwise";
                                    }
                                }
                                flow.nextProcesses.Add(nextProcess);
                            }
                            //split operation
                            if (next_nodes.Count > 1)
                            {
                                XmlNode split_node = e_flow_input.GetElementsByTagName("split").Item(0);
                                if (split_node != null)
                                {
                                    flow.splitOperation = split_node.Attributes.GetNamedItem("code").InnerXml;
                                }
                            }

                        }
                    }
                    allFlows.Add(flow);
                }
            }
        }

        string generateCondition(string variableName, string fullCondition, string operation)
        {
            string result;
            string[] op = new string[] { operation };
            var value = fullCondition.Split(op, StringSplitOptions.RemoveEmptyEntries).Last();
            if(value.Contains('\''))
            {
                value = value.Trim('\'', ' ');
            }
            var variable = allLocalVariables.Find(x => x.name == variableName);
            if(variable!=null)
            {
                if (variable.type == "string")
                {
                    value = "\"" + value + "\"";
                }
                if(operation=="=")
                {
                    operation = "==";
                }
                result = variableName + " " + operation + " " + value;
            }
            else
            {
                result = "ERROR: undefined variable";
            }

            return result;
        }

        void addFunction(XmlNode decomposition_node)
        {
            Function function_temp = new Function();
            function_temp.name = decomposition_node.Attributes.GetNamedItem("id").InnerXml;
            var flow = allFlows.Find(x => x.currentProcessName == function_temp.name);
            if (flow != null)
            {
                function_temp.nextProcess = flow;

                
                foreach(XmlNode para in decomposition_node.ChildNodes)
                {
                    if (para.GetType().Name == "XmlElement")
                    {
                        XmlElement e_para = (XmlElement)para;
                        XmlNode paraType = e_para.GetElementsByTagName("type").Item(0);
                        XmlNode paraName = e_para.GetElementsByTagName("name").Item(0);
                        if(paraType != null && paraName != null)
                        {
                            //modifiers
                            if(paraType.InnerText== "solidity_modifier")
                            {
                                var paraModif = allModifiers.Find(x => x.name == paraName.InnerText);
                                if(!function_temp.modifiers.Contains(paraModif))
                                {
                                    XmlNode paraValue = e_para.GetElementsByTagName("defaultValue").Item(0);
                                    if(paraValue!=null)
                                    {
                                        string valueStr = paraValue.InnerText;
                                        if(valueStr.Contains(","))
                                        {
                                            //more than one variables in the modifier
                                        }
                                        else
                                        {
                                            paraModif.inputVaris[0].defaultVaule = valueStr;
                                        }
                                    }
                                    function_temp.modifiers.Add(paraModif);
                                }
                            }
                            //InputParam, outputParam and in/out
                            else
                            {
                                var paraVari = allLocalVariables.Find(x => x.name == paraName.InnerText);
                                if (paraVari != null)
                                {
                                    if (para.Name == "inputParam")
                                    {
                                        var findResult = function_temp.outputVariables.Find(x => x.name == paraName.InnerText);
                                        if (findResult != null)
                                        {
                                            function_temp.outputVariables.Remove(findResult);
                                            function_temp.inOutVariables.Add(findResult);
                                        }
                                        else
                                        {
                                            function_temp.inputVariables.Add(paraVari);
                                        }

                                    }
                                    else if (para.Name == "outputParam")
                                    {

                                        var findResult = function_temp.inputVariables.Find(x => x.name == paraName.InnerText);
                                        if (findResult != null)
                                        {
                                            function_temp.inputVariables.Remove(findResult);
                                            function_temp.inOutVariables.Add(findResult);
                                        }
                                        else
                                        {
                                            function_temp.outputVariables.Add(paraVari);
                                        }

                                    }
                                }
                            }
                        } 
                        
                    }
                }

            }
            allFunctions.Add(function_temp);
        }
    }
}
