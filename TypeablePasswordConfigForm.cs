using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KeePass.UI;

namespace TypeablePasswordGenerator
{
    public partial class TypeablePasswordConfigForm : Form
    {
        private TypeablePasswordConfig m_config;
        public TypeablePasswordConfigForm(TypeablePasswordConfig config)
        {
            m_config = config;
            this.Text = Properties.Strings.cfgFormTitle;
            InitializeComponent();
        }
        private void TypeablePasswordConfigForm_Load(object sender, EventArgs e)
        {
            GlobalWindowManager.AddWindow(this);
            pbBannerImage.Image = BannerFactory.CreateBanner(pbBannerImage.Width, pbBannerImage.Height, BannerStyle.Default, Properties.Images.B48x48_KGPG_Info, Properties.Strings.cfgFormHeaderTextLine1, Properties.Strings.cfgFormHeaderTextLine2);
        }

        private void UpdateConfig()
        {
            /* update the config here */
        }

        internal TypeablePasswordConfig GetConfig()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                this.UpdateConfig();
            }
            return m_config;
        }
    }
}
