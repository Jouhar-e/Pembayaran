using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pembayaran
{
    public partial class Form1 : Form
    {

        Module md = new Module();
        string idtransaksi = "0";
        string id = "0";

        public Form1()
        {
            InitializeComponent();
        }

        public void awal(string id)
        {
            dataGridView1.DataSource = md.getData("SELECT * FROM vdetail WHERE id_transaksi = '" + id + "' AND nama_item LIKE '%" + textBox5.Text + "%'");
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Item";
            dataGridView1.Columns[3].HeaderText = "Jumlah";
            dataGridView1.Columns[4].HeaderText = "Harga Satuan";
            dataGridView1.Columns[5].HeaderText = "Subtotal";
            dataGridView1.Columns[6].HeaderText = "Tanggal Transaksi";
            dataGridView1.Columns[7].HeaderText = "Pelanggan";
            dataGridView1.Columns[8].HeaderText = "Total Pembayaran";

            string sql = "SELECT SUM(subtotal) AS total FROM vdetail WHERE id_transaksi = " + idtransaksi;
            //md.pesan(md.getValue(sql,"total"));
            string total = md.getValue(sql, "total");
            if (total == "")
            {
                lTotal.Text = "0";
            }
            else
            {
                lTotal.Text = total;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                md.pesan("Data Pelanggan Kosong");
            }
            else
            {
                var sql = "INSERT INTO transaksi VALUES ('" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + textBox1.Text + "',0)";
                //md.pesan(sql);
                md.exc(sql);
                md.pesan("Pelanggan sudah dipilih");
                groupBox1.Enabled = false;
                idtransaksi = md.getValue("SELECT * FROM transaksi ORDER BY id_transaksi DESC", "id_transaksi");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //md.pesan(idtransaksi);
            if (idtransaksi == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                md.pesan("Data masih ada yang kosong");
            }
            else
            {
                string sql = "INSERT INTO detail_transaksi VALUES ('" + idtransaksi + "','" + textBox2.Text + "'," +
                    "'" + textBox3.Text + "','" + textBox4.Text + "')";
                //md.pesan(sql);
                md.exc(sql);
                md.pesan("Berhasil ditambahkan");
                md.clearForm(groupBox2);
                awal(idtransaksi);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            awal(idtransaksi);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (id == "0")
            {
                md.pesan("Pilih item dulu");
            }
            else
            {
                if (md.dialogForm("Apakah andan ingin menghapus item ini?"))
                {
                    string sql = "DELETE FROM detail_transaksi WHERE id_detail = " + id;
                    //md.pesan(sql);
                    md.exc(sql);
                    md.clearForm(groupBox2);
                    md.pesan("Item berhasil dihapus");
                    awal(idtransaksi);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (idtransaksi == "0")
            {
                md.pesan("Data masih kosong");
            }
            else
            {
                if (md.dialogForm("Lanjutkan transaksi?"))
                {
                    string sql = "UPDATE transaksi SET tanggal_transaksi = '" + DateTime.Now.ToString("yyyy/MM/dd") + "'," +
                        " total_pembayaran = '" + double.Parse(lTotal.Text) + "' WHERE id_transaksi = " + idtransaksi;
                    //md.pesan(sql);
                    md.exc(sql);
                    md.pesan("Transaksi berhasil");
                    md.clearForm(groupBox1);
                    groupBox1.Enabled = true;
                    idtransaksi = "0";
                    awal(idtransaksi);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM vdetail WHERE id_transaksi = " + idtransaksi;
            string sqldetail = "DELETE FROM detail_transaksi WHERE id_transaksi = " + idtransaksi;
            string sqltransaksi = "DELETE FROM transaksi WHERE id_transaksi = " + idtransaksi;
            if (idtransaksi == "0")
            {
                md.pesan("Pelanggan masih kosong");
            }
            else if (md.getCount(sql) == 0)
            {
                //md.pesan("Pelanggan ada tapi item kosong");
                if (md.dialogForm("Apakah anda ingin mengakhiri transaksi?"))
                {
                    //md.pesan(sqltransaksi);
                    md.exc(sqltransaksi);
                    md.clearForm(groupBox1);
                    md.pesan("Transaksi berhasil dibatalkan");
                    groupBox1.Enabled = true;
                }
            }
            else
            {
                //md.pesan("Ada semua");
                if (md.dialogForm("Apakah anda ingin mengakhiri transaksi?"))
                {
                    //md.pesan(sqldetail);
                    //md.pesan(sqltransaksi);
                    md.exc(sqldetail);
                    md.exc(sqltransaksi);
                    md.clearForm(groupBox1);
                    awal("0");
                    md.pesan("Transaksi berhasil dibatalkan");
                    groupBox1.Enabled = true;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string sql = "SELECT * FROM vdetail WHERE id_transaksi = " + idtransaksi;
            string sqldetail = "DELETE FROM detail_transaksi WHERE id_transaksi = " + idtransaksi;
            string sqltransaksi = "DELETE FROM transaksi WHERE id_transaksi = " + idtransaksi;
            if (md.getCount(sql) == 0)
            {
                //md.pesan("Pelanggan ada tapi item kosong");
                if (md.dialogForm("Apakah anda ingin mengakhiri transaksi?"))
                {
                    //md.pesan(sqltransaksi);
                    md.exc(sqltransaksi);
                    md.clearForm(groupBox1);
                    md.pesan("Transaksi berhasil dibatalkan");
                    groupBox1.Enabled = true;
                }
            }
            else
            {
                //md.pesan("Ada semua");
                if (md.dialogForm("Apakah anda ingin mengakhiri transaksi?"))
                {
                    //md.pesan(sqldetail);
                    //md.pesan(sqltransaksi);
                    md.exc(sqldetail);
                    md.exc(sqltransaksi);
                    md.clearForm(groupBox1);
                    awal("0");
                    md.pesan("Transaksi berhasil dibatalkan");
                    groupBox1.Enabled = true;
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            md.onlyNumber(e);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            md.onlyNumber(e);
        }
    }
}
