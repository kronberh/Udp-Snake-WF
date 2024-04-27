using System.Net;
using System.Net.Sockets;
using System.Text;
using ns_UdpClientExtensionMethods;

namespace Coursework_OnlineSnake
{
    public partial class MainMenuForm : Form
    {
        Color selectedColor;
        public MainMenuForm()
        {
            InitializeComponent();
            foreach (KnownColor color in Enum.GetValues<KnownColor>().Skip(27))
            {
                ColorsCombobox.Items.Add(color);
            }
            ColorsCombobox.SelectedValueChanged += ColorsCombobox_SelectedValueChanged;
            ColorsCombobox.SelectedIndex = ColorsCombobox.FindStringExact(Color.Black.Name);
            SingleplayerButton.Click += SingleplayerButton_Click;
            JoinButton.Click += JoinButton_Click;
            HostButton.Click += HostButton_Click;
        }
        private void ColorsCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            selectedColor = Color.FromKnownColor((KnownColor)ColorsCombobox.SelectedItem);
            ColorhexMaskedtextbox.Text = $"{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            ColorShowcasePalette.BackColor = selectedColor;
        }
        private void SingleplayerButton_Click(object? sender, EventArgs e)
        {
            // todo singleplayer mode
        }
        private async void JoinButton_Click(object? sender, EventArgs e)
        {
            do
            {
                UdpClient udpClient = IPAddress.TryParse(IPTextbox.Text, out IPAddress address) ? new(address.AddressFamily) : new();
                Exception? exception = udpClient.TryConnect(IPTextbox.Text, Convert.ToInt32(PortNumeric.Value));
                if (exception != null)
                {
                    udpClient.Close();
                    DialogResult connectionErrorresponse = (exception is SocketException) ? MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation) : MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (connectionErrorresponse != DialogResult.Retry)
                    {
                        break;
                    }
                    continue;
                }
                exception = await udpClient.TrySendAsync(Encoding.UTF8.GetBytes($"Hello, I'm the new player, and my color is #{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}"));
                if (exception != null)
                {
                    udpClient.Close();
                    MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
                (UdpReceiveResult result, exception) = await udpClient.TryReceiveAsync(TimeSpan.FromSeconds(2));
                if (exception != null)
                {
                    udpClient.Close();
                    DialogResult connectionErrorresponse = MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (connectionErrorresponse != DialogResult.Retry)
                    {
                        break;
                    }
                    continue;
                }
                string response = Encoding.UTF8.GetString(result.Buffer);
                if (response != "Welcome new player!")
                {
                    udpClient.Close();
                    DialogResult connectionErrorresponse = MessageBox.Show(response, typeof(OperationCanceledException).Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
                    if (connectionErrorresponse != DialogResult.Retry)
                    {
                        break;
                    }
                    continue;
                }
                this.Hide();
                new JoinForm(udpClient, selectedColor).Show();
                break;
            }
            while (true);
        }
        private void HostButton_Click(object? sender, EventArgs e)
        {
            IPEndPoint localEndpoint = new(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], 0);
            UdpClient listener = new(localEndpoint);
            this.Hide();
            new HostForm(listener, selectedColor).Show();
        }
    }
}