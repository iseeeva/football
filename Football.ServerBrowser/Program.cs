namespace Football.ServerBrowser
{
    // ------------------- TODO LIST ---------------------
    // TODO1: MatchPassword implementation.
    // TODO2: Match join and "ready" feature. (wait for every players to be ready before starting match)
    // TODO3: MatchOwner doesn't have owner features on match tab. (start match, kick player etc.)
    // TODO4: Figure out apperance creation feature.
    // ---------------------------------------------------

    internal class AppLifetimeContext : ApplicationContext
    {
        public ServerConnection ServerConnection { get; }

        public AppLifetimeContext()
        {
            ServerConnection = new ServerConnection();
        }

        public void Start()
        {
            NavigateTo(new LoginForm());
        }

        public void NavigateTo(Form nextForm)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(() => NavigateTo(nextForm));
                return;
            }

            Form? oldForm = MainForm;
            nextForm.FormClosed += OnFormClosed;
            MainForm = nextForm;
            nextForm.Show();

            if (oldForm != null)
            {
                oldForm.FormClosed -= OnFormClosed;
                oldForm.Close();
                oldForm.Dispose();
            }
        }

        private void OnFormClosed(object? sender, FormClosedEventArgs e)
        {
            if (sender == MainForm) ExitThread();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) ServerConnection?.Dispose();
            base.Dispose(disposing);
        }
    }

    internal class ServerConnection : IDisposable
    {
        private readonly SemaphoreSlim _connectionLock = new(1, 1);
        public SobeeClient? Client { get; private set; }

        public async Task InitializeAsync(bool force = false)
        {
            await _connectionLock.WaitAsync();
            try
            {
                if (!force && Client != null)
                    return;

                Client?.Dispose();
                Client = new SobeeClient("localhost", 3000);
                await Client.ConnectAsync();
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task ResetConnectionAsync()
        {
            await InitializeAsync(force: true);
        }

        public void Dispose()
        {
            Client?.Dispose();
            _connectionLock.Dispose();
        }
    }

    internal static class Program
    {
        public static AppLifetimeContext AppContext { get; private set; } = null!;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppContext = new AppLifetimeContext();
            AppContext.Start();

            _ = AppContext.ServerConnection.InitializeAsync();
            Application.Run(AppContext);
        }
    }
}