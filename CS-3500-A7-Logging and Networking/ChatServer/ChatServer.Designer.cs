namespace ChatServer
{
    partial class ChatServer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.Participants_History = new System.Windows.Forms.TextBox();
            this.ServerShutDown_Button = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ServerName_Input = new System.Windows.Forms.TextBox();
            this.ServerHostName_Input = new System.Windows.Forms.TextBox();
            this.ChatBox_History = new System.Windows.Forms.TextBox();
            this.ServerPort_Input = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(60, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Participants";
            // 
            // Participants_History
            // 
            this.Participants_History.Location = new System.Drawing.Point(60, 76);
            this.Participants_History.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Participants_History.Multiline = true;
            this.Participants_History.Name = "Participants_History";
            this.Participants_History.ReadOnly = true;
            this.Participants_History.Size = new System.Drawing.Size(312, 374);
            this.Participants_History.TabIndex = 1;
            // 
            // ServerShutDown_Button
            // 
            this.ServerShutDown_Button.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ServerShutDown_Button.Location = new System.Drawing.Point(80, 597);
            this.ServerShutDown_Button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerShutDown_Button.Name = "ServerShutDown_Button";
            this.ServerShutDown_Button.Size = new System.Drawing.Size(291, 33);
            this.ServerShutDown_Button.TabIndex = 2;
            this.ServerShutDown_Button.Text = "Shutdown Server";
            this.ServerShutDown_Button.UseVisualStyleBackColor = true;
            this.ServerShutDown_Button.Click += new System.EventHandler(this.ServerShutDown_Button_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(473, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Server Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(473, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 19);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server IP Address";
            // 
            // ServerName_Input
            // 
            this.ServerName_Input.Location = new System.Drawing.Point(626, 31);
            this.ServerName_Input.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerName_Input.Name = "ServerName_Input";
            this.ServerName_Input.ReadOnly = true;
            this.ServerName_Input.Size = new System.Drawing.Size(237, 23);
            this.ServerName_Input.TabIndex = 5;
            // 
            // ServerHostName_Input
            // 
            this.ServerHostName_Input.Location = new System.Drawing.Point(626, 69);
            this.ServerHostName_Input.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerHostName_Input.Name = "ServerHostName_Input";
            this.ServerHostName_Input.ReadOnly = true;
            this.ServerHostName_Input.Size = new System.Drawing.Size(237, 23);
            this.ServerHostName_Input.TabIndex = 6;
            // 
            // ChatBox_History
            // 
            this.ChatBox_History.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatBox_History.Location = new System.Drawing.Point(473, 149);
            this.ChatBox_History.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChatBox_History.Multiline = true;
            this.ChatBox_History.Name = "ChatBox_History";
            this.ChatBox_History.ReadOnly = true;
            this.ChatBox_History.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatBox_History.Size = new System.Drawing.Size(509, 488);
            this.ChatBox_History.TabIndex = 7;
            // 
            // ServerPort_Input
            // 
            this.ServerPort_Input.Enabled = false;
            this.ServerPort_Input.Location = new System.Drawing.Point(626, 108);
            this.ServerPort_Input.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerPort_Input.Name = "ServerPort_Input";
            this.ServerPort_Input.ReadOnly = true;
            this.ServerPort_Input.Size = new System.Drawing.Size(237, 23);
            this.ServerPort_Input.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(473, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 19);
            this.label4.TabIndex = 8;
            this.label4.Text = "Server Port";
            // 
            // ChatServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 663);
            this.Controls.Add(this.ServerPort_Input);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ChatBox_History);
            this.Controls.Add(this.ServerHostName_Input);
            this.Controls.Add(this.ServerName_Input);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ServerShutDown_Button);
            this.Controls.Add(this.Participants_History);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ChatServer";
            this.Text = "ChatServer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private TextBox Participants_History;
        private Button ServerShutDown_Button;
        private Label label2;
        private Label label3;
        private TextBox ServerName_Input;
        private TextBox ServerHostName_Input;
        private TextBox ChatBox_History;
        private TextBox ServerPort_Input;
        private Label label4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}