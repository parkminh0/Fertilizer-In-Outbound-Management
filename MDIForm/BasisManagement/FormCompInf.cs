using DevExpress.CodeParser;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HanIlCNS
{
    public partial class FormCompInf : DevExpress.XtraEditors.XtraForm
    {
        private int ChoiceCompKey;
        private int EditMode = 1;
        private CompInfIO compInfIO;

        #region 폼 로드
        /// <summary>
        /// 
        /// </summary>
        public FormCompInf()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 로드
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormCompInf_Load(object sender, EventArgs e)
        {
        }
        #endregion

        #region 조회
        /// <summary>
        /// 거래선 데이터 가져오기
        /// </summary>
        private void GetData(string CompName, string Address)
        {
            DataTable dt = CompInfIO.Select(CompName, Address);
            grdComp.DataSource = dt;
            grdViewComp.OptionsView.BestFitMaxRowCount = 100;
            grdViewComp.BestFitColumns();
        }

        /// <summary>
        /// 조회
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData(txtSearch1.Text, txtSearch2.Text);
        }

        /// <summary>
        /// 선택 시 텍스트 채우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewComp_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            GetInfo();
        }
        private void GetInfo()
        {
            DataRow row = grdViewComp.GetFocusedDataRow();
            if (row == null)
                return;

            EditMode = 1;
            layoutControlGroup2.Text = "▷ 거래선 정보";
            ChoiceCompKey = int.Parse(row["compkey"].ToString());
            txtCompCode.Text = row["compcode"].ToString();
            txtCompName.Text = row["compname"].ToString();
            txtCompOwner.Text = row["compowner"].ToString();
            txtCompRegistNum.Text = row["compregistnum"].ToString();
            txtCompCategory.Text = row["compcategory"].ToString();
            txtCompCondition.Text = row["compcondition"].ToString();
            txtCompTel.Text = row["comptel"].ToString();
            txtCompFax.Text = row["compfax"].ToString();
            txtCompPost.Text = row["comppost"].ToString();
            txtCompAddress.Text = row["compaddress"].ToString();
            txtCompNote.Text = row["compnote"].ToString();
            SetLayoutControl(true);
        }
        #endregion

        #region CRUD
        /// <summary>
        /// 레이아웃 컨트롤 readonly on/off
        /// </summary>
        private void SetLayoutControl(bool readOnly)
        {
            txtCompCode.ReadOnly = readOnly;
            txtCompName.ReadOnly = readOnly;
            txtCompOwner.ReadOnly = readOnly;
            txtCompRegistNum.ReadOnly = readOnly;
            txtCompCategory.ReadOnly = readOnly;
            txtCompCondition.ReadOnly = readOnly;
            txtCompTel.ReadOnly = readOnly;
            txtCompFax.ReadOnly = readOnly;
            txtCompPost.ReadOnly = readOnly;
            txtCompAddress.ReadOnly = readOnly;
            txtCompNote.ReadOnly = readOnly;
        }

        /// <summary>
        /// 추가/수정/삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void layoutControlGroup2_CustomButtonClick(object sender, DevExpress.XtraBars.Docking2010.BaseButtonEventArgs e)
        {
            if (e.Button.Properties.Tag.ToString() == "Delete")
                DeleteComp();
            else
                EditComp(e.Button.Properties.Tag.ToString());
        }

        /// <summary>
        /// 거래선 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteComp()
        {
            if (ChoiceCompKey == 0)
            {
                XtraMessageBox.Show("삭제할 데이터를 선택하세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int result = -1;
            if (XtraMessageBox.Show("선택한 데이터를 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                result = CompInfIO.Delete(ChoiceCompKey);
            else
                return;

            if (result == 1)
            {
                XtraMessageBox.Show("데이터가 삭제되었습니다.", "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSearch.PerformClick();
            }
            else
            {
                XtraMessageBox.Show("삭제 중 오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 추가/수정
        /// </summary>
        /// <param name="mode"></param>
        private void EditComp(string mode)
        {
            if (mode == "New") // 신규
            {
                SetLayoutControl(false);
                ChoiceCompKey = 0;
                EditMode = 0;
                txtCompCode.Text = "";
                txtCompName.Text = "";
                txtCompOwner.Text = "";
                txtCompRegistNum.Text = "";
                txtCompCategory.Text = "";
                txtCompCondition.Text = "";
                txtCompTel.Text = "";
                txtCompFax.Text = "";
                txtCompPost.Text = "";
                txtCompAddress.Text = "";
                txtCompNote.Text = "";
                layoutControlGroup2.Text = "※ 거래선 신규 등록 ※";
            }
            else if (mode == "Edit")
            {
                if (ChoiceCompKey == 0)
                {
                    XtraMessageBox.Show("수정할 데이터를 선택하세요.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                EditMode = 1;
                layoutControlGroup2.Text = "※ 거래선 정보 수정 ※";
                SetLayoutControl(false);
            }
            else // 저장
            {
                compInfIO = new CompInfIO(ChoiceCompKey);
                int result = -1;
                SetInfo();

                if (EditMode == 0)
                {
                    if (XtraMessageBox.Show("거래선을 추가하시겠습니까?", "추가", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        result = compInfIO.Insert();
                    }
                    else
                        return;
                }
                else
                {
                    if (XtraMessageBox.Show("변경 내용을 저장하시겠습니까?", "수정", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        result = compInfIO.Update();
                    }
                    else
                        return;
                }

                if (result == 1)
                {
                    switch (EditMode)
                    {
                        case 0:
                            XtraMessageBox.Show("추가되었습니다.", "추가 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                        case 1:
                            XtraMessageBox.Show("수정되었습니다.", "수정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;
                    }

                    EditMode = 1;
                    layoutControlGroup2.Text = "▷ 거래선 정보";
                    int ChoiceRowNum = grdViewComp.FocusedRowHandle;
                    btnSearch.PerformClick();
                    if (EditMode == 1)
                        grdViewComp.FocusedRowHandle = ChoiceRowNum;
                    SetLayoutControl(true);
                }
                else
                {
                    XtraMessageBox.Show("오류가 발생하였습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        /// 데이터 저장
        /// </summary>
        private void SetInfo()
        {
            compInfIO.CompCode = txtCompCode.Text.Trim();
            compInfIO.CompName = txtCompName.Text.Trim();
            compInfIO.CompOwner = txtCompOwner.Text.Trim();
            compInfIO.CompRegistNum = txtCompRegistNum.Text.Trim();
            compInfIO.CompCategory = txtCompCategory.Text.Trim();
            compInfIO.CompCondition = txtCompCondition.Text.Trim();
            compInfIO.CompTel = txtCompTel.Text.Trim();
            compInfIO.CompFax = txtCompFax.Text.Trim();
            compInfIO.CompPost = txtCompPost.Text.Trim();
            compInfIO.CompAddress = txtCompAddress.Text.Trim();
            compInfIO.CompNote = txtCompNote.Text.Trim();
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
        /// 엑셀 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExcel_Click(object sender, EventArgs e)
        {
            targetFileName = GetFileName("xlsx", string.Format("거래선 목록"));
            if (targetFileName.Trim() != "")
                grdComp.ExportToXlsx(targetFileName, new XlsxExportOptionsEx { ExportType = ExportType.WYSIWYG });
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
        /// 입금관리 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grdViewComp_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            DataRow row = grdViewComp.GetFocusedDataRow();
            if (row == null)
                return;

            if (e.Clicks == 2)
            {
                int compKey = int.Parse(row["compkey"].ToString());
                FormMakePay frm = new FormMakePay(compKey);
                frm.ShowDialog();
            }
        }
    }
}