int BinSearch(SSTable R[],int n,KeyType k){
	//�������R[1..n]���۰���ҹؼ��ֵ���k�ļ�¼
	//���ҵ�,�򷵻ظü�¼�ڱ��е�λ��;���򷵻�0
	int low = 1,high = n,mid;
	while(low <= high){
		mid = (low + high) / 2;
		if(R[mid].key == k){
			return mid;
		}
		else if(R[mid].key < k){
			low = mid + 1;
		}
		else{
			high = mid - 1;
		}
	}
	return 0;
}	//BinSearch
