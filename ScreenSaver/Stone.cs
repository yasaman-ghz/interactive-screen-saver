using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    internal class Stone : Shape
    {
        private int hitCooldown = 0;

        public Stone(int x, int y, int dx, int dy) : base(x, y, 100, 100, dx, dy)
        {
            stateColors = new Color[]
            {
                Color.LightSlateGray, // State 0: Sleeping
                Color.RosyBrown,      // State 1: Bothered
                Color.IndianRed,      // State 2: Triggered
                Color.Firebrick       // State 3: Angry
            };
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            if (hitCooldown > 0)
            {
                hitCooldown--;
            }

            base.Move(formBounds, cursorPos);
        }

        public void HitByStar()
        {
            if (hitCooldown > 0 || IsDestroyed)
            {
                return;
            }

            hitCooldown = 15;

            if (currentState < 3)
            {
                currentState++;
                width += 20;
                height += 20;
            }
            else
            {
                IsDestroyed = true;
            }
        }

        public override void Draw(Graphics g)
        {
            if (IsDestroyed)
            {
                return;
            }

            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                if (currentState == 0)
                {
                    // State 0: Smooth, almost round stone
                    Point[] points = new Point[]
                    {
                        new Point(x + width / 4, y),
                        new Point(x + width * 3 / 4, y),
                        new Point(x + width, y + height / 2),
                        new Point(x + width * 3 / 4, y + height),
                        new Point(x + width / 4, y + height),
                        new Point(x, y + height / 2)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.5f);
                }
                else if (currentState == 1)
                {
                    // State 1: Slightly rougher surface
                    Point[] points = new Point[]
                    {
                        new Point(x + width / 4, y),
                        new Point(x + width * 3 / 4, y + 10),
                        new Point(x + width, y + height / 2),
                        new Point(x + width * 3 / 4, y + height),
                        new Point(x + width / 5, y + height - 8),
                        new Point(x, y + height / 3)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.2f);
                }
                else if (currentState == 2)
                {
                    // State 2: Rougher polygon with prominent edges
                    Point[] points = new Point[]
                    {
                        new Point(x + width / 3, y),
                        new Point(x + width / 2, y + 15),
                        new Point(x + width * 4 / 5, y),
                        new Point(x + width, y + height / 2),
                        new Point(x + width * 3 / 4, y + height),
                        new Point(x + width / 4, y + height - 10),
                        new Point(x, y + height / 2)
                    };
                    g.FillPolygon(brush, points);
                }
                else
                {
                    // State 3: Biggest and spikiest stone
                    Point[] points = new Point[]
                    {
                        new Point(x + width / 4, y),
                        new Point(x + width / 2, y + 20),
                        new Point(x + width * 3 / 4, y),
                        new Point(x + width, y + height / 3),
                        new Point(x + width - 18, y + height / 2),
                        new Point(x + width, y + height * 3 / 4),
                        new Point(x + width / 2, y + height),
                        new Point(x + width / 4, y + height - 15),
                        new Point(x, y + height * 2 / 3),
                        new Point(x + 15, y + height / 3)
                    };
                    g.FillPolygon(brush, points);
                }
            }

            // Draw the PNG face centered inside the stone
            Image faceImg = Properties.Resources.resting;
            if (currentState == 1) faceImg = Properties.Resources.bothered;
            if (currentState == 2) faceImg = Properties.Resources.triggered;
            if (currentState == 3) faceImg = Properties.Resources.angry;

            int faceW = width / 2;
            int faceH = height / 2;
            g.DrawImage(faceImg, x + (width - faceW) / 2, y + (height - faceH) / 2, faceW, faceH);
        }
    }
}
