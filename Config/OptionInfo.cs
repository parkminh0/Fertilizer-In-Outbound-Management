using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HanIlCNS
{
    public class OptionInfo
    {
        string loginID;
        //string dbConnStr;

        public string LoginID
        {
            get { return loginID; }
            set
            {
                loginID = value;
            }
        }

        string serverIP;

        public string ServerIP
        {
            get { return serverIP; }
            set
            {
                serverIP = value;
            }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        private string _DBFullPath;
        /// <summary>
        /// Database File 위치
        /// </summary>
        public string DBFullPath
        {
            get { return _DBFullPath; }
            set { _DBFullPath = value; }
        }

        private bool _IsSaveLoginID;

        public bool IsSaveLoginID
        {
            get { return _IsSaveLoginID; }
            set { _IsSaveLoginID = value; }
        }
    }
}
