Statua ListInsert_L(LinkList &L, int i,ElemType e){
    //�ڴ�ͷ���ĵ�����L�У��ڵ�i��λ��֮ǰ����ֵΪe��һ�����
    p = L;j = 0;
    while(p && j<i-1){  //Ѱ�ҵ�i����㣬����pָ����ǰ��
       p = p->next; j++;
    }
    if(p == NULL)
       return ERROR;
    s = (LinkList)malloc(sizeof(LNode)); //����һ���½��
    s->data = e;
    s->next = p->next;
    p->next = s;
    return OK;
}   //ListDelete_L
