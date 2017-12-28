using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Neusoft.DefultInterfacesAchieve
{
    public class AlterOrderAchieve : Neusoft.HISFC.BizProcess.Interface.IAlterOrder
    {

        #region ����
        /// <summary>
        /// ҩƷ����ҵ���
        /// </summary>
        Neusoft.HISFC.BizLogic.Pharmacy.Item phaItemMgr = new Neusoft.HISFC.BizLogic.Pharmacy.Item();

        /// <summary>
        /// ����������
        /// {DB30AC55-99D4-4250-AF2A-A9AC40370B67}
        /// </summary>
        private Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam ctrlMgr = new Neusoft.HISFC.BizProcess.Integrate.Common.ControlParam();

        #endregion

        #region IAlterOrder ��Ա

        public int AlterOrder(Neusoft.HISFC.Models.Registration.Register patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref Neusoft.HISFC.Models.Order.OutPatient.Order order)
        {
            if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
            {
                Neusoft.HISFC.Models.Pharmacy.Item drugItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                if (drugItem == null)
                {
                    return 0;
                }

                #region �༶��λ(��С��ҩϵ��)
                if (order.Item.Qty == 0 || string.IsNullOrEmpty(order.Item.ID))
                {
                    return 0;
                }
                if (order.Item.SysClass.ID.ToString() == Neusoft.HISFC.Models.Base.EnumSysClass.PCC.ToString())
                {//��ҩ�㷨�����ֳ��汾�����������
                    return 0;
                }
                decimal totQty;
                decimal resultTotQty;
                decimal packQty = drugItem.PackQty;

                if(order.Nurse.User03 == "0")
                {//��װ��λ
                    totQty = order.Qty * drugItem.PackQty;
                }
                else
                {//��С��λ
                    totQty = order.Qty;
                }

                this.phaItemMgr.QuerySpeUnitForClinic(drugItem, totQty, out resultTotQty);

                if (order.NurseStation.User03 == "0")
                {//��װ��λ
                    totQty = System.Math.Ceiling(resultTotQty / packQty); //����װ��ȡ��
                }
                else
                {//��С��λ
                    totQty = System.Math.Ceiling(resultTotQty);
                }
                if (order.Qty != totQty)
                {
                    if (MessageBox.Show(order.Item.Name + "����С��ҩ��Ϊ" + totQty + ",�Ƿ��޸ģ�", "ҩ����С��ҩ��", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        order.Qty = totQty;
                    }
                    else
                    {//Ŀǰ�˴���û��ǿ���޸�
                    }
                }
                #endregion

            }
    
            if (order.Item.Memo == "")
            {
                //string practicableSymptomText = "";
                //int returnValue = this.QueryItemByOutpatOrder(patient, order, ref practicableSymptomText);
                //switch (returnValue)
                //{
                //    case 0: //û��ά��
                //        {
                //            //������

                //            break;
                //        }
                //    case 1: //��ά�� 
                //        {
                //            DialogResult d = System.Windows.Forms.MessageBox.Show("����Ŀ����Ӧ֢����ά����\n" + practicableSymptomText + "\n" + "�Ƿ�ѡ������Ӧ֢�շ�", "��ʾ", MessageBoxButtons.YesNoCancel);
                //            if (d == DialogResult.Cancel)
                //            {
                //                order.Item.Memo = "0";
                //                return 1;
                //            }
                //            else if (d == DialogResult.Yes)
                //            {

                //                //�Ƿ���Ӧ֢��Ϊ1 ����order.Item.Memo
                //                order.Item.Memo = "1";
                //                return 1;
                //            }
                //            else
                //            {
                //                order.Item.Memo = "0";
                //                //������  
                //            }

                //            break;
                //        }
                //    case -1: //����
                //        {
                //            break;
                //        }

                //    default:
                //        break;
                //}
                return 1;
            }
            else
            {
                return 1;
            }
        }

        public int AlterOrder(Neusoft.HISFC.Models.Registration.Register patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.OutPatient.Order> orderList)
        {
            //{DB30AC55-99D4-4250-AF2A-A9AC40370B67}
            bool isHaveDrug = false;

            foreach (Neusoft.HISFC.Models.Order.OutPatient.Order order in orderList)
            {
                if (order.Item.ItemType == Neusoft.HISFC.Models.Base.EnumItemType.Drug)
                {
                    Neusoft.HISFC.Models.Pharmacy.Item drugItem = order.Item as Neusoft.HISFC.Models.Pharmacy.Item;
                    if (drugItem == null)
                    {
                        return 0;
                    }

                    #region �༶��λ(��С��ҩϵ��)
                    if (order.Item.Qty == 0 || string.IsNullOrEmpty(order.Item.ID))
                    {
                        return 0;
                    }
                    if (order.Item.SysClass.ID.ToString() == Neusoft.HISFC.Models.Base.EnumSysClass.PCC.ToString())
                    {//��ҩ�㷨�����ֳ��汾�����������
                        return 0;
                    }
                    decimal totQty;
                    decimal resultTotQty;
                    decimal packQty = drugItem.PackQty;

                    if (order.Nurse.User03 == "0")
                    {//��װ��λ
                        totQty = order.Qty * drugItem.PackQty;
                    }
                    else
                    {//��С��λ
                        totQty = order.Qty;
                    }

                    this.phaItemMgr.QuerySpeUnitForClinic(drugItem, totQty, out resultTotQty);

                    if (order.NurseStation.User03 == "0")
                    {//��װ��λ
                        totQty = System.Math.Ceiling(resultTotQty / packQty); //����װ��ȡ��
                    }
                    else
                    {//��С��λ
                        totQty = System.Math.Ceiling(resultTotQty);
                    }
                    if (order.Qty != totQty)
                    {
                        if (MessageBox.Show(order.Item.Name + "����С��ҩ��Ϊ" + totQty + ",�Ƿ������", "ҩ����С��ҩ��", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    #endregion

                    isHaveDrug = true;

                }
            }

            #region {DB30AC55-99D4-4250-AF2A-A9AC40370B67}

            if (isHaveDrug)
            {
                bool isJudgeDiagnose = this.ctrlMgr.GetControlParam<bool>("200302", false, false);
                if (isJudgeDiagnose)
                {
                    Neusoft.HISFC.BizProcess.Integrate.HealthRecord.HealthRecordBase diagnoseMgr = new Neusoft.HISFC.BizProcess.Integrate.HealthRecord.HealthRecordBase();

                    System.Collections.ArrayList alDiagnose = diagnoseMgr.QueryDiagnoseNoOps(patient.ID);

                    if (alDiagnose == null || alDiagnose.Count == 0)
                    {
                        MessageBox.Show("�û��߻�û��¼����ϣ�");
                        return -1;
                    }
                }
            }

            #endregion

            //Function.SISpecialLimit myManager = new Neusoft.DefultInterfacesAchieve.Function.SISpecialLimit();

            //foreach (Neusoft.HISFC.Models.Order.OutPatient.Order order in orderList)
            //{
            //    if (order.Item.Memo == "1")
            //    {
            //        //ɾ���м��

            //        myManager.DeleteOutpatOrder(order);

            //        //�����м��
            //        int iReturn = myManager.InsertOutpatOrder(order);

            //        if (iReturn < 0)
            //        {

            //            MessageBox.Show("������Ӧ֢����!" + myManager.Err);
            //            return -1;
            //        }

            //    }

            //}
            return 1;
        }

        public int AlterOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref Neusoft.HISFC.Models.Order.Inpatient.Order order)
        {
            if (order.Item.Memo == "")
            {
                //string practicableSymptomText = "";
                //int returnValue = this.QueryItemByInpatOrder(patient, order, ref practicableSymptomText);
                //switch (returnValue)
                //{
                //    case 0: //û��ά��
                //        {
                //            //������

                //            break;
                //        }
                //    case 1: //��ά�� 
                //        {
                //            DialogResult d = System.Windows.Forms.MessageBox.Show("����Ŀ����Ӧ֢����ά����\n" + practicableSymptomText + "\n" + "�Ƿ�ѡ������Ӧ֢�շ�", "��ʾ", MessageBoxButtons.YesNoCancel);
                //            if (d == DialogResult.Cancel)
                //            {
                //                order.Item.Memo = "0";
                //                return 1;
                //            }
                //            else if (d == DialogResult.Yes)
                //            {

                //                //�Ƿ���Ӧ֢��Ϊ1 ����order.Item.Memo
                //                order.Item.Memo = "1";
                //                return 1;
                //            }
                //            else
                //            {
                //                order.Item.Memo = "0";
                //                //������  
                //            }

                //            break;
                //        }
                //    case -1: //����
                //        {
                //            break;
                //        }

                //    default:
                //        break;
                //}
                return 1;
            }
            else
            {
                return 1;
            }
        }

        public int AlterOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            //Function.SISpecialLimit myManager = new Neusoft.DefultInterfacesAchieve.Function.SISpecialLimit();

            //foreach (Neusoft.HISFC.Models.Order.Inpatient.Order order in orderList)
            //{
            //    if (order.Item.Memo == "1")
            //    {
            //        //ɾ���м��

            //        myManager.DeleteOrder(order);

            //        //�����м��
            //        int iReturn = myManager.InsertOrder(order);

            //        if (iReturn < 0)
            //        {

            //            MessageBox.Show("������Ӧ֢����!" + myManager.Err);
            //            return -1;
            //        }
                    
            //    }
                
            //}
            return 1;
        }

        public int AlterOrderOnSaved(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            return 1;
        }

        /// <summary>
        /// סԺҽ����Ϣ���  
        /// �˷����ڴ������ orderList��û��ҽ����ˮ��
        /// </summary>
        /// <param name="patient">������Ϣ</param>
        /// <param name="recipeDoc">����ҽʦ</param>
        /// <param name="orderList">ҽ����Ϣ</param>
        /// <returns>�ɹ�����1 ʧ�ܷ��أ�1</returns>
        public int AlterOrderOnSaving(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.FrameWork.Models.NeuObject recipeDoc, Neusoft.FrameWork.Models.NeuObject recipeDept, ref List<Neusoft.HISFC.Models.Order.Inpatient.Order> orderList)
        {
            return 1;
        }

        #endregion


        #region ˽�з���
                
        /// <summary>
        /// �Ӷ��ձ��в���ά����Ӧ֢����Ŀ
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="order"></param>
        /// <param name="practicableSymptomText">��Ӧ֢�ı�</param>
        /// <returns> -1 ���� 0 û��ά�� 1 ��ά��</returns>
        private int QueryItemByInpatOrder(Neusoft.HISFC.Models.RADT.PatientInfo patient, Neusoft.HISFC.Models.Order.Inpatient.Order order, ref string practicableSymptomText)
        {

            Neusoft.HISFC.BizLogic.Fee.Interface myInterface = new Neusoft.HISFC.BizLogic.Fee.Interface();
            Neusoft.HISFC.Models.SIInterface.Compare myCompare = new Neusoft.HISFC.Models.SIInterface.Compare();

            if (order != null || order.Patient.ID != "")
            {
                myInterface.GetCompareSingleItem(patient.Pact.ID, order.Item.ID, ref myCompare);
                if (myCompare.Ispracticablesymptom)
                {
                    practicableSymptomText = myCompare.Practicablesymptomdepiction;
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            if (order.Item.Memo != "1")
            {
                //��ά�����ж�����Ŀ

            }
            return 0;
        }
        /// <summary>
        /// �Ӷ��ձ��в���ά����Ӧ֢����Ŀ
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="order"></param>
        /// <param name="practicableSymptomText">��Ӧ֢�ı�</param>
        /// <returns> -1 ���� 0 û��ά�� 1 ��ά��</returns>
        private int QueryItemByOutpatOrder(Neusoft.HISFC.Models.Registration.Register patient, Neusoft.HISFC.Models.Order.OutPatient.Order order, ref string practicableSymptomText)
        {

            Neusoft.HISFC.BizLogic.Fee.Interface myInterface = new Neusoft.HISFC.BizLogic.Fee.Interface();
            Neusoft.HISFC.Models.SIInterface.Compare myCompare = new Neusoft.HISFC.Models.SIInterface.Compare();

            if (order != null || order.Patient.ID != "")
            {
                myInterface.GetCompareSingleItem(patient.Pact.ID , order.Item.ID , ref myCompare);
                if (myCompare.Ispracticablesymptom)
                {
                    practicableSymptomText = myCompare.Practicablesymptomdepiction;
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            if (order.Item.Memo != "1")
            {
                //��ά�����ж�����Ŀ

            }
            return 0;
        }
        #endregion

    }
}