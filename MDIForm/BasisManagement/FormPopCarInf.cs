using DevExpress.Export;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using HanIlCNS.MDILogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HanIlCNS
{
    public partial class FormPopCarInf : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt;

        #region 폼 로드
        /// <summary>
        ///
        /// </summary>
        public FormPopCarInf()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPopCarInf_Load(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region 조회
        /// <summary>
        /// 차량 데이터 로드
        /// </summary>
        private void GetData()
        {
            string sql = string.Empty;
            sql += "select * from carinf ";
            sql += " where 1 = 1 ";
            sql += "   and isdeleted = '0' ";
            sql += "   and carkey != 1 ";
            sql += " order by carkey desc";
            dt = DBManager.Instance.GetDataTable(sql);
            grdCar.DataSource = dt;
            grdViewCar.OptionsView.BestFitMaxRowCount = 100;
            grdViewCar.BestFitColumns();
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 변경 내용 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdViewCar.IsEditing)
            {
                grdViewCar.CloseEditor();
                grdViewCar.UpdateCurrentRow();
            }

            if (XtraMessageBox.Show("변경 내용을 저장하시겠습니까?", "저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                List<string> sqls = new List<string>();
                //추가
                DataTable dtAdded = dt.GetChanges(DataRowState.Added);
                if (dtAdded != null)
                {
                    try
                    {
                        foreach (DataRow dr in dtAdded.Rows)
                        {
                            int carKey = LogicManager.Common.fnGetNextKey("lastcarkey");

                            string sql = string.Empty;
                            sql += "insert into carinf ";
                            sql += "values ( ";
                            sql += $" {carKey}, ";
                            sql += $" N'{dr["carnum"]}', ";
                            sql += $" N'{dr["carbelong"]}', ";
                            if (string.IsNullOrEmpty(dr["carton"].ToString()))
                            {
                                sql += " 0, ";
                            }
                            else
                            {
                                sql += $" {dr["carton"]}, ";
                            }
                            sql += $" N'{dr["driver"]}', ";
                            sql += $" N'{dr["drivertel"]}', ";
                            sql += $" N'{dr["carnote"]}', ";
                            sql += "   current_timestamp, ";
                            sql += "   current_timestamp, ";
                            sql += "   '0' ";
                            sql += ") ";
                            sqls.Add(sql);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                //수정
                DataTable dtChanged = dt.GetChanges(DataRowState.Modified);
                if (dtChanged != null)
                {
                    try
                    {
                        foreach (DataRow dr in dtChanged.Rows)
                        {
                            string sql = string.Empty;
                            sql += "update carinf ";
                            sql += "   set ";
                            sql += $"      carnum = N'{dr["carnum"]}', ";
                            sql += $"      carbelong = N'{dr["carbelong"]}', ";
                            if (string.IsNullOrEmpty(dr["carton"].ToString()))
                            {
                                sql += " carton = 0, ";
                            }
                            else
                            {
                                sql += $" carton = {dr["carton"]}, ";
                            }
                            sql += $"      driver = N'{dr["driver"]}', ";
                            sql += $"      drivertel = N'{dr["drivertel"]}', ";
                            sql += $"      carnote = N'{dr["carnote"]}', ";
                            sql += $"      updatedtm = current_timestamp ";
                            sql += " where 1 = 1 ";
                            sql += $"  and carkey = {dr["carkey"]} ";
                            sqls.Add(sql);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }

                string result = DBManager.Instance.ExcuteTransaction(sqls);
                if (string.IsNullOrEmpty(result))
                {
                    XtraMessageBox.Show("저장되었습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetData();
                }
                else
                {
                    XtraMessageBox.Show("저장 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        /// <summary>
        /// 폼 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (grdViewCar.IsEditing)
            {
                grdViewCar.CloseEditor();
                grdViewCar.UpdateCurrentRow();
            }

            DataTable dtAdded = dt.GetChanges(DataRowState.Added);
            DataTable dtChanged = dt.GetChanges(DataRowState.Modified);
            if (dtAdded != null || dtChanged != null)
            {
                if (XtraMessageBox.Show("변경 내용이 저장되지 않았습니다.\r\n저장하지 않고 화면을 닫으시겠습니까?", "닫기", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            targetFileName = GetFileName("xlsx", string.Format("차량 목록"));
            if (targetFileName.Trim() != "")
                grdCar.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnDelete_Click(object sender, EventArgs e)
        {

            DataRow row = grdViewCar.GetFocusedDataRow();
            if (row == null)
                return;
            
            if (XtraMessageBox.Show("선택한 데이터를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!string.IsNullOrEmpty(row["carkey"].ToString()))
            {
                int carKey = int.Parse(row["carkey"].ToString());
                string sql = string.Empty;
                sql += "update carinf ";
                sql += "   set isdeleted = '1', ";
                sql += "       updatedtm = current_timestamp ";
                sql += " where 1 = 1 ";
                sql += $"  and carkey = {carKey} ";
                int result = DBManager.Instance.ExcuteDataUpdate(sql);

                if (result != 1)
                {
                    XtraMessageBox.Show("삭제 중 오류가 발생했습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            grdViewCar.DeleteSelectedRows();
        }
    }
}