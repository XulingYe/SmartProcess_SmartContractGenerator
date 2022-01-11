using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.SCGTranslator;

namespace Graphical2SmartContact_SCG
{
    class SCGChecker
    {
        string errorMessages = "";

        void parseSC(List<SolidityFile> SCFiles)
        {

        }

        public string checkProcessComponents()
        {
            string result = "";

            //TODO: some checking rules

            //in the end
            if (result == "")
            {
                errorMessages += "\nProcess components are checked, and no error is found.";
            }
            return errorMessages;
        }

        public string checkSmartContractComponents()
        {
            string result = "";

            //TODO: some checking rules

            //in the end
            if (result == "")
            {
                errorMessages += "\nSmart contract components are checked, and no error is found.";
            }
            return errorMessages;
        }

        public string checkSmartContractCodes()
        {
            string result = "";

            //TODO: some checking rules

            //in the end
            if (result == "")
            {
                errorMessages += "\nSmart contract codes are checked, and no error is found.";
            }
            return errorMessages;
        }

    }
}
