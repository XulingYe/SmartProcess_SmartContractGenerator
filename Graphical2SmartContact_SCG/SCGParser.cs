using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static Graphical2SmartContact_SCG.ProcessComponents;
using static Graphical2SmartContact_SCG.SCGChecker;

namespace Graphical2SmartContact_SCG
{
    public class SCGParser
    {
        //SCGChecker checker = new SCGChecker();
        public void parseGraphical(string text, string fileName, ProcessComponents pcs, SCGChecker checker)
        {
            //clear all previous data
            pcs.allLocalVariables.Clear();
            pcs.allDefinedEnums.Clear();
            pcs.allFlows.Clear();
            pcs.allTasks.Clear();
            //allModifiers.Clear();
            pcs.allParticipants.Clear();
            pcs.allGateways.Clear();

            pcs.fileName = fileName;
            parseBPMN(text, pcs, checker);
        }

        #region BPMN
        void parseBPMN(string text, ProcessComponents pcs, SCGChecker scg_checker)
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
                            Participant participantTemp = new Participant();
                            participantTemp.id = e_bpmnParticipant.GetAttribute("id");
                            participantTemp.name = e_bpmnParticipant.GetAttribute("name");
                            participantTemp.bpmnProcessName = e_bpmnParticipant.GetAttribute("processRef");
                            pcs.allParticipants.Add(participantTemp);
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
 
                            Flow flowTemp = new Flow();
                            flowTemp.flowID = e_bpmnMessageFlow.GetAttribute("id");
                            flowTemp.sourceRef = e_bpmnMessageFlow.GetAttribute("sourceRef");
                            flowTemp.TargetRef = e_bpmnMessageFlow.GetAttribute("targetRef");
                            flowTemp.flowType = FlowType.MessageFlow;
                            pcs.allFlows.Add(flowTemp);

                            //the two connected ends of a message flow would be either a task, or a participant.
                            //In current case, we will only consider the message flow which connect two tasks.
                            //if (!pcs.allParticipants.Exists(x => x.id == flowSourceID) && !pcs.allParticipants.Exists(x => x.id == flowTargetID))
                            //{
                                //Store info of each message flow into allFlows
                                
                                //create source end task
                                /*var foundSourceTask = pcs.allTasks.Find(x => x.taskID == flowSourceID);
                                if (foundSourceTask == null)
                                {
                                    foundSourceTask = new SCGTask();
                                    foundSourceTask.taskID = flowSourceID;
                                }
                                if (foundSourceTask.flow == null)
                                {
                                    foundSourceTask.flow = flowTemp;
                                }
                                pcs.allTasks.Add(foundSourceTask);*/

                                //create target end task
                               /* var foundTargetTask = pcs.allTasks.Find(x => x.taskID == flowTargetID);
                                if (foundTargetTask == null)
                                {
                                    foundTargetTask = new SCGTask();
                                    foundTargetTask.taskID = flowTargetID;
                                }
                                if (foundTargetTask.flow == null)
                                {
                                    foundTargetTask.flow = flowTemp;
                                }
                                pcs.allTasks.Add(foundTargetTask);*/
                            //}

                        }
                    }
                }
            }

            //Process of each participant
            XmlNodeList bpmnProcesses_nodeList = doc.GetElementsByTagName("bpmn:process");
            foreach (XmlNode bpmnProcess_node in bpmnProcesses_nodeList)
            {
                if (bpmnProcess_node != null && bpmnProcess_node.GetType().Name == "XmlElement" 
                    && bpmnProcess_node.ChildNodes!=null && bpmnProcess_node.ChildNodes.Count>0)
                {
                    XmlElement e_bpmnProcess = (XmlElement)bpmnProcess_node;
                    var processID = e_bpmnProcess.GetAttribute("id");
                    var foundParticipant = pcs.allParticipants.Find(x => x.bpmnProcessName == processID);
                    if (foundParticipant != null)
                    {
                        foreach(XmlNode nodeProcessChild in bpmnProcess_node.ChildNodes)
                        {
                            if(nodeProcessChild!=null && nodeProcessChild.GetType().Name == "XmlElement")
                            {
                                //step 1: add address and email
                                if (nodeProcessChild.Name == "bpmn:extensionElements" && nodeProcessChild.ChildNodes != null
                                    & nodeProcessChild.ChildNodes.Count > 0)
                                {
                                    addParticipantInfo(nodeProcessChild, foundParticipant, pcs.allLocalVariables, scg_checker);
                                }
                                //step 2: add each task into allTasks and foundParticipant
                                else if (nodeProcessChild.Name.Contains("Task")|| nodeProcessChild.Name == "bpmn:task")
                                {
                                    XmlElement e_bpmnTask = (XmlElement)nodeProcessChild;
                                    SCGTask taskTemp = new SCGTask();
                                    taskTemp.operateParticipants.Add(foundParticipant);
                                    taskTemp.taskID = e_bpmnTask.GetAttribute("id");
                                    taskTemp.taskName = e_bpmnTask.GetAttribute("name");
                                    foreach(XmlNode taskNode in e_bpmnTask.ChildNodes)
                                    {
                                        if(taskNode != null && taskNode.GetType().Name == "XmlElement")
                                        {
                                            XmlElement taskElement = (XmlElement)taskNode;
                                            //Actions in extensions
                                            if(taskElement.Name == "bpmn:extensionElements")
                                            {
                                                XmlNode actions_node = taskElement.GetElementsByTagName("scg:actions").Item(0);
                                                foreach(XmlNode action_node in actions_node.ChildNodes)
                                                {
                                                    if(action_node!=null && action_node.GetType().Name == "XmlElement")
                                                    {
                                                        XmlElement actionElement = (XmlElement)action_node;
                                                        var actionTemp = parseEachActionInfo(actionElement, taskTemp, pcs.allLocalVariables, scg_checker);
                                                        //taskTemp.actions.Add(actionTemp);
                                                        pcs.allActions.Add(actionTemp);
                                                    }
                                                }
                                            }
                                            //incoming and outgoing
                                            else if(taskElement.Name == "bpmn:incoming")
                                            {

                                            }
                                            else if(taskElement.Name == "bpmn:outgoing")
                                            {

                                            }
                                        }
                                        

                                    }
                                    
                                    

                                    
                                        
                                    

                                    pcs.allTasks.Add(taskTemp);
                                }
                                else if (nodeProcessChild.Name == "bpmn:startEvent")
                                {

                                }
                                else if (nodeProcessChild.Name == "bpmn:endEvent")
                                {

                                }
                                else if (nodeProcessChild.Name == "bpmn:sequenceFlow")
                                {
                                    Flow flowTemp = new Flow();
                                    XmlElement e_bpmnSequenceFlow = (XmlElement)nodeProcessChild;
                                    
                                    flowTemp.flowID = e_bpmnSequenceFlow.GetAttribute("id");
                                    flowTemp.sourceRef = e_bpmnSequenceFlow.GetAttribute("sourceRef");
                                    flowTemp.TargetRef = e_bpmnSequenceFlow.GetAttribute("targetRef");
                                    flowTemp.ProcessID = processID;
                                    flowTemp.flowType = FlowType.SequenceFlow;
                                    pcs.allFlows.Add(flowTemp);
                                }
                                else if (nodeProcessChild.Name.Contains("Gateway"))
                                {
                                    SCGGateway gatewayTemp = new SCGGateway();
                                    XmlElement e_bpmnGateway = (XmlElement)nodeProcessChild;
                                    gatewayTemp.gatewayID = e_bpmnGateway.GetAttribute("id");
                                    gatewayTemp.gatewayName = e_bpmnGateway.GetAttribute("name");
                                    foreach(XmlNode gwFlowNode in nodeProcessChild.ChildNodes)
                                    {
                                        Flow flowTemp = new Flow();
                                        flowTemp.flowID = gwFlowNode.InnerText;

                                        if(gwFlowNode.Name == "bpmn:incoming")
                                        {
                                            gatewayTemp.incomingFlows.Add(flowTemp);
                                        }
                                        else if (gwFlowNode.Name == "bpmn:outgoing")
                                        {
                                            gatewayTemp.outgoingFlows.Add(flowTemp);
                                        }
                                    }
                                    // OR event
                                    if (nodeProcessChild.Name == "bpmn:exclusiveGateway")
                                    {
                                        gatewayTemp.splitOperation = GatewayType.OR;
                                    }
                                    // AND event
                                    else if (nodeProcessChild.Name == "bpmn:parallelGateway")
                                    {
                                        gatewayTemp.splitOperation = GatewayType.AND;
                                    }
                                    // XOR event
                                    else if (nodeProcessChild.Name == "bpmn:inclusiveGateway")
                                    {
                                        gatewayTemp.splitOperation = GatewayType.XOR;
                                    }
                                    pcs.allGateways.Add(gatewayTemp);
                                }
                            }
                        }
                    }
                    else
                    {
                        scg_checker.setErrorMessages("Error in parsing: Participant is not found for " + processID + ".");
                    }
                }
            }
        }

        private void addParticipantInfo(XmlNode extensionElements, Participant currentParticipant, List<SCGVariable> allVariables, SCGChecker scgChecker)
        {
            foreach(var elementChildNode in extensionElements.ChildNodes)
            {
                if (elementChildNode != null && elementChildNode.GetType().Name == "XmlElement")
                {
                    XmlElement e_elementChild = (XmlElement)elementChildNode;

                    //properties, name should be address
                    if(e_elementChild.Name == "scg:variable")
                    {
                        SCGVariable variTemp = obtainVariable(e_elementChild, allVariables, scgChecker);
                        currentParticipant.allInfo.Add(variTemp);
                        if(allVariables.Exists(x=>x.name==variTemp.name))
                        {
                            scgChecker.setErrorMessages("[Warning] Smae variable " + variTemp.name + " has been added twice.\n");
                        }
                        else
                        {
                            allVariables.Add(variTemp);
                        }
                        
                       
                             
                    }
                    //should not be the case
                    else
                    {
                        scgChecker.setErrorMessages("Warning in parsing Participant: invalid information for particiapnt " + currentParticipant.id);
                    }

                }
            }
            
        }

        //case 2: Action and in-output variables
        private SCGAction parseEachActionInfo(XmlElement e_bpmnAction, SCGTask task, List<SCGVariable> allVariables, SCGChecker scgChecker)
        {
            //List<SCGAction> actionsTemp = new List<SCGAction>();
            
            
            var actionTemp = new SCGAction();
            actionTemp.actionID = e_bpmnAction.GetAttribute("id");
            //This is the input variables of action
            foreach(var nodeAction in e_bpmnAction.ChildNodes)
            {
                if (nodeAction != null && nodeAction.GetType().Name == "XmlElement")
                {
                    XmlElement e_action = (XmlElement)nodeAction;

                    /*if (e_action.Name == "scg:variable")
                    {
                        var addVari = obtainVariable(e_action, "string");
                        actionTemp.addVariables.Add(addVari);
                        //if(!task.inputVariables.Contains(addVari))
                         //   task.inputVariables.Add(addVari);
                    }
                    else*/ if (e_action.Name == "scg:variableInput")
                    {
                        //When a variable has a refVari, it can directly obtained from local variable, no need to be as input vari.
                        var inputVari = obtainVariable(e_action, allVariables, scgChecker);
                        actionTemp.inputVariables.Add(inputVari);
                        
                    }
                    else if (e_action.Name == "scg:variableOutput")
                    {
                        var outputVari = obtainVariable(e_action, allVariables,scgChecker);
                        actionTemp.outputVariables.Add(outputVari);
                    }
                    else
                    {
                        scgChecker.setErrorMessages("Warning in parsing action: invalid information in action " + actionTemp.actionID);
                    }
                }  
            }
            task.actions.Add(actionTemp);
            return actionTemp;
        }
        
        private SCGVariable obtainVariable(XmlElement eVari, List<SCGVariable> allVariables, SCGChecker scgChecker)
        {
            var variTemp = new SCGVariable();
            variTemp.name= eVari.GetAttribute("name");
            variTemp.value = eVari.GetAttribute("value");
            variTemp.type = eVari.GetAttribute("type");
            var variRefName = eVari.GetAttribute("equalVari");
            if (variRefName != null && variRefName != "")
            {
                var foundVari = allVariables.Find(x => x.name == variRefName);
                if (foundVari!=null)
                {
                    variTemp.refVari = variRefName;
                    variTemp.type = foundVari.type;
                }
                else
                { 
                    scgChecker.setErrorMessages("Error in parsing action: " + variRefName + " not found.\n");
                }
            }
            else
            {
                scgChecker.setErrorMessages("[Info]: Action variable " + variTemp.name + " has no reference.The input of this variable will need to be handled by users.\n");
            }
            if(variTemp.type==null|| variTemp.type==""||variTemp.type=="email")
            {
                variTemp.type = "string";
            }

            return variTemp;
        }

        public void handleGateways(ProcessComponents pcs)
        {
            foreach(var eachGateway in pcs.allGateways)
            {
                //Store all the gateways in their incoming flows of allFlows
                foreach(var eachInFlow in eachGateway.incomingFlows)
                {
                    var foundFlow = pcs.allFlows.Find(x => x.flowID == eachInFlow.flowID);
                    foundFlow.gateway = eachGateway;
                    foundFlow.TargetRef = null;
                }
                // Store all the outgoing flows in gateways and delete them from allFlows
                for(int i=0; i< eachGateway.outgoingFlows.Count; i++)
                {
                    var foundFlow = pcs.allFlows.Find(x => x.flowID == eachGateway.outgoingFlows[i].flowID);
                    eachGateway.outgoingFlows[i] = foundFlow;
                    pcs.allFlows.Remove(foundFlow);
                }
                

            }
        }
       
        #endregion
    }
}

