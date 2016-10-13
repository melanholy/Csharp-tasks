using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public sealed partial class MenuForm : Form
    {
        public int ReturnValue { get; set; }
        PrivateFontCollection font;

        public MenuForm()
        {
            font = new PrivateFontCollection();
            font.AddFontFile("Shoguns Clan.ttf");
            font.AddFontFile("Friday13.ttf");
            ClientSize = new Size(300, 225);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text = @"Inside Job";
            BackColor = Color.Black;
            var start = new Button
            {
                Location = new Point(ClientSize.Width/2 - 125, 80),
                Size = new Size(250, 50),
                Font = new Font(font.Families[1], 32),
                Text = @"Start game",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkRed,
                BackColor = Color.GreenYellow
            };
            start.Click += (x, y) => { ReturnValue = 1; Close(); };
            var exit = new Button
            {
                Location = new Point(ClientSize.Width/2 - 125, 150),
                Size = new Size(250, 50),
                Font = new Font(font.Families[1], 32),
                Text = @"Exit",
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkRed,
                BackColor = Color.Orange
            };
            exit.Click += (x, y) => { ReturnValue = 2; Close(); };
            Controls.Add(start);
            Controls.Add(exit);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("Inside Job", new Font(font.Families[0], 45), Brushes.BlueViolet, new Point(-5, 5)); ;
        }
    }
}
