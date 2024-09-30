using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace HanIlCNS
{
    public partial class ReportSub1 : DevExpress.XtraReports.UI.XtraReport
    {
        private List<OutDetailData> data;

        /// <summary>
        /// 
        /// </summary>
        public ReportSub1(List<OutDetailData> data)
        {
            InitializeComponent();
            this.data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReportSub1_BeforePrint(object sender, CancelEventArgs e)
        {
            this.DataSource = data;
        }
    }
}
