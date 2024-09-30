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
    public partial class FormPivot : DevExpress.XtraEditors.XtraForm
    {
        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormPivot()
        {
            PivotGridLocalizer.Active = new MyPivotLocalizer();
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPivot_Load(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;
            dteFrom1.DateTime = DateTime.Now.AddDays(-7);
            dteTo1.DateTime = DateTime.Now;

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
            dteFrom1.Enabled = dteTo1.Enabled = !chkInspectDate.Checked;
        }

        /// <summary>
        /// FieldName 세팅
        /// </summary>
        private void SetFieldName()
        {
            pivotGridField1.FieldName = "compname";
            pivotGridField2.FieldName = "productname";
            pivotGridField3.FieldName = "notinspectprice";
            pivotGridField5.FieldName = "outprice";
            pivotGridField6.FieldName = "inspectqty";
            pivotGridField7.FieldName = "notinspectqty";
            pivotGridField8.FieldName = "outqty";
            pivotGridField9.FieldName = "inspectprice";
            pivotGridField13.FieldName = "outdate";
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
        #endregion

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
                sql += "select * from out_pivot ";
                sql += " where 1 = 1 ";
                if (!chkOutDate.Checked)
                {
                    sql += $" and outdate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
                }
                if (!chkInspectDate.Checked)
                {
                    sql += $" and inspectdate between '{dteFrom1.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo1.DateTime.ToString("yyyy-MM-dd")}' ";
                }
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

            targetFileName = GetFileName("xlsx", string.Format("집계"));
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