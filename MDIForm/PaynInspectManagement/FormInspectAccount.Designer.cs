namespace HanIlCNS
{
    partial class FormInspectAccount
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInspectAccount));
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.grdInspectAccount = new DevExpress.XtraGrid.GridControl();
            this.grdViewInspectAccount = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.inspectdate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outunitprice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outqty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.outprice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.inspectqty = new DevExpress.XtraGrid.Columns.GridColumn();
            this.inspectprice = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
            this.separatorControl2 = new DevExpress.XtraEditors.SeparatorControl();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.txtComp = new DevExpress.XtraEditors.TextEdit();
            this.txtProduct = new DevExpress.XtraEditors.TextEdit();
            this.chkInspectDate = new DevExpress.XtraEditors.CheckEdit();
            this.dteTo = new DevExpress.XtraEditors.DateEdit();
            this.dteFrom = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInspectAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewInspectAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProduct.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkInspectDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.groupControl1);
            this.layoutControl1.Controls.Add(this.panelControl1);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.Root;
            this.layoutControl1.Size = new System.Drawing.Size(1118, 555);
            this.layoutControl1.TabIndex = 26;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("맑은 고딕", 9.5F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.grdInspectAccount);
            this.groupControl1.CustomHeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.groupControl1.Location = new System.Drawing.Point(2, 46);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1114, 507);
            this.groupControl1.TabIndex = 19;
            this.groupControl1.Text = "≡ 조회 결과";
            // 
            // grdInspectAccount
            // 
            this.grdInspectAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdInspectAccount.Location = new System.Drawing.Point(2, 23);
            this.grdInspectAccount.MainView = this.grdViewInspectAccount;
            this.grdInspectAccount.Name = "grdInspectAccount";
            this.grdInspectAccount.Size = new System.Drawing.Size(1110, 482);
            this.grdInspectAccount.TabIndex = 0;
            this.grdInspectAccount.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grdViewInspectAccount});
            // 
            // grdViewInspectAccount
            // 
            this.grdViewInspectAccount.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.inspectdate,
            this.gridColumn2,
            this.gridColumn7,
            this.outunitprice,
            this.outqty,
            this.outprice,
            this.gridColumn10,
            this.inspectqty,
            this.inspectprice,
            this.gridColumn5});
            this.grdViewInspectAccount.GridControl = this.grdInspectAccount;
            this.grdViewInspectAccount.GroupPanelText = "항목을 드래그하여 그룹화하세요.";
            this.grdViewInspectAccount.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "inspectdate", null, "({0:n0})건"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "outqty", this.outqty, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "outprice", this.outprice, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "inspectqty", this.inspectqty, "{0:n2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "inspectprice", this.inspectprice, "{0:n2}")});
            this.grdViewInspectAccount.Name = "grdViewInspectAccount";
            this.grdViewInspectAccount.OptionsBehavior.Editable = false;
            this.grdViewInspectAccount.OptionsFind.AlwaysVisible = true;
            this.grdViewInspectAccount.OptionsFind.FindNullPrompt = "검색어를 입력하세요.";
            this.grdViewInspectAccount.OptionsView.EnableAppearanceOddRow = true;
            this.grdViewInspectAccount.OptionsView.ShowAutoFilterRow = true;
            this.grdViewInspectAccount.OptionsView.ShowFooter = true;
            // 
            // inspectdate
            // 
            this.inspectdate.AppearanceCell.Options.UseTextOptions = true;
            this.inspectdate.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.inspectdate.AppearanceHeader.Options.UseTextOptions = true;
            this.inspectdate.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.inspectdate.Caption = "검수일자";
            this.inspectdate.FieldName = "inspectdate";
            this.inspectdate.Name = "inspectdate";
            this.inspectdate.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "inspectdate", "{0:n0}건")});
            this.inspectdate.Visible = true;
            this.inspectdate.VisibleIndex = 0;
            this.inspectdate.Width = 81;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "거래선";
            this.gridColumn2.FieldName = "compname";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 134;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "품명";
            this.gridColumn7.FieldName = "productname";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 147;
            // 
            // outunitprice
            // 
            this.outunitprice.AppearanceCell.Options.UseTextOptions = true;
            this.outunitprice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.outunitprice.AppearanceHeader.Options.UseTextOptions = true;
            this.outunitprice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.outunitprice.Caption = "출고단가";
            this.outunitprice.DisplayFormat.FormatString = "n2";
            this.outunitprice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.outunitprice.FieldName = "outunitprice";
            this.outunitprice.Name = "outunitprice";
            this.outunitprice.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.outunitprice.Visible = true;
            this.outunitprice.VisibleIndex = 3;
            this.outunitprice.Width = 70;
            // 
            // outqty
            // 
            this.outqty.AppearanceCell.Options.UseTextOptions = true;
            this.outqty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.outqty.AppearanceHeader.Options.UseTextOptions = true;
            this.outqty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.outqty.Caption = "출고수량";
            this.outqty.DisplayFormat.FormatString = "n2";
            this.outqty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.outqty.FieldName = "outqty";
            this.outqty.Name = "outqty";
            this.outqty.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.outqty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "outqty", "{0:n2}")});
            this.outqty.Visible = true;
            this.outqty.VisibleIndex = 4;
            // 
            // outprice
            // 
            this.outprice.AppearanceCell.Options.UseTextOptions = true;
            this.outprice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.outprice.AppearanceHeader.Options.UseTextOptions = true;
            this.outprice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.outprice.Caption = "출고금액";
            this.outprice.DisplayFormat.FormatString = "n2";
            this.outprice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.outprice.FieldName = "outprice";
            this.outprice.Name = "outprice";
            this.outprice.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.outprice.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "outprice", "{0:n2}")});
            this.outprice.Visible = true;
            this.outprice.VisibleIndex = 5;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "검수단가";
            this.gridColumn10.DisplayFormat.FormatString = "n2";
            this.gridColumn10.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn10.FieldName = "inspectunitprice";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 6;
            this.gridColumn10.Width = 68;
            // 
            // inspectqty
            // 
            this.inspectqty.AppearanceCell.Options.UseTextOptions = true;
            this.inspectqty.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.inspectqty.AppearanceHeader.Options.UseTextOptions = true;
            this.inspectqty.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.inspectqty.Caption = "검수량";
            this.inspectqty.DisplayFormat.FormatString = "n2";
            this.inspectqty.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.inspectqty.FieldName = "inspectqty";
            this.inspectqty.Name = "inspectqty";
            this.inspectqty.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.inspectqty.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "inspectqty", "{0:n2}")});
            this.inspectqty.Visible = true;
            this.inspectqty.VisibleIndex = 7;
            this.inspectqty.Width = 64;
            // 
            // inspectprice
            // 
            this.inspectprice.AppearanceCell.Options.UseTextOptions = true;
            this.inspectprice.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.inspectprice.AppearanceHeader.Options.UseTextOptions = true;
            this.inspectprice.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.inspectprice.Caption = "검수금액";
            this.inspectprice.DisplayFormat.FormatString = "n2";
            this.inspectprice.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.inspectprice.FieldName = "inspectprice";
            this.inspectprice.Name = "inspectprice";
            this.inspectprice.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.inspectprice.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "inspectprice", "{0:n2}")});
            this.inspectprice.Visible = true;
            this.inspectprice.VisibleIndex = 8;
            this.inspectprice.Width = 82;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "농협명";
            this.gridColumn5.FieldName = "nhname";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 9;
            this.gridColumn5.Width = 83;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.btnExcel);
            this.panelControl1.Controls.Add(this.separatorControl2);
            this.panelControl1.Controls.Add(this.separatorControl1);
            this.panelControl1.Controls.Add(this.txtComp);
            this.panelControl1.Controls.Add(this.txtProduct);
            this.panelControl1.Controls.Add(this.chkInspectDate);
            this.panelControl1.Controls.Add(this.dteTo);
            this.panelControl1.Controls.Add(this.dteFrom);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.btnSearch);
            this.panelControl1.Controls.Add(this.btnClose);
            this.panelControl1.Location = new System.Drawing.Point(2, 2);
            this.panelControl1.MinimumSize = new System.Drawing.Size(0, 43);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1114, 43);
            this.panelControl1.TabIndex = 21;
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnExcel.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnExcel.Appearance.Options.UseBackColor = true;
            this.btnExcel.Appearance.Options.UseForeColor = true;
            this.btnExcel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.ImageOptions.Image")));
            this.btnExcel.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnExcel.ImageOptions.ImageToTextIndent = 10;
            this.btnExcel.Location = new System.Drawing.Point(926, 4);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(90, 36);
            this.btnExcel.TabIndex = 78;
            this.btnExcel.Text = "엑  셀";
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // separatorControl2
            // 
            this.separatorControl2.LineOrientation = System.Windows.Forms.Orientation.Vertical;
            this.separatorControl2.Location = new System.Drawing.Point(497, 5);
            this.separatorControl2.Name = "separatorControl2";
            this.separatorControl2.Size = new System.Drawing.Size(22, 34);
            this.separatorControl2.TabIndex = 76;
            // 
            // separatorControl1
            // 
            this.separatorControl1.LineOrientation = System.Windows.Forms.Orientation.Vertical;
            this.separatorControl1.Location = new System.Drawing.Point(330, 5);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(22, 34);
            this.separatorControl1.TabIndex = 77;
            // 
            // txtComp
            // 
            this.txtComp.Location = new System.Drawing.Point(396, 11);
            this.txtComp.Name = "txtComp";
            this.txtComp.Properties.NullText = "전체";
            this.txtComp.Size = new System.Drawing.Size(100, 18);
            this.txtComp.TabIndex = 74;
            // 
            // txtProduct
            // 
            this.txtProduct.Location = new System.Drawing.Point(552, 11);
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.Properties.NullText = "전체";
            this.txtProduct.Size = new System.Drawing.Size(100, 18);
            this.txtProduct.TabIndex = 75;
            // 
            // chkInspectDate
            // 
            this.chkInspectDate.Location = new System.Drawing.Point(287, 12);
            this.chkInspectDate.Name = "chkInspectDate";
            this.chkInspectDate.Properties.Caption = "전체";
            this.chkInspectDate.Size = new System.Drawing.Size(45, 20);
            this.chkInspectDate.TabIndex = 73;
            this.chkInspectDate.CheckedChanged += new System.EventHandler(this.chkInspectDate_CheckedChanged);
            // 
            // dteTo
            // 
            this.dteTo.EditValue = null;
            this.dteTo.Location = new System.Drawing.Point(181, 11);
            this.dteTo.Name = "dteTo";
            this.dteTo.Properties.Appearance.Options.UseTextOptions = true;
            this.dteTo.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.dteTo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteTo.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteTo.Size = new System.Drawing.Size(100, 18);
            this.dteTo.TabIndex = 72;
            // 
            // dteFrom
            // 
            this.dteFrom.EditValue = null;
            this.dteFrom.Location = new System.Drawing.Point(61, 11);
            this.dteFrom.Name = "dteFrom";
            this.dteFrom.Properties.Appearance.Options.UseTextOptions = true;
            this.dteFrom.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.dteFrom.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteFrom.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dteFrom.Size = new System.Drawing.Size(100, 18);
            this.dteFrom.TabIndex = 71;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(167, 15);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(9, 12);
            this.labelControl2.TabIndex = 67;
            this.labelControl2.Text = "~";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(354, 15);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 12);
            this.labelControl4.TabIndex = 68;
            this.labelControl4.Text = "거래선";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(522, 15);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 12);
            this.labelControl3.TabIndex = 69;
            this.labelControl3.Text = "품명";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(6, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 12);
            this.labelControl1.TabIndex = 70;
            this.labelControl1.Text = "검수일자";
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnSearch.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnSearch.Appearance.Options.UseBackColor = true;
            this.btnSearch.Appearance.Options.UseForeColor = true;
            this.btnSearch.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.ImageOptions.Image")));
            this.btnSearch.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSearch.ImageOptions.ImageToTextIndent = 10;
            this.btnSearch.Location = new System.Drawing.Point(830, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 36);
            this.btnSearch.TabIndex = 21;
            this.btnSearch.Text = "조  회";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Appearance.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClose.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.btnClose.Appearance.Options.UseBackColor = true;
            this.btnClose.Appearance.Options.UseForeColor = true;
            this.btnClose.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.ImageOptions.Image")));
            this.btnClose.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnClose.ImageOptions.ImageToTextIndent = 10;
            this.btnClose.Location = new System.Drawing.Point(1022, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 36);
            this.btnClose.TabIndex = 21;
            this.btnClose.Text = "닫  기";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1118, 555);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.panelControl1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(0, 44);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(5, 44);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(1118, 44);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.groupControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 44);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1118, 511);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // FormInspectAccount
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1118, 555);
            this.Controls.Add(this.layoutControl1);
            this.IconOptions.Image = global::HanIlCNS.Properties.Resources.free_icon_delivery_4586093;
            this.Name = "FormInspectAccount";
            this.Text = "검수 내역";
            this.Load += new System.EventHandler(this.FormInspectAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInspectAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewInspectAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtComp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProduct.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkInspectDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteTo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dteFrom.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl grdInspectAccount;
        private DevExpress.XtraGrid.Views.Grid.GridView grdViewInspectAccount;
        private DevExpress.XtraGrid.Columns.GridColumn inspectdate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn inspectqty;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn inspectprice;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn outunitprice;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnSearch;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraGrid.Columns.GridColumn outqty;
        private DevExpress.XtraEditors.SeparatorControl separatorControl2;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
        private DevExpress.XtraEditors.TextEdit txtComp;
        private DevExpress.XtraEditors.TextEdit txtProduct;
        private DevExpress.XtraEditors.CheckEdit chkInspectDate;
        private DevExpress.XtraEditors.DateEdit dteTo;
        private DevExpress.XtraEditors.DateEdit dteFrom;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn outprice;
        private DevExpress.XtraEditors.SimpleButton btnExcel;
    }
}