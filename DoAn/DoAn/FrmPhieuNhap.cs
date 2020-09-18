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
    public partial class FrmPhieuNhap : Form
    {
        public FrmPhieuNhap()
        {
            InitializeComponent();
        }

        private void FrmPhieuNhap_Load(object sender, EventArgs e)
        {
            PhieuNhap a = new PhieuNhap();
            crystalReportViewer1.ReportSource = a;
            a.SetParameterValue("MAPN", layten.LayThongTinThanhTien());
            a.SetParameterValue("tenNV", layten.LayThongTinTenNV());
            a.SetParameterValue("tenNCC", layten.LayThongTintenKH());
            a.SetParameterValue("tgian", layten.LayThongTintg());
            crystalReportViewer1.ReportSource = a;
            crystalReportViewer1.Refresh();
        }
    }
}
