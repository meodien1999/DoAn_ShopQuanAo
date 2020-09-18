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
    public partial class UserControlHoaDon : UserControl
    {
        DBconnect conn = new DBconnect("QL_SHOPTHOITRANG");
        SqlDataAdapter aa = new SqlDataAdapter();
        DataColumn[] primarykey = new DataColumn[1];
        DataColumn[] primarykey1 = new DataColumn[1];
        DataColumn[] primarykey2 = new DataColumn[2];
        DataColumn[] primarykey3 = new DataColumn[1];
        public UserControlHoaDon()
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
        public void createPhieuNhap()
        {
            string str = "select * from PHIEUNHAP";
            aa = conn.getDataAdapter(str, "PHIEUNHAP");
            primarykey1[0] = conn.Dset.Tables["PHIEUNHAP"].Columns["MAPN"];
            conn.Dset.Tables["PHIEUNHAP"].PrimaryKey = primarykey1;
        }
        public void createCTPhieuNhap()
        {
            string str = "select * from CHITIETPHIEUNHAP";
            aa = conn.getDataAdapter(str, "CHITIETPHIEUNHAP");
            primarykey2[0] = conn.Dset.Tables["CHITIETPHIEUNHAP"].Columns["MAPN"];
            primarykey2[1] = conn.Dset.Tables["CHITIETPHIEUNHAP"].Columns["MASANPHAM"];
            conn.Dset.Tables["CHITIETPHIEUNHAP"].PrimaryKey = primarykey2;
        }
        private void pTitle_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserControlHoaDon_Load(object sender, EventArgs e)
        {
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView3.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView4.DefaultCellStyle.ForeColor = Color.Black;
            createHoaDon();
            createCTHoaDon();
            createPhieuNhap();
            createCTPhieuNhap();
            dataGridView1.DataSource = conn.Dset.Tables["HOADON"];
            dataGridView3.DataSource = conn.Dset.Tables["PHIEUNHAP"];
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            string a = dataGridView1.Rows[index].Cells[0].Value.ToString().Trim();
            dataGridView2.DataSource = conn.layct(a);
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (conn.searchHoaDon(txtTimKiem.Text) != null)
            {
                dataGridView1.DataSource = conn.searchHoaDon(txtTimKiem.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void BtnTroLai_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = conn.Dset.Tables["HOADON"];
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (conn.searchPhieuNhap(textBox1.Text) != null)
            {
                dataGridView3.DataSource = conn.searchPhieuNhap(textBox1.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = conn.Dset.Tables["PHIEUNHAP"];
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            if (e.RowIndex == -1) return;
            string a = dataGridView3.Rows[index].Cells[0].Value.ToString().Trim();
            dataGridView4.DataSource = conn.layctPN(a);
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
        {
            if (conn.searchPhieuNhap(txtTimKiem.Text) != null)
            {
                dataGridView1.DataSource = conn.searchHoaDon(txtTimKiem.Text);
            }
            else MessageBox.Show("Không tìm thấy");
        }

        private void BtnTroLai_Click_1(object sender, EventArgs e)
        {
            dataGridView1.DataSource = conn.Dset.Tables["HOADON"];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoanhThu hienthi = new DoanhThu();
            hienthi.ShowDialog();
            
        }
    }
}
