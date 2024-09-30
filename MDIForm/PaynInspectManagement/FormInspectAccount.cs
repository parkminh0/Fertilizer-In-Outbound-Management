using DevExpress.Export;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HanIlCNS
{
    public partial class FormInspectAccount : BaseForm
    {
        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormInspectAccount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormInspectAccount_Load(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            sql += "select ia.inspectkey, ";
            sql += "       ia.inspectdate, ";
            sql += "       ci.compname, ";
            sql += "       pi.productname, ";
            sql += "       od.outunitprice, ";
            sql += "       od.outqty, ";
            sql += "       od.outprice, ";
            sql += "       ia.inspectunitprice, ";
            sql += "       ia.inspectqty, ";
            sql += "       ia.inspectprice, ";
            sql += "       ni.nhname ";
            sql += "  from inspectaccount ia ";
            sql += "  join nhinf ni on ni.nhkey = ia.nhkey ";
            sql += "  join outdetail od on od.outdetailkey = ia.outdetailkey ";
            sql += "  join productinf pi on pi.productkey = od.productkey ";
            sql += "  join outledger ol on ol.outledgerkey = od.outledgerkey ";
            sql += "  join compinf ci on ci.compkey = ol.compkey ";
            sql += " where 1 = 1 ";
            sql += "   and ia.isdeleted = '0' ";
            sql += "   and od.isdeleted = '0' ";
            if (!chkInspectDate.Checked)
            {
                sql += $" and ia.inspectdate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
            }
            if (!string.IsNullOrEmpty(txtComp.Text) && txtComp.Text != "전체")
            {
                sql += $" and p.compname like '%{txtComp.Text}%' ";
            }
            if (!string.IsNullOrEmpty(txtProduct.Text) && txtProduct.Text != "전체")
            {
                sql += $" and d.productname like '%{txtProduct.Text}%' ";
            }
            sql += " order by ia.inspectdate desc, ia.inspectkey desc ";

            DataTable dt = DBManager.Instance.GetDataTable(sql);
            grdInspectAccount.DataSource = dt;
            grdViewInspectAccount.BestFitMaxRowCount = 100;
            grdViewInspectAccount.BestFitColumns();
        }

        /// <summary>
        /// 검수일자 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkInspectDate_CheckedChanged(object sender, EventArgs e)
        {
            dteFrom.Enabled = dteTo.Enabled = !chkInspectDate.Checked;
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            targetFileName = GetFileName("xlsx", string.Format("검수 내역"));
            if (targetFileName.Trim() != "")
                grdInspectAccount.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
                {
                    ExportType = ExportType.WYSIWYG
                });
        }

        /// <summary>
        /// 파일명 가져오기
        /// </summary>
        /// <param name="extName"></param>
        /// <returns></returns>
        private string GetFileName(string extName, string defaultName)
        {
            FileDialog fileDialog = new SaveFileDialog();
            if (extName == "xlsx")
            {
                fileDialog.DefaultExt = "xlsx";
                fileDialog.Filter = "Excel File(*.xlsx)|*.xlsx";
                fileDialog.FileName = string.Format("{0}_{1}.xlsx", defaultName, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            else
            {
                return "Export_" + DateTime.Now.ToString("yyyyMMdd");
            }

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return "";

            string fileName = fileDialog.FileName;
            if (fileName.ToLower() == "*.xlsx")
                fileName = "";

            return fileName;
        }
        static string targetFileName;

        /// <summary>
        /// 폼 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}