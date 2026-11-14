using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ScreenSaver
{
    internal class Joker : Shape
    {
        private class SmoothButton : Button
        {
            public SmoothButton()
            {
                this.DoubleBuffered = true;
            }
        }

        private SmoothButton jokerButton;
        private Form1 mainForm;
        private Bitmap jokerImg;
        private float angle = 0f;
        private int paintCounter = 0;
        private static Random rng = new Random();

        public Joker(Form1 form, int x, int y, int dx, int dy)
            : base(x, y, 110, 110, dx, dy)
        {
            mainForm = form;

            // Pick one of 3 sizes (110, 145, or 180)
            int[] sizes = new int[] { 110, 145, 180 };
            int chosenSize = sizes[rng.Next(sizes.Length)];
            this.width = chosenSize;
            this.height = chosenSize;

            stateColors = new Color[]
            {
                Color.MediumPurple,
                Color.Crimson,
                Color.Teal,
                Color.Gold
            };

            // Pick one of 3 Joker PNGs (0, 1, or 2)
            int pngChoice = rng.Next(3);
            Image chosenResource = Properties.Resources.small_monster1;
            if (pngChoice == 1) chosenResource = Properties.Resources.small_monster2;
            if (pngChoice == 2) chosenResource = Properties.Resources.small_monster3;

            jokerImg = new Bitmap(chosenResource);
            jokerImg.MakeTransparent(Color.White);

            jokerButton = new SmoothButton();
            jokerButton.Location = new Point(x, y);
            jokerButton.Size = new Size(width, height);
            jokerButton.FlatStyle = FlatStyle.Flat;
            jokerButton.FlatAppearance.BorderSize = 0;
            jokerButton.BackColor = CurrentColor;

            // Shape the button into a full circle
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, width, height);
            jokerButton.Region = new Region(path);

            jokerButton.Paint += JokerButton_Paint;
            jokerButton.Click += JokerButton_Click;

            mainForm.Controls.Add(jokerButton);
        }

        private void JokerButton_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.TranslateTransform(width / 2f, height / 2f);
            e.Graphics.RotateTransform(angle);

            int imgSize = (int)(width * 0.85);
            e.Graphics.DrawImage(jokerImg, -imgSize / 2, -imgSize / 2, imgSize, imgSize);

            e.Graphics.ResetTransform();
        }

        private void JokerButton_Click(object sender, EventArgs e)
        {
            // Wipe this Joker out and spawn a new one in a different spot
            mainForm.ReplaceJoker(this);
        }

        public void RemoveFromForm()
        {
            if (jokerButton != null)
            {
                mainForm.Controls.Remove(jokerButton);
                jokerButton.Dispose();
            }
        }

        public override void Move(Rectangle formBounds, Point cursorPos)
        {
            base.Move(formBounds, cursorPos);
            jokerButton.Location = new Point(x, y);

            paintCounter++;
            if (paintCounter >= 3)
            {
                paintCounter = 0;
                angle = (angle + 5f) % 360f;
                jokerButton.Invalidate();
            }
        }

        public override void ChangeState()
        {
            base.ChangeState();
            if (jokerButton != null)
            {
                jokerButton.BackColor = CurrentColor;
            }
        }

    }
}
