using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.WinForms.Report.FinIpr
{
    public partial class ucFinIprInpatientCounts : Report.Common.ucQueryBaseForDataWindow
    {
        /// <summary>
        /// ȫԺסԺ����ͳ��
        /// </summary>
        public ucFinIprInpatientCounts ()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            try
            {
                Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڲ�ѯ���ݣ����Ժ�......");
                this.dwMain.Retrieve(new object[] { });
            }
            finally
            {
                Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
            }
        }
    }
}
