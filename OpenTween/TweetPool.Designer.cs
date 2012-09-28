namespace OpenTween
{
    partial class TweetPool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxTweet = new System.Windows.Forms.TextBox();
            this.labelMsg = new System.Windows.Forms.Label();
            this.buttonPost = new System.Windows.Forms.Button();
            this.labelHavePhoto = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxTweet
            // 
            this.textBoxTweet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTweet.Location = new System.Drawing.Point(12, 24);
            this.textBoxTweet.Multiline = true;
            this.textBoxTweet.Name = "textBoxTweet";
            this.textBoxTweet.Size = new System.Drawing.Size(260, 82);
            this.textBoxTweet.TabIndex = 0;
            this.textBoxTweet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxTweet_KeyDown);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Location = new System.Drawing.Point(12, 9);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(26, 12);
            this.labelMsg.TabIndex = 1;
            this.labelMsg.Text = "Msg";
            // 
            // buttonPost
            // 
            this.buttonPost.Location = new System.Drawing.Point(197, 112);
            this.buttonPost.Name = "buttonPost";
            this.buttonPost.Size = new System.Drawing.Size(75, 23);
            this.buttonPost.TabIndex = 2;
            this.buttonPost.Text = "Post";
            this.buttonPost.UseVisualStyleBackColor = true;
            this.buttonPost.Click += new System.EventHandler(this.buttonPost_Click);
            // 
            // labelHavePhoto
            // 
            this.labelHavePhoto.AutoSize = true;
            this.labelHavePhoto.Location = new System.Drawing.Point(12, 117);
            this.labelHavePhoto.Name = "labelHavePhoto";
            this.labelHavePhoto.Size = new System.Drawing.Size(48, 12);
            this.labelHavePhoto.TabIndex = 4;
            this.labelHavePhoto.Text = "画像なし";
            // 
            // TweetPool
            // 
            this.AcceptButton = this.buttonPost;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 147);
            this.Controls.Add(this.labelHavePhoto);
            this.Controls.Add(this.buttonPost);
            this.Controls.Add(this.labelMsg);
            this.Controls.Add(this.textBoxTweet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "TweetPool";
            this.Text = "ツイートは保留されています";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.TweetRetry_Load);
            this.Shown += new System.EventHandler(this.auto_buttonPost_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxTweet;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Button buttonPost;
        private System.Windows.Forms.Label labelHavePhoto;
    }
}