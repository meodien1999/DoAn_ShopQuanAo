using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ThuVien; 

namespace DoAn
{
    public partial class FrmDangNhap : Form
    {
        public bool admin = false;
        public string tenNhanVien;
        public string maNhanVien;
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        public FrmDangNhap()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult q = MessageBox.Show("Bạn Có Muốn Thoát Không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (q.Equals(DialogResult.Yes))
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            this.Cursor = Cursors.WaitCursor;
            if (username != "")
            {

                if ((conn.CompareValue("NHANVIEN", "UserName", username) == true) && (conn.CompareValue("NHANVIEN", "PassWord", password) == true))
                {
                    if (conn.isAdmin("NHANVIEN", "UserName", "PassWord", "ADMIN", username, password) == true)
                    {
                        string tennhanviendangnhap = "SELECT * FROM NHANVIEN WHERE UserName = '" + username + "'";


                        SqlDataReader dr = conn.getDataReader(tennhanviendangnhap);
                        while (dr.Read())
                        {
                            tenNhanVien = dr["HOTEN"].ToString().Trim();
                            maNhanVien = dr["ID"].ToString().Trim();
                        }
                        dr.Close();
                        conn.closeConnection();

                        layten.LuuThongTinDangNhap(tenNhanVien);
                        layten.LuuThongTinID(maNhanVien);
                        admin = true;
                    }
                    else
                    {

                        string tennhanviendangnhap = "SELECT * FROM NHANVIEN WHERE UserName = '" + username + "'";


                        SqlDataReader dr = conn.getDataReader(tennhanviendangnhap);
                        while (dr.Read())
                        {
                            tenNhanVien = dr["HOTEN"].ToString().Trim();
                            maNhanVien = dr["ID"].ToString().Trim();
                        }
                        dr.Close();
                        conn.closeConnection();
                        layten.LuuThongTinDangNhap(tenNhanVien);
                        layten.LuuThongTinID(maNhanVien);
                        admin = false;
                    }
                    this.DialogResult = DialogResult.OK;
                   

                }
                else
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác! Vui lòng đăng nhập lại!");
            }
            this.Cursor = Cursors.Default;
        }

        private void FrmDangNhap_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) btnLogin_Click(sender, e);
        }
    }
}
