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
        public List<GraphicalTask> allTasks = new List<GraphicalTask>();
        public List<Flow> allFlows = new List<Flow>();
        public List<Role> allRoles = new List<Role>();
        public List<MultiRoles> allMultiRoles = new List<MultiRoles>();
        public string fileName = "default";
        public bool isBPMN = false;

        public class SCGVariable
        {
            public string name;
            public string type;
            public string value;
        };
        public class Role
        {
            public string name;
            public string id;
            public string address;
            public string bpmnProcessName;
            public List<string> TaskIDs = new List<string>();
            //actionTypes: to store the types (check, change, pay) of the actions for the role
            public List<string> actionTypes = new List<string>();
        }

        //multi-roles class is used to handle the functions that multiple roles can operate.
        public class MultiRoles
        {
            public List<Role> roles = new List<Role>();
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
        public class GraphicalTask
        {
            public string taskID;
            public string taskName; //actual 
            public List<SCGVariable> inputVariables = new List<SCGVariable>();
            public List<SCGVariable> outputVariables = new List<SCGVariable>();
            //public List<SCGVariable> inOutVariables = new List<SCGVariable>();
            //public List<Modifier> modifiers = new List<Modifier>();
            public Flow processFlow = new Flow();
            public string actionType; //indicate function type. Could be check, change or pay.
            public SCGVariable payTypeVariable; // indicate the variable of the payment. The type of this variable must be uint. 
            public List<Role> operateRoles = new List<Role>(); // in bpmn, only one role
        };
        public class Flow
        {
            public string flowID;
            public string currentTaskID;
            public List<ToNextTask> nextTasks = new List<ToNextTask>();
            public string splitOperation; //XOR, OR, AND //only work when it has more than one process.
            //public List<Role> currentProcessRoles = new List<Role>(); // in bpmn, only one role
        };
        public class ToNextTask
        {
            public string taskID;
            public string condition;
        };

        
    }
}
