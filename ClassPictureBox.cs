using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    internal class MyPictureBox : PictureBox
    {
        private const string mark = "X";
        private const string zero = "O";

        private bool currentPlayerX = true; // По умолчанию начинает игрок X

        List<List<PictureBox>> pictureBoxes = new List<List<PictureBox>>();

        int move = 1;

        bool bot = new bool();

        public MyPictureBox(bool bot) { this.bot = bot; }

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
                label.Text = currentPlayerX ? mark : zero; // Меняем текст в зависимости от текущего игрока
                label.Dock = DockStyle.Fill; // Занимаем все доступное место внутри PictureBox
                label.Font = new Font("Roboto", 105, FontStyle.Regular); // Устанавливаем новый размер шрифта

                pictureBox.Controls.Add(label);
                
                if (IsWinner())
                {
                    string player = currentPlayerX ? "крестик" : "нолик";
                    var result = MessageBox.Show($"Победил {player}! Хотите заново?", "Winner", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        RestartGame();
                    }
                    else
                    {
                        ResetGame();
                    }
                }
                else if (move == 9)
                {
                    var result = MessageBox.Show("Ничья! Хотите заново?", "Nobody won?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        foreach (var row in pictureBoxes)
                        {
                            foreach (var picBox in row)
                            {
                                label = picBox.Controls[0] as Label;
                                label.Text = ""; // Сбросить текст
                                label.Dock = DockStyle.None; // Сбросить свойство Dock
                                label.Font = SystemFonts.DefaultFont; // Вернуть шрифт по умолчанию
                            }
                        }

                        // Сбросить состояние игрока
                        currentPlayerX = true;
                        move = 1;
                    }
                    else
                    {
                        foreach (var row in pictureBoxes)
                        {
                            foreach (var picBox in row)
                            {
                                picBox.Dispose();
                            }
                        }
                        pictureBoxes.Clear();
                    }
                }
                else
                {
                    if (!bot)
                    {
                        currentPlayerX = !currentPlayerX; // Меняем текущего игрока на противоположного
                        move++;
                    }
                    else
                    {
                        move++;
                        currentPlayerX = !currentPlayerX;
                        BotMove();
                        if (IsWinner())
                        {
                            string player = currentPlayerX ? "крестик" : "нолик";
                            var result = MessageBox.Show($"Победил {player}! Хотите заново?", "Winner", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                RestartGame();
                            }
                            else
                            {
                                ResetGame();
                            }
                        }
                        else
                        {
                            currentPlayerX = !currentPlayerX;
                            move++;
                        }
                    }
                }

            }
        }

        private bool IsWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                // Проверка строки на "X" и "O"
                if ((pictureBoxes[i][0].Controls[0] is Label label) &&
                    (label.Text == mark || label.Text == zero) &&
                    label.Text == pictureBoxes[i][1].Controls[0].Text &&
                    pictureBoxes[i][1].Controls[0].Text == pictureBoxes[i][2].Controls[0].Text)
                {
                    return true;
                }

                // Проверка столбца на "X" и "O"
                if ((pictureBoxes[0][i].Controls[0] is Label colLabel) &&
                    (colLabel.Text == mark || colLabel.Text == zero) &&
                    colLabel.Text == pictureBoxes[1][i].Controls[0].Text &&
                    pictureBoxes[1][i].Controls[0].Text == pictureBoxes[2][i].Controls[0].Text)
                {
                    return true;
                }
            }

            // Проверка главной диагонали на "X" и "O"
            if ((pictureBoxes[0][0].Controls[0] is Label diagLabel) &&
                (diagLabel.Text == mark || diagLabel.Text == zero) &&
                diagLabel.Text == pictureBoxes[1][1].Controls[0].Text &&
                pictureBoxes[1][1].Controls[0].Text == pictureBoxes[2][2].Controls[0].Text)
            {
                return true;
            }

            // Проверка побочной диагонали на "X" и "O"
            if ((pictureBoxes[0][2].Controls[0] is Label antiDiagLabel) &&
                (antiDiagLabel.Text == mark || antiDiagLabel.Text == zero) &&
                antiDiagLabel.Text == pictureBoxes[1][1].Controls[0].Text &&
                pictureBoxes[1][1].Controls[0].Text == pictureBoxes[2][0].Controls[0].Text)
            {
                return true;
            }

            return false;
        }

        private void BotMove()
        {
            List<PictureBox> availableMoves = new List<PictureBox>();

            // Находим все пустые клетки на поле
            foreach (var row in pictureBoxes)
            {
                foreach (var picBox in row)
                {
                    Label label = picBox.Controls[0] as Label;
                    if (label.Text != mark && label.Text != zero)
                    {
                        availableMoves.Add(picBox);
                    }
                }
            }

            // Ищем выигрышные ходы для бота
            foreach (var move in availableMoves)
            {
                Label botLabel = move.Controls[0] as Label;
                botLabel.Text = zero; // Попробуем сделать ход для бота
                botLabel.Dock = DockStyle.Fill;
                botLabel.Font = new Font("Roboto", 105, FontStyle.Regular);

                if (IsWinner()) // Проверяем, выиграет ли бот после этого хода
                {
                    return; // Если да, то ход сделан
                }

                botLabel.Text = ""; // Отменяем ход для следующей проверки
                botLabel.Dock = DockStyle.None; // Сбросить свойство Dock
                botLabel.Font = SystemFonts.DefaultFont; // Вернуть шрифт по умолчанию
            }

            // Ищем выигрышные ходы для игрока и блокируем их
            foreach (var move in availableMoves)
            {
                Label playerLabel = move.Controls[0] as Label;
                playerLabel.Text = mark; // Попробуем сделать ход для игрока

                if (IsWinner()) // Проверяем, выиграет ли игрок после этого хода
                {
                    Label botLabel = move.Controls[0] as Label;
                    botLabel.Text = zero; // Блокируем ход игрока
                    botLabel.Dock = DockStyle.Fill;
                    botLabel.Font = new Font("Roboto", 105, FontStyle.Regular);
                    return;
                }

                playerLabel.Text = ""; // Отменяем ход для следующей проверки
            }

            // Если не найдено выигрышных ходов и блокирующих ходов, выбираем случайный доступный ход
            if (availableMoves.Count > 0)
            {
                Random random = new Random();
                int index = random.Next(availableMoves.Count);

                PictureBox botMove = availableMoves[index];
                Label botLabel = botMove.Controls[0] as Label;
                botLabel.Text = zero; // Ход бота
                botLabel.Dock = DockStyle.Fill;
                botLabel.Font = new Font("Roboto", 105, FontStyle.Regular);
            }
        }

        private void ResetLabel(Label label)
        {
            label.Text = ""; // Сбросить текст
            label.Dock = DockStyle.None; // Сбросить свойство Dock
            label.Font = SystemFonts.DefaultFont; // Вернуть шрифт по умолчанию
        }

        private void RestartGame()
        {
            foreach (var row in pictureBoxes)
            {
                foreach (var picBox in row)
                {
                    Label label = picBox.Controls[0] as Label;
                    ResetLabel(label);
                }
            }

            currentPlayerX = true; // Сбросить состояние игрока
            move = 1; // Сбросить счетчик ходов
        }

        private void ResetGame()
        {
            foreach (var row in pictureBoxes)
            {
                foreach (var picBox in row)
                {
                    picBox.Dispose();
                }
            }
            pictureBoxes.Clear();
        }


    }
}
