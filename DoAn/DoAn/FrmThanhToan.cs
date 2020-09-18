using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien;
using System.IO;

namespace DoAn
{
    public partial class FrmThanhToan : Form
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        DataColumn[] primarykey1 = new DataColumn[1];
        DataColumn[] primarykey2 = new DataColumn[2];
        DataColumn[] primarykey3 = new DataColumn[1];
        public FrmThanhToan()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public void createSanPham()
        {
            string str = "select * from SANPHAM";
            aa = conn.getDataAdapter(str, "SANPHAM");
            primarykey[0] = conn.Dset.Tables["SANPHAM"].Columns["MASANPHAM"];
            conn.Dset.Tables["SANPHAM"].PrimaryKey = primarykey;
        }
        public void createKhachHang()
        {
            string str = "select * from KHACHHANG";
            aa = conn.getDataAdapter(str, "KHACHHANG");
            primarykey3[0] = conn.Dset.Tables["KHACHHANG"].Columns["MAKHACHHANG"];
            conn.Dset.Tables["KHACHHANG"].PrimaryKey = primarykey3;
        }
        public void createHoaDon()
        {
            string str = "select * from HOADON";
            aa = conn.getDataAdapter(str, "HOADON");
            primarykey1[0] = conn.Dset.Tables["HOADON"].Columns["MAHD"];
            conn.Dset.Tables["HOADON"].PrimaryKey = primarykey1;
        }
        public void createCTHoaDon()
        {
            string str = "select * from CHITIETHOADON";
            aa = conn.getDataAdapter(str, "CHITIETHOADON");
            primarykey2[0] = conn.Dset.Tables["CHITIETHOADON"].Columns["MAHD"];
            primarykey2[1] = conn.Dset.Tables["CHITIETHOADON"].Columns["MASANPHAM"];
            conn.Dset.Tables["CHITIETHOADON"].PrimaryKey = primarykey2;
        }
        int tongtientraluu;
        private void FrmThanhToan_Load(object sender, EventArgs e)
        {
            createHoaDon();
            createCTHoaDon();
            createSanPham();
            createKhachHang();
            UserControlBanHang a = new UserControlBanHang();
            int tongThanhTien = 0;
            string mahoadon = layten.LayThongTinThanhTien();
            lbMaHoaDon.Text = mahoadon;
            string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";


            SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
            while (thanhtiendr2.Read())
            {
                tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
            }
            thanhtiendr2.Close();
            conn.closeConnection();
            tongtientraluu = tongThanhTien;
            lbTong.Text = String.Format("{0:0,0}", tongThanhTien) + " VNĐ";

            string tenkhachang = "SELECT * FROM HOADON WHERE MAHD = N'" + mahoadon + "'";

            string idten = "";
            string idtennv = "";
            string thoigian = "";
            SqlDataReader dr = conn.getDataReader(tenkhachang);
            while (dr.Read())
            {
                thoigian = dr["NGAYLAP"].ToString();
                idten = dr["MAKHACHHANG"].ToString();
                idtennv = dr["ID"].ToString();
            }
            dr.Close();
            conn.closeConnection();
            string tenkh = "";
            string tennv = "";
            string laytenkh = "SELECT * FROM KHACHHANG WHERE MAKHACHHANG = '" + idten + "'";
            SqlDataReader dr1 = conn.getDataReader(laytenkh);
            while (dr1.Read())
            {
                tenkh = dr1["TENKHACHHANG"].ToString();
                
            }
            dr1.Close();
            conn.closeConnection();

            string laytennv = "SELECT * FROM NHANVIEN WHERE ID = '" + idtennv + "'";
            SqlDataReader dr2 = conn.getDataReader(laytennv);
            while (dr2.Read())
            {
                tennv = dr2["HOTEN"].ToString();

            }
            dr2.Close();
            conn.closeConnection();
            layten.LuuThongTintg(thoigian);
            layten.LuuThongTinTenNV(tennv);
            lbTenKhachHang.Text = tenkh;
            layten.LuuThongTintenKH(tenkh);
            label7.Text = "Nhân viên bán hàng: " + tennv;
            label8.Text = "Thời gian: "+ thoigian;
        }

        private void txtTienCuaKhach_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) button1_Click(sender, e);
        }

        private void txtTienCuaKhach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtTienCuaKhach_MouseEnter(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int tongtientra = 0;
                int tien = 0;
                tien = int.Parse(txtTienCuaKhach.Text);


                tongtientra = tien - tongtientraluu;
                if (tien < tongtientraluu)
                {
                    MessageBox.Show("Tiền bạn nhập không đủ");
                    return;
                }
                layten.LuuThongTienthoi(tongtientra);
                lbTraTienThua.Text = String.Format("{0:0,0.0}", tongtientra) + " VND";
            }
            catch { MessageBox.Show("Lỗi vui lòng thử lại"); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmHoaDon a = new FrmHoaDon();
            a.ShowDialog();
        }
        
    }
}
