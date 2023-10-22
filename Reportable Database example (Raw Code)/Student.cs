using System;
using System.Collections.Generic;

namespace Reportable_Database_example
{
    public class Student
    {
        public string eMail { get; set; } //Report Primary Key
        public string name { get; set; }
        public string surname { get; set; }
        public DateTime birthDate = new DateTime();
        public Student(string mail, string name, string surname, DateTime Bd)
        {
            eMail = mail;
            this.name = name;
            this.surname = surname;
            birthDate = Bd;
        }
    }
}
