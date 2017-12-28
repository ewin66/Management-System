using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Neusoft.FrameWork.Models;
using System.Collections;

using Neusoft.HISFC.Models.SIInterface;

namespace Neusoft.HISFC.Components.InpatientFee.Maintenance
{
    public partial class ucCompare : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        public ucCompare()
        {
            InitializeComponent();
        }

        #region ö��
        public enum CompareTypes
        {
            /// <summary>
            /// ��ҩ
            /// </summary>
            P = 0,
            /// <summary>
            /// �в�ҩ
            /// </summary>
            C = 1,
            /// <summary>
            /// �г�ҩ
            /// </summary>
            Z = 2,
            //{B36F2A99-872C-4659-9035-6D80B5489F50} ͬsql����Ӧ wbo 2010-08-28
            ///// <summary>
            ///// ȫ��ҩƷ
            ///// </summary>
            //All = 3,
            /// <summary>
            /// ȫ��ҩƷ
            /// </summary>
            ALL = 3,
            /// <summary>
            /// ��ҩƷ
            /// </summary>
            Undrug = 4,
        };
        #endregion

        #region ����
        ArrayList alDrug = new ArrayList();//ҩƷ�б�
        private NeuObject pactCode = new NeuObject();//��ͬ��λ
        private bool isDrug = false;
        private string code = "PY"; //��ѯ��
        private int circle = 0;
        DataTable dtHisItem = new DataTable();
        DataTable dtCenterItem = new DataTable();
        DataTable dtCompareItem = new DataTable();
        DataView dvHisItem = new DataView();
        DataView dvCenterItem = new DataView();
        DataView dvCompareItem = new DataView();
        private CompareTypes compareType;
        protected Neusoft.HISFC.BizLogic.Fee.ConnectSI myConnectSI = null;
        /// <summary>
        /// Tab
        /// </summary>
        protected Hashtable hashTableFp = new Hashtable();
        protected Neusoft.HISFC.BizLogic.Fee.Interface myInterface = new Neusoft.HISFC.BizLogic.Fee.Interface();
        protected Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        #endregion

        #region ����
        [Category("����"), Description("������Ŀ���� P:��ҩ��C:�в�ҩ��Z:�г�ҩ��ALL:ȫ��ҩƷ��Undrug:��ҩƷ")]
        public CompareTypes CompareType
        {
            get
            {
                return compareType;
            }
            set
            {
                compareType = value;
            }
        }

        /// <summary>
        /// ��ͬ��λ��Ϣ
        /// </summary>
        public NeuObject PactCode
        {
            set
            {
                pactCode = value;
            }
            get
            {
                return pactCode;
            }
        }
        #endregion

        #region ����
        /// <summary>
        /// ��ʼ����ʾ����
        /// </summary>
        public void Init()
        {
            if (CompareType.ToString() != CompareTypes.Undrug.ToString())
            {
                isDrug = true;
            }
            else
            {
                isDrug = false;
            }

            InitColumn();

            InitData();

            InitColumnProHis();

            InitColumnProCenter();

            InitColumnProCompare();

            InitHashTable();
        }
        /// <summary>
        /// ����ҽ��������
        /// </summary>
        /// <returns></returns>
        public int ConnectSIServer()
        {
            try
            {
                myConnectSI = new Neusoft.HISFC.BizLogic.Fee.ConnectSI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("����ҽ��������ʧ��!,��������������" + ex.Message);
                return -1;
            }
            return 0;
        }

        private void InitHashTable()
        {
            foreach (TabPage t in this.tabCompare.TabPages)
            {
                foreach (Control c in t.Controls)
                {
                    if (c is FarPoint.Win.Spread.FpSpread)
                    {
                        this.hashTableFp.Add(t, c);
                    }
                }
            }
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("����", "����", Neusoft.FrameWork.WinForms.Classes.EnumImageList.H�ϲ�, true, false, null);
            this.toolBarService.AddToolButton("ȡ��", "ȡ��", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Qȡ��, true, false, null);
            this.toolBarService.AddToolButton("���", "���", Neusoft.FrameWork.WinForms.Classes.EnumImageList.Q���, true, false, null);

            return this.toolBarService;
        }

        /// <summary>
        /// ���ҩƷ������Ϣ
        /// </summary>
        public void GetHisDrugItem()
        {
            alDrug = myInterface.GetNoCompareDrugItem(pactCode.ID, compareType.ToString());
        }
        /// <summary>
        /// ������ʾ����Ϣ;
        /// </summary>
        private void InitColumn()
        {

            Type str = typeof(System.String);
            Type dec = typeof(System.Decimal);
            Type date = typeof(System.DateTime);



            if (compareType.ToString() !=CompareTypes.Undrug.ToString())
            {
                //��ʼ��������Ŀ:
                DataColumn[] colHisItem = new DataColumn[]{new DataColumn("ҩƷ����", str),
                                                              new DataColumn("ҩƷ����", str),
                                                              new DataColumn("ƴ����", str),
                                                              new DataColumn("�����", str),
                                                              new DataColumn("�Զ�����", str),
                                                              new DataColumn("���", str),
                                                              new DataColumn("ͨ����", str),
                                                              new DataColumn("ͨ����ƴ��", str),
                                                              new DataColumn("ͨ�������", str),
                                                              new DataColumn("���ʱ���", str),
                                                              new DataColumn("���ұ���", str),
                                                              new DataColumn("�۸�", str),
                                                              new DataColumn("���ͱ���", str),
                                                              //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                                                              new DataColumn("ҩƷ���", str)};

                dtHisItem.Columns.AddRange(colHisItem);

                DataColumn[] keyHis = new DataColumn[1];
                keyHis[0] = dtHisItem.Columns[0];
                dtHisItem.CaseSensitive = true;
                dtHisItem.PrimaryKey = keyHis;
                dvHisItem = new DataView(dtHisItem);
                dvHisItem.Sort = "ҩƷ���� ASC";
                fpHisItem_Sheet1.DataSource = dvHisItem;

                DataColumn[] colCenterItem = new DataColumn[]{new DataColumn("���ı���", str),
                                                                 new DataColumn("������Ŀ����", str),
                                                                 new DataColumn("������ĿӢ����", str),
                                                                 new DataColumn("���", str),
                                                                 new DataColumn("����", str),
                                                                 new DataColumn("����ƴ����", str),
                                                                 new DataColumn("���÷���", str),
                                                                 new DataColumn("Ŀ¼����", str),
                                                                 new DataColumn("Ŀ¼�ȼ�", str),
                                                                 new DataColumn("�Ը�����", dec),
                                                                 new DataColumn("��׼�۸�", dec),
                                                                 new DataColumn("����ʹ��˵��", str),
                                                                 new DataColumn("��Ŀ���", str)};
                dtCenterItem.Columns.AddRange(colCenterItem);
                DataColumn[] keyCenter = new DataColumn[1];
                keyCenter[0] = dtCenterItem.Columns[0];
                dtCenterItem.CaseSensitive = true;
                dtCenterItem.PrimaryKey = keyCenter;
                dvCenterItem = new DataView(dtCenterItem);
                dvCenterItem.Sort = "���ı��� ASC";
                fpCenterItem_Sheet1.DataSource = dvCenterItem;
            }
            else 
            {
                //��ʼ��������Ŀ:
                DataColumn[] colHisItem = new DataColumn[]{new DataColumn("��ҩƷ����", str),
                                                              new DataColumn("��ҩƷ����", str),
                                                              new DataColumn("ƴ����", str),
                                                              new DataColumn("�����", str),
                                                              new DataColumn("�Զ�����", str),
                                                              new DataColumn("���", str),
                                                              new DataColumn("���ʱ���", str),
                                                              new DataColumn("���ұ���", str),
                                                              new DataColumn("�۸�", str),
                                                              new DataColumn("��λ", str)};

                dtHisItem.Columns.AddRange(colHisItem);

                DataColumn[] keyHis = new DataColumn[1];
                keyHis[0] = dtHisItem.Columns[0];
                dtHisItem.PrimaryKey = keyHis;
                dvHisItem = new DataView(dtHisItem);
                dvHisItem.Sort = "��ҩƷ���� ASC";
                fpHisItem_Sheet1.DataSource = dvHisItem;

                DataColumn[] colCenterItem = new DataColumn[]{new DataColumn("���ı���", str),
                                                                 new DataColumn("������Ŀ����", str),
                                                                 new DataColumn("������ĿӢ����", str),
                                                                 new DataColumn("���", str),
                                                                 new DataColumn("����", str),
                                                                 new DataColumn("����ƴ����", str),
                                                                 new DataColumn("���÷���", str),
                                                                 new DataColumn("Ŀ¼����", str),
                                                                 new DataColumn("Ŀ¼�ȼ�", str),
                                                                 new DataColumn("�Ը�����", dec),
                                                                 new DataColumn("��׼�۸�", dec),
                                                                 new DataColumn("����ʹ��˵��", str),
                                                                 new DataColumn("��Ŀ���", str)};
                dtCenterItem.Columns.AddRange(colCenterItem);
                DataColumn[] keyCenter = new DataColumn[1];
                keyCenter[0] = dtCenterItem.Columns[0];
                dtCenterItem.CaseSensitive=true;
                dtCenterItem.PrimaryKey = keyCenter;
                dvCenterItem = new DataView(dtCenterItem);
                dvCenterItem.Sort = "���ı��� ASC";
                fpCenterItem_Sheet1.DataSource = dvCenterItem;
            }

            DataColumn[] colCompareItem = new DataColumn[]{ 
                                                            new DataColumn("ҽԺ�Զ�����", str),
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("���ı���", str),
                                                            new DataColumn("��Ŀ���", str),
                                                            new DataColumn("ҽ���շ���Ŀ��������", str),
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("ҽ���շ���ĿӢ������", str),
                                                            new DataColumn("ҽ������", str),
                                                            new DataColumn("ҽ�����",str),
                                                            new DataColumn("ҽ��ƴ������", str),
                                                            new DataColumn("ҽ�����÷������", str),
                                                            new DataColumn("ҽ��Ŀ¼����", str),
                                                            new DataColumn("ҽ��Ŀ¼�ȼ�", str),
                                                            new DataColumn("�Ը�����", dec),
                                                            new DataColumn("��׼�۸�", dec),
                                                            new DataColumn("����ʹ��˵��", str),
                                                            new DataColumn("ҽԺƴ��", str),
                                                            new DataColumn("ҽԺ�����", str),
                                                            new DataColumn("ҽԺ���", str),
                                                            new DataColumn("ҽԺ�����۸�", dec),
                                                            new DataColumn("ҽԺ����", str),
                                                            new DataColumn("����Ա", str),
                                                            new DataColumn("����ʱ��", date),
                                                            //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                                                            new DataColumn("ҽԺ��Ŀ���", str)};
            dtCompareItem.Columns.AddRange(colCompareItem);
            DataColumn[] keyCompare = new DataColumn[1];
            keyCompare[0] = dtCompareItem.Columns[1];
            dtCompareItem.CaseSensitive=true;
            dtCompareItem.PrimaryKey = keyCompare;
            dvCompareItem = new DataView(dtCompareItem);
            dvCompareItem.Sort = "ҽԺ�Զ����� ASC";
            fpCompareItem_Sheet1.DataSource = dvCompareItem;
            
        }
        /// <summary>
        /// HIS������Ŀ������
        /// </summary>
        private void InitColumnProHis()
        {
            int width = 20;

            if (compareType.ToString()!=CompareTypes.Undrug.ToString())
            {
                //this.fpHisItem_Sheet1.Columns[0].Visible = false;
                this.fpHisItem_Sheet1.Columns[0].Visible = true;
                this.fpHisItem_Sheet1.Columns[1].Width = width * 8;
                this.fpHisItem_Sheet1.Columns[2].Visible = false;
                this.fpHisItem_Sheet1.Columns[3].Visible = false;
                this.fpHisItem_Sheet1.Columns[4].Visible = true;
                this.fpHisItem_Sheet1.Columns[5].Width = width * 8;
                this.fpHisItem_Sheet1.Columns[6].Width = width * 8;
                this.fpHisItem_Sheet1.Columns[7].Visible = false;
                this.fpHisItem_Sheet1.Columns[8].Visible = false;
                this.fpHisItem_Sheet1.Columns[9].Visible = false;
                this.fpHisItem_Sheet1.Columns[10].Visible = false;
                this.fpHisItem_Sheet1.Columns[11].Width = width * 3;
                this.fpHisItem_Sheet1.Columns[12].Width = width * 4;
            }
            else
            {
                //this.fpHisItem_Sheet1.Columns[0].Visible = false;
                this.fpHisItem_Sheet1.Columns[0].Visible = true;
                this.fpHisItem_Sheet1.Columns[1].Width = width * 8;
                this.fpHisItem_Sheet1.Columns[2].Visible = false;
                this.fpHisItem_Sheet1.Columns[3].Visible = false;
                this.fpHisItem_Sheet1.Columns[4].Visible = true;
                this.fpHisItem_Sheet1.Columns[5].Width = width * 8;
                this.fpHisItem_Sheet1.Columns[6].Visible = false;
                this.fpHisItem_Sheet1.Columns[7].Visible = false;
                this.fpHisItem_Sheet1.Columns[8].Width = width * 3;
                this.fpHisItem_Sheet1.Columns[9].Width = width * 4;
            }
        }
        /// <summary>
        /// ��ʼ��������������Ϣ
        /// </summary>
        private void InitColumnProCenter()
        {
            int width = 20;
            this.fpCenterItem_Sheet1.Columns[0].Visible = true;
            this.fpCenterItem_Sheet1.Columns[1].Width = width * 8;
            this.fpCenterItem_Sheet1.Columns[2].Width = width * 8;
            this.fpCenterItem_Sheet1.Columns[3].Width = width * 8;
            this.fpCenterItem_Sheet1.Columns[4].Width = width * 3;
            this.fpCenterItem_Sheet1.Columns[5].Visible = false;
            this.fpCenterItem_Sheet1.Columns[6].Visible = false;
            this.fpCenterItem_Sheet1.Columns[7].Visible = false;
            this.fpCenterItem_Sheet1.Columns[8].Width = width * 3;
            this.fpCenterItem_Sheet1.Columns[9].Width = width * 4;
            this.fpCenterItem_Sheet1.Columns[10].Width = width * 3;
            this.fpCenterItem_Sheet1.Columns[11].Width = width * 8;
        }
        private void InitColumnProCompare()
        {
            int width = 20;

            FarPoint.Win.Spread.CellType.DateTimeCellType dtType = new FarPoint.Win.Spread.CellType.DateTimeCellType();
            dtType.DateTimeFormat = FarPoint.Win.Spread.CellType.DateTimeFormat.ShortDateWithTime;

            fpCompareItem_Sheet1.Columns[0].Visible = true;
            fpCompareItem_Sheet1.Columns[1].Visible = true;
            fpCompareItem_Sheet1.Columns[2].Visible = true;
            fpCompareItem_Sheet1.Columns[3].Width = width * 8;
            fpCompareItem_Sheet1.Columns[4].Width = width * 8;
            fpCompareItem_Sheet1.Columns[5].Width = width * 8;
            fpCompareItem_Sheet1.Columns[6].Visible = true;
            fpCompareItem_Sheet1.Columns[7].Visible = false;
            fpCompareItem_Sheet1.Columns[8].Visible = false;
            fpCompareItem_Sheet1.Columns[9].Visible = false;
            fpCompareItem_Sheet1.Columns[10].Width = width * 4;
            fpCompareItem_Sheet1.Columns[11].Visible = true;
            fpCompareItem_Sheet1.Columns[12].Width = width * 4;
            fpCompareItem_Sheet1.Columns[13].Width = width * 4;
            fpCompareItem_Sheet1.Columns[14].Width = width * 4;
            fpCompareItem_Sheet1.Columns[15].Width = width * 6;
            fpCompareItem_Sheet1.Columns[16].Visible = false;
            fpCompareItem_Sheet1.Columns[17].Visible = false;
            fpCompareItem_Sheet1.Columns[18].Visible = false;
            fpCompareItem_Sheet1.Columns[19].Width = width * 8;
            fpCompareItem_Sheet1.Columns[20].Width = width * 4;
            fpCompareItem_Sheet1.Columns[21].Width = width * 4;
            fpCompareItem_Sheet1.Columns[22].Width = width * 4;
            fpCompareItem_Sheet1.Columns[23].Width = width * 6;
            fpCompareItem_Sheet1.Columns[23].CellType = dtType;


        }
        /// <summary>
        /// ��ʼ����ʾ����
        /// </summary>
        public void InitData()
        {
            ArrayList alHisItem = new ArrayList();
            ArrayList alCenterItem = new ArrayList();
            ArrayList alCompareItem = new ArrayList();

            if (isDrug)
            {
                #region ����ҩƷ
                alHisItem = this.myInterface.GetNoCompareDrugItem(pactCode.ID, compareType.ToString());
                if (alHisItem != null)
                {
                    foreach (Neusoft.HISFC.Models.Pharmacy.Item obj in alHisItem)
                    {
                        DataRow row = dtHisItem.NewRow();
                        row["ҩƷ����"] = obj.ID;
                        row["ҩƷ����"] = obj.Name;
                        row["ƴ����"] = obj.SpellCode;
                        row["�����"] = obj.WBCode;
                        row["�Զ�����"] = obj.UserCode;
                        row["���"] = obj.Specs;
                        row["���ʱ���"] = obj.NationCode;
                        row["���ұ���"] = obj.GBCode;
                        row["�۸�"] = obj.PriceCollection.RetailPrice;// .RetailPrice;
                        row["���ͱ���"] = obj.DosageForm.ID;
                        //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                        row["ҩƷ���"] = (obj.Type.ID == "P") ? "X" : "Z";

                        dtHisItem.Rows.Add(row);
                    }
                }
                //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                //if (compareType.ToString() == "ALL")
                //    alCenterItem = this.myInterface.GetCenterItem(pactCode.ID);
                //else
                alCenterItem = this.myInterface.GetCenterItem(pactCode.ID, compareType.ToString());

                if (alCenterItem != null)
                {
                    foreach (Neusoft.HISFC.Models.SIInterface.Item obj in alCenterItem)
                    {
                        DataRow row = dtCenterItem.NewRow();
                        row["���ı���"] = obj.ID;
                        row["������Ŀ����"] = obj.Name;
                        row["������ĿӢ����"] = obj.EnglishName;
                        row["���"] = obj.Specs;
                        row["����"] = obj.DoseCode;
                        row["����ƴ����"] = obj.SpellCode;
                        row["���÷���"] = obj.FeeCode;
                        row["Ŀ¼����"] = obj.ItemType;
                        row["Ŀ¼�ȼ�"] = obj.ItemGrade;
                        row["�Ը�����"] = obj.Rate;
                        row["��׼�۸�"] = obj.Price;
                        row["����ʹ��˵��"] = obj.Memo;
                        row["��Ŀ���"] = obj.SysClass;
                        dtCenterItem.Rows.Add(row);
                    }
                }

                alCompareItem = this.myInterface.GetCompareItem(pactCode.ID, compareType.ToString());

                if (alCompareItem != null)
                {
                    foreach (Neusoft.HISFC.Models.SIInterface.Compare obj in alCompareItem)
                    {
                        DataRow row = dtCompareItem.NewRow();

                        row["������Ŀ����"] = obj.HisCode;
                        row["���ı���"] = obj.CenterItem.ID;
                        row["��Ŀ���"] = obj.CenterItem.SysClass;
                        row["ҽ���շ���Ŀ��������"] = obj.CenterItem.Name;
                        row["ҽ���շ���ĿӢ������"] = obj.CenterItem.EnglishName;
                        row["������Ŀ����"] = obj.Name;
                        row["������Ŀ����"] = obj.RegularName;
                        row["ҽ������"] = obj.CenterItem.DoseCode;
                        row["ҽ��ƴ������"] = obj.CenterItem.SpellCode;
                        row["ҽ�����÷������"] = obj.CenterItem.FeeCode;
                        row["ҽ��Ŀ¼����"] = obj.CenterItem.ItemType;
                        row["ҽ��Ŀ¼�ȼ�"] = obj.CenterItem.ItemGrade;
                        row["�Ը�����"] = obj.CenterItem.Rate;
                        row["��׼�۸�"] = obj.CenterItem.Price;
                        row["����ʹ��˵��"] = obj.CenterItem.Memo;
                        row["ҽԺƴ��"] = obj.SpellCode.SpellCode;
                        row["ҽԺ�����"] = obj.SpellCode.WBCode;
                        row["ҽԺ�Զ�����"] = obj.SpellCode.UserCode;
                        row["ҽԺ���"] = obj.Specs;
                        row["ҽԺ�����۸�"] = obj.Price;
                        row["ҽԺ����"] = obj.DoseCode;
                        row["����Ա"] = obj.CenterItem.OperCode;
                        row["����ʱ��"] = obj.CenterItem.OperDate;

                        dtCompareItem.Rows.Add(row);
                    }
                }
                #endregion
            }
            else
            {
                #region ���ط�ҩƷ
                alHisItem = myInterface.GetNoCompareUndrugItem(pactCode.ID);
                if (alHisItem != null)
                {
                    foreach (Neusoft.HISFC.Models.Fee.Item.Undrug obj in alHisItem)
                    {
                        DataRow row = dtHisItem.NewRow();
                        row["��ҩƷ����"] = obj.ID;
                        row["��ҩƷ����"] = obj.Name;
                        row["ƴ����"] = obj.SpellCode;
                        row["�����"] = obj.WBCode;
                        row["�Զ�����"] = obj.UserCode;
                        row["���"] = obj.Specs;
                        row["���ʱ���"] = obj.NationCode;
                        row["���ұ���"] = obj.GBCode;
                        row["�۸�"] = obj.Price;
                        row["��λ"] = obj.PriceUnit;
                        dtHisItem.Rows.Add(row);
                    }
                }

                alCenterItem = this.myInterface.GetCenterItem(pactCode.ID, compareType.ToString());
                if (alCenterItem != null)
                {
                    foreach (Neusoft.HISFC.Models.SIInterface.Item obj in alCenterItem)
                    {
                        DataRow row = dtCenterItem.NewRow();
                        row["���ı���"] = obj.ID;
                        row["������Ŀ����"] = obj.Name;
                        row["������ĿӢ����"] = obj.EnglishName;
                        row["���"] = obj.Specs;
                        row["����"] = obj.DoseCode;
                        row["����ƴ����"] = obj.SpellCode;
                        row["���÷���"] = obj.FeeCode;
                        row["Ŀ¼����"] = obj.ItemType;
                        row["Ŀ¼�ȼ�"] = obj.ItemGrade;
                        row["�Ը�����"] = obj.Rate;
                        row["��׼�۸�"] = obj.Price;
                        row["����ʹ��˵��"] = obj.Memo;
                        row["��Ŀ���"] = obj.SysClass;
                        dtCenterItem.Rows.Add(row);
                    }
                }

                alCompareItem = this.myInterface.GetCompareItem(pactCode.ID, compareType.ToString());
                if (alCompareItem != null)
                {
                    foreach (Neusoft.HISFC.Models.SIInterface.Compare obj in alCompareItem)
                    {
                        DataRow row = dtCompareItem.NewRow();

                        row["������Ŀ����"] = obj.HisCode;//0
                        row["���ı���"] = obj.CenterItem.ID;
                        row["��Ŀ���"] = obj.CenterItem.SysClass;
                        row["ҽ���շ���Ŀ��������"] = obj.CenterItem.Name;
                        row["ҽ���շ���ĿӢ������"] = obj.CenterItem.EnglishName;//4
                        row["������Ŀ����"] = obj.Name;
                        row["������Ŀ����"] = obj.RegularName;
                        row["ҽ������"] = obj.CenterItem.DoseCode;
                        row["ҽ��ƴ������"] = obj.CenterItem.SpellCode;//8
                        row["ҽ�����÷������"] = obj.CenterItem.FeeCode;
                        row["ҽ��Ŀ¼����"] = obj.CenterItem.ItemType;
                        row["ҽ��Ŀ¼�ȼ�"] = obj.CenterItem.ItemGrade;
                        row["�Ը�����"] = obj.CenterItem.Rate;//12
                        row["��׼�۸�"] = obj.CenterItem.Price;
                        row["����ʹ��˵��"] = obj.CenterItem.Memo;
                        row["ҽԺƴ��"] = obj.SpellCode.SpellCode;
                        row["ҽԺ�����"] = obj.SpellCode.WBCode;//16
                        row["ҽԺ�Զ�����"] = obj.SpellCode.UserCode;
                        row["ҽԺ���"] = obj.Specs;
                        row["ҽԺ�����۸�"] = obj.Price;
                        row["ҽԺ����"] = obj.DoseCode;//20
                        row["����Ա"] = obj.CenterItem.OperCode;
                        row["����ʱ��"] = obj.CenterItem.OperDate;

                        dtCompareItem.Rows.Add(row);
                    }
                    this.fpCompareItem_Sheet1.Columns[0].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[1].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[2].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[3].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[4].Width = 120;
                    this.fpCompareItem_Sheet1.Columns[5].Width = 120;
                    this.fpCompareItem_Sheet1.Columns[6].Width = 60;
                    this.fpCompareItem_Sheet1.Columns[7].Width = 60;
                    this.fpCompareItem_Sheet1.Columns[8].Width = 60;
                    this.fpCompareItem_Sheet1.Columns[9].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[10].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[11].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[12].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[13].Width = 30;
                    this.fpCompareItem_Sheet1.Columns[14].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[15].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[16].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[17].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[18].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[19].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[20].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[21].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[22].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[23].Width = 80;
                    this.fpCompareItem_Sheet1.Columns[24].Width = 80;
                    /*
                                                            new DataColumn("ҽԺ�Զ�����", str),//0
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("���ı���", str),
                                                            new DataColumn("��Ŀ���", str),
                                                            new DataColumn("ҽ���շ���Ŀ��������", str),//4
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("������Ŀ����", str),
                                                            new DataColumn("ҽ���շ���ĿӢ������", str),
                                                            new DataColumn("ҽ������", str),//8
                                                            new DataColumn("ҽ�����",str),
                                                            new DataColumn("ҽ��ƴ������", str),
                                                            new DataColumn("ҽ�����÷������", str),
                                                            new DataColumn("ҽ��Ŀ¼����", str),//12
                                                            new DataColumn("ҽ��Ŀ¼�ȼ�", str),
                                                            new DataColumn("�Ը�����", dec),
                                                            new DataColumn("��׼�۸�", dec),
                                                            new DataColumn("����ʹ��˵��", str),//16
                                                            new DataColumn("ҽԺƴ��", str),
                                                            new DataColumn("ҽԺ�����", str),
                                                            new DataColumn("ҽԺ���", str),
                                                            new DataColumn("ҽԺ�����۸�", dec),//20
                                                            new DataColumn("ҽԺ����", str),
                                                            new DataColumn("����Ա", str),
                                                            new DataColumn("����ʱ��", date),
                                                            //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                                                            new DataColumn("ҽԺ��Ŀ���", str)};//24
                     */
                    if (pactCode.ID == "06")
                    {
                        this.fpCompareItem_Sheet1.Columns[7].Visible = false;//ҽ��Ӣ����
                        this.fpCompareItem_Sheet1.Columns[6].Visible = false;//���ر���
                        this.fpCompareItem_Sheet1.Columns[10].Visible = false;//ƴ����
                        this.fpCompareItem_Sheet1.Columns[17].Visible = false;//ƴ����
                        this.fpCompareItem_Sheet1.Columns[18].Visible = false;//�����
                    }
                }
                #endregion
            }

            this.dtCenterItem.AcceptChanges();
            this.dtCompareItem.AcceptChanges();
            this.dtHisItem.AcceptChanges();
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="input"></param>
        private void FilterItem(string flag, string input)
        {
            string filterString = "";
            input = input.ToUpper();
            switch (flag)
            {
                case "HIS":
                    switch (code)
                    {
                        case "PY":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "ƴ����" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "ƴ����" + " like '%" + input + "%'";
                            }
                            break;
                        case "WB":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "�����" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "�����" + " like '%" + input + "%'";
                            }

                            break;
                        case "US":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "�Զ�����" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "�Զ�����" + " like '%" + input + "%'";
                            }

                            break;
                        case "ZW":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "ҩƷ����" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "ҩƷ����" + " like '%" + input + "%'";
                            }

                            break;
                        case "TYPY":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "ͨ����ƴ��" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "ͨ����ƴ��" + " like '%" + input + "%'";
                            }

                            break;
                        case "TYWB":
                            if (!this.checkBox1.Checked)
                            {
                                filterString = "ͨ�������" + " like '" + input + "%'";
                            }
                            else
                            {
                                filterString = "ͨ�������" + " like '%" + input + "%'";
                            }

                            break;
                    }
                    this.dvHisItem.RowFilter = filterString;
                    InitColumnProHis();
                    break;
                case "CENTER":
                    if (!this.checkBox1.Checked)
                    {
                        filterString = "����ƴ����" + " like '" + input + "%'" + " or " + "���ı���" + " like '" + input + "%'" + " or " + "���ı���" + " like '" + input + "%'";
                    }
                    else
                    {
                        filterString = "����ƴ����" + " like '%" + input + "%'" + " or " + "���ı���" + " like '%" + input + "%'" + " or " + "���ı���" + " like '%" + input + "%'";
                    }
                    this.dvCenterItem.RowFilter = filterString;
                    InitColumnProCenter();
                    break;
                case "COMPARE":
                    if (!this.checkBox1.Checked)
                    {
                        filterString = "ҽԺƴ��" + " like '" + input + "%'" + " or " + "ҽԺ�Զ�����" + " like '" + input + "%'";
                    }
                    else
                    {
                        filterString = "ҽԺƴ��" + " like '%" + input + "%'" + " or " + "ҽԺ�Զ�����" + " like '%" + input + "%'";
                    }
                    this.dvCompareItem.RowFilter = filterString;
                    break;
            }
        }
        /// <summary>
        /// ��ʾѡ��ı�����Ϣ
        /// </summary>
        /// <param name="row"></param>
        private void SetHisItemInfo(int row)
        {
            string hisCode = "";
            if (isDrug)
            {
                hisCode = this.fpHisItem_Sheet1.Cells[row, 0].Text.Trim();
                this.tbHisName.Text = this.fpHisItem_Sheet1.Cells[row, 1].Text;
                this.tbHisPrice.Text = this.fpHisItem_Sheet1.Cells[row, 11].Text;

                Neusoft.HISFC.Models.Pharmacy.Item obj = this.GetSelectHisItem(hisCode);

                if (obj == null)
                {
                    MessageBox.Show("δ�ҵ�ѡ������ҩƷ!");
                }
                else
                {
                    this.tbHisSpell.Tag = obj;
                }

            }
            else
            {
                hisCode = this.fpHisItem_Sheet1.Cells[row, 0].Text.Trim();
                this.tbHisName.Text = this.fpHisItem_Sheet1.Cells[row, 1].Text;
                this.tbHisPrice.Text = this.fpHisItem_Sheet1.Cells[row, 8].Text;

                Neusoft.HISFC.Models.Fee.Item.Undrug obj = this.GetSelectHisUndrugItem(hisCode);

                if (obj == null)
                {
                    MessageBox.Show("δ�ҵ�ѡ�����ط�ҩƷ!");
                }
                else
                {
                    this.tbHisSpell.Tag = obj;
                }

            }

            tabCompare.SelectedTab = this.tbCenterItem;
            this.tbCenterSpell.Focus();
        }
        /// <summary>
        /// ��ʾѡ���������Ϣ
        /// </summary>
        /// <param name="row"></param>
        private void SetCenterItemInfo(int row)
        {
            string centerCode = "";

            centerCode = this.fpCenterItem_Sheet1.Cells[row, 0].Text.Trim();

            Item obj = this.GetSelectCenterItem(centerCode);

            if (obj == null)
            {
                MessageBox.Show("δ�ҵ�����ҩƷ");
            }
            else
            {
                tbCenterSpell.Tag = obj;
            }

            this.tbCenterName.Text = this.fpCenterItem_Sheet1.Cells[row, 1].Text;
            this.tbCenterPrice.Text = this.fpCenterItem_Sheet1.Cells[row, 10].Text;
            this.tabCompare.SelectedTab = this.tbCompare;
        }
        /// <summary>
        /// �����ѡ��HISҩƷ��Ϣ
        /// </summary>
        /// <param name="hisCode">ҽԺҩƷ����</param>
        /// <returns>ҩƷ��Ϣʵ��</returns>
        private Neusoft.HISFC.Models.Pharmacy.Item GetSelectHisItem(string hisCode)
        {
            Neusoft.HISFC.Models.Pharmacy.Item obj = new Neusoft.HISFC.Models.Pharmacy.Item();

            DataRow row = this.dtHisItem.Rows.Find(hisCode);

            obj.ID = row["ҩƷ����"].ToString();
            obj.Name = row["ҩƷ����"].ToString();
            obj.SpellCode = row["ƴ����"].ToString();
            obj.WBCode = row["�����"].ToString();
            obj.UserCode = row["�Զ�����"].ToString();
            obj.Specs = row["���"].ToString();
            obj.NameCollection.RegularName = row["ͨ����"].ToString();
            //obj.RegularSpellCode.Spell_Code = row["ͨ����ƴ��"].ToString();
            obj.NameCollection.SpellCode = row["ͨ����ƴ��"].ToString();
            obj.NameCollection.WBCode = row["ͨ�������"].ToString();
            obj.NameCollection.InternationalCode = row["���ʱ���"].ToString();
            obj.GBCode = row["���ұ���"].ToString();
            obj.PriceCollection.RetailPrice = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["�۸�"].ToString());
            obj.DosageForm.ID = row["���ͱ���"].ToString();
            //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
            obj.Type.ID = row["ҩƷ���"].ToString();

            return obj;
        }
        /// <summary>
        /// ��ñ���His��ҩƷ��Ϣ
        /// </summary>
        /// <param name="hisCode"></param>
        /// <returns></returns>
        private Neusoft.HISFC.Models.Fee.Item.Undrug GetSelectHisUndrugItem(string hisCode)
        {
            Neusoft.HISFC.Models.Fee.Item.Undrug obj = new Neusoft.HISFC.Models.Fee.Item.Undrug();

            DataRow row = this.dtHisItem.Rows.Find(hisCode);

            obj.ID = row["��ҩƷ����"].ToString();
            obj.Name = row["��ҩƷ����"].ToString();
            obj.SpellCode = row["ƴ����"].ToString();
            obj.WBCode = row["�����"].ToString();
            obj.UserCode = row["�Զ�����"].ToString();
            obj.Specs = row["���"].ToString();
            obj.NationCode = row["���ʱ���"].ToString();
            obj.GBCode = row["���ұ���"].ToString();
            obj.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["�۸�"].ToString());
            obj.PriceUnit = row["��λ"].ToString();

            return obj;
        }

        /// <summary>
        /// �����ѡ������Ŀ��Ϣ
        /// </summary>
        /// <param name="centerCode"></param>
        /// <returns></returns>
        private Neusoft.HISFC.Models.SIInterface.Item GetSelectCenterItem(string centerCode)
        {
            Item obj = new Item();

            DataRow row = this.dtCenterItem.Rows.Find(centerCode);
            if (isDrug)
            {
                obj.ID = row["���ı���"].ToString();
                obj.Name = row["������Ŀ����"].ToString();
                obj.EnglishName = row["������ĿӢ����"].ToString();
            }
            else
            {
                obj.ID = row["���ı���"].ToString();
                obj.Name = row["������Ŀ����"].ToString();
                obj.EnglishName = row["������ĿӢ����"].ToString();
            }

            obj.Specs = row["���"].ToString();
            obj.DoseCode = row["����"].ToString();
            obj.SpellCode = row["����ƴ����"].ToString();
            obj.FeeCode = row["���÷���"].ToString();
            obj.ItemType = row["Ŀ¼����"].ToString();
            obj.ItemGrade = row["Ŀ¼�ȼ�"].ToString();
            obj.Rate = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["�Ը�����"].ToString());
            obj.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["��׼�۸�"].ToString());
            obj.Memo = row["����ʹ��˵��"].ToString();
            obj.SysClass = row["��Ŀ���"].ToString();


            return obj;
        }
        /// <summary>
        /// ���ղ���
        /// </summary>
        public void Compare()
        {
            Compare objCom = new Compare();

            if (isDrug)
            {
                Neusoft.HISFC.Models.Pharmacy.Item objHis = new Neusoft.HISFC.Models.Pharmacy.Item();
                Item objCenter = new Item();

                if (this.tbHisSpell.Tag == null || this.tbHisSpell.Tag.ToString() == "")
                {
                    MessageBox.Show("û��ѡ�񱾵���Ŀ!");
                    return;
                }

                objHis = (Neusoft.HISFC.Models.Pharmacy.Item)this.tbHisSpell.Tag;

                if (tbCenterSpell.Tag == null || tbCenterSpell.Tag.ToString() == "")
                {
                    MessageBox.Show("û��ѡ��������Ŀ");
                    return;
                }

                objCenter = (Item)this.tbCenterSpell.Tag;

                DataRow row = this.dtCompareItem.NewRow();

                row["������Ŀ����"] = objHis.ID;
                row["���ı���"] = objCenter.ID;
                row["��Ŀ���"] = objCenter.SysClass;
                row["ҽ���շ���Ŀ��������"] = objCenter.Name;
                row["ҽ���շ���ĿӢ������"] = objCenter.EnglishName;
                row["������Ŀ����"] = objHis.Name;
                row["������Ŀ����"] = objHis.NameCollection.RegularName;//.RegularName;
                row["ҽ������"] = objCenter.DoseCode;
                row["ҽ�����"] = objCenter.Specs;
                row["ҽ��ƴ������"] = objCenter.SpellCode;
                row["ҽ�����÷������"] = objCenter.FeeCode;
                row["ҽ��Ŀ¼����"] = objCenter.ItemType;
                row["ҽ��Ŀ¼�ȼ�"] = objCenter.ItemGrade;
                row["�Ը�����"] = objCenter.Rate;
                row["��׼�۸�"] = objCenter.Price;
                row["����ʹ��˵��"] = objCenter.Memo;
                row["ҽԺƴ��"] = objHis.SpellCode;
                row["ҽԺ�����"] = objHis.WBCode;// .SpellCode.WB_Code;
                row["ҽԺ�Զ�����"] = objHis.UserCode;//SpellCode.User_Code;
                row["ҽԺ���"] = objHis.Specs;
                row["ҽԺ�����۸�"] = objHis.PriceCollection.RetailPrice; //.RetailPrice;
                row["ҽԺ����"] = objHis.DosageForm.ID;
                row["����Ա"] = myInterface.Operator.ID;
                row["����ʱ��"] = System.DateTime.Now;
                //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                row["ҽԺ��Ŀ���"] = objHis.Type.ID;

                dtCompareItem.Rows.Add(row);


                objCom.CenterItem.PactCode = pactCode.ID;
                objCom.HisCode = objHis.ID;
                objCom.CenterItem.ID = objCenter.ID;
                objCom.CenterItem.SysClass = objCenter.SysClass;
                objCom.CenterItem.Name = objCenter.Name;
                objCom.CenterItem.EnglishName = objCenter.EnglishName;
                objCom.Name = objHis.Name;
                objCom.RegularName = objHis.NameCollection.RegularName; //.RegularName;
                objCom.CenterItem.DoseCode = objCenter.DoseCode;
                objCom.CenterItem.Specs = objCenter.Specs;
                objCom.CenterItem.FeeCode = objCenter.FeeCode;
                objCom.CenterItem.ItemType = objCenter.ItemType;
                objCom.CenterItem.ItemGrade = objCenter.ItemGrade;
                objCom.CenterItem.Rate = objCenter.Rate;
                objCom.CenterItem.Price = objCenter.Price;
                objCom.CenterItem.Memo = objCenter.Memo;
                objCom.SpellCode.SpellCode = objHis.SpellCode;
                objCom.SpellCode.WBCode = objHis.WBCode;//SpellCode.WB_Code;
                objCom.SpellCode.UserCode = objHis.UserCode;//SpellCode.User_Code;
                objCom.Specs = objHis.Specs;
                objCom.Price = objHis.PriceCollection.RetailPrice;//.RetailPrice;
                objCom.DoseCode = objHis.DosageForm.ID;
                objCom.CenterItem.OperCode = myInterface.Operator.ID;
                //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                objCom.HisSysClass = objHis.Type.ID;

                DataRow rowFind = dtHisItem.Rows.Find(objHis.ID);
                dtHisItem.Rows.Remove(rowFind);
            }
            else
            {
                //neusoft.HISFC.Models.Fee.Item objHis = new neusoft.HISFC.Models.Fee.Item();
                Neusoft.HISFC.Models.Fee.Item.Undrug objHis = new Neusoft.HISFC.Models.Fee.Item.Undrug();
                Item objCenter = new Item();

                if (this.tbHisSpell.Tag == null || this.tbHisSpell.Tag.ToString() == "")
                {
                    MessageBox.Show("û��ѡ�񱾵���Ŀ!");
                    return;
                }

                objHis = (Neusoft.HISFC.Models.Fee.Item.Undrug)this.tbHisSpell.Tag;

                if (tbCenterSpell.Tag == null || tbCenterSpell.Tag.ToString() == "")
                {
                    MessageBox.Show("û��ѡ��������Ŀ");
                    return;
                }

                objCenter = (Item)this.tbCenterSpell.Tag;

                DataRow row = this.dtCompareItem.NewRow();

                row["������Ŀ����"] = objHis.ID;
                row["���ı���"] = objCenter.ID;
                row["��Ŀ���"] = objCenter.SysClass;
                row["ҽ���շ���Ŀ��������"] = objCenter.Name;
                row["ҽ���շ���ĿӢ������"] = objCenter.EnglishName;
                row["������Ŀ����"] = objHis.Name;
                row["������Ŀ����"] = "";
                row["ҽ������"] = objCenter.DoseCode;
                row["ҽ�����"] = objCenter.Specs;
                row["ҽ��ƴ������"] = objCenter.SpellCode;//SpellCode.Spell_Code;
                row["ҽ�����÷������"] = objCenter.FeeCode;
                row["ҽ��Ŀ¼����"] = objCenter.ItemType;
                row["ҽ��Ŀ¼�ȼ�"] = objCenter.ItemGrade;
                row["�Ը�����"] = objCenter.Rate;
                row["��׼�۸�"] = objCenter.Price;
                row["����ʹ��˵��"] = objCenter.Memo;
                row["ҽԺƴ��"] = objHis.SpellCode;
                row["ҽԺ�����"] = objHis.WBCode;
                row["ҽԺ�Զ�����"] = objHis.UserCode;
                row["ҽԺ���"] = objHis.Specs;
                row["ҽԺ�����۸�"] = objHis.Price;
                row["ҽԺ����"] = "";
                row["����Ա"] = myInterface.Operator.ID;
                row["����ʱ��"] = System.DateTime.Now;
                //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                row["ҽԺ��Ŀ���"] = "L";

                dtCompareItem.Rows.Add(row);

                objCom.CenterItem.PactCode = pactCode.ID;
                objCom.HisCode = objHis.ID;
                objCom.CenterItem.ID = objCenter.ID;
                objCom.CenterItem.SysClass = objCenter.SysClass;
                objCom.CenterItem.Name = objCenter.Name;
                objCom.CenterItem.EnglishName = objCenter.EnglishName;
                objCom.Name = objHis.Name;
                objCom.RegularName = "";
                objCom.CenterItem.DoseCode = objCenter.DoseCode;
                objCom.CenterItem.Specs = objCenter.Specs;
                objCom.CenterItem.FeeCode = objCenter.FeeCode;
                objCom.CenterItem.ItemType = objCenter.ItemType;
                objCom.CenterItem.ItemGrade = objCenter.ItemGrade;
                objCom.CenterItem.Rate = objCenter.Rate;
                objCom.CenterItem.Price = objCenter.Price;
                objCom.CenterItem.Memo = objCenter.Memo;
                objCom.SpellCode.SpellCode = objHis.SpellCode;
                objCom.SpellCode.WBCode = objHis.WBCode;
                objCom.SpellCode.UserCode = objHis.UserCode;
                objCom.Specs = objHis.Specs;
                objCom.Price = objHis.Price;
                objCom.DoseCode = "";
                objCom.CenterItem.OperCode = myInterface.Operator.ID;
                //{68A052FC-106E-4a2d-8FEF-FD17B46F37FF} ҽ���������ӱ�����Ŀ���
                objCom.HisSysClass = "L";

                DataRow rowFind = dtHisItem.Rows.Find(objHis.ID);
                dtHisItem.Rows.Remove(rowFind);
            }



            //neusoft.neuFC.Management.Transaction t = new neusoft.neuFC.Management.Transaction(neusoft.neuFC.Management.Connection.Instance);
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            myInterface.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            int returnValue = 0;

            returnValue = myInterface.InsertCompareItem(objCom);

            if (returnValue == -1)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("����ʧ��!" + myInterface.Err);
                return;
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            Clear();
            this.tbHisSpell.Focus();
        }
        /// <summary>
        /// ɾ���Ѷ�����Ϣ
        /// </summary>
        public void Delete()
        {
            int rowAct = this.fpCompareItem_Sheet1.ActiveRowIndex;
            if (this.fpCompareItem_Sheet1.RowCount <= 0)
                return;

            string hisCode = "";
            hisCode = this.fpCompareItem_Sheet1.Cells[rowAct, 1].Text;

            if (hisCode == "" || hisCode == null)
                return;

            //neusoft.neuFC.Management.Transaction t = new neusoft.neuFC.Management.Transaction(neusoft.neuFC.Management.Connection.Instance);
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            myInterface.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            int returnValue = 0;

            returnValue = myInterface.DeleteCompareItem(pactCode.ID, hisCode);

            if (returnValue == -1)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("ɾ������ʧ��!" + myInterface.Err);
                return;
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            DataRow row = this.dtCompareItem.Rows.Find(hisCode);

            DataRow rowHis = dtHisItem.NewRow();
            if (isDrug)
            {
                rowHis["ҩƷ����"] = row["������Ŀ����"].ToString();
                rowHis["ҩƷ����"] = row["������Ŀ����"].ToString();
                rowHis["ͨ����"] = row["������Ŀ����"].ToString();
                rowHis["���ͱ���"] = row["ҽԺ����"].ToString();
            }
            else
            {
                rowHis["��ҩƷ����"] = row["������Ŀ����"].ToString();
                rowHis["��ҩƷ����"] = row["������Ŀ����"].ToString();
                //				rowHis["���ʱ���"] = row["���ʱ���"].ToString();
                //				rowHis["���ұ���"] = row["���ұ���"].ToString();
                //				rowHis["��λ"] = row["��λ"].ToString();
            }
            rowHis["ƴ����"] = row["ҽԺƴ��"].ToString();
            rowHis["�����"] = row["ҽԺ�����"].ToString();
            rowHis["�Զ�����"] = row["ҽԺ�Զ�����"].ToString();
            rowHis["���"] = row["ҽԺ���"].ToString();
            rowHis["�۸�"] = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["ҽԺ�����۸�"].ToString());


            dtCompareItem.Rows.Remove(row);
            dtHisItem.Rows.Add(rowHis);


        }
        /// <summary>
        /// �����Ϣ
        /// </summary>
        public void Clear()
        {
            //this.tbCenterSpell.Text = "";
            this.tbCenterSpell.Tag = "";
            this.tbCenterName.Text = "";
            this.tbCenterPrice.Text = "";
            this.tbGrade.Text = "";


            this.tbHisSpell.Tag = "";
            this.tbHisName.Text = "";
            this.tbHisPrice.Text = "";
        }

        /// <summary>
        /// ���溯��
        /// </summary>
        public void Save()
        {
            //Neusoft.FrameWork.Management.Transaction t = new Neusoft.FrameWork.Management.Transaction(Neusoft.FrameWork.Management.Connection.Instance);
            Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();
            myInterface.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);
            int returnValue = 0;

            ArrayList alAdd = GetAddCompareItem();

            if (alAdd != null)
            {
                foreach (Compare obj in alAdd)
                {
                    returnValue = myInterface.InsertCompareItem(obj);
                    if (returnValue == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("���������Ϣ����!" + myInterface.Err);
                        return;
                    }
                }
            }

            ArrayList alDelete = GetDeleteCompareItem();

            if (alDelete != null)
            {
                foreach (Compare obj in alDelete)
                {
                    returnValue = myInterface.DeleteCompareItem(this.pactCode.ID, obj.HisCode);
                    if (returnValue == -1)
                    {
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        MessageBox.Show("ɾ��������Ϣ����!" + myInterface.Err);
                        return;
                    }
                }
            }

            Neusoft.FrameWork.Management.PublicTrans.Commit();

            MessageBox.Show("����ɹ�!");
        }

        public void Close()
        {

        }

        /// <summary>
        /// ������ǰ��Ŀ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="neuObject"></param>
        /// <returns></returns>
        public override int Export(object sender, object neuObject)
        {
            object obj = this.hashTableFp[this.tabCompare.SelectedTab];

            FarPoint.Win.Spread.FpSpread fp = obj as FarPoint.Win.Spread.FpSpread;

            SaveFileDialog op = new SaveFileDialog();

            op.Title = "��ѡ�񱣴��·��������";
            op.CheckFileExists = false;
            op.CheckPathExists = true;
            op.DefaultExt = "*.xls";
            op.Filter = "(*.xls)|*.xls";

            DialogResult result = op.ShowDialog();

            if (result == DialogResult.Cancel || op.FileName == string.Empty)
            {
                return -1;
            }

            string filePath = op.FileName;

            bool returnValue = fp.SaveExcel(filePath, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                                


            return base.Export(sender, neuObject);
        }
        /// <summary>
        /// ���������Ŀ
        /// </summary>
        /// <returns></returns>
        private ArrayList GetAddCompareItem()
        {
            DataTable dt = this.dtCompareItem.GetChanges(DataRowState.Added);
            ArrayList al = new ArrayList();
            if (dt == null)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                Compare obj = new Compare();

                obj.CenterItem.PactCode = pactCode.ID;
                obj.HisCode = row["������Ŀ����"].ToString();
                obj.CenterItem.ID = row["���ı���"].ToString();
                obj.CenterItem.SysClass = row["��Ŀ���"].ToString();
                obj.CenterItem.Name = row["ҽ���շ���Ŀ��������"].ToString();
                obj.CenterItem.EnglishName = row["ҽ���շ���ĿӢ������"].ToString();
                obj.Name = row["������Ŀ����"].ToString();
                obj.RegularName = row["������Ŀ����"].ToString();
                obj.CenterItem.DoseCode = row["ҽ������"].ToString();
                obj.CenterItem.Specs = row["ҽ�����"].ToString();
                obj.CenterItem.SpellCode = row["ҽ��ƴ������"].ToString();
                obj.CenterItem.FeeCode = row["ҽ�����÷������"].ToString();
                obj.CenterItem.ItemType = row["ҽ��Ŀ¼����"].ToString();
                obj.CenterItem.ItemGrade = row["ҽ��Ŀ¼�ȼ�"].ToString();
                obj.CenterItem.Rate = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["�Ը�����"].ToString());
                obj.CenterItem.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["��׼�۸�"].ToString());
                obj.CenterItem.Memo = row["����ʹ��˵��"].ToString();
                obj.SpellCode.SpellCode = row["ҽԺƴ��"].ToString();
                obj.SpellCode.WBCode = row["ҽԺ�����"].ToString();
                obj.SpellCode.UserCode = row["ҽԺ�Զ�����"].ToString();
                obj.Specs = row["ҽԺ���"].ToString();
                obj.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["ҽԺ�����۸�"].ToString());
                obj.DoseCode = row["ҽԺ����"].ToString();
                obj.CenterItem.OperCode = row["����Ա"].ToString();
                obj.CenterItem.OperDate = Convert.ToDateTime(row["����ʱ��"].ToString());

                al.Add(obj);
            }

            return al;
        }

        private ArrayList GetDeleteCompareItem()
        {
            //dtCompareItem.RejectChanges();

            DataTable dt = this.dtCompareItem.GetChanges(DataRowState.Deleted);

            ArrayList al = new ArrayList();
            if (dt == null)
            {
                return null;
            }
            foreach (DataRow row in dt.Rows)
            {
                Compare obj = new Compare();

                obj.CenterItem.PactCode = pactCode.ID;
                obj.HisCode = row["������Ŀ����"].ToString();
                obj.CenterItem.ID = row["���ı���"].ToString();
                obj.CenterItem.SysClass = row["��Ŀ���"].ToString();
                obj.CenterItem.Name = row["ҽ���շ���Ŀ��������"].ToString();
                obj.CenterItem.EnglishName = row["ҽ���շ���ĿӢ������"].ToString();
                obj.Name = row["������Ŀ����"].ToString();
                obj.RegularName = row["������Ŀ����"].ToString();
                obj.CenterItem.DoseCode = row["ҽ������"].ToString();
                obj.CenterItem.Specs = row["ҽ�����"].ToString();
                obj.CenterItem.SpellCode = row["ҽ��ƴ������"].ToString();
                obj.CenterItem.FeeCode = row["ҽ�����÷������"].ToString();
                obj.CenterItem.ItemType = row["ҽ��Ŀ¼����"].ToString();
                obj.CenterItem.ItemGrade = row["ҽ��Ŀ¼�ȼ�"].ToString();
                obj.CenterItem.Rate = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["�Ը�����"].ToString());
                obj.CenterItem.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["��׼�۸�"].ToString());
                obj.CenterItem.Memo = row["����ʹ��˵��"].ToString();
                obj.SpellCode.SpellCode = row["ҽԺƴ��"].ToString();
                obj.SpellCode.WBCode = row["ҽԺ�����"].ToString();
                obj.SpellCode.UserCode = row["ҽԺ�Զ�����"].ToString();
                obj.Specs = row["ҽԺ���"].ToString();
                obj.Price = Neusoft.FrameWork.Function.NConvert.ToDecimal(row["ҽԺ�����۸�"].ToString());
                obj.DoseCode = row["ҽԺ����"].ToString();
                obj.CenterItem.OperCode = row["����Ա"].ToString();
                obj.CenterItem.OperDate = Convert.ToDateTime(row["����ʱ��"].ToString());

                al.Add(obj);
            }

            this.dtCompareItem.AcceptChanges();

            return al;
        }

        #endregion

        #region �¼�
        private void tbHisSpell_TextChanged(object sender, EventArgs e)
        {
            this.FilterItem("HIS", this.tbHisSpell.Text);
        }

        private void tbCenterSpell_TextChanged(object sender, EventArgs e)
        {
            this.FilterItem("CENTER", this.tbCenterSpell.Text);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.F2)
            {
                circle++;

                switch (circle)
                {
                    case 0:
                        code = "PY";
                        tbSpell.Text = "ƴ����";
                        break;
                    case 1:
                        code = "WB";
                        tbSpell.Text = "�����";
                        break;
                    case 2:
                        code = "US";
                        tbSpell.Text = "�Զ�����";
                        break;
                    case 3:
                        code = "ZW";
                        tbSpell.Text = "����";
                        break;
                    case 4:
                        code = "TYPY";
                        tbSpell.Text = "ͨ��ƴ��";
                        break;
                    case 5:
                        code = "TYWB";
                        tbSpell.Text = "ͨ�����";
                        break;
                }

                if (circle == 5)
                {
                    circle = -1;
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        private void tbHisSpell_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.fpHisItem_Sheet1.RowCount <= 0)
            {
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                this.fpHisItem.SetViewportTopRow(0, this.fpHisItem_Sheet1.ActiveRowIndex - 5);
                this.fpHisItem_Sheet1.ActiveRowIndex--;
                this.fpHisItem_Sheet1.AddSelection(this.fpHisItem_Sheet1.ActiveRowIndex, 0, 1, 0);
            }
            if (e.KeyCode == Keys.Down)
            {
                this.fpHisItem.SetViewportTopRow(0, this.fpHisItem_Sheet1.ActiveRowIndex - 4);
                this.fpHisItem_Sheet1.ActiveRowIndex++;
                this.fpHisItem_Sheet1.AddSelection(this.fpHisItem_Sheet1.ActiveRowIndex, 0, 1, 0);
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.fpHisItem_Sheet1.RowCount >= 0)
                {
                    SetHisItemInfo(this.fpHisItem_Sheet1.ActiveRowIndex);
                }
            }
        }

        private void fpHisItem_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.fpHisItem_Sheet1.RowCount >= 0)
                SetHisItemInfo(this.fpHisItem_Sheet1.ActiveRowIndex);
        }

        private void fpCenterItem_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (this.fpHisItem_Sheet1.RowCount >= 0)
            {
                SetCenterItemInfo(this.fpCenterItem_Sheet1.ActiveRowIndex);
            }
        }

        private void tbCenterSpell_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.fpCenterItem_Sheet1.RowCount <= 0)
            {
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                this.fpCenterItem.SetViewportTopRow(0, this.fpCenterItem_Sheet1.ActiveRowIndex - 5);
                this.fpCenterItem_Sheet1.ActiveRowIndex--;
                this.fpCenterItem_Sheet1.AddSelection(this.fpCenterItem_Sheet1.ActiveRowIndex, 0, 1, 0);
            }
            if (e.KeyCode == Keys.Down)
            {
                this.fpCenterItem.SetViewportTopRow(0, this.fpCenterItem_Sheet1.ActiveRowIndex - 4);
                this.fpCenterItem_Sheet1.ActiveRowIndex++;
                this.fpCenterItem_Sheet1.AddSelection(this.fpCenterItem_Sheet1.ActiveRowIndex, 0, 1, 0);
            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.fpHisItem_Sheet1.RowCount >= 0)
                {
                    SetCenterItemInfo(this.fpCenterItem_Sheet1.ActiveRowIndex);
                }
            }
        }

        private void tbHisSpell_Enter(object sender, EventArgs e)
        {
            this.tabCompare.SelectedIndex = 0;
        }

        private void tbCenterSpell_Enter(object sender, EventArgs e)
        {
            this.tabCompare.SelectedIndex = 1;
        }

        private void tbCompareQuery_TextChanged(object sender, EventArgs e)
        {
            FilterItem("COMPARE", this.tbCompareQuery.Text);
        }

        private void ucCompare_Load(object sender, EventArgs e)
        {
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ������ݣ����Ժ�^^");
            Application.DoEvents();
            ////compareType = base.Tag.ToString();// this.FindForm().Tag.ToString();
            //if (this.Tag.ToString() == "DALL")
            //{
            //    drugType.ID = "ALL";
            //    drugType.Name = "ȫ��";
            //}
            //else
            //{
            //    drugType.ID = compareType.Substring(3, 1);
            //    switch (drugType.ID)
            //    {
            //        case "P":
            //            drugType.Name = "��ҩ";
            //            break;
            //        case "Z":
            //            drugType.Name = "�г�ҩ";
            //            break;
            //        case "C":
            //            drugType.Name = "��ҩ";
            //            break;
            //        case "U":
            //            drugType.Name = "��ҩƷ";
            //            break;
            //    }
            //}
            this.CompareType = this.compareType;
            this.GetPactinfo();
            //this.pactCode.ID = "2";
            this.Init();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        private int GetPactinfo()
        {
            Neusoft.HISFC.BizLogic.Fee.PactUnitInfo pactManager = new Neusoft.HISFC.BizLogic.Fee.PactUnitInfo();
            ArrayList pactList = pactManager.QueryPactUnitAll();
            if (pactList == null) 
            {
                MessageBox.Show("��ʼ����ͬ��λ����!" + pactManager.Err);
                return -1;
            }
            this.cmbPact.AddItems(pactList);
            return 1;
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text)
            {
                case "����":
                    {
                        this.Compare();
                        break;
                    }
                case "ȡ��":
                    {
                        this.Delete();
                        break;
                    }
                case "���":
                    {
                        this.Clear();
                        break;
                    }
            }
            base.ToolStrip_ItemClicked(sender, e);
        }

        #endregion 

        private void cmbPact_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pactCode.ID = this.cmbPact.Tag.ToString();
            Neusoft.FrameWork.WinForms.Classes.Function.ShowWaitForm("���ڼ������ݣ����Ժ�^^");
            Application.DoEvents();
            this.dtHisItem.Clear();
            this.dtCenterItem.Clear();
            this.dtCompareItem.Clear();
            InitData();
            Neusoft.FrameWork.WinForms.Classes.Function.HideWaitForm();
        }

        private void tabCompare_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.fpHisItem.Visible = true;
            this.fpCenterItem.Visible = true;
            this.fpCompareItem.Visible = true;
        }

        private void fpCompareItem_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(pactCode.ID != "06" || this.isDrug == true)
            {
                return;
            }
            string hisCode = this.fpCompareItem_Sheet1.Cells[e.Row, 1].Text;
            string centerCode = this.fpCompareItem_Sheet1.Cells[e.Row, 2].Text;
            string centerName = this.fpCompareItem_Sheet1.Cells[e.Row, 4].Text;
            decimal hisPrice = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpCompareItem_Sheet1.Cells[e.Row, 20].Text);
            decimal centerPrice = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpCompareItem_Sheet1.Cells[e.Row, 15].Text);
            decimal centerRate = Neusoft.FrameWork.Function.NConvert.ToDecimal(this.fpCompareItem_Sheet1.Cells[e.Row, 14].Text);
            string centerMemo = "";
            Neusoft.HISFC.Models.SIInterface.Compare com = new Compare();
            com.HisCode = hisCode;
            com.CenterItem.ID = centerCode;
            com.CenterItem.Name = centerName;
            com.Price = hisPrice;
            com.CenterItem.Price = centerPrice;
            com.CenterItem.Rate = centerRate;
            com.CenterItem.Memo = centerMemo;
            this.ShowForm(com);
            this.fpCompareItem_Sheet1.Cells[e.Row, 15].Text = com.CenterItem.Price.ToString("0.00");
            this.fpCompareItem_Sheet1.Cells[e.Row, 14].Text = com.CenterItem.Rate.ToString("0.00");
        }

        private void ShowForm(Neusoft.HISFC.Models.SIInterface.Compare com)
        {
            ucItem item = new ucItem();
            item.IsModify = true;
            item.PactCode = "06";          
            item.CompareItem = com;

            DialogResult dr = Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(item);
            if (dr == DialogResult.Yes)
            {
                //������ʾ
                DataRow dataRow = this.dtCompareItem.Rows.Find(com.HisCode);
                dataRow[15] = com.CenterItem.Price;
                dataRow[14] = com.CenterItem.Rate;
            }
        }
    }
}