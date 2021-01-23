using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace CS201301
{
    public partial class FrmCRScanCode : Form
    {
        BaseOperate boperate = new BaseOperate();
        public static string[] Array = new string[] { "", "" };
     
        public FrmCRScanCode()
        {
            InitializeComponent();
        }
        public  void Bind()
        {


            SqlConnection sqlcon = boperate.getcon();
            sqlcon.Open();
            string sql = "select * from tb_ScanCode where ID='" + Array[0] + "'";
            SqlDataAdapter da = new SqlDataAdapter(sql, sqlcon);
            DataSet1 ds = new DataSet1();
            da.Fill(ds, "tb_ScanCode");
            CRScanCode oRpt=new CRScanCode ();
            string ul = "C:\\Program Files\\Xizhe\\条码扫描程序\\CRScanCode.rpt";
            oRpt.Load(ul);
            oRpt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = oRpt;
      
        }
        private void FrmCRScanCode_Load(object sender, EventArgs e)
        {
            Bind();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
