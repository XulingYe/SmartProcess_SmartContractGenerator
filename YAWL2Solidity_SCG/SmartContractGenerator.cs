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
        public class SolidityFile
        {
            public string contractName = "";
            public string fileAllText = "pragma solidity >=0.4.22 <0.9.0;\n\n";
        };

        public List<SolidityFile> allSolidityFiles = new List<SolidityFile>();

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

            //enums
            sFile.fileAllText += "//Data type definition\n";
            foreach (var enum_graphical in graphicalP.allDefinedEnums)
            {
                sFile.fileAllText += "    enum " + enum_graphical.name + " { ";
                for(int i = 0; i < enum_graphical.elements.Count; i++)
                {
                    if(i>0)
                    {
                        sFile.fileAllText += ", ";
                    }
                    sFile.fileAllText += enum_graphical.elements[i];
                }
                sFile.fileAllText += " }\n";
            }

            //state variables
            sFile.fileAllText += "\n//Defined state variables\n";
            foreach (var localvari_graphical in graphicalP.allLocalVariables)
            {
                sFile.fileAllText += "    " + localvari_graphical.type + " "/*" public "*/ + localvari_graphical.name;
                if (localvari_graphical.defaultVaule != null && localvari_graphical.defaultVaule != "" 
                    && localvari_graphical.defaultVaule != "0")
                {
                    sFile.fileAllText += " = " + localvari_graphical.defaultVaule; 
                }
                sFile.fileAllText += ";\n";
            }

            //roles in state variables
            sFile.fileAllText += "\n//Roles in state variables\n";
            var strRolesInModifier = "";
            foreach (var role_graphical in graphicalP.allRoles)
            {
                //state variable
                sFile.fileAllText += "    address " + role_graphical.name + "Address";
                if (role_graphical.address != null && role_graphical.address != "")
                {
                    sFile.fileAllText += " = " + role_graphical.address;
                }
                sFile.fileAllText += ";\n";
                //modifier
                strRolesInModifier += "    modifier Only" + role_graphical.name + "(){\n"
                    + "        require(msg.sender == " + role_graphical.name
                    + "Address,\" Only " + role_graphical.name
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
                    file.fileAllText += "To" + flow.currentProcessName;
                    count++;
                }
                else
                {
                    initailValue = "ProcessFlow.To" + flow.nextProcesses[0].processName;
                }
            }
            file.fileAllText += " }\n\n";

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
            file.fileAllText += "    function getCurrentProcessState()\n        public\n"
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
                    function_text += inOutputVari.type + " memory " + inOutputVari.name;
                }
                else
                {
                    function_text += inOutputVari.type + inOutputVari.name;
                }
                countInputVaris++;
            }
            function_text += ")\n        public\n";
            /*if (function.inputVariables.Count == 0)
            {
                function_text += "        view\n";
            }*/
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
            function_text += "        inProcessFlow(ProcessFlow.To" + function.name + ")\n";
            foreach(var funRoletemp in function.ProcessFlow.currentProcessRoles)
            {
                function_text += "        Only" + funRoletemp.name + "()\n";
            }
            
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
                        function_text += inOutputVari.type + " memory " + inOutputVari.name;
                    }
                    else
                    {
                        function_text += inOutputVari.type + " " + inOutputVari.name;
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
            if(function.ProcessFlow.nextProcesses.Count >= 1)
            {
                if(function.ProcessFlow.nextProcesses.Count == 1)
                {
                    if(function.ProcessFlow.nextProcesses[0].processName!= "OutputCondition")
                    {
                        function_text += "        currentProcessFlows.push(ProcessFlow.To" 
                            + function.ProcessFlow.nextProcesses[0].processName + ");\n";
                    }
                    
                }
                else if (function.ProcessFlow.splitOperation == "and")
                {
                    foreach(var nextproc in function.ProcessFlow.nextProcesses)
                    {
                        if (nextproc.processName != "OutputCondition")
                        {
                            function_text += "        currentProcessFlows.push(ProcessFlow.To"
                                + nextproc.processName + ");\n";
                        }
                        
                    }
                }
                else if(function.ProcessFlow.splitOperation == "xor")
                {
                    var strElseText = "        else\n        {\n";
                    bool isFirstIf = true;
                    foreach (var nextproc in function.ProcessFlow.nextProcesses)
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
    }
}
