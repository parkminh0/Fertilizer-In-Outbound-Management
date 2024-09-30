using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class FormMakeInspect : BaseForm
    {
        private int ChoiceOutDetailKey;
        private DataTable dtOut;
        private DataTable dtInspect;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormMakeInspect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMakePaynInspect_Load(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;
            rdoOut.SelectedIndex = 2;
            SetLue();

            // 검수내역
            dtInspect = new DataTable();
            DataColumn inspectkey = new DataColumn();
            inspectkey.ColumnName = "inspectkey";
            dtInspect.Columns.Add(inspectkey);

            DataColumn nhkey = new DataColumn();
            nhkey.ColumnName = "nhkey";
            dtInspect.Columns.Add(nhkey);

            DataColumn inspectdate = new DataColumn();
            inspectdate.ColumnName = "inspectdate";
            dtInspect.Columns.Add(inspectdate);

            DataColumn inspectqty = new DataColumn();
            inspectqty.DataType = typeof(int);
            inspectqty.ColumnName = "inspectqty";
            dtInspect.Columns.Add(inspectqty);

            DataColumn inspectunitprice = new DataColumn();
            inspectunitprice.DataType = typeof(int);
            inspectunitprice.ColumnName = "inspectunitprice";
            dtInspect.Columns.Add(inspectunitprice);

            DataColumn inspectprice = new DataColumn();
            inspectprice.DataType = typeof(int);
            inspectprice.ColumnName = "inspectprice";
            dtInspect.Columns.Add(inspectprice);
            grdInspect.DataSource = dtInspect;
        }

        /// <summary>
        /// 룩업에딧 데이터 채우기
        /// </summary>
        private void SetLue()
        {
            string sql = string.Empty;
            sql += "select nhkey, nhname, location1, location2 from nhinf where isdeleted = '0' order by nhname ";
            DataTable dtNH = DBManager.Instance.GetDataTable(sql);
            rplueNH.ValueMember = "nhkey";
            rplueNH.DisplayMember = "nhname";
            rplueNH.DataSource = dtNH;
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
        /// 폼 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
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
            GetOutDetail();
        }

        /// <summary>
        /// 출고내역 로드
        /// </summary>
        private void GetOutDetail()
        {
            string sql = string.Empty;
            sql += "select od.outdetailkey, ";
            sql += "       ol.outdate, ";
            sql += "       ci.compname, ";
            sql += "       pi.productname, ";
            sql += "       od.outqty, ";
            sql += "       od.outunitprice, ";
            sql += "       od.outprice, ";
            sql += "       coalesce((select sum(inspectqty) ";
            sql += "                   from inspectaccount ";
            sql += "                  where outdetailkey = od.outdetailkey ";
            sql += "                    and isdeleted = '0'), 0) as inspectqty, ";
            sql += "       case when ol.carbelong = '자차' then '상차' ";
            sql += "            else '도착' ";
            sql += "        end as outcategory, ";
            sql += "       od.outnote, ";
            sql += "       coalesce((select sum(inspectprice) ";
            sql += "                   from inspectaccount ";
            sql += "                  where outdetailkey = od.outdetailkey ";
            sql += "                    and isdeleted = '0'), 0) as inspectprice ";
            sql += "  from outdetail od ";
            sql += "  join outledger ol on ol.outledgerkey = od.outledgerkey ";
            sql += "  join compinf ci on ci.compkey = ol.compkey ";
            sql += "  join productinf pi on pi.productkey = od.productkey";
            sql += " where 1 = 1 ";
            sql += "   and od.isdeleted = '0' ";
            if (!chkOutDate.Checked)
            {
                sql += $" and ol.outdate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
            }
            if (!string.IsNullOrEmpty(txtComp.Text) && txtComp.Text != "전체")
            {
                sql += $" and ci.compname like '%{txtComp.Text}%' ";
            }
            if (!string.IsNullOrEmpty(txtProduct.Text) && txtProduct.Text != "전체")
            {
                sql += $" and pi.productname like '%{txtProduct.Text}%' ";
            }
            sql += " order by outdate desc, outdetailkey desc ";
            dtOut = DBManager.Instance.GetDataTable(sql);
            grdOut.DataSource = dtOut;
            grdViewOut.OptionsView.BestFitMaxRowCount = 100;
            grdViewOut.BestFitColumns();
        }

        /// <summary>
        /// 검수 내역 띄우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewOut_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            grdViewInspect.OptionsBehavior.Editable = false;
            lyogInspect.Text = "≡ 검수내역";

            DataRow row = grdViewOut.GetFocusedDataRow();
            if (row == null)
            {
                ChoiceOutDetailKey = 0;
                return;
            }

            ChoiceOutDetailKey = int.Parse(row["outdetailkey"].ToString());
            dteOutDate.DateTime = DateTime.Parse(row["outdate"].ToString());
            txtCompName.Text = row["compname"].ToString();
            txtProductName.Text = row["productname"].ToString();
            spnOutQty.EditValue = double.Parse(row["outqty"].ToString());
            spnOutPrice.EditValue = double.Parse(row["outprice"].ToString());
            spnOutUnitPrice.EditValue = double.Parse(row["outunitprice"].ToString());
            spnInspectQty.EditValue = double.Parse(row["inspectqty"].ToString());
            spnInspectPrice.EditValue = double.Parse(row["inspectprice"].ToString());
            txtOutCategory.Text = row["outcategory"].ToString();

            GetInspectData();
        }

        /// <summary>
        /// 검수내역 로드
        /// </summary>
        private void GetInspectData()
        {
            string sql = string.Empty;
            sql += "select * from inspectaccount ";
            sql += " where 1 = 1 ";
            sql += "   and isdeleted = '0' ";
            sql += $"  and outdetailkey = {ChoiceOutDetailKey} ";
            sql += " order by inspectdate desc, inspectkey desc ";
            dtInspect = DBManager.Instance.GetDataTable(sql);
            grdInspect.DataSource = dtInspect;
        }

        /// <summary>
        /// 미검수/전체 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdoOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dtOut == null || dtOut.Rows.Count == 0)
                return;

            DataTable modChart = dtOut.Copy();
            DataView dv = modChart.DefaultView;

            switch (rdoOut.SelectedIndex)
            {
                case 0: // 미검수, 출고수량 > 검수량
                    dv.RowFilter = "outqty - inspectqty > 0";
                    break;
                case 1: // 전체
                    dv.RowFilter = "";
                    break;
            }

            grdOut.DataSource = modChart;
        }
        #endregion

        #region 검수내역
        /// <summary>
        /// 검수내역 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnInspectDelete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            grdViewInspect.DeleteSelectedRows();
        }

        /// <summary>
        /// 금액 자동계산
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewInspect_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            // 수량 or 단가 -> 금액
            if (e.Column.FieldName == "inspectqty" || e.Column.FieldName == "inspectunitprice")
            {
                double qty = 0;
                double price = 0;
                double.TryParse(gridView.GetFocusedRowCellValue("inspectqty").ToString(), out qty);
                double.TryParse(gridView.GetFocusedRowCellValue("inspectunitprice").ToString(), out price);

                double inspectPrice = qty * price;

                gridView.SetRowCellValue(e.RowHandle, "inspectprice", inspectPrice);
            }
        }

        /// <summary>
        /// 검수내역 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grpInspect_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (ChoiceOutDetailKey == 0)
            {
                XtraMessageBox.Show("출고내역 선택 후 다시 시도해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (e.Button.Properties.Tag.ToString())
            {
                case "Edit":
                    grdViewInspect.OptionsBehavior.Editable = true;
                    lyogInspect.Text = "≡ 검수내역[수정]";
                    break;
                case "Save":
                    if (!grdViewInspect.OptionsBehavior.Editable)
                        return;

                    if (grdViewInspect.IsEditing)
                    {
                        grdViewInspect.CloseEditor();
                        grdViewInspect.UpdateCurrentRow();
                    }

                    if (XtraMessageBox.Show("검수내역을 저장하시겠습니까?", "검수내역", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    double chkInspectQty = 0;
                    foreach (DataRow row in dtInspect.Rows)
                    {
                        try
                        {
                            chkInspectQty += double.Parse(row["inspectqty"].ToString());
                        }
                        catch
                        {
                        }
                    }

                    if (chkInspectQty > double.Parse(spnOutQty.EditValue.ToString()))
                    {
                        XtraMessageBox.Show("검수량이 출고수량보다 많습니다.\r\n검수량을 수정해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    try
                    {
                        List<string> sqls = new List<string>();
                        //추가
                        DataTable dtAdded = dtInspect.GetChanges(DataRowState.Added);
                        if (dtAdded != null)
                        {
                            try
                            {
                                foreach (DataRow dr in dtAdded.Rows)
                                {
                                    int inspectKey = LogicManager.Common.fnGetNextKey("lastinspectaccountkey");
                                    string sql = string.Empty;
                                    sql += "insert into inspectaccount ";
                                    sql += "values ( ";
                                    sql += $"      {inspectKey}, ";
                                    if (string.IsNullOrEmpty(dr["nhkey"].ToString()))
                                    {
                                        XtraMessageBox.Show("농협을 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    sql += $"      {dr["nhkey"]}, ";
                                    sql += $"      {ChoiceOutDetailKey}, ";
                                    if (string.IsNullOrEmpty(dr["inspectdate"].ToString()))
                                    {
                                        XtraMessageBox.Show("검수 일자를 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    sql += $"      '{DateTime.Parse(dr["inspectdate"].ToString()).ToString("yyyy-MM-dd")}', ";
                                    if (string.IsNullOrEmpty(dr["inspectqty"].ToString()))
                                        sql += "   0, ";
                                    else
                                        sql += $"      {dr["inspectqty"]}, ";
                                    if (string.IsNullOrEmpty(dr["inspectunitprice"].ToString()))
                                        sql += "   0, ";
                                    else
                                        sql += $"      {dr["inspectunitprice"]}, ";
                                    if (string.IsNullOrEmpty(dr["inspectprice"].ToString()))
                                        sql += "   0, ";
                                    else
                                        sql += $"      {dr["inspectprice"]}, ";
                                    sql += "       current_timestamp, ";
                                    sql += "       current_timestamp, ";
                                    sql += "       '0' ";
                                    sql += " ) ";
                                    sqls.Add(sql);
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        //수정
                        DataTable dtChanged = dtInspect.GetChanges(DataRowState.Modified);
                        if (dtChanged != null)
                        {
                            try
                            {
                                foreach (DataRow dr in dtChanged.Rows)
                                {
                                    string sql = string.Empty;
                                    sql += "update inspectaccount ";
                                    sql += "   set ";
                                    if (string.IsNullOrEmpty(dr["inspectdate"].ToString()))
                                    {
                                        XtraMessageBox.Show("검수 일자를 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    sql += $"      inspectdate = '{DateTime.Parse(dr["inspectdate"].ToString()).ToString("yyyy-MM-dd")}', ";
                                    if (string.IsNullOrEmpty(dr["inspectqty"].ToString()))
                                        sql += "   inspectqty = 0, ";
                                    else
                                        sql += $"  inspectqty = {dr["inspectqty"]}, ";
                                    if (string.IsNullOrEmpty(dr["inspectunitprice"].ToString()))
                                        sql += "   inspectunitprice = 0, ";
                                    else
                                        sql += $"  inspectunitprice = {dr["inspectunitprice"]}, ";
                                    if (string.IsNullOrEmpty(dr["inspectprice"].ToString()))
                                        sql += "   inspectprice = 0, ";
                                    else
                                        sql += $"  inspectprice = {dr["inspectprice"]}, ";
                                    sql += "       updatedtm = current_timestamp ";
                                    sql += " where 1 = 1 ";
                                    sql += $"  and inspectkey = {dr["inspectkey"]} ";
                                    sqls.Add(sql);
                                }
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show(ex.Message, "");
                            }
                        }

                        //삭제
                        DataTable dtDeleted = dtInspect.GetChanges(DataRowState.Deleted);
                        if (dtDeleted != null)
                        {
                            try
                            {
                                foreach (DataRow dr in dtDeleted.Rows)
                                {
                                    var beforeVal = dr["inspectkey", DataRowVersion.Original];
                                    string sql = string.Empty;
                                    sql += "update inspectaccount ";
                                    sql += "   set isdeleted = '1', ";
                                    sql += "       updatedtm = current_timestamp ";
                                    sql += " where 1 = 1 ";
                                    sql += $"  and inspectkey = {beforeVal} ";
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
                            XtraMessageBox.Show("검수내역이 저장되었습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            grdViewInspect.OptionsBehavior.Editable = false;
                            lyogInspect.Text = "≡ 검수내역";
                            LogicManager.Common.RefreshGridView(GetOutDetail, grdViewOut);
                        }
                        else
                        {
                            XtraMessageBox.Show("검수내역 저장 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    break;
            }
        }

        /// <summary>
        /// 출고입력될 시 새로고침
        /// </summary>
        public override void DoRefresh()
        {
            SetLue();
        }
        #endregion
    }
}