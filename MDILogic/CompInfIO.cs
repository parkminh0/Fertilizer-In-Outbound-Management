using DevExpress.Drawing.Internal.Fonts.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanIlCNS.MDILogic
{
    internal class CompInfIO
    {
		private int _CompKey;

		public int CompKey
		{
			get { return _CompKey; }
			set { _CompKey = value; }
		}

        private string _CompCode;

        public string CompCode
        {
            get { return _CompCode; }
            set { _CompCode = value; }
        }

        private string _CompName;

        public string CompName
        {
            get { return _CompName; }
            set { _CompName = value; }
        }

        private string _CompOwner;

        public string CompOwner
        {
            get { return _CompOwner; }
            set { _CompOwner = value; }
        }

        private string _CompRegistNum;

        public string CompRegistNum
        {
            get { return _CompRegistNum; }
            set { _CompRegistNum = value; }
        }

        private string _CompCategory;

        public string CompCategory
        {
            get { return _CompCategory; }
            set { _CompCategory = value; }
        }

        private string _CompCondition;

        public string CompCondition
        {
            get { return _CompCondition; }
            set { _CompCondition = value; }
        }

        private string _CompTel;

        public string CompTel
        {
            get { return _CompTel; }
            set { _CompTel = value; }
        }

        private string _CompFax;

        public string CompFax
        {
            get { return _CompFax; }
            set { _CompFax = value; }
        }

        private string _CompPost;

        public string CompPost
        {
            get { return _CompPost; }
            set { _CompPost = value; }
        }

        private string _CompAddress;

        public string CompAddress
        {
            get { return _CompAddress; }
            set { _CompAddress = value; }
        }

        private string _CompNote;

        public string CompNote
        {
            get { return _CompNote; }
            set { _CompNote = value; }
        }

        private DateTime _CreateDtm;

        public DateTime CreateDtm
        {
            get { return _CreateDtm; }
            set { _CreateDtm = value; }
        }

        private DateTime _UpdateDtm;

        public DateTime UpdateDtm
        {
            get { return _UpdateDtm; }
            set { _UpdateDtm = value; }
        }

        private bool _isDeleted;

        public bool isDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        /// <summary>
        /// 기본생성자
        /// </summary>
        public CompInfIO()
        {

        }

        /// <summary>
        /// 데이터 생성자
        /// </summary>
        /// <param name="pCompKey"></param>
        public CompInfIO(int pCompKey)
        {
            GetData(pCompKey);
        }

        /// <summary>
        /// 데이터 가져오기
        /// </summary>
        /// <param name="pCompKey"></param>
        private void GetData(int pCompKey)
        {
            string sql = string.Empty;
            sql += "select * from compinf ";
            sql += " where 1 = 1 ";
            sql += $"  and compkey = {pCompKey} ";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            if (dt == null || dt.Rows.Count == 0)
                return;

            int.TryParse(dt.Rows[0]["compkey"].ToString(), out _CompKey);
            CompCode = dt.Rows[0]["compcode"].ToString();
            CompName = dt.Rows[0]["compname"].ToString();
            CompOwner = dt.Rows[0]["compowner"].ToString();
            CompRegistNum = dt.Rows[0]["compregistnum"].ToString();
            CompCategory = dt.Rows[0]["compcategory"].ToString();
            CompCondition = dt.Rows[0]["compcondition"].ToString();
            CompTel = dt.Rows[0]["comptel"].ToString();
            CompFax = dt.Rows[0]["compfax"].ToString();
            CompPost = dt.Rows[0]["comppost"].ToString();
            CompAddress = dt.Rows[0]["compaddress"].ToString();
            CompNote = dt.Rows[0]["compnote"].ToString();
            DateTime.TryParse(dt.Rows[0]["updatedtm"].ToString(), out _UpdateDtm);
            DateTime.TryParse(dt.Rows[0]["createdtm"].ToString(), out _CreateDtm);
            bool.TryParse(dt.Rows[0]["isdeleted"].ToString(), out _isDeleted);
        }

        /// <summary>
        /// CompInfIO Select
        /// </summary>
        /// <returns></returns>
        public static DataTable Select(string CompName, string Address)
        {
            string sql = string.Empty;
            sql += "select * from compinf ";
            sql += " where 1 = 1 ";
            sql += "   and isdeleted = '0' ";
            if (!string.IsNullOrEmpty(CompName) && CompName != "전체")
            {
                sql += $" and compName like '%{CompName}%' ";
            }
            if (!string.IsNullOrEmpty(Address) && Address != "전체")
            {
                sql += $" and Address like '%{Address}%' ";
            }
            sql += " order by compkey desc ";

            return DBManager.Instance.GetDataTable(sql);
        }

        /// <summary>
        /// CompInf Insert
        /// </summary>
        /// <returns></returns>
        public int Insert()
        {
            int compKey = LogicManager.Common.fnGetNextKey("lastcompkey");

            string sql = string.Empty;
            sql += "insert into compinf ";
            sql += "values ( ";
            sql += $" {compKey}, ";
            sql += $" N'{CompCode}', ";
            sql += $" N'{CompName}', ";
            sql += $" N'{CompOwner}', ";
            sql += $" N'{CompRegistNum}', ";
            sql += $" N'{CompCategory}', ";
            sql += $" N'{CompCondition}', ";
            sql += $" N'{CompTel}', ";
            sql += $" N'{CompFax}', ";
            sql += $" N'{CompPost}', ";
            sql += $" N'{CompAddress}', ";
            sql += $" N'{CompNote}', ";
            sql += " current_timestamp, ";
            sql += " current_timestamp, ";
            sql += " '0' ";
            sql += ") ";

            return DBManager.Instance.ExcuteDataUpdate(sql);
        }

        public int Update()
        {
            string sql = string.Empty;
            sql += "update compinf ";
            sql += "   set ";
            sql += $"      compcode = N'{CompCode}', ";
            sql += $"      compname = N'{CompName}', ";
            sql += $"      compowner = N'{CompOwner}', ";
            sql += $"      compregistnum = N'{CompRegistNum}', ";
            sql += $"      compcategory = N'{CompCategory}', ";
            sql += $"      compcondition = N'{CompCondition}', ";
            sql += $"      comptel = N'{CompTel}', ";
            sql += $"      compfax = N'{CompFax}', ";
            sql += $"      comppost = N'{CompPost}', ";
            sql += $"      compaddress = N'{CompAddress}', ";
            sql += $"      compnote = N'{CompNote}', ";
            sql += $"      updatedtm = current_timestamp ";
            sql += " where 1 = 1 ";
            sql += $"  and compkey = {CompKey} ";
            return DBManager.Instance.ExcuteDataUpdate(sql);
        }

        /// <summary>
        /// CompInf Delete
        /// </summary>
        /// <returns></returns>
        public static int Delete(int ChoiceCompKey)
        {
            string sql = string.Empty;
            sql += "update compinf ";
            sql += "   set ";
            sql += "       isdeleted = '1', ";
            sql += "       updatedtm = current_timestamp ";
            sql += " where 1 = 1 ";
            sql += $"  and compkey = {ChoiceCompKey} ";
            return DBManager.Instance.ExcuteDataUpdate(sql);
        }
    }
}