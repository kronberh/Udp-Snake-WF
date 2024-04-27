using System;
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
            this.Load += (senver, e) => grid?.ClearSelection();
            this.FormClosed += (sender, e) =>
            {
                udpClient.Send(Encoding.UTF8.GetBytes("Im leaving"));
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
                Enabled = false,
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
            this.Controls.Add(grid);
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
                                    udpClient.Send(Encoding.UTF8.GetBytes("Pls revive me"));
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
            this.KeyDown += (sender, e) =>
            {
                switch (e.KeyCode)
                {
                    case Keys.W:
                    case Keys.Up:
                        udpClient.Send(Encoding.UTF8.GetBytes($"Pls set my direction to {Direction.LEFT}"));
                        // todo expect acknowledgement
                        break;
                    case Keys.S:
                    case Keys.Down:
                        udpClient.Send(Encoding.UTF8.GetBytes($"Pls set my direction to {Direction.RIGHT}"));
                        // todo expect acknowledgement
                        break;
                    case Keys.A:
                    case Keys.Left:
                        udpClient.Send(Encoding.UTF8.GetBytes($"Pls set my direction to {Direction.UP}"));
                        // todo expect acknowledgement
                        break;
                    case Keys.D:
                    case Keys.Right:
                        udpClient.Send(Encoding.UTF8.GetBytes($"Pls set my direction to {Direction.DOWN}"));
                        // todo expect acknowledgement
                        break;
                }
            };
        }
    }
}