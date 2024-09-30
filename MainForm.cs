using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Ribbon;
using Excel.Core;
using DevExpress.XtraExport.Helpers;
using static DevExpress.Data.Helpers.SyncHelper.ZombieContextsDetector;

namespace HanIlCNS
{
    public partial class MainForm : RibbonForm
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.UserSkin != "")
                defaultLookAndFeel1.LookAndFeel.SkinName = Properties.Settings.Default.UserSkin;
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            SkinHelper.InitSkinGallery(rbbGallery, true);
            Application.DoEvents();
            SplashScreenManager.CloseForm(false);
        }

        /// <summary>
        /// 시작준비
        ///  1. DB Connection
        ///  2. Ready Screen
        /// </summary>
        public string Splash()
        {
            Text = Program.constance.SystemTitle;
            string InitLoadComplete = string.Empty;
            SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, 10);

            if (Program.constance.DBConnectInSplash) // 스플래쉬에서 DB 커넥션
            {
                string networkMsg = string.Empty;
                //string networkMsg = NetUtil.Connect(JKBConstance.ServerIP);
                if (string.IsNullOrEmpty(networkMsg))
                {
                    SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetStatus, "Database Connection...");
                    DataTable dt = DBManager.Instance.GetDataTable(Program.constance.DBTestQuery);  //DB Connection
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, 30);
                        SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetStatus, "Ready to database and user screen...");
                        SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, 40);
                    }
                    else
                    {
                        InitLoadComplete = "DataBase 와의 연결에 문제가 있습니다.\r\n" + DBManager.DBMErrString;
                        return InitLoadComplete;
                    }
                }
                else
                {
                    InitLoadComplete = networkMsg;
                    return InitLoadComplete;
                }

                SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetStatus, "Loading Common data...");
                SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, 100);
                //try
                //{
                //    CommonData.GetCommonDataTable();
                //}
                //catch (Exception ex)
                //{
                //    InitLoadComplete = "공통 데이터 로딩시 오류발생!!! " + Environment.NewLine + ex.Message;
                //}
            }

            return InitLoadComplete;
        }

        /// <summary>
        /// 창이 열려있는지 검사
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public bool isNewForm(string formName)
        {
            //foreach (Form theForm in this.MdiChildren)    // 현재 MainForm의 MdiChildren에 해당하는 폼만 체크
            foreach (Form theForm in Application.OpenForms) // 현재 프로그램에 있는 모든 열려있는 폼 체크
            {
                if (formName == theForm.Name)
                {
                    theForm.BringToFront();
                    theForm.Focus();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 폼 열기 버튼 클릭 시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenForm_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(this, typeof(WaitForm1), true, true, false);
            BarButtonItem bbtnitem = e.Item as BarButtonItem;
            //SplashScreenManager.Default.SetWaitFormDescription(bbtnitem.Name + "Ready to screen ...");
            SplashScreenManager.Default.SetWaitFormDescription(bbtnitem.Caption + "화면 준비 중 ..."); // 2018.07.20 Hwang.W.S : Caption으로 WaitForm을 띄우도록 변경

            if (isNewForm(bbtnitem.Description))
            {
                switch (bbtnitem.Description)
                {
                    case "FormCompInf":
                        new FormCompInf() { MdiParent = this }.Show();
                        break;
                    case "FormInspectAccount":
                        new FormInspectAccount() { MdiParent = this }.Show();
                        break;
                    case "FormOutAccount":
                        new FormOutAccount() { MdiParent = this }.Show();
                        break;
                    case "FormOutLedger":
                        new FormOutLedger() { MdiParent = this }.Show();
                        break;
                    case "FormPayAccount":
                        new FormPayAccount() { MdiParent = this }.Show();
                        break;
                    case "FormMakeInspect":
                        new FormMakeInspect() { MdiParent = this }.Show();
                        break;
                    case "FormPivot":
                        new FormPivot() { MdiParent = this }.Show();
                        break;
                    case "FormPayPivot":
                        new FormPayPivot() { MdiParent = this }.Show();
                        break;
                    default:
                        break;
                }
            }
            SplashScreenManager.CloseForm(false);
        }

        /// <summary>
        /// 프로그램 종료시
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (XtraMessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != System.Windows.Forms.DialogResult.Yes)
            {
                e.Cancel = true;
            }
            else
            {
                Program.SaveConfig();
            }
        }

        /// <summary>
        /// 프로그램 종료
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bbtnExit_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 스킨저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbbGallery_Gallery_ItemClick(object sender, DevExpress.XtraBars.Ribbon.GalleryItemClickEventArgs e)
        {
            string skinName = e.Item.Tag.ToString();

            if (skinName == "")
                return;

            try
            {
                Properties.Settings.Default.UserSkin = skinName;
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 스킨 Palette 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void skinPaletteRibbonGalleryBarItem1_GalleryItemClick(object sender, GalleryItemClickEventArgs e)
        {
            try
            {
                Properties.Settings.Default.UserPalette = e.Item.Value.ToString();
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 농협 관리 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormPopNHInf frmNH = new FormPopNHInf();
            if (frmNH.ShowDialog() == DialogResult.OK)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    BaseForm baseFrm = frm as BaseForm;
                    if (baseFrm != null)
                    {
                        if (baseFrm.Text == "검수 관리")
                        {
                            baseFrm.DoRefresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 차량 관리 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormPopCarInf frmCar = new FormPopCarInf();
            if (frmCar.ShowDialog() == DialogResult.OK)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    BaseForm baseFrm = frm as BaseForm;
                    if (baseFrm != null)
                    {
                        if (baseFrm.Text == "출고 관리")
                        {
                            baseFrm.DoRefresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 상품 관리 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormPopProductInf frmComp = new FormPopProductInf();
            if (frmComp.ShowDialog() == DialogResult.OK)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    BaseForm baseFrm = frm as BaseForm;
                    if (baseFrm != null)
                    {
                        if (baseFrm.Text == "출고 관리")
                        {
                            baseFrm.DoRefresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 출고 입력 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormPopOut frmOut = new FormPopOut();
            if (frmOut.ShowDialog() == DialogResult.OK)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    BaseForm baseFrm = frm as BaseForm;
                    if (baseFrm != null)
                    {
                        if (baseFrm.Text == "출고 관리" || baseFrm.Text == "출고 내역")
                        {
                            baseFrm.DoRefresh();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 입금 관리 팝업
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormMakePay frmPay = new FormMakePay(0);
            if (frmPay.ShowDialog() == DialogResult.OK)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    BaseForm baseFrm = frm as BaseForm;
                    if (baseFrm != null)
                    {
                        if (baseFrm.Text == "입금 내역")
                        {
                            baseFrm.DoRefresh();
                        }
                    }
                }
            }
        }

        private void bbtnChangePasswd_ItemClick(object sender, ItemClickEventArgs e)
        {
            FormLogin frm = new FormLogin();
            frm.isChangePassword = true;
            frm.ShowDialog();
        }

        private void bbtnUserInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            if(Program.Option.LoginID.ToLower() != "admin")
            {
                XtraMessageBox.Show("해당 화면은 관리자만 접근하실 수 있습니다!", "권한없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormPopUserInf frm = new FormPopUserInf();
            frm.ShowDialog();
        }

        private void bbtnSetServer_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

    }
}
