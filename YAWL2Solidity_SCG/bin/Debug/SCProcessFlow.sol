pragma solidity >=0.4.22 <0.9.0;

contract SCProcessFlow{

//Automated generated process state based on process flows
    enum ProcessFlow {  }

    ProcessFlow[] processFlows;

    modifier inProcessFlow(ProcessFlow _processFlow){
        for(uint i=0; i<processFlows.length; i++)
        {
           if(processFlows[i] == _processFlow)
           {
             _;
             return;
           }
        }
        revert("Invalid state of the process flow. Please check by getCurrentProcessState().");
    }
    function getCurrentProcessState()
        public
        returns(ProcessFlow[] memory)
    {
        return processFlows;
    }
    constructor()
    {
        processFlows.push();
    }
