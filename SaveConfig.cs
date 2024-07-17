using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSubscribe
{
    public partial class SaveConfig : Form
    {
        public SaveConfig()
        {
            InitializeComponent();
            Config = new ClashConfig();
        }

        public ClashConfig Config { get; set; }

        private void SaveConfig_Load(object sender, EventArgs e)
        {
            Config = Utils.LoadConfigFile<ClashConfig>("clashSetting.json");
            Utils.ClashConfig = Config;

            if (Config!=null)
            {
                tbExePath.Text = Config.FilePath;
                tbProfile.Text = Config.ConfigPath;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {

            string title = $"打开Clash";
            string filter = "exe文件(*.exe)|*.exe|所有文件(*.*)|*.*";

            OpenFileDialog odf = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
                FilterIndex = 1
            };

            if (odf.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(odf.FileName);
                tbExePath.Text = fi.FullName;
                Config.FilePath = fi.FullName;
            }

        }

        private void btnSelectProfile_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();
            fbd.Description = "请选择配置文件的路径";
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.ShowNewFolderButton = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbProfile.Text = fbd.SelectedPath;
                Config.ConfigPath = fbd.SelectedPath;
            }

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            string json = JsonConvert.SerializeObject(Config);
            Utils.SaveConfigFile(json, "clashSetting.json");
            MessageBox.Show("保存配置成功.");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
