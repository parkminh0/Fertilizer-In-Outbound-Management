using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanIlCNS
{
    public class UserAccount
    {
        private string _UserID;

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        private string _UserPwd;

        public string UserPwd
        {
            get { return _UserPwd; }
            set { _UserPwd = value; }
        }
        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private int _PositionKey;

        public int PositionKey
        {
            get { return _PositionKey; }
            set { _PositionKey = value; }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set { _PhoneNumber = value; }
        }
        private string _Agency;

        public string Agency
        {
            get { return _Agency; }
            set { _Agency = value; }
        }
        private string _Department;

        public string Department
        {
            get { return _Department; }
            set { _Department = value; }
        }
        private string _Gender;

        public string Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        private int _Autority;

        public int Autority
        {
            get { return _Autority; }
            set { _Autority = value; }
        }
        private bool _IsDelete;

        public bool IsDelete
        {
            get { return _IsDelete; }
            set { _IsDelete = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        public UserAccount()
        {

        }

        /// <summary>
        /// 데이터 생성자
        /// </summary>
        /// <param name="pUserID"></param>
        public UserAccount(string pUserID)
        {
            GetData(pUserID);
        }

        /// <summary>
		/// 데이터 가져오기
		/// </summary>
		/// <param name="pUserID"></param>
		private void GetData(string pUserID)
        {
            string sql = string.Empty;
            sql += "SELECT UserID, ";
            sql += "       UserPwd, ";
            sql += "       UserName, ";
            sql += "       PositionKey, ";
            sql += "       Address, ";
            sql += "       PhoneNumber, ";
            sql += "       Agency, ";
            sql += "       Department, ";
            sql += "       Gender, ";
            sql += "       Autority, ";
            sql += "       UpdateID, ";
            sql += "       UpdateDtm, ";
            sql += "       IsDelete ";
            sql += "  FROM UserAccount ";
            sql += " WHERE 1 = 1 ";
            sql += $"   AND UserID = N'{pUserID}' ";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            if (dt == null || dt.Rows.Count == 0)
                return;

            UserID = dt.Rows[0]["UserID"].ToString();
            UserPwd = dt.Rows[0]["UserPwd"].ToString();
            UserName = dt.Rows[0]["UserName"].ToString();
            int.TryParse(dt.Rows[0]["PositionKey"].ToString(), out _PositionKey);
            Address = dt.Rows[0]["Address"].ToString();
            PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString();
            Agency = dt.Rows[0]["Agency"].ToString();
            Department = dt.Rows[0]["Department"].ToString();
            Gender = dt.Rows[0]["Gender"].ToString();
            int.TryParse(dt.Rows[0]["Autority"].ToString(), out _Autority);
            bool.TryParse(dt.Rows[0]["IsDelete"].ToString(), out _IsDelete);
        }
    }
}
