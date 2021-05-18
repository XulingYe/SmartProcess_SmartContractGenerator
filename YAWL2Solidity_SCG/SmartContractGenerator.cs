using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.GraphicalParser;

namespace Graphical2SmartContact_SCG
{
    public class SmartContractGenerator
    {        
        public class MultiRolesModifier
        {
            public string modifierName;
            public List<Role> roles = new List<Role>();
        }
        public class SolidityFile
        {
            public string contractName = "";
            public string fileAllText = "// SPDX-License-Identifier: GPL-3.0\npragma solidity >=0.4.22 <0.9.0;\n\n";
            public List<string> parentContracts = new List<string>();
            public List<SolidityEnum> allEnums = new List<SolidityEnum>();
            public List<StateVariable> allVariables = new List<StateVariable>();

        };
        public class SolidityEnum
        {
            public string enumName;
            public List<string> enumValues = new List<string>();
        }
        public class StateVariable
        {
            public string variableName;
            public string value;
            public string type;
        }


        public List<SolidityFile> allSolidityFiles = new List<SolidityFile>();
        public List<MultiRolesModifier> allMultiModifiers = new List<MultiRolesModifier>();

        /*public string solidityAllText = "";
        public string solidityProcessFlowAllText = "";
        public string SolidityMainContractName = "default";*/

        public void generateSolidityText(GraphicalParser graphicalP)
        {
            allSolidityFiles.Clear();

            SolidityFile sFile = new SolidityFile();
            sFile.contractName = graphicalP.fileName;

            //SC process flow smart contract
            string SolidityProcessFlowContractName = "SCProcessFlow";
            sFile.fileAllText += "import \"./" + SolidityProcessFlowContractName + ".sol\";\n\n";
            generateSolidityProcessFlow(graphicalP.allFlows, SolidityProcessFlowContractName);

            sFile.fileAllText += "contract " + graphicalP.fileName + " is " + SolidityProcessFlowContractName + "{\n";
            sFile.parentContracts.Add(SolidityProcessFlowContractName);
            //enums
            sFile.fileAllText += "//Data type definition\n";
            foreach (var enum_graphical in graphicalP.allDefinedEnums)
            {
                SolidityEnum enumTemp = new SolidityEnum();
                sFile.fileAllText += "    enum " + enum_graphical.name + " { ";
                enumTemp.enumName = enum_graphical.name;

                for (int i = 0; i < enum_graphical.elements.Count; i++)
                {
                    if(i>0)
                    {
                        sFile.fileAllText += ", ";
                    }
                    sFile.fileAllText += enum_graphical.elements[i];
                    enumTemp.enumValues.Add(enum_graphical.elements[i]);
                }
                sFile.fileAllText += " }\n";
                sFile.allEnums.Add(enumTemp);
            }

            //state variables
            sFile.fileAllText += "\n//Defined state variables\n";
            foreach (var localvari_graphical in graphicalP.allLocalVariables)
            {
                StateVariable variableTemp = new StateVariable();
                sFile.fileAllText += "    " + localvari_graphical.type + " "/*" public "*/ + localvari_graphical.name;
                variableTemp.type = localvari_graphical.type;
                variableTemp.variableName = localvari_graphical.name;
                if (localvari_graphical.defaultVaule != null && localvari_graphical.defaultVaule != "" 
                    && localvari_graphical.defaultVaule != "0")
                {
                    if(localvari_graphical.type=="string")
                    {
                        sFile.fileAllText += " = \"" + localvari_graphical.defaultVaule + "\"";
                        variableTemp.value = "\"" + localvari_graphical.defaultVaule + "\"";
                    }
                    else
                    {
                        sFile.fileAllText += " = " + localvari_graphical.defaultVaule;
                        variableTemp.value = localvari_graphical.defaultVaule;
                    }
                    
                }
                sFile.fileAllText += ";\n";
                sFile.allVariables.Add(variableTemp);
            }

            //roles in state variables
            sFile.fileAllText += "\n//Roles in state variables\n";
            var strRolesInModifier = "";
            foreach (var role_graphical in graphicalP.allRoles)
            {
                //state variable
                sFile.fileAllText += "    address " + role_graphical.name;
                if (role_graphical.address != null && role_graphical.address != "")
                {
                    sFile.fileAllText += " = " + role_graphical.address;
                }
                sFile.fileAllText += ";\n";
                if(role_graphical.actionTypes.Contains("pay"))
                {
                    sFile.fileAllText += "    address payable "+ role_graphical.name
                        +"Payable = payable(" + role_graphical.name + ");\n";
                }
                //modifier
                strRolesInModifier += "    modifier Only" + role_graphical.name + "(){\n"
                    + "        require(msg.sender == " + role_graphical.name
                    + ",\" Only " + role_graphical.name
                    + " can call this function.\");\n        _;\n    }\n";
            }

            //modifiers
            sFile.fileAllText += "\n//Modifiers\n";
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
            }

            //Roles in modifiers
            sFile.fileAllText += "\n//Roles in modifiers\n";
            sFile.fileAllText += strRolesInModifier;
            //MultiRoles in modifiers
            foreach(var multiRolesModifier in graphicalP.allMultiRoles)
            {
                var modifierName = "Only";
                var modifierCondition = "";
                var modifierErrorMessage = "Only ";
                MultiRolesModifier tempMultiRolesModifier = new MultiRolesModifier();
                tempMultiRolesModifier.roles = multiRolesModifier.roles;
                for(int i = 0; i< multiRolesModifier.roles.Count; i++)
                {
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
                sFile.fileAllText += "    modifier " + modifierName + "(){\n"
                    + "        require("+modifierCondition+",\" " + modifierErrorMessage
                    + " can call this function.\");\n        _;\n    }\n";
                tempMultiRolesModifier.modifierName = modifierName;
                allMultiModifiers.Add(tempMultiRolesModifier);
            }
            

            //functions
            sFile.fileAllText += "\n//Functions\n";
            foreach (var function_yawl in graphicalP.allFunctions)
            {
                sFile.fileAllText += addSolidityFunction(function_yawl, graphicalP.allLocalVariables);
            }
            sFile.fileAllText += "}";

            allSolidityFiles.Add(sFile);
        }

        void generateSolidityProcessFlow(List<Flow> allFlows, string contractName)
        {
            SolidityFile file = new SolidityFile();
            file.contractName = contractName;
            
            file.fileAllText += "contract " + contractName + "{\n";

            //Automated generated process state based on process flows
            file.fileAllText += "\n//Automated generated process state based on process flows\n";
            file.fileAllText += "    enum ProcessFlow { ";
            SolidityEnum enumProcessFlowTemp = new SolidityEnum();
            enumProcessFlowTemp.enumName = "ProcessFlow";
            int count = 0;
            string initailValue = "";
            foreach (var flow in allFlows)
            {
                if (count > 0)
                {
                    file.fileAllText += ", ";
                }
                if (flow.currentProcessName != "InputCondition")
                {
                    string strProcessName = "To" + flow.currentProcessName;
                    file.fileAllText += strProcessName;
                    enumProcessFlowTemp.enumValues.Add(strProcessName);
                    count++;
                }
                else
                {
                    initailValue = "ProcessFlow.To" + flow.nextProcesses[0].processName;
                }
            }
            file.fileAllText += " }\n\n";
            file.allEnums.Add(enumProcessFlowTemp);

            //current process flow
            file.fileAllText += "    ProcessFlow[] currentProcessFlows;\n\n";// = " + initailValue + ";\n\n";

            //process flow modifier
            file.fileAllText += "    modifier inProcessFlow(ProcessFlow _processFlow){\n"
                    + "        for(uint i=0; i<currentProcessFlows.length; i++)\n        {\n"
                    + "           if(currentProcessFlows[i] == _processFlow)\n           {\n"
                    + "             _;\n             return;\n           }\n        }\n        "
                    + "revert(\"Invalid state of the process flow. Please check by getCurrentProcessState().\");\n    }\n\n";
            
            //contractor
            file.fileAllText += "    constructor()\n    {\n"
                    + "        currentProcessFlows.push(" + initailValue + ");\n    }\n\n";
            
            //getCurrentProcessState() function
            file.fileAllText += "    function getCurrentProcessState()\n        public\n        view\n"
                    + "        returns(ProcessFlow[] memory)\n    {\n"
                    + "        return currentProcessFlows;\n    }\n\n";

            //deleteFlow(ProcessFlow) function
            file.fileAllText += "    function deleteFlow(ProcessFlow _processFlow)\n        internal\n"
                    + "    {\n        for(uint i=0; i<currentProcessFlows.length; i++)\n        {\n"
                    + "            if(currentProcessFlows[i] == _processFlow)\n            {\n                "
                    + "currentProcessFlows[i] = currentProcessFlows[currentProcessFlows.length-1];\n"
                    + "                currentProcessFlows.pop();\n            }\n        }\n    }\n\n";

            file.fileAllText += "}\n";

            allSolidityFiles.Add(file);
        }

        string addSolidityFunction(Function function,List<Variable> loclaVariables)
        {
            string function_text = "";

            function_text += "    function " + function.name + "(";
            int countInputVaris = 0;
            //input parameters
            foreach (var inputVari in function.inputVariables)
            {
                if (countInputVaris > 0)
                {
                    function_text += ", ";
                }
                if (inputVari.type == "string")
                {
                    function_text += inputVari.type + " memory _" + inputVari.name;
                }
                else
                {
                    function_text += inputVari.type + " _" + inputVari.name;
                }
                countInputVaris++;
            }
            //in/output variables for input
            foreach (var inOutputVari in function.inOutVariables)
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
            }
            function_text += ")\n        public\n";
            /*if (function.inputVariables.Count == 0)
            {
                function_text += "        view\n";
            }*/
            if (function.actionType == "pay" && function.payTypeVariable != null)
            {
                function_text += "        payable\n";
            }
            //modifiers
            foreach (var modifi in function.modifiers)
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
            }
            if(function.processFlow.currentProcessRoles.Count == 1)
            {
                function_text += "        Only" + function.processFlow.currentProcessRoles[0].name + "()\n";
            }
            else if (function.processFlow.currentProcessRoles.Count > 1)
            {
                function_text += "        " + getMultiRolesModifierName(function.processFlow.currentProcessRoles)+"()\n";
            }
            function_text += "        inProcessFlow(ProcessFlow.To" + function.name + ")\n";
            
            //return parameters
            if (function.outputVariables.Count > 0)
            {
                function_text += "        returns (";
                int countOutputVaris = 0;
                foreach (var outputVari in function.outputVariables)
                {
                    if (countOutputVaris > 0)
                    {
                        function_text += ",";
                    }
                    if (outputVari.type == "string")
                    {
                        function_text += outputVari.type + " memory _" + outputVari.name;
                    }
                    else
                    {
                        function_text += outputVari.type + " _" + outputVari.name;
                    }

                    countOutputVaris++;
                }
                //in/output variables for output
                foreach (var inOutputVari in function.inOutVariables)
                {
                    if (countOutputVaris > 0)
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

                    countOutputVaris++;
                }
                function_text += ")\n";
            }
            function_text += "    {\n";
            //Take input variables to state variables
            foreach (var inputVariForState in function.inputVariables)
            {
                if (loclaVariables.Exists(x => x.name == inputVariForState.name))
                {
                    function_text += "        " + inputVariForState.name + " = _" + inputVariForState.name + ";\n";
                }
            }
            //Check for pay type
            if(function.actionType == "pay" && function.payTypeVariable!=null)
            {
                if(function.processFlow.currentProcessRoles.Count==1)
                {
                    function_text += "        " + function.processFlow.currentProcessRoles[0].name + "Payable.transfer("
                        +function.payTypeVariable.name+");\n";
                }
                else if(function.processFlow.currentProcessRoles.Count > 1)
                {
                    //TODO: more than one roles case
                }
            }

            //Take state variables to output variables 
            foreach (var outputVariFromState in function.outputVariables)
            {
                if (outputVariFromState.defaultVaule != null
                    && outputVariFromState.defaultVaule != "" && outputVariFromState.defaultVaule != "0")
                {
                    function_text += "        _" + outputVariFromState.name + " = " + outputVariFromState.defaultVaule + ";\n";
                }
                else if (loclaVariables.Exists(x => x.name == outputVariFromState.name))
                {
                    function_text += "        _" + outputVariFromState.name + " = " + outputVariFromState.name + ";\n";
                }
            }

            function_text += "        deleteFlow(ProcessFlow.To"+ function.name+");\n";
            //deal with process flow
            if(function.processFlow.nextProcesses.Count >= 1)
            {
                if(function.processFlow.nextProcesses.Count == 1)
                {
                    if(function.processFlow.nextProcesses[0].processName!= "OutputCondition")
                    {
                        function_text += "        currentProcessFlows.push(ProcessFlow.To" 
                            + function.processFlow.nextProcesses[0].processName + ");\n";
                    }
                    
                }
                else if (function.processFlow.splitOperation == "and")
                {
                    foreach(var nextproc in function.processFlow.nextProcesses)
                    {
                        if (nextproc.processName != "OutputCondition")
                        {
                            function_text += "        currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.processName + ");\n";
                        }
                        
                    }
                }
                else if(function.processFlow.splitOperation == "xor")
                {
                    var strElseText = "        else\n        {\n";
                    bool isFirstIf = true;
                    foreach (var nextproc in function.processFlow.nextProcesses)
                    {
                        if(nextproc.condition == "otherwise")
                        {
                            if(nextproc.processName != "OutputCondition")
                            {
                                strElseText += "            currentProcessFlows.push(ProcessFlow.To"
                                    + nextproc.processName + ");\n"; 
                            }
                            strElseText +="        }\n";
                        }
                        else if(isFirstIf)
                        {
                            function_text += "        if(" + nextproc.condition + ")\n        {\n";
                            if (nextproc.processName != "OutputCondition")
                            {
                                function_text += "            currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.processName + ");\n";
                            }
                            function_text +="        }\n";
                            isFirstIf = false;
                        }
                        else
                        {
                            function_text += "        else if(" + nextproc.condition + ")\n        {\n";
                            if (nextproc.processName != "OutputCondition")
                            {
                                function_text += "            currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.processName + ");\n";
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

        string getMultiRolesModifierName(List<Role> roles)
        {
            string nameResult = "Error";
            foreach(var multiRoles in allMultiModifiers)
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
