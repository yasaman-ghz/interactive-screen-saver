using System.Drawing;

namespace ScreenSaver
{
    internal class Bubble : Shape
    {
        public Bubble() : base(550, 500, 35, 35, -3, -3)
        {
        }

        public Bubble(int x, int y, int dx, int dy) : base(x, y, 35, 35, dx, dy)
        {
        }

        public override void Draw(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                g.FillEllipse(brush, this.x, this.y, this.width, this.height);
            }
        }
    }
}
