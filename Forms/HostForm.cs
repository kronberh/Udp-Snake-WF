using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ns_Data;
using ns_SnakeGame;
using ns_Extension_methods;

namespace Coursework_OnlineSnake
{
    public partial class HostForm : Form
    {
        readonly UdpClient listener;
        readonly BindingList<UserData> playersList;
        SnakeGame game;
        readonly Color[] fieldPaints;
        Color snakeColor;
        readonly Color foodColor; // todo make changeable
        readonly int fieldSize;
        readonly Color[,] fieldColors;
        readonly DataGridView grid;
        public HostForm(UdpClient listener, string name, Color snakeColor, int fieldSize, Color foodColor, Color[] fieldPaints)
        {
            InitializeComponent();
            this.listener = listener;
            IPLabel.Text = ((IPEndPoint)this.listener.Client.LocalEndPoint).Address.ToString();
            PortLabel.Text = ((IPEndPoint)this.listener.Client.LocalEndPoint).Port.ToString();
            playersList = [new(name, (IPEndPoint)listener.Client.LocalEndPoint)];
            PlayersListbox.DataSource = playersList;
            PlayersListbox.DisplayMember = "Nickname";
            PlayersListbox.ValueMember = "Controller";
            this.fieldPaints = fieldPaints;
            playersList.ListChanged += (sender, e) =>
            {
                CommunicationUnit response = new("Update players list") { Attachment = playersList };
                foreach (IPEndPoint remote in game?.Players.Skip(1).Select(x => x.Controller))
                {
                    this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)), remote);
                }
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
            KickButton.Click += (sender, e) =>
            {
                CommunicationUnit message = new("Kicked") { Attachment = KickBanNoteTextbox.Text };
                this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), (IPEndPoint)PlayersListbox.SelectedValue);
                KickBanNoteTextbox.Text = "Reason: ";
            };
            BanButton.Click += (sender, e) =>
            {
                CommunicationUnit message = new("Banned");
                this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), (IPEndPoint)PlayersListbox.SelectedValue);
                // todo blacklist IP
                KickBanNoteTextbox.Text = "Reason: ";
            };
            Load += (sender, e) => grid.ClearSelection();
            FormClosed += (sender, e) =>
            {
                CommunicationUnit message = new("Shutdown");
                foreach (IPEndPoint endPoint in game?.Players.Skip(1).Select(x => x.Controller) ?? [])
                {
                    this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message)), endPoint);
                }
                this.listener.Close();
                Application.OpenForms[0]?.Show();
            };
            this.snakeColor = snakeColor;
            this.fieldSize = fieldSize;
            this.foodColor = foodColor;
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
                            try
                            {
                                Invoke(new EventHandler(delegate
                                {
                                    ChatTextbox.AppendText(msg + Environment.NewLine);
                                }));
                            }
                            catch { /* occures once when hist is disposed; can be ignored */ }
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
                            catch { /* occures once when host is disposed; can be ignored */ }
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
                        this.fieldPaints,
                        this.foodColor,
                        this.listener,
                        name,
                        this.snakeColor
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
                                    JoiningPlayerData playerData = ((JsonElement)request.Attachment).Deserialize<JoiningPlayerData>(JoiningPlayerData.SerializerOptions);
                                    string name = playerData.Name.Unique(game.Players.Select(x => x.Nickname));
                                    Color color = playerData.Color;
                                    CommunicationUnit response = new("Welcome") { Attachment = new JoinedPlayerData(name, this.fieldSize) };
                                    this.listener.Send(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response)), remote);
                                    game.AddPlayer(remote, name, color);
                                    playersList.Add(new(name, remote));
                                    break;
                                case "Leave lobby":
                                    playersList.Remove(playersList.FirstOrDefault(x => x.Controller.Address == remote.Address && x.Controller.Port == remote.Port));
                                    game.RemovePlayer(remote);
                                    break;
                                case "Ping":
                                    CommunicationUnit pongMessage = new("Pong");
                                    this.listener.SendAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(pongMessage)), remote);
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