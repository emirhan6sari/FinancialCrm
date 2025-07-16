using System;
using System.Linq;
using System.Windows.Forms;
using FinancialCrm.Models;

namespace FinancialCrm
{
    public partial class FrmBanks : Form
    {
        FinancialCrmDbEntities db = new FinancialCrmDbEntities();

        public FrmBanks()
        {
            InitializeComponent();
        }

        private void FrmBanks_Load(object sender, EventArgs e)
        {
            try
            {
                // Banka bakiyeleri
                lblZiraatBankBalance.Text = GetBankBalance("Ziraat Bankası");
                lblVakifbankBalance.Text = GetBankBalance("Garanti BBVA");
                lblIsBankasiBalance.Text = GetBankBalance("İş Bankası");

                // Son 5 banka işlemini getir
                var lastProcesses = db.BankProcesses
                                      .OrderByDescending(x => x.BankProcessId)
                                      .Take(5)
                                      .ToList();

                SetProcessLabel(lblProcess1, lastProcesses.ElementAtOrDefault(0));
                SetProcessLabel(lblProcess2, lastProcesses.ElementAtOrDefault(1));
                SetProcessLabel(lblProcess3, lastProcesses.ElementAtOrDefault(2));
                SetProcessLabel(lblProcess4, lastProcesses.ElementAtOrDefault(3));
                SetProcessLabel(lblProcess5, lastProcesses.ElementAtOrDefault(4));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yükleme sırasında bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetBankBalance(string bankTitle)
        {
            var balance = db.Banks
                            .Where(x => x.BankTitle == bankTitle)
                            .Select(y => y.BankBalance)
                            .FirstOrDefault();

            return balance.ToString("0.00") + " ₺";
        }

        private void SetProcessLabel(Label lbl, BankProcesses process)
        {
            if (process != null)
            {
                lbl.Text = $"{process.Description_} - {process.Amount} ₺ - {process.ProcessDate:dd.MM.yyyy}";
            }
            else
            {
                lbl.Text = "Kayıt yok";
            }
        }

        // Sayfa geçiş butonları
        private void button1_Click(object sender, EventArgs e)
        {
            new Kategoriler().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Boş - istersen fonksiyon ekleyebilirsin
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new Form2().Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new Spendings().Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new BankProcess().Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new Dashboard().Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Boş - istersen fonksiyon ekleyebilirsin
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Kullanılmayan event'ler:
        private void label1_Click(object sender, EventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void panel2_Paint(object sender, PaintEventArgs e) { }
        private void panel3_Paint(object sender, PaintEventArgs e) { }
        private void panel4_Paint(object sender, PaintEventArgs e) { }
        private void panel5_Paint(object sender, PaintEventArgs e) { }
        private void lblZiraatBankBalance_Click(object sender, EventArgs e) { }
        private void lblZiraatBank_Click(object sender, EventArgs e) { }
        private void lblVakifbankBalance_Click(object sender, EventArgs e) { }
        private void lblVakifbank_Click(object sender, EventArgs e) { }
        private void lblIsBankasiBalance_Click(object sender, EventArgs e) { }
        private void lblIsBankasi_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void label10_Click(object sender, EventArgs e) { }
        private void lblProcess5_Click(object sender, EventArgs e) { }
        private void label8_Click(object sender, EventArgs e) { }
        private void lblProcess4_Click(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void lblProcess3_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void lblProcess2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void lblProcess1_Click(object sender, EventArgs e) { }
    }
}
