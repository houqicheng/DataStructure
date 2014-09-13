Status InorderTraverse(BiTree T,Status(*Visit)(TElemType e)){
	//��������������ķǵݹ��㷨����ÿ��Ԫ�ص��ú���Visit
	InitStack(S);  p = T;
	while(p || !StackEmpty(S)){
		if(p){
			Push(S,p);
			p = p->lchild;
		}
		else{
			Pop(S,p);
			if(!Visit(p->data)){
				return ERROR;
			}
			p = p->rchild;
		}
	}//while
	return OK;
}  //InorderTraverse
