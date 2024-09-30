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
    public partial class FormOutAccount : BaseForm
    {
        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormOutAccount()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormOutAccount_Load(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;
        }

        /// <summary>
        /// 조회 기간 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkOutDate_CheckedChanged(object sender, EventArgs e)
        {
            dteFrom.Enabled = dteTo.Enabled = !chkOutDate.Checked;
        }
        #endregion

        #region 조회
        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            sql += "select l.outdate, ";
            sql += "       p.compname, ";
            sql += "       d.productname, ";
            sql += "       t.outqty, ";
            sql += "       t.outunitprice, ";
            sql += "       t.outprice, ";
            sql += "       l.destination, ";
            sql += "       case when l.carbelong = '자차' then '상차' ";
            sql += "            else '도착' ";
            sql += "        end as carbelong, ";
            sql += "       c.carnum, ";
            sql += "       t.outnote ";
            sql += "  from outdetail t ";
            sql += "  join outledger l on l.outledgerkey = t.outledgerkey ";
            sql += "  join productinf d on d.productkey = t.productkey ";
            sql += "  join carinf c on c.carkey = l.carkey ";
            sql += "  join compinf p on p.compkey = l.compkey ";
            sql += " where 1 = 1 ";
            sql += "   and t.isdeleted = '0' ";
            if (!chkOutDate.Checked)
            {
                sql += $" and l.outdate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
            }
            if (!string.IsNullOrEmpty(txtComp.Text) && txtComp.Text != "전체")
            {
                sql += $" and p.compname like '%{txtComp.Text}%' ";
            }
            if (!string.IsNullOrEmpty(txtProduct.Text) && txtProduct.Text != "전체")
            {
                sql += $" and d.productname like '%{txtProduct.Text}%' ";
            }
            if (!string.IsNullOrEmpty(txtCar.Text) && txtCar.Text != "전체")
            {
                sql += $" and c.carnum like '%{txtCar.Text}%' ";
            }
            sql += " order by l.outdate desc, t.outdetailkey desc ";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            grdOutDetail.DataSource = dt;
            grdViewOutDetail.OptionsView.BestFitMaxRowCount = 100;
            grdViewOutDetail.BestFitColumns();
        }

        /// <summary>
        /// 출고입력될 시 새로고침
        /// </summary>
        public override void DoRefresh()
        {
            btnSearch.PerformClick();
        }
        #endregion

        /// <summary>
        /// 폼 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            targetFileName = GetFileName("xlsx", string.Format("출고 내역"));
            if (targetFileName.Trim() != "")
                grdOutDetail.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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
    }
}