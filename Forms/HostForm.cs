using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
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
            this.Text = $"Host: {(IPEndPoint)this.listener.Client.LocalEndPoint}";
            this.Load += (senver, e) => grid?.ClearSelection();
            this.FormClosed += (sender, e) =>
            {
                // todo await send HOSTISLEAVING message
                listener.Close();
                Application.OpenForms[0]?.Show();
            };
            this.KeyDown += (sender, e) =>
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
            };
            Task.Run(() =>
            {
                while (this.listener != null)
                {
                    IPEndPoint remote = new(IPAddress.Any, 0);
                    try
                    {
                        byte[] requestData = this.listener.Receive(ref remote);
                        string request = Encoding.UTF8.GetString(requestData);
                        if (request.StartsWith("Pls set my direction to "))
                        {
                            if (Enum.TryParse(typeof(Direction), request.Split().Last(), true, out object? direction))
                            {
                                game.AcceptSetSnakeDirectionSignal(remote, (Direction)direction);
                                // todo send OK acknowledgement
                            }
                            else
                            {
                                // todo send ERROR acknowledgement
                            }
                        }
                        else if (request == "Pls revive me")
                        {
                            SnakeGame.AwakeSnake(game.PlayerByRemote(remote), true);
                            // todo send OK OK acknowledgement
                        }
                        else if (request.StartsWith("Hello, I'm the new player, and my color is "))
                        {
                            Color color = ColorTranslator.FromHtml(request.Split().Last());
                            if (color == Color.Empty)
                            {
                                color = Color.Black;
                            }
                            byte[] responseData = Encoding.UTF8.GetBytes("Welcome new player!");
                            this.listener.Send(responseData, remote);
                            game.AddPlayer(remote, color);
                        }
                        else if (request == "Im leaving")
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
                    catch
                    {
                        // todo send SOME ERROR message
                    }
                }
            });
        }
    }
}