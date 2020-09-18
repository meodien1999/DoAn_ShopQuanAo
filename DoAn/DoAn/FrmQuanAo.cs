using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThuVien;
namespace DoAn
{

    public partial class FrmQuanAo : Form
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        public bool isAdmin = false;
       
        private string tenNhanVien;
        private string maNhanVien;

        public string MaNhanVien
        {
            get { return maNhanVien; }
            set { maNhanVien = value; }
        }
        public string TenNhanVien
        {
            get { return tenNhanVien; }
            set { tenNhanVien = value; }
        }
        public Point mouseLocation;
        UserControlNhanVien us1;
        UserControlTrangChu us2;
        UserControlBanHang us3;
        UserControlSanPham us4;
        UserControlHoaDon us5;
        UserControlTheLoai us6;
        UserControlThuongHieu us7;
        UserControlKhachHang us8;
        UserControlHeThong us9;
        UserControlDanhMuccs us10;
        public FrmQuanAo()
        {
            InitializeComponent();
            button1_Click(null, null);
        }
        private void click()
        {
            btn1.BackColor = colorDialog1.Color;
            btn2.BackColor = colorDialog1.Color;
            btn3.BackColor = colorDialog1.Color;
            btn4.BackColor = colorDialog1.Color;
            btn5.BackColor = colorDialog1.Color;
            btn6.BackColor = colorDialog1.Color;
            btn7.BackColor = colorDialog1.Color;
            button1.BackColor = colorDialog1.Color;
            button3.BackColor = colorDialog1.Color;
        }

        private void showUserControl(UserControl user)
        {
            pCenter.Controls.Add(user);
            user.Dock = DockStyle.Fill;
            user.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            click();
            btn1.BackColor = Color.BlueViolet;
            us2 = new UserControlTrangChu();
            showUserControl(us2);
        }
        
        private void button2_Click(object sender, EventArgs e)
        {          
            click();
            btn2.BackColor = Color.BlueViolet;
            //  
            us1 = new UserControlNhanVien();
            showUserControl(us1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            click();
            btn3.BackColor = Color.BlueViolet;
            us3 = new UserControlBanHang();
            showUserControl(us3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            click();
            btn4.BackColor = Color.BlueViolet;

            us4 = new UserControlSanPham();
             showUserControl(us4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            click();
            btn5.BackColor = Color.BlueViolet;

            us5 = new UserControlHoaDon();
            showUserControl(us5);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            click();
            btn6.BackColor = Color.BlueViolet;
            UserControlNhapHang us6 = new UserControlNhapHang();
            showUserControl(us6);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }

        private void FrmAdmin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult r;
            //r = MessageBox.Show("Bạn có chắc muốn thoát ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            //if (r == DialogResult.No)
            //    e.Cancel = true;
        }
        public string ten = "";
        public void FrmAdmin_Load(object sender, EventArgs e)
        {
            
            FrmDangNhap flg = new FrmDangNhap();
            if (flg.ShowDialog() == DialogResult.OK)
            {

                if (flg.admin == true)
                {
                    TenNhanVien = layten.LayThongTinDangNhap();
                    MaNhanVien = layten.LayThongTinID();
                    isAdmin = true;
                    label2.Text = "Xin chào: " + TenNhanVien;
                }
                else
                {
                    TenNhanVien = layten.LayThongTinDangNhap();
                    MaNhanVien = layten.LayThongTinID();
                    isAdmin = false;
                    label2.Text = "Xin chào: " + TenNhanVien;
                }
            }
            else Application.Exit();

            btn2.Enabled = isAdmin;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnRestore.Visible = true;
            btnMaximizar.Visible = false;
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestore.Visible = false;
            btnMaximizar.Visible = true;
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pHeader_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void pHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mous = Control.MousePosition;
                mous.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mous;
            }
        }

        private void pCenter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblLogo_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void pMenuLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pRight_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            click();
            button3.BackColor = Color.BlueViolet;
            us9 = new UserControlHeThong();
            showUserControl(us9);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            click();
            btn7.BackColor = Color.BlueViolet;
            us10 = new UserControlDanhMuccs();
            showUserControl(us10);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
        
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
         
        }

        private void button1_Click_4(object sender, EventArgs e)
        {
            click();
            button1.BackColor = Color.BlueViolet;
            UserControlTraDo us16 = new UserControlTraDo();
            showUserControl(us16);
        }
    }
}
