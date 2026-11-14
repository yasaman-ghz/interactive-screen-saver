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

        protected int dx { get; set; }
        protected int dy { get; set; }

        // Shared 4-state array across shapes
        protected Color[] stateColors = new Color[]
        {
            Color.LightSlateGray,
            Color.RosyBrown,
            Color.IndianRed,
            Color.Firebrick
        };
        protected int currentState = 0;

        public bool IsDestroyed = false;

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

        protected Bitmap LoadTransparentPng(Image resourceImage)
        {
            Bitmap bmp = new Bitmap(resourceImage);
            bmp.MakeTransparent(Color.White);
            return bmp;
        }

        public virtual void ChangeState()
        {
            currentState = (currentState + 1) % stateColors.Length;
        }

        public virtual void Draw(Graphics g)
        {
        }

        public virtual void Move(Rectangle formBounds, Point cursorPos)
        {
            if (formBounds.Width <= 0 || formBounds.Height <= 0)
            {
                return;
            }

            x += dx;
            y += dy;

            // Bounce off left and right walls
            if (x < 0)
            {
                x = 0;
                dx = -dx;
            }
            else if (x + width > formBounds.Width)
            {
                x = Math.Max(0, formBounds.Width - width);
                dx = -dx;
            }

            // Bounce off top and bottom walls
            if (y < 0)
            {
                y = 0;
                dy = -dy;
            }
            else if (y + height > formBounds.Height)
            {
                y = Math.Max(0, formBounds.Height - height);
                dy = -dy;
            }
        }

        public virtual void CheckCollision(Shape other)
        {
            if (this.IsDestroyed || other.IsDestroyed)
            {
                return;
            }

            int overlapLeft = (this.x + this.width) - other.x;
            int overlapRight = (other.x + other.width) - this.x;
            int overlapTop = (this.y + this.height) - other.y;
            int overlapBottom = (other.y + other.height) - this.y;

            if (overlapLeft > 0 && overlapRight > 0 && overlapTop > 0 && overlapBottom > 0)
            {
                int minOverlapX = Math.Min(overlapLeft, overlapRight);
                int minOverlapY = Math.Min(overlapTop, overlapBottom);

                // Push shapes apart so they don't stick together
                if (minOverlapX < minOverlapY)
                {
                    int pushX = (minOverlapX / 2) + 1;
                    if (overlapLeft < overlapRight)
                    {
                        this.x -= pushX;
                        other.x += pushX;
                    }
                    else
                    {
                        this.x += pushX;
                        other.x -= pushX;
                    }

                    int tempDx = this.dx;
                    this.dx = other.dx;
                    other.dx = tempDx;
                }
                else
                {
                    int pushY = (minOverlapY / 2) + 1;
                    if (overlapTop < overlapBottom)
                    {
                        this.y -= pushY;
                        other.y += pushY;
                    }
                    else
                    {
                        this.y += pushY;
                        other.y -= pushY;
                    }

                    int tempDy = this.dy;
                    this.dy = other.dy;
                    other.dy = tempDy;
                }

                // Trigger Stone's state change when hit by a SpinningStar
                if (this is Stone && other is SpinningStar)
                {
                    ((Stone)this).HitByStar();
                }
                else if (other is Stone && this is SpinningStar)
                {
                    ((Stone)other).HitByStar();
                }
            }
        }
    }
}
