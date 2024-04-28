using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ns_Data;
using ns_SnakeGame;

namespace Coursework_OnlineSnake
{
    public partial class HostForm : Form
    {
        readonly UdpClient listener;
        SnakeGame game;
        Color[] fieldPaints = [Color.YellowGreen, Color.GreenYellow];
        Color snakeColor;
        Color foodColor = Color.Red;
        int fieldSize = 24;
        readonly Color[,] fieldColors;
        readonly DataGridView grid;
        public HostForm(UdpClient listener, Color selectedColor)
        {
            InitializeComponent();
            this.listener = listener;
            IPAddressCopyButton.Text = $"Copy IP: {(IPEndPoint)this.listener.Client.LocalEndPoint}";
            IPAddressCopyButton.Click += IPAddressCopyButton_Click;
            this.Load += (sender, e) => grid.ClearSelection();
            this.FormClosed += (sender, e) =>
            {
                // todo await send HOSTISLEAVING message
                listener.Close();
                Application.OpenForms[0]?.Show();
            };
            snakeColor = selectedColor;
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
                switch (e.KeyCode)
                {
                    case Keys.W:
                    case Keys.Up:
                        game?.HostSnake.SetDirection(Direction.LEFT);
                        break;
                    case Keys.S:
                    case Keys.Down:
                        game?.HostSnake.SetDirection(Direction.RIGHT);
                        break;
                    case Keys.A:
                    case Keys.Left:
                        game?.HostSnake.SetDirection(Direction.UP);
                        break;
                    case Keys.D:
                    case Keys.Right:
                        game?.HostSnake.SetDirection(Direction.DOWN);
                        break;
                }
            };
            this.Controls.Add(grid);
            this.Shown += (sender, e) =>
            {
                Task.Run(() =>
                {
                    game = new(
                        grid.Invalidate,
                        () =>
                        {
                            DialogResult connectionErrorresponse = MessageBox.Show("You died. Press \"Retry\" to revive", "Important message", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information);
                            if (connectionErrorresponse == DialogResult.Retry)
                            {
                                SnakeGame.AwakeSnake(game.Players[0], true);
                            }
                        },
                        fieldColors,
                        fieldPaints,
                        foodColor,
                        listener,
                        snakeColor
                    );
                });
                Task.Run(() =>
                {
                    while (this.listener != null)
                    {
                        IPEndPoint remote = new(IPAddress.Any, 0);
                        try
                        {
                            string json = Encoding.UTF8.GetString(listener.Receive(ref remote));
                            CommunicationUnit request = JsonSerializer.Deserialize<CommunicationUnit>(json, PackageData.SerializerOptions);
                            if (request?.Subject == "Set direction")
                            {
                                game.AcceptSetSnakeDirectionSignal(remote, ((JsonElement)request.Attachment).Deserialize<Direction>());
                            }
                            else if (request?.Subject == "Revive")
                            {
                                SnakeGame.AwakeSnake(game.PlayerByRemote(remote), true);
                            }
                            else if (request?.Subject == "Join lobby")
                            {
                                Color color = ((JsonElement)request.Attachment).Deserialize<Color>(PackageData.SerializerOptions);
                                CommunicationUnit response = new("Welcome");
                                this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)), remote);
                                game.AddPlayer(remote, color);
                            }
                            else if (request?.Subject == "Leave lobby")
                            {
                                game.RemovePlayer(remote);
                            }
                            else
                            {
                                // send IDK WYM message/acknowledgement
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            game.Dispose();
                            break;
                        }
                        catch { /* occures once when listener is disposed min function; needs to be ignored */ }
                    }
                });
            };
        }
        private void IPAddressCopyButton_Click(object? sender, EventArgs e)
        {
            Clipboard.SetText(((IPEndPoint)listener.Client.LocalEndPoint).Address.ToString());
        }
    }
}