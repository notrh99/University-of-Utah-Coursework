namespace ClientGUI
{
    partial class AgarioClient
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
            this.components = new System.ComponentModel.Container();
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.HostnameLabel = new System.Windows.Forms.Label();
            this.PlayerNameInput = new System.Windows.Forms.TextBox();
            this.HostnameInput = new System.Windows.Forms.TextBox();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label_pos = new System.Windows.Forms.Label();
            this.FPS_Label = new System.Windows.Forms.Label();
            this.HPS_Label = new System.Windows.Forms.Label();
            this.IncPacket_Label = new System.Windows.Forms.Label();
            this.Food_Label = new System.Windows.Forms.Label();
            this.Mass_Label = new System.Windows.Forms.Label();
            this.MousePosition_Label = new System.Windows.Forms.Label();
            this.Position_Label = new System.Windows.Forms.Label();
            this.PlayerNameInput_Connected = new System.Windows.Forms.TextBox();
            this.PlayerNameLabel_Connected = new System.Windows.Forms.Label();
            this.ErrorProvider_InputValidation = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider_InputValidation)).BeginInit();
            this.SuspendLayout();
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNameLabel.Location = new System.Drawing.Point(208, 232);
            this.PlayerNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(99, 21);
            this.PlayerNameLabel.TabIndex = 0;
            this.PlayerNameLabel.Text = "Player Name";
            // 
            // HostnameLabel
            // 
            this.HostnameLabel.AutoSize = true;
            this.HostnameLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HostnameLabel.Location = new System.Drawing.Point(208, 328);
            this.HostnameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HostnameLabel.Name = "HostnameLabel";
            this.HostnameLabel.Size = new System.Drawing.Size(55, 21);
            this.HostnameLabel.TabIndex = 1;
            this.HostnameLabel.Text = "Server";
            // 
            // PlayerNameInput
            // 
            this.PlayerNameInput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNameInput.Location = new System.Drawing.Point(347, 232);
            this.PlayerNameInput.Margin = new System.Windows.Forms.Padding(4);
            this.PlayerNameInput.Name = "PlayerNameInput";
            this.PlayerNameInput.Size = new System.Drawing.Size(407, 29);
            this.PlayerNameInput.TabIndex = 2;
            this.PlayerNameInput.Text = "What is your name?";
            this.PlayerNameInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PlayerNameInput_KeyDown);
            // 
            // HostnameInput
            // 
            this.HostnameInput.BackColor = System.Drawing.Color.Cyan;
            this.HostnameInput.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HostnameInput.Location = new System.Drawing.Point(347, 328);
            this.HostnameInput.Margin = new System.Windows.Forms.Padding(4);
            this.HostnameInput.Name = "HostnameInput";
            this.HostnameInput.Size = new System.Drawing.Size(407, 29);
            this.HostnameInput.TabIndex = 3;
            this.HostnameInput.Text = "localhost";
            this.HostnameInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HostnameInput_KeyDown);
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.BackColor = System.Drawing.Color.Cyan;
            this.ErrorLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ErrorLabel.Location = new System.Drawing.Point(85, 734);
            this.ErrorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(178, 21);
            this.ErrorLabel.TabIndex = 4;
            this.ErrorLabel.Text = "Error Messages Go Here";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(798, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "FPS";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(798, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "HPS";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(798, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = "Total Packets:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label6.Location = new System.Drawing.Point(798, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 15);
            this.label6.TabIndex = 8;
            this.label6.Text = "Food";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label7.Location = new System.Drawing.Point(798, 168);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(34, 15);
            this.label7.TabIndex = 9;
            this.label7.Text = "Mass";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label8.Location = new System.Drawing.Point(773, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 15);
            this.label8.TabIndex = 10;
            this.label8.Text = "Mouse Position";
            // 
            // label_pos
            // 
            this.label_pos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_pos.AutoSize = true;
            this.label_pos.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_pos.Location = new System.Drawing.Point(798, 235);
            this.label_pos.Name = "label_pos";
            this.label_pos.Size = new System.Drawing.Size(50, 15);
            this.label_pos.TabIndex = 11;
            this.label_pos.Text = "Position";
            // 
            // FPS_Label
            // 
            this.FPS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FPS_Label.AutoSize = true;
            this.FPS_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FPS_Label.Location = new System.Drawing.Point(886, 32);
            this.FPS_Label.Name = "FPS_Label";
            this.FPS_Label.Size = new System.Drawing.Size(13, 15);
            this.FPS_Label.TabIndex = 12;
            this.FPS_Label.Text = "0";
            // 
            // HPS_Label
            // 
            this.HPS_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HPS_Label.AutoSize = true;
            this.HPS_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.HPS_Label.Location = new System.Drawing.Point(886, 61);
            this.HPS_Label.Name = "HPS_Label";
            this.HPS_Label.Size = new System.Drawing.Size(99, 15);
            this.HPS_Label.TabIndex = 13;
            this.HPS_Label.Text = "0 HBs Per Second";
            // 
            // IncPacket_Label
            // 
            this.IncPacket_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IncPacket_Label.AutoSize = true;
            this.IncPacket_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.IncPacket_Label.Location = new System.Drawing.Point(886, 91);
            this.IncPacket_Label.Name = "IncPacket_Label";
            this.IncPacket_Label.Size = new System.Drawing.Size(13, 15);
            this.IncPacket_Label.TabIndex = 14;
            this.IncPacket_Label.Text = "0";
            // 
            // Food_Label
            // 
            this.Food_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Food_Label.AutoSize = true;
            this.Food_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Food_Label.Location = new System.Drawing.Point(886, 138);
            this.Food_Label.Name = "Food_Label";
            this.Food_Label.Size = new System.Drawing.Size(13, 15);
            this.Food_Label.TabIndex = 15;
            this.Food_Label.Text = "0";
            // 
            // Mass_Label
            // 
            this.Mass_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Mass_Label.AutoSize = true;
            this.Mass_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Mass_Label.Location = new System.Drawing.Point(886, 168);
            this.Mass_Label.Name = "Mass_Label";
            this.Mass_Label.Size = new System.Drawing.Size(13, 15);
            this.Mass_Label.TabIndex = 16;
            this.Mass_Label.Text = "0";
            // 
            // MousePosition_Label
            // 
            this.MousePosition_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MousePosition_Label.AutoSize = true;
            this.MousePosition_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MousePosition_Label.Location = new System.Drawing.Point(886, 201);
            this.MousePosition_Label.Name = "MousePosition_Label";
            this.MousePosition_Label.Size = new System.Drawing.Size(22, 15);
            this.MousePosition_Label.TabIndex = 17;
            this.MousePosition_Label.Text = "0,0";
            // 
            // Position_Label
            // 
            this.Position_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Position_Label.AutoSize = true;
            this.Position_Label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Position_Label.Location = new System.Drawing.Point(886, 235);
            this.Position_Label.Name = "Position_Label";
            this.Position_Label.Size = new System.Drawing.Size(22, 15);
            this.Position_Label.TabIndex = 18;
            this.Position_Label.Text = "0,0";
            // 
            // PlayerNameInput_Connected
            // 
            this.PlayerNameInput_Connected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayerNameInput_Connected.Enabled = false;
            this.PlayerNameInput_Connected.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNameInput_Connected.Location = new System.Drawing.Point(761, 726);
            this.PlayerNameInput_Connected.Margin = new System.Windows.Forms.Padding(4);
            this.PlayerNameInput_Connected.Name = "PlayerNameInput_Connected";
            this.PlayerNameInput_Connected.Size = new System.Drawing.Size(266, 29);
            this.PlayerNameInput_Connected.TabIndex = 20;
            this.PlayerNameInput_Connected.Text = "What is your name?";
            this.PlayerNameInput_Connected.Visible = false;
            this.PlayerNameInput_Connected.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PlayerNameInput_Connected_KeyDown);
            // 
            // PlayerNameLabel_Connected
            // 
            this.PlayerNameLabel_Connected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PlayerNameLabel_Connected.AutoSize = true;
            this.PlayerNameLabel_Connected.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.PlayerNameLabel_Connected.Location = new System.Drawing.Point(654, 729);
            this.PlayerNameLabel_Connected.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlayerNameLabel_Connected.Name = "PlayerNameLabel_Connected";
            this.PlayerNameLabel_Connected.Size = new System.Drawing.Size(99, 21);
            this.PlayerNameLabel_Connected.TabIndex = 19;
            this.PlayerNameLabel_Connected.Text = "Player Name";
            this.PlayerNameLabel_Connected.Visible = false;
            // 
            // ErrorProvider_InputValidation
            // 
            this.ErrorProvider_InputValidation.ContainerControl = this;
            // 
            // AgarioClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 781);
            this.Controls.Add(this.PlayerNameInput_Connected);
            this.Controls.Add(this.PlayerNameLabel_Connected);
            this.Controls.Add(this.Position_Label);
            this.Controls.Add(this.MousePosition_Label);
            this.Controls.Add(this.Mass_Label);
            this.Controls.Add(this.Food_Label);
            this.Controls.Add(this.IncPacket_Label);
            this.Controls.Add(this.HPS_Label);
            this.Controls.Add(this.FPS_Label);
            this.Controls.Add(this.label_pos);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ErrorLabel);
            this.Controls.Add(this.HostnameInput);
            this.Controls.Add(this.PlayerNameInput);
            this.Controls.Add(this.HostnameLabel);
            this.Controls.Add(this.PlayerNameLabel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AgarioClient";
            this.Text = "Agario";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AgarioClient_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider_InputValidation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label PlayerNameLabel;
        private Label HostnameLabel;
        private TextBox PlayerNameInput;
        private TextBox HostnameInput;
        private Label ErrorLabel;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label_pos;
        private Label FPS_Label;
        private Label HPS_Label;
        private Label IncPacket_Label;
        private Label Food_Label;
        private Label Mass_Label;
        private Label MousePosition_Label;
        private Label Position_Label;
        private TextBox PlayerNameInput_Connected;
        private Label PlayerNameLabel_Connected;
        private ErrorProvider ErrorProvider_InputValidation;
    }
}