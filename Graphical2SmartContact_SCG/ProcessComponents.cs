using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphical2SmartContact_SCG
{
    public class ProcessComponents
    {
        public List<SCGVariable> allLocalVariables = new List<SCGVariable>();
        public List<DefineEnum> allDefinedEnums = new List<DefineEnum>();
        //public List<Modifier> allModifiers = new List<Modifier>();
        public List<SCGTask> allTasks = new List<SCGTask>();
        public List<Flow> allFlows = new List<Flow>();
        public List<SCGGateway> allGateways = new List<SCGGateway>();
        public List<Participant> allParticipants = new List<Participant>();
        public List<MultiParticipants> allMultiParticipants = new List<MultiParticipants>();
        public List<SCGAction> allActions = new List<SCGAction>();
        public string fileName = "default";
       // public bool isBPMN = false;

        public class SCGVariable
        {
            public string name;
            public string type;
            public string value;
            public string refVari;
        };
        public class Participant
        {
            public string name;
            public string id;
            public List<SCGVariable> allInfo = new List<SCGVariable>();
            public string bpmnProcessName;
            public List<string> TaskIDs = new List<string>();
            //actionTypes: to store the types (check, change, pay) of the actions for the participant
            public List<string> actionTypes = new List<string>();
        }

        //multi-participants class is used to handle the functions that multiple participants can operate.
        public class MultiParticipants
        {
            public List<Participant> participants = new List<Participant>();
        }
        public class DefineEnum
        {
            public string enumName;
            public List<string> enumValues = new List<string>();
        };
        /*public class Modifier
        {
            public string name;
            public string condition;
            public string errorString;
            public List<Variable> inputVaris = new List<Variable>();
        };*/
        public class SCGTask
        {
            public string taskID;
            public string taskName; //actual 
            //public List<SCGVariable> inputVariables = new List<SCGVariable>();
            //public List<SCGVariable> outputVariables = new List<SCGVariable>();
            //public List<SCGVariable> inOutVariables = new List<SCGVariable>();
            //public List<Modifier> modifiers = new List<Modifier>();
            public Flow flow = new Flow();
            public List<SCGAction> actions= new List<SCGAction>();
            //public string actionType; //indicate function type. Could be check, change or pay.
            //public SCGVariable payTypeVariable; // indicate the variable of the payment. The type of this variable must be uint. 
            public List<Participant> operateParticipants = new List<Participant>(); // in bpmn, only one participant
        };
        public enum FlowType {MessageFlow=0, SequenceFlow};
        public enum GatewayType { AND, OR, XOR };
        /*public class MessageFlow //When encounter a gateway, the flow connected to the gateway is ignored, but the operation of the gateway will be record in this flow
        {
            //Only sequence flow needs to consider target could be gateway.
            public string flowID; //the flow after the task
            public SCGTask currentTask;
            public SCGTask nextTask;//Could be a task or a participant, only consider task here
        };*/

        public class Flow //When encounter a gateway, the flow connected to the gateway is ignored, but the operation of the gateway will be record in this flow
        {
            //For message flow: no processID, one next task, and no sliptOperation
            //Only sequence flow needs to consider target could be gateway.
            public string flowID; //the flow after the task
            public FlowType flowType;
            public string ProcessID;//only for sequence flow
            public string flowName;
            public string sourceRef;
            public string TargetRef;
            //public GatewayType splitOperation; //XOR, OR, AND
            //public List<ToNextTask> toNextTasks = new List<ToNextTask>();
            public SCGGateway gateway = new SCGGateway();
            //public List<Participant> currentProcessParticipants = new List<Participant>(); // in bpmn, only one participant
        };

        /*public class ToNextTask
        {
            public string taskID;
            public string condition; //OR slection condition: Yes or No
        };*/
        public class SCGGateway
        {
            public string gatewayID;
            public string gatewayName;
            public string processID;
            public GatewayType splitOperation; //XOR, OR, AND
            public List<Flow> incomingFlows = new List<Flow>();
            public List<Flow> outgoingFlows = new List<Flow>();
        };

        public class SCGAction
        {
            public string actionID;
            //public List<SCGVariable> addVariables = new List<SCGVariable>();
            public List<SCGVariable> inputVariables = new List<SCGVariable>();
            public List<SCGVariable> outputVariables = new List<SCGVariable>();
            public SCGVariable payAmountVari = new SCGVariable();
            public SCGVariable payPMvari = new SCGVariable();
            public SCGVariable payToVari = new SCGVariable();
        };

        /*public class SCGRefVariable
        {
            public string name;
            public SCGVariable refVari = new SCGVariable();
        }*/
    }
}
