using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.ProcessComponents;

namespace Graphical2SmartContact_SCG
{
    public class SmartContractComponents
    {
        public List<SmartContract> allSmartContracts = new List<SmartContract>();
        public List<MultiParticipantsModifier> allMultiModifiers = new List<MultiParticipantsModifier>();

        //MultiParticipantsModifier is used to generate a modifier with more than one participant.
        public class MultiParticipantsModifier
        {
            public string modifierName;
            public List<Participant> participants = new List<Participant>();
        }
        public class SmartContract
        {
            public string contractName = "";
            public string fileAllText = "// SPDX-License-Identifier: GPL-3.0\npragma solidity >=0.4.22 <0.9.0;\n\n";
            public List<string> parentContracts = new List<string>();
            public List<DefineEnum> enums = new List<DefineEnum>();
            public List<SCGVariable> stateVariables = new List<SCGVariable>();
            public List<Modifier> modifiers = new List<Modifier>();
            public List<Function> functions = new List<Function>();
            public List<SCGStruct> structs = new List<SCGStruct>();
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
            public List<Parameter> inputParams = new List<Parameter>();
            public List<string> calledModifiers = new List<string>();
            public List<string> keywords = new List<string>();
            public List<Parameter> returnVaris = new List<Parameter>();
            public string statementsText;
            public List<string> calledFunctions = new List<string>(); 
        }

        public class SCGStruct
        {
            public string structName;
            public List<Parameter> parameters = new List<Parameter>();
        }
    }
}
