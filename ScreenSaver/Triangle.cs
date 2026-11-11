using System.Drawing;

namespace ScreenSaver
{
    internal class Triangle : Shape
    {
        public Triangle() : base(400, 300, 50, 50, 4, -3)
        {
        }

        public Triangle(int x, int y, int dx, int dy) : base(x, y, 50, 50, dx, dy)
        {
        }

        public override void Draw(Graphics g)
        {
            //irregular polygon
            Point[] points = new Point[]
            {
                new Point(this.x + (this.width / 3), this.y),
                new Point(this.x + this.width, this.y + (this.height / 2)),
                new Point(this.x, this.y + this.height)
            };

            using (SolidBrush brush = new SolidBrush(CurrentColor))
            {
                g.FillPolygon(brush, points);
            }
        }
    }
}

