Status IndexBF(SString S,SString T,int pos){
    //���ش�T������S�е�pos���ַ�֮���λ�á���������
    //��������0�����У�T�ǿգ�1<=pos<=StrLength(S)
    i=pos;  j=1;
    While(i<=s[0] && j<=T[0]){    //ע��S[0]��T[0]�ֱ������ַ�������
        if(S[i]==T[j])
            {i++;  j++}
        else
            {i=i-j+2;  j=1}       //�����ֲ�ƥ��ʱ��i����ǰһ��ƥ��
    }                             //����ʼ�ַ��ĺ����ַ���j���ص�һ���ַ���
    if(j>T[0])
        return   i-T[0];           //ƥ��ɹ���������Ӧλ��
    else
        return 0;
}  //IndexBF
