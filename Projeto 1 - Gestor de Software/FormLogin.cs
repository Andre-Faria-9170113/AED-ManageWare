using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Projeto_1___Gestor_de_Software
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //HARDCODE LOGIN
            textBox1.Text = "andré";
            textBox2.Text = "abc123";

            //FOCAR NO CAMPO "USERNAME"
            this.ActiveControl = textBox1;

            //VERIFICAR SE AINDA NÃO EXISTE UM FIHEIRO "utilizadores.txt"
            if (!File.Exists(fileUtilizadores))
            {
                StreamWriter sw = File.CreateText(fileUtilizadores);
                sw.WriteLine("abc123;abc123@gmail.com;abc123;segurança");
                sw.WriteLine("mariopinto;mariopinto@esmad.ipp.pt;abc123;docente");
                sw.Close();
            }

            
        }

        string fileUtilizadores = "utilizadores.txt";
        string currentUser = "currentUser.txt";

        void CheckValidity() //MÉTODO PARA VERIFICAR SE AMBAS AS TEXTBOXES ESTÃO PREENCHIDAS
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //AUTENTICAR TENTATIVA DE LOGIN
            string[] linhas = File.ReadAllLines(fileUtilizadores);
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Split(';')[0] == textBox1.Text && linhas[i].Split(';')[2] == textBox2.Text) //VERIFICAR SE OS VALORES DAS TEXTBOXES CORRESPONDEM A UMA CONTA JÁ REGISTADA
                {
                    
                    label4.Visible = false;
                    this.Hide();
                    if (File.Exists(currentUser))
                    {
                        File.Delete(currentUser);
                        StreamWriter sw = File.CreateText(currentUser);
                        sw.WriteLine(linhas[i]);
                        sw.Close();
                    }
                    else
                    {
                        StreamWriter sw = File.CreateText(currentUser);
                        sw.WriteLine(linhas[i]);
                        sw.Close();
                    }

                    if (linhas[i].Split(';')[3] == "docente") //CASO SEJA DOCENTE
                    {
                        Form f2 = new FormDashboardDocente();
                        f2.Show(); //MUDAR DE FORM
                    }
                    else if (linhas[i].Split(';')[3] == "segurança") //CASO SEJA DOS SI
                    {
                        Form f2 = new FormDashboardSI();
                        f2.Show(); //MUDAR DE FORM
                    }

                }
            }

            label4.Visible = true; //CASO A TENTATIVA DE LOGIN SEJA INSUCEDIDA
            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Focus();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f3 = new FormRegistar();
            f3.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }


        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void FormLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
