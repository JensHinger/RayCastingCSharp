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

            //p1 = new Player(6, 6, 1, 0);
            p1 = new Player(5.754540397359849, 5.833449191110242, -0.93143534710751841, 0.0995057873530461);
            map = new Map();
            
            g = this.CreateGraphics();
            pG = panel1.CreateGraphics();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void Form1_KeyDown(Object sender, KeyEventArgs e)
        {
            bool didChange = false;

            if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                // Move in "look" direction
                p1.Move(forward: true);
                didChange = true;
            }
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                // Move away from "look" direction
                p1.Move(forward: false);
                didChange = true;
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                // Rotate charakter left
                p1.Rotate(right: false);
                didChange = true;
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                // Rotate charakter right
                p1.Rotate(right: true);
                didChange = true;
            }

            if (didChange) { UpdateView(); }
        }

        private void UpdateView()
        {
            (test, col) = p1.CalculateRays(ref map, 90);

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
                        pG.DrawRectangle(pen, blockSize * j, blockSize * i, blockSize, blockSize);
                    }
                }
            }

            int playerPosX = (int)(p1.X * blockSize);
            int playerPosY = (int)(p1.Y * blockSize);

            // draw player
            pG.DrawEllipse(pen, playerPosX - 2, playerPosY - 2, 4, 4);

            // draw 3 "look" rays
            pG.DrawLine(pen, playerPosX, playerPosY, (int)(playerPosX + (p1.DirX * 20)), (int)(playerPosY + (p1.DirY * 20)));
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