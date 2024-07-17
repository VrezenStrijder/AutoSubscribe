using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSubscribe
{
    public partial class Form1 : Form
    {
        private List<string> addrs = new List<string>
        {
            "OpenRunner",
            "NodeFree",
            "Mfuu",
            "Anaer",
            "Xrayfree",
            "Ermaozi",
            "anaer2",
            "Learnhard-cn",
            "Tbbatbb"
        };

        private ClashConfig cfg;

        public Form1()
        {
            InitializeComponent();
            Today = DateTime.Now.Date;
            Text = $"自动订阅  {Today.ToString("yyyy-MM-dd")}";
        }

        public DateTime Today { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAddress();
            LoadConfig();
        }

        private void LoadAddress()
        {
            lbAddr.Items.Clear();

            foreach (var item in addrs)
            {
                lbAddr.Items.Add(item);
            }

        }

        private void LoadConfig()
        {
            cfg = Utils.LoadConfigFile<ClashConfig>("clashSetting.json");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            cfg = cfg ?? Utils.ClashConfig;

            if (cfg == null)
            {
                MessageBox.Show("未配置保存路径");
                return;
            }

            ClearLogs();

            string dateStr = Today.ToString("yyyyMMdd");

            var phs = new ThreadStart(DownloadFiles);
            Thread th = new Thread(phs);
            th.Start();
        }

        private void DownloadFiles()
        {
            for (int i = 0; i < addrs.Count; i++)
            {
                string name = addrs[i];

                StartDownload(name);

                Thread.Sleep(100);
            }

            EndInfo();
        }


        private void btnUpdateOne_Click(object sender, EventArgs e)
        {
            cfg = cfg ?? Utils.ClashConfig;

            if (cfg == null)
            {
                MessageBox.Show("未配置保存路径");
                return;
            }

            if (lbAddr.SelectedIndex < 0)
            {
                return;
            }

            ClearLogs();

            var phs = new ThreadStart(DownloadOne);
            Thread th = new Thread(phs);
            th.Start();

        }

        private void DownloadOne()
        {
            string name = lbAddr.SelectedItem.ToString();

            if (!string.IsNullOrEmpty(name))
            {
                StartDownload(name);
                EndInfo();
            }
        }


        private void btnYesterday_Click(object sender, EventArgs e)
        {
            Today = DateTime.Now.AddDays(-1).Date;
            Text = $"自动订阅  {Today.ToString("yyyy-MM-dd")}";

        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var frm = new SaveConfig();
            frm.ShowDialog();
        }

        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(cfg.ConfigPath);
        }

        private void StartDownload(string name)
        {
            string dateStr = Today.ToString("yyyyMMdd");

            string addr = string.Empty;
            switch (name)
            {
                case "OpenRunner":
                    addr = $"https://freenode.openrunner.net/uploads/{dateStr}-clash.yaml";
                    break;
                case "NodeFree":
                    addr = $"https://nodefree.org/dy/{Today.ToString("yyyy")}/{Today.ToString("MM")}/{dateStr}.yaml";
                    break;
                case "Mfuu":
                    addr = " https://raw.githubusercontent.com/mfuu/v2ray/master/clash.yaml";
                    break;
                case "Anaer":
                    addr = "https://raw.githubusercontent.com/anaer/Sub/main/clash.yaml";
                    break;
                case "Xrayfree":
                    addr = "https://tt.vg/freeclash";
                    break;
                case "Ermaozi":
                    addr = "https://raw.githubusercontent.com/ermaozi/get_subscribe/main/subscribe/clash.yml";
                    break;
                case "anaer2":
                    addr = "https://jsd.onmicrosoft.cn/gh/anaer/Sub@main/clash.yaml";
                    break;
                case "Learnhard-cn":
                    addr = "https://cdn.jsdelivr.net/gh/learnhard-cn/free_proxy_ss@main/clash/clash.provider.yaml";
                    break;
                case "Tbbatbb":
                    addr = "https://raw.githubusercontent.com/tbbatbb/Proxy/master/dist/clash.config.yaml";
                    break;

            }

            StartInfo(name, addr);

            bool urlOk = ValidateUrl(addr);
            if (urlOk && cfg != null)
            {
                string saveFile = Path.Combine(cfg.ConfigPath, $"{name}.yaml");
                DownloadFile(addr, saveFile);
            }
        }

        private void StartInfo(string name, string addr)
        {
            string divide = $"--------------------------------------------------";
            string info = $"开始下载: [{name}]";
            string info2 = $"连接[{addr}] ...";

            AddInfo(divide);
            AddInfo(info);
            AddInfo(info2);
        }

        private void EndInfo()
        {
            string info = $"自动更新完成.";
            string divide = $"--------------------------------------------------";


            AddInfo(info);
            AddInfo(divide);
        }


        private void AddInfo(string info)
        {
            Invoke(new Action(() =>
            {
                lbLogs.Items.Add(info);
            }));
        }

        private void ClearLogs()
        {
            Invoke(new Action(() =>
            {
                lbLogs.Items.Clear();
            }));
        }


        private bool ValidateUrl(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException ex)
            {
                AddInfo($"URL验证失败:{ex.Message}");
                return false;
            }
        }

        private void DownloadFile(string url, string filePath)
        {
            int maxRetries = 3;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(url, filePath);
                        AddInfo($"文件已成功下载到：{filePath}");
                        return;
                    }
                }
                catch (WebException ex)
                {
                    retryCount++;
                    AddInfo($"下载失败,重试次数：{retryCount}/{maxRetries}. 错误：{ex.Message}");
                }
            }

            AddInfo("下载失败,达到最大重试次数.");
        }


    }
}
