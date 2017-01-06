using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Lop Ban Co
 * 
 * Nguoi tao : Vuong Gia Luan
 * 
 * Ngay tao 28/1/2016
 * 
 */
namespace GameCoCaro
{
    class BanCo
    {
        private int tongSoDong;

        public int TongSoDong
        {
            get { return tongSoDong; }
        }
        private int tongSoCot;

        public int TongSoCot
        {
            get { return tongSoCot; }          
        }

        public BanCo()
        {
            this.tongSoDong = 0;
            this.tongSoCot = 0;
        }

        public BanCo(int soDong,int soCot)
        {
            this.tongSoDong = soDong;
            this.tongSoCot = soCot;
        }

        public void veBanCo(Graphics g)
        {
            for (int i = 0; i <= tongSoCot; i++)
            {
                g.DrawLine(Game.pen, i * OVuong.chieuRong, 0, i * OVuong.chieuRong, tongSoDong * OVuong.chieuCao);
            }
            for (int j = 0; j <= tongSoDong; j++)
            {
                g.DrawLine(Game.pen, 0, j * OVuong.chieuCao, tongSoCot * OVuong.chieuRong, j * OVuong.chieuCao);
            }
        }

        public void veQuanCo(Graphics g, Point viTri, SolidBrush sb)
        {
            g.FillEllipse(sb, viTri.X + 2, viTri.Y + 2, OVuong.chieuRong -4, OVuong.chieuCao-4);

        }
    }
}
