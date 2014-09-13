Status PreOrderTraverse(BiTree T,Status(*Visit)(TElemType e)){
	//ǰ������������ķǵݹ��㷨����ÿ��Ԫ�ص��ú���Visit
	InitStack(S);  p = T;
	while(p || !StackEmpty(S)){
		if(p){
			if(!Visit(p->data)){
				return ERROR;
			}
			Push(S,p);
			p = p->lchild;
		}
		else{
			Pop(S,p);
			p = p->rchild;
		}
	}//while
	return OK;
}  //PreOrderTraverse
