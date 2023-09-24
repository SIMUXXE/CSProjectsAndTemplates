using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace UserLogin
{
    public partial class LoginForm : Form
    {
        List<User> Userlist = new List<User>();
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public LoginForm()
        {
            InitializeComponent();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.Height = 365;
            this.Width = 560;
            panelLogin.Dock = DockStyle.Fill;
            panelSignIn.Dock = DockStyle.Fill;
            panelHome.Dock = DockStyle.Fill;
            paneldvertisement.Dock = DockStyle.Fill;
            panelLogin.Show();
            panelSignIn.Hide();
            panelHome.Hide();
            labelError.Hide();
            paneldvertisement.Hide();
            if (!File.Exists(docPath))
                databaseCreator();
            else
                databaseLoader();
        }
        private void buttonSignIn_Click(object sender, EventArgs e)
        {
            panelLogin.Hide();
            panelSignIn.Show();
        }
        private void buttonBack_Click(object sender, EventArgs e)
        {
            panelSignIn.Hide();
            panelLogin.Show();
        }
        private void buttonRegister_Click(object sender, EventArgs e)
        {
            if (textBoxEmail.Text != null && textBoxNewUsername.Text != null && textBoxNewPassword.Text != null && textBoxRepeatPwd.Text != null)
            {
                User a = new User();
                a.Email = textBoxEmail.Text;
                a.Username = textBoxNewUsername.Text;
                if (passwordCheck(textBoxRepeatPwd.Text))
                {
                    a.Password = textBoxRepeatPwd.Text;
                    a.Id = Userlist.Count + 1;
                    Userlist.Add(a);
                    dataPrinter(a);
                    panelSignIn.Hide();
                    panelHome.Show();
                    textBoxEmail.Clear();
                    textBoxNewUsername.Clear();
                    textBoxNewPassword.Clear();
                    textBoxRepeatPwd.Clear();
                }
                else
                {
                    textBoxNewPassword.Clear();
                    textBoxRepeatPwd.Clear();
                    paneldvertisement.Show();
                }

            }
        }
        private bool passwordCheck(string pwd)
        {
            char[] specialChars = { '@', '#', '?', '!', '€', '$', '_' };
            char[] forbiddenChars = { ':', ',', '.', ';', '-', '/', '+', '(', ')', '[', ']', '{', '}' };
            bool contSpecial = false, contForbidden = false;
            for (int i = 0; i < specialChars.Length; i++)
            {
                if (pwd.Contains(specialChars[i].ToString()))
                {
                    contSpecial = true;
                    break;
                }
            }
            for (int i = 0; i < forbiddenChars.Length; i++)
            {
                if (pwd.Contains(forbiddenChars[i].ToString()))
                {
                    contForbidden = true;
                    break;
                }
            }
            if (contSpecial == true && contForbidden == false && textBoxNewPassword.Text == textBoxRepeatPwd.Text && pwd.Length > 6)
                return true;
            else
                return false;
        }
        private void databaseCreator()
        {
            string line = "Database:";
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "UserDatas.txt")))
            {
                outputFile.WriteLine(line);
            }
        }
        private void databaseLoader()
        {
            using (StreamReader inputFile = new StreamReader(Path.Combine(docPath, "UserDatas.txt")))
            {
                string line;
                string[] sublines = new string[2];
                while ((line = inputFile.ReadLine()) != "---" && line!= "Database:")
                {
                    User a = new User();
                    if (line.Contains("email"))
                    {
                        sublines = line.Split(':');
                        a.Email = sublines[1];
                    }
                    else if (line.Contains("Username"))
                    {
                        sublines = line.Split(':');
                        a.Username = sublines[1];
                    }
                    else if (line.Contains("password"))
                    {
                        sublines = line.Split(':');
                        a.Password = sublines[1];
                    }
                    else if (line.Contains("id"))
                    {
                        sublines = line.Split('#');
                        a.Id = Convert.ToInt32(sublines[1]);
                    }
                    else if (line == "")
                        break;
                    Userlist.Add(a);
                }
            }
        }
        private void dataPrinter(User a)
        {
            string[] lines = { $"email:{a.Email}", $"Username:{a.Username}", $"password:{a.Password}", $"id:#{a.Id}", "---" };
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "UserDatas.txt"), true))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (Userlist.Count > 0)
            {
                foreach (User element in Userlist)
                    if (element.Username == textBoxUsername.Text && element.Password == textBoxPassword.Text)
                    {
                        panelLogin.Hide();
                        panelHome.Show();
                        labelError.Hide();
                    }
                    else
                    {
                        textBoxUsername.Clear();
                        textBoxPassword.Clear();
                        labelError.Show();
                    }
            }
            else
            {
                panelLogin.Hide();
                panelSignIn.Show();
            }
        }
        private void buttonLogout_Click(object sender, EventArgs e)
        {
            panelHome.Hide();
            panelLogin.Show();
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            paneldvertisement.Hide();
            panelSignIn.Show();
        }
    }
}