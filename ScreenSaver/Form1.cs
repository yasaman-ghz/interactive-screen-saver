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

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            timer = new Timer
            {
                Interval = 10 
            };
            timer.Tick += ScreenTimer_Tick;
            timer.Start();

            this.Click += new EventHandler(Form1_Click);
            //this.Resize += new EventHandler(Form1_Resize);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            shapes.Add((Shape)new Bubble());
            shapes.Add((Shape)new Block());
            shapes.Add((Shape)new Triangle());
            shapes.Add((Shape)new LogoBox(this, 150, 150, 3, -4));
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
            MouseEventArgs me = e as MouseEventArgs;
            int spawnX = me != null ? me.X : rng.Next(50, Math.Max(100, this.ClientSize.Width - 100));
            int spawnY = me != null ? me.Y : rng.Next(50, Math.Max(100, this.ClientSize.Height - 100));

            int dx = rng.Next(2, 6) * (rng.Next(2) == 0 ? 1 : -1);
            int dy = rng.Next(2, 6) * (rng.Next(2) == 0 ? 1 : -1);

            int choice = rng.Next(4);
            Shape newShape;

            switch (choice)
            {
                case 0:
                    newShape = (Shape)new Bubble(spawnX, spawnY, dx, dy);
                    break;
                case 1:
                    newShape = (Shape)new Block(spawnX, spawnY, dx, dy);
                    break;
                case 2:
                    newShape = (Shape)new Triangle(spawnX, spawnY, dx, dy);
                    break;
                default:
                    newShape = (Shape)new LogoBox(this, spawnX, spawnY, dx, dy);
                    break;
            }

            shapes.Add(newShape);
        }

        //private void Form1_Resize(object sender, EventArgs e)
        //{
        //  
        //    foreach (object obj in shapes)
        //    {
        //        Shape s = (Shape)obj;
        //        s.Move(this.ClientRectangle);
        //    }
        //    Invalidate();
        //}

        private void ScreenTimer_Tick(object sender, EventArgs e)
        {
       
            for (int i = 0; i < shapes.Count; i++)
            {
                Shape s = (Shape)shapes[i];
                s.Move(this.ClientRectangle);
            }

            for (int i = 0; i < shapes.Count; i++)
            {
                for (int j = i + 1; j < shapes.Count; j++)
                {
                    Shape shapeA = (Shape)shapes[i];
                    Shape shapeB = (Shape)shapes[j];
                    shapeA.CheckCollision(shapeB);
                }
            }

            Invalidate();
        }
    }
}

