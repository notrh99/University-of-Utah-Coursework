namespace ChatClient
{
    partial class ChatClient
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
            this.ServerAddress_Input = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Name_Input = new System.Windows.Forms.TextBox();
            this.Participants_Button = new System.Windows.Forms.Button();
            this.Connect_Button = new System.Windows.Forms.Button();
            this.ChatBox_Input = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ChatHistory_Display = new System.Windows.Forms.TextBox();
            this.Participants_History = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ServerAddress_Input
            // 
            this.ServerAddress_Input.Location = new System.Drawing.Point(229, 33);
            this.ServerAddress_Input.Margin = new System.Windows.Forms.Padding(2);
            this.ServerAddress_Input.Name = "ServerAddress_Input";
            this.ServerAddress_Input.Size = new System.Drawing.Size(269, 23);
            this.ServerAddress_Input.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server Name/Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Your Name";
            // 
            // Name_Input
            // 
            this.Name_Input.Location = new System.Drawing.Point(229, 84);
            this.Name_Input.Margin = new System.Windows.Forms.Padding(2);
            this.Name_Input.Name = "Name_Input";
            this.Name_Input.Size = new System.Drawing.Size(269, 23);
            this.Name_Input.TabIndex = 3;
            // 
            // Participants_Button
            // 
            this.Participants_Button.Enabled = false;
            this.Participants_Button.Location = new System.Drawing.Point(730, 238);
            this.Participants_Button.Margin = new System.Windows.Forms.Padding(2);
            this.Participants_Button.Name = "Participants_Button";
            this.Participants_Button.Size = new System.Drawing.Size(194, 22);
            this.Participants_Button.TabIndex = 4;
            this.Participants_Button.Text = "Retrieve Participants";
            this.Participants_Button.UseVisualStyleBackColor = true;
            this.Participants_Button.Click += new System.EventHandler(this.Participants_Button_Click);
            // 
            // Connect_Button
            // 
            this.Connect_Button.Location = new System.Drawing.Point(229, 136);
            this.Connect_Button.Margin = new System.Windows.Forms.Padding(2);
            this.Connect_Button.Name = "Connect_Button";
            this.Connect_Button.Size = new System.Drawing.Size(268, 32);
            this.Connect_Button.TabIndex = 5;
            this.Connect_Button.Text = "Connect To Server";
            this.Connect_Button.UseVisualStyleBackColor = true;
            this.Connect_Button.Click += new System.EventHandler(this.Connect_Button_Click);
            // 
            // ChatBox_Input
            // 
            this.ChatBox_Input.Enabled = false;
            this.ChatBox_Input.Location = new System.Drawing.Point(229, 218);
            this.ChatBox_Input.Margin = new System.Windows.Forms.Padding(2);
            this.ChatBox_Input.Name = "ChatBox_Input";
            this.ChatBox_Input.Size = new System.Drawing.Size(269, 23);
            this.ChatBox_Input.TabIndex = 6;
            this.ChatBox_Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatBox_Input_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 221);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Type Something";
            // 
            // ChatHistory_Display
            // 
            this.ChatHistory_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ChatHistory_Display.Location = new System.Drawing.Point(40, 302);
            this.ChatHistory_Display.Margin = new System.Windows.Forms.Padding(2);
            this.ChatHistory_Display.Multiline = true;
            this.ChatHistory_Display.Name = "ChatHistory_Display";
            this.ChatHistory_Display.ReadOnly = true;
            this.ChatHistory_Display.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ChatHistory_Display.Size = new System.Drawing.Size(1022, 227);
            this.ChatHistory_Display.TabIndex = 8;
            // 
            // Participants_History
            // 
            this.Participants_History.Location = new System.Drawing.Point(730, 33);
            this.Participants_History.Margin = new System.Windows.Forms.Padding(2);
            this.Participants_History.Multiline = true;
            this.Participants_History.Name = "Participants_History";
            this.Participants_History.ReadOnly = true;
            this.Participants_History.Size = new System.Drawing.Size(195, 194);
            this.Participants_History.TabIndex = 9;
            // 
            // ChatClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1110, 559);
            this.Controls.Add(this.Participants_History);
            this.Controls.Add(this.ChatHistory_Display);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ChatBox_Input);
            this.Controls.Add(this.Connect_Button);
            this.Controls.Add(this.Participants_Button);
            this.Controls.Add(this.Name_Input);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ServerAddress_Input);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ChatClient";
            this.Text = "ChatClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox ServerAddress_Input;
        private Label label1;
        private Label label2;
        private TextBox Name_Input;
        private Button Participants_Button;
        private Button Connect_Button;
        private TextBox ChatBox_Input;
        private Label label3;
        private TextBox ChatHistory_Display;
        private TextBox Participants_History;
    }
}