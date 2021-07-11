using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace CamBamPlugIn
{
    public partial class DemoForm : Form
    {
        public DemoForm()
        {
            InitializeComponent();
            lbl_units.Text="Current Drawing Units are "+(CamBam.UI.CamBamUI.MainUI.CADFileTree.CADFile.DrawingUnits.ToString());
        }
    }
}