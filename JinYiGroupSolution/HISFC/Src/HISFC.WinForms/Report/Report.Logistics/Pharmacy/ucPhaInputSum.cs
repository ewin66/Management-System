using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Neusoft.FrameWork.Management;
using Neusoft.FrameWork.Function;

namespace Report.Logistics.Pharmacy
{
    /// <summary>
    /// [��������: ҩ��������]<br></br>
    /// [�� �� ��: ����]<br></br>
    /// [����ʱ��: 2007-10]<br></br>
    /// </summary>
    public partial class ucPhaInputSum : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public ucPhaInputSum()
        {
            InitializeComponent();
        }



        #region ����

        /// <summary>
        /// ��ѯ��ʼʱ��
        /// </summary>
        public DateTime BeginTime
        {
            get
            {
                return NConvert.ToDateTime(this.dtpBeginTime.Text);
            }
        }

        /// <summary>
        /// ��ѯ��ֹʱ��
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return NConvert.ToDateTime(this.dtpEndTime.Text);
            }
        }

        #endregion

        #region ������

        protected override int OnQuery(object sender, object neuObject)
        {
            this.Query();

            return base.OnQuery(sender, neuObject);
        }

        #endregion

        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <returns></returns>
        protected int Init()
        {
            Neusoft.HISFC.BizLogic.Manager.Department deptManager = new Neusoft.HISFC.BizLogic.Manager.Department();
            ArrayList alDept = deptManager.GetDeptmentAll();

            ArrayList alStockDept = new ArrayList();
            foreach (Neusoft.HISFC.Models.Base.Department dept in alDept)
            {
                if (dept.DeptType.ID.ToString() == "PI")
                {
                    alStockDept.Add(dept);
                }
            }

            this.cmbStockDept.AddItems(alStockDept);

            this.dtpBeginTime.Value = deptManager.GetDateTimeFromSysDateTime().AddDays(-1);
            this.dtpEndTime.Value = deptManager.GetDateTimeFromSysDateTime();
            return 1;
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        /// <returns></returns>

        protected int Query()
        {
            if (this.cmbStockDept.Tag == null || this.cmbStockDept.Tag.ToString() == "")
            {
                MessageBox.Show(Language.Msg("��ѡ���ѯҩ��"));
                return -1;
            }

            if (this.dtpBeginTime.Value > this.dtpEndTime.Value)
            {
                MessageBox.Show("��ʼʱ�䲻�ܴ��ڽ���ʱ�䣡");
                return -1;
            }

            string sqlID = string.Empty;

            if (neuCheckBox1.Checked)
            {   
                //��ʾҩƷ���
                sqlID = "WinForms.Report.Logistics.Pharmacy.InputSumBYDrugType"; 
            }
            else
            {
               //ֻ��������λ����
                sqlID = "Report.Logistics.Pharmacy.InputSum";
            }

            System.Data.DataSet ds = new DataSet();

            Neusoft.FrameWork.Management.DataBaseManger dataManager = new DataBaseManger();
            if (dataManager.ExecQuery(sqlID, ref ds, this.cmbStockDept.Tag.ToString(), this.BeginTime.ToString(), this.EndTime.ToString()) == -1)
            {
                MessageBox.Show(Language.Msg("û�������Ϣ��") + dataManager.Err);
                return -1;
            }

            if (ds == null || ds.Tables.Count <= 0)
            {
                return 0;
            }
            this.fpSpread1_Sheet1.DataSource = ds;

          

            int iTotIndex = this.fpSpread1_Sheet1.RowCount;
            decimal sumNum4 = 0;
            decimal sumNum3 = 0;
            decimal sumNum2 = 0;
            decimal sumNum1 = 0;


            if (neuCheckBox1.Checked)
            {

                for (int i = 0; i < iTotIndex; i++)
                {
                    sumNum1 = sumNum1 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 5].Text);
                    sumNum2 = sumNum2 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 2].Text);
                    sumNum3 = sumNum3 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 3].Text);
                    sumNum4 = sumNum4 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 4].Text);

                }
                //this.fpSpread1_Sheet1.RowCount = iTotIndex + 1;
                this.fpSpread1_Sheet1.Rows.Add(iTotIndex, 1);
                
                this.fpSpread1_Sheet1.Cells[iTotIndex , 0].Text = "�ϼ�";
                this.fpSpread1_Sheet1.Cells[iTotIndex , 5].Text = sumNum1.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex , 2].Text = sumNum2.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex, 3].Text = sumNum3.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex, 4].Text = sumNum4.ToString();

            }
            else
            {

                for (int i = 0; i < iTotIndex; i++)
                {
                    sumNum1 = sumNum1 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 1].Text);
                    sumNum2 = sumNum2 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 2].Text);
                    sumNum3 = sumNum3 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 3].Text);
                    sumNum4 = sumNum4 + NConvert.ToDecimal(this.fpSpread1_Sheet1.Cells[i, 4].Text);
                }
                //this.fpSpread1_Sheet1.RowCount = iTotIndex + 1;
                this.fpSpread1_Sheet1.Rows.Add(iTotIndex, 1);
                this.fpSpread1_Sheet1.Cells[iTotIndex + 1, 0].Text = "�ϼ�";
                this.fpSpread1_Sheet1.Cells[iTotIndex + 1, 1].Text = sumNum1.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex + 1, 2].Text = sumNum2.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex + 1, 3].Text = sumNum3.ToString();
                this.fpSpread1_Sheet1.Cells[iTotIndex + 1, 4].Text = sumNum4.ToString();
            }

            return 1;
        }
        /// <summary>
        /// ��ӡ
        /// </summary>
        public void Print()
        {
            Neusoft.FrameWork.WinForms.Classes.Print print = new Neusoft.FrameWork.WinForms.Classes.Print();
            print.ControlBorder = Neusoft.FrameWork.WinForms.Classes.enuControlBorder.None;
            print.PrintPage(30, 10, this);

        }
        protected override int OnPrint(object sender, object neuObject)
        {
            this.Print();
            return base.OnPrint(sender, neuObject);
        }

        /// <summary>
        /// Ԥ��
        /// </summary>
        public override int PrintPreview(object sender, object neuObject)
        {
            Neusoft.FrameWork.WinForms.Classes.Print printview = new Neusoft.FrameWork.WinForms.Classes.Print();

            //printview.PrintPreview(0, 0, this.neuTabControl1.SelectedTab);
            printview.PrintPreview(this.panel2);
            return base.OnPrintPreview(sender, neuObject);
        }

        //����
        //public override int Export(object sender, object neuObject)
        //{
        //    if (this.fpSpread1.Export() == 1)
        //    {
        //        MessageBox.Show(Language.Msg("�����ɹ�"));
        //    }

        //    return 1;
        //}

        //public override int Export(object sender, object neuObject)
        //{
        //   return this.fpSpread1.Export();
        //}
        protected override void OnLoad(EventArgs e)
        {
            this.Init();

            base.OnLoad(e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}