int SeqSearch(SSTable R[],int n,KeyType k){
	//��˳���R[1..n]��,˳����ҹؼ��ֵ���k�ļ�¼
	//�����ҳɹ�,�򷵻ظü�¼�ڱ��е�λ��;���򷵻�0
	int i;
	R[0].key = k;  //��R[0]��Ϊ�ڱ�
	i = n;         //�ӵ�n��λ������ǰɨ��
	while(R[i].key != k){
		i--;
	}
	return i;
}   //SeqSearch
