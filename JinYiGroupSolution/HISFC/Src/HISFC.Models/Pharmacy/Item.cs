using System;
 
namespace Neusoft.HISFC.Models.Pharmacy
{
	/// <summary>
	/// [��������: ҩƷ������Ϣ]<br></br>
	/// [�� �� ��: ������]<br></br>
	/// [����ʱ��: 2006-09-11]<br></br>
	/// <�޸ļ�¼
	///		�޸���=''
	///		�޸�ʱ��='yyyy-mm-dd'
	///		�޸�Ŀ��=''
	///		�޸�����=''
	///  />
	/// </summary>
    [System.ComponentModel.DisplayName("ҩƷ�ֵ���Ϣ")]
    [Serializable]
	public class Item : Neusoft.HISFC.Models.Base.Item,Neusoft.HISFC.Models.Base.IValidState
	{
		public Item()
		{
			//this.IsPharmacy = true;
            this.ItemType = Neusoft.HISFC.Models.Base.EnumItemType.Drug;
		}


		#region ����

		/// <summary>
		/// ����������Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.Base.OperEnvironment oper = new Neusoft.HISFC.Models.Base.OperEnvironment();

		/// <summary>
		/// ��Ŀ������Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.IMA.NameService nameCollection = new Neusoft.HISFC.Models.IMA.NameService();	

		/// <summary>
		/// �۸���Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.IMA.PriceService priceCollection = new Neusoft.HISFC.Models.IMA.PriceService();
			
		/// <summary>
		/// ��Ʒ��Ϣ
		/// </summary>
		private Neusoft.HISFC.Models.Pharmacy.Base.ProductService product = new Neusoft.HISFC.Models.Pharmacy.Base.ProductService();

		/// <summary>
		/// ����Ļ��ʾ
		/// </summary>
		private bool isShow;

		/// <summary>
		/// ��ʾ���� 0 ȫԺ 1 סԺ  2 ����
		/// </summary>
		private string showState;
		
		/// <summary>
		/// �Ƿ�ͣ��
		/// </summary>
		private bool isStop;	
		
		/// <summary>
		/// �Ƿ�GMP
		/// </summary>
		private bool isGMP;

		/// <summary>
		/// �Ƿ�OTC
		/// </summary>
		private bool isOTC;

		/// <summary>
		/// �Ƿ���ҩ
		/// </summary>
		private bool isNew;

		/// <summary>
		/// �Ƿ�ȱҩ
		/// </summary>
		private bool isLack;

		/// <summary>
		/// �Ƿ���Ҫ����
		/// </summary>
		private bool isAllergy;

		/// <summary>
		/// �Ƿ񸽲�
		/// </summary>
		private bool isSubtbl;

		/// <summary>
		/// ��Ч�ɷ�
		/// </summary>
		private string ingredient;

		/// <summary>
		/// ��ҩִ�б�׼
		/// </summary>
		private string executeStandard;

		/// <summary>
		/// �б���Ϣ��
		/// </summary>
		private TenderOffer tenderOffer = new TenderOffer();

		/// <summary>
		/// �䶯����
		/// </summary>
		private ItemShiftType shiftType = new ItemShiftType();

		/// <summary>
		/// �䶯ʱ��
		/// </summary>
		private DateTime shiftTime;

		/// <summary>
		/// �䶯ԭ��
		/// </summary>
		private string shiftMark;

		/// <summary>
		/// ��ϵͳҩƷ����
		/// </summary>
		private string oldDrugID;

		/// <summary>
		/// ������� 0 �ɲ��װ��λ 1 ���ܲ��װ��λ
		/// </summary>
		private string splitType = "1";

        /// <summary>
        /// ��Ч��
        /// </summary>
        private Neusoft.HISFC.Models.Base.EnumValidState validState = Neusoft.HISFC.Models.Base.EnumValidState.Valid;

        /// <summary>
        /// �Ƿ�Э������ҩ
        /// </summary>
        private bool isNostrum = false;

		#endregion

		#region ҩƷʹ����Ϣ����

		/// <summary>
		/// ��װ��λ
		/// </summary>
		private string packUnit;

		/// <summary>
		/// ��С��λ
		/// </summary>
		private string minUnit;

		/// <summary>
		/// ��������
		/// </summary>
		private decimal baseDose;

		/// <summary>
		/// ������λ
		/// </summary>
		private string doseUnit;

		/// <summary>
		/// һ�μ���
		/// </summary>
		private decimal onceDose;

		/// <summary>
		/// ����
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject dosageForm = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ��� ��ҩ����ҩ��
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject type = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ҩƷ����
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject quality = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ʹ�÷���
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject usage = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// Ƶ��
		/// </summary>
		private Neusoft.HISFC.Models.Order.Frequency frequency = new Neusoft.HISFC.Models.Order.Frequency();

		/// <summary>
		/// ҩ������1
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject phyFunction1 = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ҩ������2
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject phyFunction2 = new Neusoft.FrameWork.Models.NeuObject();

		/// <summary>
		/// ҩ������3
		/// </summary>
		private Neusoft.FrameWork.Models.NeuObject phyFunction3 = new Neusoft.FrameWork.Models.NeuObject();

		#endregion

        #region ҩƷ��չ��Ϣ����

        /// <summary>
        /// ��չ����1
        /// </summary>
        private string extendData1;

        /// <summary>
       /// ��չ����2
       /// </summary>
        private string extendData2;

        /// <summary>
        /// �ֵ佨��ʱ��
        /// </summary>
        private DateTime createTime;

        #endregion

        /// <summary>
		/// ��Ŀ����
		/// </summary>
		public new string ID
		{
			get
			{
				return base.ID;
			}
			set
			{
				base.ID = value;
				this.nameCollection.ID = value;
				this.priceCollection.ID = value;
				this.product.ID = value;
			}
		}


		/// <summary>
		/// ��Ŀ����
		/// </summary>
        [System.ComponentModel.DisplayName("ҩƷ����")]
        [System.ComponentModel.Description("ҩƷ��Ʒ����")]
		public new string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
				this.nameCollection.Name = value;
				this.priceCollection.Name = value;
				this.product.Name = value;
			}
		}
		

		/// <summary>
		/// ��Ŀ������Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.IMA.NameService NameCollection
		{
			get
			{
				return this.nameCollection;
			}
			set
			{
				this.nameCollection = value;
			}
		}

        /// <summary>
        /// �۸� (���ۼ�)
        /// </summary>
        public new decimal Price
        {
            get
            {
                if (this.priceCollection.RetailPrice != 0)
                {
                    base.Price = this.priceCollection.RetailPrice;
                    return this.priceCollection.RetailPrice;
                }
                else
                {
                    return base.Price;
                }
            }
            set
            {
                this.priceCollection.RetailPrice = value;
                base.Price = value;
            }
        }

        /// <summary>
        /// ƴ����
        /// </summary>
        [System.ComponentModel.DisplayName("ƴ����")]
        [System.ComponentModel.Description("ҩƷ��Ʒ���Ƶ�ƴ����")]
        public new string SpellCode
        {
            get
            {
                return this.nameCollection.SpellCode;
            }
            set
            {
                base.SpellCode = value;
                this.nameCollection.SpellCode = value;
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        [System.ComponentModel.DisplayName("�����")]
        [System.ComponentModel.Description("ҩƷ��Ʒ���Ƶ������")]
        public new string WBCode
        {
            get
            {
                return this.nameCollection.WBCode;
            }
            set
            {
                base.WBCode = value;
                this.nameCollection.WBCode = value;
            }
        }

        /// <summary>
        /// �Զ�����
        /// </summary>
        [System.ComponentModel.DisplayName("�Զ�����")]
        [System.ComponentModel.Description("ҩƷ��Ʒ���Ƶ��Զ�����")]
        public new string UserCode
        {
            get
            {
                return this.nameCollection.UserCode;
            }
            set
            {
                base.UserCode = value;
                this.nameCollection.UserCode = value;
            }
        }

        /// <summary>
        /// ���ұ���
        /// </summary>
        [System.ComponentModel.DisplayName("���ұ���")]
        [System.ComponentModel.Description("ҩƷ���ұ���")]
        public new string GBCode
        {
            get
            {
                return this.nameCollection.GbCode;
            }
            set
            {
                base.GBCode = value;
                this.nameCollection.GbCode = value;
            }
        }

        /// <summary>
        /// ���ʱ���
        /// </summary>
        [System.ComponentModel.DisplayName("���ʱ���")]
        [System.ComponentModel.Description("ҩƷ���ʱ���")]
        public new string NationCode
        {
            get
            {
                return this.nameCollection.InternationalCode;
            }
            set
            {
                base.NationCode = value;
                this.nameCollection.InternationalCode = value;
            }
        }		

		/// <summary>
		/// ��װ��λ
		/// </summary>
        [System.ComponentModel.DisplayName("��װ��λ")]
        [System.ComponentModel.Description("ҩƷ��װ��λ")]
		public string PackUnit
		{
			get
			{
				return this.packUnit;
			}
			set
			{
				this.packUnit = value;
                base.PriceUnit = value;
			}
		}
		

		/// <summary>
		/// ��С��λ
		/// </summary>
        [System.ComponentModel.DisplayName("��С��λ")]
        [System.ComponentModel.Description("ҩƷ��С��λ")]
		public string MinUnit
		{
			get
			{
				return this.minUnit;
			}
			set
			{
				this.minUnit = value;
			}
		}
		

		/// <summary>
		/// ��������
		/// </summary>
        [System.ComponentModel.DisplayName("��������")]
        [System.ComponentModel.Description("ҩƷ��������")]
		public decimal BaseDose
		{
			get
			{
				return this.baseDose;
			}
			set
			{
				this.baseDose = value;
			}
		}
		

		/// <summary>
		/// ������λ
		/// </summary>
        [System.ComponentModel.DisplayName("������λ")]
        [System.ComponentModel.Description("ҩƷ������λ")]
		public string DoseUnit
		{
			get
			{
				return this.doseUnit;
			}
			set
			{
				this.doseUnit = value;
			}
		}
		

		/// <summary>
		/// һ������(����)
		/// </summary>
        [System.ComponentModel.DisplayName("ÿ�μ���")]
        [System.ComponentModel.Description("ҩƷÿ�μ���")]
		public decimal OnceDose
		{
			get
			{
				return this.onceDose;
			}
			set
			{
				this.onceDose = value;
			}
		}


		/// <summary>
		/// ����
		/// </summary>
        [System.ComponentModel.DisplayName("����")]
        [System.ComponentModel.Description("ҩƷ����")]
		public Neusoft.FrameWork.Models.NeuObject DosageForm
		{
			get
			{
				return this.dosageForm;
			}
			set
			{
				this.dosageForm = value;
			}
		}
		

		/// <summary>
		/// ��� ��ҩ����ҩ��
		/// </summary>
        [System.ComponentModel.DisplayName("���")]
        [System.ComponentModel.Description("ҩƷ���")]
		public Neusoft.FrameWork.Models.NeuObject Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}
		

		/// <summary>
		/// ���� �գ��� 
		/// </summary>
        [System.ComponentModel.DisplayName("����")]
        [System.ComponentModel.Description("ҩƷ����")]
		public Neusoft.FrameWork.Models.NeuObject Quality
		{
			get
			{
				return this.quality;
			}
			set
			{
				this.quality = value;
			}
		}
		

		/// <summary>
		/// ʹ�÷���
		/// </summary>
        [System.ComponentModel.DisplayName("ʹ�÷���")]
        [System.ComponentModel.Description("ҩƷʹ�÷���")]
		public Neusoft.FrameWork.Models.NeuObject Usage
		{
			get
			{
				return this.usage;
			}
			set
			{
				this.usage = value;
			}
		}


		/// <summary>
		/// Ƶ��
		/// </summary>
        [System.ComponentModel.DisplayName("Ƶ��")]
        [System.ComponentModel.Description("ҩƷƵ��")]
		public Neusoft.HISFC.Models.Order.Frequency Frequency
		{
			get
			{
				return this.frequency;
			}
			set
			{
				this.frequency = value;
			}
		}
		

		/// <summary>
		/// һ��ҩ������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject PhyFunction1
		{
			get
			{
				return this.phyFunction1;
			}
			set
			{
				this.phyFunction1 = value;
			}
		}
		

		/// <summary>
		/// ����ҩ������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject PhyFunction2
		{
			get
			{
				return this.phyFunction2;
			}
			set
			{
				this.phyFunction2 = value;
			}
		}
		

		/// <summary>
		/// ����ҩ������
		/// </summary>
		public Neusoft.FrameWork.Models.NeuObject PhyFunction3
		{
			get
			{
				return this.phyFunction3;
			}
			set
			{
				this.phyFunction3 = value;
			}
		}
		

		/// <summary>
		/// �۸���Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.IMA.PriceService PriceCollection
		{
			get
			{
				return this.priceCollection;
			}
			set
			{
				this.priceCollection = value;
                base.Price = value.RetailPrice;
			}
		}


		/// <summary>
		/// ��Ʒ��Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.Pharmacy.Base.ProductService Product
		{
			get
			{
				return this.product;
			}
			set
			{
				this.product = value;
			}
		}
		

		/// <summary>
		/// �Ƿ�ͣ��
		/// </summary>
        [System.ComponentModel.DisplayName("�Ƿ�ͣ��")]
        [System.ComponentModel.Description("ҩƷ�Ƿ�ͣ��")]
        [Obsolete("�����Բ������ݿ��ڻ�ȡ",false)]
		public bool IsStop
		{
			get
			{
				return this.isStop;
			}
			set
			{
				this.isStop = value;

                if (value)
                {
                    this.validState = Neusoft.HISFC.Models.Base.EnumValidState.Invalid;
                }
                else
                {
                    this.validState = Neusoft.HISFC.Models.Base.EnumValidState.Valid;
                }               
			}
		}

        #region IValidState ��Ա

        public new Neusoft.HISFC.Models.Base.EnumValidState ValidState
        {
            get
            {
                return this.validState;
            }
            set
            {
                if (value == Neusoft.HISFC.Models.Base.EnumValidState.Valid)
                {
                    this.isStop = false;
                }
                else
                {
                    this.isStop = true;
                }

                this.validState = value;
            }
        }

        #endregion
		

		/// <summary>
		/// �Ƿ�GMP
		/// </summary>
        [System.ComponentModel.DisplayName("�Ƿ�GMP")]
        [System.ComponentModel.Description("ҩƷ�Ƿ�GMP")]
		public bool IsGMP
		{
			get
			{
				return this.isGMP;
			}
			set
			{
				this.isGMP = value;
			}
		}
		

		/// <summary>
		/// �Ƿ�OTC
		/// </summary>
        [System.ComponentModel.DisplayName("�Ƿ�OTC")]
        [System.ComponentModel.Description("ҩƷ�Ƿ�OTC")]
		public bool IsOTC
		{
			get
			{
				return this.isOTC;
			}
			set
			{
				this.isOTC = value;
			}
		}
		

		/// <summary>
		/// �Ƿ���ҩ
		/// </summary>
		public bool IsNew
		{
			get
			{
				return this.isNew;
			}
			set
			{
				this.isNew = value;
			}
		}
		

		/// <summary>
		/// �Ƿ�ȱҩ
		/// </summary>
		public bool IsLack
		{
			get
			{
				return this.isLack;
			}
			set
			{
				this.isLack = value;
			}
		}


		/// <summary>
		/// ����Ļ��ʾ
		/// </summary>
		public bool IsShow
		{
			get
			{
				return this.isShow;
			}
			set
			{
				this.isShow = value;
			}
		}


		/// <summary>
		/// ��ʾ���� 0 ȫԺ 1 סԺ  2 ����
		/// </summary>
		public string ShowState
		{
			get
			{
				return this.showState;
			}
			set
			{
				this.showState = value;
			}
		}


		/// <summary>
		/// �Ƿ���Ҫ����
		/// </summary>
        [System.ComponentModel.DisplayName("�Ƿ���Ҫ����")]
        [System.ComponentModel.Description("ҩƷ�Ƿ���Ҫ���� ������ʾҽ��")]
		public bool IsAllergy
		{
			get
			{
				return this.isAllergy;
			}
			set
			{
				this.isAllergy = value;
			}
		}
		

		/// <summary>
		/// �Ƿ񸽲�
		/// </summary>
		public bool IsSubtbl
		{
			get
			{
				return this.isSubtbl;
			}
			set
			{
				this.isSubtbl = value;
			}
		}
		
		
		/// <summary>
		/// ��Ч�ɷ�
		/// </summary>
		public string Ingredient
		{
			get
			{
				return this.ingredient;
			}
			set
			{
				this.ingredient = value;
			}
		}


		/// <summary>
		/// ��ҩִ�б�׼
		/// </summary>
		public string ExecuteStandard
		{
			get
			{
				return this.executeStandard;
			}
			set
			{
				this.executeStandard = value;
			}
		}
		
		
		/// <summary>
		/// �б���Ϣ��
		/// </summary>
		public TenderOffer TenderOffer
		{
			get
			{
				return this.tenderOffer;
			}
			set
			{
				this.tenderOffer = value;
			}
		}


		/// <summary>
		/// �䶯����
		/// </summary>
		public ItemShiftType ShiftType
		{
			get
			{
				return this.shiftType;
			}
			set
			{
				this.shiftType = value;
			}
		}


		/// <summary>
		/// �䶯ʱ��
		/// </summary>
		public DateTime ShiftTime
		{
			get
			{
				return this.shiftTime;
			}
			set
			{
				this.shiftTime = value;
			}
		}


		/// <summary>
		/// �䶯ԭ��
		/// </summary>
		public string ShiftMark
		{
			get
			{
				return this.shiftMark;
			}
			set
			{
				this.shiftMark = value;
			}
		}


		/// <summary>
		/// ��ϵͳҩƷ����
		/// </summary>
		public string OldDrugID
		{
			get
			{
				return this.oldDrugID;
			}
			set
			{
				this.oldDrugID = value;
			}
		}


		/// <summary>
		/// ������� 0 �ɲ��װ��λ 1 ���ܲ��װ��λ
		/// </summary>
        [System.ComponentModel.DisplayName("�������")]
        [System.ComponentModel.Description("ҩƷ������� �������Ｐ��Ժ��ҩ��Ч")]
		public string SplitType 
		{
			get
			{
				return this.splitType;
			}
			set
			{
				this.splitType = value;
			}
		}

        /// <summary>
        /// ҩƷ�ȼ�
        /// </summary>
        [System.ComponentModel.DisplayName("ҩƷ�ȼ�")]
        [System.ComponentModel.Description("ҩƷ�ȼ� ��ҽ��ְ�����")]
        public new string Grade
        {
            get
            {
                return base.Grade;
            }
            set
            {
                base.Grade = value;
            }
        }


		/// <summary>
		/// ����������Ϣ
		/// </summary>
		public Neusoft.HISFC.Models.Base.OperEnvironment Oper
		{
			get
			{
				return this.oper;
			}
			set
			{
				this.oper = value;
			}
		}

        /// <summary>
        /// �Ƿ�Э������ҩ
        /// </summary>
        [System.ComponentModel.DisplayName("Э������ҩ���")]
        [System.ComponentModel.Description("Э������ҩ���")]
        public bool IsNostrum
        {
            get
            {
                return this.isNostrum;
            }
            set
            {
                this.isNostrum = value;
            }
        }

        /// <summary>
        /// ��չ����1     {8ADD2D48-2427-48aa-A521-4B17EECBC8B4}
        /// </summary>
        public string ExtendData1
        {
            get
            {
                return this.extendData1;
            }
            set
            {
                this.extendData1 = value;
            }
        }

        /// <summary>
        /// ��չ����2     {8ADD2D48-2427-48aa-A521-4B17EECBC8B4}
        /// </summary>
        public string ExtendData2
        {
            get
            {
                return this.extendData2;
            }
            set
            {
                this.extendData2 = value;
            }
        }

        /// <summary>
        /// �ֵ佨��ʱ��  {8ADD2D48-2427-48aa-A521-4B17EECBC8B4}
        /// </summary>
        public DateTime CreateTime
        {
            get
            {
                return this.createTime;
            }
            set
            {
                this.createTime = value;
            }
        }

		#region ����

		/// <summary>
		/// ������¡
		/// </summary>
		/// <returns>�ɹ����ؿ�¡���Itemʵ�� ʧ�ܷ���null</returns>
		public new Item Clone()
		{
			Item item = base.Clone() as Item;

			item.NameCollection = this.NameCollection.Clone();
			item.DosageForm = this.DosageForm.Clone();
			item.Type = this.Type.Clone();
			item.Quality = this.Quality.Clone();
			item.Usage = this.Usage.Clone();
			item.Frequency = this.Frequency.Clone();
			item.PhyFunction1 = this.PhyFunction1.Clone();
			item.PhyFunction2 = this.PhyFunction2.Clone();
			item.PhyFunction3 = this.PhyFunction3.Clone();
			item.PriceCollection = this.PriceCollection.Clone();
			item.Product = this.Product.Clone();	
			item.TenderOffer = this.TenderOffer.Clone();
			item.ShiftType = this.ShiftType.Clone();

			return item;
		}

		#endregion
    }
}