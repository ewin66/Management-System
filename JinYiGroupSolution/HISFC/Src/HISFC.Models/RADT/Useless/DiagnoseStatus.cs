using Neusoft.FrameWork.Models;

namespace Neusoft.HISFC.Models.RADT
{
	/// <summary>
	/// [��������: ���״̬ʵ��]<br></br>
	/// [�� �� ��: ����ΰ]<br></br>
	/// [����ʱ��: 2006-09-05]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary> 
    [System.Serializable]
    public class DiagnoseStatus:NeuObject 
	{
		public DiagnoseStatus()
		{
			//
			// TODO: �ڴ˴����ӹ��캯���߼�
			//
		}
		public new DiagnoseStatus Clone()
		{
			return base.Clone() as DiagnoseStatus;
		}
	}
}