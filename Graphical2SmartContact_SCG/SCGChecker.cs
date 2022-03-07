using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Graphical2SmartContact_SCG.SCGTranslator;
using static Graphical2SmartContact_SCG.SmartContractComponents;

namespace Graphical2SmartContact_SCG
{
    public class SCGChecker
    {
        public string errorMessages = "";

        void parseSC(List<SolidityFile> SCFiles)
        {

        }

        public string checkResults()
        {
            if (errorMessages == "")
            {
                errorMessages += "No error is found.\n";
            }
            return errorMessages;
        }

        //return a valid name
        public string checkNameValid(string name)//, out string warningMessage)
        {
            Regex startLetter = new Regex("[a-zA-Z$_]");
            Regex restLetter = new Regex("[^a-zA-Z0-9$_]");
            //warningMessage = "";

            if (string.IsNullOrEmpty(name))
            {
                name = "random_"+Guid.NewGuid().ToString().GetHashCode().ToString("x");
                this.errorMessages += "Invalid name: Name is null or empty! A random id (" 
                    + name + ") is generated for this name.\n";
                
            }
            if(name.Contains(" "))
            {
                errorMessages
                    += "Invalid name " + name + ": It conatins empty space, and the empty space has been deleted from this name.";
                name = name.Replace(" ", ""); 
                errorMessages += "Current name is "+ name +"\n";
            }
            if (name.Contains("-"))
            {
                errorMessages
                    += "Invalid name " + name + ": It conatins special characters, and it is replaced by _.";
                name = name.Replace("-", "_");
                errorMessages += "Current name is " + name + "\n";
            }
            if (!startLetter.IsMatch(name.First().ToString()))
            {
                errorMessages
                    += "Invalid name " + name + ": The first letter of the name is invalid! An \"a\" is inserted as the first letter.\n";
                name = name.Insert(0, "a");
            }
            /*if(!restLetter.IsMatch(name))
            {
                errorMessages += "Invalid name " + name + ": There are special characters in this name.";
                name = "random_" + Guid.NewGuid().ToString().GetHashCode().ToString("x");
                errorMessages += " A random id ("+name+") is generated for this name.\n";
            }*/
            if(isSolidityKeyword(name))
            {
                errorMessages += "Invalid name" + name + ": This is a keyword of Solidity.";
                name += "_modified";
                errorMessages += " A new id (" + name + ") is generated for this name.\n";
            }
            return name;
        }

        private bool isSolidityKeyword(string strName)
        {
            string[] solidityKeywords = {"pragma", "import", "as", "from",
                "int", "string", "address",
                "public", "private", "external", "internal", "return", "payable",
                "modifier", "function", "contract"};
            if(solidityKeywords.Contains(strName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
