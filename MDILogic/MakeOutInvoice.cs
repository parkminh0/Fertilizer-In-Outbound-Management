using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanIlCNS
{
    public class MakeOutInvoice
    {
        /// <summary>
        /// 원장
        /// </summary>
        /// <param name="outledgerkey"></param>
        /// <returns></returns>
        public static DataTable setDataTable(int outledgerkey)
        {
            string sql = string.Empty;
            sql += "select l.outledgerkey, ";
            sql += "       p.compkey, ";
            sql += "       l.outdate, ";
            sql += "       p.compname, ";
            sql += "       sum(t.outqty) as outqty, ";
            sql += "       sum(t.outprice) as outprice, ";
            sql += "       p.compregistnum, ";
            sql += "       p.compowner, ";
            sql += "       p.compaddress, ";
            sql += "       c.carkey, ";
            sql += "       c.carnum, ";
            sql += "       l.driver, ";
            sql += "       l.drivertel, ";
            sql += "       l.carbelong, ";
            sql += "       l.destination ";
            sql += "  from outledger l ";
            sql += "  join carinf c on c.carkey = l.carkey ";
            sql += "  join compinf p on p.compkey = l.compkey ";
            sql += "  join outdetail t on t.outledgerkey = l.outledgerkey ";
            sql += " where 1 = 1 ";
            sql += "   and l.isdeleted = '0' ";
            sql += $"  and l.outledgerkey = '{outledgerkey}' ";
            sql += " group by l.outledgerkey, p.compkey, c.carkey ";
            sql += " order by l.outdate desc, l.outledgerkey desc ";
            DataTable dtOutLedger = DBManager.Instance.GetDataTable(sql);

            return dtOutLedger;
        }

        /// <summary>
        /// 품종
        /// </summary>
        /// <param name="outledgerkey"></param>
        /// <returns></returns>
        public static DataTable setOutInvoiceDetail(int outledgerkey)
        {
            // OutDetail
            string sql = string.Empty;
            sql += "select d.outdetailkey, ";
            sql += "       d.outledgerkey, ";
            sql += "       d.productkey, ";
            sql += "       p.productname, ";
            sql += "       p.prodprice, ";
            sql += "       d.outqty, ";
            sql += "       d.outunitprice, ";
            sql += "       d.outprice, ";
            sql += "       d.outnote ";
            sql += "  from outdetail d ";
            sql += "  join productinf p on p.productkey = d.productkey ";
            sql += " where 1 = 1 ";
            sql += $"  and d.outledgerkey = {outledgerkey} ";
            sql += $"  and d.isdeleted = '0' ";
            sql += $"order by d.outdetailkey ";
            DataTable dtOutDetail = DBManager.Instance.GetDataTable(sql);

            return dtOutDetail;
        }
    }
}
