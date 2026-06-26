namespace Football.ServerBrowser
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            _tabs = new TabControl();
            tabLogin = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            _loginUser = new TextBox();
            _loginPass = new TextBox();
            _loginBtn = new Button();
            tabRegister = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            _regUser = new TextBox();
            _regPass = new TextBox();
            _regPass2 = new TextBox();
            _regBtn = new Button();
            _lblMsg = new Label();
            _statusStrip = new StatusStrip();
            _statusStripLabel = new ToolStripStatusLabel();
            _regPlayerName = new TextBox();
            _tabs.SuspendLayout();
            tabLogin.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tabRegister.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _tabs
            // 
            _tabs.Controls.Add(tabLogin);
            _tabs.Controls.Add(tabRegister);
            _tabs.Location = new Point(14, 14);
            _tabs.Margin = new Padding(4, 3, 4, 3);
            _tabs.Name = "_tabs";
            _tabs.SelectedIndex = 0;
            _tabs.Size = new Size(373, 242);
            _tabs.TabIndex = 2;
            // 
            // tabLogin
            // 
            tabLogin.Controls.Add(tableLayoutPanel1);
            tabLogin.Location = new Point(4, 24);
            tabLogin.Margin = new Padding(4, 3, 4, 3);
            tabLogin.Name = "tabLogin";
            tabLogin.Size = new Size(365, 214);
            tabLogin.TabIndex = 0;
            tabLogin.Text = "Login";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(_loginUser, 0, 0);
            tableLayoutPanel1.Controls.Add(_loginPass, 0, 1);
            tableLayoutPanel1.Controls.Add(_loginBtn, 0, 2);
            tableLayoutPanel1.Location = new Point(30, 35);
            tableLayoutPanel1.Margin = new Padding(0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(300, 140);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // _loginUser
            // 
            _loginUser.Location = new Point(4, 3);
            _loginUser.Margin = new Padding(4, 3, 4, 3);
            _loginUser.Name = "_loginUser";
            _loginUser.PlaceholderText = "Username";
            _loginUser.Size = new Size(292, 23);
            _loginUser.TabIndex = 1;
            // 
            // _loginPass
            // 
            _loginPass.Location = new Point(4, 49);
            _loginPass.Margin = new Padding(4, 3, 4, 3);
            _loginPass.Name = "_loginPass";
            _loginPass.PasswordChar = '●';
            _loginPass.PlaceholderText = "Password";
            _loginPass.Size = new Size(292, 23);
            _loginPass.TabIndex = 0;
            // 
            // _loginBtn
            // 
            _loginBtn.Location = new Point(4, 95);
            _loginBtn.Margin = new Padding(4, 3, 4, 3);
            _loginBtn.Name = "_loginBtn";
            _loginBtn.Size = new Size(292, 40);
            _loginBtn.TabIndex = 2;
            _loginBtn.Text = "Login";
            _loginBtn.Click += HandleLoginAction;
            // 
            // tabRegister
            // 
            tabRegister.Controls.Add(tableLayoutPanel2);
            tabRegister.Location = new Point(4, 24);
            tabRegister.Margin = new Padding(4, 3, 4, 3);
            tabRegister.Name = "tabRegister";
            tabRegister.Size = new Size(365, 214);
            tabRegister.TabIndex = 1;
            tabRegister.Text = "Register";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(_regPlayerName, 0, 0);
            tableLayoutPanel2.Controls.Add(_regBtn, 0, 4);
            tableLayoutPanel2.Controls.Add(_regPass2, 0, 3);
            tableLayoutPanel2.Controls.Add(_regPass, 0, 2);
            tableLayoutPanel2.Controls.Add(_regUser, 0, 1);
            tableLayoutPanel2.Location = new Point(30, 10);
            tableLayoutPanel2.Margin = new Padding(0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Size = new Size(300, 195);
            tableLayoutPanel2.TabIndex = 5;
            // 
            // _regUser
            // 
            _regUser.Location = new Point(4, 42);
            _regUser.Margin = new Padding(4, 3, 4, 3);
            _regUser.Name = "_regUser";
            _regUser.PlaceholderText = "Username";
            _regUser.Size = new Size(292, 23);
            _regUser.TabIndex = 3;
            // 
            // _regPass
            // 
            _regPass.Location = new Point(4, 81);
            _regPass.Margin = new Padding(4, 3, 4, 3);
            _regPass.Name = "_regPass";
            _regPass.PasswordChar = '●';
            _regPass.PlaceholderText = "Password";
            _regPass.Size = new Size(292, 23);
            _regPass.TabIndex = 1;
            // 
            // _regPass2
            // 
            _regPass2.Location = new Point(4, 120);
            _regPass2.Margin = new Padding(4, 3, 4, 3);
            _regPass2.Name = "_regPass2";
            _regPass2.PasswordChar = '●';
            _regPass2.PlaceholderText = "Password (Again)";
            _regPass2.Size = new Size(292, 23);
            _regPass2.TabIndex = 0;
            // 
            // _regBtn
            // 
            _regBtn.Location = new Point(4, 159);
            _regBtn.Margin = new Padding(4, 3, 4, 3);
            _regBtn.Name = "_regBtn";
            _regBtn.Size = new Size(292, 33);
            _regBtn.TabIndex = 4;
            _regBtn.Text = "Register";
            _regBtn.Click += HandleRegisterAction;
            // 
            // _lblMsg
            // 
            _lblMsg.AutoEllipsis = true;
            _lblMsg.Location = new Point(14, 265);
            _lblMsg.Margin = new Padding(4, 0, 4, 0);
            _lblMsg.Name = "_lblMsg";
            _lblMsg.Size = new Size(373, 29);
            _lblMsg.TabIndex = 1;
            _lblMsg.Text = "Initializing cluster state network connection...";
            // 
            // _statusStrip
            // 
            _statusStrip.Items.AddRange(new ToolStripItem[] { _statusStripLabel });
            _statusStrip.Location = new Point(0, 303);
            _statusStrip.Name = "_statusStrip";
            _statusStrip.Padding = new Padding(1, 0, 16, 0);
            _statusStrip.Size = new Size(401, 22);
            _statusStrip.SizingGrip = false;
            _statusStrip.TabIndex = 0;
            // 
            // _statusStripLabel
            // 
            _statusStripLabel.Name = "_statusStripLabel";
            _statusStripLabel.Size = new Size(222, 17);
            _statusStripLabel.Text = "Network State: Syncing Pipeline Gateway";
            // 
            // _regPlayerName
            // 
            _regPlayerName.Location = new Point(4, 3);
            _regPlayerName.Margin = new Padding(4, 3, 4, 3);
            _regPlayerName.Name = "_regPlayerName";
            _regPlayerName.PlaceholderText = "Playername (Visible)";
            _regPlayerName.Size = new Size(292, 23);
            _regPlayerName.TabIndex = 5;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(401, 325);
            Controls.Add(_statusStrip);
            Controls.Add(_lblMsg);
            Controls.Add(_tabs);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "LoginForm";
            Text = "Server Browser";
            _tabs.ResumeLayout(false);
            tabLogin.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tabRegister.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private TabControl _tabs;
        private TabPage tabLogin;
        private TextBox _loginPass;
        private TextBox _loginUser;
        private Button _loginBtn;
        private TabPage tabRegister;
        private TextBox _regPass2;
        private TextBox _regPass;
        private TextBox _regUser;
        private Button _regBtn;
        private Label _lblMsg;
        private StatusStrip _statusStrip;
        private ToolStripStatusLabel _statusStripLabel;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox _regPlayerName;
    }
}