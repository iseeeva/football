using Football.Serialization;
using Football.ServerBrowser.Messages;

namespace Football.ServerBrowser
{
    public partial class LoginForm : Form
    {
        private bool _connected;
        private SobeeClient? _socketClient => Program.AppContext?.ServerConnection.Client;

        public LoginForm()
        {
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RenderUserInterfaceStatus("Attempting infrastructure gateway link…", TextTheme.TextMuted);
            UpdateConnectivityStatus(false, "Connecting...");

            if (Program.AppContext == null)
            {
                HandleDisconnection("Application context is not initialized.");
                return;
            }

            try
            {
                var connection = Program.AppContext.ServerConnection;
                await connection.InitializeAsync(true);

                if (IsDisposed || Disposing)
                    return;

                if (connection.Client != null)
                {
                    InitializeConnectionPipeline();
                }
                else
                {
                    throw new Exception("Socket initialization returned null.");
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing)
                    return;

                HandleDisconnection(ex.Message);
            }
        }

        private void InitializeConnectionPipeline()
        {
            _connected = true;
            UpdateConnectivityStatus(true, "Gateway Linked");
            RenderUserInterfaceStatus("Ready for secure authorization.", TextTheme.Green);
        }

        private async Task ExecuteAuth(Func<Task<IMessage>> authFunc, Action onSuccess)
        {
            _tabs.Enabled = false;

            try
            {
                if (IsDisposed || Disposing)
                    return;

                IMessage response = await authFunc();
                if (response is BrowserStatusResponseMessage authResp)
                {
                    if (authResp.Status)
                    {
                        RenderUserInterfaceStatus($"✓ {authResp.StatusMessage}", TextTheme.Green);
                        onSuccess();
                    }
                    else
                    {
                        RenderUserInterfaceStatus($"✗ {authResp.StatusMessage}", TextTheme.Red);
                    }
                }
                else
                {
                    RenderUserInterfaceStatus("✗ Unexpected response format from server.", TextTheme.Red);
                }
            }
            catch (Exception ex)
            {
                if (IsDisposed || Disposing)
                    return;

                RenderUserInterfaceStatus($"Transport error: {ex.Message}", TextTheme.Red);
                HandleDisconnection(ex.Message);
            }
            finally
            {
                if (!IsDisposed && !Disposing)
                {
                    _tabs.Enabled = true;
                    if (!_connected)
                    {
                        UpdateConnectivityStatus(false, "Disconnected");
                    }
                }
            }
        }

        private async void HandleLoginAction(object sender, EventArgs e)
        {
            if (_socketClient == null)
            {
                RenderUserInterfaceStatus("Not connected to server.", TextTheme.Red);
                return;
            }

            string user = _loginUser.Text.Trim();
            string pass = _loginPass.Text;

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                RenderUserInterfaceStatus("Username and password are required.", TextTheme.Orange);
                return;
            }

            RenderUserInterfaceStatus("Authenticating...", TextTheme.TextMuted);

            await ExecuteAuth(
                () => _socketClient.SendRequestAsync(new BrowserLoginAuthMessage(user, pass)),
                () => Program.AppContext?.NavigateTo(new MatchBrowserForm())
            );
        }

        private async void HandleRegisterAction(object sender, EventArgs e)
        {
            if (_socketClient == null)
            {
                RenderUserInterfaceStatus("Not connected to server.", TextTheme.Red);
                return;
            }

            string player = _regPlayerName.Text.Trim();
            string user = _regUser.Text.Trim();
            string pass1 = _regPass.Text;
            string pass2 = _regPass2.Text;

            if (string.IsNullOrWhiteSpace(player) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass1))
            {
                RenderUserInterfaceStatus("All fields are required.", TextTheme.Orange);
                return;
            }

            if (pass1 != pass2)
            {
                RenderUserInterfaceStatus("Passwords do not match.", TextTheme.Orange);
                return;
            }

            RenderUserInterfaceStatus("Registering account...", TextTheme.TextMuted);

            await ExecuteAuth(
                () => _socketClient.SendRequestAsync(new BrowserRegisterAuthMessage(player, user, pass1)),
                () =>
                {
                    _tabs.SelectedIndex = 0;
                    _loginUser.Text = user;
                    _loginPass.Clear();

                    _regPlayerName.Clear();
                    _regUser.Clear();
                    _regPass.Clear();
                    _regPass2.Clear();

                    RenderUserInterfaceStatus("Registration successful. Please log in.", TextTheme.Green);
                }
            );
        }

        private void HandleDisconnection(string message)
        {
            _connected = false;
            _socketClient?.Dispose();

            if (IsDisposed || Disposing) return;

            UpdateConnectivityStatus(false, "Disconnected");
            RenderUserInterfaceStatus($"Connection lost: {message}", TextTheme.Red);
        }

        private void UpdateConnectivityStatus(bool active, string display)
        {
            _statusStripLabel.Text = $"Network State: {display}";
            _loginBtn.Enabled = active;
            _regBtn.Enabled = active;
        }

        private void RenderUserInterfaceStatus(string text, TextTheme style)
            => FormHelper.RenderColoredText(_lblMsg, text, style);
    }
}