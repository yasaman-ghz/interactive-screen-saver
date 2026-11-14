using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ScreenSaver
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private ArrayList shapes = new ArrayList();
        private Random rng = new Random();
        private int clickCount = 0;
        private Bitmap bgImage;
        private int bgX = 0;
        private int bgY = 0;
        private int bgSpeedX = -1; // Negative moves background left (shapes appear to move right)
        private int bgSpeedY = -1; // Negative moves background up (shapes appear to move down)
        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += ScreenTimer_Tick;
            timer.Start();

            this.Click += Form1_Click;
            this.Resize += Form1_Resize;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Set the fixed screen-sized background image from Resources
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            bgImage = new Bitmap(Properties.Resources.background, screenBounds.Width, screenBounds.Height);


            // Start with 1 Stone, 1 GummyBear, 1 RandomRectangle, and 1 SpinningStar
            shapes.Add((Shape)new Stone(220, 180, 2, 2));
            shapes.Add((Shape)new RandomRectangle(450, 250, rng));
            shapes.Add((Shape)new SpinningStar(600, 150, -4, 3));
            ReplaceJoker(null);
        }

        internal void ReplaceJoker(Joker oldJoker)
        {
            // 1. Wipe out the existing Joker if one was clicked
            if (oldJoker != null)
            {
                oldJoker.RemoveFromForm();
                shapes.Remove(oldJoker);
            }

            // 2. Spawn a new Joker in a different random spot
            int x = rng.Next(50, Math.Max(100, this.ClientSize.Width - 180));
            int y = rng.Next(50, Math.Max(100, this.ClientSize.Height - 180));
            int dx = rng.Next(1, 3) * (rng.Next(2) == 0 ? 1 : -1);
            int dy = rng.Next(1, 3) * (rng.Next(2) == 0 ? 1 : -1);

            shapes.Add((Shape)new Joker(this, x, y, dx, dy));
        }

        private void CheckAndSpawnMonster()
        {
            int clutterCount = 0;
            bool monsterExists = false;

            foreach (object obj in shapes)
            {
                if (obj is RandomRectangle || obj is SpinningStar)
                {
                    clutterCount++;
                }
                else if (obj is Monster && !((Monster)obj).IsDestroyed)
                {
                    monsterExists = true;
                }
            }

            // Spawn 1 Monster when there are 40+ RandomRectangles + SpinningStars on screen
            if (clutterCount >= 40 && !monsterExists)
            {
                int x = rng.Next(50, Math.Max(100, this.ClientSize.Width - 150));
                int y = rng.Next(50, Math.Max(100, this.ClientSize.Height - 150));
                int dx = rng.Next(3, 6) * (rng.Next(2) == 0 ? 1 : -1);
                int dy = rng.Next(3, 6) * (rng.Next(2) == 0 ? 1 : -1);

                shapes.Add((Shape)new Monster(x, y, dx, dy));
            }
        }

        //private bool IsStoneOnScreen()
        //{
        //    foreach (object obj in shapes)
        //    {
        //        if (obj is Stone)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // 2. Draw the scrolling background wrapped seamlessly across 4 tiles
            if (bgImage != null)
            {
                int w = bgImage.Width;
                int h = bgImage.Height;

                e.Graphics.DrawImage(bgImage, bgX, bgY, w, h);
                e.Graphics.DrawImage(bgImage, bgX + w, bgY, w, h);
                e.Graphics.DrawImage(bgImage, bgX, bgY + h, w, h);
                e.Graphics.DrawImage(bgImage, bgX + w, bgY + h, w, h);
            }

            // 3. Draw all shapes on top of the moving background
            foreach (object obj in shapes)
            {
                Shape s = (Shape)obj;
                s.Draw(e.Graphics);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            clickCount++;

            MouseEventArgs me = (MouseEventArgs)e;
            int spawnX = me.X;
            int spawnY = me.Y;

                shapes.Add((Shape)new RandomRectangle(spawnX, spawnY, rng));
           
            if (clickCount % 4 == 0 && clickCount % 6 != 0)
            {
                shapes.Add((Shape)new Stone(spawnX, spawnY, 1, 1));
            }

            // Chance of spawning a SpinningStar every 6 clicks
            if (clickCount % 6 == 0 && rng.Next(2) == 0)
            {
                shapes.Add((Shape)new SpinningStar(spawnX, spawnY, 1, 1));
            }

            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Point cursorPos = this.PointToClient(Cursor.Position);
            foreach (object obj in shapes)
            {
                Shape s = (Shape)obj;
                s.Move(this.ClientRectangle, cursorPos);
            }
            Invalidate();
        }

        private void ScreenTimer_Tick(object sender, EventArgs e)
        {
            Point cursorPos = this.PointToClient(Cursor.Position);

            // 1. Move all shapes
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape s = (Shape)shapes[i];
                s.Move(this.ClientRectangle, cursorPos);
            }

            // 2. Check collisions
            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    Shape shapeA = (Shape)shapes[i];
                    Shape shapeB = (Shape)shapes[j];
                    shapeA.CheckCollision(shapeB);
                }
            }

            CheckAndSpawnMonster();

            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                Shape s = (Shape)shapes[i];
                if (s.IsDestroyed)
                {
                    shapes.RemoveAt(i);
                }
            }

            if (bgImage != null)
            {
                bgX = (bgX + bgSpeedX) % bgImage.Width;
                bgY = (bgY + bgSpeedY) % bgImage.Height;
            }

            Invalidate();
        }
    }
}
