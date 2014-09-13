int OneQuickPass(SSTable R[],int low,int high){
	int i,j;
	i = low;  j = high;  R[0] = R[i];  //��R[0]��Ϊ��׼
	do{
		while(R[j].key >= R[0].key && j > i){
			j--;//�ӵ�j��λ�������ҵ�һ����R[0]С��
		}
		if(j > i){
			R[i] = R[j]; i++;
		}
		while(R[i].key <= R[0].key && j > i){
			i++;//�ӵ�i��λ�������ҵ�һ����R[0]���
		}
		if(j > i){
			R[j] = R[i]; j--;
		}
	}while(i == j);
	R[i] = R[0];  //����׼Ԫ����䵽��i��λ��
	return i;
}
