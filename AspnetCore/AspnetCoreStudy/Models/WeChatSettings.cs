namespace AspnetCoreStudy.Models
{
    public class WeChatSettings
    {
        public string AppId{get;set;}

        public string AppSecret{get;set;}
    }

    public class Authentication
    {
        public WeChatSettings WeChat{get;set;}
    }
}