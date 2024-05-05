namespace Coursework_OnlineSnake
{
    partial class JoinForm
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
            SkinTab = new TabPage();
            tabControl3 = new TabControl();
            MenuTab = new TabPage();
            GiveUpButton = new Button();
            ReviveButton = new Button();
            MessageTextbox = new TextBox();
            SendMessageButton = new Button();
            ChatTextbox = new TextBox();
            panel2 = new Panel();
            ChatTab = new TabPage();
            PlayersListbox = new ListBox();
            PlayersTab = new TabPage();
            tabControl1 = new TabControl();
            ServerTab = new TabPage();
            label2 = new Label();
            label1 = new Label();
            PortShowHideButton = new Button();
            IPShowHideButton = new Button();
            PortCopyButton = new Button();
            PortLabel = new Label();
            IPCopyButton = new Button();
            IPLabel = new Label();
            StatisticsTab = new TabPage();
            ScoreLabel = new Label();
            label3 = new Label();
            tabControl2 = new TabControl();
            tabControl3.SuspendLayout();
            MenuTab.SuspendLayout();
            panel2.SuspendLayout();
            ChatTab.SuspendLayout();
            PlayersTab.SuspendLayout();
            tabControl1.SuspendLayout();
            ServerTab.SuspendLayout();
            StatisticsTab.SuspendLayout();
            tabControl2.SuspendLayout();
            SuspendLayout();
            // 
            // SkinTab
            // 
            SkinTab.Location = new Point(4, 29);
            SkinTab.Name = "SkinTab";
            SkinTab.Padding = new Padding(3);
            SkinTab.Size = new Size(417, 130);
            SkinTab.TabIndex = 1;
            SkinTab.Text = "Skin";
            SkinTab.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            tabControl3.Controls.Add(MenuTab);
            tabControl3.Controls.Add(SkinTab);
            tabControl3.Dock = DockStyle.Bottom;
            tabControl3.Location = new Point(400, 439);
            tabControl3.Name = "tabControl3";
            tabControl3.SelectedIndex = 0;
            tabControl3.Size = new Size(425, 163);
            tabControl3.TabIndex = 6;
            // 
            // MenuTab
            // 
            MenuTab.Controls.Add(GiveUpButton);
            MenuTab.Controls.Add(ReviveButton);
            MenuTab.Location = new Point(4, 29);
            MenuTab.Name = "MenuTab";
            MenuTab.Padding = new Padding(3);
            MenuTab.Size = new Size(417, 130);
            MenuTab.TabIndex = 0;
            MenuTab.Text = "Menu";
            MenuTab.UseVisualStyleBackColor = true;
            // 
            // GiveUpButton
            // 
            GiveUpButton.Anchor = AnchorStyles.Right;
            GiveUpButton.Location = new Point(317, 53);
            GiveUpButton.Name = "GiveUpButton";
            GiveUpButton.Size = new Size(94, 29);
            GiveUpButton.TabIndex = 1;
            GiveUpButton.Text = "Give up";
            GiveUpButton.UseVisualStyleBackColor = true;
            // 
            // ReviveButton
            // 
            ReviveButton.Anchor = AnchorStyles.Left;
            ReviveButton.Enabled = false;
            ReviveButton.Location = new Point(6, 53);
            ReviveButton.Name = "ReviveButton";
            ReviveButton.Size = new Size(94, 29);
            ReviveButton.TabIndex = 0;
            ReviveButton.Text = "Revive";
            ReviveButton.UseVisualStyleBackColor = true;
            // 
            // MessageTextbox
            // 
            MessageTextbox.Dock = DockStyle.Fill;
            MessageTextbox.Location = new Point(0, 0);
            MessageTextbox.Name = "MessageTextbox";
            MessageTextbox.Size = new Size(357, 27);
            MessageTextbox.TabIndex = 0;
            // 
            // SendMessageButton
            // 
            SendMessageButton.AutoSize = true;
            SendMessageButton.Dock = DockStyle.Right;
            SendMessageButton.Location = new Point(357, 0);
            SendMessageButton.Name = "SendMessageButton";
            SendMessageButton.Size = new Size(29, 29);
            SendMessageButton.TabIndex = 1;
            SendMessageButton.Text = ">";
            SendMessageButton.UseVisualStyleBackColor = true;
            // 
            // ChatTextbox
            // 
            ChatTextbox.Dock = DockStyle.Fill;
            ChatTextbox.Location = new Point(3, 3);
            ChatTextbox.Multiline = true;
            ChatTextbox.Name = "ChatTextbox";
            ChatTextbox.ReadOnly = true;
            ChatTextbox.ScrollBars = ScrollBars.Vertical;
            ChatTextbox.Size = new Size(386, 534);
            ChatTextbox.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(MessageTextbox);
            panel2.Controls.Add(SendMessageButton);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(3, 537);
            panel2.Name = "panel2";
            panel2.Size = new Size(386, 29);
            panel2.TabIndex = 1;
            // 
            // ChatTab
            // 
            ChatTab.Controls.Add(ChatTextbox);
            ChatTab.Controls.Add(panel2);
            ChatTab.Location = new Point(4, 29);
            ChatTab.Name = "ChatTab";
            ChatTab.Padding = new Padding(3);
            ChatTab.Size = new Size(392, 569);
            ChatTab.TabIndex = 1;
            ChatTab.Text = "Chat";
            ChatTab.UseVisualStyleBackColor = true;
            // 
            // PlayersListbox
            // 
            PlayersListbox.Dock = DockStyle.Fill;
            PlayersListbox.FormattingEnabled = true;
            PlayersListbox.Location = new Point(3, 3);
            PlayersListbox.Name = "PlayersListbox";
            PlayersListbox.Size = new Size(386, 563);
            PlayersListbox.TabIndex = 0;
            // 
            // PlayersTab
            // 
            PlayersTab.Controls.Add(PlayersListbox);
            PlayersTab.Location = new Point(4, 29);
            PlayersTab.Name = "PlayersTab";
            PlayersTab.Padding = new Padding(3);
            PlayersTab.Size = new Size(392, 569);
            PlayersTab.TabIndex = 0;
            PlayersTab.Text = "Players";
            PlayersTab.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(PlayersTab);
            tabControl1.Controls.Add(ChatTab);
            tabControl1.Dock = DockStyle.Right;
            tabControl1.Location = new Point(825, 0);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(400, 602);
            tabControl1.TabIndex = 4;
            // 
            // ServerTab
            // 
            ServerTab.Controls.Add(label2);
            ServerTab.Controls.Add(label1);
            ServerTab.Controls.Add(PortShowHideButton);
            ServerTab.Controls.Add(IPShowHideButton);
            ServerTab.Controls.Add(PortCopyButton);
            ServerTab.Controls.Add(PortLabel);
            ServerTab.Controls.Add(IPCopyButton);
            ServerTab.Controls.Add(IPLabel);
            ServerTab.Location = new Point(4, 29);
            ServerTab.Name = "ServerTab";
            ServerTab.Padding = new Padding(3);
            ServerTab.Size = new Size(392, 569);
            ServerTab.TabIndex = 1;
            ServerTab.Text = "Server";
            ServerTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.Location = new Point(6, 41);
            label2.Name = "label2";
            label2.Size = new Size(40, 29);
            label2.TabIndex = 7;
            label2.Text = "Port:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.Location = new Point(6, 6);
            label1.Name = "label1";
            label1.Size = new Size(40, 29);
            label1.TabIndex = 6;
            label1.Text = "IP:";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PortShowHideButton
            // 
            PortShowHideButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PortShowHideButton.Location = new Point(325, 41);
            PortShowHideButton.Name = "PortShowHideButton";
            PortShowHideButton.Size = new Size(61, 29);
            PortShowHideButton.TabIndex = 5;
            PortShowHideButton.Text = "Show";
            PortShowHideButton.UseVisualStyleBackColor = true;
            // 
            // IPShowHideButton
            // 
            IPShowHideButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            IPShowHideButton.Location = new Point(325, 6);
            IPShowHideButton.Name = "IPShowHideButton";
            IPShowHideButton.Size = new Size(61, 29);
            IPShowHideButton.TabIndex = 4;
            IPShowHideButton.Text = "Show";
            IPShowHideButton.UseVisualStyleBackColor = true;
            // 
            // PortCopyButton
            // 
            PortCopyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            PortCopyButton.Location = new Point(267, 41);
            PortCopyButton.Name = "PortCopyButton";
            PortCopyButton.Size = new Size(52, 29);
            PortCopyButton.TabIndex = 3;
            PortCopyButton.Text = "Copy";
            PortCopyButton.UseVisualStyleBackColor = true;
            // 
            // PortLabel
            // 
            PortLabel.BackColor = Color.Black;
            PortLabel.Location = new Point(52, 41);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(209, 29);
            PortLabel.TabIndex = 2;
            PortLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // IPCopyButton
            // 
            IPCopyButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            IPCopyButton.Location = new Point(267, 6);
            IPCopyButton.Name = "IPCopyButton";
            IPCopyButton.Size = new Size(52, 29);
            IPCopyButton.TabIndex = 1;
            IPCopyButton.Text = "Copy";
            IPCopyButton.UseVisualStyleBackColor = true;
            // 
            // IPLabel
            // 
            IPLabel.BackColor = Color.Black;
            IPLabel.Location = new Point(52, 6);
            IPLabel.Name = "IPLabel";
            IPLabel.Size = new Size(209, 29);
            IPLabel.TabIndex = 0;
            IPLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StatisticsTab
            // 
            StatisticsTab.Controls.Add(ScoreLabel);
            StatisticsTab.Controls.Add(label3);
            StatisticsTab.Location = new Point(4, 29);
            StatisticsTab.Name = "StatisticsTab";
            StatisticsTab.Padding = new Padding(3);
            StatisticsTab.Size = new Size(392, 569);
            StatisticsTab.TabIndex = 0;
            StatisticsTab.Text = "Statistics";
            StatisticsTab.UseVisualStyleBackColor = true;
            // 
            // ScoreLabel
            // 
            ScoreLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ScoreLabel.Location = new Point(74, 3);
            ScoreLabel.Name = "ScoreLabel";
            ScoreLabel.Size = new Size(312, 25);
            ScoreLabel.TabIndex = 1;
            ScoreLabel.Text = "0";
            ScoreLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new Point(6, 3);
            label3.Name = "label3";
            label3.Size = new Size(62, 25);
            label3.TabIndex = 0;
            label3.Text = "Score:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tabControl2
            // 
            tabControl2.Controls.Add(StatisticsTab);
            tabControl2.Controls.Add(ServerTab);
            tabControl2.Dock = DockStyle.Left;
            tabControl2.Location = new Point(0, 0);
            tabControl2.Multiline = true;
            tabControl2.Name = "tabControl2";
            tabControl2.RightToLeft = RightToLeft.No;
            tabControl2.SelectedIndex = 0;
            tabControl2.Size = new Size(400, 602);
            tabControl2.TabIndex = 5;
            // 
            // JoinForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1225, 602);
            Controls.Add(tabControl3);
            Controls.Add(tabControl2);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "JoinForm";
            Text = "Join";
            WindowState = FormWindowState.Maximized;
            tabControl3.ResumeLayout(false);
            MenuTab.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ChatTab.ResumeLayout(false);
            ChatTab.PerformLayout();
            PlayersTab.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            ServerTab.ResumeLayout(false);
            StatisticsTab.ResumeLayout(false);
            tabControl2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabPage SkinTab;
        private TabControl tabControl3;
        private TabPage MenuTab;
        private TextBox MessageTextbox;
        private Button SendMessageButton;
        private TextBox ChatTextbox;
        private Panel panel2;
        private TabPage ChatTab;
        private ListBox PlayersListbox;
        private TabPage PlayersTab;
        private TabControl tabControl1;
        private Button GiveUpButton;
        private Button ReviveButton;
        private TabPage ServerTab;
        private Label label2;
        private Label label1;
        private Button PortShowHideButton;
        private Button IPShowHideButton;
        private Button PortCopyButton;
        private Label PortLabel;
        private Button IPCopyButton;
        private Label IPLabel;
        private TabPage StatisticsTab;
        private Label ScoreLabel;
        private Label label3;
        private TabControl tabControl2;
    }
}