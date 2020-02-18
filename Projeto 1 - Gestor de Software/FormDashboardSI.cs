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
    public partial class FormDashboardSI : Form
    {
        public FormDashboardSI()
        {
            InitializeComponent();
        }

        private void FormDashboardSI_Load(object sender, EventArgs e)
        {
            labelUsername.Text = "Logged-in como:   " + File.ReadAllLines(currentUser)[0].Split(';')[0];
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
            
            Software();

            CountNotificationsPendentes();
        }

        //FILES
        string topicos = "topicos.txt";
        string salas = "salas.txt";
        string notifications = "notifications.txt";
        string currentUser = "currentUser.txt";
        string utilizadores = "utilizadores.txt";
        

        //MÉTODO PARA VERIFICAR SE O BUTTON DE SUBMISSÃO DE SOFTWARE PASSA A ESTAR ATIVO
        void CheckSubmitSoftware()
        {
            if(textBoxSoftwareName.Text != ""   &&  comboBoxSoftwareSala.Text != "" &&  comboBoxSoftwareLicense.Text != "")
            {
                buttonSoftware.Enabled = true;
            }
            else
            {
                buttonSoftware.Enabled = false;
            }
        }

        //MÉTODO PARA ATUALIZAR DataGrid DOS TOPICOS
        void RefreshDatagridTopics()
        {
            //CLEAR À DATAGRID
            dataGridViewTopics.Rows.Clear();

            //POPULAR DATAGRID
            string[] linhas = File.ReadAllLines(topicos);

            for (int i = 0; i < linhas.Length; i++)
            {
                dataGridViewTopics.Rows.Add(linhas[i].Split(';')[0], linhas[i].Split(';')[1]);
            }
        }

        //MÉTODO PARA ALTERNAR ENTRE NOVO TOPICO / EDITAR TOPICO
        void CheckTopicMode()
        {
            if (radioButtonTopicsNew.Checked) //NOVO TOPICO
            {
                comboBoxTopicsID.SelectedIndex = -1;
                comboBoxTopicsID.Enabled = false;
                textBoxTopicsTopic.Text = "";
                buttonTopicsRemove.Enabled = false;
            }

            else if (radioButtonTopicsEdit.Checked) //EDITAR TOPICO
            {

                comboBoxTopicsID.SelectedItem = comboBoxTopicsID.Items[0];

                comboBoxTopicsID.Enabled = true;

                string[] linhas = File.ReadAllLines(topicos);

                textBoxTopicsTopic.Text = linhas[0].Split(';')[1];
                    
                buttonTopicsRemove.Enabled = true;
            }
        }

        //MÉTODO PARA ALTERNAR ENTRENOVA SALA/ EDITAR SALA
        void CheckSalasMode()
        {
            if(radioButtonSalasNew.Checked == true)
            {
                textBoxSalasID.Enabled = true;
                textBoxSalasID.Text = "";
                textBoxSalasID.Focus();
                buttonSalasRemove.Enabled = false;
            }
            else if(radioButtonSalasEdit.Checked == true)
            {
              buttonSalasRemove.Enabled = true;
              listBoxSalasNumber.SelectedIndex = 0;
              textBoxSalasID.Text = listBoxSalasNumber.SelectedItem.ToString();
            }
        }

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

        //MÉTODO PARA VERIFICAR SE ESTÁ A SER INSERIDA UM SALA DUPLICADA
        bool CheckDuplicateSalas() {
            string[] linhas = File.ReadAllLines(salas);
            for (int i = 0; i < linhas.Length; i++)
            {   
                if(linhas[i] == textBoxSalasID.Text)
                {
                    return true;
                }
            }
            return false;
        }

        //MÉTODO PREENCHER DATAGRID NOTIFICAÇÕES
        void RefreshNotificationsDatagrid()
        {
            dataGridViewNotifications.Rows.Clear();

            string[] linhas = File.ReadAllLines(notifications);
            for (int i = 0; i < linhas.Length; i++)
            {
                dataGridViewNotifications.Rows.Add(linhas[i].Split(';')[0], linhas[i].Split(';')[1], linhas[i].Split(';')[2], linhas[i].Split(';')[3], linhas[i].Split(';')[4], linhas[i].Split(';')[5], linhas[i].Split(';')[6]);
            }


           
           

            if (dataGridViewNotifications.SelectedRows.Count > 0)
            {
                buttonNotificationsConcluir.Enabled = true;
            }
            else
            {
                buttonNotificationsConcluir.Enabled = false;
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

        //MÉTODO PRA VERIFICAR SE PODE SUBMETER COMENTARIO
        void CheckSubmitComment()
        {
            if (dataGridViewNotifications.SelectedRows.Count > 0)
            {

                if (dataGridViewNotifications.SelectedRows[0].Cells[6].Value.ToString() == "Pendente")
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

        //MÉTODO PARA CONTAR NOTIFICAÇÕES PENDENTES
        void CountNotificationsPendentes()
        {
            int count = 0;
            string[] linhas = File.ReadAllLines(notifications);
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Split(';')[6] == "Pendente")
                {
                    count++;
                }
            }

            labelNotificationCount.Text = count.ToString();
 
        }

        //MÉTODOS PARA ACEDER A CADA FUNCIONALIDADE
        //SOFTWARE
        void Software()
        {
            //RESET A TODOS OS CAMPOS
            textBoxSoftwareName.Text = "";
            comboBoxSoftwareSala.ResetText();
            comboBoxSoftwareLicense.ResetText();
            dateTimePickerSoftwareDate.Value = dateTimePickerSoftwareDate.MaxDate;
            dateTimePickerSoftwareHour.Value = DateTime.Now;
            buttonSoftware.Enabled = false;
            labelSoftwareError.Visible = false;
            labelSoftwareSuccess.Visible = false;
            comboBoxSoftwareSala.Items.Clear();

            //PREENCHER DROPDOWN COM SALAS
            string[] linhas = File.ReadAllLines(salas);
            for (int i = 0; i < linhas.Length; i++)
            {
                comboBoxSoftwareSala.Items.Add(linhas[i]);
            }

            //LIMITAR dateTimePickerSoftwareDate À DATA DO SISTEMA
            dateTimePickerSoftwareDate.MaxDate = DateTime.Now.Date;

            //TORNAR VISIVEL A RESPETIVA GROUPBOX
            groupBoxSoftware.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupBoxSalas.Visible = false;
            groupBoxTopics.Visible = false;
            groupNotifications.Visible = false;

            //FOCAR NA TEXTBOX DO NOME DO SOFTWARE
            textBoxSoftwareName.Select();

        }

        //TOPICOS
        void Topicos()
        {
            //RESET A TODOS OS CAMPOS
            radioButtonTopicsNew.Checked = true;
            radioButtonTopicsEdit.Checked = false;
            textBoxTopicsTopic.Text = "";
            buttonTopicsConfirm.Enabled = false;

            

            //MOSTRAR GROUPBOX
            groupBoxTopics.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupBoxSalas.Visible = false;
            groupBoxSoftware.Visible = false;
            groupNotifications.Visible = false;

            //ATUALIZAR Datagrid
            RefreshDatagridTopics();

            //PREENCHER COMBOBOX DE ID'S
            string[] linhas = File.ReadAllLines(topicos);

            for (int i = 0; i < linhas.Length; i++)
            {
                comboBoxTopicsID.Items.Add(linhas[i].Split(';')[0]);
            }

            CheckTopicMode();

            textBoxTopicsTopic.Focus();


        }

        //SALAS
        void Salas()
        {
            //MOSTRAR GROUPBOX
            groupBoxSalas.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupBoxTopics.Visible = false;
            groupBoxSoftware.Visible = false;
            groupNotifications.Visible = false;

            //RESET A TODOS OS CAMPOS
            textBoxSalasID.Text = "";
            radioButtonSalasNew.Checked = true;
            buttonSalasConfirm.Enabled = false;

            labelSalasSuccess.Visible = false;
            labelSalasError.Visible = false;

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

        //NOTIFICAÇÕES
        void Notifications()
        {
            //MOSTRAR GROUPBOX
            groupNotifications.Visible = true;

            //TORNAR AS OUTRAS GROUPBOXES INVISIVEIS
            groupBoxSoftware.Visible = false;
            groupBoxSalas.Visible = false;
            groupBoxTopics.Visible = false;

            //RESET A TODOS OS CAMPOS
            textBoxNotificationsComments.Text = "";
            textBoxNotificationsComment.Text = "";
            

            //DELIMITAR FILTROS DE DATAS ENTRE NOTIFICAÇÃO MAIS ANTIGA E A MAIS RECENTE
            string[] linhas = File.ReadAllLines(notifications);
            DateTime[] arrayDatas = new DateTime[linhas.Length];
            for (int i = 0; i < linhas.Length; i++)
            {
                arrayDatas[i] = Convert.ToDateTime(linhas[i].Split(';')[4]);
            }

            dateTimePickerFilterStartDate.MinDate = arrayDatas.Min();
            dateTimePickerFilterStartDate.MaxDate = arrayDatas.Max();
            dateTimePickerFilterEndDate.MinDate = arrayDatas.Min();
            dateTimePickerFilterEndDate.MaxDate = arrayDatas.Max();

            dateTimePickerFilterStartDate.Value = dateTimePickerFilterStartDate.MinDate;
            dateTimePickerFilterEndDate.Value = dateTimePickerFilterEndDate.MaxDate;

            //POPULAR COMBOBOXES
            //SALAS
            comboBoxFilterSala.Items.Clear();
            string[] lines = File.ReadAllLines(salas);
            comboBoxFilterSala.Items.Add("(nenhum)");
            for (int i = 0; i < lines.Length; i++)
            {
                comboBoxFilterSala.Items.Add(lines[i]);
            }
            comboBoxFilterSala.SelectedIndex = 0;

            //DOCENTE
            comboBoxFilterDocente.Items.Clear();
            string[] lines1 = File.ReadAllLines(utilizadores);
            comboBoxFilterDocente.Items.Add("(nenhum)");
            for (int i = 0; i < lines1.Length; i++)
            {
                if(lines1[i].Split(';')[3] == "docente")
                {
                    comboBoxFilterDocente.Items.Add(lines1[i].Split(';')[0]);
                }
            }
            comboBoxFilterDocente.SelectedIndex = 0;

            RefreshNotificationsDatagrid();

        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Software();
        }

        private void comboBoxSoftwareSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBoxSoftwareTipoLicença_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void dateTimePickerSoftwareDate_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void textBoxSoftwareName_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void comboBoxSoftwareSala_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void comboBoxSoftwareLicense_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void labelSoftwareError_Click(object sender, EventArgs e)
        {

        }

        private void groupNotifications_Enter(object sender, EventArgs e)
        {

        }

        private void labelSoftwareError_Click_1(object sender, EventArgs e)
        {

        }

        private void textBoxSoftwareName_TextChanged_1(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void comboBoxSoftwareLicense_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void comboBoxSoftwareSala_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            CheckSubmitSoftware();
        }

        private void buttonSoftware_Click(object sender, EventArgs e)
        {
            bool canRegisterSoftware = true;

            //VERIFICAR SE DateTime RESULTANTE ULTRAPASSA O DateTime DO SISTEMA
            if (dateTimePickerSoftwareDate.Value.Add(dateTimePickerSoftwareHour.Value.TimeOfDay) > DateTime.Now)
            {
                Software();
                labelSoftwareError.Visible = true;
                labelSoftwareError.Text = "ERRO! Data/Hora Inválida.";
                canRegisterSoftware = false;
            }

            
            //VERIFICAR SE JÁ EXISTE UM FILE COM O NOME DA SALA
            if (!File.Exists(comboBoxSoftwareSala.Text + ".txt")    &&  canRegisterSoftware == true)
            {
                StreamWriter sw = File.CreateText(comboBoxSoftwareSala.Text + ".txt");
                sw.WriteLine(textBoxSoftwareName.Text + ";" + dateTimePickerSoftwareDate.Text + ";" + dateTimePickerSoftwareHour.Text + ";" + comboBoxSoftwareLicense.Text);
                sw.Close();
                Software();
                labelSoftwareSuccess.Visible = true;
            }
            else if(File.Exists(comboBoxSoftwareSala.Text + ".txt") && canRegisterSoftware == true)
            {
                //VERIFICAR SE O SOFTWARE JÁ ESTÁ INSTALADO NA SALA
                string[] linhas = File.ReadAllLines(comboBoxSoftwareSala.Text + ".txt");
                for (int i = 0; i < linhas.Length; i++) 
                {
                    if(linhas[i].Split(';')[0] == textBoxSoftwareName.Text)
                    {
                        canRegisterSoftware = false;
                    }
                }

                if(canRegisterSoftware == true)
                {
                    StreamWriter sw = File.AppendText(comboBoxSoftwareSala.Text + ".txt");
                    sw.WriteLine(textBoxSoftwareName.Text + ";" + dateTimePickerSoftwareDate.Text + ";" + dateTimePickerSoftwareHour.Text + ";" + comboBoxSoftwareLicense.Text);
                    sw.Close();
                    Software();
                    labelSoftwareSuccess.Visible = true;
                }
                else
                {
                    Software();
                    labelSoftwareError.Visible = true;
                    labelSoftwareError.Text = "ERRO!\n Software já registado nesta sala.";

                }
            }

        }

        private void groupBoxSoftware_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //VERIFICAR QUE FILE DOS TOPICOS EXISTE
            if (!File.Exists(topicos))
            {
                StreamWriter sw = File.CreateText(topicos);
                sw.WriteLine("01;Pedido de Instalação de Software");
                sw.WriteLine("02;Pedido de Configuração de Software");
                sw.WriteLine("03;Computador avariado");
                sw.WriteLine("04;Cabos");
                sw.WriteLine("05;Outro");
                sw.Close();
            }
            Topicos();

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Salas();
        }

        private void radioButtonTopicsNew_CheckedChanged(object sender, EventArgs e)
        {
            CheckTopicMode();
        }

        private void radioButtonTopicsEdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckTopicMode();
        }

        private void comboBoxTopicsID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] linhas = File.ReadAllLines(topicos);

            for (int i = 0; i < linhas.Length; i++)
            {   
                if(radioButtonTopicsEdit.Checked == true)
                {
                    if (linhas[i].Split(';')[0] == comboBoxTopicsID.SelectedItem.ToString())
                    {
                        textBoxTopicsTopic.Text = linhas[i].Split(';')[1];
                    }
                }
                
            }
        }

        private void buttonTopicsRemove_Click(object sender, EventArgs e)
        {
            //REMOVER TOPICO DA DATAGRID
            for (int i = 0; i < dataGridViewTopics.RowCount; i++)
            {
                if(dataGridViewTopics.Rows[i].Cells[0].Value.ToString() == comboBoxTopicsID.SelectedItem.ToString()   && dataGridViewTopics.Rows[i].Cells[1].Value.ToString() == textBoxTopicsTopic.Text)
                {
                    dataGridViewTopics.Rows.RemoveAt(i);
                    break;
                }
            }

            //ATUALIZAR FILE TOPICOS
            File.Delete(topicos);

            

            StreamWriter sw = File.AppendText(topicos);
            for (int i = 0; i < dataGridViewTopics.RowCount-1; i++)
            {
                sw.WriteLine(dataGridViewTopics.Rows[i].Cells[0].Value + ";" + dataGridViewTopics.Rows[i].Cells[1].Value);
            }
            sw.Close();

            //RESET Á COMBOBOX DE IDS DE TOPICO
            comboBoxTopicsID.Items.Clear();

            Topicos();
        }

        private void buttonTopicsConfirm_Click(object sender, EventArgs e)
        {
            if (radioButtonTopicsNew.Checked == true)
            {
                
                string[] linhas = File.ReadAllLines(topicos);
                int[] arrayID = new int[linhas.Length];

                for (int i = 0; i < linhas.Length; i++)
                {
                    arrayID[i] = Convert.ToInt32(linhas[i].Split(';')[0]);
                }

                string newID = "0" + (arrayID.Max()+1).ToString(); //NOVO ID QUE NUNCA VAI SER REPETIDO

                StreamWriter sw = File.AppendText(topicos);
                sw.WriteLine(newID +";"+ textBoxTopicsTopic.Text);
                sw.Close();

                //RESET Á COMBOBOX DE IDS DE TOPICO
                comboBoxTopicsID.Items.Clear();

                Topicos();

            }
            else if (radioButtonTopicsEdit.Checked == true)
            {
                //ALTERAR TOPICO DA DATAGRID
                for (int i = 0; i < dataGridViewTopics.RowCount; i++)
                {
                    if (dataGridViewTopics.Rows[i].Cells[0].Value.ToString() == comboBoxTopicsID.SelectedItem.ToString())
                    {
                        dataGridViewTopics.Rows[i].Cells[1].Value = textBoxTopicsTopic.Text;
                        break;
                    }
                }

                //ATUALIZAR FILE TOPICOS
                File.Delete(topicos);



                StreamWriter sw = File.AppendText(topicos);
                for (int i = 0; i < dataGridViewTopics.RowCount - 1; i++)
                {
                    sw.WriteLine(dataGridViewTopics.Rows[i].Cells[0].Value + ";" + dataGridViewTopics.Rows[i].Cells[1].Value);
                }
                sw.Close();

                //RESET Á COMBOBOX DE IDS DE TOPICO
                comboBoxTopicsID.Items.Clear();

                Topicos();
            }
        }

        private void textBoxTopicsTopic_TextChanged(object sender, EventArgs e)
        {
            if(textBoxTopicsTopic.Text == "")
            {
                buttonTopicsConfirm.Enabled = false;
            }
            else
            {
                buttonTopicsConfirm.Enabled = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Notifications();
        }

        private void groupBoxSalas_Enter(object sender, EventArgs e)
        {

        }

        private void listBoxSalasNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RESET E PREENCHER SOFTWARE
            listBoxSalasSoftware.Items.Clear();

            if(listBoxSalasNumber.SelectedIndex != -1)
            {
                string[] lines = File.ReadAllLines(listBoxSalasNumber.SelectedItem.ToString() + ".txt");
                for (int i = 0; i < lines.Length; i++)
                {
                    listBoxSalasSoftware.Items.Add(lines[i].Split(';')[0]);
                }
            }
            

            if(radioButtonSalasEdit.Checked == true)
            {
                if (listBoxSalasNumber.SelectedIndex != -1)
                {
                    textBoxSalasID.Text = listBoxSalasNumber.SelectedItem.ToString();
                }
                    
            }

            if (listBoxSalasSoftware.SelectedIndex != -1)
            {
                buttonSalasSoftwareRemove.Enabled = true;
            }
            else
            {
                buttonSalasSoftwareRemove.Enabled = false;
            }
        }

        private void textBoxSalasID_TextChanged(object sender, EventArgs e)
        {
            if(textBoxSalasID.Text== "")
            {
                buttonSalasConfirm.Enabled = false;
            }
            else
            {
                buttonSalasConfirm.Enabled = true;
            }
        }

        private void radioButtonSalasNew_CheckedChanged(object sender, EventArgs e)
        {
            CheckSalasMode();
        }

        private void radioButtonSalasEdit_CheckedChanged(object sender, EventArgs e)
        {
            CheckSalasMode();
        }

        private void buttonSalasConfirm_Click(object sender, EventArgs e)
        {
            //FALTA IMPEDIR Q SEJA SUBMETIDA UMA SALA IDENTICA
            if(radioButtonSalasEdit.Checked == true) //EDITAR
            {
                    if (CheckDuplicateSalas() == true)
                    {
                        Salas();
                        labelSalasError.Visible = true;
                    }
                    else
                    {
                        File.Delete(salas);
                        File.Move(listBoxSalasNumber.SelectedItem.ToString() + ".txt", textBoxSalasID.Text + ".txt");
                        File.Delete(listBoxSalasNumber.SelectedItem.ToString() + ".txt");
                        
                        listBoxSalasNumber.Items[listBoxSalasNumber.SelectedIndex] = textBoxSalasID.Text;
                        StreamWriter sw = File.CreateText(salas);
                        for (int i = 0; i < listBoxSalasNumber.Items.Count; i++)
                        {
                            sw.WriteLine(listBoxSalasNumber.Items[i].ToString());
                        }
                        sw.Close();

                        

                        Salas();
                        labelSalasSuccess.Visible = true;
                    } 
            }
            else if(radioButtonSalasNew.Checked == true) //NOVA SALA
            {
                if (CheckDuplicateSalas() == true)
                {
                    Salas();
                    labelSalasError.Visible = true;
                }
                else
                {
                    StreamWriter sw = File.AppendText(salas);
                    sw.WriteLine(textBoxSalasID.Text);
                    sw.Close();

                    Salas();
                    labelSalasSuccess.Visible = true;
                }
                
            }
        }

        private void buttonSalasRemove_Click(object sender, EventArgs e)
        {
            File.Delete(salas);
            File.Delete(listBoxSalasNumber.Items[listBoxSalasNumber.SelectedIndex].ToString() + ".txt");
            listBoxSalasNumber.Items.RemoveAt(listBoxSalasNumber.SelectedIndex);

            StreamWriter sw = File.CreateText(salas);
            for (int i = 0; i < listBoxSalasNumber.Items.Count; i++)
            {
                sw.WriteLine(listBoxSalasNumber.Items[i].ToString());
            }
            sw.Close();


            Salas();
            labelSalasSuccess.Visible = true;
        }

        private void listBoxSalasSoftware_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBoxSalasSoftware.SelectedIndex != -1)
            {
                buttonSalasSoftwareRemove.Enabled = true;
            }
            else
            {
                buttonSalasSoftwareRemove.Enabled = false;
            }
        }

        private void buttonSalasSoftwareRemove_Click(object sender, EventArgs e)
        {

            string[] linhas = File.ReadAllLines(listBoxSalasNumber.SelectedItem.ToString() + ".txt");

            StreamWriter sw = File.CreateText(listBoxSalasNumber.SelectedItem.ToString() + ".txt");

            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Split(';')[0] != listBoxSalasSoftware.SelectedItem.ToString())
                {
                    sw.WriteLine(linhas[i]);
                }
            }
            sw.Close();

            RefreshSalasLists();
            
        }

        private void dataGridViewNotifications_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridViewNotifications_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            CheckSubmitComment();

            ListComments();

            if(dataGridViewNotifications.SelectedRows.Count > 0)
            {
                if (dataGridViewNotifications.SelectedRows[0].Cells[6].Value.ToString() == "Pendente")
                {
                    buttonNotificationsConcluir.Enabled = true;
                }
                else
                {
                    buttonNotificationsConcluir.Enabled = false;
                }
            }
            
        }

        private void textBoxNotificationsComment_TextChanged(object sender, EventArgs e)
        {
            CheckSubmitComment();
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

        private void buttonNotificationsConcluir_Click(object sender, EventArgs e)
        {
            string[] linhas = File.ReadAllLines(notifications);
            File.Delete(notifications);

            StreamWriter sw = File.CreateText(notifications);
            for (int i = 0; i < linhas.Length; i++)
            {
                if (linhas[i].Split(';')[0] == dataGridViewNotifications.SelectedRows[0].Cells[0].Value.ToString())
                {
                    sw.WriteLine(linhas[i].Replace(";Pendente;", ";Concluído;"));
                }
                else
                {
                    sw.WriteLine(linhas[i]);
                }
            }
            sw.Close();

            RefreshNotificationsDatagrid();
            if (dataGridViewNotifications.SelectedRows[0].Cells[6].Value.ToString() == "Pendente")
            {
                buttonNotificationsConcluir.Enabled = true;
            }
            else
            {
                buttonNotificationsConcluir.Enabled = false;
            }

            CountNotificationsPendentes();
        }

        private void labelNotificationCount_Click(object sender, EventArgs e)
        {
            
        }

        private void labelNotificationCount_TextChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(labelNotificationCount.Text) > 0)
            {
                labelNotificationCount.Visible = true;
            }
            else
            {
                labelNotificationCount.Visible = false;
            }
        }

        private void comboBoxFilterSala_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshNotificationsDatagrid();
            if (comboBoxFilterSala.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[2].Value.ToString() != comboBoxFilterSala.SelectedItem.ToString())
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (comboBoxFilterDocente.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[1].Value.ToString() != comboBoxFilterDocente.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }

            for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
            {
                if (Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) > dateTimePickerFilterEndDate.Value || Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) < dateTimePickerFilterStartDate.Value)
                {
                    dataGridViewNotifications.Rows.RemoveAt(i);
                    i--;
                }
            }

        }

        private void dateTimePickerFilterStartDate_ValueChanged(object sender, EventArgs e)
        {
            RefreshNotificationsDatagrid();
            if (comboBoxFilterSala.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[2].Value.ToString() != comboBoxFilterSala.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (comboBoxFilterDocente.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[1].Value.ToString() != comboBoxFilterDocente.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
            {
                if (Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) > dateTimePickerFilterEndDate.Value || Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) < dateTimePickerFilterStartDate.Value)
                {
                    dataGridViewNotifications.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        private void dateTimePickerFilterEndData_ValueChanged(object sender, EventArgs e)
        {
            RefreshNotificationsDatagrid();
            if (comboBoxFilterSala.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[2].Value.ToString() != comboBoxFilterSala.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (comboBoxFilterDocente.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[1].Value.ToString() != comboBoxFilterDocente.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
            {
                if (Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) > dateTimePickerFilterEndDate.Value || Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) < dateTimePickerFilterStartDate.Value)
                {
                    dataGridViewNotifications.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        private void comboBoxFilterDocente_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshNotificationsDatagrid();
            if (comboBoxFilterSala.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[2].Value.ToString() != comboBoxFilterSala.Text)
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            if (comboBoxFilterDocente.SelectedIndex != 0)
            {
                for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
                {
                    if (dataGridViewNotifications.Rows[i].Cells[1].Value.ToString() != comboBoxFilterDocente.SelectedItem.ToString())
                    {
                        dataGridViewNotifications.Rows.RemoveAt(i);
                        i--;
                    }
                }
            }
            for (int i = 0; i < dataGridViewNotifications.RowCount; i++)
            {
                if (Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) > dateTimePickerFilterEndDate.Value || Convert.ToDateTime(dataGridViewNotifications.Rows[i].Cells[4].Value.ToString()) < dateTimePickerFilterStartDate.Value)
                {
                    dataGridViewNotifications.Rows.RemoveAt(i);
                    i--;
                }
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form login = new FormLogin();
            login.Show();
            File.Delete(currentUser);
        }

        private void FormDashboardSI_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
