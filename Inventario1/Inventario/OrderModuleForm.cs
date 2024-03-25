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
using System.Xml.Linq;

namespace Inventario
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=RAY\SERVIDOR_SQL_RAY;Initial Catalog=dbIMS;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            Loadproduct();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
            
        }

        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid, cname FROM tbCustomer WHERE CONCAT(cid,cname) LIKE '%"+txtSearchCust.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close(); 
            con.Close();
        }
        public void Loadproduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM dbOrder WHERE CONCAT(pid, pname,pprice,pdescription,pcategory) LIKE '%" + txtSearchProduct.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }
        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();

        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            Loadproduct();
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        int qty = 0;
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
           
        {
            if (Convert.ToInt16(numericUpDown1.Value) > qty)
            {
                MessageBox.Show("La cantidad en stock no es suficiente", "peligro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericUpDown1.Value = numericUpDown1.Value - 1;
                return;
            }
            int total = Convert.ToInt16(txtprice.Text) * Convert.ToInt32(numericUpDown1.Value);
            txttotal.Text = total.ToString();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtcid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtcname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            txtpid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtpname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtprice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
            qty = Convert.ToInt32(dgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString());
        }

       private void btnInsert_Click(object sender, EventArgs e)
        {
                    try
                 {
                     if (txtcid.Text == "")
                     {
                         MessageBox.Show("Seleciona un cliente", "Peligro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     }
                     if (txtpid.Text == "")
                     {
                         MessageBox.Show("Seleciona un producto", "Peligro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                     }
                     if (MessageBox.Show("¿Estas seguro que quieres insertar la orden ?", "In", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                     {
                         cm = new SqlCommand("INSERT INTO dbOrder(odate, pid, cid, qty, price, total)VALUES(@odate, @pid, @cid, @qty, @price, @total)", con);
                         cm.Parameters.AddWithValue("@odate", dtOrder.Value);
                         cm.Parameters.AddWithValue("@pid", Convert.ToInt32(txtpid.Text));
                         cm.Parameters.AddWithValue("@cid", Convert.ToInt32(txtcid.Text));
                         cm.Parameters.AddWithValue("@qty", Convert.ToInt32(numericUpDown1.Value));
                         cm.Parameters.AddWithValue("@price", Convert.ToInt32(txtprice.Text));
                         cm.Parameters.AddWithValue("@total", Convert.ToInt32(txttotal.Text));
                         con.Open();
                         cm.ExecuteNonQuery();
                         con.Close();
                         MessageBox.Show("Order echa");
                         Clear();
                     }
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message);
                 }
        }
        public void Clear()
        {

            txtcname.Clear();
            txtcid.Clear();

            txtpid.Clear();
            txtpname.Clear();

            txtprice.Clear();
            numericUpDown1.Value = 1;
            txttotal.Clear();
            dtOrder.Value = DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnInsert.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void dtOrder_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
