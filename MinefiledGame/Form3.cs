using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace mayin_tarlasi_3
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
       public static SQLiteConnection connection = new SQLiteConnection("Data source =.\\vt2.db3 ; version=3");
        SQLiteConnection con;
        SQLiteDataAdapter da;
        SQLiteCommand cmd;
        DataTable dt;
        private DataGridViewRow selectedRow;


        private void GetList()
        {
            SQLiteCommand command = new SQLiteCommand("select * from users2", connection);
            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter(command);
            dt = new DataTable();
            dataAdapter.Fill(dt);

            dataGridView1.DataSource = dt;


            int highestScore = 0;
            foreach (DataRow row in dt.Rows)
            {
                int score = Convert.ToInt32(row["Skor"]);
                if (score > highestScore)
                {
                    highestScore = score;
                }
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int score = Convert.ToInt32(row.Cells["Skor"].Value);
                if (score == highestScore)
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
            }
        }


        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }


        private void Form3_Load(object sender, EventArgs e)
        {
            connection.Open();
            GetList();
            connection.Close();
            


        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DeleteSelectedRecord();
            GetList();

          
        }
        private void DeleteSelectedRecord()
        {
            if (selectedRow != null)
            {
                int userId = Convert.ToInt32(selectedRow.Cells["id"].Value); 
                using (SQLiteConnection connection = new SQLiteConnection("Data source =.\\vt2.db3 ; version=3"))
                {
                    connection.Open();
                    string query = "DELETE FROM users2 WHERE id = @UserId";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Record deleted successfully.");
                           
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete record.");
                        }
                    }
                }
            }
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRow = dataGridView1.SelectedRows[0];
            }
            else
            {
                selectedRow = null;
            }

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }

