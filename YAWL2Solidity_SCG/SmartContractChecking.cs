using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.SmartContractGenerator;

namespace Graphical2SmartContact_SCG
{
    class SmartContractChecking
    {


        void parseSC(List<SolidityFile> SCFiles)
        {

        }

        public string printErrorMessage()
        {
            string result = "";
            
            //TODO: some checking rules

            //in the end
            if(result=="")
            {
                result = "No error is found.";
            }
            return result;
        }
    }
}
