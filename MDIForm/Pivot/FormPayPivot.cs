using DevExpress.Export;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraPivotGrid.Localization;
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
    public partial class FormPayPivot : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 
        /// </summary>
        public FormPayPivot()
        {
            PivotGridLocalizer.Active = new MyPivotLocalizer();
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPayPivot_Load(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;

            SetFieldName();
        }

        /// <summary>
        /// 일자 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkOutDate_CheckedChanged(object sender, EventArgs e)
        {
            dteFrom.Enabled = dteTo.Enabled = !chkOutDate.Checked;
        }

        /// <summary>
        /// FieldName 세팅
        /// </summary>
        private void SetFieldName()
        {
            pivotGridField1.FieldName = "compname";
            pivotGridField3.FieldName = "prevprice";
            pivotGridField2.FieldName = "outprice";
            pivotGridField4.FieldName = "remainprice";
            pivotGridField10.FieldName = "payprice";
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

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        /// <summary>
        /// 데이터 로드
        /// </summary>
        private void GetData()
        {
            try
            {
                string sql = string.Empty;
                sql += "select ci.compname, (prevoutprice - prevpayprice) prevprice, outprice, payprice, (prevoutprice - prevpayprice + outprice - payprice) remainprice ";
                sql += "  from ( ";
                sql += "		select outledgerkey, compkey, outdate, ";
                sql += $"				case when outdate < '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' then outprice else 0 end prevoutprice,  ";
                sql += $"				case when outdate < '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' then 0 else outprice end outprice,  ";
                sql += $"				case when outdate < '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' then payprice else 0 end prevpayprice,  ";
                sql += $"				case when outdate < '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' then 0 else payprice end payprice  ";
                sql += "		from pay_pivot ";
                sql += $"		 where outdate <= '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
                sql += "	  ) x ";
                sql += "  join compinf ci ";
                sql += "    on ci.compkey = x.compkey";
                DataTable dt = DBManager.Instance.GetDataTable(sql);
                pgrdPivotResult.DataSource = dt;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("Date Load Error : " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {

            targetFileName = GetFileName("xlsx", string.Format("입금 집계"));
            if (targetFileName.Trim() != "")
                pgrdPivotResult.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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
        /// grand total 커스타마이즈
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pgrdPivotResult_CustomDrawFieldValue(object sender, DevExpress.XtraPivotGrid.PivotCustomDrawFieldValueEventArgs e)
        {
            if (e.ValueType == PivotGridValueType.Total)
                e.Info.Caption = "합계";
        }
    }
}