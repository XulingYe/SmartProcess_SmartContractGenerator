using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.ProcessComponents;
using static Graphical2SmartContact_SCG.SCGParser;
using static Graphical2SmartContact_SCG.SmartContractComponents;
using static Graphical2SmartContact_SCG.SCGChecker;
using static Graphical2SmartContact_SCG.ActionTranslator;

namespace Graphical2SmartContact_SCG
{
    public class SCGTranslator
    {
        //SCGChecker checker = new SCGChecker();
        /*public string solidityAllText = "";
        public string solidityProcessFlowAllText = "";
        public string SolidityMainContractName = "default";*/
        
        public void generateSolidityMain(ProcessComponents pcs, SmartContractComponents sccs, SCGChecker checker)
        {
            sccs.allSmartContracts.Clear();

            SmartContract mainContract = new SmartContract();
            pcs.fileName = checker.checkNameValid(pcs.fileName);
            mainContract.contractName = pcs.fileName;

            //SCG flow contract
            string SCProcessFlow_contractName = "SCGFlow";
            mainContract.fileAllText += "import \"./" + SCProcessFlow_contractName + ".sol\";\n\n";
            generateSCGFlowContract(pcs.allFlows, SCProcessFlow_contractName, sccs, checker);
            mainContract.parentContracts.Add(SCProcessFlow_contractName);

            //SCG action contract
            string SCGAction_contractName = "SCGAction";
            mainContract.fileAllText += "import \"./" + SCGAction_contractName + ".sol\";\n\n";
            generateSCGActionContract(pcs.allActions, SCGAction_contractName, sccs, checker);
            mainContract.parentContracts.Add(SCGAction_contractName);

            mainContract.fileAllText += "contract " + pcs.fileName + " is ";
            //all the parent contracts
            for(int i = 0; i< mainContract.parentContracts.Count; i++)
            {
                mainContract.fileAllText += mainContract.parentContracts[i];
                if (i< mainContract.parentContracts.Count-1)
                {
                    mainContract.fileAllText += ", ";
                }
            }
            mainContract.fileAllText += "{\n";
            
            //enums
            mainContract.fileAllText += "//Data type definition\n";
            foreach (var enum_graphical in pcs.allDefinedEnums)
            {
                DefineEnum enumTemp = new DefineEnum();
                enum_graphical.enumName = checker.checkNameValid(enum_graphical.enumName);
                mainContract.fileAllText += "    enum " + enum_graphical.enumName + " { ";
                enumTemp.enumName = enum_graphical.enumName;

                for (int i = 0; i < enum_graphical.enumValues.Count; i++)
                {
                    if(i>0)
                    {
                        mainContract.fileAllText += ", ";
                    }
                    mainContract.fileAllText += enum_graphical.enumValues[i];
                    enumTemp.enumValues.Add(enum_graphical.enumValues[i]);
                }
                mainContract.fileAllText += " }\n";
                mainContract.enums.Add(enumTemp);
            }

            //state variables
            mainContract.fileAllText += "\n//Defined state variables\n";
            foreach (var localvari_graphical in pcs.allLocalVariables)
            {
                if(localvari_graphical.type != "Action")//Action type should not be stored
                {
                    SCGVariable variableTemp = new SCGVariable();
                    mainContract.fileAllText += "    " + localvari_graphical.type + " "/*" public "*/ + localvari_graphical.name;
                    variableTemp.type = localvari_graphical.type;
                    variableTemp.name = localvari_graphical.name;
                    if (localvari_graphical.value != null && localvari_graphical.value != "" 
                        && localvari_graphical.value != "0")
                    {
                        if(localvari_graphical.type=="string")
                        {
                            mainContract.fileAllText += " = \"" + localvari_graphical.value + "\"";
                            variableTemp.value = "\"" + localvari_graphical.value + "\"";
                        }
                        else 
                        {
                            mainContract.fileAllText += " = " + localvari_graphical.value;
                            variableTemp.value = localvari_graphical.value;
                        }
                    
                    }
                    mainContract.fileAllText += ";\n";
                    mainContract.stateVariables.Add(variableTemp);
                }

            }

            //participants in state variables
            mainContract.fileAllText += "\n//Participants in state variables\n";
            //var strParticipantsInModifier = "";
            foreach (var participant_graphical in pcs.allParticipants)
            {
                SCGVariable variTemp = new SCGVariable();
                //state variable
                mainContract.fileAllText += "    address ";
                variTemp.type = "address";
                if (participant_graphical.actionTypes.Contains("pay"))
                {
                    mainContract.fileAllText += "payable ";
                    variTemp.type += " payable";
                }
                participant_graphical.id = checker.checkNameValid(participant_graphical.id);
                mainContract.fileAllText += participant_graphical.id;
                variTemp.name = participant_graphical.id;
                var founAddr = participant_graphical.allInfo.Find(x => x.type == "address");
                if (founAddr!=null)
                {
                    mainContract.fileAllText += " = ";
                    string strParticipantValue;
                    if(participant_graphical.actionTypes.Contains("pay"))
                    {
                        strParticipantValue = "payable(" + founAddr.value + ")";
                    }
                    else
                    {
                        strParticipantValue = founAddr.value;
                    }
                    mainContract.fileAllText += strParticipantValue;
                    variTemp.value = strParticipantValue;
                }
                mainContract.fileAllText += "; //" + participant_graphical.name + "\n";
                mainContract.stateVariables.Add(variTemp);

                
            }

            //defined modifiers in graphical representations
            mainContract.fileAllText += "\n//Modifiers\n";
            foreach (var eachParticipant in pcs.allParticipants)
            {
                //modifier
                Modifier tempModi = new Modifier();
                tempModi.name = "Only" + eachParticipant.name;
                mainContract.fileAllText += "    modifier " + tempModi.name + "("
                    + "){\n        ";
                tempModi.statementsText = "require(\n          msg.sender == "+ eachParticipant.id;

                /*var foundAddr = eachParticipant.allInfo.Find(x => x.type == "address");
                tempModi.statementsText += foundAddr.name;*/
                tempModi.statementsText += ",\n           \"Only " + eachParticipant.name + " can access this function.\"\n"
                    + "         );\n";


                mainContract.fileAllText += tempModi.statementsText +"        _;\n    }\n";
                mainContract.modifiers.Add(tempModi);
                
                
            }

            //Participants in modifiers
            //mainContract.fileAllText += "\n//Participants in modifiers\n";
            //mainContract.fileAllText += strParticipantsInModifier;
            //MultiParticipants in modifiers
            foreach(var multiParticipantsModifier in pcs.allMultiParticipants)
            {
                Modifier temp_modifier = new Modifier();
                var modifierName = "Only";
                var modifierCondition = "";
                var modifierErrorMessage = "Only ";
                MultiParticipantsModifier tempMultiParticipantsModifier = new MultiParticipantsModifier();
                tempMultiParticipantsModifier.participants = multiParticipantsModifier.participants;
                for(int i = 0; i< multiParticipantsModifier.participants.Count; i++)
                {
                    multiParticipantsModifier.participants[i].name = checker.checkNameValid(multiParticipantsModifier.participants[i].name);
                    var strParticipantName = multiParticipantsModifier.participants[i].name;
                    if(i>0)
                    {
                        modifierName += "Or";
                        modifierCondition += " || ";
                        modifierErrorMessage += " or ";
                    }
                    modifierName += strParticipantName;
                    modifierCondition += "msg.sender == " + strParticipantName + "Address";
                    modifierErrorMessage += strParticipantName;

                }
                //Modifier
                mainContract.fileAllText += "    modifier " + modifierName + "(){\n        ";
                temp_modifier.statementsText = "require(" + modifierCondition + ",\" " + modifierErrorMessage
                    + " can call this function.\");";
                mainContract.fileAllText += temp_modifier.statementsText;
                mainContract.fileAllText += "\n        _;\n    }\n";
                tempMultiParticipantsModifier.modifierName = modifierName;
                sccs.allMultiModifiers.Add(tempMultiParticipantsModifier);
                //add into file
                temp_modifier.name = modifierName;
                mainContract.modifiers.Add(temp_modifier); 
            }
            

            //functions
            mainContract.fileAllText += "\n//Functions\n";
            foreach (var function_yawl in pcs.allTasks)
            {
                //Function fun_temp;
                mainContract.fileAllText += addTaskFunction(function_yawl, pcs.allLocalVariables, sccs, checker, out Function fun_temp);
                mainContract.functions.Add(fun_temp);
            }
            mainContract.fileAllText += "}";
            sccs.allSmartContracts.Add(mainContract);
        }

        void generateSCGFlowContract(List<Flow> allFlows, string contractName, SmartContractComponents sccs, SCGChecker checker)
        {
            SmartContract file = new SmartContract();
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
                if (flow.sourceRef != "InputCondition")
                {
                    string strProcessName = "To" + flow.sourceRef;
                    file.fileAllText += strProcessName;
                    enumProcessFlowTemp.enumValues.Add(strProcessName);
                    count++;
                }
                else
                {
                    initailValue = "ProcessFlow.To" + flow.TargetRef;
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
            //add modifier to allSmartContracts list
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


            sccs.allSmartContracts.Add(file);
        }

        string addTaskFunction(SCGTask task,List<SCGVariable> loclaVariables, SmartContractComponents sccs, SCGChecker checker, out Function func)
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
            for(int i = 0;  i< task.actions.Count; i++)
            {
                if(i>0)
                { function_text += ", "; }
                function_text += translateInOutputPara(task.actions[i].inputVariables, func.inputParams, true);
            }
            function_text += ")\n        public\n";
            func.keywords.Add("public");
            /*if (function.inputVariables.Count == 0)
            {
                function_text += "        view\n";
            }*/
            if (task.actions.Exists(x=>x.actionID == "pay"))// && task.payTypeVariable != null)
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
            if(task.operateParticipants.Count == 1)
            {
                var str_temp = "Only" + task.operateParticipants[0].name + "()";
                func.calledModifiers.Add(str_temp);
                function_text += "        " + str_temp + " \n";//////////////
            }
            else if (task.operateParticipants.Count > 1)
            {
                var str_temp = getMultiParticipantsModifierName(task.operateParticipants,sccs) + "()";
                function_text += "        " + str_temp + "\n";
                func.calledModifiers.Add(str_temp);
            }
            var str_temp2 = "inProcessFlow(ProcessFlow.To" + task.taskID + ")";
            function_text += "        " + str_temp2 + "\n";
            func.calledModifiers.Add(str_temp2);

            //return parameters
            bool isReturn = false;
            foreach(var eachtAction in task.actions)
            {
                if (eachtAction.outputVariables.Count > 0)
                {
                    if(!isReturn)
                        function_text += "        returns (";

                    function_text += translateInOutputPara(eachtAction.outputVariables, func.returnVaris, false);

                    function_text += ")\n";
                }
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
            //TODO:!!!Each action one called function
            foreach(var t_action in task.actions)
            {
                function_text += "        ";
                var strCalledFunc = t_action.actionID + "(";
                for(int i = 0; i< t_action.inputVariables.Count; i++)
                {
                    if(t_action.inputVariables[i].refVari!="" 
                        && t_action.inputVariables[i].refVari!=null)
                    {
                        strCalledFunc += t_action.inputVariables[i].refVari;
                    }
                    else
                    {
                        strCalledFunc += t_action.inputVariables[i].name;
                    }
                    if(i< t_action.inputVariables.Count-1)
                    {
                        strCalledFunc += ", ";
                    }
                }
                strCalledFunc += ")";
                func.calledFunctions.Add(strCalledFunc);
                function_text += strCalledFunc + ";\n";
                
            }

            function_text += "        ";
            var strCallFuncTemp = "deleteFlow(ProcessFlow.To" + task.taskID + ")";
            func.calledFunctions.Add(strCallFuncTemp);
            function_text += strCallFuncTemp + ";\n";

            //Take state variables to output variables 
            foreach(var eachAction in task.actions)
            {
                foreach (var outputVariFromState in eachAction.outputVariables)
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
            }
            

            
            //TODO:!!!!deal with process flow
            /*if(task.flow.nextTaskIDs.Count >= 1)
            {
                if(task.flow.nextTaskIDs.Count == 1)
                {
                    if(task.flow.nextTaskIDs[0]!= "OutputCondition")
                    {
                        function_text += "        currentProcessFlows.push(ProcessFlow.To" 
                            + task.flow.nextTaskIDs[0] + ");\n";
                    }
                    
                }
                else if (task.flow.splitOperation == "and")
                {
                    foreach(var nextproc in task.flow.nextTaskIDs)
                    {
                        if (nextproc != "OutputCondition")
                        {
                            function_text += "        currentProcessFlows.push(ProcessFlow.To"
                                + nextproc + ");\n";
                        }
                        
                    }
                }
                /*else if(task.flow.splitOperation == "xor")
                {
                    var strElseText = "        else\n        {\n";
                    bool isFirstIf = true;
                    foreach (var nextproc in task.flow.nextTaskIDs)
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
            }*/
            
            function_text += "    }\n\n";
            return function_text;
        }

        string getMultiParticipantsModifierName(List<Participant> participants, SmartContractComponents sccs)
        {
            string nameResult = "Error";
            foreach(var multiParticipants in sccs.allMultiModifiers)
            {
                if(participants == multiParticipants.participants)
                {
                    nameResult = multiParticipants.modifierName;
                }
                else if(participants.Count == multiParticipants.participants.Count)
                {
                    bool isMatch = true;
                    foreach(var tempParticipant in participants)
                    {
                        if(!multiParticipants.participants.Contains(tempParticipant))
                        {
                            isMatch = false;
                        }
                    }
                    if(isMatch)
                    {
                        nameResult = multiParticipants.modifierName;
                    }
                }
            }
            return nameResult;
        }


        protected void generateSCGActionContract(List<SCGAction> allActions, string contractName, SmartContractComponents sccs, SCGChecker checker)
        {
            SmartContract file = new SmartContract();
            file.contractName = checker.checkNameValid(contractName);

            file.fileAllText += "contract " + file.contractName + "{\n";

            //Automated generated data struct for managing the data in the basic level of action
            SCGStruct scgDataStruct = new SCGStruct();
            scgDataStruct.structName = "SCGData";
            Parameter pName = new Parameter();
            pName.name = "name";
            pName.type = "string";
            scgDataStruct.parameters.Add(pName);
            Parameter pValue = new Parameter();
            pValue.name = "value";
            pValue.type = "string";
            scgDataStruct.parameters.Add(pValue);
            file.fileAllText += "\n   //Data structure for basic level of actions" +
                                "\n    struct "+ scgDataStruct.structName + " { " +
                                "\n       "+ pName.type + " "+ pName.name + "; " +
                                "\n       " + pValue.type + " " + pValue.name + "; " +
                                "\n   }";
            file.structs.Add(scgDataStruct);

            //Generate a state variable for storing all the data
            //The way of adding a SCGData: SCGData example = SCGData("name1", "value1");
            SCGVariable variSCGData = new SCGVariable();
            variSCGData.name = "allSCGData";
            variSCGData.type = scgDataStruct.structName + "[]";
            file.fileAllText += "\n   //All data for basic level of actions" +
                                "\n   SCGData[] allSCGData;\n";
            file.stateVariables.Add(variSCGData);

            // Action functions
            foreach(var eachAction in allActions)
            {
                if(!file.functions.Exists(x=>x.name== eachAction.actionID))
                {
                    Function funActionTemp = new Function();

                    file.fileAllText += addActionFunction(eachAction, funActionTemp);

                    file.functions.Add(funActionTemp);
                }
                else
                {
                    checker.setErrorMessages("[Warning] action " + eachAction.actionID + " exists.\n");
                }
                
            }

            file.fileAllText += "}\n";


            sccs.allSmartContracts.Add(file);
        }

        string addActionFunction(SCGAction eachAction, Function actionFunc)
        {
            string text = "";
            actionFunc.name = eachAction.actionID;
            text += "    function " + actionFunc.name + "(";

            //Input variables
            if (actionFunc.name == "add")
            {
                Parameter inputPara = new Parameter();
                inputPara.name = "addData";
                inputPara.type = "SCGData[] memory";
                text += inputPara.type + " " + inputPara.name;
                actionFunc.inputParams.Add(inputPara);
            }
            else if(actionFunc.name == "pay")
            {
                var type = "string memory";
                Parameter inputPara1 = new Parameter();
                inputPara1.name = "paymentAmount";
                inputPara1.type = type;
                text += inputPara1.type + " " + inputPara1.name;
                actionFunc.inputParams.Add(inputPara1);
                text += ", ";
                Parameter inputPara2 = new Parameter();
                inputPara2.name = "paymentMethod";
                inputPara2.type = type;
                text += inputPara2.type + " " + inputPara2.name;
                actionFunc.inputParams.Add(inputPara2);
                text += ", ";
                Parameter inputPara3 = new Parameter();
                inputPara3.name = "paymentReceiver";
                inputPara3.type = "address";
                text += inputPara3.type + " " + inputPara3.name;
                actionFunc.inputParams.Add(inputPara3);
            }
            else if (eachAction.inputVariables.Count > 0)
            {
                text += translateInOutputPara(eachAction.inputVariables, actionFunc.inputParams, false);
            }


            text += ")\n        internal\n    ";

            if (eachAction.outputVariables.Count > 0)
            {
                text += "    returns (";
                text += translateInOutputPara(eachAction.outputVariables, actionFunc.returnVaris, false);
                text += ")\n";
            }




            text += "{\n        ";



            //TODO:!!!Think about what statements to be added here!
            //TODO: !! check about this pay function
            /*if (task.operateParticipants.Count == 1)
            {
                function_text += "        " + task.operateParticipants[0].name + "Payable.transfer("
                    + task.payTypeVariable.name + ");\n";
            }
            else if (task.operateParticipants.Count > 1)
            {
                //TODO: more than one participants case
            }*/
            text += "\n    }\n\n";
            return text;
        }

        /*string translateInOutputParaMain(List<SCGVariable> targetVariables, List<Parameter> targetParas, bool isMain)
        {
            int countInOutputVaris = 0;
            string function_text = "";
            //put each vari to para
            foreach (var eachVari in targetVariables)
            {
                // isMain & isInput: the variable with variRef no need to print
                // not isMain (isAction): the variRef should be print
                if (!targetParas.Exists(x => x.name == eachVari.name)
                    && !(isInput && allVariables.Exists(x => x.name == eachVari.name)))
                {
                    Parameter para_temp = new Parameter();
                    para_temp.name = eachVari.name;
                    if (countInOutputVaris > 0)
                    {
                        function_text += ", ";
                    }
                    if (eachVari.type == "string" || eachVari.type == "email" || eachVari.type == "")// || inputVari.type == null)
                    {
                        function_text += "string memory " + eachVari.name;
                        para_temp.type = "string memory";
                    }
                    else
                    {
                        function_text += eachVari.type + " " + eachVari.name;
                        para_temp.type = eachVari.type;
                    }
                    countInOutputVaris++;
                    targetParas.Add(para_temp);

                }
            }
            return function_text;
        }*/

        //countMain = -1 means not main (i.e., is action)
        string translateInOutputPara(List<SCGVariable> targetVariables, List<Parameter> targetParas, bool isInputMain)
        {
            string function_text = "";
            //put each vari to para
            for (int i= 0; i < targetVariables.Count; i++)
            {
                if (!targetParas.Exists(x => x.name == targetVariables[i].name) 
                    && !( isInputMain && targetVariables[i].refVari!=null 
                    && targetVariables[i].refVari!=""))
                {
                    Parameter para_temp = new Parameter();
                    if(targetVariables[i].refVari != null && targetVariables[i].refVari != "")
                    {
                        para_temp.name = targetVariables[i].refVari;
                    }
                    else
                    {
                        para_temp.name = targetVariables[i].name;
                    }
                    
                    if (i < targetVariables.Count-1)
                    {
                        function_text += ", ";
                    }
                    if (targetVariables[i].type == "string" || targetVariables[i].type == "email" 
                        || targetVariables[i].type == "")// || inputVari.type == null)
                    {
                        function_text += "string memory " + targetVariables[i].name;
                        para_temp.type = "string memory";
                    }
                    else
                    {
                        function_text += targetVariables[i].type + " " + targetVariables[i].name;
                        para_temp.type = targetVariables[i].type;
                    }
                    targetParas.Add(para_temp);

                }
            }
            return function_text;
        }

    }
}
