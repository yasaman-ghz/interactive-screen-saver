using System.Drawing;

namespace ScreenSaver
{
    internal class Block : Shape
    {
        public Block() : base(250, 200, 80, 40, 3, 3)
        {
        }

        public Block(int x, int y, int dx, int dy) : base(x, y, 80, 40, dx, dy)
        {
        }

        public override void Draw(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                g.FillRectangle(brush, this.x, this.y, this.width, this.height);
            }
        }
    }
}
