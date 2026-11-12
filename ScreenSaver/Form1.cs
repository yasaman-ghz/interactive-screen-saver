using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace ScreenSaver
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private ArrayList shapes = new ArrayList();
        private Random rng = new Random();
        private int clickCount = 0;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            timer = new Timer();
            timer.Interval = 30;
            timer.Tick += ScreenTimer_Tick;
            timer.Start();

            this.Click += Form1_Click;
            this.Resize += Form1_Resize;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 1. Set the fixed screen-sized background image from Resources
            Rectangle screenBounds = Screen.PrimaryScreen.Bounds;
            this.BackgroundImage = new Bitmap(Properties.Resources.background, screenBounds.Width, screenBounds.Height);
            this.BackgroundImageLayout = ImageLayout.None;

            // Start with 1 Stone, 1 GummyBear, 1 RandomRectangle, and 1 SpinningStar
            shapes.Add((Shape)new Stone(220, 180, 2, 2));
            shapes.Add((Shape)new RandomRectangle(450, 250, rng));
            shapes.Add((Shape)new SpinningStar(600, 150, -4, 3));
            AddNewGummyBear();
        }

        public void AddNewGummyBear()
        {
            int x = rng.Next(50, Math.Max(100, this.ClientSize.Width - 100));
            int y = rng.Next(50, Math.Max(100, this.ClientSize.Height - 100));
            int dx = rng.Next(2, 5) * (rng.Next(2) == 0 ? 1 : -1);
            int dy = rng.Next(2, 5) * (rng.Next(2) == 0 ? 1 : -1);

            shapes.Add((Shape)new GummyBear(this, x, y, dx, dy));
        }

        private bool IsStoneOnScreen()
        {
            foreach (object obj in shapes)
            {
                if (obj is Stone)
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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

            // Spawn a Stone if none exists on screen
            if (!IsStoneOnScreen())
            {
                shapes.Add((Shape)new Stone(spawnX, spawnY, 2, 2));
            }
            else
            {
                shapes.Add((Shape)new RandomRectangle(spawnX, spawnY, rng));
            }

            // Chance of spawning a SpinningStar every 4 clicks
            if (clickCount % 4 == 0 && rng.Next(2) == 0)
            {
                shapes.Add((Shape)new SpinningStar(spawnX, spawnY, -4, 3));
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

            // 3. Remove destroyed stones
            for (int i = shapes.Count - 1; i >= 0; i--)
            {
                Shape s = (Shape)shapes[i];
                if (s.IsDestroyed)
                {
                    shapes.RemoveAt(i);
                }
            }

            Invalidate();
        }
    }
}
