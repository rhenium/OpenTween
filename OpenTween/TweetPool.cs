// OpenTween - Client of Twitter
// Copyright (c) 2012      re4k (@re4k) <http://re4k.info/>
// All rights reserved.
//
// This file is part of OpenTween.
//
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
// for more details.
//
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace OpenTween
{
    public partial class TweetPool : Form
    {
        private string tweet;
        private string msg = "";
        private long reply_to;
        private FileInfo mediaFile;
        private Twitter ppc;
        public TweenMain mainForm;

        public TweetPool()
        {
            InitializeComponent();
        }

        public void Set(string msg, string tweet, long reply_to, FileInfo mediaFile, Twitter ppc)
        {
            this.msg = msg;
            this.tweet = tweet.Replace("\r", "").Replace("\n", "\r\n");
            this.reply_to = reply_to;
            this.mediaFile = mediaFile;
            this.ppc = ppc;
        }

        private void TweetRetry_Load(object sender, EventArgs e)
        {
            this.textBoxTweet.Text = tweet;
            this.labelMsg.Text = msg;
            if (this.mediaFile != null)
                this.labelHavePhoto.Text = "Twitter Official";
            this.mainForm.Activate();
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ParameterizedThreadStart(worker));
            t.Start(new object[] { this.textBoxTweet.Text, this.reply_to, this.mediaFile });
            this.Close();
        }

        private void worker(object param)
        {
            string tweet = (string)((object[])param)[0];
            long reply_to = (long)((object[])param)[1];
            FileInfo mf = (FileInfo)((object[])param)[2];

            this.ppc.PostStatusRetry(tweet.Replace("\r\n", "\n"), reply_to, false, this.mainForm, mediaFile);
        }

        private void textBoxTweet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                textBoxTweet.SelectAll();
            }
        }
    }
}
