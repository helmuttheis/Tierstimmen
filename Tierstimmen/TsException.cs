using System;
using System.Collections.Generic;
using System.Text;

namespace Tierstimmen
{
    public class TsException: Exception
    {
        public string szMessage { get; set; }
        public TsException(string szMessage)
        {
            System.Diagnostics.Debug.WriteLine(szMessage);
            this.szMessage = szMessage;
        }
    }
}
