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
    public partial class UserControlNhanVien : UserControl
    {

        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        public UserControlNhanVien()
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
        

        private void Load_Combobox_GioiTinh()
        {
            string sql = "SELECT * FROM GIOI";
            DataTable dt = conn.getDataTable(sql, "GIOI");
            cboGioiTinh.DataSource = dt;
            cboGioiTinh.DisplayMember = "GIOITINH";
            cboGioiTinh.ValueMember = "GIOITINH";

            cboGioiTinh.SelectedIndex = 0;
        }

        private void Load_Combobox_Quyen()
        {
            string sql = "SELECT * FROM QUYEN";
            DataTable dt = conn.getDataTable(sql, "QUYEN");
            cboQuyen.DataSource = dt;
            cboQuyen.DisplayMember = "ADMIN";
            cboQuyen.ValueMember = "ADMIN";

            cboQuyen.SelectedIndex = 0;
        }

        public bool ktNgaySinhHopLe()
        {
            if (txtNgaySinh.Text!="")
            {
                try
                {
                    DateTime dateOfBirth = DateTime.ParseExact(txtNgaySinh.Text, "dd/MM/yyyy", null);
                    int tuoi = DateTime.Now.Year - dateOfBirth.Year;
                    if (tuoi < 18)
                    {
                        MessageBox.Show("Tuổi của bạn là: " + tuoi + " bạn chưa đủ tuổi!");
                        txtNgaySinh.ResetText();
                        txtNgaySinh.Focus();
                        return false;
                    }
                }
                catch
                {
                    MessageBox.Show("ngày sinh chưa hợp lệ!");
                    txtNgaySinh.ResetText();
                    txtNgaySinh.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Nhập ngày sinh dd/MM/yyyy!");
                txtNgaySinh.ResetText();
                txtNgaySinh.Focus();
                return false;
            }
            return true;
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (txtMaNhanVien.Text == "" || txtTenNhanVien.Text == "" || txtTaiKhoan.Text == "" || txtMatKhau.Text == ""
               || txtEmail.Text == "" || txtDiaChi.Text == "" || txtDienThoai.Text == "")
                {
                    MessageBox.Show("Không được để trống");
                }
                else
                {
                    string manhanvien = txtMaNhanVien.Text.Trim();
                    string tenhanvien = txtTenNhanVien.Text.Trim();
                    string taikhoan = txtTaiKhoan.Text.Trim();
                    string matkhau = txtMatKhau.Text.Trim();
                    string gioitinh = cboGioiTinh.SelectedValue.ToString().Trim();
                    string email = txtEmail.Text.Trim();
                    string diachi = txtDiaChi.Text.Trim();
                    string tenhinh = txtTenHinh.Text.Trim();
                    string sdt = txtDienThoai.Text.Trim();
                    string quyen = cboQuyen.SelectedValue.ToString().Trim();
                    string ngaysinh = DateTime.ParseExact(txtNgaySinh.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
                    string strSQL = "SELECT COUNT(*) FROM NHANVIEN WHERE ID = '" + manhanvien + "'";
                    string strSQL2 = "SELECT COUNT(*) FROM NHANVIEN WHERE UserName = '" + taikhoan + "'";
                    bool kq = conn.kiemTraTrung(strSQL);
                    bool kq2 = conn.kiemTraTrung(strSQL2);
                    bool kq_tuoi = ktNgaySinhHopLe();
                    if (kq_tuoi == false)
                    {
                        return;
                    }
                    bool ktemail = conn.isEmail(email);
                    if (ktemail == false)
                    {
                        MessageBox.Show("Định dạng email sai vui lòng nhập lại");
                        return;
                    }
                    if (kq == true)
                    {
                        MessageBox.Show("Đã tồn tại mã nhân viên này: " + manhanvien);
                        return;
                    }
                    if (kq2 == true)
                    {
                        MessageBox.Show("Đã tồn tại tên tài khoản này: " + taikhoan);
                        return;
                    }
                    strSQL = "INSERT NHANVIEN VALUES('" + manhanvien + "',N'" + tenhanvien + "','" + taikhoan + "','" + matkhau + "',N'" + gioitinh + "','" + ngaysinh + "','" + email + "','" + diachi + "','" + sdt + "','" + tenhinh + "','" + quyen + "')";

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
        private void loadLaiData() 
        {
            string loadLaiDuLieu = "SELECT * FROM NHANVIEN";
            dGVNhanVien.DataSource = conn.LoadData(loadLaiDuLieu);
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string manhanvien = txtMaNhanVien.Text.Trim();
     
                string strSQL = "SELECT COUNT(*) FROM NHANVIEN WHERE ID = '" + manhanvien + "'";

                bool kq = conn.kiemTraTrung(strSQL);

                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã nhân viên này: " + manhanvien);
                    return;
                }
                strSQL = "DELETE NHANVIEN WHERE ID = '" + manhanvien + "'";
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
                string manhanvien = txtMaNhanVien.Text.Trim();
                string tenhanvien = txtTenNhanVien.Text.Trim();
                string taikhoan = txtTaiKhoan.Text.Trim();
                string matkhau = txtMatKhau.Text.Trim();
                string gioitinh = cboGioiTinh.SelectedValue.ToString().Trim();
                string email = txtEmail.Text.Trim();
                string diachi = txtDiaChi.Text.Trim();
                string tenhinh = txtTenHinh.Text.Trim();
                string sdt = txtDienThoai.Text.Trim();
                string quyen = cboQuyen.SelectedValue.ToString().Trim();
                string ngaysinh = DateTime.ParseExact(txtNgaySinh.Text, "dd/MM/yyyy", null).ToString("yyyy/MM/dd");
                string strSQL = "SELECT COUNT(*) FROM NHANVIEN WHERE ID = '" + manhanvien + "'";
                bool kq = conn.kiemTraTrung(strSQL);
                if (kq == false)
                {
                    MessageBox.Show("Không tồn tại mã nhân viên này: " + manhanvien);
                    return;
                }

                strSQL = "UPDATE NHANVIEN SET HOTEN = N'" + tenhanvien + "', UserName = '" + taikhoan + "',PassWord = '" + matkhau + "',GIOITINH = N'" + gioitinh + "',NGAYSINH = '" + ngaysinh + "',EMAIL = '" + email + "',DIACHI = '" + diachi + "',SODIENTHOAI = '" + sdt + "',Image = '" + tenhinh + "', ADMIN = '" + quyen + "' WHERE ID = '" + manhanvien + "'";
                conn.updateTODB(strSQL);
                loadLaiData();
                MessageBox.Show("Sửa thành công nha ^^");
            }
            catch
            {
                MessageBox.Show("thất bại");
            }
           
        }
        
        string paths = Application.StartupPath.Substring(0, Application.StartupPath.Length - 10);
        private void btnUpHinh_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.InitialDirectory = "C:\\";
            open.Filter = "Image File (*.jpg)|*.jpg|All File (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string name = System.IO.Path.GetFileName(open.FileName);
                string luu = paths + "\\img\\" + name;
                try
                {
                    FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read);
                    System.IO.File.Copy(open.FileName, luu);

                    MessageBox.Show("Upload file ảnh thành công", "Thông báo");
                    txtTenHinh.Text = name;
                    //picHinh.Image = Image.FromFile(luu);
                    picHinh.Image = System.Drawing.Image.FromStream(fs);
                    //  picHinh.ImageLocation = open.FileName;
                    fs.Close();

                }
                catch
                {
                    MessageBox.Show("Hình ảnh đã tồn tại hoặc trùng tên, vui lòng kiểm tra lại");
                }
            }
        }

        private void loadDuLieu()
        {
            dGVNhanVien.DataSource = conn.Dset.Tables["NHANVIEN"];
        }

        private void UserControlNhanVien_Load(object sender, EventArgs e)
        {
         
            this.dGVNhanVien.DefaultCellStyle.ForeColor = Color.Black;
            createNhanVien();
            Load_Combobox_GioiTinh();
            Load_Combobox_Quyen();
           loadDuLieu();
        }

        private void dGVNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            txtMaNhanVien.Text = dGVNhanVien.Rows[index].Cells[0].Value.ToString();
            txtTenNhanVien.Text = dGVNhanVien.Rows[index].Cells[1].Value.ToString();
            txtTaiKhoan.Text = dGVNhanVien.Rows[index].Cells[2].Value.ToString();
            txtMatKhau.Text = dGVNhanVien.Rows[index].Cells[3].Value.ToString();
            cboGioiTinh.Text = dGVNhanVien.Rows[index].Cells[4].Value.ToString();
            txtNgaySinh.Text = dGVNhanVien.Rows[index].Cells[5].Value.ToString();
            txtEmail.Text = dGVNhanVien.Rows[index].Cells[6].Value.ToString();
            txtDiaChi.Text = dGVNhanVien.Rows[index].Cells[7].Value.ToString();
            txtDienThoai.Text = dGVNhanVien.Rows[index].Cells[8].Value.ToString();
            txtTenHinh.Text = dGVNhanVien.Rows[index].Cells[9].Value.ToString();
            cboQuyen.Text = dGVNhanVien.Rows[index].Cells[10].Value.ToString();
            try
            {
                if (txtTenHinh.Text != " " && txtTenHinh.Text != "" && txtTenHinh.Text != null)
                {
                    string url = paths + "\\img\\" + txtTenHinh.Text;
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
            //if (txtTenHinh.Text != " " && txtTenHinh.Text != "" && txtTenHinh.Text != null)
            //{
            //    string url = paths + "\\img\\" + txtTenHinh.Text;
            //    picHinh.Image = Image.FromFile(url);
            //    FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read);
            //    picHinh.Image = System.Drawing.Image.FromStream(fs);
            //    fs.Close();
            //}
            //else picHinh.Image = Image.FromFile(paths + "\\img\\48976731_1434489996692906_20412982781187794432_n.jpg");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (conn.searchNV(txtTimKiem.Text) != null)
            {
                dGVNhanVien.DataSource = conn.searchNV(txtTimKiem.Text);

            }
            else MessageBox.Show("Không tìm thấy nhân viên");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            loadDuLieu();
        }

        private void btnThem_Click_1(object sender, EventArgs e)
        {
            txtMaNhanVien.Enabled = true;
            txtMaNhanVien.Text = "";
            txtTenNhanVien.Text = "";
            txtTaiKhoan.Text = "";
            txtMatKhau.Text = "";
            txtNgaySinh.Text = "";
            txtEmail.Text = "";
            txtDiaChi.Text = "";
            txtDienThoai.Text = "";
            txtTenHinh.Text = "";
            txtMaNhanVien.Focus();

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtMaNhanVien_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
