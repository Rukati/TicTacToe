using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        List<List<PictureBox>> pictureBoxes = new List<List<PictureBox>>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game(false); 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected void game(bool bot)
        {

            MyPictureBox MyPicBox = new MyPictureBox(bot);
            MyPicBox.CreateSquares(ref pictureBoxes);
            show_picture_box();
        }

        protected void show_picture_box()
        {
            foreach (var row in pictureBoxes)
            {
                foreach (var pictureBox in row)
                {
                    Controls.Add(pictureBox);
                    pictureBox.BringToFront();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            game(true);
        }
    }
}
