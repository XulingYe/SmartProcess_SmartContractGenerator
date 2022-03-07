using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.ProcessComponents;
using static Graphical2SmartContact_SCG.SCGParser;
using static Graphical2SmartContact_SCG.SmartContractComponents;
using static Graphical2SmartContact_SCG.SCGChecker;

namespace Graphical2SmartContact_SCG
{
    public class SCGTranslator
    {
        //SCGChecker checker = new SCGChecker();
        /*public string solidityAllText = "";
        public string solidityProcessFlowAllText = "";
        public string SolidityMainContractName = "default";*/
        
        public void generateSolidityText(ProcessComponents pcs, SmartContractComponents sccs, SCGChecker checker)
        {
            sccs.allSolidityFiles.Clear();

            SolidityFile sFile = new SolidityFile();
            pcs.fileName = checker.checkNameValid(pcs.fileName);
            sFile.contractName = pcs.fileName;

            //SC process flow smart contract
            string SCProcessFlow_contractName = "SCProcessFlow";
            sFile.fileAllText += "import \"./" + SCProcessFlow_contractName + ".sol\";\n\n";
            generateSolidityProcessFlow(pcs.allFlows, SCProcessFlow_contractName, sccs, checker);

            
            sFile.fileAllText += "contract " + pcs.fileName + " is " + SCProcessFlow_contractName + "{\n";
            sFile.parentContracts.Add(SCProcessFlow_contractName);
            //enums
            sFile.fileAllText += "//Data type definition\n";
            foreach (var enum_graphical in pcs.allDefinedEnums)
            {
                DefineEnum enumTemp = new DefineEnum();
                enum_graphical.enumName = checker.checkNameValid(enum_graphical.enumName);
                sFile.fileAllText += "    enum " + enum_graphical.enumName + " { ";
                enumTemp.enumName = enum_graphical.enumName;

                for (int i = 0; i < enum_graphical.enumValues.Count; i++)
                {
                    if(i>0)
                    {
                        sFile.fileAllText += ", ";
                    }
                    sFile.fileAllText += enum_graphical.enumValues[i];
                    enumTemp.enumValues.Add(enum_graphical.enumValues[i]);
                }
                sFile.fileAllText += " }\n";
                sFile.enums.Add(enumTemp);
            }

            //state variables
            sFile.fileAllText += "\n//Defined state variables\n";
            foreach (var localvari_graphical in pcs.allLocalVariables)
            {
                if(localvari_graphical.type != "Action")//Action type should not be stored
                {
                    SCGVariable variableTemp = new SCGVariable();
                    sFile.fileAllText += "    " + localvari_graphical.type + " "/*" public "*/ + localvari_graphical.name;
                    variableTemp.type = localvari_graphical.type;
                    variableTemp.name = localvari_graphical.name;
                    if (localvari_graphical.value != null && localvari_graphical.value != "" 
                        && localvari_graphical.value != "0")
                    {
                        if(localvari_graphical.type=="string")
                        {
                            sFile.fileAllText += " = \"" + localvari_graphical.value + "\"";
                            variableTemp.value = "\"" + localvari_graphical.value + "\"";
                        }
                        else 
                        {
                            sFile.fileAllText += " = " + localvari_graphical.value;
                            variableTemp.value = localvari_graphical.value;
                        }
                    
                    }
                    sFile.fileAllText += ";\n";
                    sFile.stateVariables.Add(variableTemp);
                }

            }

            //roles in state variables
            sFile.fileAllText += "\n//Roles in state variables\n";
            var strRolesInModifier = "";
            foreach (var role_graphical in pcs.allRoles)
            {
                SCGVariable variTemp = new SCGVariable();
                //state variable
                sFile.fileAllText += "    address ";
                variTemp.type = "address";
                if (role_graphical.actionTypes.Contains("pay"))
                {
                    sFile.fileAllText += "payable ";
                    variTemp.type += " payable";
                }
                role_graphical.name = checker.checkNameValid(role_graphical.name);
                sFile.fileAllText += role_graphical.name;
                variTemp.name = role_graphical.name;

                if (role_graphical.address != null && role_graphical.address != "")
                {
                    sFile.fileAllText += " = ";
                    string strRoleValue;
                    if(role_graphical.actionTypes.Contains("pay"))
                    {
                        strRoleValue = "payable(" + role_graphical.address + ")";
                    }
                    else
                    {
                        strRoleValue = role_graphical.address;
                    }
                    sFile.fileAllText += strRoleValue;
                    variTemp.value = strRoleValue;
                }
                sFile.fileAllText += ";\n";
                sFile.stateVariables.Add(variTemp);

                //modifier
                Modifier tempModi = new Modifier();
                strRolesInModifier += "    modifier Only" + role_graphical.name + "(){\n        ";
                tempModi.name = "Only" + role_graphical.name;
                tempModi.statementsText = "require(msg.sender == " + role_graphical.name
                    + ",\" Only " + role_graphical.name + " can call this function.\");";
                strRolesInModifier += tempModi.statementsText;
                strRolesInModifier += "\n        _;\n    }\n";
                sFile.modifiers.Add(tempModi);
            }

            //defined modifiers in graphical representations
            /*sFile.fileAllText += "\n//Modifiers\n";
            foreach (var modifier_graphical in graphicalP.allModifiers)
            {
                sFile.fileAllText += "    modifier " + modifier_graphical.name + "(";
                if(modifier_graphical.inputVaris.Count>0)
                {
                    for(int i = 0; i < modifier_graphical.inputVaris.Count; i++)
                    {
                        if(i>0)
                        {
                            sFile.fileAllText += ", ";
                        }
                        sFile.fileAllText += modifier_graphical.inputVaris[i].type 
                            + " " + modifier_graphical.inputVaris[i].name;
                    }
                }

                sFile.fileAllText += "){\n        require(\n          " + modifier_graphical.condition
                    +",\n           \"" + modifier_graphical.errorString + "\"\n"
                    + "         );\n        _;\n    }\n";
            }*/

            //Roles in modifiers
            sFile.fileAllText += "\n//Roles in modifiers\n";
            sFile.fileAllText += strRolesInModifier;
            //MultiRoles in modifiers
            foreach(var multiRolesModifier in pcs.allMultiRoles)
            {
                Modifier temp_modifier = new Modifier();
                var modifierName = "Only";
                var modifierCondition = "";
                var modifierErrorMessage = "Only ";
                MultiRolesModifier tempMultiRolesModifier = new MultiRolesModifier();
                tempMultiRolesModifier.roles = multiRolesModifier.roles;
                for(int i = 0; i< multiRolesModifier.roles.Count; i++)
                {
                    multiRolesModifier.roles[i].name = checker.checkNameValid(multiRolesModifier.roles[i].name);
                    var strRoleName = multiRolesModifier.roles[i].name;
                    if(i>0)
                    {
                        modifierName += "Or";
                        modifierCondition += " || ";
                        modifierErrorMessage += " or ";
                    }
                    modifierName += strRoleName;
                    modifierCondition += "msg.sender == " + strRoleName + "Address";
                    modifierErrorMessage += strRoleName;

                }
                //Modifier
                sFile.fileAllText += "    modifier " + modifierName + "(){\n        ";
                temp_modifier.statementsText = "require(" + modifierCondition + ",\" " + modifierErrorMessage
                    + " can call this function.\");";
                sFile.fileAllText += temp_modifier.statementsText;
                sFile.fileAllText += "\n        _;\n    }\n";
                tempMultiRolesModifier.modifierName = modifierName;
                sccs.allMultiModifiers.Add(tempMultiRolesModifier);
                //add into file
                temp_modifier.name = modifierName;
                sFile.modifiers.Add(temp_modifier); 
            }
            

            //functions
            sFile.fileAllText += "\n//Functions\n";
            foreach (var function_yawl in pcs.allTasks)
            {
                //Function fun_temp;
                sFile.fileAllText += addSolidityFunction(function_yawl, pcs.allLocalVariables, sccs, checker, out Function fun_temp);
                sFile.functions.Add(fun_temp);
            }
            sFile.fileAllText += "}";
            sccs.allSolidityFiles.Add(sFile);
        }

        void generateSolidityProcessFlow(List<Flow> allFlows, string contractName, SmartContractComponents sccs, SCGChecker checker)
        {
            SolidityFile file = new SolidityFile();
            file.contractName = checker.checkNameValid(contractName);
            
            file.fileAllText += "contract " + file.contractName + "{\n";

            //Automated generated process state based on process flows
            file.fileAllText += "\n//Automated generated process state based on process flows\n";
            file.fileAllText += "    enum ProcessFlow { ";
            DefineEnum enumProcessFlowTemp = new DefineEnum();
            enumProcessFlowTemp.enumName = "ProcessFlow";
            int count = 0;
            string initailValue = "";
            foreach (var flow in allFlows)
            {
                if (count > 0)
                {
                    file.fileAllText += ", ";
                }
                if (flow.currentTaskID != "InputCondition")
                {
                    string strProcessName = "To" + flow.currentTaskID;
                    file.fileAllText += strProcessName;
                    enumProcessFlowTemp.enumValues.Add(strProcessName);
                    count++;
                }
                else
                {
                    initailValue = "ProcessFlow.To" + flow.nextTasks[0].taskID;
                }
            }
            file.fileAllText += " }\n\n";
            file.enums.Add(enumProcessFlowTemp);

            //current process flow
            file.fileAllText += "    ProcessFlow[] currentProcessFlows;\n\n";// = " + initailValue + ";\n\n";
            SCGVariable variTemp = new SCGVariable();
            variTemp.name = "currentProcessFlows";
            variTemp.type = "ProcessFlow[]";
            file.stateVariables.Add(variTemp);

            //process flow modifier
            file.fileAllText += "    modifier inProcessFlow(ProcessFlow _processFlow){\n        ";
            var strModStatement = "for(uint i=0; i<currentProcessFlows.length; i++)\n        {\n"
                    + "           if(currentProcessFlows[i] == _processFlow)\n           {\n"
                    + "             _;\n             return;\n           }\n        }\n        "
                    + "revert(\"Invalid state of the process flow. Please check by getCurrentProcessState().\");";
            file.fileAllText += strModStatement + "\n    }\n\n";
            //add modifier to allSolidityFiles list
            Modifier modTemp = new Modifier();
            modTemp.name = "inProcessFlow";
            Parameter paraTemp = new Parameter();
            paraTemp.name = "_processFlow";
            paraTemp.type = "ProcessFlow";
            modTemp.inputParam.Add(paraTemp);
            modTemp.statementsText = strModStatement;
            file.modifiers.Add(modTemp);

            //contractor
            file.fileAllText += "    constructor()\n    {\n"
                    + "        currentProcessFlows.push(" + initailValue + ");\n    }\n\n";
            
            //getCurrentProcessState() function
            file.fileAllText += "    function getCurrentProcessState()\n        public\n        view\n"
                    + "        returns(ProcessFlow[] memory)\n    {\n"
                    + "        return currentProcessFlows;\n    }\n\n";
            //////////////
            Function funTemp1 = new Function();
            funTemp1.name = "getCurrentProcessState";
            funTemp1.keywords.Add("public");
            funTemp1.keywords.Add("view");
            Parameter funTemp1retpara = new Parameter();
            funTemp1retpara.type = "ProcessFlow[] memory";
            funTemp1.returnVaris.Add(funTemp1retpara);
            file.functions.Add(funTemp1);

            //deleteFlow(ProcessFlow) function
            file.fileAllText += "    function deleteFlow(ProcessFlow _processFlow)\n        internal\n    {\n        ";
            var strFunTemp2Statement = "for(uint i=0; i<currentProcessFlows.length; i++)\n        {\n"
                    + "            if(currentProcessFlows[i] == _processFlow)\n            {\n                "
                    + "currentProcessFlows[i] = currentProcessFlows[currentProcessFlows.length-1];\n"
                    + "                currentProcessFlows.pop();\n            }\n        }";
            file.fileAllText += strFunTemp2Statement + "\n    }\n\n";
            ///////
            Function funTemp2 = new Function();
            funTemp2.name = "deleteFlow";
            Parameter funTemp2intpara = new Parameter();
            funTemp2intpara.name = "_processFlow";
            funTemp2intpara.type = "ProcessFlow";
            funTemp2.keywords.Add("internal");
            funTemp2.statementsText = strFunTemp2Statement;
            file.functions.Add(funTemp2);

            file.fileAllText += "}\n";


            sccs.allSolidityFiles.Add(file);
        }

        string addSolidityFunction(GraphicalTask task,List<SCGVariable> loclaVariables, SmartContractComponents sccs, SCGChecker checker, out Function func)
        {
            //In Graphical workflow, inputParam is for output;
            //OutputParam is for input;
            //Therefore, we do a reverse here.
            string function_text = "";
            //task.taskID = checker.checkNameValid(task.taskID);
            function_text += "    //This "+ task.taskID + " function is for " + task.taskName + "\n";
            function_text += "    function " + task.taskID + "(";
            func = new Function();
            func.name = task.taskID;
            int countInputVaris = 0;
            //input parameters
            foreach (var inputVari in task.outputVariables)
            {
                Parameter para_temp = new Parameter();
                para_temp.name = inputVari.name;
                if (countInputVaris > 0)
                {
                    function_text += ", ";
                }
                if (inputVari.type == "string")
                {
                    function_text += inputVari.type + " memory " + inputVari.name;
                    para_temp.type = inputVari.type + " memory";
                }
                else
                {
                    function_text += inputVari.type + " " + inputVari.name;
                    para_temp.type = inputVari.type;
                }
                countInputVaris++;
                func.inputParam.Add(para_temp);
            }
            //in/output variables for input
            /*foreach (var inOutputVari in task.inOutVariables)
            {
                if (countInputVaris > 0)
                {
                    function_text += ", ";
                }
                if (inOutputVari.type == "string")
                {
                    function_text += inOutputVari.type + " memory _" + inOutputVari.name;
                }
                else
                {
                    function_text += inOutputVari.type + " _" + inOutputVari.name;
                }
                countInputVaris++;
            }*/
            function_text += ")\n        public\n";
            func.keywords.Add("public");
            /*if (function.inputVariables.Count == 0)
            {
                function_text += "        view\n";
            }*/
            if (task.actionType == "pay")// && task.payTypeVariable != null)
            {
                function_text += "        payable\n";
                func.keywords.Add("payable");
            }
            /*
            //modifiers
            foreach (var modifi in task.modifiers)
            {
                function_text += "        " + modifi.name + "(";
                for (int i = 0; i < modifi.inputVaris.Count; i++)
                {
                    if (i > 0)
                    {
                        function_text += ",";
                    }
                    function_text += modifi.inputVaris[i].defaultVaule;
                }
                function_text += ")\n";
            }*/
            if(task.operateRoles.Count == 1)
            {
                var str_temp = "Only" + task.operateRoles[0].name + "()";
                func.calledModifiers.Add(str_temp);
                function_text += "        " + str_temp + " \n";//////////////
            }
            else if (task.operateRoles.Count > 1)
            {
                var str_temp = getMultiRolesModifierName(task.operateRoles,sccs) + "()";
                function_text += "        " + str_temp + "\n";
                func.calledModifiers.Add(str_temp);
            }
            var str_temp2 = "inProcessFlow(ProcessFlow.To" + task.taskID + ")";
            function_text += "        " + str_temp2 + "\n";
            func.calledModifiers.Add(str_temp2);

            //return parameters
            if (task.inputVariables.Count > 0)
            {
                function_text += "        returns (";
                int countOutputVaris = 0;
                foreach (var outputVari in task.inputVariables)
                {
                    Parameter returnPara_temp = new Parameter();
                    returnPara_temp.name = outputVari.name;
                    if (countOutputVaris > 0)
                    {
                        function_text += ",";
                    }
                    if (outputVari.type == "string")
                    {
                        function_text += outputVari.type + " memory";// + outputVari.name;
                        returnPara_temp.type = outputVari.type + " memory";
                    }
                    else
                    {
                        function_text += outputVari.type;// + " " + outputVari.name;
                        returnPara_temp.type = outputVari.type;
                    }
                    countOutputVaris++;
                    func.returnVaris.Add(returnPara_temp);
                }
                //in/output variables for output
                /*foreach (var inOutputVari in task.inOutVariables)
                {
                    if (countOutputVaris > 0)
                    {
                        function_text += ", ";
                    }
                    if (inOutputVari.type == "string")
                    {
                        function_text += inOutputVari.type + " memory " + inOutputVari.name;
                    }
                    else
                    {
                        function_text += inOutputVari.type + " " + inOutputVari.name;
                    }

                    countOutputVaris++;
                }*/
                function_text += ")\n";
            }
            function_text += "    {\n";
            //Take input variables to state variables
            /*foreach (var inputVariForState in task.inputVariables)
            {
                if (loclaVariables.Exists(x => x.name == inputVariForState.name))
                {
                    function_text += "        " + inputVariForState.name + " = " + inputVariForState.name + ";\n";
                }
            }*/
            //Check for pay type
            if(task.actionType == "pay" && task.payTypeVariable!=null)
            {
                if(task.operateRoles.Count==1)
                {
                    function_text += "        " + task.operateRoles[0].name + "Payable.transfer("
                        + task.payTypeVariable.name+");\n";
                }
                else if(task.operateRoles.Count > 1)
                {
                    //TODO: more than one roles case
                }
            }

            //Take state variables to output variables 
            foreach (var outputVariFromState in task.outputVariables)
            {
                //It is a state variable
                if (loclaVariables.Exists(x => x.name == outputVariFromState.name))
                {
                    function_text += "        return " + outputVariFromState.name + ";\n";
                }
                else if (outputVariFromState.value != null
                    && outputVariFromState.value != "" && outputVariFromState.value != "0")
                {
                    function_text += "        return "  + outputVariFromState.value + ";\n";
                }
            }

            function_text += "        deleteFlow(ProcessFlow.To"+ task.taskID+");\n";
            //deal with process flow
            if(task.processFlow.nextTasks.Count >= 1)
            {
                if(task.processFlow.nextTasks.Count == 1)
                {
                    if(task.processFlow.nextTasks[0].taskID!= "OutputCondition")
                    {
                        function_text += "        currentProcessFlows.push(ProcessFlow.To" 
                            + task.processFlow.nextTasks[0].taskID + ");\n";
                    }
                    
                }
                else if (task.processFlow.splitOperation == "and")
                {
                    foreach(var nextproc in task.processFlow.nextTasks)
                    {
                        if (nextproc.taskID != "OutputCondition")
                        {
                            function_text += "        currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.taskID + ");\n";
                        }
                        
                    }
                }
                else if(task.processFlow.splitOperation == "xor")
                {
                    var strElseText = "        else\n        {\n";
                    bool isFirstIf = true;
                    foreach (var nextproc in task.processFlow.nextTasks)
                    {
                        if(nextproc.condition == "otherwise")
                        {
                            if(nextproc.taskID != "OutputCondition")
                            {
                                strElseText += "            currentProcessFlows.push(ProcessFlow.To"
                                    + nextproc.taskID + ");\n"; 
                            }
                            strElseText +="        }\n";
                        }
                        else if(isFirstIf)
                        {
                            function_text += "        if(" + nextproc.condition + ")\n        {\n";
                            if (nextproc.taskID != "OutputCondition")
                            {
                                function_text += "            currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.taskID + ");\n";
                            }
                            function_text +="        }\n";
                            isFirstIf = false;
                        }
                        else
                        {
                            function_text += "        else if(" + nextproc.condition + ")\n        {\n";
                            if (nextproc.taskID != "OutputCondition")
                            {
                                function_text += "            currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.taskID + ");\n";
                            }
                            function_text += "        }\n";
                        }
                    }
                    function_text += strElseText;
                }
            }
            
            function_text += "    }\n\n";
            return function_text;
        }

        string getMultiRolesModifierName(List<Role> roles, SmartContractComponents sccs)
        {
            string nameResult = "Error";
            foreach(var multiRoles in sccs.allMultiModifiers)
            {
                if(roles == multiRoles.roles)
                {
                    nameResult = multiRoles.modifierName;
                }
                else if(roles.Count == multiRoles.roles.Count)
                {
                    bool isMatch = true;
                    foreach(var tempRole in roles)
                    {
                        if(!multiRoles.roles.Contains(tempRole))
                        {
                            isMatch = false;
                        }
                    }
                    if(isMatch)
                    {
                        nameResult = multiRoles.modifierName;
                    }
                }
            }
            return nameResult;
        }
    }
}
