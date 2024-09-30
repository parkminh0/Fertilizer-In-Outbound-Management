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

namespace HanIlCNS
{
    public partial class FormPayAccount : BaseForm
    {
        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormPayAccount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPayAccount_Load(object sender, EventArgs e)
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
            sql += "select pa.payDate, ";
            sql += "       ci.compname, ";
            sql += "       ci.compowner, ";
            sql += "       ci.compaddress, ";
            sql += "       ci.compname, ";
            sql += "       pa.paycategory, ";
            sql += "       pa.payprice, ";
            sql += "       pa.paynote ";
            sql += "  from payaccount pa ";
            sql += "  join compinf ci on ci.compkey = pa.compkey ";
            sql += " where 1 = 1 ";
            sql += "   and pa.isdeleted = '0' ";
            if (!chkPayDate.Checked)
            {
                sql += $" and pa.paydate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
            }
            if (!string.IsNullOrEmpty(txtComp.Text) && txtComp.Text != "전체")
            {
                sql += $" and ci.compname like '%{txtComp.Text}%' ";
            }
            if (!string.IsNullOrEmpty(cboPayCategory.Text) && txtComp.Text != "전체")
            {
                sql += $" and pa.paycategory like '%{cboPayCategory.Text}%' ";
            }
            sql += " order by pa.paydate desc, pa.paykey desc";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            grdPayAccount.DataSource = dt;
            grdViewPayAccount.BestFitMaxRowCount = 100;
            grdViewPayAccount.BestFitColumns();
        }

        /// <summary>
        /// 입금일자 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPayDate_CheckedChanged(object sender, EventArgs e)
        {
            dteFrom.Enabled = dteTo.Enabled = !chkPayDate.Checked;
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            targetFileName = GetFileName("xlsx", string.Format("입금 내역"));
            if (targetFileName.Trim() != "")
                grdPayAccount.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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
        /// 입금입력될 시 새로고침
        /// </summary>
        public override void DoRefresh()
        {
            btnSearch.PerformClick();
        }

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