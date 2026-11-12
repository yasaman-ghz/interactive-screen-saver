using System.Drawing;

namespace ScreenSaver
{
    internal class SpinningStar : Shape
    {
        private float angle = 0f;

        public SpinningStar(int x, int y, int dx, int dy) : base(x, y, 50, 50, dx, dy)
        {
            stateColors = new Color[]
            {
                Color.HotPink,
                Color.DeepPink,
                Color.LightPink,
                Color.PaleVioletRed
            };
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            base.Move(formBounds, cursorPos);
            angle += 6f;
        }

        public override void CheckCollision(Shape other)
        {
            int oldDx = dx;
            int oldDy = dy;

            base.CheckCollision(other);

            if (dx != oldDx || dy != oldDy)
            {
                ChangeState();
            }
        }

        public override void Draw(Graphics g)
        {
            // 10-point star centered at (0, 0)
            Point[] starPoints = new Point[]
            {
                new Point(0, -25),
                new Point(7, -8),
                new Point(24, -8),
                new Point(10, 4),
                new Point(15, 22),
                new Point(0, 11),
                new Point(-15, 22),
                new Point(-10, 4),
                new Point(-24, -8),
                new Point(-7, -8)
            };

            g.TranslateTransform(x + width / 2f, y + height / 2f);
            g.RotateTransform(angle);

            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                g.FillPolygon(brush, starPoints);
            }

            g.ResetTransform();
        }
    }
}

