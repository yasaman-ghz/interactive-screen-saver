using System;
using System.Drawing;

namespace ScreenSaver
{
    internal class RandomRectangle : Shape
    {
        private bool isSpinning;
        private bool runsFromCursor;
        private float angle = 0f;
        private Color rectColor;

        public RandomRectangle(int x, int y, Random rng)
            : base(x, y, rng.Next(40, 141), rng.Next(40, 141),
                   rng.Next(2, 6) * (rng.Next(2) == 0 ? 1 : -1),
                   rng.Next(2, 6) * (rng.Next(2) == 0 ? 1 : -1))
        {
            rectColor = Color.FromArgb(rng.Next(80, 240), rng.Next(80, 240), rng.Next(80, 240));

            int mode = rng.Next(3);
            isSpinning = (mode == 1);
            runsFromCursor = (mode == 2);
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            if (runsFromCursor)
            {
                int centerX = x + width / 2;
                int centerY = y + height / 2;
                int diffX = centerX - cursorPos.X;
                int diffY = centerY - cursorPos.Y;

                // Run away if the cursor gets within 120 pixels
                if (Math.Abs(diffX) < 120 && Math.Abs(diffY) < 120)
                {
                    dx = diffX >= 0 ? 6 : -6;
                    dy = diffY >= 0 ? 6 : -6;
                }
            }

            base.Move(formBounds, cursorPos);

            if (isSpinning)
            {
                angle += 5f;
            }
        }

        public override void Draw(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(rectColor))
            {
                if (isSpinning)
                {
                    g.TranslateTransform(x + width / 2f, y + height / 2f);
                    g.RotateTransform(angle);
                    g.FillRectangle(brush, -width / 2, -height / 2, width, height);
                    g.ResetTransform();
                }
                else
                {
                    g.FillRectangle(brush, x, y, width, height);
                }
            }
        }
    }
}
