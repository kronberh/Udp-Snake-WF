using System.ComponentModel;
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
        readonly BindingList<string> nicknamesList;
        SnakeGame game;
        Color[] fieldPaints = [Color.YellowGreen, Color.GreenYellow];
        Color snakeColor;
        Color foodColor = Color.Red;
        readonly int fieldSize;
        readonly Color[,] fieldColors;
        readonly DataGridView grid;
        public HostForm(UdpClient listener, string name, Color selectedColor)
        {
            InitializeComponent();
            this.listener = listener;
            IPLabel.Text = ((IPEndPoint)this.listener.Client.LocalEndPoint).Address.ToString();
            PortLabel.Text = ((IPEndPoint)this.listener.Client.LocalEndPoint).Port.ToString();
            nicknamesList = [name];
            PlayersListbox.DataSource = nicknamesList;
            nicknamesList.ListChanged += (sender, e) =>
            {
                CommunicationUnit response = new("Update players list") { Attachment = nicknamesList };
                foreach (IPEndPoint remote in game?.Players.Skip(1).Select(x => x.Controller))
                {
                    this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)), remote);
                }
                Invoke(new EventHandler(delegate
                {
                    PlayersListbox.DataSource = null;
                    PlayersListbox.DataSource = nicknamesList;
                }));
            };
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
                        PortLabel.BackColor = Color.Black;
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
                game?.SendChatMessageToAll($"💬 {name}: {MessageTextbox.Text}");
                MessageTextbox.Clear();
            };
            ReviveButton.Click += (sender, e) =>
            {
                game?.AwakeSnake(game.Players[0], true);
                ReviveButton.Enabled = false;
                GiveUpButton.Enabled = true;
            };
            GiveUpButton.Click += (sender, e) => {
                game?.GiveUp(game.Players[0]);
                GiveUpButton.Enabled = false;
                ReviveButton.Enabled = true;
            };
            Load += (sender, e) => grid.ClearSelection();
            FormClosed += (sender, e) =>
            {
                // todo await send HOSTISLEAVING message
                listener.Close();
                Application.OpenForms[0]?.Show();
            };
            fieldSize = 24;
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
            Controls.Add(grid);
            Controls.SetChildIndex(grid, 0);
            Shown += (sender, e) =>
            {
                Task.Run(() =>
                {
                    game = new(
                        grid.Invalidate,
                        msg =>
                        {
                            Invoke(new EventHandler(delegate
                            {
                                ChatTextbox.AppendText(msg + Environment.NewLine);
                            }));
                        },
                        () =>
                        {
                            try
                            {
                                Invoke(new EventHandler(delegate
                                {
                                    ScoreLabel.Text = game?.HostSnake.Count().ToString();
                                }));
                            }
                            catch { /* occures once when listener is disposed; needs to be ignored */ }
                        },
                        () =>
                        {
                            Invoke(new EventHandler(delegate
                            {
                                GiveUpButton.Enabled = false;
                                ReviveButton.Enabled = true;
                            }));
                        },
                        fieldColors,
                        fieldPaints,
                        foodColor,
                        listener,
                        name,
                        snakeColor
                    );
                });
                Task.Run(() =>
                {
                    while (true)
                    {
                        IPEndPoint remote = new(IPAddress.Any, 0);
                        try
                        {
                            string json = Encoding.UTF8.GetString(listener.Receive(ref remote));
                            CommunicationUnit request = JsonSerializer.Deserialize<CommunicationUnit>(json, FieldDataUnit.SerializerOptions);
                            switch (request?.Subject)
                            {
                                case "Set direction":
                                    game.SetSnakeDirection(remote, ((JsonElement)request.Attachment).Deserialize<Direction>());
                                    break;
                                case "Message":
                                    game.SendChatMessageToAll(request.Attachment.ToString());
                                    break;
                                case "Revive":
                                    game.AwakeSnake(game.PlayerByRemote(remote), true);
                                    break;
                                case "Give up":
                                    game.GiveUp(game.PlayerByRemote(remote));
                                    break;
                                case "Join lobby":
                                    PlayerData playerData = ((JsonElement)request.Attachment).Deserialize<PlayerData>(PlayerData.SerializerOptions);
                                    string name = playerData.Name;
                                    // todo check name uniqueness
                                    Color color = playerData.Color;
                                    CommunicationUnit response = new("Welcome");
                                    this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)), remote);
                                    game.AddPlayer(remote, name, color);
                                    nicknamesList.Add(name);
                                    break;
                                case "Leave lobby":
                                    nicknamesList.Remove(game.PlayerByRemote(remote).Nickname);
                                    game.RemovePlayer(remote);
                                    break;
                                default:
                                    CommunicationUnit errorResponse = new("Error") { Attachment = "Cannot proceed the received request." };
                                    this.listener.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(errorResponse)), remote);
                                    break;
                            }
                        }
                        catch (ObjectDisposedException)
                        {
                            game.Dispose();
                            break;
                        }
                        catch { /* occures once when listener is disposed; needs to be ignored */ }
                    }
                });
            };
        }
    }
}