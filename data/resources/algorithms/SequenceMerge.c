 Void Mergelist_Sq(SqList La,SqList Lb,Sqlist &Lc)
    //��֪˳�����Ա�La��Lb��Ԫ�ذ�ֵ�ǵݼ�˳������
    //�ϲ�La��Lb��Ԫ�أ��õ���˳���Lc,ʹLcҲ����
 {
    i=j=0;k=0;
    while��i<La.length && j<Lb.length��
    {
       If(La.elem[i]<=Lb.elem[j])
  	     Lc.elem[k++]=La.elem[i++];
       Else
           Lc.elem[k++]=Lb.elem[j++];
����}
����While(i<La.length)   //����La�е�ʣ��Ԫ��
����   Lc.elem[k++]=La.elem[i++];
����While(j<Lb.length)   //����Lb�е�ʣ��Ԫ��
����   Lc.elem[k++]=Lb.elem[j++];
����Lc.length=k;
 }  //MergeList_Sq
