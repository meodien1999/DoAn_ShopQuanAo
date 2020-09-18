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
    public partial class UserControlThuongHieu : UserControl
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlThuongHieu()
        {
            InitializeComponent();
        }
        public void createThuongHieu()
        {
            string str = "select * from THUONGHIEU";
            aa = conn.getDataAdapter(str, "THUONGHIEU");
            primarykey[0] = conn.Dset.Tables["THUONGHIEU"].Columns["MATHUONGHIEU"];
            conn.Dset.Tables["THUONGHIEU"].PrimaryKey = primarykey;
        }
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserControlThuongHieu_Load(object sender, EventArgs e)
        {
            this.dGVThuongHieu.DefaultCellStyle.ForeColor = Color.Black;
            createThuongHieu();
            dGVThuongHieu.DataSource = conn.Dset.Tables["THUONGHIEU"];
        }

        private void dGVThuongHieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMa.Text = dGVThuongHieu.Rows[index].Cells[0].Value.ToString();
            txtTen.Text = dGVThuongHieu.Rows[index].Cells[1].Value.ToString();
        }
        private void loadLaiData()
        {
            string loadLaiDuLieu = "SELECT * FROM THUONGHIEU";
            dGVThuongHieu.DataSource = conn.LoadData(loadLaiDuLieu);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            txtMa.Enabled = true;
            txtMa.Text = "";
            txtTen.Text = "";
            txtMa.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMa.Text == "" || txtTen.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    string mathuonghieu = txtMa.Text.Trim();
                    string tenthuonghieu = txtTen.Text.Trim();

                    string strSQL = "SELECT COUNT(*) FROM THUONGHIEU WHERE MATHUONGHIEU = '" + mathuonghieu + "'";
                    bool kq = conn.kiemTraTrung(strSQL);


                    if (kq == true)
                    {
                        MessageBox.Show("Đã tồn tại mã Thương hiệu này: " + mathuonghieu);
                        return;
                    }

                    strSQL = "INSERT THUONGHIEU VALUES('" + mathuonghieu + "',N'" + tenthuonghieu + "')";

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
                string ma = txtMa.Text.Trim();

                string strSQL = "SELECT COUNT(*) FROM THUONGHIEU WHERE MATHUONGHIEU = '" + ma + "'";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã thương hiệu này: " + ma);
                    return;
                }
                strSQL = "DELETE THUONGHIEU WHERE MATHUONGHIEU = '" + ma + "'";
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
                string ma = txtMa.Text.Trim();
                string ten = txtTen.Text.Trim();


                string strSQL = "SELECT COUNT(*) FROM THUONGHIEU WHERE MATHUONGHIEU = '" + ma + "'";
                bool kq = conn.kiemTraTrung(strSQL);
                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã thương hiệu này: " + ma);
                    return;
                }

                strSQL = "UPDATE THUONGHIEU SET TENTHUONGHIEU = N'" + ten + "' WHERE MATHUONGHIEU = '" + ma + "'";
                conn.updateTODB(strSQL);
                loadLaiData();
                MessageBox.Show("Sửa thành công nha ^^");
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
        }
    }
}
