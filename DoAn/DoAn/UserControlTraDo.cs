using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien;
using System.IO;

namespace DoAn
{
    public partial class UserControlTraDo : UserControl
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        DataColumn[] primarykey1 = new DataColumn[1];
        DataColumn[] primarykey2 = new DataColumn[2];
        DataColumn[] primarykey3 = new DataColumn[1];
        public UserControlTraDo()
        {
            InitializeComponent();
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
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void UserControlTraDo_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
           
            createHoaDon();
            createCTHoaDon();
            dataGridView1.DataSource = conn.Dset.Tables["HOADON"];
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            string a = dataGridView1.Rows[index].Cells[0].Value.ToString().Trim();
            dataGridView2.DataSource = conn.layct(a);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                string mahoadon = txtMaHoadon.Text.Trim();
                string masanpham = txtMaSanPham.Text.Trim();
                string strSQL = "SELECT COUNT(*) FROM CHITIETHOADON WHERE MASANPHAM = '" + masanpham + "'  AND MAHD = '" + mahoadon + "' ";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại ");
                    return;
                }
                int sl = 0;
                int sl1 = 0;
                string trado = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + txtMaSanPham.Text + "'";
                SqlDataReader tr = conn.getDataReader(trado);
                while (tr.Read())
                {
                    sl = int.Parse(tr["SOLUONGSP"].ToString());
                }
                tr.Close();
                conn.closeConnection();
                string trados2 = "SELECT * FROM CHITIETHOADON WHERE MASANPHAM = '" + masanpham + "'  AND MAHD = '" + mahoadon + "' ";
                SqlDataReader tr1 = conn.getDataReader(trados2);
                while (tr1.Read())
                {
                    sl1 = int.Parse(tr1["SOLUONG"].ToString());
                }
                tr1.Close();
                conn.closeConnection();
                int slNhap =int.Parse(txtSL.Text.Trim());
                if(slNhap>sl1)
                {
                    MessageBox.Show("số lượng bạn nhập đã vượt quá số lượng hàng mua");
                    return;
                }
                int sl2 = sl1 - slNhap;
                int slhoantra = sl + slNhap;
                int dongiagoc = 0;
                string dongia = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                SqlDataReader a = conn.getDataReader(dongia);
                while (a.Read())
                {
                    dongiagoc = int.Parse(a["THANHTIEN"].ToString());
                }
                a.Close();
                conn.closeConnection();
                strSQL = "UPDATE SANPHAM SET SOLUONGSP = " + slhoantra + " WHERE MASANPHAM = '" + masanpham + "'";
                conn.updateTODB(strSQL);
                if (sl2 == 0)
                {
                    strSQL = "DELETE CHITIETHOADON WHERE MASANPHAM = '" + masanpham + "' AND MAHD = '" + mahoadon + "' ";
                    conn.updateTODB(strSQL);
                }
                int thanhtienupdate = dongiagoc * sl2;
                strSQL = "UPDATE CHITIETHOADON SET SOLUONG = " + sl2 + ", THANHTIEN = " + thanhtienupdate + " WHERE MASANPHAM = '" + masanpham + "' AND MAHD = '" + mahoadon + "' ";
                conn.updateTODB(strSQL);
                MessageBox.Show("Trả đồ thành công nha ^^ ");

                int tongThanhTien = 0;
                string tongthanhtien2 = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                SqlDataReader thanhtiendr2 = conn.getDataReader(tongthanhtien2);
                while (thanhtiendr2.Read())
                {
                    tongThanhTien += int.Parse(thanhtiendr2["THANHTIEN"].ToString());
                }
                thanhtiendr2.Close();
                conn.closeConnection();
                string updateTONGTIEN = "UPDATE HOADON SET TONGTIEN = " + tongThanhTien + " WHERE MAHD = '" + mahoadon + "'";

                conn.updateTODB(updateTONGTIEN);

                string loadLaiDuLieu1 = "SELECT * FROM HOADON ";
                dataGridView1.DataSource = conn.LoadData(loadLaiDuLieu1);
                string loadLaiDuLieu = "SELECT * FROM CHITIETHOADON WHERE MAHD = '" + mahoadon + "'";
                dataGridView2.DataSource = conn.LoadData(loadLaiDuLieu);

              

            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string ten = "";
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaHoadon.Text = dataGridView2.Rows[index].Cells[0].Value.ToString().Trim();
           txtMaSanPham.Text = dataGridView2.Rows[index].Cells[1].Value.ToString().Trim();
           txtSL.Text = dataGridView2.Rows[index].Cells[2].Value.ToString().Trim();
           string trado = "SELECT * FROM SANPHAM WHERE MASANPHAM = '" + txtMaSanPham.Text + "'";
           SqlDataReader tr = conn.getDataReader(trado);
           while (tr.Read())
           {
               ten = tr["TENSANPHAM"].ToString();
           }
           tr.Close();
           conn.closeConnection();
           txtTenSanPham.Text = ten.ToString();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (conn.searchPhieuNhap(txtTimKiem.Text) != null)
            {
                dataGridView1.DataSource = conn.searchHoaDon(txtTimKiem.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void BtnTroLai_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = conn.Dset.Tables["HOADON"];
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtSL_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
