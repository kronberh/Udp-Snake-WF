namespace Coursework_OnlineSnake
{
    partial class MainMenuForm
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
            MainMenuTabcontrol = new TabControl();
            MultiplayerTab = new TabPage();
            ClearFieldColorsButton = new Button();
            AddFieldColorCombobox = new ComboBox();
            FieldColorsListbox = new ListBox();
            label7 = new Label();
            label6 = new Label();
            FieldSizeNumeric = new NumericUpDown();
            label4 = new Label();
            label3 = new Label();
            JoinButton = new Button();
            HostButton = new Button();
            IPTextbox = new TextBox();
            PortNumeric = new NumericUpDown();
            SingleplayerTab = new TabPage();
            CharacterTab = new TabPage();
            NameTextbox = new TextBox();
            label5 = new Label();
            ColorHexTextbox = new TextBox();
            label2 = new Label();
            SnakeColorCombobox = new ComboBox();
            label1 = new Label();
            ColorShowcasePalette = new Panel();
            StatisticsTab = new TabPage();
            BannedIPsTab = new TabPage();
            label8 = new Label();
            FoodColorCombobox = new ComboBox();
            MainMenuTabcontrol.SuspendLayout();
            MultiplayerTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FieldSizeNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PortNumeric).BeginInit();
            CharacterTab.SuspendLayout();
            SuspendLayout();
            // 
            // MainMenuTabcontrol
            // 
            MainMenuTabcontrol.Controls.Add(MultiplayerTab);
            MainMenuTabcontrol.Controls.Add(SingleplayerTab);
            MainMenuTabcontrol.Controls.Add(CharacterTab);
            MainMenuTabcontrol.Controls.Add(StatisticsTab);
            MainMenuTabcontrol.Controls.Add(BannedIPsTab);
            MainMenuTabcontrol.Dock = DockStyle.Fill;
            MainMenuTabcontrol.Location = new Point(0, 0);
            MainMenuTabcontrol.Name = "MainMenuTabcontrol";
            MainMenuTabcontrol.SelectedIndex = 0;
            MainMenuTabcontrol.Size = new Size(800, 450);
            MainMenuTabcontrol.TabIndex = 9;
            // 
            // MultiplayerTab
            // 
            MultiplayerTab.Controls.Add(FoodColorCombobox);
            MultiplayerTab.Controls.Add(label8);
            MultiplayerTab.Controls.Add(ClearFieldColorsButton);
            MultiplayerTab.Controls.Add(AddFieldColorCombobox);
            MultiplayerTab.Controls.Add(FieldColorsListbox);
            MultiplayerTab.Controls.Add(label7);
            MultiplayerTab.Controls.Add(label6);
            MultiplayerTab.Controls.Add(FieldSizeNumeric);
            MultiplayerTab.Controls.Add(label4);
            MultiplayerTab.Controls.Add(label3);
            MultiplayerTab.Controls.Add(JoinButton);
            MultiplayerTab.Controls.Add(HostButton);
            MultiplayerTab.Controls.Add(IPTextbox);
            MultiplayerTab.Controls.Add(PortNumeric);
            MultiplayerTab.Location = new Point(4, 29);
            MultiplayerTab.Name = "MultiplayerTab";
            MultiplayerTab.Padding = new Padding(3);
            MultiplayerTab.Size = new Size(792, 417);
            MultiplayerTab.TabIndex = 1;
            MultiplayerTab.Text = "Multiplayer";
            MultiplayerTab.UseVisualStyleBackColor = true;
            // 
            // ClearFieldColorsButton
            // 
            ClearFieldColorsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ClearFieldColorsButton.Location = new Point(551, 345);
            ClearFieldColorsButton.Name = "ClearFieldColorsButton";
            ClearFieldColorsButton.Size = new Size(98, 29);
            ClearFieldColorsButton.TabIndex = 17;
            ClearFieldColorsButton.Text = "Clear colors";
            ClearFieldColorsButton.UseVisualStyleBackColor = true;
            // 
            // AddFieldColorCombobox
            // 
            AddFieldColorCombobox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            AddFieldColorCombobox.FormattingEnabled = true;
            AddFieldColorCombobox.Location = new Point(551, 313);
            AddFieldColorCombobox.Name = "AddFieldColorCombobox";
            AddFieldColorCombobox.Size = new Size(98, 28);
            AddFieldColorCombobox.TabIndex = 16;
            // 
            // FieldColorsListbox
            // 
            FieldColorsListbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FieldColorsListbox.FormattingEnabled = true;
            FieldColorsListbox.Location = new Point(655, 290);
            FieldColorsListbox.Name = "FieldColorsListbox";
            FieldColorsListbox.Size = new Size(129, 84);
            FieldColorsListbox.TabIndex = 15;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new Point(551, 290);
            label7.Name = "label7";
            label7.Size = new Size(98, 20);
            label7.TabIndex = 14;
            label7.Text = "Field color(s):";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Location = new Point(551, 384);
            label6.Name = "label6";
            label6.Size = new Size(39, 20);
            label6.TabIndex = 12;
            label6.Text = "Size:";
            // 
            // FieldSizeNumeric
            // 
            FieldSizeNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FieldSizeNumeric.Location = new Point(596, 382);
            FieldSizeNumeric.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            FieldSizeNumeric.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            FieldSizeNumeric.Name = "FieldSizeNumeric";
            FieldSizeNumeric.Size = new Size(82, 27);
            FieldSizeNumeric.TabIndex = 11;
            FieldSizeNumeric.Value = new decimal(new int[] { 24, 0, 0, 0 });
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(8, 384);
            label4.Name = "label4";
            label4.Size = new Size(38, 20);
            label4.TabIndex = 10;
            label4.Text = "Port:";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(8, 350);
            label3.Name = "label3";
            label3.Size = new Size(24, 20);
            label3.TabIndex = 9;
            label3.Text = "IP:";
            // 
            // JoinButton
            // 
            JoinButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            JoinButton.Location = new Point(140, 380);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new Size(100, 29);
            JoinButton.TabIndex = 5;
            JoinButton.Text = "Join";
            JoinButton.UseVisualStyleBackColor = true;
            // 
            // HostButton
            // 
            HostButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            HostButton.Location = new Point(684, 380);
            HostButton.Name = "HostButton";
            HostButton.Size = new Size(100, 29);
            HostButton.TabIndex = 6;
            HostButton.Text = "Host";
            HostButton.UseVisualStyleBackColor = true;
            // 
            // IPTextbox
            // 
            IPTextbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            IPTextbox.Location = new Point(38, 347);
            IPTextbox.Name = "IPTextbox";
            IPTextbox.Size = new Size(202, 27);
            IPTextbox.TabIndex = 7;
            IPTextbox.Text = "127.0.0.1";
            // 
            // PortNumeric
            // 
            PortNumeric.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            PortNumeric.Location = new Point(52, 382);
            PortNumeric.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumeric.Name = "PortNumeric";
            PortNumeric.Size = new Size(82, 27);
            PortNumeric.TabIndex = 8;
            PortNumeric.Value = new decimal(new int[] { 8888, 0, 0, 0 });
            // 
            // SingleplayerTab
            // 
            SingleplayerTab.Location = new Point(4, 29);
            SingleplayerTab.Name = "SingleplayerTab";
            SingleplayerTab.Padding = new Padding(3);
            SingleplayerTab.Size = new Size(792, 417);
            SingleplayerTab.TabIndex = 0;
            SingleplayerTab.Text = "Singleplayer";
            SingleplayerTab.UseVisualStyleBackColor = true;
            // 
            // CharacterTab
            // 
            CharacterTab.Controls.Add(NameTextbox);
            CharacterTab.Controls.Add(label5);
            CharacterTab.Controls.Add(ColorHexTextbox);
            CharacterTab.Controls.Add(label2);
            CharacterTab.Controls.Add(SnakeColorCombobox);
            CharacterTab.Controls.Add(label1);
            CharacterTab.Controls.Add(ColorShowcasePalette);
            CharacterTab.Location = new Point(4, 29);
            CharacterTab.Name = "CharacterTab";
            CharacterTab.Size = new Size(792, 417);
            CharacterTab.TabIndex = 2;
            CharacterTab.Text = "Character";
            CharacterTab.UseVisualStyleBackColor = true;
            // 
            // NameTextbox
            // 
            NameTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            NameTextbox.Location = new Point(633, 3);
            NameTextbox.Name = "NameTextbox";
            NameTextbox.Size = new Size(151, 27);
            NameTextbox.TabIndex = 16;
            // 
            // label5
            // 
            label5.Location = new Point(8, 6);
            label5.Name = "label5";
            label5.Size = new Size(62, 25);
            label5.TabIndex = 15;
            label5.Text = "Name";
            // 
            // ColorHexTextbox
            // 
            ColorHexTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ColorHexTextbox.CharacterCasing = CharacterCasing.Upper;
            ColorHexTextbox.Location = new Point(683, 70);
            ColorHexTextbox.Name = "ColorHexTextbox";
            ColorHexTextbox.ReadOnly = true;
            ColorHexTextbox.Size = new Size(101, 27);
            ColorHexTextbox.TabIndex = 14;
            // 
            // label2
            // 
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(8, 36);
            label2.Name = "label2";
            label2.Size = new Size(66, 61);
            label2.TabIndex = 13;
            label2.Text = "Color";
            // 
            // SnakeColorCombobox
            // 
            SnakeColorCombobox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SnakeColorCombobox.FormattingEnabled = true;
            SnakeColorCombobox.Location = new Point(633, 36);
            SnakeColorCombobox.Name = "SnakeColorCombobox";
            SnakeColorCombobox.Size = new Size(151, 28);
            SnakeColorCombobox.TabIndex = 9;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.Font = new Font("Segoe UI", 12F);
            label1.ImageAlign = ContentAlignment.MiddleRight;
            label1.Location = new Point(659, 70);
            label1.Name = "label1";
            label1.Size = new Size(18, 27);
            label1.TabIndex = 12;
            label1.Text = "#";
            // 
            // ColorShowcasePalette
            // 
            ColorShowcasePalette.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ColorShowcasePalette.BackColor = Color.White;
            ColorShowcasePalette.BorderStyle = BorderStyle.FixedSingle;
            ColorShowcasePalette.Location = new Point(633, 70);
            ColorShowcasePalette.Name = "ColorShowcasePalette";
            ColorShowcasePalette.Size = new Size(20, 27);
            ColorShowcasePalette.TabIndex = 11;
            // 
            // StatisticsTab
            // 
            StatisticsTab.Location = new Point(4, 29);
            StatisticsTab.Name = "StatisticsTab";
            StatisticsTab.Size = new Size(792, 417);
            StatisticsTab.TabIndex = 3;
            StatisticsTab.Text = "Statistics";
            StatisticsTab.UseVisualStyleBackColor = true;
            // 
            // BannedIPsTab
            // 
            BannedIPsTab.Location = new Point(4, 29);
            BannedIPsTab.Name = "BannedIPsTab";
            BannedIPsTab.Size = new Size(792, 417);
            BannedIPsTab.TabIndex = 4;
            BannedIPsTab.Text = "Banned IPs";
            BannedIPsTab.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label8.AutoSize = true;
            label8.Location = new Point(551, 259);
            label8.Name = "label8";
            label8.Size = new Size(84, 20);
            label8.TabIndex = 18;
            label8.Text = "Food color:";
            // 
            // FoodColorCombobox
            // 
            FoodColorCombobox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            FoodColorCombobox.FormattingEnabled = true;
            FoodColorCombobox.Location = new Point(655, 256);
            FoodColorCombobox.Name = "FoodColorCombobox";
            FoodColorCombobox.Size = new Size(129, 28);
            FoodColorCombobox.TabIndex = 19;
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(MainMenuTabcontrol);
            Name = "MainMenuForm";
            Text = "MainMenu";
            MainMenuTabcontrol.ResumeLayout(false);
            MultiplayerTab.ResumeLayout(false);
            MultiplayerTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FieldSizeNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)PortNumeric).EndInit();
            CharacterTab.ResumeLayout(false);
            CharacterTab.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TabControl MainMenuTabcontrol;
        private TabPage SingleplayerTab;
        private TabPage MultiplayerTab;
        private TabPage CharacterTab;
        private TabPage StatisticsTab;
        private TabPage BannedIPsTab;
        private Label label2;
        private ComboBox SnakeColorCombobox;
        private Label label1;
        private Panel ColorShowcasePalette;
        private TextBox ColorHexTextbox;
        private Button JoinButton;
        private Button HostButton;
        private TextBox IPTextbox;
        private NumericUpDown PortNumeric;
        private Label label4;
        private Label label3;
        private Label label5;
        private TextBox NameTextbox;
        private Label label7;
        private Label label6;
        private NumericUpDown FieldSizeNumeric;
        private ListBox FieldColorsListbox;
        private Button ClearFieldColorsButton;
        private ComboBox AddFieldColorCombobox;
        private ComboBox FoodColorCombobox;
        private Label label8;
    }
}
