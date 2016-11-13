using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Npgsql;

namespace task_1
{
    public partial class Form1 : Form
    {
        private DataSet ds = new DataSet();
        private DataSet dsStatus = new DataSet();
        private DataTable dt = new DataTable();
        private DataTable dtStatus = new DataTable();
        private string dateFrom = "";
        private string dateTo = "";
        private string selectedState = "";
        private string selectedStatus = "";
        private string selectedEmploy = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Startup(string server, string port, string user, string pass, string db)
        {
            try
            {
                string connectionString = String.Format("Server={0};Port={1};User Id={2};Password={3};Database={4}",
                                                         server, port, user, pass, db);
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                connection.Open();
                string sqlResult = "SELECT * FROM get_result_query()";
                string sqlStatus = "SELECT * FROM status";
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(sqlResult, connection);
                ds.Reset();
                da.Fill(ds);
                dt = ds.Tables[0];
                da = new NpgsqlDataAdapter(sqlStatus, connection);
                dsStatus.Reset();
                da.Fill(dsStatus);
                dtStatus = dsStatus.Tables[0];
                DataRow[] drStatusArray = dtStatus.Select();
                for (int i = 0; i < drStatusArray.Length; i++)
                {
                    comboBox2.Items.Add(drStatusArray[i]["name"].ToString());
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
                selectedState = comboBox1.SelectedItem.ToString();
                selectedStatus = comboBox2.SelectedItem.ToString();
                selectedEmploy = comboBox3.SelectedItem.ToString();
                dateFrom = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                dateTo = dateTimePicker2.Value.ToString("yyyy-MM-dd");
                dataGridView1.DataSource = dt;
                connection.Close();

                dataGridView1.Visible = true;
                dataGridView1.Enabled = true;
                comboBox1.Visible = true;
                comboBox1.Enabled = true;
                comboBox2.Visible = true;
                comboBox2.Enabled = true;
                comboBox3.Visible = true;
                comboBox3.Enabled = true;
                findByTextBox.Visible = true;
                findByTextBox.Enabled = true;
                findButton.Visible = true;
                findButton.Enabled = true;
                clearButton.Visible = true;
                clearButton.Enabled = true;
                button1.Visible = true;
                button1.Enabled = true;
                tableLayoutPanel3.Visible = true;
                tableLayoutPanel3.Enabled = true;
                label1.Visible = true;
                label1.Enabled = true;
                label2.Visible = true;
                label2.Enabled = true;
                label3.Visible = true;
                label3.Enabled = true;
                label4.Visible = true;
                label4.Enabled = true;
                label5.Visible = true;
                label5.Enabled = true;
                label6.Visible = true;
                label6.Enabled = true;
                label7.Visible = true;
                label7.Enabled = true;
                dateTimePicker1.Visible = true;
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Visible = true;
                dateTimePicker2.Enabled = true;
            }
            catch (Exception msg)
            {
                MessageBox.Show(msg.ToString());
            }
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Utils.findByLastname(findByTextBox.Text, selectedState, dt);
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            findByTextBox.Text = "";
            dataGridView1.DataSource = dt;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedState = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedStatus = comboBox2.SelectedItem.ToString();
        
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateFrom = dateTimePicker1.Value.ToString("yyyy-MM-dd");
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            dateTo = dateTimePicker2.Value.ToString("yyyy-MM-dd");
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedEmploy = comboBox3.SelectedItem.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            label6.Text = Utils.numberOfEmployesInStatus(selectedStatus, dateFrom, dateTo, selectedEmploy, dt).ToString();
        }

        private void defaultStartButton_Click(object sender, EventArgs e)
        {
            Startup("95.213.204.15", "5432", "testuser", "testuser", "kek");
            defaultStartButton.Visible = false;
            defaultStartButton.Enabled = false;
            button2.Visible = false;
            button2.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string configFilePath = openFileDialog1.FileName;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = configFilePath;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            string server = config.AppSettings.Settings["server"].Value;
            string port = config.AppSettings.Settings["port"].Value;
            string user = config.AppSettings.Settings["user"].Value;
            string pass = config.AppSettings.Settings["pass"].Value;
            string db = config.AppSettings.Settings["db"].Value;
            Startup(server, port, user, pass, db);
            defaultStartButton.Visible = false;
            defaultStartButton.Enabled = false;
            button2.Visible = false;
            button2.Enabled = false;
        }
    }

    public class Utils
    {
        public static DataTable findByLastname(string findBy, string selectedState, DataTable dt)
        {
            DataTable returnDt = new DataTable();
            DataRow[] drArray = dt.Select(selectedState + " LIKE '%" + findBy + "%'");
            if (drArray.Length > 0)
            {
                returnDt = drArray.CopyToDataTable();
            }
            else
            {
                MessageBox.Show("Invalid lastname.");
            }
            return returnDt;
        }

        public static int numberOfEmployesInStatus(string statusName, string dateFrom, string dateTo, string selectedEmploy, DataTable dt)
        {
            DataRow[] drArray = dt.Select("status_name LIKE '" + statusName + 
                "' AND " + selectedEmploy + " > #" + dateFrom + "#" + 
                " AND " + selectedEmploy + " < #" + dateTo + "#");
            return drArray.Length;
        }
    }
}
