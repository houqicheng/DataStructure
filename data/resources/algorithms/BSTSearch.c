BSTree BSTSearch(BSTree BST,KeyType k){
	BSTree p;
	p = BST;
	while(p != NULL && p->key != k){
		if(k < p->key){
			p = p->lchild;  //����������
		}
		else{
			p = p->rchild;  //����������
		}
	}
	return p;
}  //BinSearch
