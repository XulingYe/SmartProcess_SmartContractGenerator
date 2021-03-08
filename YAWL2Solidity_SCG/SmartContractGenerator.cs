using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphical2SmartContact_SCG
{
    public class SmartContractGenerator
    {
        public string solidityAllText = "";
        public string solidityFileName = "default";

        public string generateSolidityText(GraphicalParser yAWL)
        {
            
            solidityFileName = yAWL.fileName;
            solidityAllText = "pragma solidity >=0.4.22 <0.9.0;\n";
            solidityAllText += "contract " + yAWL.fileName + "{\n";

            //enums
            solidityAllText += "//Data type definition\n";
            foreach (var enum_yawl in yAWL.allDefinedEnums)
            {
                solidityAllText += "    enum " + enum_yawl.name + " { ";
                for(int i = 0; i < enum_yawl.elements.Count; i++)
                {
                    if(i>0)
                    {
                        solidityAllText += ", ";
                    }
                    solidityAllText += enum_yawl.elements[i];
                }
                solidityAllText += " }\n";
            }

            //state variables
            solidityAllText += "\n//Defined state variables\n";
            foreach (var localvari_yawl in yAWL.allLocalVariables)
            {
                solidityAllText += "    " + localvari_yawl.type + " "/*" public "*/ + localvari_yawl.name;
                if(localvari_yawl.defaultVaule != "" && localvari_yawl.defaultVaule != "0")
                {
                    solidityAllText += " = " + localvari_yawl.defaultVaule; 
                }
                solidityAllText += ";\n";
            }

            //modifiers
            solidityAllText += "\n//Modifiers\n";
            foreach (var modifier_yawl in yAWL.allModifiers)
            {
                solidityAllText += "    modifier " + modifier_yawl.name + "(";
                if(modifier_yawl.inputVaris.Count>0)
                {
                    for(int i = 0; i < modifier_yawl.inputVaris.Count; i++)
                    {
                        if(i>0)
                        {
                            solidityAllText += ", ";
                        }
                        solidityAllText += modifier_yawl.inputVaris[i].type + " " + modifier_yawl.inputVaris[i].name;
                    }
                }

                solidityAllText += "){\n        require(\n          " + modifier_yawl.condition
                    +",\n           \"" + modifier_yawl.errorString + "\"\n"
                    + "         );\n        _;\n    }\n";
            }

            //Automated generated process state based on process flows
            solidityAllText += "\n//Automated generated process state based on process flows\n";
            solidityAllText += "    enum ProcessFlow { ";
            int count = 0;
            string initailValue = "";
            foreach (var flow in  yAWL.allFlows)
            {
                if (count > 0)
                {
                    solidityAllText += ", ";
                }
                if (flow.currentProcessName != "InputCondition")
                {
                    solidityAllText += "To" + flow.currentProcessName;
                    count++;
                }
                else
                {
                    initailValue = "ProcessFlow.To" + flow.nextProcesses[0].processName;
                }
            }
            solidityAllText += " }\n\n";
            solidityAllText += "    ProcessFlow processFlow = " + initailValue + ";\n\n";
            //process flow modifier
            solidityAllText += "    modifier inProcessFlow(ProcessFlow _processFlow){\n"
                    + "        require(\n          processFlow == _processFlow,\n"
                    + "           \"Invalid state of the process flow.\"\n"
                    + "         );\n        _;\n    }\n";




            //functions
            solidityAllText += "\n//Functions\n";
            foreach (var function_yawl in yAWL.allFunctions)
            {
                solidityAllText += "    function " + function_yawl.name + "(";
                int countInputVaris = 0;
                //input parameters
                foreach(var inputVari in function_yawl.inputVariables)
                {
                    if (countInputVaris > 0)
                    {
                        solidityAllText += ", ";
                    }
                    if(inputVari.type=="string")
                    {
                        solidityAllText += inputVari.type + " memory _" + inputVari.name;
                    }
                    else
                    {
                        solidityAllText += inputVari.type + " _" + inputVari.name;
                    }
                    countInputVaris++;
                }
                //in/output variables for input
                foreach (var inOutputVari in function_yawl.inOutVariables)
                {
                    if (countInputVaris > 0)
                    {
                        solidityAllText += ", ";
                    }
                    if (inOutputVari.type == "string")
                    {
                        solidityAllText += inOutputVari.type + " memory " + inOutputVari.name;
                    }
                    else
                    {
                        solidityAllText += inOutputVari.type + inOutputVari.name;
                    }
                    countInputVaris++;
                }
                solidityAllText += ")\n        public\n";
                //modifiers
                foreach (var modifi in function_yawl.modifiers)
                {
                    solidityAllText += "        "+ modifi.name + "(";
                    for (int i = 0; i < modifi.inputVaris.Count; i++)
                    {
                        if (i > 0)
                        {
                            solidityAllText += ",";
                        }
                        solidityAllText += modifi.inputVaris[i].defaultVaule;
                    }
                    solidityAllText += ")\n";
                }
                solidityAllText += "        inProcessFlow(ProcessFlow.To" + function_yawl.name + ")\n";
                //return parameters
                if (function_yawl.outputVariables.Count>0)
                {
                    solidityAllText += "        returns (";
                    int countOutputVaris = 0;
                    foreach(var outputVari in function_yawl.outputVariables)
                    {
                        if (countOutputVaris > 0)
                        {
                            solidityAllText += ",";
                        }
                        if(outputVari.type=="string")
                        {
                            solidityAllText += outputVari.type + " memory _" + outputVari.name;
                        }
                        else
                        {
                            solidityAllText += outputVari.type +" _" + outputVari.name;
                        }
                            
                        countOutputVaris++;
                    }
                    //in/output variables for output
                    foreach (var inOutputVari in function_yawl.inOutVariables)
                    {
                        if (countOutputVaris > 0)
                        {
                            solidityAllText += ", ";
                        }
                        if(inOutputVari.type == "string")
                        {
                            solidityAllText +=  inOutputVari.type + " memory " + inOutputVari.name;
                        }
                        else
                        {
                            solidityAllText += inOutputVari.type +" " + inOutputVari.name;
                        }
                            
                        countOutputVaris++;
                    }
                    solidityAllText += ")\n";
                }
                solidityAllText += "    {\n";
                //Take input variables to state variables
                foreach(var inputVariForState in function_yawl.inputVariables)
                {
                    if(yAWL.allLocalVariables.Exists(x=>x.name == inputVariForState.name))
                    {
                        solidityAllText += "        " + inputVariForState.name+ " = _" + inputVariForState.name + ";\n";
                    }
                }
                //Take state variables to output variables 
                foreach (var outputVariFromState in function_yawl.outputVariables)
                {
                    if(outputVariFromState.defaultVaule != "" && outputVariFromState.defaultVaule != "0")
                    {
                        solidityAllText += "        _" + outputVariFromState.name + " = " + outputVariFromState.defaultVaule + ";\n";
                    }
                    else if (yAWL.allLocalVariables.Exists(x => x.name == outputVariFromState.name))
                    {
                        solidityAllText += "        _" + outputVariFromState.name + " = " + outputVariFromState.name + ";\n";
                    }
                }
                //deal with process flow

                solidityAllText += "    }\n\n";
            }


            solidityAllText += "}";
            
            return solidityAllText;
        }
    }
}
