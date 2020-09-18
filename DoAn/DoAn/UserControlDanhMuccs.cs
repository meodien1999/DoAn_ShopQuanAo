using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class UserControlDanhMuccs : UserControl
    {
        public UserControlDanhMuccs()
        {
            InitializeComponent();
        }
        private void showUserControl(UserControl user)
        {
            pcenter.Controls.Add(user);
            user.Dock = DockStyle.Fill;
            user.BringToFront();
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            UserControlTheLoai us7 = new UserControlTheLoai();
            showUserControl(us7);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            UserControlThuongHieu us8 = new UserControlThuongHieu();
            showUserControl(us8);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            UserControlKhachHang us9 = new UserControlKhachHang();
            showUserControl(us9);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UserControlNhaCungCap us1 = new UserControlNhaCungCap();
            showUserControl(us1);
        }
    }
}
