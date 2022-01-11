using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphical2SmartContact_SCG
{
    public class SmartContractComponents
    {
        //MultiRolesModifier is used to generate a modifier with more than one role.
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
            public List<DefineEnum> enums = new List<DefineEnum>();
            public List<SCGVariable> stateVariables = new List<SCGVariable>();
            public List<Modifier> modifiers = new List<Modifier>();
            public List<Function> functions = new List<Function>();
        };
        public class Modifier
        {
            public string name;
            public List<Parameter> inputParam = new List<Parameter>();
            public string statementsText;
        };
        public class Parameter
        {
            public string name;
            public string type;
        };

        public class Function
        {
            public string name;
            public List<Parameter> inputParam = new List<Parameter>();
            public List<string> calledModifiers = new List<string>();
            public List<string> keywords = new List<string>();
            public List<Parameter> returnVaris = new List<Parameter>();
            public string statementsText;
            public string actionType; //tag action
        }
        public List<SolidityFile> allSolidityFiles = new List<SolidityFile>();
        public List<MultiRolesModifier> allMultiModifiers = new List<MultiRolesModifier>();
    }
}
