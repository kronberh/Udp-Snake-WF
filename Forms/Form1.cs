using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ns_Data;
using ns_UdpClientExtensionMethods;

namespace Coursework_OnlineSnake
{
    public partial class MainMenuForm : Form
    {
        Color selectedColor;
        public MainMenuForm()
        {
            InitializeComponent();
            List<Color> systemColors = typeof(SystemColors).GetProperties().Select(x => (Color)x.GetValue(null)).ToList();
            foreach (KnownColor color in Enum.GetValues<KnownColor>().Where(x => !systemColors.Contains(Color.FromKnownColor(x)) && x != KnownColor.Transparent))
            {
                ColorsCombobox.Items.Add(color);
            }
            ColorsCombobox.SelectedValueChanged += ColorsCombobox_SelectedValueChanged;
            ColorsCombobox.SelectedIndex = ColorsCombobox.FindStringExact(Color.Black.Name);
            JoinButton.Click += JoinButton_Click;
            HostButton.Click += HostButton_Click;
        }
        private void ColorsCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            selectedColor = Color.FromKnownColor((KnownColor)ColorsCombobox.SelectedItem);
            ColorHexTextbox.Text = $"{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            ColorShowcasePalette.BackColor = selectedColor;
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
                CommunicationUnit message = new("Join lobby") { Attachment = selectedColor };
                exception = await udpClient.TrySendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, PackageData.SerializerOptions)));
                if (exception != null)
                {
                    udpClient.Close();
                    MessageBox.Show(exception.Message, exception.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
                (UdpReceiveResult responseResult, exception) = await udpClient.TryReceiveAsync(TimeSpan.FromSeconds(2));
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
                string responseJson = Encoding.UTF8.GetString(responseResult.Buffer);
                CommunicationUnit response = JsonSerializer.Deserialize<CommunicationUnit>(responseJson, PackageData.SerializerOptions);
                if (response?.Subject != "Welcome")
                {
                    udpClient.Close();
                    DialogResult connectionErrorresponse = MessageBox.Show((response == null) ? "Could not decode server response." : response.Subject, typeof(OperationCanceledException).Name, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
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