using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FinancialCrm.Models;

namespace FinancialCrm
{
    public partial class Form2 : Form
    {
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            try
            {
                var values = db.Bills.ToList();
                dataGridView1.DataSource = values;

                if (dataGridView1.Columns.Contains("BillAmount"))
                    dataGridView1.Columns["BillAmount"].DefaultCellStyle.Format = "0.00";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veriler yüklenirken bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBillList_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void btnCreateBill_Click(object sender, EventArgs e)
        {
            string title = txtBillTitle.Text.Trim();
            string period = txtBillPeriod.Text.Trim();
            string amountText = txtBillAmount.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(amountText) ||
                string.IsNullOrWhiteSpace(period))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (title.Length > 100)
            {
                MessageBox.Show("Başlık çok uzun. Lütfen daha kısa bir başlık girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (period.Length > 50)
            {
                MessageBox.Show("Dönem çok uzun. Lütfen daha kısa bir dönem girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (title.Any(c => "!@#$%^&*()+=[]{};:'\"|\\<>,/?".Contains(c)))
            {
                MessageBox.Show("Başlıkta özel karakter kullanmayın.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli, pozitif bir tutar girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (amount > 10000)
            {
                MessageBox.Show("Tutar çok yüksek. Lütfen 10.000 TL'den küçük bir tutar girin.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var bill = new Bills
                {
                    BillTitle = title,
                    BillPeriod = period,
                    BillAmount = amount
                };

                db.Bills.Add(bill);
                db.SaveChanges();

                MessageBox.Show("Ödeme başarıyla eklendi.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemoveBill_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillId.Text, out int id))
            {
                MessageBox.Show("Geçerli bir fatura ID girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bill = db.Bills.Find(id);
            if (bill == null)
            {
                MessageBox.Show("Fatura bulunamadı.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                db.Bills.Remove(bill);
                db.SaveChanges();

                MessageBox.Show("Fatura başarıyla silindi.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Silme işlemi sırasında bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateBill_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBillId.Text, out int id))
            {
                MessageBox.Show("Güncellenecek faturanın ID'sini seçin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string title = txtBillTitle.Text.Trim();
            string period = txtBillPeriod.Text.Trim();
            string amountText = txtBillAmount.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(amountText) ||
                string.IsNullOrWhiteSpace(period))
            {
                MessageBox.Show("Tüm alanları doldurun.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(amountText, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli, pozitif bir tutar girin.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var bill = db.Bills.Find(id);
            if (bill == null)
            {
                MessageBox.Show("Fatura bulunamadı.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                bill.BillTitle = title;
                bill.BillAmount = amount;
                bill.BillPeriod = period;

                db.SaveChanges();

                MessageBox.Show("Fatura başarıyla güncellendi.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                RefreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme sırasında hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem is Bills bill)
                {
                    txtBillId.Text = bill.BillId.ToString();
                    txtBillTitle.Text = bill.BillTitle;
                    txtBillAmount.Text = bill.BillAmount.ToString("0.00");
                    txtBillPeriod.Text = bill.BillPeriod;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri doldurulurken hata: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sayfa geçiş butonları
        private void button1_Click(object sender, EventArgs e)
        {
            Kategoriler frm = new Kategoriler();
            frm.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmBanks frm = new FrmBanks();
            frm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Eğer kullanılmayacaksa silebilirsin
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Spendings frm = new Spendings();
            frm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BankProcess frm = new BankProcess();
            frm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dashboard frm = new Dashboard();
            frm.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Gerek yoksa boş kalabilir
        }
    }
}
