using System;
using System.Drawing;
using System.Windows.Forms;

namespace TreasureHuntApp
{
    public partial class Form1 : Form
    {
        private Player player;
        private Treasure treasure;
        private Obstacle[] obstacles;
        private System.Windows.Forms.Timer gameTimer;
        private int score;
        private int health;
        private int timeLeft;
        private bool isGameOver;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
        }


        private void InitializeGame()
        {
            random = new Random();
            player = new Player(0, 0);
            treasure = new Treasure(random);
            obstacles = new Obstacle[]
            {
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random),
                new Obstacle(random)
            };

            score = 0;
            health = 100;
            timeLeft = 30;
            isGameOver = false;

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            this.KeyDown += Form1_KeyDown;
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
                timeLeft--;
            else
                EndGame();

            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (isGameOver)
                return;

            switch (e.KeyCode)
            {
                case Keys.Up:
                    player.Move(0, -1);
                    break;
                case Keys.Down:
                    player.Move(0, 1);
                    break;
                case Keys.Left:
                    player.Move(-1, 0);
                    break;
                case Keys.Right:
                    player.Move(1, 0);
                    break;
                case Keys.W:
                    player.Move(0, -1);
                    break;
                case Keys.S:
                    player.Move(0, 1);
                    break;
                case Keys.A:
                    player.Move(-1, 0);
                    break;
                case Keys.D:
                    player.Move(1, 0);
                    break;
            }

            if (player.X == treasure.X && player.Y == treasure.Y)
            {
                score += 10;
                treasure.Randomize(random);
            }

            if (Obstacle.ExistsAt(player.X, player.Y, obstacles))
            {
                health -= 20;
                if (health <= 0)
                    EndGame();
            }

            Invalidate();
        }

        private void EndGame()
        {
            isGameOver = true;
            gameTimer.Stop();

            DialogResult result = MessageBox.Show("Game Over! Final Score: " + score + "\nDo you want to exit?", "Game Over", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                InitializeGame();
                player.Reset();
                Invalidate();
            }
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            DrawGameArea(g);
        }

        private void DrawGameArea(Graphics g)
        {
            g.FillRectangle(Brushes.Blue, player.X * 40, player.Y * 40, 40, 40);

            g.FillRectangle(Brushes.Gold, treasure.X * 40, treasure.Y * 40, 40, 40);

            foreach (var obstacle in obstacles)
            {
                g.FillRectangle(Brushes.Red, obstacle.X * 40, obstacle.Y * 40, 40, 40);
            }

            g.DrawString($"Score: {score}", this.Font, Brushes.Black, 10, 10);
            g.DrawString($"Health: {health}", this.Font, Brushes.Black, 10, 30);
            g.DrawString($"Time Left: {timeLeft}s", this.Font, Brushes.Black, 10, 50);
        }
    }

    public class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }

        public Player(int x, int y)
        {
            X = x;
            Y = y;
            Speed = 1;
        }

        public void Move(int dx, int dy)
        {
            X += dx * Speed;
            Y += dy * Speed;

            if (X < 0) X = 0;
            if (Y < 0) Y = 0;
            if (X > 9) X = 9;
            if (Y > 9) Y = 9;
        }

        public void Reset()
        {
            Speed = 1;
        }
    }


    public class Treasure
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Treasure(Random random)
        {
            Randomize(random);
        }

        public void Randomize(Random random)
        {
            X = random.Next(0, 10);
            Y = random.Next(0, 10);
        }
    }

    public class Obstacle
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Obstacle(Random random)
        {
            Randomize(random);
        }

        public void Randomize(Random random)
        {
            X = random.Next(0, 10);
            Y = random.Next(0, 10);
        }

        public static bool ExistsAt(int x, int y, Obstacle[] obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle.X == x && obstacle.Y == y)
                    return true;
            }
            return false;
        }
    }
}