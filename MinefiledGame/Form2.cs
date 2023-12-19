using mayin_tarlasi_3;
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
    public partial class Form2 : Form
    {
        public string getUserName { get; set; }
        private int fullmine = 30;
        private int remainingFlags = 30;
        private int remainingTimeInSeconds = 0;
        MineField minefield;
        Image minePicture = Image.FromFile(@"mayin.jpg");
        private List<Mine> bomb;
        int cleanArea;
        int totalArea= 400 ;
        private int score = 0;
        private List<Image> flagImages = new List<Image>();

        public  string UserName { get; internal set; }
        


        public Form2()
        {
            
            InitializeComponent();
            

        }


        private void Form2_Load(object sender, EventArgs e)
        {
            
            StartNewGame(new Size(400, 400), 30);
            label3.Text = getUserName;
            label3.Text = Properties.Settings.Default.Username;
            UpdateScoreLabel();

        }



        void btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (sender as Button);

            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    if (btn.BackgroundImage == null)
                    {
                        if (flagImages.Count < 10)
                        {
                            AddFlag();
                            Image picture = Image.FromFile("flag.jpg");
                            flagImages.Add(picture);
                            btn.BackgroundImage = flagImages[flagImages.Count - 1];
                            btn.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                    }
                    else
                    {
                        RemoveFlag();
                        btn.BackgroundImage = null;
                    }
                }
                catch (Exception)
                {

                }

            }
        }




        private void AddFlag()
        {

            if (remainingFlags > 0)
            {
                remainingFlags--;
                UpdateBayrakLabel();
            }


        }

        private void RemoveFlag()
        {
            if (remainingFlags <= 30)
            {
                remainingFlags++;
                UpdateBayrakLabel();

            }

        }

        private void UpdateBayrakLabel()
        {

            lblBayrakSayisi.Text = remainingFlags.ToString();
        }

        private void StartNewGame(Size fieldSize, int numBombs)
        {
            minefield = new MineField(fieldSize, numBombs);
            panel1.Size = minefield.HeightPanel;
            panel1.Controls.Clear();
            cleanArea = 0;
            addBomb();
        }

        private void UpdateScore(int points)
        {
            score += points;
            UpdateScoreLabel();
        }

        private void UpdateScoreLabel()
        {
            lblScore.Text = score.ToString();
        }


        //Bomba ekleme yeri
        public void addBomb()
        {
            for (int x = 0; x < panel1.Width; x += 20)
            {
                for (int y = 0; y < panel1.Height; y += 20)
                {
                    addButton(new Point(x, y));
                }
            }

        }
        //Buton ekleme
        public void addButton(Point loc)
        {
            Button btn = new Button()
            {
                Name = loc.X + "" + loc.Y,
                Size = new Size(20, 20),
                Location = loc,
                BackColor = Color.White,
            };

            btn.Click += Btn_Click;
            btn.MouseUp += new MouseEventHandler(btn_MouseUp);

            panel1.Controls.Add(btn);
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            timer1.Start();
            Button btn = (sender as Button);

            Mine myn = minefield.ShowLocMine(btn.Location);
            bomb = new List<Mine>();

            if (myn.AnyMines)
            {
                MessageBox.Show("You Lost try again !!!");
                showallbombs();
                timer1.Stop();
                panel1.Enabled = false;
            }
            else
            {
                int s = BombsCount(myn);

                if (s == 0)
                {
                    bomb.Add(myn);
                    for (int i = 0; i < bomb.Count; i++)
                    {
                        Mine item = bomb[i];
                        if (item != null)
                        {
                            if (item.Looked == false && item.AnyMines == false)
                            {
                                Button btnx = (Button)panel1.Controls.Find(item.GetLocation.X + "" + item.GetLocation.Y, false)[0];
                                if (BombsCount(bomb[i]) == 0)
                                {
                                    btnx.Enabled = false;
                                    CleanEnvironment(item);
                                }
                                else
                                {
                                    btnx.Text = BombsCount(item).ToString();
                                }
                                cleanArea++;
                                item.Looked = true;
                            }
                        }
                    }
                    // 
                    if (s > 0)
                    {
                        score += s; 
                        UpdateScoreLabel();
                    }
                }
                else
                {
                    btn.Text = s.ToString();
                    cleanArea++;
                    score += s; 
                    UpdateScoreLabel();
                }
            }

            IsCheckWon();
        }






        //Mayın sayısı
        public int BombsCount(Mine m)
        {
            int sayi = 0;
            Point[] surroundingdot =
            {
                     new Point(-20, 0),
                     new Point(20, 20),
                     new Point(20, 0),
                     new Point(-20, -20),
                     new Point(0, -20),
                     new Point(-20, 20),
                     new Point(0, 20),
                     new Point(20, -20)

            };

            foreach (Point dot in surroundingdot)
            {
                int newX = m.GetLocation.X + dot.X;
                int newY = m.GetLocation.Y + dot.Y;

                if (newX >= 0 && newX < panel1.Width && newY >= 0 && newY < panel1.Height)
                {
                    if (minefield.ShowLocMine(new Point(newX, newY)).AnyMines)
                    {
                        sayi++;
                    }
                }
            }

            return sayi;
        }

        //tıklanan yeri genişletme
        public void CleanEnvironment(Mine m)
        {
            int[] locationX = { 1, 0, -1, 0 };
            int[] locationY = { 0, -1, 0, 1 };

            Point currentLocation = m.GetLocation;

            for (int i = 0; i < 4; i++)
            {
                int newX = currentLocation.X + locationX[i] * 20;
                int newY = currentLocation.Y + locationY[i] * 20;

                if (IsWithinPanelBounds(newX, newY))
                {
                    Button btn = GetButtonAtLocation(newX, newY);
                    if (btn != null)
                    {
                        bomb.Add(minefield.ShowLocMine(new Point(newX, newY)));
                        btn.BackColor = Color.Gray;
                    }
                }
            }
        }

        private bool IsWithinPanelBounds(int x, int y)
        {
            return x >= 0 && x < panel1.Width && y >= 0 && y < panel1.Height;
        }

        private Button GetButtonAtLocation(int x, int y)
        {
            string controlName = x + "" + y;
            Control[] foundControls = panel1.Controls.Find(controlName, false);

            if (foundControls.Length > 0 && foundControls[0] is Button btn)
            {
                return btn;
            }

            return null;
        }


        //Oyunu kazanıp kazanmadığını kontrol eden method
        private void IsCheckWon()
        {
            if (cleanArea == totalArea - fullmine)
            {
                MessageBox.Show("Congratulations you won the game");
                timer1.Stop();
                panel1.Enabled = false;
                //skoru kullanıcı oyun kazandıktan sonra veritabanına kaydettim
                int score = Convert.ToInt32(lblScore.Text); 
                SaveScoreToDatabase(score);
            }
          
        }

        //Bombaların yerini gösterme
        public void showallbombs()
        {
            foreach (Mine item in minefield.gettallMayin)
            {
                if (item.AnyMines)
                {
                    Button btn = (Button)panel1.Controls.Find(item.GetLocation.X + "" + item.GetLocation.Y, false)[0];
                    btn.BackgroundImage = minePicture;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;

                }

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            StartNewGame(new Size(400, 400), 20);

            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            remainingTimeInSeconds++;

            UpdateTimeLabel();

            

        }
        private void UpdateTimeLabel()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(remainingTimeInSeconds);
            label1.Text = timeSpan.ToString(@"mm\:ss");
        }


       

        private void EasyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame(new Size(200, 200), 10);
            showallbombs();
            IsCheckWon();
            remainingFlags = 10;
            UpdateBayrakLabel();
           
            
            panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, (this.ClientSize.Height - panel1.Height) / 2);
            

        }


        private void MiddleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame(new Size(400, 400), 20);
            showallbombs();
            remainingFlags = 10;
            UpdateBayrakLabel();
            
            
            IsCheckWon();
            panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, (this.ClientSize.Height - panel1.Height) / 2);

        }

        private void HardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartNewGame(new Size(440, 440), 70);
            showallbombs();
            remainingFlags = 70;
            UpdateBayrakLabel();
            
            
            addBomb();
            IsCheckWon();
            //Paneli formun ortasına ortalamak için 
            panel1.Location = new Point((this.ClientSize.Width - panel1.Width) / 2, (this.ClientSize.Height - panel1.Height) / 2);

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }



        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void quitTheGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            Application.Exit();
        }

        private void restartTheGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show();
            this.Hide();
        }

        private void viewUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form3 frm3 = new Form3();
            frm3.Show();

        }


        private void SaveScoreToDatabase(int score)
        {
            using (SQLiteConnection connection = new SQLiteConnection("Data source =.\\vt2.db3 ; version=3"))
            {
                connection.Open();

                string username = label3.Text;

                using (SQLiteCommand cmd = new SQLiteCommand("INSERT INTO users2 (Ad, Skor) VALUES (@Username, @Score)", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Score", score);
                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

    }

}
