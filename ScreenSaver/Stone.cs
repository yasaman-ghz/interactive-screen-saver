using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    internal class Stone : Shape
    {
        private int hitCooldown = 0;
        private Bitmap[] faceImages;
        private float angle = 0f;

        public Stone(int x, int y, int dx, int dy) : base(x, y, 170, 170, dx, dy)
        {
            stateColors = new Color[]
            {
                Color.LightSlateGray, // State 0: Sleeping
                Color.RosyBrown,      // State 1: Bothered
                Color.IndianRed,      // State 2: Triggered
                Color.Firebrick       // State 3: Angry
            };

            Bitmap restingBmp = new Bitmap(Properties.Resources.resting);
            restingBmp.MakeTransparent(Color.White);

            Bitmap botheredBmp = new Bitmap(Properties.Resources.bothered);
            botheredBmp.MakeTransparent(Color.White);

            Bitmap triggeredBmp = new Bitmap(Properties.Resources.triggered);
            triggeredBmp.MakeTransparent(Color.White);

            Bitmap angryBmp = new Bitmap(Properties.Resources.angry);
            angryBmp.MakeTransparent(Color.White);

            faceImages = new Bitmap[]
            {
                restingBmp,
                botheredBmp,
                triggeredBmp,
                angryBmp
            };
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            if (hitCooldown > 0)
            {
                hitCooldown--;
            }

            base.Move(formBounds, cursorPos);
            angle = (angle + 2f) % 360f;
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

            // Shift origin to the stone's center and rotate
            g.TranslateTransform(x + width / 2f, y + height / 2f);
            g.RotateTransform(angle);

            // Local top-left corner relative to the center (0, 0)
            int left = -width / 2;
            int top = -height / 2;

            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                if (currentState == 0)
                {
                    // State 0: Smooth, almost round stone
                    Point[] points = new Point[]
                    {
                        new Point(left + width / 4, top),
                        new Point(left + width * 3 / 4, top),
                        new Point(left + width, top + height / 2),
                        new Point(left + width * 3 / 4, top + height),
                        new Point(left + width / 4, top + height),
                        new Point(left, top + height / 2)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.4f);
                }
                else if (currentState == 1)
                {
                    // State 1: Slightly rougher surface
                    Point[] points = new Point[]
                    {
                        new Point(left + width / 4, top),
                        new Point(left + width * 3 / 4, top + 6),
                        new Point(left + width, top + height / 2),
                        new Point(left + width * 3 / 4, top + height),
                        new Point(left + width / 5, top + height - 4),
                        new Point(left, top + height / 3)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.4f);
                }
                else if (currentState == 2)
                {
                    // State 2: Rougher polygon with prominent edges
                    Point[] points = new Point[]
                    {
                        new Point(left + width / 3, top),
                        new Point(left + width / 2, top + 15),
                        new Point(left + width * 4 / 5, top),
                        new Point(left + width, top + height / 2),
                        new Point(left + width * 3 / 4, top + height),
                        new Point(left + width / 4, top + height - 10),
                        new Point(left, top + height / 2)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.4f);
                }
                else
                {
                    // State 3: Biggest and spikiest stone
                    Point[] points = new Point[]
                    {
                        new Point(left + width / 4, top),
                        new Point(left + width / 2, top + 20),
                        new Point(left + width * 3 / 4, top),
                        new Point(left + width, top + height / 3),
                        new Point(left + width - 18, top + height / 2),
                        new Point(left + width, top + height * 3 / 4),
                        new Point(left + width / 2, top + height),
                        new Point(left + width / 4, top + height - 15),
                        new Point(left, top + height * 2 / 3),
                        new Point(left + 15, top + height / 3)
                    };
                    g.FillClosedCurve(brush, points, FillMode.Winding, 0.4f);
                }
            }

            // Draw the PNG face centered around (0, 0) so it spins with the stone
            int faceW = (int)(width / 1.5);
            int faceH = (int)(height / 1.5);
            g.DrawImage(faceImages[currentState], -faceW / 2, -faceH / 2, faceW, faceH);

            // Reset the transform so other shapes draw normally
            g.ResetTransform();
        }
    }
}
