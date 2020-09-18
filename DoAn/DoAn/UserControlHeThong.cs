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
    public partial class UserControlHeThong : UserControl
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlHeThong()
        {
            InitializeComponent();
        }
        public void createNhanVien()
        {
            string str = "select * from NHANVIEN";
            aa = conn.getDataAdapter(str, "NHANVIEN");
            primarykey[0] = conn.Dset.Tables["NHANVIEN"].Columns["ID"];
            conn.Dset.Tables["NHANVIEN"].PrimaryKey = primarykey;
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
        string tendangnhap = "";
        string matkhau = "";
        string idnhanvien = layten.LayThongTinID();
        private void UserControlHeThong_Load(object sender, EventArgs e)
        {
            string tennv = "";
            string gioitinh = "";
            string ngaysinh = "";
            string email = "";
            string diachi = "";
            string sdt = "";
            string hinh = "";
            createNhanVien();
            string nhanvienthongtin = "SELECT * FROM NHANVIEN WHERE ID = '" + idnhanvien + "'";


            SqlDataReader dr = conn.getDataReader(nhanvienthongtin);
            while (dr.Read())
            {
                tendangnhap = dr["UserName"].ToString();
                matkhau = dr["PassWord"].ToString();
                tennv = dr["HOTEN"].ToString();
                gioitinh = dr["GIOITINH"].ToString();
                ngaysinh = dr["NGAYSINH"].ToString();
                email = dr["EMAIL"].ToString();
                diachi = dr["DIACHI"].ToString();
                sdt = dr["SODIENTHOAI"].ToString();
                hinh = dr["Image"].ToString();
            }
            dr.Close();
            conn.closeConnection();
            txtTenDN.Text = tendangnhap;
            txtMaNhanVien.Text = idnhanvien;
            txtTenNhanVien.Text = tennv;
            txtGioiTinh.Text = gioitinh;
            txtNgaySinh.Text = ngaysinh;
            txtDiaChi.Text = diachi;
            txtDienThoai.Text = sdt;
            txtEmail.Text = email;
            if (hinh == "")
            {

                picHinh.Image = Image.FromFile(paths + "\\img\\no-image-available-icon-6.jpg");

            }
            else
            {
                picHinh.Image = Image.FromFile(paths + "\\img\\" + hinh);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (txtMKCu.Text == matkhau)
            {
                if (txtMKMoi.Text.Equals(txtXacNhanMK.Text) == true)
                {
                    try
                    {
                        string strSQL = "UPDATE NHANVIEN SET PassWord = '" + txtXacNhanMK.Text + "' WHERE ID = '" + idnhanvien + "'";
                        conn.updateTODB(strSQL);

                        MessageBox.Show("Đổi mật khẩu thành công nha ^^");
                    }
                    catch
                    {
                        MessageBox.Show("thất bại");
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu nhập lại không chính xác vui lòng nhập lại");
                }

            }
            else
            {
                MessageBox.Show("Mật khẩu không chính xác vui lòng nhập lại");
            }
        }
    }
}
