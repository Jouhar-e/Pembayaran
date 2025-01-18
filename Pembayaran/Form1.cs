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
        string idtransaksi;

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
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            idtransaksi = md.getValue("SELECT * FROM transaksi ORDER BY id_transaksi DESC", "id_transaksi");
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
    }
}
