using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ns_Data;
using ns_ExtensionMethods;

namespace Coursework_OnlineSnake
{
    public partial class MainMenuForm : Form
    {
        public MainMenuForm()
        {
            InitializeComponent();
            List<Color> systemColors = typeof(SystemColors).GetProperties().Select(x => (Color)x.GetValue(null)).ToList();
            foreach (KnownColor color in Enum.GetValues<KnownColor>().Where(x => !systemColors.Contains(Color.FromKnownColor(x)) && x != KnownColor.Transparent))
            {
                SnakeColorCombobox.Items.Add(color);
                AddFieldColorCombobox.Items.Add(color);
                FoodColorCombobox.Items.Add(color);
            }
            SnakeColorCombobox.SelectedValueChanged += ColorsCombobox_SelectedValueChanged;
            SnakeColorCombobox.SelectedIndex = SnakeColorCombobox.FindStringExact(Color.Black.Name);
            FoodColorCombobox.SelectedIndex = SnakeColorCombobox.FindStringExact(Color.Red.Name);
            AddFieldColorCombobox.SelectedValueChanged += AddFieldColorCombobox_SelectedValueChanged;
            FieldColorsListbox.Items.Add(Color.YellowGreen);
            FieldColorsListbox.Items.Add(Color.GreenYellow);
            JoinButton.Click += JoinButton_Click;
            HostButton.Click += HostButton_Click;
            ClearFieldColorsButton.Click += ClearFieldColorsButton_Click;
        }
        private void ColorsCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            Color selectedColor = Color.FromKnownColor((KnownColor)SnakeColorCombobox.SelectedItem);
            ColorHexTextbox.Text = $"{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";
            ColorShowcasePalette.BackColor = selectedColor;
        }
        private void AddFieldColorCombobox_SelectedValueChanged(object? sender, EventArgs e)
        {
            FieldColorsListbox.Items.Add(Color.FromKnownColor((KnownColor)AddFieldColorCombobox.SelectedItem));
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
                CommunicationUnit message = new("Join lobby") { Attachment = new JoiningPlayerData(string.IsNullOrEmpty(NameTextbox.Text) ? "guest" : NameTextbox.Text, Color.FromKnownColor((KnownColor)SnakeColorCombobox.SelectedItem)) };
                exception = await udpClient.TrySendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message, JoiningPlayerData.SerializerOptions)));
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
                CommunicationUnit response = JsonSerializer.Deserialize<CommunicationUnit>(responseJson, FieldDataUnit.SerializerOptions);
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
                JoinedPlayerData info = ((JsonElement)response.Attachment).Deserialize<JoinedPlayerData>();
                this.Hide();
                new JoinForm(udpClient, info.FieldSize, info.Name, Color.FromKnownColor((KnownColor)SnakeColorCombobox.SelectedItem)).Show();
                break;
            }
            while (true);
        }
        private void HostButton_Click(object? sender, EventArgs e)
        {
            IPEndPoint localEndpoint = new(Dns.GetHostEntry(Dns.GetHostName()).AddressList[0], 0);
            UdpClient listener = new(localEndpoint);
            this.Hide();
            new HostForm(listener, string.IsNullOrEmpty(NameTextbox.Text) ? "guest" : NameTextbox.Text, Color.FromKnownColor((KnownColor)SnakeColorCombobox.SelectedItem), (int)FieldSizeNumeric.Value, Color.FromKnownColor((KnownColor)FoodColorCombobox.SelectedItem), (FieldColorsListbox.Items.Count == 0) ? [Color.YellowGreen, Color.GreenYellow] : FieldColorsListbox.Items.Cast<Color>().ToArray()).Show();
        }
        private void ClearFieldColorsButton_Click(object? sender, EventArgs e)
        {
            FieldColorsListbox.Items.Clear();
        }
    }
}