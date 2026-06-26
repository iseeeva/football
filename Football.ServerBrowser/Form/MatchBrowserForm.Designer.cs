namespace Football.ServerBrowser
{
    partial class MatchBrowserForm
    {
        private System.ComponentModel.IContainer components = null;

        // ── Control declarations ────────────────────────────────────────
        private Panel _topBar;
        private Label _titleLabel;
        private TextBox _searchBox;
        private Button _refreshBtn;
        private Panel _statusPanel;
        private Label _statusBar;
        private Label _serverCountLabel;
        private SplitContainer _splitter;
        private Panel _serverListPanel;
        private Panel _detail;
        private Panel _detailHeader;
        private Label _detailTitle;
        private Panel _detailBody;

        // ── Design-time constants (also used by the logic file) ─────────
        internal static readonly Color Win7Window = SystemColors.Window;
        internal static readonly Color Win7Control = SystemColors.Control;
        internal static readonly Color Win7Text = SystemColors.ControlText;
        internal static readonly Color Border = Color.FromArgb(160, 160, 160);
        internal static readonly Color BorderLight = Color.FromArgb(215, 215, 215);
        internal static readonly Color Accent = Color.FromArgb(0, 102, 204);
        internal static readonly Color TextSecond = Color.FromArgb(64, 64, 64);
        internal static readonly Color TextMuted = Color.FromArgb(120, 120, 120);
        internal const string PlaceholderText = "Search servers...";

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            _topBar = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            _titleLabel = new Label();
            _matchCreateButton = new Button();
            _searchBox = new TextBox();
            _refreshBtn = new Button();
            _statusPanel = new Panel();
            _statusBar = new Label();
            _serverCountLabel = new Label();
            _splitter = new SplitContainer();
            _serverListPanel = new Panel();
            _detail = new Panel();
            _detailBody = new Panel();
            _detailHeader = new Panel();
            _detailTitle = new Label();
            _topBar.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            _statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_splitter).BeginInit();
            _splitter.Panel1.SuspendLayout();
            _splitter.Panel2.SuspendLayout();
            _splitter.SuspendLayout();
            _detail.SuspendLayout();
            _detailHeader.SuspendLayout();
            SuspendLayout();
            // 
            // _topBar
            // 
            _topBar.Controls.Add(tableLayoutPanel1);
            _topBar.Controls.Add(_searchBox);
            _topBar.Controls.Add(_refreshBtn);
            _topBar.Dock = DockStyle.Top;
            _topBar.Location = new Point(0, 0);
            _topBar.Name = "_topBar";
            _topBar.Size = new Size(1099, 42);
            _topBar.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Left;
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(_titleLabel, 0, 0);
            tableLayoutPanel1.Controls.Add(_matchCreateButton, 1, 0);
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddColumns;
            tableLayoutPanel1.Location = new Point(12, 9);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(333, 25);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // _titleLabel
            // 
            _titleLabel.Anchor = AnchorStyles.None;
            _titleLabel.AutoEllipsis = true;
            _titleLabel.AutoSize = true;
            _titleLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _titleLabel.Location = new Point(0, 3);
            _titleLabel.Margin = new Padding(0);
            _titleLabel.MaximumSize = new Size(300, 0);
            _titleLabel.Name = "_titleLabel";
            _titleLabel.Size = new Size(113, 19);
            _titleLabel.TabIndex = 0;
            _titleLabel.Text = "Server Browser";
            // 
            // _matchCreateButton
            // 
            _matchCreateButton.Anchor = AnchorStyles.Left;
            _matchCreateButton.Location = new Point(113, 0);
            _matchCreateButton.Margin = new Padding(0);
            _matchCreateButton.Name = "_matchCreateButton";
            _matchCreateButton.Size = new Size(110, 25);
            _matchCreateButton.TabIndex = 4;
            _matchCreateButton.Text = "Create Match";
            _matchCreateButton.UseVisualStyleBackColor = true;
            // 
            // _searchBox
            // 
            _searchBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _searchBox.BorderStyle = BorderStyle.FixedSingle;
            _searchBox.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            _searchBox.Location = new Point(779, 9);
            _searchBox.Name = "_searchBox";
            _searchBox.Size = new Size(220, 23);
            _searchBox.TabIndex = 1;
            // 
            // _refreshBtn
            // 
            _refreshBtn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _refreshBtn.Location = new Point(1009, 8);
            _refreshBtn.Name = "_refreshBtn";
            _refreshBtn.Size = new Size(80, 25);
            _refreshBtn.TabIndex = 2;
            _refreshBtn.Text = "Refresh";
            _refreshBtn.UseVisualStyleBackColor = true;
            // 
            // _statusPanel
            // 
            _statusPanel.Controls.Add(_statusBar);
            _statusPanel.Controls.Add(_serverCountLabel);
            _statusPanel.Dock = DockStyle.Bottom;
            _statusPanel.Location = new Point(0, 632);
            _statusPanel.Name = "_statusPanel";
            _statusPanel.Size = new Size(1099, 24);
            _statusPanel.TabIndex = 2;
            // 
            // _statusBar
            // 
            _statusBar.Font = new Font("Segoe UI", 9F);
            _statusBar.Location = new Point(6, 0);
            _statusBar.Name = "_statusBar";
            _statusBar.Size = new Size(300, 24);
            _statusBar.TabIndex = 0;
            _statusBar.Text = " Ready";
            _statusBar.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // _serverCountLabel
            // 
            _serverCountLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _serverCountLabel.Font = new Font("Segoe UI", 9F);
            _serverCountLabel.Location = new Point(935, 0);
            _serverCountLabel.Name = "_serverCountLabel";
            _serverCountLabel.Size = new Size(150, 24);
            _serverCountLabel.TabIndex = 1;
            _serverCountLabel.Text = "0 servers";
            _serverCountLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // _splitter
            // 
            _splitter.Dock = DockStyle.Fill;
            _splitter.Location = new Point(0, 42);
            _splitter.Name = "_splitter";
            // 
            // _splitter.Panel1
            // 
            _splitter.Panel1.Controls.Add(_serverListPanel);
            // 
            // _splitter.Panel2
            // 
            _splitter.Panel2.Controls.Add(_detail);
            _splitter.Size = new Size(1099, 590);
            _splitter.SplitterDistance = 459;
            _splitter.TabIndex = 1;
            // 
            // _serverListPanel
            // 
            _serverListPanel.AutoScroll = true;
            _serverListPanel.Dock = DockStyle.Fill;
            _serverListPanel.Location = new Point(0, 0);
            _serverListPanel.Name = "_serverListPanel";
            _serverListPanel.Size = new Size(459, 590);
            _serverListPanel.TabIndex = 0;
            // 
            // _detail
            // 
            _detail.BackColor = Color.FromArgb(240, 244, 249);
            _detail.Controls.Add(_detailBody);
            _detail.Controls.Add(_detailHeader);
            _detail.Dock = DockStyle.Fill;
            _detail.Location = new Point(0, 0);
            _detail.Name = "_detail";
            _detail.Size = new Size(636, 590);
            _detail.TabIndex = 0;
            // 
            // _detailBody
            // 
            _detailBody.AutoScroll = true;
            _detailBody.BackColor = Color.FromArgb(240, 244, 249);
            _detailBody.Dock = DockStyle.Fill;
            _detailBody.Location = new Point(0, 44);
            _detailBody.Name = "_detailBody";
            _detailBody.Padding = new Padding(12);
            _detailBody.Size = new Size(636, 546);
            _detailBody.TabIndex = 1;
            // 
            // _detailHeader
            // 
            _detailHeader.BackColor = Color.FromArgb(230, 238, 247);
            _detailHeader.Controls.Add(_detailTitle);
            _detailHeader.Dock = DockStyle.Top;
            _detailHeader.Location = new Point(0, 0);
            _detailHeader.Name = "_detailHeader";
            _detailHeader.Size = new Size(636, 44);
            _detailHeader.TabIndex = 0;
            // 
            // _detailTitle
            // 
            _detailTitle.Dock = DockStyle.Fill;
            _detailTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            _detailTitle.Location = new Point(0, 0);
            _detailTitle.Name = "_detailTitle";
            _detailTitle.Padding = new Padding(12, 0, 0, 0);
            _detailTitle.Size = new Size(636, 44);
            _detailTitle.TabIndex = 0;
            _detailTitle.Text = "Select a server to view details";
            _detailTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MatchBrowserForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1099, 656);
            Controls.Add(_splitter);
            Controls.Add(_topBar);
            Controls.Add(_statusPanel);
            Font = new Font("Segoe UI", 9F);
            MinimumSize = new Size(850, 520);
            Name = "MatchBrowserForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sobee Server Browser";
            _topBar.ResumeLayout(false);
            _topBar.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            _statusPanel.ResumeLayout(false);
            _splitter.Panel1.ResumeLayout(false);
            _splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitter).EndInit();
            _splitter.ResumeLayout(false);
            _detail.ResumeLayout(false);
            _detailHeader.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button _matchCreateButton;
        private TableLayoutPanel tableLayoutPanel1;
    }
}