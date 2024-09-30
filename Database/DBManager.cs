using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace HanIlCNS
{
    public class DBManager
    {
        public static string DBMErrString;
        public static bool IsConnected = false;
        static DBManager instance;
        public static DBManager Instance
        {
            get
            {
                if (instance == null || !IsConnected)
                {
                    // DB 연결 실패된 상태에서 DB 사용 시 연결 재시도
                    instance = new DBManager();
                }
                return instance;
            }
        }
        OptionInfo OCF
        {
            get
            {
                return Program.Option;
            }
        }

        string pswd = AES.AESDecrypt256("6snUHxhR5ee36Xa5C6NcQg==", Program.constance.compName); // passw0rd132$
        public string Pswd
        {
            get
            {
                return pswd;
            }
        }

        const int CONN_CNT = 1;
        DBMPostgreSql[] dbArr = new DBMPostgreSql[CONN_CNT];

        /// <summary>
        /// 생성자
        /// </summary>
        public DBManager()
        {
            lock (thisLock)
            {
                //string connStr = $"Server=wooribnc.iptime.org;Database=HanIlCNS;User Id=postgres;Password={pswd};Port=10011;charset=unicode"; // ClientEncoding=utf-8;";
                //개발서버
                //string connStr = $"Server=wooribnc.iptime.org;Database=HanIlCNS;User Id=postgres;Password={pswd};Port=10011;"; // ClientEncoding=utf-8;";
                //고객서버
                string connStr = $"Server={Program.constance.ServerIP};Database=HanIlCNS;User Id=postgres;Password={pswd};Port=5432;"; // ClientEncoding=utf-8;";
                for (int i = 0; i < dbArr.Length; i++)
                {
                    dbArr[i] = new DBMPostgreSql();
                    DBMErrString = dbArr[i].SetReady(connStr);
                    if (!string.IsNullOrEmpty(DBMErrString))
                    {
                        // DB 연결 실패 시 항상 메세지 출력
                        IsConnected = false;
                        DevExpress.XtraEditors.XtraMessageBox.Show("[오류] DB Connection에 문제가 있습니다.\r\n" + DBMErrString, "Error");
                    }
                    else
                    {
                        IsConnected = true;
                    }
                }
            }
        }

        object thisLock = new object();
        int currIdx = -1;
        public DBMPostgreSql MDB
        {
            get
            {
                //DBMSSQL db = null;
                DBMPostgreSql db = null;
                lock (thisLock)
                {
                    if (currIdx >= CONN_CNT - 1)
                    {
                        currIdx = -1;
                    }
                    currIdx++;
                    db = dbArr[currIdx];
                }
                return db;
            }
        }

        /// <summary>
        /// Database에서 데이터를 불러와 DataTable에 담아 넘겨줌
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public DataTable GetDataTable(string query, bool isMultiThread = false)
        {
            DataTable resultDT = null;
            try
            {
                resultDT = MDB.GetDataTable(query);
            }
            catch (ExceptionManager pException)
            {
                MDB.moleCommand.Connection.Close();
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(string.Format("Exception Method = {0}\r\n InnerException = {1} \r\n Message = {2} ", pException.Method, pException.InnerException.Message, pException.Message));
                }
                else
                {
                    throw pException;
                }
            }
            catch (Exception e)
            {
                MDB.moleCommand.Connection.Close();
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return resultDT;
        }

        /// <summary>
        /// 실행
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public int ExcuteDataUpdate(string query, bool isMultiThread = false)
        {
            int result = -1;
            try
            {
                result = MDB.mUpdate(query);
            }
            catch (ExceptionManager pException)
            {
                MDB.moleCommand.Connection.Close();
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(string.Format("Exception Method = {0}\r\n InnerException = {1} \r\n Message = {2} ", pException.Method, pException.InnerException.Message, pException.Message));
                }
                else
                {
                    throw pException;
                }
            }
            catch (Exception e)
            {
                MDB.moleCommand.Connection.Close();
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        /// <summary>
        /// Int값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public int GetIntScalar(string query, bool isMultiThread = false)
        {
            int result = 0;
            try
            {
                result = MDB.GetIntScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        /// <summary>
        /// Int값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public float GetFloatScalar(string query, bool isMultiThread = false)
        {
            float result = 0;
            try
            {
                result = MDB.getFloatScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        /// <summary>
        /// Double값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public double GetDoubleScalar(string query, bool isMultiThread = false)
        {
            double result = 0;
            try
            {
                result = MDB.getDoubleScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        /// <summary>
        /// Double값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public decimal GetDecimalScalar(string query, bool isMultiThread = false)
        {
            decimal result = 0;
            try
            {
                result = MDB.getDecimalScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return result;
        }

        /// <summary>
        /// string값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public string GetStringScalar(string query, bool isMultiThread = false)
        {
            string resultString = string.Empty;
            try
            {
                resultString = MDB.getStringScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return resultString;
        }

        /// <summary>
        /// Date값 하나 가져오기
        /// </summary>
        /// <param name="query"></param>
        /// <param name="isMultiThread">Thread 사용 시 true</param>
        /// <returns></returns>
        public DateTime GetDateTimeScalar(string query, bool isMultiThread = false)
        {
            DateTime resultDate = new DateTime();
            try
            {
                resultDate = MDB.getDateTimeScalar(query);
            }
            catch (Exception e)
            {
                if (!isMultiThread)
                {
                    Program.WMSG.MSG(e.Message);
                }
                else
                {
                    throw e;
                }
            }

            return resultDate;
        }       

        /// <summary>
        /// 트랜잭션 처리
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public string ExcuteTransaction(string[] querys)
        {
            return MDB.ExcuteTransaction(querys);
        }
        /// <summary>
        /// 트랜잭션 처리
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public string ExcuteTransaction(List<string> querys)
        {
            return MDB.ExcuteTransaction(querys.ToArray());
        }
        /// <summary>
        /// 고정 DBMPostgreSql 트랜잭션 처리
        /// </summary>
        /// <param name="querys"></param>
        /// <returns></returns>
        public string ExcuteTransaction(List<string> querys, int index)
        {
            DBMPostgreSql db = dbArr[index];
            return db.ExcuteTransaction(querys.ToArray());
        }
    }
}
