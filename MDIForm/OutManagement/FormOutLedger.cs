using DevExpress.Data.Mask.Internal;
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
    public partial class FormOutLedger : BaseForm
    {
        private DataTable dt;
        private int ChoiceOutLedgerKey;
        private int ChoiceCarKey;
        private bool isEditOut;
        private int maxRow = 7;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormOutLedger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormOutLedger_Load(object sender, EventArgs e)
        {
            SetLue();
        }

        /// <summary>
        /// 폼 쇼운
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSlipOut_Shown(object sender, EventArgs e)
        {
            dteFrom.DateTime = DateTime.Now.AddDays(-7);
            dteTo.DateTime = DateTime.Now;
        }

        /// <summary>
        /// 조회일자 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkOutDate_CheckedChanged(object sender, EventArgs e)
        {
            dteFrom.Enabled = dteTo.Enabled = !chkOutDate.Checked;
        }

        /// <summary>
        /// lookupedit 세팅
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
            sql += "       ) x ";
            sql += " order by seq, productname";
            DataTable dtProduct = DBManager.Instance.GetDataTable(sql);
            rplueProduct.ValueMember = "productkey";
            rplueProduct.DisplayMember = "productname";
            rplueProduct.DataSource = dtProduct;
        }

        /// <summary>
        /// 상품/거래선/차량 수정 시 새로고침
        /// </summary>
        public override void DoRefresh()
        {
            SetLue();
        }
        #endregion

        #region 출고원장 조회
        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetOutLedger();
        }
       
        /// <summary>
        /// 출고원장 로드
        /// </summary>
        private void GetOutLedger()
        {
            string sql = string.Empty;
            sql += "select l.outledgerkey, ";
            sql += "       p.compkey, ";
            sql += "       l.outdate, ";
            sql += "       p.compname, ";
            sql += "       sum(t.outqty) as outqty, ";
            sql += "       sum(t.outprice) as outprice, ";
            sql += "       p.compregistnum, ";
            sql += "       p.compowner, ";
            sql += "       p.compaddress, ";
            sql += "       c.carkey, ";
            sql += "       c.carnum, ";
            sql += "       l.driver, ";
            sql += "       l.drivertel, ";
            sql += "       l.carbelong, ";
            sql += "       l.destination ";
            sql += "  from outledger l ";
            sql += "  join carinf c on c.carkey = l.carkey ";
            sql += "  join compinf p on p.compkey = l.compkey ";
            sql += "  join outdetail t on t.outledgerkey = l.outledgerkey ";
            sql += " where 1 = 1 ";
            sql += "   and l.isdeleted = '0' ";
            if (!chkOutDate.Checked)
            {
                sql += $" and l.outdate between '{dteFrom.DateTime.ToString("yyyy-MM-dd")}' and '{dteTo.DateTime.ToString("yyyy-MM-dd")}' ";
            }
            if (!string.IsNullOrEmpty(txtComp.Text) && txtComp.Text != "전체")
            {
                sql += $" and p.compname like '%{txtComp.Text}%' ";
            }
            if (!string.IsNullOrEmpty(txtCar.Text) && txtCar.Text != "전체")
            {
                sql += $" and c.carnum like '%{txtCar.Text}%' ";
            }
            sql += " group by l.outledgerkey, p.compkey, c.carkey ";
            sql += " order by l.outdate desc, l.outledgerkey desc ";
            DataTable dtOutLedger = DBManager.Instance.GetDataTable(sql);
            grdOut.DataSource = dtOutLedger;
            grdViewOut.OptionsView.BestFitMaxRowCount = 100;
            grdViewOut.BestFitColumns();
        }

        /// <summary>
        /// 원장 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewOut_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            grdViewOutDetail.OptionsBehavior.Editable = false;
            SetOutDetail();
        }
        private void SetOutDetail()
        {
            DataRow row = grdViewOut.GetFocusedDataRow();
            if (row == null)
                return;

            SetLayoutControl(true);
            isEditOut = false;
            ChoiceOutLedgerKey = int.Parse(row["outledgerkey"].ToString());
            dteOutDate.DateTime = DateTime.Parse(row["outdate"].ToString());
            spnTotalOutQty.EditValue = double.Parse(row["outqty"].ToString());
            spnTotalOutPrice.EditValue = double.Parse(row["outprice"].ToString());
            lueComp.EditValue = int.Parse(row["compkey"].ToString());
            lueCar.EditValue = int.Parse(row["carkey"].ToString());
            txtDriver.Text = row["driver"].ToString();
            txtDriverTel.Text = row["drivertel"].ToString();
            string CarBelong = row["carbelong"].ToString();
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
            txtDestination.Text = row["destination"].ToString();

            // OutDetail
            string sql = string.Empty;
            sql += "select d.outdetailkey, ";
            sql += "       d.outledgerkey, ";
            sql += "       d.productkey, ";
            sql += "       p.productname, ";
            sql += "       p.prodprice, ";
            sql += "       d.outqty, ";
            sql += "       d.outunitprice, ";
            sql += "       d.outprice, ";
            sql += "       d.outnote ";
            sql += "  from outdetail d ";
            sql += "  join productinf p on p.productkey = d.productkey ";
            sql += " where 1 = 1 ";
            sql += $"  and d.outledgerkey = {ChoiceOutLedgerKey} ";
            sql += $"  and d.isdeleted = '0' ";
            sql += $"order by d.outdetailkey ";
            dt = DBManager.Instance.GetDataTable(sql);
            grdOutDetail.DataSource = dt;
            grdViewOutDetail.OptionsView.BestFitMaxRowCount = 100;
            grdViewOutDetail.BestFitColumns();
        }

        /// <summary>
        /// 레이아웃컨트롤 on/off
        /// </summary>
        private void SetLayoutControl(bool ReadOnly)
        {
            lyogOutLedger.Text = ReadOnly ? "▷ 출고정보" : "▷ 출고정보[수정]";
            lyogOutDetail.Text = grdViewOutDetail.OptionsBehavior.Editable ? "≡ 출고품종[수정]" : "≡ 출고품종";
            dteOutDate.ReadOnly = ReadOnly;
            lueComp.ReadOnly = ReadOnly;
            lueCar.ReadOnly = ReadOnly;
            txtDriver.ReadOnly = ReadOnly;
            txtDriverTel.ReadOnly = ReadOnly;
            txtCarBelong.ReadOnly = ReadOnly;
            chkCarBelong.ReadOnly = ReadOnly;
            txtDestination.ReadOnly = ReadOnly;
        }
        #endregion

        #region 출고원장
        /// <summary>
        /// 소속 = 자차 or not
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkCarBelong_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCarBelong.Checked)
                txtCarBelong.Enabled = false;
            else
                txtCarBelong.Enabled = true;
        }

        /// <summary>
        /// 차량 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueCar_EditValueChanged(object sender, EventArgs e)
        {
            ChoiceCarKey = int.Parse(lueCar.EditValue.ToString());
            string sql = string.Empty;
            sql += $"select driver, drivertel, carbelong from carinf where carkey = {ChoiceCarKey} ";
            DataTable dtCar = DBManager.Instance.GetDataTable(sql);
            txtDriver.Text = dtCar.Rows[0]["driver"].ToString();
            txtDriverTel.Text = dtCar.Rows[0]["drivertel"].ToString();
            string CarBelong = dtCar.Rows[0]["carbelong"].ToString();
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
        #endregion

        #region 출고세부
        /// <summary>
        /// Grid 세팅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewOutDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
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
        private void grdViewOutDetail_RowCountChanged(object sender, EventArgs e)
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
        /// outdetail 품종 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbDelete_Click(object sender, EventArgs e)
        {
            DataRow row = grdViewOutDetail.GetFocusedDataRow();
            if (row == null)
                return;

            if (XtraMessageBox.Show("선택한 데이터를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!string.IsNullOrEmpty(row["outdetailkey"].ToString()))
            {
                int outDetailKey = int.Parse(row["outdetailkey"].ToString());
                string sql = string.Empty;
                sql += "update outdetail ";
                sql += "   set isdeleted = '1', ";
                sql += "       updatedtm = current_timestamp ";
                sql += " where 1 = 1 ";
                sql += $"  and outdetailkey = {outDetailKey} ";
                int result = DBManager.Instance.ExcuteDataUpdate(sql);

                if (result != 1)
                {
                    XtraMessageBox.Show("삭제 중 오류가 발생했습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            grdViewOutDetail.DeleteSelectedRows();
        }
        #endregion

        #region 저장
        /// <summary>
        /// 출고원장 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lyogOutLedger_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (ChoiceOutLedgerKey == 0)
            {
                XtraMessageBox.Show("출고원장 선택 후 다시 시도해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch(e.Button.Properties.Tag.ToString())
            {
                case "Edit":
                    SetLayoutControl(false);
                    isEditOut = true;
                    break;
                case "Save":
                    if (!isEditOut)
                        return;

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

                    if (XtraMessageBox.Show("출고정보를 저장하시겠습니까?", "출고정보", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    string sql = string.Empty;
                    sql += "update outledger ";
                    sql += $"  set carkey = {lueCar.EditValue}, ";
                    sql += $"      compkey = {lueComp.EditValue}, ";
                    sql += $"      outdate = '{dteOutDate.DateTime.ToString("yyyy-MM-dd")}', ";
                    sql += $"      driver = N'{txtDriver.Text}', ";
                    sql += $"      drivertel = N'{txtDriverTel.Text}', ";
                    if (chkCarBelong.Checked)
                    {
                        sql += $"      carbelong = N'자차', ";
                    }
                    else
                    {
                        sql += $"      carbelong = N'{txtCarBelong.Text}', ";
                    }
                    sql += $"      destination = N'{txtDestination.Text}', ";
                    sql += "       updatedtm = current_timestamp ";
                    sql += " where 1 = 1 ";
                    sql += $"  and outledgerkey = {ChoiceOutLedgerKey} ";

                    int result = -1;
                    result = DBManager.Instance.ExcuteDataUpdate(sql);

                    if (result == 1)
                    {
                        XtraMessageBox.Show("변경한 출고정보가 저장되었습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogicManager.Common.RefreshGridView(GetOutLedger, grdViewOut);
                        SetLayoutControl(true);
                        isEditOut = false;
                    }
                    else
                    {
                        XtraMessageBox.Show("출고정보 저장 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    break;
            }

        }

        /// <summary>
        /// 출고상품 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lyogOutDetail_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (ChoiceOutLedgerKey == 0)
            {
                XtraMessageBox.Show("출고원장 선택 후 다시 시도해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            switch (e.Button.Properties.Tag.ToString())
            {
                case "Edit":
                    grdViewOutDetail.OptionsBehavior.Editable = true;
                    lyogOutDetail.Text = "≡ 출고품종[수정]";
                    break;
                case "Save":
                    if (!grdViewOutDetail.OptionsBehavior.Editable)
                        return;

                    if (grdViewOutDetail.IsEditing)
                    {
                        grdViewOutDetail.CloseEditor();
                        grdViewOutDetail.UpdateCurrentRow();
                    }

                    if (XtraMessageBox.Show("수정하시겠습니까?", "수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        XtraMessageBox.Show("출고 품종을 입력해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

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
                                    int outdetailkey = LogicManager.Common.fnGetNextKey("lastoutdetailkey");

                                    string sql = string.Empty;
                                    sql += "insert into outdetail ";
                                    sql += "values ( ";
                                    sql += $"      {outdetailkey}, ";
                                    sql += $"      {ChoiceOutLedgerKey}, ";
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
                                    sql += "update outdetail ";
                                    sql += "   set ";
                                    if (string.IsNullOrEmpty(dr["productkey"].ToString()))
                                    {
                                        XtraMessageBox.Show("품종을 선택해주세요.", "품종 미선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        return;
                                    }
                                    sql += $"      productkey = {dr["productkey"]}, ";
                                    if (string.IsNullOrEmpty(dr["outqty"].ToString()))
                                        sql += "   outqty = 0, ";
                                    else
                                        sql += $"  outqty = {dr["outqty"]}, ";
                                    if (string.IsNullOrEmpty(dr["outunitprice"].ToString()))
                                        sql += "   outunitprice = 0, ";
                                    else
                                        sql += $"  outunitprice = {dr["outunitprice"]}, ";
                                    if (string.IsNullOrEmpty(dr["outprice"].ToString()))
                                        sql += "   outprice = 0, ";
                                    else
                                        sql += $"  outprice = {dr["outprice"]}, ";
                                    sql += $"      outnote = N'{dr["outnote"]}', ";
                                    sql += $"      updatedtm = current_timestamp ";
                                    sql += " where 1 = 1 ";
                                    sql += $"  and outdetailkey = {dr["outdetailkey"]} ";
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
                            XtraMessageBox.Show("변경한 출고상품이 저장되었습니다.", "저장완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogicManager.Common.RefreshGridView(GetOutLedger, grdViewOut);
                            grdViewOutDetail.OptionsBehavior.Editable = false;
                            lyogOutDetail.Text = "≡ 출고품종";
                        }
                        else
                        {
                            XtraMessageBox.Show("출고상품 저장 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    break;
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
        /// 출고원장 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("선택하신 출고원장을 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            List<string> sqls = new List<string>();
            // 원장 삭제
            string sql = string.Empty;
            sql += "update outledger ";
            sql += "   set isdeleted = '1', ";
            sql += "       updatedtm = current_timestamp ";
            sql += $"where outledgerkey = {ChoiceOutLedgerKey} ";
            sqls.Add(sql);

            sql = string.Empty;
            sql += "update outdetail ";
            sql += "   set isdeleted = '1', ";
            sql += "       updatedtm = current_timestamp ";
            sql += $"where outledgerkey = {ChoiceOutLedgerKey} ";
            sqls.Add(sql);

            string result = DBManager.Instance.ExcuteTransaction(sqls);

            if (string.IsNullOrEmpty(result))
            {
                XtraMessageBox.Show("출고 원장이 성공적으로 삭제되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LogicManager.Common.RefreshGridView(GetOutLedger, grdViewOut);
            }
            else
            {
                XtraMessageBox.Show("삭제 중 오류가 발생하였습니다.", "삭제 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 송장 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataRow row = grdViewOut.GetFocusedDataRow();
            if (row == null || dt == null || dt.Rows.Count == 0)
            {
                XtraMessageBox.Show("출고 선택 후 다시 시도해주세요.", "송장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataTable sdt1 = dt.Copy();
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