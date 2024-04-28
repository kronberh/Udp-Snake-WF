namespace Coursework_OnlineSnake
{
    partial class HostForm
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
            IPAddressCopyButton = new Button();
            SuspendLayout();
            // 
            // IPAddressCopyButton
            // 
            IPAddressCopyButton.Dock = DockStyle.Bottom;
            IPAddressCopyButton.Location = new Point(0, 474);
            IPAddressCopyButton.Name = "IPAddressCopyButton";
            IPAddressCopyButton.Size = new Size(532, 29);
            IPAddressCopyButton.TabIndex = 0;
            IPAddressCopyButton.TabStop = false;
            IPAddressCopyButton.Text = "Copy IP:";
            IPAddressCopyButton.UseVisualStyleBackColor = true;
            // 
            // HostForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(532, 503);
            Controls.Add(IPAddressCopyButton);
            Name = "HostForm";
            Text = "Host";
            ResumeLayout(false);
        }

        #endregion

        private Button IPAddressCopyButton;
    }
}