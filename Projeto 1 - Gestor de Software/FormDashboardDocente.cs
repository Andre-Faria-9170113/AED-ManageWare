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
    public partial class FormDashboardDocente : Form
    {
        public FormDashboardDocente()
        {
            InitializeComponent();
        }

        private void FormDashboardDocente_Load(object sender, EventArgs e)
        {
            labelUsername.Text ="Logged-in como:   "+ File.ReadAllLines(currentUser)[0].Split(';')[0];
            if (!File.Exists(salas))
            {
                StreamWriter sw = File.CreateText(salas);
                sw.WriteLine("B201");
                sw.WriteLine("B202");
                sw.WriteLine("B203");
                sw.WriteLine("B204");
                sw.WriteLine("B205");
                sw.WriteLine("B222");
                sw.Close();
            }

            Salas();
        }

        //FILES
        string topicos = "topicos.txt";
        string salas = "salas.txt";
        string notifications = "notifications.txt";
        string currentUser = "currentUser.txt";

        //MÉTODO PRA DAR REFRESH ÀS LISTBOXES
        void RefreshSalasLists()
        {
            //RESET LISTBOXES
            listBoxSalasNumber.Items.Clear();
            listBoxSalasSoftware.Items.Clear();

            //PREENCHER SALAS
            string[] linhas = File.ReadAllLines(salas);
            for (int i = 0; i < linhas.Length; i++)
            {
                listBoxSalasNumber.Items.Add(linhas[i]);
            }

            //FOCAR PRIMEIRA SALA
            listBoxSalasNumber.SelectedIndex = 0;
        }

        //MÉTODO PRA VERIFICAR SE PODE SUBMETER NOVA NOTIFICAÇÃO
        void CheckSubmitNotification()
        {
           if(comboBoxNotificationNewSala.SelectedIndex != -1   && comboBoxNotificationNewAssunto.SelectedIndex != -1 && textBoxNotificationNewComment.Text != "")
            {
                buttonNotificationNewSubmit.Enabled = true;
            }
            else
            {
                buttonNotificationNewSubmit.Enabled = false;
            }
        }

        //MÉTODO PRA VERIFICAR SE PODE SUBMETER COMENTARIO
        void CheckSubmitComment()
        {
            if(dataGridViewNotifications.SelectedRows.Count > 0)
            {
                if (dataGridViewNotifications.SelectedRows[0].Cells[5].Value.ToString() == "Pendente")
                {
                    textBoxNotificationsComment.Enabled = true;
                }
                else
                {
                    textBoxNotificationsComment.Enabled = false;
                    textBoxNotificationsComment.Text = "";
                }

                if (textBoxNotificationsComment.Text != "")
                {
                    buttonNotificationsComment.Enabled = true;
                }
                else
                {
                    buttonNotificationsComment.Enabled = false;
                }

            }


        }

        //MÉTODO PREENCHER DATAGRID
        void RefreshNotificationsDatagrid()
        {
            dataGridViewNotifications.Rows.Clear();

            string[] linhas = File.ReadAllLines(notifications);
            for (int i = 0; i < linhas.Length; i++)
            {   
                if(linhas[i].Split(';')[1] == File.ReadAllLines(currentUser)[0].Split(';')[0])
                {
                    dataGridViewNotifications.Rows.Add(linhas[i].Split(';')[0], linhas[i].Split(';')[2], linhas[i].Split(';')[3], linhas[i].Split(';')[4], linhas[i].Split(';')[5], linhas[i].Split(';')[6]);
                }
            }
        }

        //MÉTODO LISTAR COMENTÁRIOS
        void ListComments()
        {
            textBoxNotificationsComments.Text = "";

            string[] linhas = File.ReadAllLines(notifications);

            for (int i = 0; i < linhas.Length; i++)
            {
                if (dataGridViewNotifications.SelectedRows.Count > 0)
                {
                    if (linhas[i].Split(';')[0] == dataGridViewNotifications.SelectedRows[0].Cells[0].Value.ToString())
                    {
                        for (int j = 7; j < linhas[i].Split(';').Length; j++)
                        {
                            textBoxNotificationsComments.AppendText(linhas[i].Split(';')[j] + "\n");

                        }
                    }
                }
            }
        }



        //MÉTODOS PARA ACEDER A CADA FUNCIONALIDADE
        //SALAS
        void Salas()
        {
            //MOSTRAR GROUPBOX
            groupBoxSalas.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupNotifications.Visible = false;
            groupBoxNotificationNew.Visible = false;

            //REFRESH LISTAS
            RefreshSalasLists();

            //VERIFICAR QUE TODAS AS SALAS TÊM UM FICHEIRO
            string[] linhas = File.ReadAllLines(salas);
            for (int i = 0; i < linhas.Length; i++)
            {
                if (!File.Exists(linhas[i] + ".txt"))
                {
                    StreamWriter sw = File.CreateText(linhas[i] + ".txt");
                    sw.Close();
                }
            }
        }

        //NOVA NOTIFICAÇÃO
        void NotificationNew()
        {
            //MOSTRAR GROUPBOX
            groupBoxNotificationNew.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupNotifications.Visible = false;
            groupBoxSalas.Visible = false;

            //RESET A TODOS OS CAMPOS
            comboBoxNotificationNewSala.Items.Clear();
            comboBoxNotificationNewAssunto.Items.Clear();
            textBoxNotificationNewComment.Text = "";
            labelNotificationNewSuccess.Visible = false;

            string[] linhas = File.ReadAllLines(salas);
            for (int i = 0; i < linhas.Length; i++)
            {
                comboBoxNotificationNewSala.Items.Add(linhas[i]);
            }

            string[] lines = File.ReadAllLines(topicos);
            for (int i = 0; i < lines.Length; i++)
            {
                comboBoxNotificationNewAssunto.Items.Add(lines[i].Split(';')[0]+". "+ lines[i].Split(';')[1]);
            }

            
        }

        //NOTIFICAÇÕES
        void Notifications()
        {
            //MOSTRAR GROUPBOX
            groupNotifications.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupBoxNotificationNew.Visible = false;
            groupBoxSalas.Visible = false;

            //RESET A TODOS OS CAMPOS
            textBoxNotificationsComments.Text = "";
            textBoxNotificationsComment.Text = "";
            RefreshNotificationsDatagrid();
        }

        private void listBoxSalasNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RESET E PREENCHER SOFTWARE
            listBoxSalasSoftware.Items.Clear();

            if (listBoxSalasNumber.SelectedIndex != -1)
            {
                string[] lines = File.ReadAllLines(listBoxSalasNumber.SelectedItem.ToString() + ".txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    listBoxSalasSoftware.Items.Add(lines[i].Split(';')[0]);
                }
            }
        }

        private void groupNotifications_Enter(object sender, EventArgs e)
        {

        }

        private void labelSoftwareLicense_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Notifications();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NotificationNew();
        }

        private void comboBoxNotificationNewSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSubmitNotification();
        }

        private void comboBoxNotificationNewAssunto_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSubmitNotification();
        }

        private void textBoxNotificationNewComment_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitNotification();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Salas();
        }

        private void buttonNotificationNewSubmit_Click(object sender, EventArgs e)
        {
            if (!File.Exists(notifications))
            {
                StreamWriter sw = File.CreateText(notifications);
                sw.Close();
            }

            string[] linhas = File.ReadAllLines(notifications);
            int[] arrayID = new int[linhas.Length];

            for (int i = 0; i < linhas.Length; i++)
            {
                arrayID[i] = Convert.ToInt32(linhas[i].Split(';')[0]);
            }

            StreamWriter sw1 = File.AppendText(notifications);

            string newID = "";
            if (arrayID.Length > 0)
            {
                newID = "0" + (arrayID.Max() + 1).ToString(); //NOVO ID QUE NUNCA VAI SER REPETIDO
            }
            else
            {
                newID = "01";
            }
            

            sw1.WriteLine(newID+";"+ File.ReadAllLines(currentUser)[0].Split(';')[0] + ";" + comboBoxNotificationNewSala.SelectedItem.ToString() + ";" + comboBoxNotificationNewAssunto.SelectedItem.ToString().Split('.')[0] + ";" + DateTime.Now.ToShortDateString() + ";" + DateTime.Now.ToShortTimeString()+";Pendente;"+ File.ReadAllLines(currentUser)[0].Split(';')[0]+": "+textBoxNotificationNewComment.Text+";");
            sw1.Close();

            NotificationNew();
            labelNotificationNewSuccess.Visible = true;
        }

        private void textBoxNotificationsComments_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxNotificationsComment_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitComment();
        }

        private void dataGridViewNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridViewNotifications_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dataGridViewNotifications_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            CheckSubmitComment();

           
            ListComments();
            
        }

        private void buttonNotificationsComment_Click(object sender, EventArgs e)
        {
            string[] linhas = File.ReadAllLines(notifications);
            File.Delete(notifications);

            StreamWriter sw = File.CreateText(notifications);
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Split(';')[0] == dataGridViewNotifications.SelectedRows[0].Cells[0].Value.ToString())
                {
                    sw.WriteLine(linhas[i] + File.ReadAllLines(currentUser)[0].Split(';')[0] + ": " + textBoxNotificationsComment.Text + ";");
                }
                else
                {
                    sw.WriteLine(linhas[i]);
                }
            }
            sw.Close();
            
            ListComments();
            textBoxNotificationsComment.Text = "";
            buttonNotificationsComment.Enabled = false;

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new FormLogin();
            login.Show();
            File.Delete(currentUser);
        }

        private void FormDashboardDocente_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
