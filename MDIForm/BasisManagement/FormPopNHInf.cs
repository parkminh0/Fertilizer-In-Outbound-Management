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
    public partial class FormPopNHInf : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormPopNHInf()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPopNHInf_Load(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region 조회
        /// <summary>
        /// 데이터 로드
        /// </summary>
        private void GetData()
        {
            string sql = string.Empty;
            sql += "select * from nhinf where isdeleted = '0' order by nhkey desc";
            dt = DBManager.Instance.GetDataTable(sql);
            grdNH.DataSource = dt;
            grdViewNH.OptionsView.BestFitMaxRowCount = 100;
            grdViewNH.BestFitColumns();
        }

        /// <summary>
        /// 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = grdViewNH.GetFocusedDataRow();
            if (row == null)
                return;

            if (XtraMessageBox.Show("선택한 데이터를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!string.IsNullOrEmpty(row["nhkey"].ToString()))
            {
                int nhKey = int.Parse(row["nhkey"].ToString());
                string sql = string.Empty;
                sql += "update nhinf ";
                sql += "   set isdeleted = '1', ";
                sql += "       updatedtm = current_timestamp ";
                sql += " where 1 = 1 ";
                sql += $"  and nhkey = {nhKey} ";
                int result = DBManager.Instance.ExcuteDataUpdate(sql);

                if (result != 1)
                {
                    XtraMessageBox.Show("삭제 중 오류가 발생했습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            grdViewNH.DeleteSelectedRows();
        }
        #endregion

        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdViewNH.IsEditing)
            {
                grdViewNH.CloseEditor();
                grdViewNH.UpdateCurrentRow();
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
                            int NHKey = LogicManager.Common.fnGetNextKey("lastnhkey");

                            string sql = string.Empty;
                            sql += "insert into nhinf ";
                            sql += "values ( ";
                            sql += $" {NHKey}, ";
                            sql += $" N'{dr["location1"]}', ";
                            sql += $" N'{dr["location2"]}', ";
                            sql += $" N'{dr["nhname"]}', ";
                            sql += "   current_timestamp, ";
                            sql += "   current_timestamp, ";
                            sql += "   '0', ";
                            sql += $" N'{dr["nhnote"]}' ";
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
                if (dtChanged != null )
                {
                    try
                    {
                        foreach (DataRow dr in dtChanged.Rows)
                        {
                            string sql = string.Empty;
                            sql += "update nhinf ";
                            sql += "   set ";
                            sql += $"      location1 = N'{dr["location1"]}', ";
                            sql += $"      location2 = N'{dr["location2"]}', ";
                            sql += $"      nhname = N'{dr["nhname"]}', ";
                            sql += $"      nhnote = N'{dr["nhnote"]}', ";
                            sql += $"      updatedtm = current_timestamp ";
                            sql += " where 1 = 1 ";
                            sql += $"  and nhkey = {dr["nhkey"]} ";
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
                    XtraMessageBox.Show("오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 폼 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (grdViewNH.IsEditing)
            {
                grdViewNH.CloseEditor();
                grdViewNH.UpdateCurrentRow();
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
            targetFileName = GetFileName("xlsx", string.Format("농협 목록"));
            if (targetFileName.Trim() != "")
                grdNH.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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