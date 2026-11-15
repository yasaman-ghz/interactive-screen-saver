using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ScreenSaver
{
    internal class Monster : Shape
    {
        private int eatenCount = 0;
        private Bitmap monsterImg;
        private float angle = 0f;

        public Monster(int x, int y, int dx, int dy) : base(x, y, 500, 500, dx, dy)
        {
            stateColors = new Color[]
            {
                Color.DarkViolet,
                Color.Indigo,
                Color.Purple,
                Color.MediumOrchid
            };

            // Optional: Load your Monster PNG from Resources and strip white background
            monsterImg = new Bitmap(Properties.Resources.angry);
            monsterImg.MakeTransparent(Color.White);
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            base.Move(formBounds, cursorPos);
            angle = (angle + 2f) % 360f;
        }

        public void EatShape(Shape target)
        {
            if (IsDestroyed || target.IsDestroyed)
            {
                return;
            }

            // Destroy the collided RandomRectangle or SpinningStar
            target.IsDestroyed = true;
            eatenCount++;

            // Cycle monster's 4 state colors as it eats
            ChangeState();

            // Disappear after 15 collisions
            if (eatenCount >= 15)
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

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(x + width / 2f, y + height / 2f);
            g.RotateTransform(angle);

            int left = -width / 2;
            int top = -height / 2;

            // Draw a jagged, spiky monster aura behind the PNG
            Point[] spikes = new Point[]
            {
                new Point(left + width / 2, top),
                new Point(left + width * 3 / 4, top + 15),
                new Point(left + width, top + height / 4),
                new Point(left + width - 15, top + height / 2),
                new Point(left + width, top + height * 3 / 4),
                new Point(left + width * 3 / 4, top + height - 15),
                new Point(left + width / 2, top + height),
                new Point(left + width / 4, top + height - 15),
                new Point(left, top + height * 3 / 4),
                new Point(left + 15, top + height / 2),
                new Point(left, top + height / 4),
                new Point(left + width / 4, top + 15)
            };

            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                g.FillClosedCurve(brush, spikes, FillMode.Winding, 0.4f);
            }

            // Draw the Monster PNG centered inside
            if (monsterImg != null)
            {
                int imgSize = (int)(width * 0.75);
                g.DrawImage(monsterImg, -imgSize / 2, -imgSize / 2, imgSize, imgSize);
            }

            g.ResetTransform();
        }
    }
}
