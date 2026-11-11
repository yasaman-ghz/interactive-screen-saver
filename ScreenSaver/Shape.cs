using System;
using System.Drawing;

namespace ScreenSaver
{
    internal class Shape
    {
        protected int x { get; set; }
        protected int y { get; set; }
        protected int width { get; set; }
        protected int height { get; set; }
        protected int dx { get; set; } = 3;
        protected int dy { get; set; } = 3;
        protected Color[] stateColors = new Color[]
        {
            Color.GhostWhite,
            Color.SandyBrown,
            Color.Cyan,
            Color.HotPink
        };
        protected int currentState = 0;

        public Shape(int x, int y, int width, int height, int dx, int dy)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.dx = dx;
            this.dy = dy;
        }

        protected Color CurrentColor
        {
            get { return stateColors[currentState]; }
        }

        public virtual void ChangeState()
        {
            currentState = (currentState + 1) % stateColors.Length;
        }

        public virtual void Draw(Graphics g)
        {
        }

        public virtual void Move(Rectangle formBounds)
        {
            if (formBounds.Width <= 0 || formBounds.Height <= 0)
            {
                return;
            }

            this.x += this.dx;
            this.y += this.dy;

            bool hitWall = false;


            if (this.x < 0) //horizontal bounds
            {
                this.x = 0;
                this.dx = Math.Abs(this.dx);
                hitWall = true;
            }
            else if (this.x + this.width > formBounds.Width)
            {
                this.x = Math.Max(0, formBounds.Width - this.width);
                this.dx = -Math.Abs(this.dx);
                hitWall = true;
            }

            if (this.y < 0) //verticall bounds
            {
                this.y = 0;
                this.dy = Math.Abs(this.dy);
                hitWall = true;
            }
            else if (this.y + this.height > formBounds.Height)
            {
                this.y = Math.Max(0, formBounds.Height - this.height);
                this.dy = -Math.Abs(this.dy);
                hitWall = true;
            }

            if (hitWall)
            {
                ChangeState();
            }
        }

        public void CheckCollision(Shape other)
        {
            bool isOverlapping =
                this.x < other.x + other.width &&
                this.x + this.width > other.x &&
                this.y < other.y + other.height &&
                this.y + this.height > other.y;

            if (isOverlapping)
            {
                int tempDx = this.dx;
                int tempDy = this.dy;
                this.dx = other.dx;
                this.dy = other.dy;
                other.dx = tempDx;
                other.dy = tempDy;


                this.x += this.dx * 3;
                this.y += this.dy * 3;
                other.x += other.dx * 3;
                other.y += other.dy * 3;


                this.ChangeState();
                other.ChangeState();
            }
        }
    }
}

