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
        int fieldSize = 24;
        //Color snakeColor; // todo make it possible to change color
        readonly Color[,] fieldColors;
        readonly DataGridView grid;
        public JoinForm(UdpClient udpClient, Color selectedColor)
        {
            InitializeComponent();
            this.udpClient = udpClient;
            this.Load += (senver, e) => grid.ClearSelection();
            this.FormClosed += (sender, e) =>
            {
                CommunicationUnit message = new("Leave lobby");
                udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                Application.OpenForms[0]?.Show();
            };
            //snakeColor = selectedColor; // todo see line 13
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
            grid.Select();
            Task.Run(async () =>
            {
                while (udpClient.Client.Connected)
                {
                    UdpReceiveResult result = await this.udpClient.ReceiveAsync();
                    string json = Encoding.UTF8.GetString(result.Buffer);
                    CommunicationUnit message = JsonSerializer.Deserialize<CommunicationUnit>(json, PackageData.SerializerOptions);
                    switch (message?.Subject)
                    {
                        case "Field data":
                            List<PackageData> packageData = ((JsonElement)message.Attachment).Deserialize<List<PackageData>>(PackageData.SerializerOptions);
                            foreach (PackageData package in packageData ?? [])
                            {
                                fieldColors[package.X, package.Y] = package.Color;
                            }
                            // todo send OK acknowledgement
                            grid.Invalidate();
                            break;
                        case "You died":
                            Task.Run(() =>
                            {
                                DialogResult connectionErrorresponse = MessageBox.Show("You died. Press \"Retry\" to revive", "Important message", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                                if (connectionErrorresponse == DialogResult.Retry)
                                {
                                    CommunicationUnit message = new("Revive");
                                    udpClient.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)));
                                }
                                else
                                {
                                    this.Close();
                                }
                            });
                            break;
                        // todo accept other subjects
                        default:
                            // todo send idkwym acknowledgement
                            break;
                    }
                }
            });
        }
    }
}