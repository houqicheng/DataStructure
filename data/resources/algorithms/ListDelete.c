Statua ListDelete_L(LinkList &L, int i,ElemType &e){
    //�ڴ�ͷ���ĵ�����L�У�ɾ����i����㣬����e������ֵ
    p = L;j = 0;
    while(p->next && j<i-1){  //Ѱ�ҵ�i����㣬����pָ����ǰ��
       p = p->next; j++;
    }
    if(p->next == NULL)
       return ERROR;
    q = p->next;   //�ݴ�Ҫɾ�����
    p->next = q->next;
    e = q->data; free(q);   //�ͷŽ��
    return OK;
}   //ListDelete_L
