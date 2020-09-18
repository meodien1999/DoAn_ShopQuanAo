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
    public partial class UserControlBanHang : UserControl
    {
        public string tenNV = "";
        public string maNV = "";
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        DataColumn[] primarykey1 = new DataColumn[1];
        DataColumn[] primarykey2 = new DataColumn[2];
        DataColumn[] primarykey3 = new DataColumn[1];

        public UserControlBanHang()
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
        public void createKhachHang()
        {
            string str = "select * from KHACHHANG";
            aa = conn.getDataAdapter(str, "KHACHHANG");
            primarykey3[0] = conn.Dset.Tables["KHACHHANG"].Columns["MAKHACHHANG"];
            conn.Dset.Tables["KHACHHANG"].PrimaryKey = primarykey3;
        }
        private void LoadcbKH()
        {
            string sql = "SELECT * FROM KHACHHANG";
            DataTable dt = conn.getDataTable(sql, "KHACHHANG");
            cbKhachHang.DataSource = dt;
            cbKhachHang.DisplayMember = "TENKHACHHANG";
            cbKhachHang.ValueMember = "MAKHACHHANG";

            cbKhachHang.SelectedIndex = 0;
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
        
        private void loadDuLieuHoaDon()
        {
            dgvHoaDon.DataSource = conn.Dset.Tables["HOADON"];
        }
        
        public void UserControlBanHang_Load(object sender, EventArgs e)
        {
            this.dgvHoaDon.DefaultCellStyle.ForeColor = Color.Black;
            this.dgvSanPham.DefaultCellStyle.ForeColor = Color.Black;
            txtNgayLap.Text = DateTime.Now.ToString();
            createSanPham();
            createHoaDon();
            createCTHoaDon();
            createKhachHang();
            
            txtNhanVien.Text = layten.LayThongTinDangNhap();
            maNV = layten.LayThongTinID();
                    
            loadDuLieuSanPham();
           
        }

    
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            //int tongThanhTien = 0;

           
                //string mahoadon = txtMaHoaDon.Text.ToString().Trim();
                //string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";


                //SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                //while (thanhtiendr2.Read())
                //{
                //    tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                //}
                //thanhtiendr2.Close();
                //conn.closeConnection();
                //tongThanhTien = (tongThanhTien * 90) / 100;
                //MessageBox.Show("Tổng tiền đã giảm 10% cần thanh toán là: " + tongThanhTien + "VNĐ");
                FrmThanhToan tt = new FrmThanhToan();
                tt.ShowDialog();
                int tongThanhTien = 0;
                string mahoadon = txtMaHoaDon.Text.Trim();
                string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";
                SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                while (thanhtiendr2.Read())
                {
                    tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                }
                thanhtiendr2.Close();
                conn.closeConnection();
                string updateTONGTIEN = "UPDATE HOADON SET TONGTIEN = " + tongThanhTien + " WHERE MAHD = '" + mahoadon + "'";

                conn.updateTODB(updateTONGTIEN);
                txtMaHoaDon.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = false;
                txtMaHoaDon.Text = "";
                string mahoadon1 = "";
                string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon1 + "'";
                dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
                txtMaHoaDon.Text = "";
                txtTenKhachHang.Text = "";
                txtMaSanPham.Text = "";
                txtTenSanPham.Text = "";
                txtDonGia.Text = "";
                lbTongTien.Text = "0 VNĐ";
                label11.Text = "Tổng số lượng trong giỏ hàng: 0 món";
           
            
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
        int dongiatinh = 0;
        float loi = 0;
        string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
        private void dgvSanPham_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
           txtMaSanPham.Text = dgvSanPham.Rows[index].Cells[0].Value.ToString().Trim();
           txtTenSanPham.Text = dgvSanPham.Rows[index].Cells[1].Value.ToString().Trim();
           //txtdongiagocan.Text = dgvSanPham.Rows[index].Cells[4].Value.ToString().Trim();
           dongiatinh = int.Parse(dgvSanPham.Rows[index].Cells[4].Value.ToString().Trim());
            loi = float.Parse(dgvSanPham.Rows[index].Cells[6].Value.ToString().Trim());
            float tinh = dongiatinh + (dongiatinh * loi);
            txtDonGia.Text = tinh.ToString();
            txtdongiagocan.Text = dgvSanPham.Rows[index].Cells[9].Value.ToString();
            try
            {
                if (txtdongiagocan.Text != " " && txtdongiagocan.Text != "" && txtdongiagocan.Text != null)
                {
                    string url = paths + "\\img\\" + txtdongiagocan.Text;
                    picHinh.Image = Image.FromFile(url);
                    FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
                    picHinh.Image = System.Drawing.Image.FromStream(fs);
                    fs.Close();
                }
                else
                {
                    picHinh.Image = Image.FromFile(paths + "\\img\\no-image-available-icon-6.jpg");
                }
            }
            catch
            {
                picHinh.Image = Image.FromFile(paths + "\\img\\no-image-available-icon-6.jpg");
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaHoaDon.Text == "" || txtNhanVien.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    dgvHoaDon.Enabled = true;
                    btnXoa.Enabled = true;
                    btnLamMoi.Enabled = true;
                    btnThanhToan.Enabled = true;
                    int soluongtrukhimua = 1;
                    int soluong = 1;
                    int tong = 0;
                    int tongThanhTien = 0;
                    string masanpham = txtMaSanPham.Text.Trim();
                    string tensanpham = txtTenSanPham.Text.Trim();
                    int dongiagoc = int.Parse(txtDonGia.Text.Trim());
                    double dongia = double.Parse(txtDonGia.Text.Trim());
                    string mahoadon = txtMaHoaDon.Text.Trim();
                    string tenkhachhang = "";
                    if (checkBox1.Checked == true)
                    {
                        string makh = cbKhachHang.SelectedValue.ToString().Trim();
                        string mk = "SELECT * FROM KHACHHANG WHERE MAKHACHHANG = '" + makh + "'";
                        SqlDataReader drmk = conn.getDataReader(mk);
                        while (drmk.Read())
                        {
                            tenkhachhang = drmk["TENKHACHHANG"].ToString();
                        }
                        drmk.Close();
                        conn.closeConnection();

                    }
                    else
                    {
                        if (txtMaHoaDon.Text == "" || txtNhanVien.Text == ""||txtTenKhachHang.Text == "")
                        {
                            MessageBox.Show("Không được để trống");
                            return;
                        }
                        else
                        {
                            tenkhachhang = txtTenKhachHang.Text.Trim();
                        }
                    }
                   
                    string tennhanvien = txtNhanVien.Text.Trim();
                    string ngaycapnhat = DateTime.ParseExact(txtNgayLap.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
                   
                    double thanhtien = dongia * soluong;




                    string strSql1 = "select count(*) from CHITIETHOADON where MAHD = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";
                    int dem1 = conn.getCount(strSql1);
                    if (dem1 > 0)
                    {
                        //string soluongthaydoi = "SELECT * FROM CHITIETHOADON  where MAHD = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";

                        //SqlDataReader drsl = conn.getDataReader(soluongthaydoi);
                        //while (drsl.Read())
                        //{
                        //    txtSoLuong.Text = drsl["SOLUONG"].ToString();
                        //}
                        //drsl.Close();
                        //conn.closeConnection();
                        ////----------------------------------------------------------------------------------
                        //int soluongTang = int.Parse(txtSoLuong.Text.Trim());
                        //soluongTang++;
                        //double thanhtientang = dongia * soluongTang;


                        //string strSQLSL = "UPDATE CHITIETHOADON SET SOLUONG = " + soluongTang + ", THANHTIEN = " + thanhtientang + " WHERE MASANPHAM = '" + masanpham + "' AND MAHD = '" + mahoadon + "'";
                        //conn.updateTODB(strSQLSL);
                        ////----------------------------------------------------------------------------------
                        ////----------------------------------------------------------------------------------
                       
                      

                        ////----------------------------------------------------------------------------------
                        //string tongthanhtien0 = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";

                        //SqlDataReader thanhtiendr = conn.getDataReader(tongthanhtien0);
                        //while (thanhtiendr.Read())
                        //{
                        //    tongThanhTien += int.Parse(thanhtiendr["THANHTIEN"].ToString());
                        //}
                        //thanhtiendr.Close();
                        //conn.closeConnection();
                        //lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";


                        ////----------------------------------------------------------------------------------
                        //string soluongiohang2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                        //SqlDataReader drslgio2 = conn.getDataReader(soluongiohang2);
                        //while (drslgio2.Read())
                        //{
                        //    tong += int.Parse(drslgio2["SOLUONG"].ToString());
                        //}
                        //drslgio2.Close();
                        //conn.closeConnection();

                       
                        //label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";

                       
                        ////----------------------------------------------------------------------------------

                        //string loadLaiDuLieu2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                        //dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu2);
                        MessageBox.Show("sản phẩm đã có trong giỏ hàng của bạn");
                        return;
                    }
                    //----------------------------------------------------------------------------------
                    string strSql = "select count(*) from CHITIETHOADON where MAHD = '" + mahoadon + "'";
                    int dem = conn.getCount(strSql);
                    if (dem > 0)
                    {
                        int sltra1 = 0;
                        string trasl1 = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + txtMaSanPham.Text + "'";
                        SqlDataReader tr1 = conn.getDataReader(trasl1);
                        while (tr1.Read())
                        {
                            sltra1 = int.Parse(tr1["SOLUONGSP"].ToString());
                        }
                        tr1.Close();
                        conn.closeConnection();
                        if (sltra1 == 0)
                        {
                            MessageBox.Show("hết hàng");
                            return;
                        }

                        string strSQLCHITIETHOADON1 = "INSERT CHITIETHOADON (MAHD,MASANPHAM,SOLUONG,THANHTIEN,GIAGOC) VALUES('" + mahoadon + "', '" + masanpham + "'," + soluong + ", " + dongia + "," + dongiagoc + ")";
                        conn.updateTODB(strSQLCHITIETHOADON1);
                        string soluongiohang1 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";


                        SqlDataReader drslgio1 = conn.getDataReader(soluongiohang1);
                        while (drslgio1.Read())
                        {
                            tong += int.Parse(drslgio1["SOLUONG"].ToString());
                        }
                        drslgio1.Close();
                        conn.closeConnection();


                        label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";
                        string loadLaiDuLieu1 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                        dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu1);

                        //----------------------------------------------------------------------------------
                        string tongthanhtien1 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";

                            
                        SqlDataReader thanhtiendr1 = conn.getDataReader(tongthanhtien1);
                        while (thanhtiendr1.Read())
                        {
                            tongThanhTien += int.Parse(thanhtiendr1["THANHTIEN"].ToString());
                        }
                        thanhtiendr1.Close();
                        conn.closeConnection();




                        
                        int tru1khiclickthem1 = sltra1 - soluongtrukhimua;
                        string strSQL11 = "UPDATE SANPHAM SET SOLUONGSP = " + tru1khiclickthem1 + " WHERE MASANPHAM = '" + masanpham + "'";
                        conn.updateTODB(strSQL11);






                        lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";
                        return;
                    }
                    int sltra = 0;
                    string trasl = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + txtMaSanPham.Text + "'";
                    SqlDataReader tr = conn.getDataReader(trasl);
                    while (tr.Read())
                    {
                        sltra = int.Parse(tr["SOLUONGSP"].ToString());
                    }
                    tr.Close();
                    conn.closeConnection();

                    if (sltra == 0)
                    {
                        MessageBox.Show("hết hàng");
                        return;
                    }
                    string strSQLkhach = "INSERT KHACHHANG (TENKHACHHANG,SDT,GIOITINH,NGAYSINH,DIACHI) VALUES (N'" + tenkhachhang + "',NULL, NULL, NULL, NULL)";
                    conn.updateTODB(strSQLkhach);
                    string makhachhangsql = "SELECT * FROM KHACHHANG WHERE TENKHACHHANG = N'" + tenkhachhang + "'";
                   

                    SqlDataReader dr = conn.getDataReader(makhachhangsql);
                    while (dr.Read())
                    {
                        txtTenKhachHangAn.Text = dr["MAKHACHHANG"].ToString();
                    }
                    dr.Close();
                    conn.closeConnection();
                    string makhachhang = txtTenKhachHangAn.Text.Trim();

                   
                    //strSQL = "INSERT HOADON VALUES('" + mahoadon + "','" + ngaycapnhat + "','" + 15 + "','" + 2 + "','" + dongia + "','" + dongia + "')";
                    string strSQL = "INSERT HOADON (MAHD,NGAYLAP,MAKHACHHANG,ID,TONGTIEN) VALUES('" + mahoadon + "', '" + ngaycapnhat + "'," + makhachhang + ", '" + maNV + "', NULL)";

                    conn.updateTODB(strSQL);

                   layten.LuuThongTinThanhTien(txtMaHoaDon.Text);
                   string strSQLCHITIETHOADON = "INSERT CHITIETHOADON (MAHD,MASANPHAM,SOLUONG,THANHTIEN,GIAGOC) VALUES('" + mahoadon + "', '" + masanpham + "'," + soluong + ", " + thanhtien + "," + dongiagoc + ")";
                    conn.updateTODB(strSQLCHITIETHOADON);

                    string soluongiohang = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";


                    SqlDataReader drslgio = conn.getDataReader(soluongiohang);
                    while (drslgio.Read())
                    {
                        tong += int.Parse(drslgio["SOLUONG"].ToString());
                    }
                    drslgio.Close();
                    conn.closeConnection();
                     
                    
                    label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";



                    string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";


                    SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                    while (thanhtiendr2.Read())
                    {
                        tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                    }
                    thanhtiendr2.Close();
                    conn.closeConnection();
                    lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";
                   
                    int tru1khiclickthem = sltra - soluongtrukhimua;
                    strSQL = "UPDATE SANPHAM SET SOLUONGSP = " + tru1khiclickthem + " WHERE MASANPHAM = '" + masanpham + "'";
                    conn.updateTODB(strSQL);
                    string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                    dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
                }

            }
            catch
            {
                MessageBox.Show("thất bại");
            }
           
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                int Numrd;  
                
                Random rd = new Random();
                Numrd = rd.Next(1, 100);//biến Numrd sẽ nhận có giá trị ngẫu nhiên trong khoảng 1 đến 100
                txtMaHoaDon.Text = "HD0" +  rd.Next(1000, 99999).ToString();
                txtMaHoaDon.Enabled = false;
                checkBox2.Enabled = false;
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaHoaDon.Enabled = true;
            checkBox2.Enabled = true;
            checkBox2.Checked = false;
            cbKhachHang.Visible = false;
            string mahoadon = txtMaHoaDon.Text.Trim();
            string strSQL = "DELETE CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
            conn.updateTODB(strSQL);
            MessageBox.Show("Làm thành công nha ^^");
            string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
            txtMaHoaDon.Text = "";
            txtTenKhachHang.Text = "";
            txtMaSanPham.Text = "";
            txtTenSanPham.Text = "";
            txtDonGia.Text = "";
            lbTongTien.Text = "0 VNĐ";
            label11.Text = "Tổng số lượng trong giỏ hàng: 0 món";
            txtSoLuong.Text = "";
        }

        private void dgvHoaDon_DataMemberChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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

        private void btnThemSoLuong_Click(object sender, EventArgs e)
        {
            int tong = 0;
            int slsp = 0;
            string masanpham = txtMaSanPhamAn.Text.Trim();
            string mahoadon = txtMaHoaDonAn.Text.Trim();
            string mahoadondanghien = txtMaHoaDon.Text.Trim();
            int sl = int.Parse(txtSoLuongSanPhamChon.Text.Trim());
            int dongiagoc = 0;
            int tongThanhTien = 0;
            string dongia = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader dongiadr = conn.getDataReader(dongia);
            while (dongiadr.Read())
            {
                dongiagoc = int.Parse(dongiadr["DONGIA"].ToString());
            }
            dongiadr.Close();
            conn.closeConnection();
            int sltronggio = 0;
            string sltra = "SELECT * FROM CHITIETHOADON WHERE MASANPHAM = '" + masanpham + "' AND  MAHD = '" + mahoadon + "'";
            SqlDataReader tr12 = conn.getDataReader(sltra);
            while (tr12.Read())
            {
                sltronggio = int.Parse(tr12["SOLUONG"].ToString());
            }
            tr12.Close();
            conn.closeConnection();

            string trado = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader tr = conn.getDataReader(trado);
            while (tr.Read())
            {
                slsp = int.Parse(tr["SOLUONGSP"].ToString());
            }
            tr.Close();
            conn.closeConnection();
            int soluonghoantra = slsp + sltronggio;
            string strSQL = "UPDATE SANPHAM SET SOLUONGSP = " + soluonghoantra + " WHERE MASANPHAM = '" + masanpham + "'";
            conn.updateTODB(strSQL);
            int slsp2 = 0;
            string trado2 = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader tr3 = conn.getDataReader(trado2);
            while (tr3.Read())
            {
                slsp2 = int.Parse(tr3["SOLUONGSP"].ToString());
            }
            tr3.Close();
            conn.closeConnection();
            int slupdate = slsp2 - sl;
            
            if (slupdate < 0)
            {
               
                MessageBox.Show("Số lượng trong kho không đủ bạn ơi");
                strSQL = "UPDATE SANPHAM SET SOLUONGSP = " + slsp + " WHERE MASANPHAM = '" + masanpham + "'";
                conn.updateTODB(strSQL);

                return;
            }
             strSQL = "UPDATE SANPHAM SET SOLUONGSP = " + slupdate + " WHERE MASANPHAM = '" + masanpham + "'";
            conn.updateTODB(strSQL);

            float ln = 0;

            string loinhuan = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader drln = conn.getDataReader(loinhuan);
            while (drln.Read())
            {
                ln = float.Parse(drln["LOINHUAN"].ToString());
            }
            drln.Close();
            conn.closeConnection();

            double thanhtien = ((dongiagoc * ln) + dongiagoc) * sl;
            string soluongiohang = "UPDATE CHITIETHOADON SET SOLUONG = " + sl + ", THANHTIEN = "+thanhtien+" WHERE MAHD = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";
            
            conn.updateTODB(soluongiohang);
            string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
            SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
            while (thanhtiendr2.Read())
            {
                tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
            }
            thanhtiendr2.Close();
            conn.closeConnection();
            lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";





            string soluongiohangThayDoi = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";


            SqlDataReader drslgio = conn.getDataReader(soluongiohangThayDoi);
            while (drslgio.Read())
            {
                tong += int.Parse(drslgio["SOLUONG"].ToString());
            }
            drslgio.Close();
            conn.closeConnection();


            label11.Text = "Số lượng trong giỏ hàng: " + tong.ToString() + " món";




            string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadondanghien + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
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

        private void txtTenNhanVien_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int tongThanhTien = 0;
            int tong = 0;
            string mahoadon = txtMaHoaDon.Text.Trim();
            string masanpham = txtMaSanPhamAn.Text.Trim();
            int slsp = 0;
            int sltronggio = 0;
            string sltra = "SELECT * FROM CHITIETHOADON WHERE MASANPHAM = '" + masanpham + "' AND  MAHD = '" + mahoadon + "'";
            SqlDataReader tr12 = conn.getDataReader(sltra);
            while (tr12.Read())
            {
                sltronggio = int.Parse(tr12["SOLUONG"].ToString());
            }
            tr12.Close();
            conn.closeConnection();

            string trado = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + masanpham + "'";
            SqlDataReader tr = conn.getDataReader(trado);
            while (tr.Read())
            {
                slsp = int.Parse(tr["SOLUONGSP"].ToString());
            }
            tr.Close();
            conn.closeConnection();
            int soluonghoantra = slsp + sltronggio;
            string strSQL12 = "UPDATE SANPHAM SET SOLUONGSP = " + soluonghoantra + " WHERE MASANPHAM = '" + masanpham + "'";
            conn.updateTODB(strSQL12);

            string strSQL = "DELETE CHITIETHOADON WHERE MAHD = '" + mahoadon + "' AND MASANPHAM = '" + masanpham + "'";
            conn.updateTODB(strSQL);






          











            string soluongiohang = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";


            SqlDataReader drslgio = conn.getDataReader(soluongiohang);
            while (drslgio.Read())
            {
                tong += int.Parse(drslgio["SOLUONG"].ToString());
            }
            drslgio.Close();
            conn.closeConnection();


            label11.Text = "Tổng số lượng trong giỏ hàng: " + tong.ToString() + " cái";



            string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = N'" + mahoadon + "'";


            SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
            while (thanhtiendr2.Read())
            {
                tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
            }
            thanhtiendr2.Close();
            conn.closeConnection();
            lbTongTien.Text = String.Format("{0:0,0.0}", tongThanhTien) + " VNĐ";
            string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
            dgvHoaDon.DataSource = conn.LoadData(loadLaiDuLieu);
        }

        private void btnInHoaDon_Click(object sender, EventArgs e)
        {
            FrmHoaDon a = new FrmHoaDon();
            a.ShowDialog();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cbKhachHang.Visible = true;
                LoadcbKH();
            }
            else
            {
                cbKhachHang.Visible = false;
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void txtSoLuongSanPhamChon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                textKhuyenmai.Enabled = true;

            }
            else
            {
                textKhuyenmai.Enabled = false;
            }
        }
    }
}
