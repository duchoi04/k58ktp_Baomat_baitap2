using LoveApp.Lib;
using System;
using System.Windows.Forms;

namespace LoveApp.WinFrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Nếu cần chạy gì khi Form load thì viết ở đây
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            LoveCalculator calc = new LoveCalculator();
            int score = calc.CalculateLove(txtName1.Text, txtName2.Text);
            string message = calc.GetMessage(score);

            lblResult.Text = $"Tỷ lệ: {score}% - {message}";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Nếu label1 cần sự kiện click thì viết ở đây
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Nếu có nút button1 riêng thì xử lý ở đây
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Nếu có xử lý khi textBox1 thay đổi thì viết ở đây
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Nếu label3 cần sự kiện click thì viết ở đây
        }
    }
}
