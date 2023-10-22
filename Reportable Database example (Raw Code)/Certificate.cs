using System;

namespace Reportable_Database_example
{
    class Certificate
    {
        public string eMail { get; set; } //Report Foreign key
        public string certName { get; set; }
        public string company { get; set; }
        public DateTime obtainementDate = new DateTime();
        public Certificate(string mail, string cName, DateTime obDate)
        { 
            eMail = mail;
            string[] x = cName.Split('-');
            certName = x[0];
            company = x[1]; 
            obtainementDate = obDate;
        }
    }
}
