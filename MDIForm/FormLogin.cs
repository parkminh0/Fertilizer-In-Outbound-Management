using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HanIlCNS
{
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 비밀번호 변경인지 여부
        /// </summary>
        public bool isChangePassword;

        /// <summary>
        /// 
        /// </summary>
        public FormLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLogin_Load(object sender, EventArgs e)
        {
            Program.GRunYN = false;
        }
        private void FormLogin_Shown(object sender, EventArgs e)
        {
            chkSaveID.Checked = Properties.Settings.Default.isSave;
            if (chkSaveID.Checked)
                txtUserID.Text = Properties.Settings.Default.UserID;
            else
                txtUserID.Text = string.Empty;
            txtServerIP.Text = Properties.Settings.Default.ServerIP;

            if (isChangePassword)   //비밀번호 변경
            {
                txtServerIP.Enabled = false;
                txtUserID.Enabled = false;
                lblPasswd1.Visible = true;
                lblPasswd2.Visible = true;
                txtPasswd1.Visible = true;
                txtPasswd2.Visible = true;
                chkSaveID.Enabled = false;
                txtPassword.Focus();
            }
            else
            {
                txtServerIP.Enabled = true;
                txtUserID.Enabled = true;
                lblPasswd1.Visible = false;
                lblPasswd2.Visible = false;
                txtPasswd1.Visible = false;
                txtPasswd2.Visible = false;
                chkSaveID.Enabled = true;

                if (string.IsNullOrEmpty(txtUserID.Text))
                    txtUserID.Focus();
                else
                    txtPassword.Focus();
            }
        }

        /// <summary>
        /// DB Connect
        /// </summary>
        /// <returns></returns>
        private void DBConnect()
        {
            try
            {
                DataTable dt = DBManager.Instance.GetDataTable(Program.constance.DBTestQuery);  //DB Connection
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("[오류] DB Connection에 문제가 있습니다.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// 닫기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Program.GRunYN = false;
            this.Close();
        }

        /// <summary>
        /// 로그인: fincsuser:Xsl/zdnpCnqbO2cmCUZClQ== / fincsadmin:n0qPlANwaMB5DBIBbF8MGg==
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (isChangePassword)
                ChangePasswordProcess();
            else
                LoginProcess();
        }

        /// <summary>
        /// 로그인 로직
        /// </summary>
        private void LoginProcess()
        {
            Program.constance.ServerIP = txtServerIP.Text.Trim();

            if (string.IsNullOrEmpty(txtUserID.Text))
            {
                XtraMessageBox.Show("사용자ID를 입력해 주세요.", "사용자ID", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!CheckLoginID())
            {
                XtraMessageBox.Show("등록된 사용자가 아닙니다. 사용자 ID를 확인해주세요.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserID.Text = string.Empty;
                txtUserID.Focus();
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                XtraMessageBox.Show("현재 암호를 입력해 주세요.", "현재암호", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //if (AES.AESDecrypt256(usr, Program.constance.compName) != txtPassword.Text)
            string usr = GetPassword();
            if (usr.Trim() != txtPassword.Text)
            {
                XtraMessageBox.Show("암호가 다릅니다. 정확한 암호를 다시 입력해 주세요.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
                return;
            }

            GetAuthority();
            Program.GRunYN = true;
            Properties.Settings.Default.UserID = txtUserID.Text.Trim();
            Properties.Settings.Default.isSave = chkSaveID.Checked;
            Properties.Settings.Default.ServerIP = txtServerIP.Text.Trim();
            Properties.Settings.Default.Save();
            this.Close();
        }

        /// <summary>
        /// 암호변경 로직
        /// </summary>
        private void ChangePasswordProcess()
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                XtraMessageBox.Show("현재 암호를 입력해 주세요.", "현재암호", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrEmpty(txtPasswd1.Text.Trim()) || txtPassword.Text.Trim() == txtPasswd1.Text.Trim())
            {
                XtraMessageBox.Show("변경할 암호를 입력해 주세요.", "변경암호", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPasswd1.Focus();
                return;
            }

            if (txtPasswd1.Text.Trim() != txtPasswd2.Text.Trim())
            {
                XtraMessageBox.Show("변경할 암호화 암호확인 암호가 다릅니다. 다시 입력해 주세요.", "암호확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPasswd2.Focus();
                return;
            }

            string usr = GetPassword();
            if (usr.Trim() != txtPassword.Text)
            {
                XtraMessageBox.Show("현재 암호가 다릅니다. 정확한 암호를 다시 입력해 주세요.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPassword.Text = string.Empty;
                txtPassword.Focus();
                return;
            }

            //암호 Update
            if (ChangePasswd())
            {
                XtraMessageBox.Show("[성공] 암호가 정상적으로 변경되었습니다.", "성공", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                XtraMessageBox.Show("[실패] 오류가 발생하여 암호를 변경하지 못했습니다.", "실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 암호변경
        /// </summary>
        private bool ChangePasswd()
        {
            string sql = $"update userinf set userpwd = '{txtPasswd1.Text.Trim()}' where UserID = '{txtUserID.Text}' ";
            try
            {
                int result = DBManager.Instance.ExcuteDataUpdate(sql);
                if (result < 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("암호변경시 예기치못한 오류가 발생되었습니다.\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 사용자 ID 존재 유무 체크하기 
        /// </summary>
        /// <returns></returns>
        private bool CheckLoginID()
        {
            string sql = $"select count(*) from userinf where UserID = '{txtUserID.Text}' and isDeleted = '0' ";

            try
            {
                //if (DBManager.Instance.GetIntScalar(sql) > 0)
                DataTable dt = DBManager.Instance.GetDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// DB에서 암호 가져오기
        /// </summary>
        /// <returns></returns>
        private string GetPassword()
        {
            string sql = $"select max(userpwd) from (select userpwd from userinf where UserID = '{txtUserID.Text}' union ALL select '') x ";
            string passwd = string.Empty;
            try
            {
                passwd = DBManager.Instance.GetStringScalar(sql);
                //AES.AESDecrypt256(passwd, Program.constance.compName);
            }
            catch (Exception)
            {
                passwd = "!@#!@%^&*!@#@#@";
            }

            return passwd;
        }

        /// <summary>
        /// 엔터키 누를시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (!txtPasswd1.Visible)
                    btnLogIn.PerformClick();
            }
        }

        /// <summary>
        /// 사용자 권한
        /// </summary>
        private void GetAuthority()
        {
            string sql = $"select UserName from userinf where UserID = '{txtUserID.Text.Trim()}' ";
            DataTable dtData;
            try
            {
                dtData = DBManager.Instance.GetDataTable(sql);
                Program.Option.LoginID = txtUserID.Text.Trim();
                Program.Option.UserName = dtData.Rows[0]["UserName"].ToString();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}