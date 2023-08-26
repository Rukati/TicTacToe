using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    internal class MyPictureBox : Form1
    {

        private bool currentPlayerX = true; // По умолчанию начинает игрок X
        List<List<PictureBox>> pictureBoxes = new List<List<PictureBox>>();
        public void CreateSquares(ref List<List<PictureBox>> pictureBoxes)
        {
            this.pictureBoxes = pictureBoxes;

            int rowCount = 3; // Количество строк квадратов
            int colCount = 3; // Количество столбцов квадратов

            int squareSize = 150; // Размер одного квадрата

            // Создайте и добавьте квадраты на панель
            for (int i = 0; i < rowCount; i++)
            {
                List<PictureBox> row = new List<PictureBox>();

                for (int j = 0; j < colCount; j++)
                {
                    PictureBox pictureBox = new PictureBox();
                    Label label = new Label();

                    pictureBox.Width = squareSize;
                    pictureBox.Height = squareSize;
                    pictureBox.BackColor = Color.White; // Цвет квадрата
                    pictureBox.Location = new Point(j * squareSize, i * squareSize);
                    pictureBox.BorderStyle = BorderStyle.FixedSingle;

                    pictureBox.MouseEnter += PictureBox_MouseEnter;
                    pictureBox.MouseLeave += PictureBox_MouseLeave;
                    pictureBox.MouseClick += PictureBox_MouseClick;

                    pictureBox.Controls.Add(label);
                    row.Add(pictureBox);
                }

                pictureBoxes.Add(row);
            }
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.LightGray; // Меняем цвет на ярко-зеленый при наведении
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            pictureBox.BackColor = Color.White; // Возвращаем обратно на свой цвет при уходе курсора
        }
        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PictureBox pictureBox = (PictureBox)sender;

                Label label = pictureBox.Controls[0] as Label;
                label.Text = currentPlayerX ? "X" : "O"; // Меняем текст в зависимости от текущего игрока
                label.Dock = DockStyle.Fill; // Занимаем все доступное место внутри PictureBox
                label.Font = new Font("Roboto", 105, FontStyle.Regular); // Устанавливаем новый размер шрифта

                pictureBox.Controls.Add(label);

                if (IsWinner())
                {
                    string player = currentPlayerX ? "крестик" : "нолик";
                    var result = MessageBox.Show($"Победил {player}! Хотите заново?", "Winner", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        menu();
                    }
                    else
                        menu();

                }
                else
                    currentPlayerX = !currentPlayerX; // Меняем текущего игрока на противоположного
            }
        }
        private bool IsWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                // Проверка строки на "X" и "O"
                if ((pictureBoxes[i][0].Controls[0] is Label label) &&
                    (label.Text == "X" || label.Text == "O") &&
                    label.Text == pictureBoxes[i][1].Controls[0].Text &&
                    pictureBoxes[i][1].Controls[0].Text == pictureBoxes[i][2].Controls[0].Text)
                {
                    return true;
                }

                // Проверка столбца на "X" и "O"
                if ((pictureBoxes[0][i].Controls[0] is Label colLabel) &&
                    (colLabel.Text == "X" || colLabel.Text == "O") &&
                    colLabel.Text == pictureBoxes[1][i].Controls[0].Text &&
                    pictureBoxes[1][i].Controls[0].Text == pictureBoxes[2][i].Controls[0].Text)
                {
                    return true;
                }
            }

            // Проверка главной диагонали на "X" и "O"
            if ((pictureBoxes[0][0].Controls[0] is Label diagLabel) &&
                (diagLabel.Text == "X" || diagLabel.Text == "O") &&
                diagLabel.Text == pictureBoxes[1][1].Controls[0].Text &&
                pictureBoxes[1][1].Controls[0].Text == pictureBoxes[2][2].Controls[0].Text)
            {
                return true;
            }

            // Проверка побочной диагонали на "X" и "O"
            if ((pictureBoxes[0][2].Controls[0] is Label antiDiagLabel) &&
                (antiDiagLabel.Text == "X" || antiDiagLabel.Text == "O") &&
                antiDiagLabel.Text == pictureBoxes[1][1].Controls[0].Text &&
                pictureBoxes[1][1].Controls[0].Text == pictureBoxes[2][0].Controls[0].Text)
            {
                return true;
            }

            return false;
        }


    }
}
