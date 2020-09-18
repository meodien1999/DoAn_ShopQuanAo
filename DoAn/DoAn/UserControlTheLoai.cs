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
    public partial class UserControlTheLoai : UserControl
    {

        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlTheLoai()
        {
            InitializeComponent();
        }
        public void createLoai()
        {
            string str = "select * from LOAISANPHAM";
            aa = conn.getDataAdapter(str, "LOAISANPHAM");
            primarykey[0] = conn.Dset.Tables["LOAISANPHAM"].Columns["MALOAI"];
            conn.Dset.Tables["LOAISANPHAM"].PrimaryKey = primarykey;
        }
        private void UserControlTheLoai_Load(object sender, EventArgs e)
        {
            this.dGVLoaiSP.DefaultCellStyle.ForeColor = Color.Black;
            createLoai();
            dGVLoaiSP.DataSource = conn.Dset.Tables["LOAISANPHAM"];
        }

        private void dGVLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaSLoai.Text = dGVLoaiSP.Rows[index].Cells[0].Value.ToString();
            txtTenLoai.Text = dGVLoaiSP.Rows[index].Cells[1].Value.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaSLoai.Enabled = true;
            txtMaSLoai.Text = "";
            txtTenLoai.Text = "";
            txtMaSLoai.Focus();
        }
        private void loadLaiData()
        {
            string loadLaiDuLieu = "SELECT * FROM LOAISANPHAM";
            dGVLoaiSP.DataSource = conn.LoadData(loadLaiDuLieu);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaSLoai.Text == "" || txtTenLoai.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    string maloai = txtMaSLoai.Text.Trim();
                    string tenloai = txtTenLoai.Text.Trim();

                    string strSQL = "SELECT COUNT(*) FROM LOAISANPHAM WHERE MALOAI = '" + maloai + "'";
                    bool kq = conn.kiemTraTrung(strSQL);


                    if (kq == true)
                    {
                        MessageBox.Show("Đã tồn tại mã sản loại này: " + maloai);
                        return;
                    }

                    strSQL = "INSERT LOAISANPHAM VALUES('" + maloai + "',N'" + tenloai + "')";

                    conn.updateTODB(strSQL);
                    loadLaiData();

                    MessageBox.Show("Thêm thành công nha ^^");
                }

            }
            catch
            {
                MessageBox.Show("thất bại");
            }
           
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string maloai = txtMaSLoai.Text.Trim();

                string strSQL = "SELECT COUNT(*) FROM LOAISANPHAM WHERE MALOAI = '" + maloai + "'";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã Loại này: " + maloai);
                    return;
                }
                strSQL = "DELETE LOAISANPHAM WHERE MALOAI = '" + maloai + "'";
                conn.updateTODB(strSQL);
                MessageBox.Show("Xóa thành công nha ^^");
                loadLaiData();
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maloai = txtMaSLoai.Text.Trim();
                string tenloai = txtTenLoai.Text.Trim();


                string strSQL = "SELECT COUNT(*) FROM LOAISANPHAM WHERE MALOAI = '" + maloai + "'";
                bool kq = conn.kiemTraTrung(strSQL);
                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã loại này: " + maloai);
                    return;
                }

                strSQL = "UPDATE LOAISANPHAM SET TENLOAI = N'" + tenloai + "' WHERE MALOAI = '" + maloai + "'";
                conn.updateTODB(strSQL);
                loadLaiData();
                MessageBox.Show("Sửa thành công nha ^^");
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
