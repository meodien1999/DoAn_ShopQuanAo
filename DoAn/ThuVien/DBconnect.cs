using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ThuVien
{
    public class DBconnect
    {
        SqlConnection con;


        string tennv;

        public string Tennv
        {
            get { return tennv; }
            set { tennv = value; }
        }

        string manv;

        public string Manv
        {
            get { return manv; }
            set { manv = value; }
        }
        string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        string pass;

        public string Pass
        {
            get { return pass; }
            set { pass = value; }
        }


        public SqlConnection Con
        {
            get { return con; }
            set { con = value; }
        }

        string strcon;

        public string Strcon
        {
            get { return strcon; }
            set { strcon = value; }
        }

        private DataSet dset;

        public DataSet Dset
        {
            get { return dset; }
            set { dset = value; }
        }

        public DBconnect(string dataSetName)
        {

            Strcon = "Data Source= .\\SQLEXPRESS;Initial Catalog=QL_SHOPTHOITRANG;User ID=sa;Password=123";
            Con = new SqlConnection(strcon);
            Dset = new DataSet(dataSetName);
        }
        // nếu đóng thì mở
        public void openConnection()
        {
            if (Con.State == ConnectionState.Closed)
                Con.Open();
        }
        //nếu mở thì đóng
        public void closeConnection()
        {
            if (Con.State == ConnectionState.Open)
                Con.Close();
        }

        public void disposeConnection()
        {
            if (Con.State == ConnectionState.Open)
                Con.Close();
            Con.Dispose(); //giải phóng bộ nhớ
        }
        // cập nhật lại database
        public void updateTODB(string strSQL)
        {
            //mở
            openConnection();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSQL;
            cmd.Connection = Con;
            cmd.ExecuteNonQuery();

            //đóng
            closeConnection();
        }
        //đếm các phần tử có trong database (kiểm tra trùng)
        public int getCount(string strSQL)
        {
            openConnection();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSQL;
            cmd.Connection = Con;
            int count = (int)cmd.ExecuteScalar();
            //đếm

            //đóng
            closeConnection();
            return count;
        }
        //nến trùng thì đếm ở trên lớn hơn 0
        public bool kiemTraTrung(string strSQL)
        {
            int count = getCount(strSQL);
            if (count > 0)
                return true; //trùng
            return false; //không trùng
        }

       

        public SqlDataReader getDataReader(string strSQL)
        {
            openConnection();

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = strSQL;
            cmd.Connection = Con;
            return cmd.ExecuteReader();
        }

        public DataTable getDataTable(string strSQL, string tableName)
        {
            openConnection();

            SqlDataAdapter ada = new SqlDataAdapter(strSQL, this.con);
            ada.Fill(Dset, tableName);
            closeConnection();
            return Dset.Tables[tableName];
        }
        

        public SqlDataAdapter getDataAdapter(string strSQL, string tableName)
        {
            openConnection();

            SqlDataAdapter ada = new SqlDataAdapter(strSQL, this.con);
            ada.Fill(Dset, tableName);
            closeConnection();
            return ada;
        }

       

        public bool CompareValue(string strDataTableName, string strDataColName, string strValue)
        {
            SqlCommand cmm;
            int count = 0;
            openConnection();
            string strReq = string.Format("select * from {0}", strDataTableName);
            cmm = new SqlCommand(strReq, con);

            SqlDataReader reader = cmm.ExecuteReader();

            //So sánh giá trị với từng dòng truyền vào
            while (reader.Read())
            {
                //Kiểm tra sự tồn tại của giá trị trong cột
                if (reader[strDataColName].ToString() == strValue)
                    count++;
            }

            cmm.Dispose();
            reader.Dispose();

            if (count > 0)
                return true;
            else
                return false;
        }

        public bool isAdmin(string strDataTableName, string strDataColName, string strDataColName2, string strAdminCol, string username, string pass)
        {
            SqlCommand cmm;
            int count = 0;
            openConnection();
            string strReq = string.Format("select * from {0}", strDataTableName);
            cmm = new SqlCommand(strReq, con);

            SqlDataReader reader = cmm.ExecuteReader();

            //So sánh giá trị với từng dòng truyền vào
            while (reader.Read())
            {

                //Kiểm tra sự tồn tại của giá trị trong cột
                if (reader[strDataColName].ToString() == username && reader[strDataColName2].ToString() == pass)
                    if (bool.Parse(reader[strAdminCol].ToString()) == true)
                        count++;

            }

            cmm.Dispose();
            reader.Dispose();

            if (count > 0)
                return true;
            else
                return false;
        }

        public DataTable searchNV(string ma)
        {
            DataTable table = new DataTable();
            openConnection();
            string lenh = string.Format("SELECT * FROM NHANVIEN WHERE HOTEN like '%" + ma + "%'");
            SqlDataAdapter ada = new SqlDataAdapter(lenh, this.con);
            ada.Fill(table);

            closeConnection();
            return table;
        }
        public DataTable searchSanPham(string ma)
        {
            DataTable table = new DataTable();
            openConnection();
            string lenh = string.Format("SELECT * FROM SANPHAM WHERE MASANPHAM = '" + ma + "'");
            SqlDataAdapter ada = new SqlDataAdapter(lenh, this.con);
            ada.Fill(table);

            closeConnection();
            return table;
        }

        public DataTable searchHoaDon(string ma)
        {
            DataTable table = new DataTable();
            openConnection();
            string lenh = string.Format("SELECT * FROM HOADON WHERE MAHD = '" + ma + "'");
            SqlDataAdapter ada = new SqlDataAdapter(lenh, this.con);
            ada.Fill(table);

            closeConnection();
            return table;
        }
        public DataTable searchPhieuNhap(string ma)
        {
            DataTable table = new DataTable();
            openConnection();
            string lenh = string.Format("SELECT * FROM PHIEUNHAP WHERE MAPN = '" + ma + "'");
            SqlDataAdapter ada = new SqlDataAdapter(lenh, this.con);
            ada.Fill(table);

            closeConnection();
            return table;
        }

        public DataTable LoadData(string sql)
        {
            DataTable table = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(sql, this.con);
            adt.Fill(table);
            return table;
        }

        public DataTable GetCt(string ma)
        {
            string lenh = string.Format("SELECT * From CHITIETHOADON where MAHD ='" +ma+"'");
              DataTable table = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(lenh, this.con);
            adt.Fill(table);
            return table;
          
        }

        public DataTable layct(string ma)
        {
            return GetCt(ma);
        }
        public DataTable GetCtPN(string ma)
        {
            string lenh = string.Format("SELECT * From CHITIETPHIEUNHAP where MAPN ='" + ma + "'");
            DataTable table = new DataTable();
            SqlDataAdapter adt = new SqlDataAdapter(lenh, this.con);
            adt.Fill(table);
            return table;

        }

        public DataTable layctPN(string ma)
        {
            return GetCtPN(ma);
        }
        public bool isEmail(string inputEmail)
        {
            inputEmail = inputEmail ?? string.Empty;
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}

