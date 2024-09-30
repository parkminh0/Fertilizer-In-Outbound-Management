using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Import.Html;
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
    public partial class FormMakePay : DevExpress.XtraEditors.XtraForm
    {
        private int ChoiceCompKey = 0;
        private DataTable dtPay;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormMakePay(int ChoiceCompKey)
        {
            InitializeComponent();
            this.ChoiceCompKey = ChoiceCompKey;
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMakePay_Load(object sender, EventArgs e)
        {
            SetLue();
        }

        /// <summary>
        /// comp lookupedit 세팅
        /// </summary>
        private void SetLue()
        {
            string sql = string.Empty;
            sql += "select compkey, compcode, compname from compinf where isdeleted = '0' order by compname ";
            DataTable dtComp = DBManager.Instance.GetDataTable(sql);
            lueComp.Properties.ValueMember = "compkey";
            lueComp.Properties.DisplayMember = "compname";
            lueComp.Properties.DataSource = dtComp;
        }

        /// <summary>
        /// 폼 쇼운
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMakePay_Shown(object sender, EventArgs e)
        {
            if (ChoiceCompKey != 0)
                lueComp.EditValue = ChoiceCompKey;
        }

        /// <summary>
        /// 텍스트 셋팅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lueComp_EditValueChanged(object sender, EventArgs e)
        {
            lygPayAccount.Text = "≡ 입금내역";
            ChoiceCompKey = int.Parse(lueComp.EditValue.ToString());
            string sql = string.Empty;
            sql += $"select compowner, compregistnum, comptel, compaddress, compnote from compinf where compkey = {ChoiceCompKey} ";
            DataTable dt = DBManager.Instance.GetDataTable(sql);
            txtCompRegistNum.Text = dt.Rows[0]["compregistnum"].ToString();
            txtCompOwner.Text = dt.Rows[0]["compowner"].ToString();
            txtCompTel.Text = dt.Rows[0]["comptel"].ToString();
            txtCompAddress.Text = dt.Rows[0]["compaddress"].ToString();
            txtCompNote.Text = dt.Rows[0]["compnote"].ToString();

            SetPayAccount();
        }

        /// <summary>
        /// 입금내역 나타내기
        /// </summary>
        private void SetPayAccount()
        {
            string sql = string.Empty;
            sql += $"select * from payaccount where isdeleted = '0' and compkey = {ChoiceCompKey} order by paydate desc, paykey desc ";
            dtPay = DBManager.Instance.GetDataTable(sql);
            grdPay.DataSource = dtPay;
            grdViewPay.OptionsView.BestFitMaxRowCount = 100;
            grdViewPay.BestFitColumns();
        }

        /// <summary>
        /// 팝업 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 입금 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (grdViewPay.IsEditing)
            {
                grdViewPay.CloseEditor();
                grdViewPay.UpdateCurrentRow();
            }
            
            if (ChoiceCompKey == 0 || string.IsNullOrEmpty(lueComp.Text))
            {
                XtraMessageBox.Show("거래선을 선택해주세요.", "입금", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (XtraMessageBox.Show("입금 내역을 저장하시겠습니까?", "저장", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            try
            {
                List<string> sqls = new List<string>();
                //추가
                DataTable dtAdded = dtPay.GetChanges(DataRowState.Added);
                if (dtAdded != null)
                {
                    try
                    {
                        foreach (DataRow dr in dtAdded.Rows)
                        {
                            int payKey = LogicManager.Common.fnGetNextKey("lastpayaccountkey");
                            string sql = string.Empty;
                            sql += "insert into payaccount ";
                            sql += "values ( ";
                            sql += $" {payKey}, ";
                            sql += $" {ChoiceCompKey}, ";
                            if (string.IsNullOrEmpty(dr["paydate"].ToString()))
                            {
                                XtraMessageBox.Show("입금일자를 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            sql += $"      '{DateTime.Parse(dr["paydate"].ToString()).ToString("yyyy-MM-dd")}', ";
                            if (string.IsNullOrEmpty(dr["paycategory"].ToString()))
                            {
                                XtraMessageBox.Show("구분을 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            sql += $" N'{dr["paycategory"]}', ";
                            sql += $" {dr["payprice"]}, ";
                            sql += $" N'{dr["paynote"]}', ";
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
                DataTable dtChanged = dtPay.GetChanges(DataRowState.Modified);
                if (dtChanged != null)
                {
                    try
                    {
                        foreach (DataRow dr in dtChanged.Rows)
                        {
                            string sql = string.Empty;
                            sql += "update payaccount ";
                            sql += "   set ";
                            if (string.IsNullOrEmpty(dr["paydate"].ToString()))
                            {
                                XtraMessageBox.Show("입금 일자를 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            sql += $"      paydate = '{DateTime.Parse(dr["paydate"].ToString()).ToString("yyyy-MM-dd")}', ";
                            if (string.IsNullOrEmpty(dr["paycategory"].ToString()))
                            {
                                XtraMessageBox.Show("구분을 선택해주세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            sql += $"      paycategory = N'{dr["paycategory"]}', ";
                            sql += $"      payprice = {dr["payprice"]}, ";
                            sql += $"      paynote = N'{dr["paynote"]}', ";
                            sql += $"      updatedtm = current_timestamp ";
                            sql += " where 1 = 1 ";
                            sql += $"  and paykey = {dr["paykey"]} ";
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
                    LogicManager.Common.RefreshGridView(SetPayAccount, grdViewPay);
                }
                else
                {
                    XtraMessageBox.Show("입금내역 저장 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpbtnDelete_Click(object sender, EventArgs e)
        {
            DataRow row = grdViewPay.GetFocusedDataRow();
            if (row == null)
                return;

            if (XtraMessageBox.Show("선택한 입금 내역을 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!string.IsNullOrEmpty(row["paykey"].ToString()))
            {
                int payKey = int.Parse(row["paykey"].ToString());
                string sql = string.Empty;
                sql += "update payaccount ";
                sql += "   set isdeleted = '1', ";
                sql += "       updatedtm = current_timestamp ";
                sql += " where 1 = 1 ";
                sql += $"  and paykey = {payKey} ";
                int result = DBManager.Instance.ExcuteDataUpdate(sql);

                if (result == 1)
                {
                    XtraMessageBox.Show("삭제가 완료되었습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show("삭제 중 오류가 발생했습니다.", "삭제", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            grdViewPay.DeleteSelectedRows();
        }
        #endregion
    }
}