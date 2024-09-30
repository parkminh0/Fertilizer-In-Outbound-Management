using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;

namespace HanIlCNS
{
    public partial class ReportInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        private DataTable dtOutDetail;
        private DataRow row;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtOutDetail"></param>
        public ReportInvoice(DataTable dtOutDetail, DataRow row)
        {
            InitializeComponent();
            this.dtOutDetail = dtOutDetail;
            this.row = row;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrSubreport1_BeforePrint(object sender, CancelEventArgs e)
        {
            ReportSub1 reportSub1 = new ReportSub1(LogicManager.Report.CreateOutDetailData(dtOutDetail));
            xrSubreport1.ReportSource = reportSub1;

            reportSub1.Parameters["outdate"].Value = string.Format($"출하일자:    {DateTime.Parse(row["outdate"].ToString()).ToString("yyyy-MM-dd")}");
            reportSub1.Parameters["compregistnum"].Value = row["compregistnum"].ToString();
            reportSub1.Parameters["compname"].Value = row["compname"].ToString();
            reportSub1.Parameters["compowner"].Value = row["compowner"].ToString();
            reportSub1.Parameters["compaddress"].Value = row["compaddress"].ToString();
            reportSub1.Parameters["destination"].Value = row["destination"].ToString();
            reportSub1.Parameters["carbelong"].Value = row["carbelong"].ToString();
            reportSub1.Parameters["driver"].Value = row["driver"].ToString();
            reportSub1.Parameters["carnum"].Value = row["carnum"].ToString();
            reportSub1.Parameters["drivertel"].Value = row["drivertel"].ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xrSubreport2_BeforePrint(object sender, CancelEventArgs e)
        {
            ReportSub2 reportSub2 = new ReportSub2(LogicManager.Report.CreateOutDetailData(dtOutDetail));
            xrSubreport2.ReportSource = reportSub2;

            reportSub2.Parameters["outdate"].Value = string.Format($"출하일자:    {DateTime.Parse(row["outdate"].ToString()).ToString("yyyy-MM-dd")}");
            reportSub2.Parameters["compregistnum"].Value = row["compregistnum"].ToString();
            reportSub2.Parameters["compname"].Value = row["compname"].ToString();
            reportSub2.Parameters["compowner"].Value = row["compowner"].ToString();
            reportSub2.Parameters["compaddress"].Value = row["compaddress"].ToString();
            reportSub2.Parameters["destination"].Value = row["destination"].ToString();
            reportSub2.Parameters["carbelong"].Value = row["carbelong"].ToString();
            reportSub2.Parameters["driver"].Value = row["driver"].ToString();
            reportSub2.Parameters["carnum"].Value = row["carnum"].ToString();
            reportSub2.Parameters["drivertel"].Value = row["drivertel"].ToString();
        }
    }
}
