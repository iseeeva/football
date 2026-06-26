using Football.GameServer.Messages;
using Football.ServerBrowser.Messages;

namespace Football.ServerBrowser
{
    public partial class MatchCreateForm : Form
    {
        private SobeeClient? _socketClient => Program.AppContext?.ServerConnection.Client;

        public MatchCreateForm()
        {
            InitializeComponent();
            InitializeAgain();
        }

        private void InitializeAgain()
        {
            _scenarioTypeComboBox.Items.AddRange(Enum.GetValues(typeof(ScenarioType)).Cast<object>().ToArray());
        }

        private async void CreateMatchButton_Click(object? sender, EventArgs e)
        {
            // TODO: KONTROLLER EKSIK
            if (string.IsNullOrWhiteSpace(_matchNameTextBox.Text))
            {
                FormHelper.RenderColoredText(_statusLabel, "Please enter a match name.", TextTheme.Red);
                return;
            }

            await FormHelper.ExecuteRequestAsync<BrowserStatusResponseMessage>(
                requestFunc: () => _socketClient?.SendRequestAsync(
                    new BrowserCreateMatchRequestMessage(
                        _matchNameTextBox.Text.Trim(), _matchPassTextBox.Text.Trim(), (ScenarioType)_scenarioTypeComboBox.SelectedIndex,
                        _homeFullNameTextBox.Text.Trim(), _homeShortNameTextBox.Text.Trim(), (int)_homeCapacityNumeric.Value,
                        _awayFullNameTextBox.Text.Trim(), _awayShortNameTextBox.Text.Trim(), (int)_awayCapacityNumeric.Value
                    )
                ),
                onSuccess: (response) =>
                {
                    Program.AppContext?.NavigateTo(new MatchBrowserForm());
                },
                onExpection: (ex) =>
                {
                    Program.AppContext?.NavigateTo(new LoginForm());
                },
                onFinally: () =>
                {
                    _createMatchButton.Enabled = false;
                },
                _statusLabel
            );
        }
    }
}
