using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCoCaro
{
    class Game
    {
        public static Pen pen;
        public static SolidBrush red;
        public static SolidBrush green;
        private Stack<OVuong> dsCacNuocDaDanh;
        private int turn;

        private BanCo banCo;
        private OVuong[,] mangOCo;
        private int ketQua; // = 0 neu hoa co, = 1 neu nguoi choi 1 thang (com), = 2 nen nguoi choi 2 thang.
        public bool vaoTran = false;
        public Game()
        {
            vaoTran = true;
            pen = new Pen(Color.Brown);
            green = new SolidBrush(Color.Green);
            red = new SolidBrush(Color.Red);
            banCo = new BanCo(17,20);
            mangOCo = new OVuong[banCo.TongSoDong, banCo.TongSoCot];
            dsCacNuocDaDanh = new Stack<OVuong>();
            turn = 1;

        }

        public int getResult()
        {
            return ketQua;
        }
        public void veBanCo(Graphics graph)
        {
            banCo.veBanCo(graph);
        }

        public void khoiTaoMangOCo()
        {
            for (int i = 0; i < banCo.TongSoDong; i++)
            {
                for (int j = 0; j < banCo.TongSoCot; j++)
                {
                    mangOCo[i,j] = new OVuong(i,j,new Point(j*OVuong.chieuRong,i*OVuong.chieuCao),0);
                }
            }
        }

        public bool danhCo(int xCuaChuot, int yCuaChuot,Graphics g)
        {
            int cot = xCuaChuot / OVuong.chieuRong;
            int dong = yCuaChuot / OVuong.chieuCao;
            if (mangOCo[dong, cot].TrangThai != 0) //neu nhu o do da co nguoi choi chon vao
            {
                return false; //thi khong danh duoc
            }
            switch (turn)
            {
                case 1:
                    mangOCo[dong, cot].TrangThai = 1;
                    banCo.veQuanCo(g, mangOCo[dong, cot].ViTri, green);
                    turn = 2;//chuyen turn cho nguoi choi 2.
                    break;
                case 2:
                    mangOCo[dong, cot].TrangThai = 2;
                    banCo.veQuanCo(g, mangOCo[dong, cot].ViTri, red);
                    turn = 1; //chuyen turn cho nguoi choi 1.
                    break;
            }
            dsCacNuocDaDanh.Push(mangOCo[dong, cot]);
            return true;
        }

        public void veLaiMangOCo(Graphics g)
        {
            foreach (OVuong oco in dsCacNuocDaDanh)
            {
                if (oco.TrangThai == 1)
                {
                    banCo.veQuanCo(g, oco.ViTri, green);
                }
                else if (oco.TrangThai == 2)
                {
                    banCo.veQuanCo(g, oco.ViTri, red);
                }
            }
        }

        public void undo(Graphics g)
        {
            if (dsCacNuocDaDanh.Count != 0)
            {
                OVuong oco = dsCacNuocDaDanh.Pop();
                mangOCo[oco.SoDong, oco.SoCot].TrangThai = 0;
                if (turn == 1)
                    turn = 2;
                else
                    turn = 1;
            }
            veBanCo(g);
            veLaiMangOCo(g);
            
            
        }

        public bool kiemTraChienThang()
        {
            if (dsCacNuocDaDanh.Count == banCo.TongSoDong * banCo.TongSoCot)
            {
                ketQua = 0; //hoa co.
                vaoTran = false;
                return true;
            }
            foreach(OVuong oco in dsCacNuocDaDanh)
            {
                if (duyetDoc(oco.SoDong, oco.SoCot, oco.TrangThai) || duyetNgang(oco.SoDong, oco.SoCot, oco.TrangThai) || duyetCheoNguoc(oco.SoDong, oco.SoCot, oco.TrangThai) || duyetCheoXuoi(oco.SoDong, oco.SoCot, oco.TrangThai))
                {
                    ketQua = oco.TrangThai == 1 ? 1 : 2;
                    vaoTran = false;
                    return true;
                }
            }
            return false;
        }

        public void newGame(Graphics g)        
        {
            vaoTran = true;
            dsCacNuocDaDanh = new Stack<OVuong>();
            turn = 1;
            khoiTaoMangOCo();
            veBanCo(g);
            vaoGame(g);

        }
        private bool duyetDoc(int currDong, int currCot, int currTrangThai)
        {
            int dem;
            if (currDong > banCo.TongSoDong - 5)
                return false;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong + dem, currCot].TrangThai != currTrangThai)
                    return false;
            }
            if (currDong == 0 || currDong + dem == banCo.TongSoDong)
                return true;
            if (mangOCo[currDong - 1, currCot].TrangThai == 0 || mangOCo[currDong + dem, currCot].TrangThai == 0)
                return true;
            
            return false;
        }

        private bool duyetNgang(int currDong, int currCot, int currTrangThai)
        {
            int dem;
            if (currCot > banCo.TongSoCot - 5)
                return false;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong, currCot + dem].TrangThai != currTrangThai)
                    return false;
            }
            if (currCot == 0 || currCot + dem == banCo.TongSoCot)
                return true;
            if (mangOCo[currDong, currCot - 1].TrangThai == 0 || mangOCo[currDong, currCot + dem].TrangThai == 0)
                return true;

            return false;
        }

        private bool duyetCheoXuoi(int currDong, int currCot, int currTrangThai)
        {
            int dem;
            if (currDong > banCo.TongSoDong - 5 || currCot >banCo.TongSoCot -5)
                return false;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong +dem, currCot + dem].TrangThai != currTrangThai)
                    return false;
            }
            if (currDong ==0 || currDong+dem == banCo.TongSoDong || currCot == 0 || currCot + dem == banCo.TongSoCot)
                return true;
            if (mangOCo[currDong -1, currCot - 1].TrangThai == 0 || mangOCo[currDong +dem, currCot + dem].TrangThai == 0)
                return true;

            return false;
        }

        private bool duyetCheoNguoc(int currDong, int currCot, int currTrangThai)
        {
            int dem;
            if (currDong < 4 || currCot > banCo.TongSoCot - 5)
                return false;
            for (dem = 1; dem < 5; dem++)
            {
                if (mangOCo[currDong - dem, currCot + dem].TrangThai != currTrangThai)
                    return false;
            }
            if (currDong == 4 || currDong == banCo.TongSoDong - 1 || currCot == 0 || currCot + dem == banCo.TongSoCot)
                return true;
            if (mangOCo[currDong+1, currCot - 1].TrangThai == 0 || mangOCo[currDong-dem, currCot + dem].TrangThai == 0)
                return true;

            return false;
        }
        private long[] mangDiemTanCong = new long[7] { 0, 3, 24, 192, 1536, 15000, 100000 };
        private long[] mangDiemPhongThu = new long[7] { 0, 2, 20, 190, 1000, 10000, 700000 };
        public void vaoGame(Graphics g)
        {
            if (dsCacNuocDaDanh.Count == 0)
            {
                danhCo(banCo.TongSoCot / 2 * OVuong.chieuCao, banCo.TongSoDong / 2 * OVuong.chieuRong, g);
            }
            else
            {
                OVuong oco = timKiemNuocDi();
                danhCo(oco.ViTri.X + 1, oco.ViTri.Y +1,g);
            }
        }
        private OVuong timKiemNuocDi()
        {
            OVuong result = new OVuong();
            long maxPoint = 0;
            for (int i = 0; i < banCo.TongSoDong; i++)
            {
                for (int j = 0; j < banCo.TongSoCot; j++)
                {
                    if (mangOCo[i, j].TrangThai == 0)
                    {
                        long diemTanCong = DiemTC_DuyetDoc(i, j) + DiemTC_DuyetNgang(i, j) + DiemTC_DuyetCheoXuoi(i, j) + DiemTC_DuyetCheoNguoc(i, j);
                        long diemPhongThu = DiemPT_DuyetDoc(i, j) + DiemPT_DuyetNgang(i, j) + DiemPT_DuyetCheoXuoi(i, j) + DiemPT_DuyetCheoNguoc(i, j);
                        long t = diemTanCong > diemPhongThu ? diemTanCong : diemPhongThu;
                        if (maxPoint < t)
                        {
                            maxPoint = t;
                            result = new OVuong(mangOCo[i, j].SoDong, mangOCo[i, j].SoCot, mangOCo[i, j].ViTri,1);
                        }
                    }
                }
            }

            return result;
        }
        private long DiemTC_DuyetDoc(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentD + dem < banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD + dem, currentC].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD + dem, currentC].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentD - dem >=0; dem++)
            {
                if (mangOCo[currentD - dem, currentC].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD - dem, currentC].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }

            if (soQuanCuaNguoiChoi == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaNguoiChoi];
            tong += this.mangDiemTanCong[soQuanCuaMay];
            return tong;
        }
        private long DiemTC_DuyetNgang(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot; dem++)
            {
                if (mangOCo[currentD, currentC+dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD, currentC+dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0; dem++)
            {
                if (mangOCo[currentD , currentC-dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD, currentC-dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }

            if (soQuanCuaNguoiChoi == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaNguoiChoi];
            tong += this.mangDiemTanCong[soQuanCuaMay];
            return tong;
        }
        private long DiemTC_DuyetCheoXuoi(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot && currentD + dem <banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD + dem, currentC + dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD + dem, currentC + dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0 && currentD - dem >= 0; dem++)
            {
                if (mangOCo[currentD - dem, currentC - dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD - dem, currentC - dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }

            if (soQuanCuaNguoiChoi == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaNguoiChoi];
            tong += this.mangDiemTanCong[soQuanCuaMay];
            return tong;
        }
        private long DiemTC_DuyetCheoNguoc(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot && currentD-dem >=0 ; dem++)
            {
                if (mangOCo[currentD -dem, currentC+dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD - dem, currentC+dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0 && currentD + dem < banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD +dem, currentC-dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                }
                else if (mangOCo[currentD + dem, currentC-dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                    break;
                }
                else
                    break;
            }

            if (soQuanCuaNguoiChoi == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaNguoiChoi];
            tong += this.mangDiemTanCong[soQuanCuaMay];
            return tong;
        }

        //Phong thu
        private long DiemPT_DuyetDoc(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentD + dem < banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD + dem, currentC].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD + dem, currentC].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentD - dem >= 0; dem++)
            {
                if (mangOCo[currentD - dem, currentC].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD - dem, currentC].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }

            if (soQuanCuaMay == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaMay+1];
            tong += this.mangDiemTanCong[soQuanCuaNguoiChoi];

            return tong;
        }
        private long DiemPT_DuyetNgang(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot; dem++)
            {
                if (mangOCo[currentD, currentC + dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD, currentC + dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0; dem++)
            {
                if (mangOCo[currentD, currentC - dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD, currentC - dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }

            if (soQuanCuaMay == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaMay+1];
            tong += this.mangDiemTanCong[soQuanCuaNguoiChoi];
            return tong;
        }
        private long DiemPT_DuyetCheoXuoi(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot && currentD + dem < banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD + dem, currentC + dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD + dem, currentC + dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0 && currentD - dem >= 0; dem++)
            {
                if (mangOCo[currentD - dem, currentC - dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD - dem, currentC - dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }

            if (soQuanCuaMay == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaMay+1];
            tong += this.mangDiemTanCong[soQuanCuaNguoiChoi];
            return tong;
        }
        private long DiemPT_DuyetCheoNguoc(int currentD, int currentC)
        {
            long tong = 0;
            int soQuanCuaMay = 0;
            int soQuanCuaNguoiChoi = 0;
            for (int dem = 1; dem < 6 && currentC + dem < banCo.TongSoCot && currentD - dem >= 0; dem++)
            {
                if (mangOCo[currentD - dem, currentC + dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD - dem, currentC + dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }
            for (int dem = 1; dem < 6 && currentC - dem >= 0 && currentD + dem < banCo.TongSoDong; dem++)
            {
                if (mangOCo[currentD + dem, currentC - dem].TrangThai == 1) // quan cua may
                {
                    soQuanCuaMay++;
                    break;
                }
                else if (mangOCo[currentD + dem, currentC - dem].TrangThai == 2)
                {
                    soQuanCuaNguoiChoi++;
                }
                else
                    break;
            }

            if (soQuanCuaMay == 2)
                return 0;
            tong -= this.mangDiemPhongThu[soQuanCuaMay+1];
            tong += this.mangDiemTanCong[soQuanCuaNguoiChoi];
            return tong;
        }

    }
}
