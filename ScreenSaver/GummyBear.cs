using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal class GummyBear : Shape
    {
        private Button bearButton;
        private Form1 mainForm;

        public GummyBear(Form1 form, int x, int y, int dx, int dy)
            : base(x, y, 56, 54, dx, dy)
        {
            mainForm = form;

            stateColors = new Color[]
            {
                Color.SandyBrown,
                Color.LightCoral,
                Color.LightGreen,
                Color.Gold
            };

            bearButton = new Button();
            bearButton.Location = new Point(x, y);
            bearButton.Size = new Size(width, height);
            bearButton.FlatStyle = FlatStyle.Flat;
            bearButton.FlatAppearance.BorderSize = 0;
            bearButton.BackColor = CurrentColor;

            // Shape the button like a teddy bear head
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(2, 0, 18, 18);   // Left ear
            path.AddEllipse(36, 0, 18, 18);  // Right ear
            path.AddEllipse(6, 8, 44, 44);   // Head
            bearButton.Region = new Region(path);

            // Subscribe to the button's Paint and Click events
            bearButton.Paint += BearButton_Paint;
            bearButton.Click += BearButton_Click;

            mainForm.Controls.Add(bearButton);
        }

        private void BearButton_Paint(object sender, PaintEventArgs e)
        {
            // Draw your single combined PNG face centered on the bear's head
            e.Graphics.DrawImage(Properties.Resources.gummybear, 12, 16, 32, 28);
        }

        private void BearButton_Click(object sender, EventArgs e)
        {
            // Spawn a new GummyBear when this button is clicked
            mainForm.AddNewGummyBear();
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            base.Move(formBounds, cursorPos);
            bearButton.Location = new Point(x, y);
        }

        public override void CheckCollision(Shape other)
        {
            int oldDx = dx;
            int oldDy = dy;

            base.CheckCollision(other);

            if (dx != oldDx || dy != oldDy)
            {
                ChangeState();
                bearButton.BackColor = CurrentColor;
            }

            bearButton.Location = new Point(x, y);
        }
    }
}
