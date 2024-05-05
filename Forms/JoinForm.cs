using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ns_Data;
using ns_SnakeGame;

namespace Coursework_OnlineSnake
{
    public partial class JoinForm : Form
    {
        readonly UdpClient udpClient;
        readonly int fieldSize;
        //readonly string name;  // todo use it to draw nametag as "you"
        //Color snakeColor; // todo make it possible to change color
        readonly Color[,] fieldColors;
        readonly DataGridView grid;
        public JoinForm(UdpClient udpClient, string name, Color selectedColor)
        {
            InitializeComponent();
            this.udpClient = udpClient;
            IPLabel.Text = ((IPEndPoint)this.udpClient.Client.RemoteEndPoint).Address.ToString();
            PortLabel.Text = ((IPEndPoint)this.udpClient.Client.RemoteEndPoint).Port.ToString();
            IPCopyButton.Click += (sender, e) => Clipboard.SetText(IPLabel.Text);
            PortCopyButton.Click += (sender, e) => Clipboard.SetText(PortLabel.Text);
            IPShowHideButton.Click += (sender, e) =>
            {
                switch (IPLabel.BackColor == Color.Black)
                {
                    case true:
                        IPLabel.BackColor = Color.Transparent;
                        IPShowHideButton.Text = "Hide";
                        break;
                    case false:
                        IPLabel.BackColor = Color.Black; ;
                        IPShowHideButton.Text = "Show";
                        break;
                }
            };
            PortShowHideButton.Click += (sender, e) =>
            {
                switch (PortLabel.BackColor == Color.Black)
                {
                    case true:
                        PortLabel.BackColor = Color.Transparent;
                        PortShowHideButton.Text = "Hide";
                        break;
                    case false:
                        PortLabel.BackColor = Color.Black; ;
                        PortShowHideButton.Text = "Show";
                        break;
                }
            };
            SendMessageButton.Click += (sender, e) =>
            {
                if (string.IsNullOrEmpty(MessageTextbox.Text))
                {
                    return;
                }
                CommunicationUnit message = new("Message") { Attachment = $"💬 {name}: {MessageTextbox.Text}" };
                udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                MessageTextbox.Clear();
            };
            ReviveButton.Click += (sender, e) =>
            {
                CommunicationUnit message = new("Revive");
                udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                ReviveButton.Enabled = false;
                GiveUpButton.Enabled = true;
            };
            ReviveButton.Click += (sender, e) =>
            {
                CommunicationUnit message = new("Give up");
                udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                GiveUpButton.Enabled = false;
                ReviveButton.Enabled = true;
            };
            this.Load += (senver, e) => grid.ClearSelection();
            this.FormClosed += (sender, e) =>
            {
                CommunicationUnit message = new("Leave lobby");
                udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                Application.OpenForms[0]?.Show();
            };
            fieldSize = 24;
            //this.name = name; // todo see line 14
            //snakeColor = selectedColor; // todo see line 15
            fieldColors = new Color[fieldSize, fieldSize];
            grid = new()
            {
                ColumnCount = fieldColors.GetLength(0),
                RowCount = fieldColors.GetLength(1) + 1,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeColumns = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.None,
                ColumnHeadersVisible = false,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                RowHeadersVisible = false
            };
            grid.RowTemplate.Resizable = DataGridViewTriState.True;
            grid.CellFormatting += (sender, e) => e.CellStyle.BackColor = fieldColors[e.RowIndex, e.ColumnIndex];
            grid.SizeChanged += (sender, e) =>
            {
                int newRowHeight = grid.Size.Height / grid.RowCount;
                foreach (DataGridViewRow row in grid.Rows)
                {
                    row.Height = newRowHeight;
                }
            };
            grid.SelectionChanged += (sender, e) =>
            {
                grid.ClearSelection();
            };
            grid.KeyDown += (sender, e) =>
            {
                Direction? direction = e.KeyCode switch
                {
                    Keys.W => Direction.LEFT,
                    Keys.S => Direction.RIGHT,
                    Keys.A => Direction.UP,
                    Keys.D => Direction.DOWN,
                    Keys.Up => Direction.LEFT,
                    Keys.Down => Direction.RIGHT,
                    Keys.Left => Direction.UP,
                    Keys.Right => Direction.DOWN,
                    _ => null
                };
                if (direction.HasValue)
                {
                    CommunicationUnit message = new("Set direction") { Attachment = direction };
                    udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                }
            };
            this.Controls.Add(grid);
            this.Controls.SetChildIndex(grid, 0);
            grid.Select();
            Task.Run(async () =>
            {
                while (udpClient.Client.Connected)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    string json = Encoding.UTF8.GetString(result.Buffer);
                    CommunicationUnit message = JsonSerializer.Deserialize<CommunicationUnit>(json, FieldDataUnit.SerializerOptions);
                    switch (message?.Subject)
                    {
                        case "Field data":
                            List<FieldDataUnit> packageData = ((JsonElement)message.Attachment).Deserialize<List<FieldDataUnit>>(FieldDataUnit.SerializerOptions);
                            foreach (FieldDataUnit package in packageData ?? [])
                            {
                                fieldColors[package.X, package.Y] = package.Color;
                            }
                            grid.Invalidate();
                            break;
                        case "New score":
                            this.Invoke(new EventHandler(delegate
                            {
                                ScoreLabel.Text = message.Attachment?.ToString();
                            }));
                            break;
                        case "Message":
                            Invoke(new EventHandler(delegate
                            {
                                ChatTextbox.AppendText(message.Attachment?.ToString() + Environment.NewLine);
                            }));
                            break;
                        case "You died":
                            GiveUpButton.Enabled = false;
                            ReviveButton.Enabled = true;
                            break;
                        // todo accept future subjects
                        default:
                            CommunicationUnit errorResponse = new("Error") { Attachment = "Cannot proceed the received request." };
                            udpClient.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(errorResponse)));
                            break;
                    }
                }
            });
        }
    }
}