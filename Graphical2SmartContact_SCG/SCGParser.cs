using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Graphical2SmartContact_SCG.ProcessComponents;

namespace Graphical2SmartContact_SCG
{
    public class SCGParser
    {
        public void parseGraphical(string text, bool isBPMN, string fileName, ProcessComponents pcs)
        {
            //clear all previous data
            pcs.allLocalVariables.Clear();
            pcs.allDefinedEnums.Clear();
            pcs.allFlows.Clear();
            pcs.allTasks.Clear();
            //allModifiers.Clear();
            pcs.allRoles.Clear();

            pcs.fileName = fileName;
            pcs.isBPMN = isBPMN;

            if (!isBPMN)
            {
                parseYawl(text, pcs);
            }
            else
            {
                parseBPMN(text, pcs);
            }


        }

        #region YAWL
        void parseYawl(string text, ProcessComponents pcs)
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
                //pcs.fileName = e_specification_yawl.Attributes.GetNamedItem("uri").InnerXml;

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
                                                addLocalVariable(e_LVinRootNet, originalType, pcs);
                                            }

                                        }
                                    }
                                    //process flow, here should initial all the functions
                                    else if (e_LVinRootNet.Name == "processControlElements")
                                    {
                                        foreach (XmlNode flow_node in e_LVinRootNet.ChildNodes)
                                        {
                                            addProcessFlow(flow_node, pcs);
                                        }
                                    }
                                }

                            }
                        }
                        //others: functions
                        else
                        {
                            addYawlTask(decomposition_node, pcs);
                        }
                    }
                }
            }
        }

        void addLocalVariable(XmlElement e_LVinRootNet, string originalType, ProcessComponents pcs)
        {
            ProcessComponents.SCGVariable lv_temp = new ProcessComponents.SCGVariable();
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
                if (pcs.allDefinedEnums.Exists(x => x.enumName == lv_temp.type) && !lvValue_node.InnerXml.Contains(lv_temp.type))
                {
                    lv_temp.value = lv_temp.type + "." + lvValue_node.InnerXml;
                }
                else
                {
                    lv_temp.value = lvValue_node.InnerXml;
                }

            }

            pcs.allLocalVariables.Add(lv_temp);
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

        void addProcessFlow(XmlNode flow_node, ProcessComponents pcs)
        {
            ProcessComponents.Flow flow = new ProcessComponents.Flow();
            if (flow_node.Name == "inputCondition" || flow_node.Name == "task")
            {
                XmlNode flow_id = flow_node.Attributes.GetNamedItem("id");
                if (flow_id != null)
                {
                    flow.currentTaskID = flow_id.InnerXml;
                    if (flow_node.GetType().Name == "XmlElement")
                    {
                        XmlElement e_flow_input = (XmlElement)flow_node;
                        //flowsInto
                        XmlNodeList next_nodes = e_flow_input.GetElementsByTagName("flowsInto");
                        if (next_nodes != null)
                        {
                            foreach (XmlNode nextNode in next_nodes)
                            {
                                ToNextTask nextProcess = new ToNextTask();
                                foreach (XmlNode flowInfo in nextNode.ChildNodes)
                                {
                                    if (flowInfo.Name == "nextElementRef")
                                    {
                                        nextProcess.taskID = flowInfo.Attributes.GetNamedItem("id").InnerXml;
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
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "<=", pcs.allLocalVariables);
                                            }
                                            else if (conditions[1].Contains(">="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], ">=", pcs.allLocalVariables);
                                            }
                                            else if (conditions[1].Contains("!="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "!=", pcs.allLocalVariables);
                                            }
                                            else if (conditions[1].Contains("<"))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "<", pcs.allLocalVariables);
                                            }
                                            else if (conditions[1].Contains(">"))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], ">", pcs.allLocalVariables);
                                            }
                                            else if (conditions[1].Contains("="))
                                            {
                                                nextProcess.condition = generateCondition(variables.Last(), conditions[1], "=", pcs.allLocalVariables);
                                            }
                                        }
                                    }
                                    else if (flowInfo.Name == "isDefaultFlow")
                                    {
                                        nextProcess.condition = "otherwise";
                                    }
                                }
                                flow.nextTasks.Add(nextProcess);
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
                        if (rolesCount > 0)
                        {
                            var tempMultiRoles = new ProcessComponents.MultiRoles();
                            foreach (XmlNode role_node in roles_nodes)
                            {
                                //role's id is not empty
                                if (role_node.InnerText != null && role_node.InnerText != "")
                                {
                                    //Step 1: add this role into allRoles list
                                    var foundRole = pcs.allRoles.Find(x => x.id == role_node.InnerText);
                                    if (foundRole != null)
                                    {
                                        //add this function/process name into AllRoles list
                                        foundRole.TaskIDs.Add(flow.currentTaskID);
                                        if (rolesCount > 1 && !tempMultiRoles.roles.Contains(foundRole))
                                        {
                                            tempMultiRoles.roles.Add(foundRole);
                                        }
                                    }
                                    else
                                    {
                                        //create a new role into AllRoles list
                                        foundRole = new ProcessComponents.Role();
                                        foundRole.id = role_node.InnerText;
                                        foundRole.TaskIDs.Add(flow.currentTaskID);
                                        pcs.allRoles.Add(foundRole);
                                        if (rolesCount > 1 && !tempMultiRoles.roles.Contains(foundRole))
                                        {
                                            tempMultiRoles.roles.Add(foundRole);
                                        }
                                    }

                                    //Step 2: add this role into the corresponding task
                                    var foundTask = pcs.allTasks.Find(x => x.taskID == flow.currentTaskID);

                                    //Task not existed
                                    if (foundTask == null)
                                    {
                                        foundTask = new GraphicalTask();
                                        foundTask.taskID = flow.currentTaskID;
                                        foundTask.operateRoles.Add(foundRole);
                                        foundTask.processFlow = flow;
                                        pcs.allTasks.Add(foundTask);
                                    }
                                    else
                                    {
                                        //TODO: should not be this case. But if yes, handle later

                                    }
                                }
                            }
                            if (rolesCount > 1)
                            {
                                pcs.allMultiRoles.Add(tempMultiRoles);
                            }
                        }
                    }
                    pcs.allFlows.Add(flow);
                }
            }
        }

        string generateCondition(string variableName, string fullCondition, string operation, List<SCGVariable> allLocalVariables)
        {
            string result;
            string[] op = new string[] { operation };
            var value = fullCondition.Split(op, StringSplitOptions.RemoveEmptyEntries).Last();
            if (value.Contains('\''))
            {
                value = value.Trim('\'', ' ');
            }
            var variable = allLocalVariables.Find(x => x.name == variableName);
            if (variable != null)
            {
                if (variable.type == "string")
                {
                    value = "\"" + value + "\"";
                }
                if (operation == "=")
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

        void addYawlTask(XmlNode decomposition_node, ProcessComponents pcs)
        {
            var taskID = decomposition_node.Attributes.GetNamedItem("id").InnerXml;
            var foundTask = pcs.allTasks.Find(x => x.taskID == taskID);

            if (foundTask == null)
            {
                foundTask = new GraphicalTask();
                foundTask.taskID = taskID;
            }
            if (foundTask.processFlow == null)
            {
                var flow = pcs.allFlows.Find(x => x.currentTaskID == taskID);
                if (flow != null)
                {
                    foundTask.processFlow = flow;
                }
            }

            foreach (XmlNode para in decomposition_node.ChildNodes)
            {
                if (para.GetType().Name == "XmlElement")
                {
                    XmlElement e_para = (XmlElement)para;
                    if (e_para.Name == "name")
                    {
                        foundTask.taskName = e_para.InnerText;
                    }
                    else
                    {
                        XmlNode paraTypeNode = e_para.GetElementsByTagName("type").Item(0);
                        XmlNode paraNameNode = e_para.GetElementsByTagName("name").Item(0);
                        if (paraTypeNode != null && paraNameNode != null)
                        {
                            string strParaType = paraTypeNode.InnerText;
                            string strParaName = paraNameNode.InnerText;
                            if (strParaType == "Action")//solidity_modifier")
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
                                if (foundTask.actionType == null || foundTask.actionType == "")
                                {
                                    foundTask.actionType = strParaName;
                                    if (strParaName == "pay")
                                    {
                                        XmlNode paraValueNode = e_para.GetElementsByTagName("defaultValue").Item(0);
                                        if (paraValueNode != null)
                                        {
                                            var findPayVariable = pcs.allLocalVariables.Find(x => x.name == paraValueNode.InnerText);
                                            if (findPayVariable != null)
                                            {
                                                foundTask.payTypeVariable = findPayVariable;
                                                /////Wrong here!
                                            }
                                        }
                                    }
                                }
                            }

                            //In Graphical workflow, inputParam is for output;
                            //OutputParam is for input;
                            //Therefore, we do a reverse here.
                            /*else
                            {
                                var paraVari = pcs.allLocalVariables.Find(x => x.name == strParaName);
                                if (paraVari != null)
                                {
                                    if (para.Name == "inputParam")
                                    {
                                        var findResult = task_temp.inputVariables.Find(x => x.name == strParaName);
                                        if (findResult != null)
                                        {
                                            task_temp.inputVariables.Remove(findResult);
                                            //task_temp.inOutVariables.Add(findResult);
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
                                            //task_temp.inOutVariables.Add(findResult);
                                        }
                                        else
                                        {
                                            task_temp.inputVariables.Add(paraVari);
                                        }
                                    }
                                }
                            }*/
                        }
                    }

                }
            }

            pcs.allTasks.Add(foundTask);
        }

        public void parseYawlRoles(string text, ProcessComponents pcs)
        {
            //parse YAWL roles
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            //file name
            XmlNodeList roles_nodelist = doc.GetElementsByTagName("role");

            foreach (XmlNode role_node in roles_nodelist)
            {
                if (role_node.GetType().Name == "XmlElement")
                {
                    XmlElement e_role_node = (XmlElement)role_node;
                    var RoleName_node = e_role_node.GetElementsByTagName("name").Item(0);
                    var RoleAddress_node = e_role_node.GetElementsByTagName("description").Item(0);
                    if (RoleName_node != null && RoleAddress_node != null)
                    {
                        var str_RoleId = e_role_node.GetAttribute("id");
                        if (str_RoleId != null && str_RoleId != "")
                        {
                            //find this role in allRoles list
                            var foundRole = pcs.allRoles.Find(x => x.id == str_RoleId);
                            if (foundRole != null)
                            {
                                //put the function names and types in all roles
                                foreach (var functionName in foundRole.TaskIDs)
                                {
                                    var funTemp = pcs.allTasks.Find(x => x.taskID == functionName);
                                    if (funTemp != null && funTemp.processFlow != null)
                                    {
                                        foreach (var processTempRole in funTemp.operateRoles)
                                        {
                                            if (processTempRole.id == str_RoleId)
                                            {
                                                processTempRole.name = RoleName_node.InnerText;
                                                processTempRole.address = RoleAddress_node.InnerText;
                                            }
                                        }
                                        if ((funTemp.actionType != null || funTemp.actionType != "")
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
                                ProcessComponents.Role role_temp = new ProcessComponents.Role();
                                role_temp.id = str_RoleId;
                                role_temp.name = RoleName_node.InnerText;
                                role_temp.address = RoleAddress_node.InnerText;
                                pcs.allRoles.Add(role_temp);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region BPMN
        void parseBPMN(string text, ProcessComponents pcs)
        {
            //parse BPMN
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text);

            //participants and message flow
            XmlNode bpmnCollaboration_node = doc.GetElementsByTagName("bpmn:collaboration").Item(0);
            if (bpmnCollaboration_node != null && bpmnCollaboration_node.GetType().Name == "XmlElement")
            {
                XmlElement e_bpmnCollaboration = (XmlElement)bpmnCollaboration_node;

                //Print detail of participants
                XmlNodeList bpmnParticipants_nodeList = e_bpmnCollaboration.GetElementsByTagName("bpmn:participant");
                if (bpmnParticipants_nodeList != null && bpmnParticipants_nodeList.Count > 0)
                {
                    foreach (var bpmnParticipant_node in bpmnParticipants_nodeList)
                    {
                        if (bpmnParticipant_node != null && bpmnParticipant_node.GetType().Name == "XmlElement")
                        {
                            XmlElement e_bpmnParticipant = (XmlElement)bpmnParticipant_node;
                            //Store info of each participant
                            Role roleTemp = new Role();
                            roleTemp.id = e_bpmnParticipant.GetAttribute("id");
                            roleTemp.name = e_bpmnParticipant.GetAttribute("name");
                            roleTemp.bpmnProcessName = e_bpmnParticipant.GetAttribute("processRef");
                            pcs.allRoles.Add(roleTemp);
                        }
                    }
                }
                //message flow
                XmlNodeList bpmnMessageFlows_nodeList = e_bpmnCollaboration.GetElementsByTagName("bpmn:messageFlow");
                if (bpmnMessageFlows_nodeList != null && bpmnMessageFlows_nodeList.Count > 0)
                {
                    foreach (var bpmnMessageFlow_node in bpmnMessageFlows_nodeList)
                    {
                        if (bpmnMessageFlow_node != null && bpmnMessageFlow_node.GetType().Name == "XmlElement")
                        {
                            XmlElement e_bpmnMessageFlow = (XmlElement)bpmnMessageFlow_node;

                            var flowSourceEndID = e_bpmnMessageFlow.GetAttribute("sourceRef");
                            var flowTargetEndID = e_bpmnMessageFlow.GetAttribute("targetRef");

                            //the two connected ends of a message flow would be either a task, or a role.
                            //In current case, we will only consider the message flow which connect two tasks.
                            if (!pcs.allRoles.Exists(x => x.id == flowSourceEndID) && !pcs.allRoles.Exists(x => x.id == flowTargetEndID))
                            {
                                //Store info of each message flow into allFlows
                                Flow flowTemp = new Flow();
                                flowTemp.flowID = e_bpmnMessageFlow.GetAttribute("id");
                                flowTemp.currentTaskID = flowSourceEndID;
                                ToNextTask nextTaskTemp = new ToNextTask();
                                nextTaskTemp.taskID = flowTargetEndID;
                                flowTemp.nextTasks.Add(nextTaskTemp);
                                pcs.allFlows.Add(flowTemp);

                                //create source end task
                                var foundSourceTask = pcs.allTasks.Find(x => x.taskID == flowSourceEndID);
                                if (foundSourceTask == null)
                                {
                                    foundSourceTask = new GraphicalTask();
                                    foundSourceTask.taskID = flowSourceEndID;
                                }
                                if (foundSourceTask.processFlow == null)
                                {
                                    foundSourceTask.processFlow = flowTemp;
                                }
                                pcs.allTasks.Add(foundSourceTask);

                                //create target end task
                                var foundTargetTask = pcs.allTasks.Find(x => x.taskID == flowTargetEndID);
                                if (foundTargetTask == null)
                                {
                                    foundTargetTask = new GraphicalTask();
                                    foundTargetTask.taskID = flowTargetEndID;
                                }
                                if (foundTargetTask.processFlow == null)
                                {
                                    foundTargetTask.processFlow = flowTemp;
                                }
                                pcs.allTasks.Add(foundTargetTask);
                            }

                        }
                    }
                }
            }

            //Process of each participant
            XmlNodeList bpmnProcesses_nodeList = doc.GetElementsByTagName("bpmn:process");
            foreach (var bpmnProcess_node in bpmnProcesses_nodeList)
            {
                if (bpmnProcess_node != null && bpmnProcess_node.GetType().Name == "XmlElement")
                {
                    XmlElement e_bpmnProcess = (XmlElement)bpmnProcess_node;
                    var processID = e_bpmnProcess.GetAttribute("id");
                    var foundRole = pcs.allRoles.Find(x => x.bpmnProcessName == processID);
                    if (foundRole != null)
                    {
                        //step 1: add address
                        parseBPMNextensions(e_bpmnProcess, foundRole, pcs.allLocalVariables);
                        //step 2: add each task into allTasks and foundRole


                    }
                    else
                    {
                        //TODO: error message
                    }
                }
            }
        }

        //store properties (action or address), in-output variables 
        //case 1: address
        private void parseBPMNextensions(XmlElement e_bpmnProcess, Role currentRole, List<SCGVariable> allVariables)
        {
            var extensionElements = e_bpmnProcess.GetElementsByTagName("bpmn:extensionElements").Item(0);
            if (extensionElements != null && extensionElements.ChildNodes != null & extensionElements.ChildNodes.Count > 0)
            {
                foreach(var elementChildNode in extensionElements.ChildNodes)
                {
                    if (elementChildNode != null && elementChildNode.GetType().Name == "XmlElement")
                    {
                        XmlElement e_elementChild = (XmlElement)elementChildNode;

                        //properties, name should be address
                        if(e_elementChild.Name == "camunda:properties")
                        {
                            if(e_elementChild.ChildNodes != null && e_elementChild.ChildNodes.Count > 0)
                            {
                                foreach(var bpmnProperty_node in e_elementChild.ChildNodes)
                                {
                                    if(bpmnProperty_node != null && bpmnProperty_node.GetType().Name == "XmlElement")
                                    {
                                        XmlElement e_bpmnProperty = (XmlElement)bpmnProperty_node;
                                        var propertyName = e_bpmnProperty.GetAttribute("name");
                                        var propertyValue = e_bpmnProperty.GetAttribute("value");
                                        if(propertyName == "address")
                                        {
                                            //add it to currentRole
                                            currentRole.address = propertyValue;

                                            //add it to allVariables
                                            var variTemp = new SCGVariable();
                                            variTemp.name = currentRole.name;
                                            variTemp.type = propertyName;
                                            variTemp.value = propertyValue;
                                            allVariables.Add(variTemp);
                                        }
                                        else
                                        {
                                            //TODO: should only be address, please handle other cases. Or anything missing?
                                        }
                                    }
                                }
                            }
                        }
                        //should not be the case
                        else
                        {
                            //TODO: a case that is not considered
                        }

                    }
                }
            }
        }

        //case 2: Action and in-output variables
        private void parseBPMNextensions(XmlElement e_bpmnProcess, GraphicalTask currentTask, ProcessComponents pcs)
        {
            var extensionElements = e_bpmnProcess.GetElementsByTagName("bpmn:extensionElements").Item(0);
            if (extensionElements != null && extensionElements.ChildNodes != null & extensionElements.ChildNodes.Count > 0)
            {
                foreach (var elementChildNode in extensionElements.ChildNodes)
                {
                    if (elementChildNode != null && elementChildNode.GetType().Name == "XmlElement")
                    {
                        XmlElement e_elementChild = (XmlElement)elementChildNode;

                        //properties, name should be address or Action
                        if (e_elementChild.Name == "camunda:properties")
                        {

                        }
                        //in-output
                        else if (e_elementChild.Name == "camunda:inputOutput")
                        {

                        }

                    }
                }
            }
        }
        #endregion
    }
}

