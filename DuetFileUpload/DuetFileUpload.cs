using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Data;
using CamBam.CAD;
using CamBam.Geom;
using CamBam.UI;


namespace CamBamPlugIn
{
    public class DuetFileUpload
    {
        public DuetFileUpload()
        {
        }

        public static void InitPlugin(CamBamUI ui)
        {
            ToolStripMenuItem DuetUploadMenuItem = new ToolStripMenuItem();

            DuetUploadMenuItem.Text = "Upload to Duet CNC";
            DuetUploadMenuItem.Click += new EventHandler(plugin_clicked);
            ui.Menus.mnuTools.DropDownItems.Add(DuetUploadMenuItem);
        }

        public static void plugin_clicked(object sender, EventArgs e)
        {
            frmMain plugin = new frmMain();
            plugin.Owner = CamBam.ThisApplication.TopWindow;
            plugin.Show();

        }
    }
}
