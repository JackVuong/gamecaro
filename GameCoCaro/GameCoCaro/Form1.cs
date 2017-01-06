using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCoCaro
{
    public partial class CaroForm : Form
    {
        private Game game;
        Graphics gr;
        public CaroForm()
        {
            InitializeComponent();
            game = new Game();
            gr = pnlBanCo.CreateGraphics();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game.khoiTaoMangOCo();
            game.vaoGame(gr);
        }

        private void pnlBanCo_Paint(object sender, PaintEventArgs e)
        {
            game.veBanCo(gr);
            game.veLaiMangOCo(gr);
        }

        private void pnlBanCo_MouseClick(object sender, MouseEventArgs e)
        {
            if (game.vaoTran == false)
                return;
            game.danhCo(e.X, e.Y, gr);
            if (game.kiemTraChienThang())
            {
                if (game.getResult() == 0)
                    MessageBox.Show("Hòa");
                if (game.getResult() == 1)
                    MessageBox.Show("Computer win");
                if (game.getResult() == 2)
                    MessageBox.Show("You win");
                return;
            }
            game.vaoGame(gr);
            if (game.kiemTraChienThang())
            {
                if (game.getResult() == 0)
                    MessageBox.Show("Hòa");
                if (game.getResult() == 1)
                    MessageBox.Show("Computer win");
                if (game.getResult() == 2)
                    MessageBox.Show("You win");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gr.Clear(pnlBanCo.BackColor);
            game.undo(gr);
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            gr.Clear(pnlBanCo.BackColor);
            game.newGame(gr);
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnNewGame_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
