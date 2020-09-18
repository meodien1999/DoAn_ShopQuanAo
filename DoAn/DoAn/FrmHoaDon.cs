using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class FrmHoaDon : Form
    {
        public FrmHoaDon()
        {
            InitializeComponent();
        }

        private void FrmHoaDon_Load(object sender, EventArgs e)
        {
            HoaDon a = new HoaDon();
            crystalReportViewer1.ReportSource = a;
            a.SetParameterValue("MaHD",layten.LayThongTinThanhTien());
            a.SetParameterValue("tenNV", layten.LayThongTinTenNV());
            a.SetParameterValue("TenKH", layten.LayThongTintenKH());
            a.SetParameterValue("tgian", layten.LayThongTintg());
            //a.SetParameterValue("TienThoi", layten.LayThongTienthoi());
            crystalReportViewer1.ReportSource = a;
            crystalReportViewer1.Refresh();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
