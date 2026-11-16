using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace ScreenSaver
{
    internal class Monster : Shape
    {
        private int eatenCount = 0;
        private Bitmap monsterImg;
        private float angle = 0f;

        private Bitmap[] stateFaces;

        public Monster(int x, int y, int dx, int dy) : base(x, y, 300, 300, dx, dy)
        {
            stateFaces = new Bitmap[]
            {
                Properties.Resources.monster1,
                Properties.Resources.monster2
            };
        }

        public override void ChangeState()
        {
            currentState = (currentState + 1) % stateFaces.Length;
            if (currentState < stateFaces.Length)
            {
                monsterImg = stateFaces[currentState];
                monsterImg.MakeTransparent(Color.White);
            }
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

            // Disappear after 30 collisions
            if (eatenCount >= 30)
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
                new Point(left + width / 2, top),                   // Top outer bump
                new Point(left + width * 3 / 4, top + 12),          // Top-right gentle dip
                new Point(left + width - 6, top + height / 4),      // Upper-right bump
                new Point(left + width + 10, top + height / 2),     // Middle-right gentle dip
                new Point(left + width - 6, top + height * 3 / 4),  // Lower-right bump
                new Point(left + width * 3 / 4, top + height - 12), // Bottom-right gentle dip
                new Point(left + width / 2, top + height),          // Bottom outer bump
                new Point(left + width / 4, top + height - 12),     // Bottom-left gentle dip
                new Point(left + 6, top + height * 3 / 4),          // Lower-left bump
                new Point(left - 10, top + height / 2),             // Middle-left gentle dip
                new Point(left + 6, top + height / 4),              // Upper-left bump
                new Point(left + width / 4, top + 12)               // Top-left gentle dip
            };

            using (SolidBrush brush = new SolidBrush(Color.LemonChiffon))
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
