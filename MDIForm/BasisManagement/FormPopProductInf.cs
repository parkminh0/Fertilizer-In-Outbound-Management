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
    public partial class FormPopProductInf : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormPopProductInf()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPopProductInf_Load(object sender, EventArgs e)
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
            sql = "select * from productinf ";
            sql += "where 1 = 1 ";
            sql += "  and isdeleted = '0' "; 
            sql += "  and productkey != 1 ";
            sql += "order by productcode desc ";
            dt = DBManager.Instance.GetDataTable(sql);
            grdProduct.DataSource = dt;
            grdViewProduct.OptionsView.BestFitMaxRowCount = 100;
            grdViewProduct.BestFitColumns();
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = grdViewProduct.GetFocusedDataRow();
            if (row == null)
                return;

            if (XtraMessageBox.Show("선택한 데이터를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!string.IsNullOrEmpty(row["productkey"].ToString()))
            {
                int productKey = int.Parse(row["productkey"].ToString());
                string sql = string.Empty;
                sql += "update productinf ";
                sql += "   set isdeleted = '1', ";
                sql += "       updatedtm = current_timestamp ";
                sql += " where 1 = 1 ";
                sql += $"  and productkey = {productKey} ";
                int result = DBManager.Instance.ExcuteDataUpdate(sql);

                if (result != 1)
                {
                    XtraMessageBox.Show("삭제 중 오류가 발생했습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            grdViewProduct.DeleteSelectedRows();
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (grdViewProduct.IsEditing)
            {
                grdViewProduct.CloseEditor();
                grdViewProduct.UpdateCurrentRow();
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
                            int productKey = LogicManager.Common.fnGetNextKey("lastproductkey");

                            string sql = string.Empty;
                            sql += "insert into productinf ";
                            sql += "values ( ";
                            sql += $" {productKey}, ";
                            sql += $" N'{dr["productcode"]}', ";
                            sql += $" N'{dr["productname"]}', ";
                            sql += $" N'{dr["prodweight"]}', ";
                            sql += $" N'{dr["prodcategory"]}', ";
                            if (string.IsNullOrEmpty(dr["prodprice"].ToString()))
                            {
                                sql += " 0, ";
                            }
                            else
                            {
                                sql += $" {dr["prodprice"]}, ";
                            }
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
                if ( dtChanged != null)
                {
                    try
                    {
                        foreach (DataRow dr in dtChanged.Rows)
                        {
                            string sql = string.Empty;
                            sql += "update productinf ";
                            sql += "   set ";
                            sql += $"      productcode = N'{dr["productcode"]}', ";
                            sql += $"      productname = N'{dr["productname"]}', ";
                            sql += $"      prodweight = N'{dr["prodweight"]}', ";
                            sql += $"      prodcategory = N'{dr["prodcategory"]}', ";
                            if (string.IsNullOrEmpty(dr["prodprice"].ToString()))
                            {
                                sql += " prodprice = null, ";
                            }
                            else
                            {
                                sql += $" prodprice = {dr["prodprice"]}, ";
                            }
                            sql += $"      updatedtm = current_timestamp ";
                            sql += " where 1 = 1 ";
                            sql += $"  and productkey = {dr["productkey"]} ";
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
        #endregion

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (grdViewProduct.IsEditing)
            {
                grdViewProduct.CloseEditor();
                grdViewProduct.UpdateCurrentRow();
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
            targetFileName = GetFileName("xlsx", string.Format("상품 목록"));
            if (targetFileName.Trim() != "")
                grdProduct.ExportToXlsx(targetFileName, new XlsxExportOptionsEx
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