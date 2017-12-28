using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.HISFC.Components.Nurse
{
    /// <summary>
    /// �������ά��������
    /// </summary>
    public partial class ucQueue : Neusoft.FrameWork.WinForms.Controls.ucBaseControl
    {
        #region ��������������

        /// <summary>
        /// ������
        /// </summary>
        private Neusoft.FrameWork.WinForms.Forms.ToolBarService toolBarService = new Neusoft.FrameWork.WinForms.Forms.ToolBarService();

        protected override Neusoft.FrameWork.WinForms.Forms.ToolBarService OnInit(object sender, object neuObject, object param)
        {
            this.toolBarService.AddToolButton("����", "����", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.X�½�, true, false, null);
            this.toolBarService.AddToolButton("ɾ��", "ɾ��", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.Sɾ��, true, false, null);
            this.toolBarService.AddToolButton("�༭", "�༭", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.X�޸�, true, false, null);
            this.toolBarService.AddToolButton("ģ��", "ģ��", (int)Neusoft.FrameWork.WinForms.Classes.EnumImageList.A����, true, false, null);

            return this.toolBarService;
            //return base.OnInit(sender, neuObject, param);
        }

        public override void ToolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Text.Trim())
            {
                case "����":
                    this.AddData();
                    break;
                case "ɾ��":
                    this.DelData();
                    break;
                case "�༭":
                    this.EditData();
                    break;
                case "ģ��":
                    this.PopWin("2");
                    break;
                default:
                    break;
            }
        }

        #endregion
        

        public ucQueue()
        {
            InitializeComponent();
        }

        #region ������

        /// <summary>
        /// ���ӱ༭���пؼ�
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Registration.Registration myMgr = null;

        /// <summary>
        /// ���������
        /// </summary>
        private Neusoft.HISFC.BizLogic.Nurse.Assign assignMgr = new Neusoft.HISFC.BizLogic.Nurse.Assign();

        private Neusoft.HISFC.BizProcess.Integrate.Manager personMgr = null;

        private ArrayList alNurse = new ArrayList();
        private ArrayList al = new ArrayList();
        private Hashtable htNoon = new Hashtable();
        private Hashtable htDoct = new Hashtable();

        private Neusoft.HISFC.BizLogic.Nurse.Queue myQueue = new Neusoft.HISFC.BizLogic.Nurse.Queue();
        private Neusoft.HISFC.Models.Nurse.Queue Queue = new Neusoft.HISFC.Models.Nurse.Queue();
        private Neusoft.HISFC.BizProcess.Integrate.Manager cDept = new Neusoft.HISFC.BizProcess.Integrate.Manager();
        private Neusoft.FrameWork.Models.NeuObject neuobj = new Neusoft.FrameWork.Models.NeuObject();
        private Nurse.cResult myResult = new cResult();

        #endregion

        #region ����
        /// <summary>
        /// У������
        /// </summary>
        /// <returns></returns>
        public int myValidDate()
        {
            if (this.dtDate.Value.Date < this.assignMgr.GetDateTimeFromSysDateTime().Date)
            {
                MessageBox.Show("��ά�����е����ڲ���С�ڵ�ǰ����");
                return -1;
            }
            return 1;
        }
        /// <summary>
        /// 1"�༭����"
        /// 2"ģ�崰��"
        /// </summary>
        /// <param name="strTag"></param>
        private void PopWin(string strTag)
        {
            try
            {
                if (this.myValidDate() < 0)
                {
                    return;
                }
                if (this.tabPage1.Tag == null)
                {
                    MessageBox.Show("��ѡ����!!");
                    return;
                }
                neuobj = this.tabPage1.Tag as Neusoft.FrameWork.Models.NeuObject;
                this.myResult.Queue.QueueDate = this.dtDate.Value;
                this.myResult.Queue.Dept.ID = neuobj.ID;
                this.myResult.Queue.Dept.Name = neuobj.Name;
                Neusoft.FrameWork.Models.NeuObject obj = (Neusoft.FrameWork.Models.NeuObject)this.tvPatientList1.SelectedNode.Tag;
                if (obj == null)
                {
                    MessageBox.Show("��ѡ����");
                    return;
                }
                this.myResult.Nurse = obj;
                if (strTag == "1")
                {
                    ucAddQueue ucaddQueue = new ucAddQueue(this.myResult);

                    this.myResult.QueueList = new List<Neusoft.HISFC.Models.Nurse.Queue>();
                    for (int i = 0, j = this.neuSpread1_Sheet1.RowCount; i < j; i++)
                    {
                        this.myResult.QueueList.Add(this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Nurse.Queue);
                    }
                    
                    ucaddQueue.RefList +=new ucAddQueue.ClickSave(ucQueue_RefList); 
                    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(ucaddQueue);

                    #region {B3E7633A-D9FB-492f-9D62-D2F7188D5643}
                    if (ucaddQueue.AddQueDiagResult == DialogResult.OK)
                    {
                        //���¼��ؽ����ϵ�ǰ�е�����
                        #region //��Ҫ������1��{7F5DF30E-AB9C-439f-A7A3-F0FEC05BBF2B}
                        if (this.myResult.strTab == "ADD")
                        {
                            this.neuSpread1_Sheet1.AddRows(0, 1);
                        }
                        #endregion
                        this.ReLoadFpData(this.neuSpread1_Sheet1.ActiveRowIndex, ucaddQueue.Queue);
                    }
                    #endregion
                }
                if (strTag == "2")
                {
                    ucQueueQuery uc = new ucQueueQuery(this.myResult.Queue.Dept);
                    uc.RefList += new Nurse.ucQueueQuery.RefQueue(uc_RefList);
                    Neusoft.FrameWork.WinForms.Classes.Function.PopShowControl(uc);

                }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void AddData()
        {
            if (this.btnRefresh.Tag != null && this.btnRefresh.Tag.ToString() == "SAVE")
            {
                MessageBox.Show("����������֮ǰ��Ҫ��֮ǰ��������ݱ��棡");
                return;
            }

            myResult = new cResult();
            myResult.Queue.IsValid = true;
            this.myResult.strTab = "ADD";
            this.PopWin("1");
        }

        /// <summary>
        /// ˢ�°�ť
        /// </summary>
        /// <param name="alQueue"></param>
        private void uc_RefList(ArrayList alQueue)
        {
            try
            {
                this.al = new ArrayList();
                this.al = alQueue;
                if (alQueue == null)
                    return;
                this.LoadFp(alQueue, "2");
                this.btnRefresh.Text = "�� ��";
                this.btnRefresh.Tag = "SAVE";
            }
            catch { }
        }

        private void ucQueue_RefList(Neusoft.FrameWork.Models.NeuObject obj)
        {
            this.RefreshList(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetNoonNameByID(string ID)
        {
            IDictionaryEnumerator dict = this.htNoon.GetEnumerator();
            while (dict.MoveNext())
            {
                if (dict.Key.ToString() == ID)
                    return dict.Value.ToString();
            }
            return ID;
        }

        private string GetDoctNameByID(string ID)
        {
            IDictionaryEnumerator dict = this.htDoct.GetEnumerator();
            while (dict.MoveNext())
            {
                if (dict.Key.ToString() == ID)
                    return dict.Value.ToString();
            }
            return ID;
        }

        /// <summary>
        /// ˢ��FarPoint�б�
        /// </summary>
        /// <param name="nurse"></param>
        private void RefreshList(Neusoft.FrameWork.Models.NeuObject nurse)
        {
            try
            {
                if (this.neuSpread1_Sheet1.RowCount > 0)
                    this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);

                //����������ά����Ϣ
                this.alNurse = this.myQueue.Query(nurse.ID, this.dtDate.Value.ToShortDateString());

                this.tabPage1.Text = nurse.Name;

                this.neuSpread1_Sheet1.Tag = nurse;

                if (alNurse != null)
                {
                    this.LoadFp(alNurse, "1");
                }
                this.SetFP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + this.myQueue.Err);
            }
        }

        /// <summary>
        /// �������ӵ�farpoint��
        /// </summary>
        /// <param name="al">���м���</param>
        /// <param name="strNew">����2��ʾ����ģ�壬1Ϊ�����ݿ���ȡ</param>
        private void LoadFp(ArrayList al, string strNew)
        {
            foreach (Neusoft.HISFC.Models.Nurse.Queue obj in al)
            {
                #region {4BC497AF-5A35-485f-9F5F-A58139E1BDFD}
                //��ģ�嵼������ݶ���������Ϊ����ѡ�������,�Է������ж��Ƿ��ظ�
                //����ʱIDҲ��Ϊ�գ�Ҫ��ɾ��ʱ�ᰴ�����ID�������ݿ�
                if (strNew == "2")
                {
                    obj.ID = "";
                    obj.QueueDate = this.dtDate.Value;
                }
                #endregion
                if (this.FilterExistQueue(obj))
                {
                    continue;
                }

                this.neuSpread1_Sheet1.Rows.Add(this.neuSpread1_Sheet1.RowCount, 1);
                int row = this.neuSpread1_Sheet1.RowCount - 1;
                this.neuSpread1_Sheet1.Rows[row].Tag = obj;

                //��������
                this.neuSpread1_Sheet1.SetValue(row, 0, obj.Name, false);
                //���
                obj.Noon.Name = this.GetNoonNameByID(obj.Noon.ID);
                this.neuSpread1_Sheet1.SetValue(row, 1, obj.Noon.Name, false);
                //��ʾ˳��
                this.neuSpread1_Sheet1.SetValue(row, 2, obj.Order, false);
                //�Ƿ���Ч
                this.neuSpread1_Sheet1.SetValue(row, 3, obj.IsValid ? "��Ч" : "��Ч", false);
                //��������

                //����2��ʾ����ģ�壬1Ϊ�����ݿ���ȡ
                if (strNew == "2")
                {
                    this.neuSpread1_Sheet1.SetValue(row, 4, this.dtDate.Value.ToString("yyyy-MM-dd"), false);
                }
                else
                {
                    this.neuSpread1_Sheet1.SetValue(row, 4, obj.QueueDate.ToString("yyyy-MM-dd"), false);
                }
                //�������
                this.neuSpread1_Sheet1.SetValue(row, 5, obj.AssignDept.Name);
                //����ҽ��
                obj.Doctor.Name = this.GetDoctNameByID(obj.Doctor.ID);
                this.neuSpread1_Sheet1.SetValue(row, 6, obj.Doctor.Name, false);
                //����
                this.neuSpread1_Sheet1.SetValue(row, 7, obj.SRoom.Name, false);
                //��̨
                this.neuSpread1_Sheet1.SetValue(row, 8, obj.Console.Name, false);
                //�Ƿ�ר��
                if (obj.ExpertFlag == "1")
                {
                    this.neuSpread1_Sheet1.SetValue(row, 9, "��", false);
                }
                else
                {
                    this.neuSpread1_Sheet1.SetValue(row, 9, "��", false);
                }
                //��ע
                this.neuSpread1_Sheet1.SetValue(row, 10, obj.Memo, false);
                //����Ա
                this.neuSpread1_Sheet1.SetValue(row, 11, obj.Oper.ID, false);
                //����ʱ��
                this.neuSpread1_Sheet1.SetValue(row, 12, this.myQueue.GetDateTimeFromSysDateTime(), false);
                //��������
                this.neuSpread1_Sheet1.SetValue(row, 13, obj.User01);
            }
            this.SetFP();
        }

        /// <summary>
        /// ��������Ѿ������򷵻�true
        /// </summary>
        /// <param name="queue">����ʵ��,���ڱȽ�</param>
        /// <returns>true,�Ѿ�����</returns>
        private bool FilterExistQueue(Neusoft.HISFC.Models.Nurse.Queue queue)
        {
            Neusoft.HISFC.Models.Nurse.Queue temp = null;

            for (int i = 0, j = this.neuSpread1_Sheet1.RowCount; i < j; i++)
            {
                temp = this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Nurse.Queue;
                if (queue.Noon.ID == temp.Noon.ID && 
                    queue.QueueDate.Date == temp.QueueDate.Date &&
                    queue.AssignDept.ID == temp.AssignDept.ID && 
                    queue.Doctor.ID == temp.Doctor.ID &&
                    queue.Console.ID == temp.Console.ID &&
                    queue.IsValid == temp.IsValid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �������ģ��
        /// </summary>
        /// <param name="alQueue"></param>
        private void SaveQueue(ArrayList alQueue)
        {
            //Neusoft.FrameWork.Management.Transaction tran = new Neusoft.FrameWork.Management.Transaction(this.myQueue.Connection);
            //��ʼ����
            try
            {
                //tran.BeginTransaction();

                Neusoft.FrameWork.Management.PublicTrans.BeginTransaction();

                this.myQueue.SetTrans(Neusoft.FrameWork.Management.PublicTrans.Trans);

                foreach (Neusoft.HISFC.Models.Nurse.Queue obj in alQueue)
                {
                    obj.Oper.OperTime = Neusoft.FrameWork.Function.NConvert.ToDateTime(this.myQueue.GetDateTimeFromSysDateTime());
                    obj.QueueDate = this.dtDate.Value;
                    if (this.myQueue.InsertQueue(obj) == -1)
                    {
                        MessageBox.Show("����ʧ��!!" + this.myQueue.Err);
                        Neusoft.FrameWork.Management.PublicTrans.RollBack();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Neusoft.FrameWork.Management.PublicTrans.RollBack();
                MessageBox.Show("����ʧ��!!" + ex.ToString());
            }
            Neusoft.FrameWork.Management.PublicTrans.Commit();
            MessageBox.Show("����ɹ�!!");
            this.btnRefresh.Text = "ˢ ��";
            this.btnRefresh.Tag = "REF";
            this.SetFP();
        }

        /// <summary>
        /// �޸�����
        /// </summary>
        private void EditData()
        {
            try
            {
                this.myResult = new cResult();
                int iRow = this.neuSpread1_Sheet1.ActiveRowIndex;
                this.SetEdit(iRow);
                this.myResult.strTab = "EDIT";
                this.PopWin("1");
            }
            catch { }
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        private void DelData()
        {
            try
            {
                int row = this.neuSpread1_Sheet1.ActiveRowIndex;
                if (row < 0 || this.neuSpread1_Sheet1.RowCount == 0) return;
                string strNo = ((Neusoft.HISFC.Models.Nurse.Queue)this.neuSpread1_Sheet1.Rows[row].Tag).ID;

                //������δ�����������ɾ��
                ArrayList alTemp = new ArrayList();
                alTemp = this.assignMgr.QueryByQueueCode(strNo);
                if (alTemp != null && alTemp.Count > 0)
                {
                    MessageBox.Show("�ö����л���δ������ߣ�����ɾ��!", "�����л���ȫ����������ɾ��!");
                    return;
                }
                if (MessageBox.Show("�Ƿ�Ҫɾ���ü�¼?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.No) return;

                //�Ѿ��������Ŀ,�����ݿ���ɾ��
                if (this.neuSpread1_Sheet1.Rows[row].Tag != null)
                {
                    if (this.myQueue.DelQueue(strNo) == -1)
                    {
                        MessageBox.Show("ɾ��ʧ��!!" + this.myQueue.Err);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("ɾ���ɹ�!");
                        if (this.tabPage1.Tag != null)
                        {
                            #region {58E77C58-5FC4-4b5e-A3DF-2BCB952B340E}
                            //�������ϵ����ݶ���δ����ģ�ɾ������ʱ��Ӧ��ˢ�½��棬����δ���������ȫ����ʧ

                            this.neuSpread1_Sheet1.Rows.Remove(row, 1);
                            //this.RefreshList(((Neusoft.FrameWork.Models.NeuObject)this.tabPage1.Tag));
                            #endregion
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + this.myQueue.Err);
            }
        }

        /// <summary>
        /// ��ʼ��������
        /// </summary>
        private void InitTree()
        {
            this.tvPatientList1.Nodes.Clear();

            TreeNode root = new TreeNode("��ʿվ");
            this.tvPatientList1.Nodes.Add(root);

            Neusoft.HISFC.Models.Base.Employee e = (Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator;
            //if (this.Tag != null && this.Tag.ToString() == "ALL")
            if( e.IsManager )
            {
                //��ȫ����ʿվ�б�(ȫ��Ȩ�޵�ά��)
                this.alNurse = cDept.GetDepartment(Neusoft.HISFC.Models.Base.EnumDepartmentType.N);
                if (alNurse != null)
                {
                    foreach (Neusoft.FrameWork.Models.NeuObject obj in alNurse)
                    {
                        TreeNode node = new TreeNode(obj.Name);
                        node.Tag = obj;
                        root.Nodes.Add(node);
                    }
                }
            }
            else
            {
                //ֻά���Լ�����վ----------------------------------
                //TreeNode node = new TreeNode(((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse.Name);
                //node.Tag = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Nurse;
                TreeNode node = new TreeNode(((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept.Name);
                node.Tag = ((Neusoft.HISFC.Models.Base.Employee)Neusoft.FrameWork.Management.Connection.Operator).Dept;
                root.Nodes.Add(node);
                //end-----------------------------------------------
            }
            root.Expand();
        }

        /// <summary>
        /// ���¼��ؽ���ĳһ�е�����
        /// {B3E7633A-D9FB-492f-9D62-D2F7188D5643}
        /// </summary>
        /// <param name="activeRow">�к�</param>
        /// <param name="obj">����ʵ��</param>
        private void ReLoadFpData(int activeRow,Neusoft.HISFC.Models.Nurse.Queue obj)
        {
            //��������
            this.neuSpread1_Sheet1.SetValue(activeRow, 0, obj.Name, false);
            //���
            obj.Noon.Name = this.GetNoonNameByID(obj.Noon.ID);
            this.neuSpread1_Sheet1.SetValue(activeRow, 1, obj.Noon.Name, false);
            //��ʾ˳��
            this.neuSpread1_Sheet1.SetValue(activeRow, 2, obj.Order, false);
            //�Ƿ���Ч
            this.neuSpread1_Sheet1.SetValue(activeRow, 3, obj.IsValid ? "��Ч" : "��Ч", false);
            //��������
            this.neuSpread1_Sheet1.SetValue(activeRow, 4, obj.QueueDate.ToString("yyyy-MM-dd"), false);
            
            //�������
            this.neuSpread1_Sheet1.SetValue(activeRow, 5, obj.AssignDept.Name);
            //����ҽ��
            obj.Doctor.Name = this.GetDoctNameByID(obj.Doctor.ID);
            this.neuSpread1_Sheet1.SetValue(activeRow, 6, obj.Doctor.Name, false);
            //����
            this.neuSpread1_Sheet1.SetValue(activeRow, 7, obj.SRoom.Name, false);
            //��̨
            this.neuSpread1_Sheet1.SetValue(activeRow, 8, obj.Console.Name, false);
            //�Ƿ�ר��
            if (obj.ExpertFlag == "1")
            {
                this.neuSpread1_Sheet1.SetValue(activeRow, 9, "��", false);
            }
            else
            {
                this.neuSpread1_Sheet1.SetValue(activeRow, 9, "��", false);
            }
            //��ע
            this.neuSpread1_Sheet1.SetValue(activeRow, 10, obj.Memo, false);
            //����Ա
            this.neuSpread1_Sheet1.SetValue(activeRow, 11, obj.Oper.ID, false);
            //����ʱ��
            this.neuSpread1_Sheet1.SetValue(activeRow, 12, this.myQueue.GetDateTimeFromSysDateTime(), false);
            //��������
            this.neuSpread1_Sheet1.SetValue(activeRow, 13, obj.User01);

            this.neuSpread1_Sheet1.Rows[activeRow].Tag = obj;
        }

        #endregion

        private void ucQueue_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitTree();

                this.Init();
            }
            catch { }
        }

        private void Init()
        {
            if (this.myMgr == null) this.myMgr = new Neusoft.HISFC.BizProcess.Integrate.Registration.Registration();
            al = this.myMgr.Query();
            foreach (Neusoft.HISFC.Models.Registration.Noon noon in al)
            {
                this.htNoon.Add(noon.ID, noon.Name);
            }

            if (this.personMgr == null) this.personMgr = new Neusoft.HISFC.BizProcess.Integrate.Manager();
            //�õ�ҽ���б�
            al = new ArrayList();
            al = this.personMgr.QueryEmployee(Neusoft.HISFC.Models.Base.EnumEmployeeType.D);
            foreach (Neusoft.HISFC.Models.Base.Employee person in al)
            {
                this.htDoct.Add(person.ID, person.Name);
            }
            this.btnRefresh.Text = "ˢ ��";
            this.btnRefresh.Tag = "REF";
            this.SetFP();
            this.dtDate.Value = this.myQueue.GetDateTimeFromSysDateTime();
        }

        private void SetRef()
        {
            this.btnRefresh.Text = "ˢ ��";
            this.btnRefresh.Tag = "REF";
        }
        private void SetSave()
        {
            this.btnRefresh.Text = "�� ��";
            this.btnRefresh.Tag = "SAVE";
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            int altKey = Keys.Alt.GetHashCode();

            //if (keyData == Keys.Add || keyData == Keys.Oemplus)
            //{
            //    this.AddData();
            //    return true;
            //}
            //else if (keyData == Keys.Subtract || keyData == Keys.OemMinus)
            //{
            //    this.DelData();
            //    return true;
            //}
            //if (keyData.GetHashCode() == altKey + Keys.E.GetHashCode())
            //{
            //    this.EditData();
            //    return true;
            //}
            //if (keyData.GetHashCode() == altKey + Keys.X.GetHashCode())
            //{
            //    this.FindForm().Close();
            //    return true;
            //}

            return base.ProcessDialogKey(keyData);
        }

        private void neuSpread1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (neuSpread1_Sheet1.Rows.Count == 0)
            {
                MessageBox.Show("û�����ݿ�ѡ���޸�");
                return;
            }
        }

        private void SetEdit(int iRow)
        {
            if (this.myResult == null) this.myResult = new cResult();
#region {231F627C-1215-4024-AE4A-6E06F7D20FC1}
            this.myResult.Queue = (this.neuSpread1_Sheet1.Rows[iRow].Tag as Neusoft.HISFC.Models.Nurse.Queue).Clone(); 
            #endregion
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (this.btnRefresh.Tag != null && this.btnRefresh.Tag.ToString() == "SAVE")
            {
                #region {B3E7633A-D9FB-492f-9D62-D2F7188D5643}
                ArrayList alSave = new ArrayList();
                for (int i = 0; i < this.neuSpread1_Sheet1.RowCount; i++)
                {
                    Neusoft.HISFC.Models.Nurse.Queue obj = new Neusoft.HISFC.Models.Nurse.Queue();
                    obj = this.neuSpread1_Sheet1.Rows[i].Tag as Neusoft.HISFC.Models.Nurse.Queue;
                    alSave.Add(obj);
                }
                this.SaveQueue(alSave);

                //this.SaveQueue(this.al);
                #endregion
            }

            if (this.tabPage1.Tag != null && this.btnRefresh.Tag.ToString() == "REF")
            {
                Neusoft.FrameWork.Models.NeuObject obj = new Neusoft.FrameWork.Models.NeuObject();
                obj = this.tabPage1.Tag as Neusoft.FrameWork.Models.NeuObject;
                this.RefreshList(obj);
            }
        }

        private void neuSpread1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            this.EditData();
        }

        private void SetFP()
        {
            this.neuSpread1_Sheet1.Columns[11].Visible = false;
            this.neuSpread1_Sheet1.Columns[12].Visible = false;
            this.neuSpread1_Sheet1.Columns[13].Visible = false;
        }

        private void tvPatientList1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode current = this.tvPatientList1.SelectedNode;

                if (current == null || current.Parent == null)
                {
                    if (this.neuSpread1_Sheet1.RowCount > 0)
                        this.neuSpread1_Sheet1.Rows.Remove(0, this.neuSpread1_Sheet1.RowCount);

                    this.neuSpread1_Sheet1.Tag = null;
                }
                else
                {
                    Neusoft.FrameWork.Models.NeuObject nurse = (Neusoft.FrameWork.Models.NeuObject)current.Tag;
                    this.tabPage1.Tag = nurse;
                    this.RefreshList(nurse);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


    }




    #region ��

    public class cResult
    {
        public string Result1 = "";
        public string Result2 = "";
        public string strTab = "";
        public ArrayList arrFee = new ArrayList();
        public Neusoft.FrameWork.Models.NeuObject Nurse = new Neusoft.FrameWork.Models.NeuObject();
        public Neusoft.HISFC.Models.Nurse.Queue Queue = new Neusoft.HISFC.Models.Nurse.Queue();
        public List<Neusoft.HISFC.Models.Nurse.Queue> QueueList = new List<Neusoft.HISFC.Models.Nurse.Queue>();
    }
    #endregion
}