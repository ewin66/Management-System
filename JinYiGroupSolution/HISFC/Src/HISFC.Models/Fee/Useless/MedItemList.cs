using System;
using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Models.Fee
{
	/// <summary>
	/// MedItemList ��ժҪ˵����
	/// ҩƷ������Ϣ��
	/// </summary>
    /// 
    [System.Serializable]
	public class MedItemList:Neusoft.HISFC.Models.Base.Item
	{
		public MedItemList()
		{
			//
			// TODO: �ڴ˴����ӹ��캯���߼�
			//
		}
//		/// <summary>
//		/// ������Ϣ
//		/// </summary>
		//public FeeInfo FeeInfo=new FeeInfo();
		/// <summary>
		/// ��Ŀ��Ϣ
		/// </summary>
        public Models.Base.Item Item = new Neusoft.HISFC.Models.Base.Item();
		/// <summary>
		/// ��������ˮ��
		/// </summary>
		public int SequenceNo;
		/// <summary>
		/// ���¿����ˮ��
		/// </summary>
		public int UpdateSequenceNo;
		/// <summary>
		/// ��ҩ�����к�
		/// </summary>
		public int SendSequenceNo;
		/// <summary>
		/// 0����1�շ�2��ҩ
		/// </summary>
		public string SendState;
		/// <summary>
		/// �������ȱ��
		/// </summary>
		public bool IsEmergency;
		/// <summary>
		/// �Ƿ��Ժ��ҩ
		/// </summary>
		public bool IsBrought;
		/// <summary>
		/// ��Ŀ���ı���(ҽ����)
		/// </summary>
		public string CenterCode;
		/// <summary>
		/// ������(ҽ����)
		/// </summary>
		public string ApprNo;
		/// <summary>
		/// ҽ����ˮ��
		/// </summary>
		public string MoOrder;
		/// <summary>
		/// ҽ��ִ�е���ˮ��
		/// </summary>
		public string MoExecSqn;
		/// <summary>
		/// ��ҩ��
		/// </summary>
		public NeuObject SendDrugOper = new NeuObject();
		/// <summary>
		/// ��ҩ����
		/// </summary>
		public DateTime DtSendDrug;
		/// <summary>
		/// �Ƿ�����
		/// </summary>
        public bool IsMadeSelf;
		/// <summary>
		/// ҩƷ���
		/// </summary>
		public NeuObject DrugType = new NeuObject();
		/// <summary>
		/// ҩƷ����
		/// </summary>
		public string DrugQuality;
		/// <summary>
		/// �����
		/// </summary>
		public NeuObject CheckOper = new NeuObject();


	}
}