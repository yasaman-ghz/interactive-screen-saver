using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal class Smartie : Shape
    {
        private float angle = 0f;
        private float spinSpeed;
        private Color shellColor;

        public Smartie(int x, int y, Random rng)
            : base(x, y, rng.Next(30, 50), rng.Next(30, 50),
                   rng.Next(1, 3) * (rng.Next(2) == 0 ? 1 : -1),
                   rng.Next(1, 3) * (rng.Next(2) == 0 ? 1 : -1))
        {
            Color[] smartieColors = new Color[]
            {
                Color.Crimson,
                Color.DodgerBlue,
                Color.Gold,
                Color.LimeGreen,
                Color.DarkOrange,
                Color.MediumOrchid,
                Color.HotPink
            };
            shellColor = smartieColors[rng.Next(smartieColors.Length)];

            // Every Smartie spins gently clockwise or counter-clockwise
            spinSpeed = rng.Next(2, 5) * (rng.Next(2) == 0 ? 1 : -1);
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            base.Move(formBounds, cursorPos);
            angle = (angle + spinSpeed) % 360f;
        }

        public override void Draw(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Move origin to center of Smartie and rotate
            g.TranslateTransform(x + width / 2f, y + height / 2f);
            g.RotateTransform(angle);

            int left = -width / 2;
            int top = -height / 2;

            // 1. Soft 3D drop shadow underneath
            using (SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(75, Color.Black)))
            {
                g.FillEllipse(shadowBrush, left + 5, top + 7, width, height);
            }

            // 2. 3D rounded candy shell using PathGradientBrush
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(left, top, width, height);

                using (PathGradientBrush pgb = new PathGradientBrush(path))
                {
                    pgb.CenterPoint = new PointF(left + width * 0.35f, top + height * 0.35f);
                    pgb.CenterColor = ControlPaint.Light(shellColor, 0.45f);
                    pgb.SurroundColors = new Color[] { ControlPaint.Dark(shellColor, 0.25f) };

                    g.FillPath(pgb, path);
                }
            }

            // 3. Glossy candy glaze shine spot near the top-left
            using (SolidBrush shineBrush = new SolidBrush(Color.FromArgb(145, Color.White)))
            {
                g.FillEllipse(shineBrush, left + (int)(width * 0.20), top + (int)(height * 0.16), width / 4, height / 5);
            }

            g.ResetTransform();
        }
    }
}

