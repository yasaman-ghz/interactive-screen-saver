using System.Drawing;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal class LogoBox : Shape
    {
        private PictureBox pictureBox;

        public LogoBox(Form parentForm, int x, int y, int dx, int dy) : base(x, y, 55, 55, dx, dy)
        {
            pictureBox = new PictureBox
            {
                Location = new Point(this.x, this.y),
                Size = new Size(this.width, this.height),
                BackColor = CurrentColor,
                BorderStyle = BorderStyle.FixedSingle
            };

            parentForm.Controls.Add(pictureBox);
        }

        public override void ChangeState()
        {
            base.ChangeState();
            if (pictureBox != null)
            {
                pictureBox.BackColor = CurrentColor;
            }
        }

        public override void Move(Rectangle formBounds)
        {
            base.Move(formBounds);
            if (pictureBox != null)
            {
                pictureBox.Location = new Point(this.x, this.y);
            }
        }

        public override void Draw(Graphics g)
        {
            // renders itself on the form.
        }
    }
}
