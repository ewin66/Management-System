namespace Neusoft.WinForms.Report.Pharmacy
{
    partial class ucPhaDeptOutput
    {
        /// <summary>
        /// ����������������
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// ������������ʹ�õ���Դ��
        /// </summary>
        /// <param name="disposing">���Ӧ�ͷ��й���Դ��Ϊ true������Ϊ false��</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows ������������ɵĴ���

        /// <summary>
        /// �����֧������ķ��� - ��Ҫ
        /// ʹ�ô���༭���޸Ĵ˷��������ݡ�
        /// </summary>
        private void InitializeComponent()
        {
            this.plLeft.SuspendLayout();
            this.plRight.SuspendLayout();
            this.plMain.SuspendLayout();
            this.plTop.SuspendLayout();
            this.plBottom.SuspendLayout();
            this.plRightTop.SuspendLayout();
            this.plRightBottom.SuspendLayout();
            this.gbMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // plLeft
            // 
            this.plLeft.Size = new System.Drawing.Size(185, 541);
            // 
            // plRight
            // 
            this.plRight.Location = new System.Drawing.Point(189, 0);
            this.plRight.Size = new System.Drawing.Size(807, 541);
            // 
            // plQueryCondition
            // 
            this.plQueryCondition.Size = new System.Drawing.Size(185, 62);
            // 
            // slLeft
            // 
            this.slLeft.Location = new System.Drawing.Point(185, 0);
            // 
            // plLeftControl
            // 
            this.plLeftControl.Size = new System.Drawing.Size(185, 479);
            // 
            // plRightTop
            // 
            this.plRightTop.Size = new System.Drawing.Size(807, 537);
            // 
            // slTop
            // 
            this.slTop.Location = new System.Drawing.Point(0, 537);
            this.slTop.Size = new System.Drawing.Size(807, 4);
            // 
            // plRightBottom
            // 
            this.plRightBottom.Location = new System.Drawing.Point(0, 541);
            this.plRightBottom.Size = new System.Drawing.Size(807, 0);
            // 
            // gbMid
            // 
            this.gbMid.Size = new System.Drawing.Size(797, 48);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(766, 11);
            // 
            // dwMain
            // 
            this.dwMain.DataWindowObject = "d_pha_stat_dept_output";
            this.dwMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dwMain.LibraryList = "Report\\pha.pbd;pha.pbl";
            this.dwMain.Location = new System.Drawing.Point(0, 0);
            this.dwMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dwMain.Name = "dwMain";
            this.dwMain.ScrollBars = Sybase.DataWindow.DataWindowScrollBars.Both;
            this.dwMain.Size = new System.Drawing.Size(807, 537);
            this.dwMain.TabIndex = 0;
            this.dwMain.Text = "neuDataWindow1";
            // 
            // ucPhaDeptOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.LeftControl = Report.Common.ucQueryBaseForDataWindow.QueryControls.Tree;
            this.MainDWDataObject = "d_pha_stat_dept_output";
            this.MainDWLabrary = "Report\\pha.pbd;pha.pbl";
            this.Name = "ucPhaDeptOutput";
            this.plLeft.ResumeLayout(false);
            this.plRight.ResumeLayout(false);
            this.plMain.ResumeLayout(false);
            this.plTop.ResumeLayout(false);
            this.plTop.PerformLayout();
            this.plBottom.ResumeLayout(false);
            this.plRightTop.ResumeLayout(false);
            this.plRightBottom.ResumeLayout(false);
            this.gbMid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}