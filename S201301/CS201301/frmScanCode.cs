using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Media;
using Excel = Microsoft.Office.Interop.Excel;

namespace CS201301
{
    public partial class frmScanCode : Form
    {
        BaseOperate boperate = new BaseOperate();
        protected string M_str_sql = "select ScanCode as 条码 from tb_ScanCode";
        protected string M_str_table = "tb_ScanCode";
        DataTable dt = new DataTable();
        int k;
        string sql = @"select ScanCode as 条码,ID AS 单号,WName AS 品名,Dealer as 经销商,BOXID AS 箱数,EMPLOYEEID AS 工号,DATE AS 日期 
                            from tb_ScanCode";
       public frmScanCode()
        {
         
            InitializeComponent();
        }
       private void frmScanCode_Load(object sender, EventArgs e)
       {
       
          
       }
       #region load1
       private void load1()
       {
            string M_str_sqlcount = "select count(Scancode) from tb_ScanCode where ID='"+txtID .Text.Trim () +"'";
            string sqlo = sql+" where ID='" + txtID.Text.Trim() + "' order by date asc";
           SqlConnection sqlcon = boperate.getcon();
           sqlcon.Open();
           SqlCommand sqlcom = new SqlCommand(M_str_sqlcount, sqlcon);
           int iCount = (int)sqlcom.ExecuteScalar();
           dt=boperate.getdt (sqlo);
           dataGridView1.DataSource = dt;
           dgvStateControl();
           this.dataGridView1.ReadOnly = true;


       }
       #endregion
       #region dgvStateControl
       private void dgvStateControl()
       {
           int i;
           int numCols = dataGridView1.Columns.Count;

           for (i = 0; i < numCols; i++)
           {

               dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
          
               if (i == 1|| i==2 || i==3 || i==0 || i==4 || i==6)
               {
                   dataGridView1.Columns[i].Width = 150;

               }

               dataGridView1.EnableHeadersVisualStyles = false;
               dataGridView1.Columns[i].HeaderCell.Style.BackColor = Color.Lavender;

           }
           for (i = 0; i < dataGridView1.Rows.Count; i++)
           {
               for (int j = 0; j < dataGridView1.Columns.Count; j++)
               {

                   dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.OldLace;


               }

               i = i + 1;
           }
       }
       #endregion
          private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
          #region lock
          private void btnLock_Click(object sender, EventArgs e)
          {
              try
              {
                  SqlDataReader sqlread1 = boperate.getread("select * from tb_scancode where ID='" + txtID.Text.Trim() + "'");
                  sqlread1.Read();
                  if (sqlread1.HasRows)
                  {
                      MessageBox.Show("该单号已经存在！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                  }
                  else if (txtEmployeeID.Text == "")
                  {
                      MessageBox.Show("工号不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);


                  }
                  else if (txtID.Text == "")
                  {

                      MessageBox.Show("单号不能为空", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);

                  }
            
                  else if (btnLock.Text == "锁定")
                  {
                      txtEmployeeID.Enabled = false;
                      txtID.Enabled = false;
                      txtWName.Enabled = false;
                      txtDealer.Enabled = false;
                      btnLock.Text = "解锁";
                      txtScanCode.Enabled = true;
                      btnDel.Enabled = true;
                      btnDelScanCode.Enabled = true;
                      txtScanCode.Focus();
                      this.txtScanCode.BackColor = Color.Lime;
                      dt = null;
                      dataGridView1.DataSource = dt;
                    
                  }
                  else
                  {
                      txtEmployeeID.Enabled = true;
                      txtID.Enabled = true;
                      txtWName.Enabled = true;
                      txtDealer.Enabled = true;
                      btnLock.Text = "锁定";
                      btnDel.Enabled = false;
                      txtScanCode.Enabled = false;
                   
                  }
                  sqlread1.Close();
            
                  
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);

              }
          }
          #endregion
          #region save
          private void btnSave_Click(object sender, EventArgs e)
          {
              try
              {
                  
                  txtEmployeeID.Enabled = true;
                  txtID.Enabled = true;
                  txtWName.Enabled = true;
                  txtDealer.Enabled = true;
                  btnLock.Text = "锁定";
                  txtScanCode.Enabled = false;
                  txtID.Text = "";
                  txtEmployeeID.Text = "";
                  txtScanCode.Text = "";
                  txtWName.Text = "";
                  txtDealer.Text = "";
                  txtEmployeeID.Focus();
                  btnDel.Enabled = false;
                  btnDelScanCode.Enabled = false;
                  load1();
              }
              catch (Exception )
              {


              }

          }
          #endregion 
          #region look
          private void btnLook_Click(object sender, EventArgs e)
          {
         
              if (btnLock.Text == "解锁")
              {
                  btnDel.Enabled = true ;
                  btnDelScanCode.Enabled = true ;
              }
              else
              {
                  btnDel.Enabled = false ;
                  btnDelScanCode.Enabled = false ;

              }
              try
              {

              if (txtScanCodeQuery.Text == "")
                {

                          MessageBox.Show("内容不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 }
               else 
                {

                    if (cmbCondition.Text  == "单号")
                    {


                        string sqlt1 = sql+" where ID='" + txtScanCodeQuery.Text.Trim() + "' order by date desc";
                        dt = boperate.getdt(sqlt1);

                        if (dt.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dt;
                        }
                        else
                        {

                            MessageBox.Show("不存在该单号！", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            dt = null;
                            dataGridView1.DataSource = dt;

                        }

                        dgvStateControl();
                    }
                    else
                    {

                       
                            string sqlt1 =sql+" where ScanCode='" +txtScanCodeQuery .Text .Trim ()+ "' order by date desc";
                            dt = boperate.getdt(sqlt1);

                            if (dt.Rows.Count > 0)
                            {
                                dataGridView1.DataSource = dt;
                            }
                            else
                            {

                             MessageBox.Show("不存在该条码！", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            dt = null;
                            dataGridView1.DataSource = dt;

                            }

                            dgvStateControl();
                  
                    }
             }
           
              }
              catch (Exception )
              {

                  MessageBox.Show("操作异常请检查！", "", MessageBoxButtons.OK, MessageBoxIcon.Error );

              }


          }
          #endregion
          #region del
          private void btnDel_Click(object sender, EventArgs e)
          {
              try
              {
                  dt = boperate.getdt("select * from tb_scancode where ID='" + txtID.Text.Trim() + "'");
                  if(dt.Rows.Count >0)
                  {
                     
                      if (MessageBox.Show("确认要删除该单号下的记录吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                      {
                         
                              
                              boperate.getcom("delete tb_scancode where ID='"+txtID .Text .Trim () + "'");
                              load1();
                              int i = dt.Rows.Count;
                              boperate.getcom("UPDATE TB_SCANCODE SET BOXID='" + Convert.ToString(i) + "' WHERE ID='" + txtID.Text.Trim() + "'");
                              load1();
                              txtScanCode.Focus();
                         
                      }
                  }
                  else
                  {
                      MessageBox.Show("无记录可删！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                  }
              }
              catch (Exception)
              {

                  MessageBox.Show("出错啦！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
           
          }
          private int  yesno(string vars)
          {
           
                int i;
                for (i = 0; i < vars.Length; i++)
                  {
                      int p = Convert.ToInt32(vars[i]);
                      if (p >= 48 && p <= 57 || p >= 65 && p <= 90 || p >= 97 && p <= 122)
                      {
                          k = 1;
                      }
                      else
                      {
                          k = 0; break;
                      }

                  }
         
                return k;

          }
          #endregion
          #region keypress
          private void txtScanCode_KeyPress(object sender, KeyPressEventArgs e)
          {
              string varScanCode, varID, varEmployeeID, varDate,varDealer,varWName;
              varScanCode = txtScanCode.Text.Trim();
              varID = txtID.Text.Trim();
              varEmployeeID = txtEmployeeID.Text.Trim();
            varDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
              varDealer = txtDealer.Text;
              varWName = txtWName.Text;
            if (e.KeyChar == 13)
            {

                if (varScanCode == "")
                {
                    MessageBox.Show("条码不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (yesno(varScanCode) == 0)
                {
                    MessageBox.Show("输入的字符不合法", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtScanCode.Text = "";

                }
                else
                {
                    SqlDataReader sqlread = boperate.getread("select * from tb_scancode where scancode='" + varScanCode + "'");
                    sqlread.Read();
                    if (sqlread.HasRows)
                    {

                        string filename = "sound";
                        string mediasound = "D:\\" + filename + ".wav";
                        System.Media.SoundPlayer a = new SoundPlayer(mediasound);
                        a.Play();
                        MessageBox.Show("该条码已经存在！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }

                    else
                    {
                        boperate.getcom("insert into tb_ScanCode(ScanCode,ID,EmployeeID,Date,Dealer,WName) values('" + varScanCode +
                            "','" + varID + "','" + varEmployeeID + "','" + varDate + "','" + varDealer + "','" + varWName + "')");
                        load1();
                        boperate.getcom("UPDATE TB_SCANCODE SET BOXID='" + Convert.ToString(dt.Rows.Count) + "' WHERE ID='" + txtID.Text.Trim() + "'");
                        load1();
                    }
                    sqlread.Close();
                    txtScanCode.Text = "";
                }

            }

            load1();
            try
              {


              }
              catch (Exception ex)
              {


                  MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

              }

          }
#endregion
          private void btnToExcel_Click(object sender, EventArgs e)
          {
              dgvtoExcel(dataGridView1 );
          }
          #region toexcel
          public void dgvtoExcel(DataGridView dataGridView1)
          {

              SaveFileDialog sfdg = new SaveFileDialog();
              sfdg.DefaultExt = "xls";
              sfdg.Filter = "Excel(*.xls)|*.xls";
              //sfdg.RestoreDirectory = true;
              sfdg.FileName = "扫描明细";
              //sfdg.CreatePrompt = true;
              sfdg.Title = "導出到EXCEL";
              int n, w;
              n = dataGridView1.RowCount;
              w = dataGridView1.ColumnCount;


              if (sfdg.ShowDialog() == DialogResult.OK)
              {
                  try
                  {
                    Excel.ApplicationClass excel = new Excel.ApplicationClass();

                    excel.Application.Workbooks.Add(true);

                      for (int j = 0; j < dataGridView1.ColumnCount; j++)
                      {

                          excel.Cells[1, j + 1] = dataGridView1.Columns[j].HeaderText;
                      }
                      for (int i = 0; i < dataGridView1.RowCount; i++)
                      {
                          for (int x = 0; x < dataGridView1.ColumnCount; x++)
                          {
                              if (dataGridView1[x, i].Value != null)
                              {
                                  if (dataGridView1[x, i].ValueType == typeof(string))
                                  {
                                      excel.Cells[i + 2, x + 1] = "'" + dataGridView1[x, i].Value.ToString();
                                  }
                                  else
                                  {
                                      excel.Cells[i + 2, x + 1] = dataGridView1[x, i].Value.ToString();
                                  }
                              }
                          }
                      }
                      excel.get_Range(excel.Cells[1, 1], excel.Cells[1, w]).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;

                      //excel.get_Range(excel.Cells[2, 3], excel.Cells[n, 3]).HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlRight;
                      excel.get_Range(excel.Cells[1, 1], excel.Cells[n, w]).Borders.LineStyle = 1;
                      excel.get_Range(excel.Cells[1, 1], excel.Cells[n, w]).Select();
                      excel.get_Range(excel.Cells[1, 1], excel.Cells[n, w]).Columns.AutoFit();
                      excel.Visible = false;
                      excel.ExtendList = false;
                      excel.DisplayAlerts = false;
                      excel.AlertBeforeOverwriting = false;
                      excel.ActiveWorkbook.SaveAs(sfdg.FileName, Excel.XlFileFormat.xlExcel7, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                      excel.Quit();
                  
                      excel = null;
                      GC.Collect();
                  }

                  catch (Exception ex)
                  {
                      MessageBox.Show(ex.Message);
                  }
                  finally
                  {
                      GC.Collect();
                  }

              }
          }

          #endregion
          #region del_scancode
          private void btnDelScanCode_Click(object sender, EventArgs e)
          {
              try
              {
                  dt = boperate.getdt("select * from tb_scancode where ID='" + txtID.Text.Trim() + "'");
                  if(dt.Rows .Count >0)
                  {

                      int rowindex = dataGridView1.CurrentCell.RowIndex;
                      string varScanCode = dataGridView1[0, rowindex].Value.ToString();
                      string varID = dataGridView1[1, rowindex].Value.ToString();
                      if (MessageBox.Show("确认要删除该条记录吗？", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                      {
                          if (varID != txtID.Text)
                          {
                              MessageBox.Show("删除项不位于当前单号下","",MessageBoxButtons .OK ,MessageBoxIcon.Warning );
                          }
                          else
                          {
                              boperate.getcom("delete tb_scancode where scancode='" + varScanCode + "' and ID='" + txtID.Text.Trim() + "'");
                              load1();
                              int i = dt.Rows.Count;
                              boperate.getcom("UPDATE TB_SCANCODE SET BOXID='" + Convert.ToString(i) + "' WHERE ID='" + txtID.Text.Trim() + "'");
                              load1();
                              txtScanCode.Focus();
                          }
                      }

                  }
                  else
                  {
             
                      MessageBox.Show("无记录可删！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                  }
              }
              catch (Exception)
              {

                  MessageBox.Show("出错啦！", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
          }
          #endregion
          #region print
          private void btnprint_Click(object sender, EventArgs e)
          {
              string varID = txtID.Text.Trim();
              if (varID == "")
              {
                  MessageBox.Show("单号不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
              }
              else
              {
                  dt = boperate.getdt ("select * from tb_scancode where ID='" + txtID.Text.Trim() + "'");
                  if (dt.Rows.Count > 0)
                  {
                      CS201301.FrmCRScanCode.Array[0] = txtID.Text;
                      CS201301.FrmCRScanCode frm = new FrmCRScanCode();
                      frm.Show();

                  }
                  else
                  {
                      MessageBox.Show("单号不存在！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                  }

              }
          }
          #endregion
          #region generate
          private void btngenerate_Click(object sender, EventArgs e)
          {
              string varScanCode, varID, varEmployeeID, varDate, varDealer, varWName;
              varScanCode = txtScanCode.Text.Trim();
              varID = txtID.Text.Trim();
              varEmployeeID = txtEmployeeID.Text.Trim();
              varDate = DateTime.Now.ToString();
              varDealer = txtDealer.Text;
              varWName = txtWName.Text;
              string varstartcode = txtstartcode.Text;
              string varendcode = txtendcode.Text;
              string var5 = "";
              int varsuccess = 0;
              int varfail= 0;
              try
              {
                  if (varstartcode == "")
                  {
                      MessageBox.Show("起始条码不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                 
                  }
                  else if (varendcode == "")
                  {

                      MessageBox.Show("终止条码不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (txtID.Text == "")
                  {

                      MessageBox.Show("单号不能为空！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (varstartcode.Length != 18)
                  {
                      MessageBox.Show("起始条码长度不为18！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (varendcode.Length != 18)
                  {
                      MessageBox.Show("终止条码长度不为18！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (yesno(varstartcode) == 0)
                  {
                      MessageBox.Show("起始条码存在不合法字符！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (yesno(varendcode) == 0)
                  {
                      MessageBox.Show("终止条码存在不合法字符！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (varstartcode.Substring (0,6) !=varendcode.Substring(0,6))
                  {
                      MessageBox.Show("起始条码前6位与终止条码前6位不一致！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
                  else if (varstartcode.Substring(15, 3) != varendcode.Substring(15, 3))
                  {
                      MessageBox.Show("起始条码后3位与终止条码后3位不一致！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                  }
         
         
                  else
                  {
                      dt=boperate .getdt ("select * from tb_scancode where ID='" + txtID.Text.Trim() + "'");
                     if(dt.Rows.Count >0)
                     {
                          MessageBox.Show("该单号已经存在！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
                     }
                      else 
                     {

                 
                    
                          string var1 = varstartcode.Substring(6, 9);
                          string var2 = varendcode.Substring(6, 9);
                          int var3 = (Convert.ToInt32(var2) - Convert.ToInt32(var1));
                          if (var3 > 0)
                          {

                              for (int i = 0; i <= var3; i++)
                              {
                                  int var4 = Convert.ToInt32(var1) + i;
                                  var5 = Convert.ToString(var4);
                                  while (Convert.ToString(var5).Length < 9)
                                  {

                                      var5 = "0" + var5;

                                  }

                                  string var7 = varstartcode.Substring(0, 6) + var5 + varstartcode.Substring(15, 3);
                                  dt = boperate.getdt("select * from tb_scancode where scancode='" + var7 + "'");

                                  if (dt.Rows.Count > 0)
                                  {

                                      varfail = varfail + 1;
                                     

                                  }
                                  else
                                  {
                                      boperate.getcom("insert into tb_ScanCode(ScanCode,ID,EmployeeID,Date,Dealer,WName) values('" + var7 +
                                            "','" + varID + "','" + varEmployeeID + "','" + varDate + "','" + varDealer + "','" + varWName + "')");
                                      varsuccess = varsuccess + 1;

                                  }


                              }
                              dataGridView1.DataSource = boperate.getdt("select * from tb_scancode where id='" + txtID.Text.Trim() + "'");
                              load1();
                              boperate.getcom("UPDATE TB_SCANCODE SET BOXID='" + Convert.ToString(dt.Rows.Count) + "' WHERE ID='" + txtID.Text.Trim() + "'");
                              load1();
                              btnDelScanCode.Enabled = true;
                              btnDel.Enabled = true;
                              txtstartcode.Text = "";
                              txtendcode.Text = "";
                              MessageBox.Show("系统已经存在：" + Convert.ToString(varfail) + "个" + " 成功生成：" + Convert.ToString(varsuccess) + "个", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                          }
                               

                          else
                          {
                              MessageBox.Show("终止条码值须大于起始条码值！", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                          }
                     }
                  }
              }
              catch (Exception ex)
              {

                  MessageBox.Show(ex.Message);
              }
          }
          #endregion

          private void btnCloseSound_Click(object sender, EventArgs e)
          {
              try
              {
                  string filename = "sound";
                  string mediasound = "D:\\" + filename + ".wav";
                  System.Media.SoundPlayer a = new SoundPlayer(mediasound);
                  a.Stop();
              }
              catch (Exception)
              {



              }
          }
    }
}
