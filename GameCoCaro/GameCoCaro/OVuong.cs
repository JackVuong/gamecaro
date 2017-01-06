using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
 * Lop O Vuong
 * 
 * Nguoi tao : Vuong Gia Luan
 * 
 * Ngay tao 28/1/2016
 * 
 */
namespace GameCoCaro
{
    class OVuong
    {
        public const int chieuRong = 20;
        public const int chieuCao = 20;

        private int soDong;

        public int SoDong
        {
            get { return soDong; }
            set { soDong = value; }
        }
        private int soCot;

        public int SoCot
        {
            get { return soCot; }
            set { soCot = value; }
        }
        private Point viTri;

        public Point ViTri
        {
            get { return viTri; }
            set { viTri = value; }
        }
        private int trangThai; // trang thai = 0 neu chua co nguoi choi nao danh vao o nay.
        //            = 1 neu nguoi choi thu 1 (computer) da danh vao o nay.
        //            = 2 neu nguoi choi thu 2 da danh vao o nay.
        public int TrangThai
        {
            get { return trangThai; }
            set { trangThai = value; }
        }

        public OVuong()
        { }

        public OVuong(int dong, int cot, Point vt, int tt)
        {
            this.soDong = dong;
            this.soCot = cot;
            this.viTri = vt;
            this.trangThai = tt;
        }
    }
}
