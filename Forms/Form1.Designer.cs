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
            JoinButton = new Button();
            SingleplayerButton = new Button();
            HostButton = new Button();
            IPTextbox = new TextBox();
            PortNumeric = new NumericUpDown();
            ColorsCombobox = new ComboBox();
            ColorhexMaskedtextbox = new MaskedTextBox();
            ColorShowcasePalette = new Panel();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)PortNumeric).BeginInit();
            SuspendLayout();
            // 
            // JoinButton
            // 
            JoinButton.Anchor = AnchorStyles.None;
            JoinButton.Location = new Point(336, 191);
            JoinButton.Name = "JoinButton";
            JoinButton.Size = new Size(100, 29);
            JoinButton.TabIndex = 1;
            JoinButton.Text = "Join";
            JoinButton.UseVisualStyleBackColor = true;
            // 
            // SingleplayerButton
            // 
            SingleplayerButton.Anchor = AnchorStyles.None;
            SingleplayerButton.Location = new Point(126, 191);
            SingleplayerButton.Name = "SingleplayerButton";
            SingleplayerButton.Size = new Size(100, 29);
            SingleplayerButton.TabIndex = 0;
            SingleplayerButton.Text = "Singleplayer";
            SingleplayerButton.UseVisualStyleBackColor = true;
            // 
            // HostButton
            // 
            HostButton.Anchor = AnchorStyles.None;
            HostButton.Location = new Point(550, 191);
            HostButton.Name = "HostButton";
            HostButton.Size = new Size(100, 29);
            HostButton.TabIndex = 2;
            HostButton.Text = "Host";
            HostButton.UseVisualStyleBackColor = true;
            // 
            // IPTextbox
            // 
            IPTextbox.Anchor = AnchorStyles.None;
            IPTextbox.Location = new Point(301, 157);
            IPTextbox.Name = "IPTextbox";
            IPTextbox.Size = new Size(125, 27);
            IPTextbox.TabIndex = 3;
            IPTextbox.Text = "127.0.0.1";
            // 
            // PortNumeric
            // 
            PortNumeric.Anchor = AnchorStyles.None;
            PortNumeric.Increment = new decimal(new int[] { 0, 0, 0, 0 });
            PortNumeric.Location = new Point(432, 158);
            PortNumeric.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumeric.Name = "PortNumeric";
            PortNumeric.Size = new Size(69, 27);
            PortNumeric.TabIndex = 4;
            PortNumeric.Value = new decimal(new int[] { 8888, 0, 0, 0 });
            // 
            // ColorsCombobox
            // 
            ColorsCombobox.Anchor = AnchorStyles.None;
            ColorsCombobox.FormattingEnabled = true;
            ColorsCombobox.Location = new Point(318, 12);
            ColorsCombobox.Name = "ColorsCombobox";
            ColorsCombobox.Size = new Size(151, 28);
            ColorsCombobox.TabIndex = 5;
            // 
            // ColorhexMaskedtextbox
            // 
            ColorhexMaskedtextbox.Anchor = AnchorStyles.None;
            ColorhexMaskedtextbox.Location = new Point(368, 46);
            ColorhexMaskedtextbox.Name = "ColorhexMaskedtextbox";
            ColorhexMaskedtextbox.ReadOnly = true;
            ColorhexMaskedtextbox.Size = new Size(101, 27);
            ColorhexMaskedtextbox.TabIndex = 6;
            // 
            // ColorShowcasePalette
            // 
            ColorShowcasePalette.Anchor = AnchorStyles.None;
            ColorShowcasePalette.BackColor = Color.White;
            ColorShowcasePalette.BorderStyle = BorderStyle.FixedSingle;
            ColorShowcasePalette.Location = new Point(318, 46);
            ColorShowcasePalette.Name = "ColorShowcasePalette";
            ColorShowcasePalette.Size = new Size(20, 27);
            ColorShowcasePalette.TabIndex = 7;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.None;
            label1.Font = new Font("Segoe UI", 12F);
            label1.ImageAlign = ContentAlignment.MiddleRight;
            label1.Location = new Point(344, 46);
            label1.Name = "label1";
            label1.Size = new Size(18, 27);
            label1.TabIndex = 8;
            label1.Text = "#";
            // 
            // MainMenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(ColorShowcasePalette);
            Controls.Add(ColorhexMaskedtextbox);
            Controls.Add(ColorsCombobox);
            Controls.Add(PortNumeric);
            Controls.Add(IPTextbox);
            Controls.Add(HostButton);
            Controls.Add(JoinButton);
            Controls.Add(SingleplayerButton);
            Name = "MainMenuForm";
            Text = "MainMenu";
            ((System.ComponentModel.ISupportInitialize)PortNumeric).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button JoinButton;
        private Button SingleplayerButton;
        private Button HostButton;
        private TextBox IPTextbox;
        private NumericUpDown PortNumeric;
        private ComboBox ColorsCombobox;
        private MaskedTextBox ColorhexMaskedtextbox;
        private Panel ColorShowcasePalette;
        private Label label1;
    }
}
