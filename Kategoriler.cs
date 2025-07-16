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
    public partial class Kategoriler : Form
    {
        public Kategoriler()
        {
            InitializeComponent();
        }
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();

        private void btnCategoryList_Click(object sender, EventArgs e)
        {
            var values =db.Categories.ToList();
            dataGridView1.DataSource = values;
        }

        private void Kategoriler_Load(object sender, EventArgs e)
        {
            var values = db.Categories.ToList();
            dataGridView1.DataSource = values;
        }

        private void btnCreateCategory_Click(object sender, EventArgs e)
        {
            string nameCategories = txtCategoryName.Text.Trim();

            if (string.IsNullOrWhiteSpace(nameCategories))
            {
                MessageBox.Show("Lütfen kategori adını giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Categories kategori = new Categories();
                kategori.CategoryName = nameCategories;

                db.Categories.Add(kategori);
                db.SaveChanges();

                MessageBox.Show("Kategori başarılı bir şekilde eklendi.",
                    "Kategoriler", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Ekleme sonrası güncel listeyi yükle
                var values = db.Categories.ToList();
                dataGridView1.DataSource = values;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryId.Text))
            {
                MessageBox.Show("Lütfen silinecek kategori ID'sini giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCategoryId.Text, out int id))
            {
                MessageBox.Show("Geçerli bir kategori ID'si giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var removeValue = db.Categories.Find(id);
            if (removeValue == null)
            {
                MessageBox.Show("Belirtilen ID'ye sahip bir kategori bulunamadı.",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                db.Categories.Remove(removeValue);
                db.SaveChanges();

                MessageBox.Show("Kategori başarılı bir şekilde sistemden silindi.",
                    "Kategoriler", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Silme sonrası güncel listeyi yükle
                var values = db.Categories.ToList();
                dataGridView1.DataSource = values;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Başlık satırı değilse
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                txtCategoryId.Text = row.Cells["CategoryId"].Value.ToString();
                txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
                
            }
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryId.Text))
            {
                MessageBox.Show("Lütfen güncellenecek kategori ID'sini giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtCategoryId.Text, out int id))
            {
                MessageBox.Show("Geçerli bir kategori ID'si giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newCategoryName = txtCategoryName.Text.Trim();
            if (string.IsNullOrWhiteSpace(newCategoryName))
            {
                MessageBox.Show("Lütfen yeni kategori adını giriniz.",
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var existingCategory = db.Categories.Find(id);
                if (existingCategory == null)
                {
                    MessageBox.Show("Belirtilen ID'ye sahip bir kategori bulunamadı.",
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                existingCategory.CategoryName = newCategoryName;
                db.SaveChanges();

                MessageBox.Show("Kategori başarılı bir şekilde güncellendi.",
                    "Kategoriler", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Güncelleme sonrası güncel listeyi yükle
                var values = db.Categories.ToList();
                dataGridView1.DataSource = values;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmBanks frm = new FrmBanks();
            frm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Spendings frm = new Spendings();
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

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BankProcess frm = new BankProcess();
            frm.Show();
            this.Hide();
        }
    }
}
