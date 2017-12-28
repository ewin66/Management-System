namespace Neusoft.HISFC.Models.PhysicalExamination.Management 
{
	/// <summary>
	/// Department <br></br>
	/// [��������: ������ʵ��]<br></br>
	/// [�� �� ��: ��һ��]<br></br>
	/// [����ʱ��: 2006-11-10]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��=''
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [System.Serializable]
    public class Department : Neusoft.HISFC.Models.PhysicalExamination.Base.PE 
	{
		/// <summary>
		/// ���캯��
		/// </summary>
		public Department()
		{

		}

		#region ˽�б���
		
		/// <summary>
		/// ���ҵ��
		/// </summary>
		private Neusoft.HISFC.Models.PhysicalExamination.Base.PE business;

		#endregion

		#region ����

		/// <summary>
		/// ���ҵ��
		/// </summary>
		public Neusoft.HISFC.Models.PhysicalExamination.Base.PE Business 
		{
			get 
			{
				return this.business;
			}
			set 
			{
				this.business = value;
			}
		}

		#endregion

		#region ����
		
		/// <summary>
		/// ��¡
		/// </summary>
		/// <returns>��������</returns>
		public new Department Clone()
		{
			Department department = base.Clone() as Department;

			department.Business = this.Business.Clone();

			return department;
		}
		#endregion
	}
}