Status ListDelete_Sq(SqList &L, int i, ElemType &e)
  //��˳�����Ա�L��ɾ����i��Ԫ��,i�ĺϷ�ֵΪ1<=i<=L.length
{
   If(i<1||i>L.length){
       Return ERROR;   //i��ֵ���Ϸ�
   }
   e=L.elem[i-1];     //��ɾ��Ԫ����e����
   for(j=i;j<=L.length-1;j++) {
		L.elem[j-1]=L.elem[j];   //��ɾ��Ԫ��֮���Ԫ������
   }
   L.length--;    //����1   
   Return OK;
} //ListDelete_Sq
