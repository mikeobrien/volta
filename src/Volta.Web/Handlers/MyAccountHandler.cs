using System;

namespace Volta.Web.Handlers
{
    public class MyAccountOutputModel
    {
        public string Time { get; set; }
    }

    public class MyAccountHandler
    {
        public MyAccountOutputModel Query()
        {
            return new MyAccountOutputModel { Time = DateTime.Now.ToString() };
        }
    }
}