namespace Football.ServerBrowser
{
    partial class MatchCreateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            _matchCreationTab = new TabControl();
            _commonTab = new TabPage();
            flowLayoutPanel1 = new FlowLayoutPanel();
            _matchNameLayout = new FlowLayoutPanel();
            _matchNameLabel = new Label();
            _matchNameTextBox = new TextBox();
            _matchPassLayout = new FlowLayoutPanel();
            _matchPassLabel = new Label();
            _matchPassTextBox = new TextBox();
            _scenarioTypeLayout = new FlowLayoutPanel();
            _scenarioTypeLabel = new Label();
            _scenarioTypeComboBox = new ComboBox();
            _homeTeamTab = new TabPage();
            flowLayoutPanel2 = new FlowLayoutPanel();
            _homeFullNameLayout = new FlowLayoutPanel();
            _homeFullNameLabel = new Label();
            _homeFullNameTextBox = new TextBox();
            _homeShortNameLayout = new FlowLayoutPanel();
            _homeShortNameLabel = new Label();
            _homeShortNameTextBox = new TextBox();
            _homeCapacityLayout = new FlowLayoutPanel();
            _homeCapacityLabel = new Label();
            _homeCapacityNumeric = new NumericUpDown();
            _awayTeamTab = new TabPage();
            flowLayoutPanel3 = new FlowLayoutPanel();
            _awayFullNameLayout = new FlowLayoutPanel();
            _awayFullNameLabel = new Label();
            _awayFullNameTextBox = new TextBox();
            _awayShortNameLayout = new FlowLayoutPanel();
            _awayShortNameLabel = new Label();
            _awayShortNameTextBox = new TextBox();
            _awayCapacityLayout = new FlowLayoutPanel();
            _awayCapacityLabel = new Label();
            _awayCapacityNumeric = new NumericUpDown();
            flowLayoutPanel4 = new FlowLayoutPanel();
            _createMatchButton = new Button();
            _statusLabel = new Label();
            _matchCreationTab.SuspendLayout();
            _commonTab.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            _matchNameLayout.SuspendLayout();
            _matchPassLayout.SuspendLayout();
            _scenarioTypeLayout.SuspendLayout();
            _homeTeamTab.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            _homeFullNameLayout.SuspendLayout();
            _homeShortNameLayout.SuspendLayout();
            _homeCapacityLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_homeCapacityNumeric).BeginInit();
            _awayTeamTab.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            _awayFullNameLayout.SuspendLayout();
            _awayShortNameLayout.SuspendLayout();
            _awayCapacityLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_awayCapacityNumeric).BeginInit();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // _matchCreationTab
            // 
            _matchCreationTab.Controls.Add(_commonTab);
            _matchCreationTab.Controls.Add(_homeTeamTab);
            _matchCreationTab.Controls.Add(_awayTeamTab);
            _matchCreationTab.Location = new Point(12, 12);
            _matchCreationTab.Name = "_matchCreationTab";
            _matchCreationTab.SelectedIndex = 0;
            _matchCreationTab.Size = new Size(460, 414);
            _matchCreationTab.TabIndex = 0;
            // 
            // _commonTab
            // 
            _commonTab.Controls.Add(flowLayoutPanel1);
            _commonTab.Location = new Point(4, 24);
            _commonTab.Margin = new Padding(0);
            _commonTab.Name = "_commonTab";
            _commonTab.Size = new Size(452, 386);
            _commonTab.TabIndex = 0;
            _commonTab.Text = "Common";
            _commonTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(_matchNameLayout);
            flowLayoutPanel1.Controls.Add(_matchPassLayout);
            flowLayoutPanel1.Controls.Add(_scenarioTypeLayout);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(452, 386);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // _matchNameLayout
            // 
            _matchNameLayout.AutoSize = true;
            _matchNameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _matchNameLayout.BorderStyle = BorderStyle.FixedSingle;
            _matchNameLayout.Controls.Add(_matchNameLabel);
            _matchNameLayout.Controls.Add(_matchNameTextBox);
            _matchNameLayout.FlowDirection = FlowDirection.TopDown;
            _matchNameLayout.Location = new Point(3, 3);
            _matchNameLayout.Name = "_matchNameLayout";
            _matchNameLayout.Size = new Size(138, 50);
            _matchNameLayout.TabIndex = 3;
            // 
            // _matchNameLabel
            // 
            _matchNameLabel.AutoSize = true;
            _matchNameLabel.Font = new Font("Segoe UI", 10F);
            _matchNameLabel.Location = new Point(3, 0);
            _matchNameLabel.Name = "_matchNameLabel";
            _matchNameLabel.Size = new Size(88, 19);
            _matchNameLabel.TabIndex = 0;
            _matchNameLabel.Text = "Match Name";
            // 
            // _matchNameTextBox
            // 
            _matchNameTextBox.Location = new Point(3, 22);
            _matchNameTextBox.Name = "_matchNameTextBox";
            _matchNameTextBox.Size = new Size(130, 23);
            _matchNameTextBox.TabIndex = 1;
            // 
            // _matchPassLayout
            // 
            _matchPassLayout.AutoSize = true;
            _matchPassLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _matchPassLayout.BorderStyle = BorderStyle.FixedSingle;
            _matchPassLayout.Controls.Add(_matchPassLabel);
            _matchPassLayout.Controls.Add(_matchPassTextBox);
            _matchPassLayout.FlowDirection = FlowDirection.TopDown;
            _matchPassLayout.Location = new Point(3, 59);
            _matchPassLayout.Name = "_matchPassLayout";
            _matchPassLayout.Size = new Size(138, 50);
            _matchPassLayout.TabIndex = 4;
            // 
            // _matchPassLabel
            // 
            _matchPassLabel.AutoSize = true;
            _matchPassLabel.Font = new Font("Segoe UI", 10F);
            _matchPassLabel.Location = new Point(3, 0);
            _matchPassLabel.Name = "_matchPassLabel";
            _matchPassLabel.Size = new Size(110, 19);
            _matchPassLabel.TabIndex = 0;
            _matchPassLabel.Text = "Match Password";
            // 
            // _matchPassTextBox
            // 
            _matchPassTextBox.Location = new Point(3, 22);
            _matchPassTextBox.Name = "_matchPassTextBox";
            _matchPassTextBox.Size = new Size(130, 23);
            _matchPassTextBox.TabIndex = 1;
            _matchPassTextBox.UseSystemPasswordChar = true;
            // 
            // _scenarioTypeLayout
            // 
            _scenarioTypeLayout.AutoSize = true;
            _scenarioTypeLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _scenarioTypeLayout.BorderStyle = BorderStyle.FixedSingle;
            _scenarioTypeLayout.Controls.Add(_scenarioTypeLabel);
            _scenarioTypeLayout.Controls.Add(_scenarioTypeComboBox);
            _scenarioTypeLayout.FlowDirection = FlowDirection.TopDown;
            _scenarioTypeLayout.Location = new Point(3, 115);
            _scenarioTypeLayout.Name = "_scenarioTypeLayout";
            _scenarioTypeLayout.Size = new Size(138, 50);
            _scenarioTypeLayout.TabIndex = 2;
            // 
            // _scenarioTypeLabel
            // 
            _scenarioTypeLabel.AutoSize = true;
            _scenarioTypeLabel.Font = new Font("Segoe UI", 10F);
            _scenarioTypeLabel.Location = new Point(3, 0);
            _scenarioTypeLabel.Name = "_scenarioTypeLabel";
            _scenarioTypeLabel.Size = new Size(92, 19);
            _scenarioTypeLabel.TabIndex = 0;
            _scenarioTypeLabel.Text = "Scenario Type";
            // 
            // _scenarioTypeComboBox
            // 
            _scenarioTypeComboBox.FormattingEnabled = true;
            _scenarioTypeComboBox.Location = new Point(3, 22);
            _scenarioTypeComboBox.Name = "_scenarioTypeComboBox";
            _scenarioTypeComboBox.Size = new Size(130, 23);
            _scenarioTypeComboBox.TabIndex = 1;
            // 
            // _homeTeamTab
            // 
            _homeTeamTab.Controls.Add(flowLayoutPanel2);
            _homeTeamTab.Location = new Point(4, 24);
            _homeTeamTab.Margin = new Padding(0);
            _homeTeamTab.Name = "_homeTeamTab";
            _homeTeamTab.Size = new Size(452, 386);
            _homeTeamTab.TabIndex = 1;
            _homeTeamTab.Text = "Home Team";
            _homeTeamTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(_homeFullNameLayout);
            flowLayoutPanel2.Controls.Add(_homeShortNameLayout);
            flowLayoutPanel2.Controls.Add(_homeCapacityLayout);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel2.Location = new Point(0, 0);
            flowLayoutPanel2.Margin = new Padding(0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(452, 386);
            flowLayoutPanel2.TabIndex = 2;
            // 
            // _homeFullNameLayout
            // 
            _homeFullNameLayout.AutoSize = true;
            _homeFullNameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _homeFullNameLayout.BorderStyle = BorderStyle.FixedSingle;
            _homeFullNameLayout.Controls.Add(_homeFullNameLabel);
            _homeFullNameLayout.Controls.Add(_homeFullNameTextBox);
            _homeFullNameLayout.FlowDirection = FlowDirection.TopDown;
            _homeFullNameLayout.Location = new Point(3, 3);
            _homeFullNameLayout.Name = "_homeFullNameLayout";
            _homeFullNameLayout.Size = new Size(138, 50);
            _homeFullNameLayout.TabIndex = 3;
            // 
            // _homeFullNameLabel
            // 
            _homeFullNameLabel.AutoSize = true;
            _homeFullNameLabel.Font = new Font("Segoe UI", 10F);
            _homeFullNameLabel.Location = new Point(3, 0);
            _homeFullNameLabel.Name = "_homeFullNameLabel";
            _homeFullNameLabel.Size = new Size(106, 19);
            _homeFullNameLabel.TabIndex = 0;
            _homeFullNameLabel.Text = "Team Full Name";
            // 
            // _homeFullNameTextBox
            // 
            _homeFullNameTextBox.Location = new Point(3, 22);
            _homeFullNameTextBox.Name = "_homeFullNameTextBox";
            _homeFullNameTextBox.Size = new Size(130, 23);
            _homeFullNameTextBox.TabIndex = 1;
            // 
            // _homeShortNameLayout
            // 
            _homeShortNameLayout.AutoSize = true;
            _homeShortNameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _homeShortNameLayout.BorderStyle = BorderStyle.FixedSingle;
            _homeShortNameLayout.Controls.Add(_homeShortNameLabel);
            _homeShortNameLayout.Controls.Add(_homeShortNameTextBox);
            _homeShortNameLayout.FlowDirection = FlowDirection.TopDown;
            _homeShortNameLayout.Location = new Point(3, 59);
            _homeShortNameLayout.Name = "_homeShortNameLayout";
            _homeShortNameLayout.Size = new Size(138, 50);
            _homeShortNameLayout.TabIndex = 5;
            // 
            // _homeShortNameLabel
            // 
            _homeShortNameLabel.AutoSize = true;
            _homeShortNameLabel.Font = new Font("Segoe UI", 10F);
            _homeShortNameLabel.Location = new Point(3, 0);
            _homeShortNameLabel.Name = "_homeShortNameLabel";
            _homeShortNameLabel.Size = new Size(118, 19);
            _homeShortNameLabel.TabIndex = 0;
            _homeShortNameLabel.Text = "Team Short Name";
            // 
            // _homeShortNameTextBox
            // 
            _homeShortNameTextBox.Location = new Point(3, 22);
            _homeShortNameTextBox.Name = "_homeShortNameTextBox";
            _homeShortNameTextBox.Size = new Size(130, 23);
            _homeShortNameTextBox.TabIndex = 1;
            // 
            // _homeCapacityLayout
            // 
            _homeCapacityLayout.AutoSize = true;
            _homeCapacityLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _homeCapacityLayout.BorderStyle = BorderStyle.FixedSingle;
            _homeCapacityLayout.Controls.Add(_homeCapacityLabel);
            _homeCapacityLayout.Controls.Add(_homeCapacityNumeric);
            _homeCapacityLayout.FlowDirection = FlowDirection.TopDown;
            _homeCapacityLayout.Location = new Point(3, 115);
            _homeCapacityLayout.Name = "_homeCapacityLayout";
            _homeCapacityLayout.Size = new Size(138, 50);
            _homeCapacityLayout.TabIndex = 4;
            // 
            // _homeCapacityLabel
            // 
            _homeCapacityLabel.AutoSize = true;
            _homeCapacityLabel.Font = new Font("Segoe UI", 10F);
            _homeCapacityLabel.Location = new Point(3, 0);
            _homeCapacityLabel.Name = "_homeCapacityLabel";
            _homeCapacityLabel.Size = new Size(97, 19);
            _homeCapacityLabel.TabIndex = 0;
            _homeCapacityLabel.Text = "Team Capacity";
            // 
            // _homeCapacityNumeric
            // 
            _homeCapacityNumeric.Location = new Point(3, 22);
            _homeCapacityNumeric.Maximum = new decimal(new int[] { 11, 0, 0, 0 });
            _homeCapacityNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _homeCapacityNumeric.Name = "_homeCapacityNumeric";
            _homeCapacityNumeric.Size = new Size(130, 23);
            _homeCapacityNumeric.TabIndex = 5;
            _homeCapacityNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // _awayTeamTab
            // 
            _awayTeamTab.Controls.Add(flowLayoutPanel3);
            _awayTeamTab.Location = new Point(4, 24);
            _awayTeamTab.Name = "_awayTeamTab";
            _awayTeamTab.Size = new Size(452, 386);
            _awayTeamTab.TabIndex = 2;
            _awayTeamTab.Text = "Away Team";
            _awayTeamTab.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel3.Controls.Add(_awayFullNameLayout);
            flowLayoutPanel3.Controls.Add(_awayShortNameLayout);
            flowLayoutPanel3.Controls.Add(_awayCapacityLayout);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(0, 0);
            flowLayoutPanel3.Margin = new Padding(0);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(452, 386);
            flowLayoutPanel3.TabIndex = 3;
            // 
            // _awayFullNameLayout
            // 
            _awayFullNameLayout.AutoSize = true;
            _awayFullNameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _awayFullNameLayout.BorderStyle = BorderStyle.FixedSingle;
            _awayFullNameLayout.Controls.Add(_awayFullNameLabel);
            _awayFullNameLayout.Controls.Add(_awayFullNameTextBox);
            _awayFullNameLayout.FlowDirection = FlowDirection.TopDown;
            _awayFullNameLayout.Location = new Point(3, 3);
            _awayFullNameLayout.Name = "_awayFullNameLayout";
            _awayFullNameLayout.Size = new Size(138, 50);
            _awayFullNameLayout.TabIndex = 3;
            // 
            // _awayFullNameLabel
            // 
            _awayFullNameLabel.AutoSize = true;
            _awayFullNameLabel.Font = new Font("Segoe UI", 10F);
            _awayFullNameLabel.Location = new Point(3, 0);
            _awayFullNameLabel.Name = "_awayFullNameLabel";
            _awayFullNameLabel.Size = new Size(106, 19);
            _awayFullNameLabel.TabIndex = 0;
            _awayFullNameLabel.Text = "Team Full Name";
            // 
            // _awayFullNameTextBox
            // 
            _awayFullNameTextBox.Location = new Point(3, 22);
            _awayFullNameTextBox.Name = "_awayFullNameTextBox";
            _awayFullNameTextBox.Size = new Size(130, 23);
            _awayFullNameTextBox.TabIndex = 1;
            // 
            // _awayShortNameLayout
            // 
            _awayShortNameLayout.AutoSize = true;
            _awayShortNameLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _awayShortNameLayout.BorderStyle = BorderStyle.FixedSingle;
            _awayShortNameLayout.Controls.Add(_awayShortNameLabel);
            _awayShortNameLayout.Controls.Add(_awayShortNameTextBox);
            _awayShortNameLayout.FlowDirection = FlowDirection.TopDown;
            _awayShortNameLayout.Location = new Point(3, 59);
            _awayShortNameLayout.Name = "_awayShortNameLayout";
            _awayShortNameLayout.Size = new Size(138, 50);
            _awayShortNameLayout.TabIndex = 5;
            // 
            // _awayShortNameLabel
            // 
            _awayShortNameLabel.AutoSize = true;
            _awayShortNameLabel.Font = new Font("Segoe UI", 10F);
            _awayShortNameLabel.Location = new Point(3, 0);
            _awayShortNameLabel.Name = "_awayShortNameLabel";
            _awayShortNameLabel.Size = new Size(118, 19);
            _awayShortNameLabel.TabIndex = 0;
            _awayShortNameLabel.Text = "Team Short Name";
            // 
            // _awayShortNameTextBox
            // 
            _awayShortNameTextBox.Location = new Point(3, 22);
            _awayShortNameTextBox.Name = "_awayShortNameTextBox";
            _awayShortNameTextBox.Size = new Size(130, 23);
            _awayShortNameTextBox.TabIndex = 1;
            // 
            // _awayCapacityLayout
            // 
            _awayCapacityLayout.AutoSize = true;
            _awayCapacityLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _awayCapacityLayout.BorderStyle = BorderStyle.FixedSingle;
            _awayCapacityLayout.Controls.Add(_awayCapacityLabel);
            _awayCapacityLayout.Controls.Add(_awayCapacityNumeric);
            _awayCapacityLayout.FlowDirection = FlowDirection.TopDown;
            _awayCapacityLayout.Location = new Point(3, 115);
            _awayCapacityLayout.Name = "_awayCapacityLayout";
            _awayCapacityLayout.Size = new Size(138, 50);
            _awayCapacityLayout.TabIndex = 4;
            // 
            // _awayCapacityLabel
            // 
            _awayCapacityLabel.AutoSize = true;
            _awayCapacityLabel.Font = new Font("Segoe UI", 10F);
            _awayCapacityLabel.Location = new Point(3, 0);
            _awayCapacityLabel.Name = "_awayCapacityLabel";
            _awayCapacityLabel.Size = new Size(97, 19);
            _awayCapacityLabel.TabIndex = 0;
            _awayCapacityLabel.Text = "Team Capacity";
            // 
            // _awayCapacityNumeric
            // 
            _awayCapacityNumeric.Location = new Point(3, 22);
            _awayCapacityNumeric.Maximum = new decimal(new int[] { 11, 0, 0, 0 });
            _awayCapacityNumeric.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            _awayCapacityNumeric.Name = "_awayCapacityNumeric";
            _awayCapacityNumeric.Size = new Size(130, 23);
            _awayCapacityNumeric.TabIndex = 5;
            _awayCapacityNumeric.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(_createMatchButton);
            flowLayoutPanel4.Controls.Add(_statusLabel);
            flowLayoutPanel4.Location = new Point(12, 430);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(456, 25);
            flowLayoutPanel4.TabIndex = 1;
            // 
            // _createMatchButton
            // 
            _createMatchButton.AutoSize = true;
            _createMatchButton.Location = new Point(0, 0);
            _createMatchButton.Margin = new Padding(0);
            _createMatchButton.Name = "_createMatchButton";
            _createMatchButton.Size = new Size(88, 25);
            _createMatchButton.TabIndex = 0;
            _createMatchButton.Text = "Create Match";
            _createMatchButton.UseVisualStyleBackColor = true;
            _createMatchButton.Click += CreateMatchButton_Click;
            // 
            // _statusLabel
            // 
            _statusLabel.Anchor = AnchorStyles.Left;
            _statusLabel.AutoSize = true;
            _statusLabel.Location = new Point(91, 5);
            _statusLabel.Name = "_statusLabel";
            _statusLabel.Size = new Size(0, 15);
            _statusLabel.TabIndex = 1;
            // 
            // CreateMatchForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
            Controls.Add(flowLayoutPanel4);
            Controls.Add(_matchCreationTab);
            Name = "CreateMatchForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CreateMatchForm";
            _matchCreationTab.ResumeLayout(false);
            _commonTab.ResumeLayout(false);
            _commonTab.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            _matchNameLayout.ResumeLayout(false);
            _matchNameLayout.PerformLayout();
            _matchPassLayout.ResumeLayout(false);
            _matchPassLayout.PerformLayout();
            _scenarioTypeLayout.ResumeLayout(false);
            _scenarioTypeLayout.PerformLayout();
            _homeTeamTab.ResumeLayout(false);
            _homeTeamTab.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            _homeFullNameLayout.ResumeLayout(false);
            _homeFullNameLayout.PerformLayout();
            _homeShortNameLayout.ResumeLayout(false);
            _homeShortNameLayout.PerformLayout();
            _homeCapacityLayout.ResumeLayout(false);
            _homeCapacityLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_homeCapacityNumeric).EndInit();
            _awayTeamTab.ResumeLayout(false);
            _awayTeamTab.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            _awayFullNameLayout.ResumeLayout(false);
            _awayFullNameLayout.PerformLayout();
            _awayShortNameLayout.ResumeLayout(false);
            _awayShortNameLayout.PerformLayout();
            _awayCapacityLayout.ResumeLayout(false);
            _awayCapacityLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_awayCapacityNumeric).EndInit();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl _matchCreationTab;
        private TabPage _commonTab;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel _scenarioTypeLayout;
        private Label _scenarioTypeLabel;
        private ComboBox _scenarioTypeComboBox;
        private FlowLayoutPanel _matchNameLayout;
        private Label _matchNameLabel;
        private TextBox _matchNameTextBox;
        private FlowLayoutPanel _matchPassLayout;
        private Label _matchPassLabel;
        private TextBox _matchPassTextBox;
        private TabPage _awayTeamTab;
        private TabPage _homeTeamTab;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel _homeFullNameLayout;
        private Label _homeFullNameLabel;
        private TextBox _homeFullNameTextBox;
        private FlowLayoutPanel _homeShortNameLayout;
        private Label _homeShortNameLabel;
        private TextBox _homeShortNameTextBox;
        private FlowLayoutPanel _homeCapacityLayout;
        private Label _homeCapacityLabel;
        private NumericUpDown _homeCapacityNumeric;
        private FlowLayoutPanel flowLayoutPanel3;
        private FlowLayoutPanel _awayFullNameLayout;
        private Label _awayFullNameLabel;
        private TextBox _awayFullNameTextBox;
        private FlowLayoutPanel _awayShortNameLayout;
        private Label _awayShortNameLabel;
        private TextBox _awayShortNameTextBox;
        private FlowLayoutPanel _awayCapacityLayout;
        private Label _awayCapacityLabel;
        private NumericUpDown _awayCapacityNumeric;
        private FlowLayoutPanel flowLayoutPanel4;
        private Button _createMatchButton;
        private Label _statusLabel;
    }
}