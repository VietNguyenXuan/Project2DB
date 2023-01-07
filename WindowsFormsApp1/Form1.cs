using McProtocol;
using McProtocol.Mitsubishi;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Đưa con trỏ tại Ip khi bắt đầu chạy chương trình
            this.ActiveControl = textBox_ip1;
            textBox_ip1.Focus();
        }

        private void textBox_ip1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox_ip2.Focus();
        }

        private void textBox_ip2_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox_ip3.Focus();
        }

        private void textBox_ip3_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox_ip4.Focus();
        }

        private void textBox_ip4_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                textBox_port.Focus();
        }

        McProtocolTcp mcProtocolTcp;
        private async void button_connect_Click_1(object sender, EventArgs e)
        {
            int port = Convert.ToInt32(textBox_port.Text);
            string iP = textBox_ip1.Text + "." +
                textBox_ip2.Text + "." +
                textBox_ip3.Text + "." +
                textBox_ip4.Text;
            
            mcProtocolTcp = new McProtocolTcp(iP, port, McFrame.MC3E);
            await mcProtocolTcp.Open();

            if (mcProtocolTcp.Connected == true)
            {
                MessageBox.Show("Connect Successful!");
                timer1.Start();
                timer2.Start();
            }
        }

        string[] id_register = {"3000", "3001", "3002", "3003", "3004", "3005", "3006", "3007", "3008", "3009" };
        int[] value_register = new int[10];


        List<dataCollect> data_collect;
        private async void button_write_Click_1(object sender, EventArgs e)
        {
            int data;
            string register;

            DialogResult result = MessageBox.Show("Do you want to write data ?", "Question",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1,
                MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.OK)
            {     
                if (comboBox_write.SelectedItem != null)
                {
                    if (textBox_value_write.Text.Length != 0)
                    {
                        if (Int32.TryParse(textBox_value_write.Text, out data))
                        {
                            data = int.Parse(textBox_value_write.Text);

                            value_register[comboBox_write.SelectedIndex] = data;

                            await mcProtocolTcp.WriteDeviceBlock("D3000", 10, value_register);
                            MessageBox.Show("Complete");
                        }
                        else
                            MessageBox.Show("Vui lòng nhập giá trị số", "Error");
                    }
                    else
                        MessageBox.Show("Vui lòng nhập giá trị", "Error");
                }
            } 
        }

        private async void button_read_Click_1(object sender, EventArgs e)
        {
            if (comboBox_read.SelectedItem != null)
            {
                var oDataNew1 = await mcProtocolTcp.ReadDeviceBlock("D3000", 10);

                for (int i = 0; i < oDataNew1.Length; i++)
                    value_register[i] = oDataNew1[i];

                textBox_value_read.Text = value_register[comboBox_read.SelectedIndex].ToString();

                data_collect = new List<dataCollect>()
                {
                    new dataCollect(){STT = 1, Register = "D3000", Value=oDataNew1[0]},
                    new dataCollect(){STT = 2, Register = "D3001", Value=oDataNew1[1]},
                    new dataCollect(){STT = 3, Register = "D3002", Value=oDataNew1[2]},
                    new dataCollect(){STT = 4, Register = "D3003", Value=oDataNew1[3]},
                    new dataCollect(){STT = 5, Register = "D3004", Value=oDataNew1[4]},
                    new dataCollect(){STT = 6, Register = "D3005", Value=oDataNew1[5]},
                    new dataCollect(){STT = 7, Register = "D3006", Value=oDataNew1[6]},
                    new dataCollect(){STT = 8, Register = "D3007", Value=oDataNew1[7]},
                    new dataCollect(){STT = 9, Register = "D3008", Value=oDataNew1[8]},
                    new dataCollect(){STT = 10, Register = "D3009", Value=oDataNew1[9]},
                };

               

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            button_status.Visible = !button_status.Visible;
        }

        private async void timer2_Tick(object sender, EventArgs e)
        {
            if (comboBox_read.SelectedItem != null )
            {
                var oDataNew1 = await mcProtocolTcp.ReadDeviceBlock("D3000", 10);

                textBox_value_read.Text = oDataNew1[comboBox_read.SelectedIndex].ToString();
                
                data_collect = new List<dataCollect>()
                {
                    new dataCollect(){STT = 1, Register = "D3000", Value=oDataNew1[0]},
                    new dataCollect(){STT = 2, Register = "D3001", Value=oDataNew1[1]},
                    new dataCollect(){STT = 3, Register = "D3002", Value=oDataNew1[2]},
                    new dataCollect(){STT = 4, Register = "D3003", Value=oDataNew1[3]},
                    new dataCollect(){STT = 5, Register = "D3004", Value=oDataNew1[4]},
                    new dataCollect(){STT = 6, Register = "D3005", Value=oDataNew1[5]},
                    new dataCollect(){STT = 7, Register = "D3006", Value=oDataNew1[6]},
                    new dataCollect(){STT = 8, Register = "D3007", Value=oDataNew1[7]},
                    new dataCollect(){STT = 9, Register = "D3008", Value=oDataNew1[8]},
                    new dataCollect(){STT = 10, Register = "D3009", Value=oDataNew1[9]},
                };

                dataGridView1.DataSource = data_collect;
                
            }
        }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
  }

  public class dataCollect
    {
        public int STT { get; set; }
        public string Register { get; set; }
        public float Value { get; set; }
    }
}
