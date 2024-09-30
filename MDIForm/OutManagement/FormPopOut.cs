using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
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
    public partial class FormPopOut : DevExpress.XtraEditors.XtraForm
    {
        private DataTable dt;
        private int ChoiceCarKey;
        private int ChoiceCompKey;
        private int maxRow = 7;
        private int outledgerkey;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormPopOut()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormPopOut_Load(object sender, EventArgs e)
        {
            dteOutDate.EditValue = DateTime.Now;
            SetLue();

            dt = new DataTable();
            DataColumn productkey = new DataColumn();
            productkey.ColumnName = "productkey";
            dt.Columns.Add(productkey);

            DataColumn outqty = new DataColumn();
            outqty.DataType = typeof(double);
            outqty.ColumnName = "outqty";
            dt.Columns.Add(outqty);
            
            DataColumn outunitprice = new DataColumn();
            outunitprice.DataType = typeof(double);
            outunitprice.ColumnName = "outunitprice";
            dt.Columns.Add(outunitprice);
            
            DataColumn outprice = new DataColumn();
            outprice.DataType = typeof(double);
            outprice.ColumnName = "outprice";
            dt.Columns.Add(outprice);
            
            DataColumn outnote = new DataColumn();
            outnote.ColumnName = "outnote";
            dt.Columns.Add(outnote);
            grdOut.DataSource = dt;
        }

        /// <summary>
        /// 룩업에딧 데이터 채우기
        /// </summary>
        private void SetLue()
        {
            string sql = string.Empty;
            sql += "select compkey, compcode, compname from compinf where isdeleted = '0' order by compname ";
            DataTable dtComp = DBManager.Instance.GetDataTable(sql);
            lueComp.Properties.ValueMember = "compkey";
            lueComp.Properties.DisplayMember = "compname";
            lueComp.Properties.DataSource = dtComp;

            sql = string.Empty;
            sql += "select * ";
            sql += "from ";
            sql += "(  select carkey, ";
            sql += "          carnum, ";
            sql += "          carbelong, ";
            sql += "     case when carkey = 1 and carnum = '미선택' then 0 ";
            sql += "          else 1 ";
            sql += "          end seq ";
            sql += "     from carinf ";
            sql += "    where isdeleted = '0') x ";
            sql += " order by seq, carnum";
            DataTable dtCar = DBManager.Instance.GetDataTable(sql);
            lueCar.Properties.ValueMember = "carkey";
            lueCar.Properties.DisplayMember = "carnum";
            lueCar.Properties.DataSource = dtCar;
            lueCar.EditValue = 1;

            sql = string.Empty;
            sql += "select * ";
            sql += "  from ( ";
            sql += "	select productkey, ";
            sql += "	       productname, ";
            sql += "	       prodprice, ";
            sql += "      case when productkey = 1 and productname = '부가세' ";
            sql += "	       then 0 ";
            sql += "	       else 1 ";
            sql += "	   end seq ";
            sql += "	  from productinf ";
            sql += "     where isdeleted = '0' ";
            sql += "       ) x ";
            sql += " order by seq, productname";
            DataTable dtProduct = DBManager.Instance.GetDataTable(sql);
            rplueProduct.ValueMember = "productkey";
            rplueProduct.DisplayMember = "productname";
            rplueProduct.DataSource = dtProduct;
        }
        #endregion

        #region 출고원장 설정
        /// <summary>
        /// 거래선키 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueComp_EditValueChanged(object sender, EventArgs e)
        {
            ChoiceCompKey = int.Parse(lueComp.EditValue.ToString());
        }

        /// <summary>
        /// 차량키 저장 및 차량 관련 txt 채우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueCar_EditValueChanged(object sender, EventArgs e)
        {
            ChoiceCarKey = int.Parse(lueCar.EditValue.ToString());
            string sql = string.Empty;
            sql += $"select driver, drivertel, carbelong from carinf where carkey = {ChoiceCarKey} ";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            txtDriver.Text = dt.Rows[0]["driver"].ToString();
            txtDriverTel.Text = dt.Rows[0]["drivertel"].ToString();
            string CarBelong = dt.Rows[0]["carbelong"].ToString();
            if (CarBelong == "자차")
            {
                chkCarBelong.Checked = true;
                txtCarBelong.Text = "";
            }
            else
            {
                chkCarBelong.Checked = false;
                txtCarBelong.Text = CarBelong;
            }
        }

        /// <summary>
        /// 차량 소속 자차 or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCarBelong_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCarBelong.Checked)
            {
                txtCarBelong.Enabled = false;
            }
            else
            {
                txtCarBelong.Enabled = true;
            }
        }
        #endregion

        #region 출고세부 설정
        /// <summary>
        /// Grid 세팅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewOut_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GridView gridView = sender as GridView;

            // 품명 선택 -> 단가
            if (e.Column.FieldName == "productkey")
            {
                int key = int.Parse(gridView.GetFocusedRowCellValue("productkey").ToString());
                if (key == 1)
                {
                    gridView.SetRowCellValue(e.RowHandle, "outqty", 1);
                }
                double price = DBManager.Instance.GetDoubleScalar($"select prodprice from productinf where productkey = {key}");
                gridView.SetRowCellValue(e.RowHandle, "outunitprice", price);
            }

            // 수량 or 단가 -> 금액
            if (e.Column.FieldName == "outqty" || e.Column.FieldName == "outunitprice")
            {
                double qty = 0;
                double price = 0;
                double.TryParse(gridView.GetFocusedRowCellValue("outqty").ToString(), out qty);
                double.TryParse(gridView.GetFocusedRowCellValue("outunitprice").ToString(), out price);
                double outPrice = qty * price;
                gridView.SetRowCellValue(e.RowHandle, "outprice", outPrice);
            }

            // 토탈 수량, 금액
            if (e.Column.FieldName == "outprice")
            {
                double totalPrice = 0;
                double totalQty = 0;

                foreach (DataRow row in dt.Rows)
                {
                    double tprice = 0;
                    double tqty = 0;
                    double.TryParse(row["outprice"].ToString(), out tprice);
                    double.TryParse(row["outqty"].ToString(), out tqty);
                    totalPrice += tprice;
                    totalQty += tqty;
                }

                spnTotalOutPrice.EditValue = totalPrice;
                spnTotalOutQty.EditValue = totalQty;
            }
        }

        /// <summary>
        /// 토탈 수량, 금액
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewOut_RowCountChanged(object sender, EventArgs e)
        {
            double totalPrice = 0;
            double totalQty = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                    continue;

                double tprice = 0;
                double tqty = 0;
                try
                {
                    double.TryParse(row["outprice"].ToString(), out tprice);
                    double.TryParse(row["outqty"].ToString(), out tqty);
                }
                catch
                {
                }
                totalPrice += tprice;
                totalQty += tqty;
            }

            spnTotalOutPrice.EditValue = totalPrice;
            spnTotalOutQty.EditValue = totalQty;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnDelete_Click(object sender, EventArgs e)
        {
            grdViewOut.DeleteSelectedRows();
        }
        #endregion

        #region 저장
        /// <summary>
        /// 출고 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOut_Click(object sender, EventArgs e)
        {
            if (grdViewOut.IsEditing)
            {
                grdViewOut.CloseEditor();
                grdViewOut.UpdateCurrentRow();
            }

            if (string.IsNullOrEmpty(dteOutDate.Text))
            {
                XtraMessageBox.Show("출고일자를 입력해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(lueComp.Text))
            {
                XtraMessageBox.Show("거래선을 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(lueCar.Text))
            {
                XtraMessageBox.Show("차량을 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dt.Rows.Count == 0 || dt == null)
            {
                XtraMessageBox.Show("출고 품종을 입력해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (XtraMessageBox.Show("출고하시겠습니까?", "출고", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            outledgerkey = LogicManager.Common.fnGetNextKey("lastoutledgerkey");

            List<string> sqls = new List<string>();
            string sql = string.Empty;
            sql += "insert into outledger ";
            sql += "values ( ";
            sql += $"      {outledgerkey}, ";
            sql += $"      {ChoiceCarKey}, ";
            sql += $"      {ChoiceCompKey}, ";
            sql += $"      '{dteOutDate.DateTime.ToString("yyyy-MM-dd")}', ";
            sql += $"      N'{txtDriver.Text}', ";
            sql += $"      N'{txtDriverTel.Text}', ";
            if (chkCarBelong.Checked)
            {
                sql += $"  N'자차', ";
            }
            else
            {
                sql += $"  N'{txtCarBelong.Text}', ";
            }
            sql += $"      N'{txtDestination.Text}', ";
            sql += "       current_timestamp, ";
            sql += "       current_timestamp, ";
            sql += "       '0' ";
            sql += " ) ";
            sqls.Add(sql);

            foreach (DataRow dr in dt.Rows)
            {
                int outdetailkey = LogicManager.Common.fnGetNextKey("lastoutdetailkey");
                sql = string.Empty;
                sql += "insert into outdetail ";
                sql += "values ( ";
                sql += $"      {outdetailkey}, ";
                sql += $"      {outledgerkey}, ";
                if (string.IsNullOrEmpty(dr["productkey"].ToString()))
                {
                    XtraMessageBox.Show("품종을 선택해주세요.", "품종 미선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                sql += $"      {dr["productkey"]}, ";
                if (string.IsNullOrEmpty(dr["outqty"].ToString()))
                    sql += "   0, ";
                else
                    sql += $"      {dr["outqty"]}, ";
                sql += $"      (select prodprice from productinf where productkey = {dr["productkey"]}), ";
                if (string.IsNullOrEmpty(dr["outunitprice"].ToString()))
                    sql += "   0, ";
                else
                    sql += $"      {dr["outunitprice"]}, ";
                if (string.IsNullOrEmpty(dr["outprice"].ToString()))
                    sql += "   0, ";
                else
                    sql += $"      {dr["outprice"]}, ";
                sql += $"      N'{dr["outnote"]}', ";
                sql += "       current_timestamp, ";
                sql += "       current_timestamp, ";
                sql += "       '0' ";
                sql += " ) ";
                sqls.Add(sql);
            }

            string result = DBManager.Instance.ExcuteTransaction(sqls);
            if (string.IsNullOrEmpty(result))
            {
                XtraMessageBox.Show("출고가 완료되었습니다.", "출고완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;

                Show_OutInvoice();
            }
            else
            {
                XtraMessageBox.Show("출고 중 오류가 발생하였습니다.", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            Close();
        }

        /// <summary>
        /// 송장 출력
        /// </summary>
        private void Show_OutInvoice()
        {
            DataTable dtOutLedger = MakeOutInvoice.setDataTable(outledgerkey);
            if (dtOutLedger == null || dtOutLedger.Rows.Count == 0)
            {
                XtraMessageBox.Show("송장 생성 중 오류가 발생했습니다.", "송장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataRow row = dtOutLedger.Rows[0];

            DataTable dtOutDetail = MakeOutInvoice.setOutInvoiceDetail(outledgerkey);
            if (dtOutDetail == null || dtOutDetail.Rows.Count == 0)
            {
                XtraMessageBox.Show("송장 생성 중 오류가 발생했습니다.", "송장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable sdt1 = dtOutDetail.Copy();
            sdt1.Columns.Add("no");

            LogicManager.Report.makeReportCommon("Out Invoice", 50, 50, 50, 50, false);
            DataTable tmpDt = sdt1.Clone();
            int i = 1;
            foreach (DataRow dr in sdt1.Rows)
            {
                dr["no"] = i;

                tmpDt.ImportRow(dr);
                if (i % maxRow == 0)
                {
                    ReportInvoice report = new ReportInvoice(tmpDt, row);
                    LogicManager.Report.CreateCompositeReport(report, true);
                    tmpDt = sdt1.Clone();
                }
                i++;
            }

            if (tmpDt.Rows.Count != 0)
            {
                DataRow tmpRow = sdt1.Rows[0];
                tmpRow[3] = tmpRow[8] = null;
                tmpRow[0] = tmpRow[1] = tmpRow[2] = tmpRow[4] = tmpRow[5] = tmpRow[6] = tmpRow[7] = tmpRow[9] = DBNull.Value;
                int limit = tmpDt.Rows.Count;
                for (int j = 0; j < maxRow - limit; j++)
                {
                    tmpDt.ImportRow(tmpRow);
                }
                ReportInvoice report = new ReportInvoice(tmpDt, row);
                LogicManager.Report.CreateCompositeReport(report, true);
            }

            LogicManager.Report.MainReport.ShowPreview();
        }
    }
}