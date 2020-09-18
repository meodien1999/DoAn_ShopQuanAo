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
     
    public partial class UserControlNhaCungCap : UserControl
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlNhaCungCap()
        {
            InitializeComponent();
        }
        public void createNCC()
        {
            string str = "select * from NHACUNGCAP";
            aa = conn.getDataAdapter(str, "NHACUNGCAP");
            primarykey[0] = conn.Dset.Tables["NHACUNGCAP"].Columns["MANCC"];
            conn.Dset.Tables["NHACUNGCAP"].PrimaryKey = primarykey;
        }
        private void UserControlNhaCungCap_Load(object sender, EventArgs e)
        {
            txtDiaChi.Enabled = true;
            this.dGVLoaiSP.DefaultCellStyle.ForeColor = Color.Black;
            createNCC();
            dGVLoaiSP.DataSource = conn.Dset.Tables["NHACUNGCAP"];
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMaNCC.Enabled = true;
           
            txtMaNCC.Text = "";
            txtTenNCC.Text = "";
            txtMaNCC.Focus();
            txtDiaChi.Text = "";
            txtSDT.Text = "";
        }
        private void loadLaiData()
        {
            string loadLaiDuLieu = "SELECT * FROM NHACUNGCAP";
            dGVLoaiSP.DataSource = conn.LoadData(loadLaiDuLieu);
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMaNCC.Text == "" || txtTenNCC.Text == "" || txtDiaChi.Text == "" || txtSDT.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    string ma = txtMaNCC.Text.Trim();
                    string ten = txtTenNCC.Text.Trim();
                    string dc = txtDiaChi.Text.Trim();
                    string sdt = txtSDT.Text.Trim();
                    string strSQL = "SELECT COUNT(*) FROM NHACUNGCAP WHERE MANCC = '" + ma + "'";
                    bool kq = conn.kiemTraTrung(strSQL);


                    if (kq == true)
                    {
                        MessageBox.Show("Đã tồn tại mã này: " + ma);
                        return;
                    }

                    strSQL = "INSERT NHACUNGCAP VALUES('" + ma + "',N'" + ten + "',N'" + dc + "'," + sdt + ")";

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
                string ma = txtMaNCC.Text.Trim();

                string strSQL = "SELECT COUNT(*) FROM NHACUNGCAP WHERE MANCC = '" + ma + "'";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã này: " + ma);
                    return;
                }
                strSQL = "DELETE NHACUNGCAP WHERE MANCC = '" + ma + "'";
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
                string ma = txtMaNCC.Text.Trim();
                string ten = txtTenNCC.Text.Trim();
                string dc = txtDiaChi.Text.Trim();
                string sdt = txtSDT.Text.Trim();

                string strSQL = "SELECT COUNT(*) FROM NHACUNGCAP WHERE MANCC = '" + ma + "'";
                bool kq = conn.kiemTraTrung(strSQL);
                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã này: " + ma);
                    return;
                }

                strSQL = "UPDATE NHACUNGCAP SET TENNCC = N'" + ten + "',DIACHI = N'" + dc + "',SODIENTHOAI = " + sdt + " WHERE MANCC = '" + ma + "'";
                conn.updateTODB(strSQL);
                loadLaiData();
                MessageBox.Show("Sửa thành công nha ^^");
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }

        private void dGVLoaiSP_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaNCC.Text = dGVLoaiSP.Rows[index].Cells[0].Value.ToString();
            txtTenNCC.Text = dGVLoaiSP.Rows[index].Cells[1].Value.ToString();

            txtDiaChi.Text = dGVLoaiSP.Rows[index].Cells[2].Value.ToString();
            txtSDT.Text= dGVLoaiSP.Rows[index].Cells[3].Value.ToString();
        }
    }
}
