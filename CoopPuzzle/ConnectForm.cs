﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoopPuzzle
{
    public partial class ConnectForm : Form
    {
        public Game1 gameEntry;
        public ConnectForm()
        {
            InitializeComponent();
            //radBtnHost.Select();
            radBtnEdit.Select();
        }

        private void radHost_CheckedChanged(object sender, EventArgs e)
        {
            grpEdit.Visible = false;
            grpJoin.Visible = false;
            grpCredits.Visible = false;
            grpHost.Visible = true;
        }

        private void radJoin_CheckedChanged(object sender, EventArgs e)
        {
            grpEdit.Visible = false;
            grpHost.Visible = false;
            grpCredits.Visible = false;
            grpJoin.Visible = true;
        }
        private void radBtnEdit_CheckedChanged(object sender, EventArgs e)
        {
            grpHost.Visible = false;
            grpJoin.Visible = false;
            grpCredits.Visible = false;
            grpEdit.Visible = true;
        }
        private void radBtnCredits_CheckedChanged(object sender, EventArgs e)
        {
            grpHost.Visible = false;
            grpJoin.Visible = false;
            grpEdit.Visible = false;
            grpCredits.Visible = true;
        }
        private void btnHost_Click(object sender, EventArgs e)
        {
            gameEntry.port = Convert.ToInt32(txtHostPort.Text);
            gameEntry.password = txtHostPassword.Text.ToString();
            gameEntry.Host();
            Close();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            gameEntry.port = Convert.ToInt32(txtJoinPort.Text);
            gameEntry.password = txtJoinPassword.Text.ToString();
            gameEntry.ip = txtJoinIp.Text.ToString();
            gameEntry.Join();
            Close();
        }

        private void btnEditMode_Click(object sender, EventArgs e)
        {
            gameEntry.editmode= true;
            Close();
        }

        private void linkFlower_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                linkFlower.LinkVisited= true;
                Process.Start(new ProcessStartInfo() { FileName = "https://somewhatgoodroyaltyfreemusic.bandcamp.com/track/frog-shop", UseShellExecute = true });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                
            }
        }
    }
}
