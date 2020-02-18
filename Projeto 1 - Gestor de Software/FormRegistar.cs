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
    public partial class FormRegistar : Form
    {
        public FormRegistar()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        string fileUtilizadores = "utilizadores.txt";

        void CheckValidity() //MÉTODO PARA VERIFICAR SE AMBAS AS TEXTBOXES ESTÃO PREENCHIDAS
        {
            if (textBox1.Text != "" && textBox2.Text != ""  &&  textBox3.Text != "" && textBox4.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form f1 = new FormLogin();
            f1.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            CheckValidity();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //VERIFICAR VALIDADE DOS VÁRIOS CAMPOS
            bool validUser = true;

            //USER
            string[] linhas = File.ReadAllLines(fileUtilizadores);
            for (int i = 0; i < linhas.Length; i++)
            {
              if(textBox1.Text == linhas[i].Split(';')[0]) //VERIFICAR SE JÁ SE ENCONTRA REGISTADO UM USER COM NOME IDÊNTIDCO
                {
                    validUser = false;

                    //RESETAR CAMPOS DE INPUT
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";

                    //EXIBIR LABEL COM ERRO
                    label6.Visible = true;

                    //FOCAR 1º CAMPO
                    textBox1.Focus();

                    break;
                }
                else
                {
                    //OCULTAR LABEL DE ERRO
                    label6.Visible = false;
                }
            }

            //PASSWORDS
            if(textBox2.Text != textBox3.Text) //VERIFICAR SE OS DOIS CAMPOS PARA PASSWORD TÊM VALORES DIFERENTES
            {
                validUser = false;

                //RESETAR CAMPOS DE INPUT
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                //EXIBIR LABEL COM ERRO
                label7.Visible = true;

                //FOCAR 1º CAMPO
                textBox1.Focus();
            }
            else
            {
                //OCULTAR LABEL DE ERRO
                label7.Visible = false;
            }

            if (textBox2.Text.Length < 6) //VERIFICAR SE A PASSWORD TEM MENOS DE 6 CARATERES
            {
                validUser = false;

                //RESETAR CAMPOS DE INPUT
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                //EXIBIR LABEL COM ERRO
                label8.Visible = true;

                //FOCAR 1º CAMPO
                textBox1.Focus();
            }
            else
            {
                //OCULTAR LABEL DE ERRO
                label8.Visible = false;
            }

            //EMAIL 
            if (!textBox4.Text.Contains('@') || !textBox4.Text.Contains('.')) //VERIFICAR SE FOI INTRODUZIDO UM EMAIL INVÁLIDO
            {
                validUser = false;

                //RESETAR CAMPOS DE INPUT
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";

                //EXIBIR LABEL COM ERRO
                label9.Visible = true;

                //FOCAR 1º CAMPO
                textBox1.Focus();
            }
            else
            {
                //OCULTAR LABEL DE ERRO
                label9.Visible = false;
            }

            if (validUser == true)
            {
                //ACRESCENTAR O NOVO DOCENTE AO FICHEIRO DE UTILIZADORES
                StreamWriter sw = File.AppendText(fileUtilizadores);
                sw.WriteLine(textBox1.Text+";"+textBox4.Text+";"+textBox2.Text+";docente");
                sw.Close();

                //MOSTRAR MESSAGEBOX A INFORMAR QUE O PERFIL DO DOCENTE FOI CRIADO COM SUCESSO
                DialogResult result = MessageBox.Show("Docente registado com sucesso!", "Mensagem",MessageBoxButtons.OK);

                if(result == DialogResult.OK)
                {
                    this.Hide();
                    Form f1 = new FormLogin();
                    f1.Show();
                }
            }
        }
    }
}
