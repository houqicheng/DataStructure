Status ListInsert_Sq(SeqList *L, int i, ElemType e)
{
	//��˳���L�е�i��λ���ϲ����½��e��
   	//i�ĺϷ�ֵΪ1<=i<=Length+1
   	if (i<1||i>L->length+1){           //ע������λ���Ǵ�1��ʼ��
    	return ERROR;
   	}
   	else{
   		for(j=L->length-1;j>=i-1;--j){ //ע��C�������±��0��ʼ
			L->elem[j+1]=L->elem[j];   //����λ�ü�֮���Ԫ������
    	}
    	L->elem[i-1]=e;                //����e
      	L->length++;                   //���� 1
	}
	return OK;
}
