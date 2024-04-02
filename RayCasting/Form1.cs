using TestApp.src;

namespace TestApp
{
    public partial class Form1 : Form
    {
        Graphics g;
        Graphics pG;

        Map map;
        Player p1;
        double[] test = new double[720];
        int[] col = new int[720];

        public Form1()
        {
            InitializeComponent();

            p1 = new Player(2.4, 6.5, 0.5, 1);
            map = new Map();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
            pG = panel1.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void Form1_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                p1.Move();
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                // Rotate charakter left
                p1.RotateLeft();
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                // Rotate charakter right
                p1.RotateRight();
            }

            UpdateView();
        }

        private void UpdateView()
        {
            (test, col) = p1.CalculateRays(ref map, 60);

            ClearView();
            DrawMap();
            DrawView();
        }

        private void ClearView()
        {
            g.Clear(Color.White);
            pG.Clear(Color.White);
        }

        private void DrawMap()
        {

            Pen pen = new Pen(Color.FromArgb(0, 0, 255));
            int blockSize = 10;

            // draw mini map
            for (int i = 0; i < map.GameField.GetLength(0); i++)
            {
                for (int j = 0; j < map.GameField.GetLength(1); j++)
                {
                    if (map.GameField[i, j])
                    {
                        pG.DrawRectangle(pen, blockSize * i, blockSize * j, blockSize, blockSize);
                    }
                }
            }

            int playerPosX = (int)(p1.X * blockSize);
            int playerPosY = (int)(p1.Y * blockSize);

            // draw player
            pG.DrawEllipse(pen, playerPosX - 2, playerPosY - 2, 4, 4);

            // draw 3 "look" rays
            pG.DrawLine(pen, playerPosX, playerPosY, (int)(playerPosX + p1.DirX * 20), (int)(playerPosY + p1.DirY * 20));
        }

        private void DrawView()
        {
            Pen penY = new Pen(Color.FromArgb(100, 0, 100));
            Pen penX = new Pen(Color.FromArgb(0, 100, 50));

            for (int i = 0; i < test.Length; i++)
            {

                Pen pen = col[i] == 0 ? penY : penX;

                g.DrawLine(pen, i, (int)(Height - 20 * test[i]), i, (int)(20 * test[i]));
            }
        }
    }
}