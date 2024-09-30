using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace HanIlCNS
{
    public class CommonLogic
    {
        public delegate void LoadDataMethod();
        /// <summary>
        /// 상속폼의 리스트 새로고침
        /// </summary>
        /// <param name="LoadData">Data Load & Bind Method</param>
        /// <param name="view">GridView to refresh</param>
        public void RefreshGridView(LoadDataMethod LoadMethod, DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            if (view != null)
            {
                int handle = view.FocusedRowHandle;
                int top = view.TopRowIndex;

                LoadMethod();
                
                view.TopRowIndex = top;
                if (view.RowCount <= handle)
                {
                    view.FocusedRowHandle = view.RowCount - 1;
                }
                else
                {
                    view.FocusedRowHandle = handle;
                }
            }
        }

        /// <summary>
        /// 코드 가져오기
        /// </summary>
        /// <returns></returns>
        public DataTable GetProdCode(string[] acctTypes)
        {
            try
            {
                if (acctTypes.Length == 0) return null;
            }
            catch (Exception)
            {
                return null;
            }

            string cond = "in (";
            foreach (string type in acctTypes)
            {
                cond += "'" + type + "',";
            }
            cond += "'Empty!@#$')";

            string sql = string.Empty;
            sql += " SELECT pci.ProdNo, ";
            sql += "        pci.ProdName + CASE WHEN pci.AccountType = '3.유효경과' THEN '(유효경과) ' ELSE ' ' END + pci.Standard AS ProdName ";
            sql += "   FROM ProductCodeInfo pci ";
            sql += "  WHERE pci.AccountType " + cond;

            return DBManager.Instance.GetDataTable(sql);
        }

        /// <summary>
        /// 키 체번
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public int fnGetNextKey(string category)
        {
            string sql = $"select fn_getnextkey('{category}', 1) NextKey ";
            return DBManager.Instance.GetIntScalar(sql);
        }

    }
}
