using TestApp.src;

namespace TestApp
{
    public partial class Form1 : Form
    {
        Graphics g;
        Map map;
        Player p1;
        double[] test = new double[720];
        int[] col = new int[720];

        public Form1()
        {
            InitializeComponent();

            p1 = new Player(7, 2, 0, -1);
            map = new Map();

            (test, col) = p1.CalculateRays(ref map, 60);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1_Load();

            Pen penY = new Pen(Color.FromArgb(100, 0, 100));
            Pen penX = new Pen(Color.FromArgb(0, 100, 50));

            for (int i = 0; i < test.Length; i++)
            {
                Console.WriteLine(test[i]);

                Pen pen = col[i] == 0 ? penY : penX;

                g.DrawLine(pen, i, (int)(Height - 20 * test[i]), i, (int)(20 * test[i]));
            }
        }

        private void panel1_Load()
        {
            Graphics pG = panel1.CreateGraphics();
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

            int playerPosX = (int)p1.X * blockSize;
            int playerPosY = (int)p1.Y * blockSize;

            // draw player
            pG.DrawEllipse(pen, playerPosX, playerPosY, 5, 5);

            // draw 3 "look" rays
            pG.DrawLine(pen, playerPosX, playerPosY, (int)(playerPosX + p1.DirX * 20), (int)(playerPosY + p1.DirY * 20));
        }
    }
}