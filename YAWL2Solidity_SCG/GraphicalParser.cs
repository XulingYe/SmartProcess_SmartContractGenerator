using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Graphical2SmartContact_SCG
{
    public class GraphicalParser
    {
        #region DataTypes
        public class SCGVariable
        {
            public string name;
            public string type;
            public string value;
        };
        public class Role
        {
            public string name;
            public string id;
            public string address;
            public List<string> functionNames = new List<string>();
            //actionTypes: to store the types (info, check, pay) of the actions for the role
            public List<string> actionTypes = new List<string>(); 
        }

        //multi-roles class is used to handle the functions that multiple roles can operate.
        public class MultiRoles
        {
            public List<Role> roles = new List<Role>();
        }
        public class DefineEnum
        {
            public string enumName;
            public List<string> enumValues = new List<string>();
        };
        /*public class Modifier
        {
            public string name;
            public string condition;
            public string errorString;
            public List<Variable> inputVaris = new List<Variable>();
        };*/
        public class YawlTask
        {
            public string name;
            public List<SCGVariable> inputVariables = new List<SCGVariable>();
            public List<SCGVariable> outputVariables = new List<SCGVariable>();
            public List<SCGVariable> inOutVariables = new List<SCGVariable>();
            //public List<Modifier> modifiers = new List<Modifier>();
            public Flow processFlow = new Flow();
            public string actionType; //indicate function type. Could be check, change or pay.
            public SCGVariable payTypeVariable; // indicate the variable of the payment. The type of this variable must be uint. 
        };
        public class Flow
        {
            public string currentProcessName;
            public List<ToNextProcess> nextProcesses = new List<ToNextProcess>();
            public string splitOperation; //XOR, OR, AND //only work when it has more than one process.
            public List<Role> currentProcessRoles = new List<Role>();
        };
        public class ToNextProcess
        {
            public string processName;
            public string condition;
        };
        #endregion

        public List<SCGVariable> allLocalVariables = new List<SCGVariable>();
        public List<DefineEnum> allDefinedEnums = new List<DefineEnum>();
        //public List<Modifier> allModifiers = new List<Modifier>();
        public List<YawlTask> allTasks = new List<YawlTask>();
        public List<Flow> allFlows = new List<Flow>();
        public List<Role> allRoles = new List<Role>();
        public List<MultiRoles> allMultiRoles = new List<MultiRoles>();
        public string fileName = "default";

        public void parseGraphical(string text, bool isBPMN)
        {
            //clear all previous data
            allLocalVariables.Clear();
            allDefinedEnums.Clear();
            allFlows.Clear();
            allTasks.Clear();
            //allModifiers.Clear();
            allRoles.Clear();

            if (!isBPMN)
            {
                parseYawl(text);
            }
            else
            {
                parseBPMN(text);
            }
            

        }

        #region YAWL
        void parseYawl(string text)
        {
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
                /*XmlNode data_define = e_specification_yawl.GetElementsByTagName("xs:schema").Item(0);
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
                            if (restriction != null && restriction.Name == "xs:restriction" &&
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

                }*/
                #endregion

                foreach (XmlNode decomposition_node in e_specification_yawl.ChildNodes)
                {
                    if (decomposition_node.Name == "decomposition")
                    {
                        //RootNet: Local variables and process flow
                        if (decomposition_node.Attributes.GetNamedItem("isRootNet") != null
                            && decomposition_node.Attributes.GetNamedItem("isRootNet").InnerXml == "true")
                        {
                            foreach (XmlNode nodeInRootNet in decomposition_node.ChildNodes)
                            {
                                if (nodeInRootNet.GetType().Name == "XmlElement")
                                {
                                    XmlElement e_LVinRootNet = (XmlElement)nodeInRootNet;

                                    if (e_LVinRootNet.Name == "localVariable")
                                    {
                                        XmlNode lvType_node = e_LVinRootNet.GetElementsByTagName("type").Item(0);
                                        if (lvType_node != null)
                                        {
                                            var originalType = lvType_node.InnerXml;
                                            //modifier
                                            /*if (originalType == "solidity_modifier")
                                            {
                                                addModifier(e_LVinRootNet);
                                            }*/

                                            //local variable
                                            if (originalType != "Type")
                                            {
                                                addLocalVariable(e_LVinRootNet, originalType);
                                            }

                                        }
                                    }
                                    //process flow, here should initial all the functions
                                    else if (e_LVinRootNet.Name == "processControlElements")
                                    {
                                        foreach (XmlNode flow_node in e_LVinRootNet.ChildNodes)
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
        }

        void addLocalVariable(XmlElement e_LVinRootNet, string originalType)
        {
            SCGVariable lv_temp = new SCGVariable();
            //name
            XmlNode lvName_node = e_LVinRootNet.GetElementsByTagName("name").Item(0);
            if (lvName_node != null)
            {
                lv_temp.name = lvName_node.InnerXml;
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
            //initial value
            XmlNode lvValue_node = e_LVinRootNet.GetElementsByTagName("initialValue").Item(0);
            if (lvValue_node != null)
            {
                if(allDefinedEnums.Exists(x=>x.enumName==lv_temp.type)&& !lvValue_node.InnerXml.Contains(lv_temp.type))
                {
                    lv_temp.value = lv_temp.type + "." + lvValue_node.InnerXml;
                }
                else
                {
                    lv_temp.value = lvValue_node.InnerXml;
                }
                
            }

            allLocalVariables.Add(lv_temp);
        }

        /*
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
        }*/

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
                        //Roles in resourcing
                        XmlNodeList roles_nodes = e_flow_input.GetElementsByTagName("role");
                        int rolesCount = roles_nodes.Count;
                        if(rolesCount > 0)
                        {
                            var tempMultiRoles = new MultiRoles();
                            foreach(XmlNode role_node in roles_nodes)
                            {
                                //role's id is not empty
                                if(role_node.InnerText != null && role_node.InnerText !="")
                                {
                                    //add this id into process
                                    Role tempRole = new Role();
                                    tempRole.id = role_node.InnerText;
                                    flow.currentProcessRoles.Add(tempRole);

                                    //add this id into allRoles list
                                    var foundRole = allRoles.Find(x => x.id == role_node.InnerText);
                                    if(foundRole != null)
                                    {
                                        //add this function/process name into AllRoles list
                                        foundRole.functionNames.Add(flow.currentProcessName);
                                        if(rolesCount>1 && !tempMultiRoles.roles.Contains(foundRole))
                                        {
                                            tempMultiRoles.roles.Add(foundRole);
                                        }
                                    }
                                    else
                                    {
                                        tempRole.functionNames.Add(flow.currentProcessName);
                                        allRoles.Add(tempRole);
                                        if (rolesCount > 1 && !tempMultiRoles.roles.Contains(foundRole))
                                        {
                                            tempMultiRoles.roles.Add(foundRole);
                                        }
                                    }
                                }
                            }
                            if(rolesCount>1)
                            {
                                allMultiRoles.Add(tempMultiRoles);
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
            YawlTask task_temp = new YawlTask();
            task_temp.name = decomposition_node.Attributes.GetNamedItem("id").InnerXml;
            var flow = allFlows.Find(x => x.currentProcessName == task_temp.name);
            if (flow != null)
            {
                task_temp.processFlow = flow;

                
                foreach(XmlNode para in decomposition_node.ChildNodes)
                {
                    if (para.GetType().Name == "XmlElement")
                    {
                        XmlElement e_para = (XmlElement)para;
                        XmlNode paraTypeNode = e_para.GetElementsByTagName("type").Item(0);
                        XmlNode paraNameNode = e_para.GetElementsByTagName("name").Item(0);
                        if(paraTypeNode != null && paraNameNode != null)
                        {
                            string strParaType = paraTypeNode.InnerText;
                            string strParaName = paraNameNode.InnerText;
                            if (strParaType == "Type")//solidity_modifier")
                            {
                                #region modifiers in function
                                /*//modifiers
                                var paraModif = allModifiers.Find(x => x.name == paraName.InnerText);
                                if(!task_temp.modifiers.Contains(paraModif))
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
                                    task_temp.modifiers.Add(paraModif);
                                }*/
                                #endregion
                                if (task_temp.actionType == null || task_temp.actionType == "")
                                {
                                    task_temp.actionType = strParaName;
                                    if(strParaName=="pay")
                                    {
                                        XmlNode paraValueNode = e_para.GetElementsByTagName("defaultValue").Item(0);
                                        if(paraValueNode!=null)
                                        {
                                            var findPayVariable = allLocalVariables.Find(x => x.name == paraValueNode.InnerText);
                                            if(findPayVariable!=null)
                                            {
                                                task_temp.payTypeVariable = findPayVariable;
                                            }
                                        }
                                    }
                                }
                            }
                            //InputParam, outputParam and in/out
                            //In YAWL, inputParam is for output;
                            //OutputParam is for input;
                            //Therefore, we do a reverse here.
                            else
                            {
                                var paraVari = allLocalVariables.Find(x => x.name == strParaName);
                                if (paraVari != null)
                                {
                                    if (para.Name == "inputParam")
                                    {
                                        var findResult = task_temp.inputVariables.Find(x => x.name == strParaName);
                                        if (findResult != null)
                                        {
                                            task_temp.inputVariables.Remove(findResult);
                                            task_temp.inOutVariables.Add(findResult);
                                        }
                                        else
                                        {
                                            task_temp.outputVariables.Add(paraVari);
                                        }
                                    }
                                    else if (para.Name == "outputParam")
                                    {

                                        var findResult = task_temp.outputVariables.Find(x => x.name == strParaName);
                                        if (findResult != null)
                                        {
                                            task_temp.outputVariables.Remove(findResult);
                                            task_temp.inOutVariables.Add(findResult);
                                        }
                                        else
                                        {
                                            task_temp.inputVariables.Add(paraVari);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            allTasks.Add(task_temp);
        }

        public void parseYawlRoles(string text)
        {
            //parse YAWL roles
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            //file name
            XmlNodeList roles_nodelist = doc.GetElementsByTagName("role");
            
            foreach(XmlNode role_node in roles_nodelist)
            {
                if(role_node.GetType().Name == "XmlElement")
                {
                    XmlElement e_role_node = (XmlElement)role_node;
                    var RoleName_node = e_role_node.GetElementsByTagName("name").Item(0);
                    var RoleAddress_node = e_role_node.GetElementsByTagName("description").Item(0);
                    if(RoleName_node != null && RoleAddress_node != null)
                    {
                        var str_RoleId = e_role_node.GetAttribute("id");
                        if(str_RoleId != null && str_RoleId != "")
                        {
                            //find this role in allRoles list
                            var foundRole = allRoles.Find(x => x.id == str_RoleId);
                            if(foundRole!=null)
                            {
                                //put the function names and types in all roles
                                foreach(var functionName in foundRole.functionNames)
                                {
                                    var funTemp = allTasks.Find(x => x.name == functionName);
                                    if(funTemp != null && funTemp.processFlow != null)
                                    {
                                        foreach(var processTempRole in funTemp.processFlow.currentProcessRoles)
                                        {
                                            if(processTempRole.id == str_RoleId)
                                            {
                                                processTempRole.name = RoleName_node.InnerText;
                                                processTempRole.address = RoleAddress_node.InnerText;
                                            }
                                        }
                                        if((funTemp.actionType!=null||funTemp.actionType!="") 
                                            && !foundRole.actionTypes.Contains(funTemp.actionType))
                                        {
                                            foundRole.actionTypes.Add(funTemp.actionType);
                                        }
                                    }
                                }
                                //foundRole.name = RoleName_node.InnerText;
                                //foundRole.address = RoleAddress_node.InnerText;
                            }
                            else
                            {
                                //This role is not in allRoles list
                                Role role_temp = new Role();
                                role_temp.id = str_RoleId;
                                role_temp.name = RoleName_node.InnerText;
                                role_temp.address = RoleAddress_node.InnerText;
                                allRoles.Add(role_temp);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region BPMN
        void parseBPMN(string text)
        {

        }
        #endregion
    }
}
