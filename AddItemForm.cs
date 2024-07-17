using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSubscribe
{
    public partial class AddItemForm : Form
    {
        public AddItemForm()
        {
            InitializeComponent();
        }

        private void AddItemForm_Load(object sender, EventArgs e)
        {
            tbName.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty( tbName.Text.Trim()))
            {
                tbName.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(tbAddr.Text.Trim()))
            {
                tbAddr.Focus();
                return;
            }

            var list = new List<SubscribeItem>();
            list.Add(new SubscribeItem()
            {
                Name = tbName.Text.Trim(),
                Addr = tbAddr.Text.Trim()
            });

            string json = JsonConvert.SerializeObject(list);
            Utils.SaveConfigFile(json, "subscribes.json");
            MessageBox.Show("保存配置成功.");
            Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
