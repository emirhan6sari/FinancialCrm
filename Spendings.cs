using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinancialCrm.Models;

namespace FinancialCrm
{
    public partial class Spendings : Form
    {
        public Spendings()
        {
            InitializeComponent();
        }

        FinancialCrmDbEntities db = new FinancialCrmDbEntities();

        private void Spendings_Load(object sender, EventArgs e)
        {
            var values = db.Spendings.ToList();
            dataGridView1.DataSource = values;

            // ComboBox'a kategorileri yükle
            var categories = db.Categories
                               .Select(x => new { x.CategoryId, x.CategoryName })
                               .ToList();

            comboBoxSpending.DisplayMember = "CategoryName";
            comboBoxSpending.ValueMember = "CategoryId";
            comboBoxSpending.DataSource = categories;
        }

        private void btnSpendingList_Click(object sender, EventArgs e)
        {
            var values = db.Spendings.ToList();
            dataGridView1.DataSource = values;
        }

        private void btnSpendingBill_Click(object sender, EventArgs e)
        {
            // Boşluk ve tip kontrolü
            if (string.IsNullOrWhiteSpace(txtSpendingTitle.Text) ||
                string.IsNullOrWhiteSpace(txtSpendingAmount.Text) ||
                comboBoxSpending.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm bilgileri eksiksiz doldurunuz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtSpendingAmount.Text, out decimal amount))
            {
                MessageBox.Show("Geçerli bir tutar giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Models.Spendings spending = new Models.Spendings();
                spending.SpendingTitle = txtSpendingTitle.Text.Trim();
                spending.SpendingAmount = amount;
                spending.SpendingDate = dateTimeRSpending.Value;
                spending.CategoryId = Convert.ToInt32(comboBoxSpending.SelectedValue);

                db.Spendings.Add(spending);
                db.SaveChanges();

                MessageBox.Show("Harcama başarılı bir şekilde eklendi.",
                    "Harcama", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi güncelle
                var values = db.Spendings.ToList();
                dataGridView1.DataSource = values;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmBanks frm = new FrmBanks();
            frm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kategoriler frm = new Kategoriler();
            frm.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Dashboard frm = new Dashboard();
            frm.Show();
            this.Hide();
        }

        private void btnRemoveSpending_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSpendingId.Text))
            {
                MessageBox.Show("Lütfen silinecek harcamanın ID'sini giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSpendingId.Text, out int id))
            {
                MessageBox.Show("Geçerli bir harcama ID'si giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var spending = db.Spendings.Find(id);
                if (spending == null)
                {
                    MessageBox.Show("Belirtilen ID'ye sahip bir harcama bulunamadı.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                db.Spendings.Remove(spending);
                db.SaveChanges();

                MessageBox.Show("Harcama başarılı bir şekilde silindi.",
                    "Harcama", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi güncelle
                var spendings = db.Spendings.ToList();
                dataGridView1.DataSource = spendings;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateSpending_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSpendingId.Text))
            {
                MessageBox.Show("Lütfen güncellenecek harcamanın ID'sini giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtSpendingId.Text, out int id))
            {
                MessageBox.Show("Geçerli bir harcama ID'si giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtSpendingTitle.Text) ||
                string.IsNullOrWhiteSpace(txtSpendingAmount.Text) ||
                comboBoxSpending.SelectedValue == null)
            {
                MessageBox.Show("Lütfen tüm bilgileri eksiksiz doldurunuz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtSpendingAmount.Text, out decimal amount))
            {
                MessageBox.Show("Geçerli bir tutar giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var spending = db.Spendings.Find(id);
                if (spending == null)
                {
                    MessageBox.Show("Belirtilen ID'ye sahip bir harcama bulunamadı.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                spending.SpendingTitle = txtSpendingTitle.Text.Trim();
                spending.SpendingAmount = amount;
                spending.SpendingDate = dateTimeRSpending.Value;
                spending.CategoryId = Convert.ToInt32(comboBoxSpending.SelectedValue);

                db.SaveChanges();

                MessageBox.Show("Harcama başarılı bir şekilde güncellendi.",
                    "Harcama", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Listeyi güncelle
                var spendings = db.Spendings.ToList();
                dataGridView1.DataSource = spendings;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Geçerli ve satır tıklanmış mı kontrol et
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                // ID
                txtSpendingId.Text = selectedRow.Cells["SpendingId"].Value?.ToString();

                // Başlık
                txtSpendingTitle.Text = selectedRow.Cells["SpendingTitle"].Value?.ToString();

                // Tutar
                txtSpendingAmount.Text = selectedRow.Cells["SpendingAmount"].Value?.ToString();

                // Tarih
                if (DateTime.TryParse(selectedRow.Cells["SpendingDate"].Value?.ToString(), out DateTime date))
                {
                    dateTimeRSpending.Value = date;
                }

                // Kategori
                if (selectedRow.Cells["CategoryId"].Value != null)
                {
                    comboBoxSpending.SelectedValue = Convert.ToInt32(selectedRow.Cells["CategoryId"].Value);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BankProcess frm = new BankProcess();
            frm.Show();
            this.Hide();
        }
    }
}
