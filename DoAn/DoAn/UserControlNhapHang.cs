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
    public partial class UserControlNhapHang : UserControl
    {
        public string tenNV = "";
        public string maNV = "";
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        DataColumn[] primarykey1 = new DataColumn[1];
        DataColumn[] primarykey2 = new DataColumn[2];
        DataColumn[] primarykey3 = new DataColumn[1];
        public UserControlNhapHang()
        {
            InitializeComponent();
        }
        public void createSanPham()
        {
            string str = "select * from SANPHAM";
            aa = conn.getDataAdapter(str, "SANPHAM");
            primarykey[0] = conn.Dset.Tables["SANPHAM"].Columns["MASANPHAM"];
            conn.Dset.Tables["SANPHAM"].PrimaryKey = primarykey;
        }
        public void createPhieuNhap()
        {
            string str = "select * from PHIEUNHAP";
            aa = conn.getDataAdapter(str, "PHIEUNHAP");
            primarykey1[0] = conn.Dset.Tables["PHIEUNHAP"].Columns["MAPN"];
            conn.Dset.Tables["PHIEUNHAP"].PrimaryKey = primarykey1;
        }
        public void createCTPhieuNhap()
        {
            string str = "select * from CHITIETPHIEUNHAP";
            aa = conn.getDataAdapter(str, "CHITIETPHIEUNHAP");
            primarykey2[0] = conn.Dset.Tables["CHITIETPHIEUNHAP"].Columns["MAPN"];
            primarykey2[1] = conn.Dset.Tables["CHITIETPHIEUNHAP"].Columns["MASANPHAM"];
            conn.Dset.Tables["CHITIETPHIEUNHAP"].PrimaryKey = primarykey2;
        }
        public void createNhaCungCap()
        {
            string str = "select * from KHACHHANG";
            aa = conn.getDataAdapter(str, "KHACHHANG");
            primarykey3[0] = conn.Dset.Tables["KHACHHANG"].Columns["MAKHACHHANG"];
            conn.Dset.Tables["KHACHHANG"].PrimaryKey = primarykey3;
        }
        private void Load_Combobox_NhaCungCap()
        {
            string sql = "SELECT * FROM NHACUNGCAP";
            DataTable dt = conn.getDataTable(sql, "NHACUNGCAP");
            cboNhaCungCap.DataSource = dt;
            cboNhaCungCap.DisplayMember = "TENNCC";
            cboNhaCungCap.ValueMember = "MANCC";

            cboNhaCungCap.SelectedIndex = 0;
        }

        private void loadDuLieuSanPham()
        {
            string sql = "SELECT* FROM SANPHAM";
            DataTable dt = conn.getDataTable(sql, "SANPHAM");
            dgvSanPham.DataSource = dt;
            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnThanhToan.Enabled = false;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaHoaDon.Text == "" || cboNhaCungCap.Text == "" || txtNhanVien.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    btnXoa.Enabled = true;
                    btnLamMoi.Enabled = true;
                    btnThanhToan.Enabled = true;

                    int soluong = 1;
                    int tong = 0;
                    int tongThanhTien = 0;
                    string masanpham = txtMaSanPham.Text.Trim();
                    string tensanpham = txtTenSanPham.Text.Trim();
                    double dongia = double.Parse(txtDonGia.Text.Trim());
                    string mahoadon = txtMaHoaDon.Text.Trim();
                    int soluongnhaphang = int.Parse(txtSoLuongNhapSP.Text.Trim());
                    string nhacungcap = cboNhaCungCap.SelectedValue.ToString().Trim();
                    string tennhanvien = txtNhanVien.Text.Trim();
                    string ngaycapnhat = DateTime.ParseExact(txtNgayLap.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");

                    double thanhtien = dongia * soluongnhaphang;




                    string strSql1 = "select count(*) from CHITIETPHIEUNHAP where MAPN = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";
                    int dem1 = conn.getCount(strSql1);
                    if (dem1 > 0)
                    {
                        string soluongthaydoi = "SELECT * FROM CHITIETPHIEUNHAP  where MAPN = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";

                        SqlDataReader drsl = conn.getDataReader(soluongthaydoi);
                        while (drsl.Read())
                        {
                            txtSoLuong.Text = drsl["SOLUONG"].ToString();
                        }
                        drsl.Close();
                        conn.closeConnection();
                        //----------------------------------------------------------------------------------
                        int soluongTang = int.Parse(txtSoLuong.Text.Trim());
                        soluongTang++;
                        double thanhtientang = dongia * soluongTang;
                        string strSQLSL = "UPDATE CHITIETPHIEUNHAP SET SOLUONG = " + soluongTang + ", THANHTIEN = " + thanhtientang + " WHERE MASANPHAM = '" + masanpham + "' AND MAPN = '" + mahoadon + "'";
                        conn.updateTODB(strSQLSL);


                        string soluonsanphamupdate = "UPDATE SANPHAM SET SOLUONGSP = " + soluongTang + " WHERE MASANPHAM = '" + masanpham + "'";

                        conn.updateTODB(soluonsanphamupdate);
                        //----------------------------------------------------------------------------------
                        string tongthanhtien0 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";

                        SqlDataReader thanhtiendr = conn.getDataReader(tongthanhtien0);
                        while (thanhtiendr.Read())
                        {
                            tongThanhTien += int.Parse(thanhtiendr["THANHTIEN"].ToString());
                        }
                        thanhtiendr.Close();
                        conn.closeConnection();
                        lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";


                        //----------------------------------------------------------------------------------
                        string soluongiohang2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
                        SqlDataReader drslgio2 = conn.getDataReader(soluongiohang2);
                        while (drslgio2.Read())
                        {
                            tong += int.Parse(drslgio2["SOLUONG"].ToString());
                        }
                        drslgio2.Close();
                        conn.closeConnection();


                        label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";


                        //----------------------------------------------------------------------------------

                        string loadLaiDuLieu2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
                        dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu2);
                        return;
                    }
                    //----------------------------------------------------------------------------------
                    string strSql = "select count(*) from CHITIETPHIEUNHAP where MAPN = '" + mahoadon + "'";
                    int dem = conn.getCount(strSql);
                    if (dem > 0)
                    {

                        string strSQLCHITIETHOADON1 = "INSERT CHITIETPHIEUNHAP (MAPN,MASANPHAM,SOLUONG,DONGIA,THANHTIEN) VALUES('" + mahoadon + "', '" + masanpham + "'," + soluongnhaphang + ", " + dongia + ", " + thanhtien + ")";
                        conn.updateTODB(strSQLCHITIETHOADON1);
                        string soluongiohang1 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";


                        SqlDataReader drslgio1 = conn.getDataReader(soluongiohang1);
                        while (drslgio1.Read())
                        {
                            tong += int.Parse(drslgio1["SOLUONG"].ToString());
                        }
                        drslgio1.Close();
                        conn.closeConnection();


                        label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";
                        string loadLaiDuLieu1 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
                        dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu1);
                      
                        //----------------------------------------------------------------------------------
                        string tongthanhtien1 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";


                        SqlDataReader thanhtiendr1 = conn.getDataReader(tongthanhtien1);
                        while (thanhtiendr1.Read())
                        {
                            tongThanhTien += int.Parse(thanhtiendr1["THANHTIEN"].ToString());
                        }
                        thanhtiendr1.Close();
                        conn.closeConnection();
                        lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";
                        return;
                    }



                    //strSQL = "INSERT HOADON VALUES('" + mahoadon + "','" + ngaycapnhat + "','" + 15 + "','" + 2 + "','" + dongia + "','" + dongia + "')";
                    string strSQL = "INSERT PHIEUNHAP (MAPN,ID,MANCC,NGAYLAP,TONGTIEN) VALUES('" + mahoadon + "', '" + maNV + "','" + nhacungcap + "','" + ngaycapnhat + "', NULL)";

                    conn.updateTODB(strSQL);

                    layten.LuuThongTinThanhTien(txtMaHoaDon.Text);
                    string strSQLCHITIETHOADON = "INSERT CHITIETPHIEUNHAP (MAPN,MASANPHAM,SOLUONG,DONGIA,THANHTIEN) VALUES('" + mahoadon + "', '" + masanpham + "'," + soluongnhaphang + ", " + dongia + ", " + thanhtien + ")";
                    conn.updateTODB(strSQLCHITIETHOADON);

                    string soluongiohang = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";


                    SqlDataReader drslgio = conn.getDataReader(soluongiohang);
                    while (drslgio.Read())
                    {
                        tong += int.Parse(drslgio["SOLUONG"].ToString());
                    }
                    drslgio.Close();
                    conn.closeConnection();


                    label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";



                    string tongthanhtien2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";


                    SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                    while (thanhtiendr2.Read())
                    {
                        tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                    }
                    thanhtiendr2.Close();
                    conn.closeConnection();
                    lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";


                    string loadLaiDuLieu = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
                    dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
                }

            }
            catch
            {
                MessageBox.Show("thất bại");
            }


        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string mahoadon = txtMaHoaDon.Text.Trim();
            int soluongnhaphang = int.Parse(txtSoLuongNhapSP.Text.Trim());
            string nhacungcap = cboNhaCungCap.SelectedValue.ToString().Trim();
            string tennhanvien = txtNhanVien.Text.Trim();
            string tenkhachang = "SELECT * FROM PHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
            string tennv = "";
            string idtenNCC = "";
            string idtennv = "";
            string thoigian = "";
            SqlDataReader dr = conn.getDataReader(tenkhachang);
            while (dr.Read())
            {
                thoigian = dr["NGAYLAP"].ToString();
                idtenNCC = dr["MANCC"].ToString();
                idtennv = dr["ID"].ToString();
            }
            dr.Close();
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

            string tenncc = "";
            string laytenncc = "SELECT * FROM NHACUNGCAP WHERE MANCC = '" + idtenNCC + "'";
            SqlDataReader dr1 = conn.getDataReader(laytenncc);
            while (dr1.Read())
            {
                tenncc = dr1["TENNCC"].ToString();

            }
            dr1.Close();
            conn.closeConnection();
            layten.LuuThongTintenKH(tenncc);

            //FrmPhieuNhap tt = new FrmPhieuNhap();
            //tt.ShowDialog();
            DialogResult dialogResult = MessageBox.Show("Bạn có muốn in hóa đơn không", "THÔNG BÁO", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                FrmPhieuNhap tt = new FrmPhieuNhap();
                tt.ShowDialog();
                int tongThanhTien = 0;
                mahoadon = txtMaHoaDon.Text.Trim();
                string tongthanhtien2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";
                SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                while (thanhtiendr2.Read())
                {
                    tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                }
                thanhtiendr2.Close();
                conn.closeConnection();
                string updateTONGTIEN = "UPDATE PHIEUNHAP SET TONGTIEN = " + tongThanhTien + " WHERE MAPN = '" + mahoadon + "'";

                conn.updateTODB(updateTONGTIEN);
                txtMaHoaDon.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = false;
                txtMaHoaDon.Text = "";
                string mahoadon1 = "";
                string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon1 + "'";
                dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
                txtMaHoaDon.Text = "";
                //txtTenKhachHang.Text = "";
                txtMaSanPham.Text = "";
                txtTenSanPham.Text = "";
                txtDonGia.Text = "";
                lbTongTien.Text = "0 VNĐ";
                label11.Text = "Tổng số lượng trong giỏ hàng: 0 món";
                return;
            }
            else if (dialogResult == DialogResult.No)
            {
                int tongThanhTien = 0;
                mahoadon = txtMaHoaDon.Text.Trim();
                string tongthanhtien2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";
                SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                while (thanhtiendr2.Read())
                {
                    tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                }
                thanhtiendr2.Close();
                conn.closeConnection();
                string updateTONGTIEN = "UPDATE PHIEUNHAP SET TONGTIEN = " + tongThanhTien + " WHERE MAPN = '" + mahoadon + "'";

                conn.updateTODB(updateTONGTIEN);
                txtMaHoaDon.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = false;
                txtMaHoaDon.Text = "";
                string mahoadon1 = "";
                string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon1 + "'";
                dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
                txtMaHoaDon.Text = "";
                //txtTenKhachHang.Text = "";
                txtMaSanPham.Text = "";
                txtTenSanPham.Text = "";
                txtDonGia.Text = "";
                lbTongTien.Text = "0 VNĐ";
                label11.Text = "Tổng số lượng trong giỏ hàng: 0 món";
            }
            
        }

        private void btnThemSoLuong_Click(object sender, EventArgs e)
        {
            int tong = 0;
            string masanpham = txtMaSanPhamAn.Text.Trim();
            string mahoadon = txtMaHoaDonAn.Text.Trim();
            string mahoadondanghien = txtMaHoaDon.Text.Trim();
            int sl = int.Parse(txtSoLuongSanPhamChon.Text.Trim());
            int dongiagoc = 0;
            int tongThanhTien = 0;
            int soluonggoc = 0;
            string dongia = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader dongiadr = conn.getDataReader(dongia);
            while (dongiadr.Read())
            {
                dongiagoc = int.Parse(dongiadr["DONGIA"].ToString());
                soluonggoc = int.Parse(dongiadr["SOLUONGSP"].ToString());
            }
            dongiadr.Close();



            string tongthanhtien2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";






            double thanhtien = dongiagoc * (sl + soluonggoc);
            int soluongsauupdate = sl + soluonggoc;
            string soluongiohang = "UPDATE CHITIETPHIEUNHAP SET SOLUONG = " + soluongsauupdate + ", THANHTIEN = " + thanhtien + " WHERE MAPN = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";

            conn.updateTODB(soluongiohang);
            string soluonsanphamupdate = "UPDATE SANPHAM SET SOLUONGSP = " + soluongsauupdate + " WHERE MASANPHAM = '" + masanpham + "'";

            conn.updateTODB(soluonsanphamupdate);
            SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
            while (thanhtiendr2.Read())
            {
                tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
            }
            thanhtiendr2.Close();
            conn.closeConnection();
            lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";





            string soluongiohangThayDoi = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";


            SqlDataReader drslgio = conn.getDataReader(soluongiohangThayDoi);
            while (drslgio.Read())
            {
                tong += int.Parse(drslgio["SOLUONG"].ToString());
            }
            drslgio.Close();
            conn.closeConnection();


            label11.Text = "Số lượng trong giỏ hàng: " + tong.ToString() + " món";




            string loadLaiDuLieuSP = "SELECT * FROM SANPHAM";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieuSP);

            string loadLaiDuLieu = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadondanghien + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaHoaDon.Enabled = true;
            checkBox2.Enabled = true;
            checkBox2.Checked = false;
            string mahoadon = txtMaHoaDon.Text.Trim();
            string strSQL = "DELETE CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
            conn.updateTODB(strSQL);
            MessageBox.Show("Làm thành công nha ^^");
            string loadLaiDuLieu = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
            txtMaHoaDon.Text = "";
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtDonGia.Text = "";
            lbTongTien.Text = "0 VNĐ";
            label11.Text = "Tổng số lượng trong giỏ hàng: 0 món";
            txtSoLuong.Text = "";
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int tongThanhTien = 0;
            int tong = 0;
            string mahoadon = txtMaHoaDon.Text.Trim();
            string masanpham = txtMaSanPhamAn.Text.Trim();
            string strSQL = "DELETE CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";
            conn.updateTODB(strSQL);
            string soluongiohang = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";


            SqlDataReader drslgio = conn.getDataReader(soluongiohang);
            while (drslgio.Read())
            {
                tong += int.Parse(drslgio["SOLUONG"].ToString());
            }
            drslgio.Close();
            conn.closeConnection();


            label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";



            string tongthanhtien2 = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = N'" + mahoadon + "'";


            SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
            while (thanhtiendr2.Read())
            {
                tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
            }
            thanhtiendr2.Close();
            conn.closeConnection();
            lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";
            string loadLaiDuLieu = "SELECT * FROM CHITIETPHIEUNHAP WHERE MAPN = '" + mahoadon + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                int Numrd;

                Random rd = new Random();
                Numrd = rd.Next(1, 100);//biến Numrd sẽ nhận có giá trị ngẫu nhiên trong khoảng 1000 đến 99999
                txtMaHoaDon.Text = "PN0" + rd.Next(1000, 99999).ToString();
                txtMaHoaDon.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void UserControlNhapHang_Load(object sender, EventArgs e)
        {
            this.dgvHoaDon.DefaultCellStyle.ForeColor = Color.Black;
            this.dgvSanPham.DefaultCellStyle.ForeColor = Color.Black;
            txtNgayLap.Text = DateTime.Now.ToString();

            createSanPham();
            createPhieuNhap();
            createCTPhieuNhap();
            createNhaCungCap();
            Load_Combobox_NhaCungCap();
            txtNhanVien.Text = layten.LayThongTinDangNhap();
            maNV = layten.LayThongTinID();
            
            
            loadDuLieuSanPham();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvSanPham_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaSanPham.Text = dgvSanPham.Rows[index].Cells[0].Value.ToString().Trim();
            txtTenSanPham.Text = dgvSanPham.Rows[index].Cells[1].Value.ToString().Trim();
            txtDonGia.Text = dgvSanPham.Rows[index].Cells[4].Value.ToString().Trim();
            txtSoLuongNhapSP.Text = dgvSanPham.Rows[index].Cells[5].Value.ToString().Trim();

        }

        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaHoaDonAn.Text = dgvHoaDon.Rows[index].Cells[0].Value.ToString().Trim();
            txtMaSanPhamAn.Text = dgvHoaDon.Rows[index].Cells[1].Value.ToString().Trim();
            txtSoLuongSanPhamChon.Text = dgvHoaDon.Rows[index].Cells[2].Value.ToString().Trim();
            txtDonGiaAn.Text = dgvHoaDon.Rows[index].Cells[3].Value.ToString().Trim();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (conn.searchSanPham(txtTimKiem.Text) != null)
            {
                dgvSanPham.DataSource = conn.searchSanPham(txtTimKiem.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void BtnTroLai_Click(object sender, EventArgs e)
        {

            dgvSanPham.DataSource = conn.Dset.Tables["SANPHAM"];
        }

        private void pTitle_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
