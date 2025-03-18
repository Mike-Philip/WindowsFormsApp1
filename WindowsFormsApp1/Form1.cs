using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int blocksX = 110;
        int blocksY = 80;
        int score = 0;
        int level = 3;
        int wins = 0;
        int lose = 0;

        List<PictureBox> pictureBoxes = new List<PictureBox>();
        List<PictureBox> chosenBoxes = new List<PictureBox>();

        Random rand = new Random();

        Color temp;

        int index = 0;
        int tries = 0;

        int timeLimit = 0;
        bool selectingColours = false;
        
        string correctOrder = string.Empty;
        string playerOrder = string.Empty;

        public Form1()
        {
            InitializeComponent();
            SetUpBlocks();
            KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
                this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (selectingColours)
            {
                timeLimit++;

                switch (timeLimit)
                {
                    case 10:
                        temp = chosenBoxes[index].BackColor;
                        chosenBoxes[index].BackColor = Color.White;
                        break;
                    case 20:
                        chosenBoxes[index].BackColor = temp;
                        break;
                    case 30:
                        chosenBoxes[index].BackColor = Color.White;
                        break;
                    case 40:
                        chosenBoxes[index].BackColor = temp;
                        break;
                    case 50:
                        if (index < chosenBoxes.Count - 1)
                        {
                            index++;
                            timeLimit = 0;
                        }
                        else
                        {
                            selectingColours = false;
                        }
                        break;
                }
            }
            if (tries >= level)
            {
                if (correctOrder == playerOrder)
                {
                    tries = 0;
                    GameTimer.Stop();
                    MessageBox.Show("Well done, you got them correctly.", "Game Says: ");
                    score++;
                    wins++;
                }
                else
                {
                    tries = 0;
                    GameTimer.Stop();
                    MessageBox.Show("Your guesses did not match, try again.", "Game Says: ");
                    lose++;
                }
            }
            lblInfo.Text = "Click on " + level + " blocks in the same sequence.";
        }
        private void buttonClickEvent(object sender, EventArgs e)
        {
            if (score == 3 && level < 7)
            {
                level++;
                score = 0;
            }

            correctOrder = string.Empty;
            playerOrder = string.Empty;
            chosenBoxes.Clear();
            chosenBoxes = pictureBoxes.OrderBy(x => rand.Next()).Take(level).ToList();

            for (int i = 0; i < chosenBoxes.Count; i++)
            {
                correctOrder += chosenBoxes[i].Name + " ";
            }

            foreach (PictureBox x in pictureBoxes)
            {
                x.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
            }

            Debug.WriteLine(correctOrder);
            index = 0;
            timeLimit = 0;
            selectingColours = true;
            GameTimer.Start();
        }

        private void SetUpBlocks()
        {
            for (int i = 1; i < 17; i++)
            {
                PictureBox newPic = new PictureBox();
                newPic.Name = "pic_" + i;
                newPic.Height = 60;
                newPic.Width = 60;
                newPic.BackColor = Color.Black;
                newPic.Left = blocksX;
                newPic.Top = blocksY;
                newPic.Click += ClickOnPictureBox;

                if (i == 4 || i == 8 || i == 12)
                {
                    blocksY += 65;
                    blocksX = 110;
                }
                else
                {
                    blocksX += 65;
                }
                this.Controls.Add(newPic);
                pictureBoxes.Add(newPic);
            }
        }

        private void ClickOnPictureBox(object sender, EventArgs e)
        {
            if (!selectingColours && chosenBoxes.Count > 1)
            {
                PictureBox temp = sender as PictureBox;
                temp.BackColor = Color.Black;
                playerOrder += temp.Name + " ";
                Debug.WriteLine(playerOrder);
                tries++;
            }
            else
            {
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wins: " + wins +" \nLose: " + lose, "Game Says: ");
        }
    }
}