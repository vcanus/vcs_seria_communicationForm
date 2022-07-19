using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vcs_seria_communicationForm
{
    class CommandExecute
    {
        private Dictionary<string, DelgateFunction> executeFunction = new Dictionary<string, DelgateFunction>();
        public delegate string DelgateFunction(string command);
        public CommandExecute()
        {
            executeFunction.Add("REST", CommandRestExecute);
            executeFunction.Add("RSWV", CommandRswvExecute);
            executeFunction.Add("CMOD", CommandCmodExecute);
            executeFunction.Add("RUST", CommandRustExecute);
            executeFunction.Add("RFSC", CommandRfscExecute);
            executeFunction.Add("RMCS", CommandRmcsExecute);
            executeFunction.Add("MTHV", CommandMthvExecute);
            executeFunction.Add("RTHV", CommandRthvExecute);
            executeFunction.Add("LEAK", CommandLeakExecute);
            executeFunction.Add("LSTI", CommandLstiExecute);
            executeFunction.Add("SLCP", CommandSlcpExecute);
            executeFunction.Add("RLCP", CommandRlcpExecute);
            executeFunction.Add("SN2E", CommandSn2eExecute);
            executeFunction.Add("ESLS", CommandEslsExecute);
            executeFunction.Add("SNSM", CommandSnsmExecute);
            executeFunction.Add("RPRI", CommandRpriExecute);
            executeFunction.Add("SN2S", CommandSn2sExecute);
            executeFunction.Add("SFEP", CommandSfepExecute);
            executeFunction.Add("RFEP", CommandRfepExecute);
            executeFunction.Add("IFEP", CommandIfepExecute);
            executeFunction.Add("CCAM", CommandCcamExecute);
            executeFunction.Add("SLED", CommandSledExecute);
            executeFunction.Add("SODR", CommandSodrExecute);
            executeFunction.Add("CSVD", CommandCsvdExecute);
            executeFunction.Add("RFAR", CommandRfarExecute);
            executeFunction.Add("RMAR", CommandRmarExecute);
            executeFunction.Add("STMG", CommandStmgExecute);
        }

        public string commandExecuteFunction(string command)
        {
            if(executeFunction.ContainsKey(command))
                return executeFunction[command](command);
            return "None";
        }
        public string CommandRestExecute(string command)
        {
            return "after" + command;
        }

        public string CommandRswvExecute(string command)
        {
            return "after" + command;;
        }
        public string CommandCmodExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRustExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRfscExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRmcsExecute(string command)
        {
            return "after" + command;
        }
        public string CommandMthvExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRthvExecute(string command)
        {
            return "after" + command;
        }
        public string CommandLeakExecute(string command)
        {
            return "after" + command;
        }
        public string CommandLstiExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSlcpExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRlcpExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSn2eExecute(string command)
        {
            return "after" + command;
        }
        public string CommandEslsExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSnsmExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRpriExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSn2sExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSfepExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRfepExecute(string command)
        {
            return "after" + command;
        }
        public string CommandIfepExecute(string command)
        {
            return "after" + command;
        }
        public string CommandCcamExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSledExecute(string command)
        {
            return "after" + command;
        }
        public string CommandSodrExecute(string command)
        {
            return "after" + command;
        }
        public string CommandCsvdExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRfarExecute(string command)
        {
            return "after" + command;
        }
        public string CommandRmarExecute(string command)
        {
            return "after" + command;
        }
        public string CommandStmgExecute(string command)
        {
            return "after" + command;
        }
    }
}
