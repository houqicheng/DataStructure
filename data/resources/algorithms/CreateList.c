 Void CreateList_l(LinkList &L, int n){
    //��λ������n������ֵ��������ͷ���ĵ������Ա�L
    //��ν��λ����ָÿ������Ľ�㶼�嵽�����е�һ�����֮ǰ
    L=(LinkList)malloc(sizeof(Lnode));  L->next=NULL;//ͷ����ʼ��
    for(i=n;i>0;i--){
       p=(LinkList)malloc(sizeof(Lnode));  scanf(&p->data); //�����½��     
       p->next=L->next;   //���½����뵽��һ�����֮ǰ
       L->next=p;
    }
    return;
 }  //CreateList_L
