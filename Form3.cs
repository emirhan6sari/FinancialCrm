using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FinancialCrm.Models;

namespace FinancialCrm
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        FinancialCrmDbEntities db = new FinancialCrmDbEntities();
        int count = 0;

        private void Dashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // Toplam bakiye
                var totalBalances = db.Banks.Sum(x => (decimal?)x.BankBalance) ?? 0;
                lblTotalBalance.Text = totalBalances.ToString("N2") + " ₺";

                // Son bank işlemi tutarı
                var lastBankProcessAmount = db.BankProcesses
                    .OrderByDescending(x => x.BankProcessId)
                    .Select(y => (decimal?)y.Amount)
                    .FirstOrDefault() ?? 0;
                lblLastBankProcessAmount.Text = lastBankProcessAmount.ToString("N2") + " ₺";

                // chart1 - bankalar
                var bankData = db.Banks.Select(x => new
                {
                    x.BankTitle,
                    x.BankBalance
                }).ToList();

                chart1.Series.Clear();
                if (bankData.Any())
                {
                    var series = chart1.Series.Add("Bankalar");
                    foreach (var item in bankData)
                    {
                        series.Points.AddXY(item.BankTitle, item.BankBalance);
                    }
                }

                // chart2 - faturalar
                var billData = db.Bills.Select(x => new
                {
                    x.BillTitle,
                    x.BillAmount
                }).ToList();

                chart2.Series.Clear();
                if (billData.Any())
                {
                    var series2 = chart2.Series.Add("Faturalar");
                    series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
                    foreach (var item in billData)
                    {
                        series2.Points.AddXY(item.BillTitle, item.BillAmount);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                count++;
                if (count % 3 == 1)
                {
                    var fatura = db.Bills
                        .Where(x => x.BillTitle == "Elektrik Faturası")
                        .Select(y => (decimal?)y.BillAmount)
                        .FirstOrDefault() ?? 0;
                    lblBillTitle.Text = "Elektrik Faturası";
                    lblBillAmount.Text = fatura.ToString("N2") + " ₺";
                }
                else if (count % 3 == 2)
                {
                    var fatura = db.Bills
                        .Where(x => x.BillTitle == "Doğalgaz Faturası")
                        .Select(y => (decimal?)y.BillAmount)
                        .FirstOrDefault() ?? 0;
                    lblBillTitle.Text = "Doğalgaz Faturası";
                    lblBillAmount.Text = fatura.ToString("N2") + " ₺";
                }
                else
                {
                    var fatura = db.Bills
                        .Where(x => x.BillTitle == "Su faturası")
                        .Select(y => (decimal?)y.BillAmount)
                        .FirstOrDefault() ?? 0;
                    lblBillTitle.Text = "Su faturası";
                    lblBillAmount.Text = fatura.ToString("N2") + " ₺";
                }
            }
            catch (Exception ex)
            {
                lblBillTitle.Text = "Hata";
                lblBillAmount.Text = "-";
                MessageBox.Show("Fatura bilgileri alınırken bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
            Form2 frm = new Form2();
            frm.Show();
            this.Hide();
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

        private void button8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
