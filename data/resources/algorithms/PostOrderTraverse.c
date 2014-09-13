Status PostOrderTraverse(BiTree T,Status(*Visit)(TElemType e)){
    //��������������ķǵݹ��㷨����ÿ��Ԫ�ص��ú���Visit
    BiTree p = T,q = NULL;
    SqStack S;	InitStack(S);  Push(S,p);
    while (!StackEmpty(S)){
		if(p && p != q){
	    	Push(S,p);
	    	p=p->lchild;
		}
    	else{
	    	Pop(S,p);
	    	if(!StackEmpty(S)){
				if(p->rchild && p->rchild != q){
		    		Push(S,p);
            	    p=p->rchild;}  //if
				else{			
           	   		Visit(p->data);
		    		q = p;}  //else
	    	} //if
		}  //else
    }  //while
    return OK;
}
