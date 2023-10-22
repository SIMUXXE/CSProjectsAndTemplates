using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace Reportable_Database_example
{
    public partial class Form1 : Form
    {
        List<Student> RegisteredStudents = new List<Student>();                                 //"Emulated" Student DB
        List<Certificate> RegisteredCertificates = new List<Certificate>();                     //"Emulated" Certificate DB
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);      //Gets the Windows "Document" folder Path

        public Form1()
        {
            InitializeComponent();
            ComboBoxCertificateFiller(comboBoxCertificate);
            ComboBoxCertificateFiller(comboBoxCertificatieToAdd);
            this.Height = 490;
            this.Width = 815;
            panelRegister.Dock = DockStyle.Fill;
            panelAdd.Dock = DockStyle.Fill;
            panelAdd.Hide();
            panelRegister.Show();
            if (!File.Exists(Path.Combine(docPath, "StudentsDatas.json")) && !File.Exists(Path.Combine(docPath, "CertificateDatas.json")))
            {
                StudentDBSerializer();
                CertificateDBSerializer();
            }
            else
            {
                StudentDBLoader();
                CertificateDBLoader();
            }
        }
        void ComboBoxCertificateFiller(ComboBox a)
        {
            a.Items.Add("ICDL base Skillcard - MIUR");
            a.Items.Add("English B1 - MIUR");
            a.Items.Add("English B2 - MIUR");
            a.Items.Add("Introduction to IOT - Cisco");
            a.Items.Add("Introduction to Cybersecurity - Cisco");
        }

        bool CheckFormInput()
        {
            bool isEmpty = false;
            bool isValidDate = false;

            if (textBoxName.Text != "" && textBoxSurname.Text != "" && textBoxEMail.Text != "" && comboBoxCertificate.Text != "")
                isEmpty = false;
            else isEmpty = true;

            string bdToCheck = birthdateTimePicker.Text;
            string odToCheck = obtainementDateTimePicker.Text;
            DateTime bd = Convert.ToDateTime(bdToCheck);
            DateTime od = Convert.ToDateTime(odToCheck);
            if (bd.Year < DateTime.Now.Year - 15 && od.Year <= DateTime.Now.Year)
                isValidDate = true;
            else isValidDate = false;

            if (!isEmpty && isValidDate)
                return true;
            else return false;
        }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (CheckFormInput())
            {
                Student newStudent = new Student(textBoxEMail.Text, textBoxName.Text, textBoxSurname.Text, Convert.ToDateTime(birthdateTimePicker.Text));
                RegisteredStudents.Add(newStudent);
                Certificate newCertificate = new Certificate(textBoxEMail.Text, comboBoxCertificate.SelectedIndex.ToString(), Convert.ToDateTime(obtainementDateTimePicker.Text));
                RegisteredCertificates.Add(newCertificate);
                StudentDBSerializer();
                CertificateDBSerializer();
            }
        }
        /*=====================================================================================================================================================*/
        void StudentDBSerializer()  //This method serializes the "RegisteredStudents" list in an external Json
        {
            File.Delete(Path.Combine(docPath, "StudentsDatas.json"));
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "StudentsDatas.json")))
            {
                outputFile.WriteLine(JsonSerializer.Serialize(RegisteredStudents));
            }
        }
        void StudentDBLoader()  //This method loads the "StudentsDatas.json" file in the Homonym list
        {
            using (StreamReader inputFile = new StreamReader(Path.Combine(docPath, "StudentsDatas.json")))
            {
                RegisteredStudents = JsonSerializer.Deserialize<List<Student>>(inputFile.ReadLine());
            }
        }
        void CertificateDBSerializer() //This method serializes the "RegisteredCertificates" list in an external Json
        {
            File.Delete(Path.Combine(docPath, "CertificateDatas.json"));
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "CertificateDatas.json")))
            {
                outputFile.WriteLine(JsonSerializer.Serialize(RegisteredCertificates));
            }
        }
        void CertificateDBLoader()  //This method loads the "CertificateDatas.json" file in the Homonym list
        {
            using (StreamReader inputFile = new StreamReader(Path.Combine(docPath, "CertificateDatas.json")))
            {
                RegisteredCertificates = JsonSerializer.Deserialize<List<Certificate>>(inputFile.ReadLine());
            }
        }
        /*=====================================================================================================================================================*/

        private void buttonAddCertification_Click(object sender, EventArgs e)
        {
            panelRegister.Hide();
            panelAdd.Show();
        }

        bool EMailDBSearch(string a) //Chechks if a specific mail is existing in our DB ("RegisteredStudents")
        {
            bool found = false;
            foreach (Student item in RegisteredStudents)
            {
                if (item.eMail == a)
                    found = true;
                else
                    found = false;
            }
            if (found)
                return true;
            else return false;
        }

        void DisplayInListBox(string Email) //Shows the Teport in the ListBox
        {
            foreach (Student Item in RegisteredStudents)
            {
                if (Item.eMail == Email)
                {
                    listBoxDisplay.Items.Add($"{Item.name} {Item.surname}   {Item.eMail}");
                    listBoxDisplay.Items.Add($"Birthdate: {Item.birthDate.Day}/{Item.birthDate.Month}/{Item.birthDate.Year}");
                    break;
                }
            }
            foreach (Certificate item in RegisteredCertificates)
            {
                if (item.eMail == Email)
                {
                    listBoxDisplay.Items.Add($"{item.certName}-{item.company}");
                    listBoxDisplay.Items.Add($"Obtained: {item.obtainementDate.Day}/{item.obtainementDate.Month}/{item.obtainementDate.Year}");
                }
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (EMailDBSearch(textBoxEMailToSearch.Text))
            {
                Certificate CertificateToAdd = new Certificate(textBoxEMailToSearch.Text, comboBoxCertificatieToAdd.SelectedIndex.ToString(), Convert.ToDateTime(ObtainementDateTimePickerToAdd.Text));
                RegisteredCertificates.Add(CertificateToAdd);
                DisplayInListBox(textBoxEMailToSearch.Text);
            }
        }
    }
}
