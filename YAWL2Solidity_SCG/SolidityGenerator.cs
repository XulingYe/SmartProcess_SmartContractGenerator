using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAWL2Solidity_SCG
{
    public class SolidityGenerator
    {
        public string solidityAllText = "";
        public string solidityFileName = "default";

        public string generateSolidityText(YAWLParser yAWL)
        {
            if(yAWL.allDefinedEnums.Count<=0)
            {
                solidityAllText = "error,no data in the yawl";
            }
            else
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

                //local variables
                solidityAllText += "\n//Local variables\n";
                foreach (var localvari_yawl in yAWL.allLocalVariables)
                {
                    solidityAllText += "    " + localvari_yawl.type + "  public " + localvari_yawl.name;
                    if(localvari_yawl.defaultVaule != "" && localvari_yawl.defaultVaule != "0")
                    {
                        solidityAllText += " = " + localvari_yawl.defaultVaule; 
                    }
                    solidityAllText += ";\n";
                }

                //modifier
                solidityAllText += "\n//Modifier\n";
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

                //functions
                solidityAllText += "\n//Functions\n";



                solidityAllText += "\n}";
            }
            return solidityAllText;
        }
    }
}
