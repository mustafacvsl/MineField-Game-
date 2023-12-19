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

namespace MinefieldGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static SQLiteConnection connection = new SQLiteConnection("Data source =.\\vt2.db3 ; version=3");
        SQLiteConnection con;
        SQLiteDataAdapter da;
        SQLiteCommand cmd;
        DataSet ds;

        private void label1_Click(object sender, EventArgs e)
        {

        }
        

        private void timer1_Tick(object sender, EventArgs e)
        {
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                    MessageBox.Show("You cannot enter your username blank.");

            }
            else
            {

                string username = textBox1.Text.Trim();
                int score = 0;

               
                using (SQLiteConnection connection = new SQLiteConnection("Data source =.\\vt2.db3 ; version=3"))
                {
                    connection.Open();

                    try
                    {


                        using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO users2 (Ad, Skor) VALUES (@Username, @Score)", connection))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Score", score);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                               
                            }
                            else
                            {
                                MessageBox.Show("Error.");
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                Properties.Settings.Default.Username = username;
                Properties.Settings.Default.Save();

                Form2 frm2 = new Form2();
                frm2.UserName = username; 
                frm2.Show();
                this.Hide();
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
           


        }
    }
}
