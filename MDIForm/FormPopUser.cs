using DevExpress.XtraEditors;
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
    public partial class FormPopUser : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 편집구분 --> 1:신규 2:수정 3:삭제
        /// </summary>
        public int EditMode;
        public string ChoiceUserID;

        public FormPopUser()
        {
            InitializeComponent();
        }


        private void FormPopUser_Load(object sender, EventArgs e)
        {

        }

        private void FormPopUser_Shown(object sender, EventArgs e)
        {
            switch (EditMode)
            {
                case 1:
                    this.Text = "사용자 신규";
                    btnApply.Text = "신    규";
                    txtUserID.Enabled = true;
                    txtUserID.Focus();
                    break;
                case 2:
                    this.Text = "사용자 수정";
                    btnApply.Text = "수    정";
                    txtUserID.Enabled = false;
                    txtUserName.Focus();
                    GetInfo();
                    break;
                case 3:
                    this.Text = "사용자 삭제";
                    btnApply.Text = "삭    제";
                    txtUserID.Enabled = false;
                    txtPassword.Enabled = false;
                    txtUserName.Enabled = false;
                    txtNote.Enabled = false;
                    btnApply.Focus();
                    GetInfo();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 기존 정보 읽어오기
        /// </summary>
        private void GetInfo()
        {
            string sql = string.Empty;
            sql += "select UserID ";
            sql += "	  ,UserName ";
            sql += "	  ,UserPwd ";
            sql += "	  ,UserNote ";
            sql += "  from UserInf ";
            sql += $" where UserID = '{ChoiceUserID}' ";

            DataTable dtData;
            try
            {
                dtData = DBManager.Instance.GetDataTable(sql);
                txtUserID.Text = dtData.Rows[0]["UserID"].ToString();
                txtUserName.Text = dtData.Rows[0]["UserName"].ToString();
                txtPassword.Text = dtData.Rows[0]["UserPwd"].ToString();
                txtNote.Text = dtData.Rows[0]["UserNote"].ToString();
            }
            catch (Exception)
            {
                dtData = null;
            }
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserID.Text) && EditMode != 3)
            {
                XtraMessageBox.Show("UserID를 입력해 주세요.", "User ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserID.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUserName.Text) && EditMode != 3)
            {
                XtraMessageBox.Show("사용자명을 입력해 주세요.", "User Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserName.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPassword.Text) && EditMode != 3)
            {
                XtraMessageBox.Show("사용자 암호를 입력해 주세요.", "암호", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Focus();
                return;
            }
            if (txtUserID.Text.Trim().ToLower() == "admin" && EditMode == 3)
            {
                XtraMessageBox.Show("관리자 계정은 삭제하실 수 없습니다!", "삭제불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserID.Focus();
                return;
            }

            ChoiceUserID = txtUserID.Text.Trim();
            List<string> sqls = new List<string>();
            string sql = string.Empty;

            switch (EditMode)
            {
                case 1: //신규
                    if (isExists()) //중복검사
                        return;

                    sql = string.Empty;
                    sql += "INSERT INTO UserInf ";
                    sql += "           (UserID ";
                    sql += "           ,UserName ";
                    sql += "           ,UserPwd ";
                    sql += "           ,UserNote ";
                    sql += "           ,isDeleted ";
                    sql += "           ,CreateDtm) ";
                    sql += "     VALUES ";
                    sql += $"           ('{ChoiceUserID}' ";
                    sql += $"           ,'{txtUserName.Text}' ";
                    sql += $"           ,'{txtPassword.Text.Trim()}' ";
                    sql += $"           ,'{txtNote.Text}' ";
                    sql += "           ,'0' ";
                    sql += "           ,now())";
                    break;
                case 2: //변경
                    sql = string.Empty;
                    sql += "UPDATE UserInf ";
                    sql += $"   SET UserName = '{txtUserName.Text}' ";
                    sql += $"      ,UserPwd = '{txtPassword.Text}' ";
                    sql += $"      ,UserNote = '{txtNote.Text}' ";
                    sql += $"      ,CreateDtm = now() ";
                    sql += $" WHERE UserID = '{ChoiceUserID}' ";
                    break;
                case 3: //삭제
                    if (ChoiceUserID.ToLower() == "admin")
                    {
                        XtraMessageBox.Show("admin계정은 삭제하실 수 없습니다!", "삭제불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (XtraMessageBox.Show($"선택하신 {ChoiceUserID} 사용자를 삭제합니다. 정말로 삭제하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                        return;
                    sql = string.Empty;
                    sql += $"Update UserInf Set isDeleted = '1' WHERE UserID = '{ChoiceUserID}' ";
                    break;
                default:
                    break;
            }
            sqls.Add(sql);

            string result = DBManager.Instance.ExcuteTransaction(sqls);
            if (string.IsNullOrWhiteSpace(result))
            {
                XtraMessageBox.Show("정상적으로 적용되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                XtraMessageBox.Show("오류가 발생하여 DB에 반영하지 못했습니다.\r\n" + result, "실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        /// <summary>
        /// 신규시 ID중복 검사
        /// </summary>
        /// <returns></returns>
        private bool isExists()
        {
            string sql = $"select count(*) from UserInf where UserID = '{ChoiceUserID}' ";
            try
            {
                if (DBManager.Instance.GetIntScalar(sql) > 0)
                {
                    XtraMessageBox.Show("이미 존재하는 User ID입니다. 새로운 ID를 입력해 주세요.", "ID중복", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("입력하신 ID를 검사할 수 없는 상태입니다. 서버 상태확인 후 다시 시도해 주세요.\r\n" + e.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

    }
}