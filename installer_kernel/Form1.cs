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

namespace installer_kernel
{
    public partial class WinConfig : Form
    {

        public String URL = "project";

        public void getParam(dynamic jsonObj) 
        {
            textBox1.Text = jsonObj["name"];
            textBox2.Text = jsonObj["company"];
            comboBox1.Text = jsonObj["clanguage"];
                if (jsonObj["assets"]["jquery"] == "1.x") radioButton1.Checked = true;
                if (jsonObj["assets"]["jquery"] == "2.x") radioButton2.Checked = true;
                if (jsonObj["assets"]["jquery"] == "3.x") radioButton3.Checked = true;
            textBox3.Text = jsonObj["db_config"]["db_host"];
            textBox5.Text = jsonObj["db_config"]["db_user"];
            textBox6.Text = jsonObj["db_config"]["db_pass"];
            textBox4.Text = jsonObj["db_config"]["db_name"];
        }

        public WinConfig()
        {
            InitializeComponent();

            if (System.IO.File.Exists(URL + "/core/manifest.json"))
            {
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"" + URL + "/core/manifest.json"));
                status.Text = "Файлы для конфигурации найдены.\nТекущая версия: " + jsonObj["version"];
                status.ForeColor = Color.Green;
                    
            }else {

                using (var dialog = new FolderBrowserDialog())
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        URL = dialog.SelectedPath;

                        if (System.IO.File.Exists(URL + "/core/manifest.json"))
                        {
                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"" + URL + "/core/manifest.json"));
                            status.Text = "Файлы для конфигурации найдены.\nТекущая версия: " + jsonObj["version"];
                            status.ForeColor = Color.Green;

                            perebor_updates(URL, "project");
                        }
                        else
                        {
                            MessageBox.Show("Файлы Kernel(не найдены);.", "Уведомление", MessageBoxButtons.OK);

                            status.Text = "Файлы для конфигурации не найдены.";
                            status.ForeColor = Color.Red;
                            button1.Enabled = false;
                        }

                    }
                    else
                    {

                        status.Text = "Файлы для конфигурации не найдены.";
                        status.ForeColor = Color.Red;
                        button1.Enabled = false;

                    }
                
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        public void perebor_updates(string begin_dir, string end_dir)
        {
            DirectoryInfo dir_inf = new DirectoryInfo(begin_dir);
            foreach (DirectoryInfo dir in dir_inf.GetDirectories())
            {
                if (Directory.Exists(end_dir + "\\" + dir.Name) != true)
                {
                    Directory.CreateDirectory(end_dir + "\\" + dir.Name);
                }
                perebor_updates(dir.FullName, end_dir + "\\" + dir.Name);
            }
            foreach (string file in Directory.GetFiles(begin_dir))
            {
                string filik = file.Substring(file.LastIndexOf('\\'), file.Length - file.LastIndexOf('\\'));
                File.Copy(file, end_dir + "\\" + filik, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string message = "Вы уверены что хотите создать проект с данной конфигурацией?";
            string caption = "Подтверждение конфигурации";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            //MAIN VARIABLES
            String PROJECT_NAME = textBox1.Text;
            String PROJECT_COMPANY = textBox2.Text;
            String PROJECT_LANGUAGE = (comboBox1.Text == "Русский (RU_ru)") ? "ru" : "en";
            String PROJECT_JQUERY = (radioButton1.Checked) ? "1.x" : "2.x";
            PROJECT_JQUERY = (radioButton3.Checked) ? "3.x" : PROJECT_JQUERY;
            //--------------
            //DB VARIABLES
            String DB_HOST = textBox3.Text;
            String DB_USER = textBox5.Text;
            String DB_PASS = textBox6.Text;
            String DB_NAME = textBox4.Text;
            //------------


            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"" + URL + "/core/manifest.json"));
            jsonObj["name"] = PROJECT_NAME;
            jsonObj["company"] = PROJECT_COMPANY;
            jsonObj["clanguage"] = PROJECT_LANGUAGE;
            jsonObj["assets"]["jquery"] = PROJECT_JQUERY;
            jsonObj["db_config"]["db_host"] = DB_HOST;
            jsonObj["db_config"]["db_user"] = DB_USER;
            jsonObj["db_config"]["db_pass"] = DB_PASS;
            jsonObj["db_config"]["db_name"] = DB_NAME;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(@"" + URL + "/core/manifest.json", output);

            result = MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                string PATH = null;
                using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    PATH = dialog.SelectedPath;

                    perebor_updates(URL,PATH);

                    MessageBox.Show("Операция успешно завершена.", "Уведомление", MessageBoxButtons.OK);
                }
            }
        }

        

        private void status_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            string message = "Вы уверены что хотите обновить файлы Kernel(";
            string caption = "Подтверждение обновления конфигурации";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            

            

                string PATH = null;
                using (var dialog = new FolderBrowserDialog())
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        PATH = dialog.SelectedPath;

                        if (System.IO.File.Exists(PATH + "/core/manifest.json"))
                        {

                            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(@"" + PATH + "/core/manifest.json"));
                            result = MessageBox.Show(message + jsonObj["version"] + ");?", caption, buttons);

                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {

                                perebor_updates(PATH, "project");

                                status.Text = "Файлы для конфигурации найдены.\nТекущая версия: " + jsonObj["version"];
                                status.ForeColor = Color.Green;

                                MessageBox.Show("Операция успешно завершена.", "Уведомление", MessageBoxButtons.OK);

                            }
                            else
                            {
                                MessageBox.Show("Операция отменена.", "Уведомление", MessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Файлы Kernel(не найдены);.", "Уведомление", MessageBoxButtons.OK);
                        }

                        
                    }
            }

        }
    }

