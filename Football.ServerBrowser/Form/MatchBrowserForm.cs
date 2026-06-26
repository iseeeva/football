using Football.GameServer.Messages.Match;
using Football.GameServer.Messages.Player;
using Football.ServerBrowser.Messages;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Reflection;

namespace Football.ServerBrowser
{
    public partial class MatchBrowserForm : Form
    {
        private List<BrowserServerListItem> _servers = new();
        private List<BrowserServerListItem> _filteredServers = new();

        private int _selected = -1;
        private int _hoveredCard = -1;
        private readonly List<Rectangle> _cardRects = new();

        private SobeeClient? _sobeeClient => Program.AppContext?.ServerConnection.Client;

        public MatchBrowserForm()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                typeof(Panel)
                    .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    ?.SetValue(_serverListPanel, true, null);

                AttachEvents();
                InitializeUiWithDb();
                Load += async (s, e) => await LoadServersAsync();
            }
        }

        // ── Event wiring ────────────────────────────────────────────────

        private void AttachEvents()
        {
            _topBar.Paint += TopBar_Paint;
            _statusPanel.Paint += StatusPanel_Paint;
            _statusPanel.Resize += StatusPanel_Resize;

            _serverListPanel.Paint += ListPanel_Paint;
            _serverListPanel.MouseMove += ListPanel_MouseMove;
            _serverListPanel.MouseLeave += ListPanel_MouseLeave;
            _serverListPanel.MouseClick += ListPanel_Click;

            _detailHeader.Paint += DetailHeader_Paint;
            _searchBox.GotFocus += SearchBox_GotFocus;
            _searchBox.LostFocus += SearchBox_LostFocus;

            // Re-filter only when text actually changes
            _searchBox.TextChanged += (s, e) => RefreshList();
            _refreshBtn.Click += RefreshBtn_Click;
            _matchCreateButton.Click += MatchCreateButton_Click;

            Shown += (s, e) => _splitter.SplitterDistance = 460;
        }

        // ── UI initialization ───────────────────────────────────────────

        public async void InitializeUiWithDb()
        {
            if (_sobeeClient == null) return;

            try
            {
                var userDetails = await _sobeeClient.GetUserDetailsFromDb();
                _titleLabel.Text = $"Welcome back, {userDetails?.PlayerName ?? "Guest"}";
            }
            catch (Exception ex)
            {
                _titleLabel.Text = "Welcome back, User";
                Debug.WriteLine($"Error fetching user: {ex.Message}");
            }
        }

        // ── Data loading & Filtering ────────────────────────────────────

        private async void RefreshBtn_Click(object? sender, EventArgs e)
        {
            _refreshBtn.Enabled = false;
            try { await LoadServersAsync(); }
            finally { _refreshBtn.Enabled = true; }
        }

        private async Task LoadServersAsync()
        {
            if (_sobeeClient is null) return;

            var response = await _sobeeClient.SendRequestAsync(new BrowserServerListRequestMessage());
            if (response is BrowserServerListResponseMessage msg)
            {
                _servers = msg.Matches;
                _selected = _hoveredCard = -1;

                RefreshList();
                ShowDetail(-1);

                _statusBar.Text = $" Last refresh: {DateTime.Now:HH:mm:ss}";
                _serverCountLabel.Text = $"{_servers.Count} servers online ";
            }
        }

        private void RefreshList()
        {
            string filter = GetFilterText();
            _filteredServers = _servers
                .Where(s => string.IsNullOrEmpty(filter)
                    || s.MatchInfo.MatchState.ToString().ToLower().Contains(filter)
                    || s.MatchInfo.FieldPositioning.ToString().ToLower().Contains(filter))
                .ToList();

            if (_selected >= _filteredServers.Count) _selected = -1;
            _hoveredCard = -1;

            _serverListPanel.Invalidate();
        }

        // ── Match Creation ───────────────────────────────────────────────

        private void MatchCreateButton_Click(object? sender, EventArgs e)
        {
            if (_sobeeClient is null)
                return;

            var childMatchCreateForm = new MatchCreateForm();
            childMatchCreateForm.ShowDialog(this);
        }

        // ── Search box placeholder ──────────────────────────────────────

        private string GetFilterText() =>
            _searchBox.Text == PlaceholderText ? "" : _searchBox.Text.Trim().ToLower();

        private void SearchBox_GotFocus(object? sender, EventArgs e)
        {
            if (_searchBox.Text != PlaceholderText) return;
            _searchBox.Text = "";
            _searchBox.ForeColor = Win7Text;
            _searchBox.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
        }

        private void SearchBox_LostFocus(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_searchBox.Text)) return;
            _searchBox.Text = PlaceholderText;
            _searchBox.ForeColor = TextMuted;
            _searchBox.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
        }

        // ── Paint handlers ──────────────────────────────────────────────

        private void TopBar_Paint(object? sender, PaintEventArgs e)
        {
            using var brush = new LinearGradientBrush(
                _topBar.ClientRectangle,
                Color.FromArgb(246, 248, 250),
                Color.FromArgb(228, 232, 238), 90f);
            e.Graphics.FillRectangle(brush, _topBar.ClientRectangle);

            using var pen = new Pen(BorderLight, 1);
            e.Graphics.DrawLine(pen, 0, _topBar.Height - 1, _topBar.Width, _topBar.Height - 1);
        }

        private void StatusPanel_Paint(object? sender, PaintEventArgs e)
        {
            using var pen = new Pen(BorderLight, 1);
            e.Graphics.DrawLine(pen, 0, 0, _statusPanel.Width, 0);
        }

        private void StatusPanel_Resize(object? sender, EventArgs e)
        {
            _serverCountLabel.Left = _statusPanel.Width - _serverCountLabel.Width - 14;
        }

        private void DetailHeader_Paint(object? sender, PaintEventArgs e)
        {
            using var grad = new LinearGradientBrush(
                _detailHeader.ClientRectangle,
                Color.FromArgb(226, 236, 247),
                Color.FromArgb(240, 244, 249), 90f);
            e.Graphics.FillRectangle(grad, _detailHeader.ClientRectangle);

            using var pen = new Pen(Border, 1);
            e.Graphics.DrawLine(pen, 0, _detailHeader.Height - 1, _detailHeader.Width, _detailHeader.Height - 1);
        }

        // ── Server list painting ────────────────────────────────────────

        private void ListPanel_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            int scrollY = _serverListPanel.AutoScrollPosition.Y;
            _cardRects.Clear();

            const int cardH = 75, gap = 4, padX = 6;
            int y = 6 + scrollY;
            int w = _serverListPanel.ClientSize.Width - padX * 2;

            using var fontBold = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            using var fontVs = new Font("Segoe UI", 10.5f, FontStyle.Bold);
            using var fontSub = new Font("Segoe UI", 8f);

            for (int i = 0; i < _filteredServers.Count; i++)
            {
                var srv = _filteredServers[i];
                bool hovered = _hoveredCard == i;
                bool selected = _selected == i;

                var cardRect = new Rectangle(padX, y, w, cardH);
                _cardRects.Add(cardRect);

                Color cardBg = selected ? Color.FromArgb(218, 236, 251)
                                 : hovered ? Color.FromArgb(237, 245, 253)
                                 : Win7Window;

                Color cardBorder = selected ? Color.FromArgb(120, 174, 229)
                                 : hovered ? Color.FromArgb(180, 210, 240)
                                 : BorderLight;

                using (var bgBrush = new SolidBrush(cardBg))
                    g.FillRectangle(bgBrush, cardRect);
                using (var pen = new Pen(cardBorder, 1))
                    g.DrawRectangle(pen, cardRect);

                int serverIndex = _servers.IndexOf(srv) + 1;
                DrawText(g, $"{srv.MatchName.ToUpper()} #{serverIndex:D2}", fontBold,
                    selected ? Accent : TextSecond,
                    new Point(cardRect.X + 12, cardRect.Y + 8));

                string stateLabel = StateLabel(srv.MatchInfo.MatchState);
                string stateFull = $"[{stateLabel}]";
                DrawText(g, stateFull, fontBold, StateColor(srv.MatchInfo.MatchState),
                    new Point(cardRect.Right - (int)g.MeasureString(stateFull, fontBold).Width - 12, cardRect.Y + 8));

                string matchStr = $"{srv.MatchInfo.ScenarioInfo.team1ShortName} ({srv.MatchInfo.HomePlayer.Count})" +
                                  $"   vs   " +
                                  $"{srv.MatchInfo.ScenarioInfo.team2ShortName} ({srv.MatchInfo.AwayPlayer.Count})";
                DrawText(g, matchStr, fontVs, Win7Text, new Point(cardRect.X + 12, cardRect.Y + 32));

                string ballHolder = srv.MatchInfo.BallOwner >= 0
                    ? srv.MatchInfo.GetPlayer(srv.MatchInfo.BallOwner)?.PlayerName ?? "None"
                    : "None";

                string infoStr = $"Time: {TimeSpan.FromSeconds(srv.MatchInfo.PhaseInfo.MatchTime):mm\\:ss}" +
                                 $"  |  {srv.MatchInfo.FieldPositioning}" +
                                 $"  |  Speed: x{srv.MatchInfo.TimeMultiplier:F1}" +
                                 $"  |  {ballHolder}";
                DrawText(g, infoStr, fontSub, TextMuted, new Point(cardRect.X + 12, cardRect.Bottom - 18));

                y += cardH + gap;
            }

            int neededHeight = y - scrollY + 10;
            if (_serverListPanel.AutoScrollMinSize.Height != neededHeight)
            {
                _serverListPanel.AutoScrollMinSize = new Size(0, neededHeight);
            }

            if (_filteredServers.Count == 0)
            {
                using var f = new Font("Segoe UI", 9f);
                using var br = new SolidBrush(TextMuted);
                g.DrawString("No servers match your criteria.", f, br, 15, 20 + scrollY);
            }
        }

        // ── List mouse handling ─────────────────────────────────────────

        private void ListPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            int prev = _hoveredCard;
            _hoveredCard = -1;

            for (int i = 0; i < _cardRects.Count; i++)
            {
                if (_cardRects[i].Contains(e.Location))
                {
                    _hoveredCard = i;
                    break;
                }
            }
            if (_hoveredCard != prev) _serverListPanel.Invalidate();
        }

        private void ListPanel_MouseLeave(object? sender, EventArgs e)
        {
            _hoveredCard = -1;
            _serverListPanel.Invalidate();
        }

        private void ListPanel_Click(object? sender, MouseEventArgs e)
        {
            for (int i = 0; i < _cardRects.Count; i++)
            {
                if (_cardRects[i].Contains(e.Location) && i < _filteredServers.Count)
                {
                    _selected = i;
                    _serverListPanel.Invalidate();
                    ShowDetail(_selected);
                    return;
                }
            }
        }

        // ── Detail panel ────────────────────────────────────────────────

        private void ShowDetail(int filteredIndex)
        {
            foreach (Control ctrl in _detailBody.Controls)
            {
                ctrl.Dispose();
            }
            _detailBody.Controls.Clear();

            if (filteredIndex < 0 || filteredIndex >= _filteredServers.Count)
            {
                _detailTitle.Text = "Select a server to view details";
                _detailTitle.ForeColor = TextMuted;
                _detailHeader.Invalidate();
                return;
            }

            var srv = _filteredServers[filteredIndex];
            int absoluteIndex = _servers.IndexOf(srv) + 1;

            _detailTitle.Text = $"{srv.MatchName} #{absoluteIndex}   —   {StateLabel(srv.MatchInfo.MatchState)}";
            _detailTitle.ForeColor = Accent;
            _detailHeader.Invalidate();

            var layout = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Color.FromArgb(240, 244, 249)
            };

            layout.Controls.Add(SectionLabel("MATCH METADATA"));
            layout.Controls.Add(InfoGrid(new[,]
            {
                { "Guid",        srv.MatchId.ToString("D") },
                { "State",       srv.MatchInfo.MatchState.ToString() },
                { "Field State", srv.MatchInfo.FieldPositioning.ToString() },
                { "Time",        $"{TimeSpan.FromSeconds(srv.MatchInfo.PhaseInfo.MatchTime):mm\\:ss}" },
                { "Time Rate",   $"x{srv.MatchInfo.TimeMultiplier:F1}" }
            }));

            string ballOwnerName = srv.MatchInfo.BallOwner >= 0
                ? srv.MatchInfo.GetPlayer(srv.MatchInfo.BallOwner)?.PlayerName ?? "None"
                : "None";

            layout.Controls.Add(SectionLabel("BALL PHYSICS"));
            layout.Controls.Add(InfoGrid(new[,]
            {
                { "Ball Owner",  ballOwnerName },
                { "Coordinates", $"X: {srv.MatchInfo.BallPosition.X:F1}  Y: {srv.MatchInfo.BallPosition.Y:F1}  Z: {srv.MatchInfo.BallPosition.Z:F1}" }
            }));

            layout.Controls.Add(SectionLabel($"HOME LINEUP ({srv.MatchInfo.HomePlayer.Count} / {srv.MatchInfo.ScenarioInfo.team1Size})"));
            layout.Controls.Add(PlayerTable(srv.MatchInfo.HomePlayer, Color.FromArgb(0, 130, 0)));

            layout.Controls.Add(SectionLabel($"AWAY LINEUP ({srv.MatchInfo.AwayPlayer.Count} / {srv.MatchInfo.ScenarioInfo.team2Size})"));
            layout.Controls.Add(PlayerTable(srv.MatchInfo.AwayPlayer, Color.FromArgb(190, 90, 0)));

            int specCount = srv.MatchInfo.HomePlayer.Count + srv.MatchInfo.AwaySpectator.Count;
            if (specCount > 0)
            {
                var specs = srv.MatchInfo.HomeSpectator.Concat(srv.MatchInfo.AwaySpectator).ToList();
                layout.Controls.Add(SectionLabel($"SPECTATORS ({specs.Count})"));
                layout.Controls.Add(PlayerTable(specs, TextSecond));
            }

            _detailBody.Controls.Add(layout);
        }

        // ── Detail helper controls ──────────────────────────────────────

        private Label SectionLabel(string text) => new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 8f, FontStyle.Bold),
            ForeColor = Accent,
            AutoSize = false,
            Width = _detailBody.ClientSize.Width - 24,
            Height = 22,
            TextAlign = ContentAlignment.BottomLeft,
            Margin = new Padding(0, 10, 0, 4)
        };

        private Panel InfoGrid(string[,] rows)
        {
            const int rowH = 24;
            int count = rows.GetLength(0);

            var panel = new Panel
            {
                Width = _detailBody.ClientSize.Width - 24,
                Height = count * rowH,
                BackColor = Win7Window
            };
            panel.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(BorderLight, 1), 0, 0, panel.Width - 1, panel.Height - 1);

            for (int i = 0; i < count; i++)
            {
                if (i > 0)
                {
                    int rowIndex = i;
                    panel.Paint += (s, e) =>
                        e.Graphics.DrawLine(
                            new Pen(Color.FromArgb(242, 242, 242)),
                            0, rowIndex * rowH, panel.Width, rowIndex * rowH);
                }
                panel.Controls.Add(new Label
                {
                    Text = rows[i, 0],
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = TextSecond,
                    Location = new Point(10, i * rowH + 4),
                    Width = 100,
                    Height = 18
                });
                panel.Controls.Add(new Label
                {
                    Text = rows[i, 1],
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = Win7Text,
                    Location = new Point(115, i * rowH + 4),
                    Width = panel.Width - 125,
                    Height = 18
                });
            }
            return panel;
        }

        private Panel PlayerTable(List<PlayerMatchInformationMessage> players, Color accentCol)
        {
            const int rowH = 22;

            if (players.Count == 0)
            {
                var empty = new Panel { Width = _detailBody.ClientSize.Width - 24, Height = rowH, BackColor = Win7Window };
                empty.Paint += (s, e) =>
                    e.Graphics.DrawRectangle(new Pen(BorderLight, 1), 0, 0, empty.Width - 1, empty.Height - 1);
                empty.Controls.Add(new Label
                {
                    Text = "No registered connections",
                    ForeColor = TextMuted,
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Italic),
                    Location = new Point(10, 3),
                    AutoSize = true
                });
                return empty;
            }

            var panel = new Panel
            {
                Width = _detailBody.ClientSize.Width - 24,
                Height = players.Count * rowH,
                BackColor = Win7Window
            };
            panel.Paint += (s, e) =>
                e.Graphics.DrawRectangle(new Pen(BorderLight, 1), 0, 0, panel.Width - 1, panel.Height - 1);

            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                int rowTop = i * rowH;

                if (i > 0)
                {
                    int rowIndex = i;
                    panel.Paint += (s, e) =>
                        e.Graphics.DrawLine(
                            new Pen(Color.FromArgb(246, 246, 246)),
                            0, rowIndex * rowH, panel.Width, rowIndex * rowH);
                }

                panel.Controls.Add(new Label
                {
                    Text = $"[{p.SquadNumber.Value:D2}]",
                    Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                    ForeColor = accentCol,
                    Location = new Point(10, rowTop + 3),
                    Width = 35,
                    Height = 16
                });
                panel.Controls.Add(new Label
                {
                    Text = p.PlayerName,
                    Font = new Font("Segoe UI", 8.5f),
                    ForeColor = Win7Text,
                    Location = new Point(48, rowTop + 3),
                    Width = 140,
                    Height = 16
                });
                panel.Controls.Add(new Label
                {
                    Text = p.PlayerId.ToString("D"),
                    Font = new Font("Courier New", 8.5f),
                    ForeColor = TextMuted,
                    Location = new Point(195, rowTop + 3),
                    Width = panel.Width - 200,
                    Height = 16
                });
            }
            return panel;
        }

        // ── Static helpers ──────────────────────────────────────────────

        private static void DrawText(Graphics g, string txt, Font f, Color c, Point p)
        {
            using var b = new SolidBrush(c);
            g.DrawString(txt, f, b, p.X, p.Y);
        }

        private static Color StateColor(MatchState s) => s switch
        {
            MatchState.Positioning => Color.FromArgb(200, 100, 0),
            MatchState.Running => Color.FromArgb(0, 130, 0),
            _ => SystemColors.GrayText
        };

        private static string StateLabel(MatchState s) => s switch
        {
            MatchState.Positioning => "POSITIONING",
            MatchState.Running => "RUNNING",
            _ => "UNKNOWN"
        };
    }
}