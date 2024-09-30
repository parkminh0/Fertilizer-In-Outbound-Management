using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
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
    public enum SetPage
    {
        SetServer,
        Users
    }

    public partial class FormPopUserInf : DevExpress.XtraEditors.XtraForm
    {
        private string ChoiceUserID;

        /// <summary>
        /// 
        /// </summary>
        public FormPopUserInf()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSettings_Load(object sender, EventArgs e)
        {
            ShowUsersPage();
        }

        private void FormSettings_Shown(object sender, EventArgs e)
        {
      
        }

        /// <summary>
        /// 사용자 관리
        /// </summary>
        private void ShowUsersPage()
        {
            GetDataList();
            GetFocusedRow();
            btnNew.Focus();
        }

        /// <summary>
        /// 사용자 신규
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            FormPopUser frm = new FormPopUser();
            frm.EditMode = 1;
            frm.ChoiceUserID = string.Empty;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                ChoiceUserID = frm.ChoiceUserID;
                GetDataList();
            }
        }

        /// <summary>
        /// 사용자 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            FormPopUser frm = new FormPopUser();
            frm.EditMode = 2;
            frm.ChoiceUserID = ChoiceUserID;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetDataList();
            }
        }

        /// <summary>
        /// 사용자 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            FormPopUser frm = new FormPopUser();
            frm.EditMode = 3;
            frm.ChoiceUserID = ChoiceUserID;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                GetDataList();
            }
        }

        /// <summary>
        /// 사용자목록 클릭시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetFocusedRow();
        }

        /// <summary>
        /// 그리드 선택시
        /// </summary>
        private void GetFocusedRow()
        {
            DataRow row = grdViewUserInf.GetFocusedDataRow();
            try
            {
                ChoiceUserID = row["UserID"].ToString();
            }
            catch (Exception)
            {
                ChoiceUserID = string.Empty;
                return;
            }
        }

        /// <summary>
        /// 더블클릭시 편집모드 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdUserInf_DoubleClick(object sender, EventArgs e)
        {
            GetFocusedRow();
            btnModify.PerformClick();
        }

        /// <summary>
        /// 기존 목록 불러오기
        /// </summary>
        private void GetDataList()
        {
            string sql = string.Empty;
            sql += "select userid ";
            sql += "	  ,userpwd ";
            sql += "	  ,username ";
            sql += "	  ,usernote ";
            sql += "  from UserInf ";
            sql += " where isDeleted = '0' ";
            try
            {
                DataTable dtDataList = DBManager.Instance.GetDataTable(sql);
                grdUserInf.DataSource = dtDataList;
            }
            catch (Exception)
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}