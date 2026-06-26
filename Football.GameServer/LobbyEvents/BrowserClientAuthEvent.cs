using Football.Common;
using Football.Database;
using Football.GameServer.Game;
using Football.GameServer.Lobby;
using Football.Network.Messaging;
using Football.ServerBrowser.Messages;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Football.GameServer.LobbyEvents
{
    public class BrowserClientAuthEvent
    {
        private static readonly ILogger _log = LogFactory.GetContextForType<BrowserClientAuthEvent>();

        private const int MaxUserNameLength = 20;
        private const int MaxPlayerNameLength = 20;
        private const int MaxPasswordLength = 100;

        public static async void LoginAuthRequestReceived(object? sender, MessageEventArgs e)
        {
            try
            {
                if (sender is not LobbyRoom authRoom) return;
                if (e.Handler is not LobbyUser authUser) return;
                if (e.Message is not BrowserLoginAuthMessage authInformation) return;
                if (authRoom.Owner is not GameHub) return;

                if (authUser.Socket == null)
                {
                    _log.Error("[LoginAuthRequestReceived] Session ({SessionId}) socket is null.", authUser.Id);
                    return;
                }

                if (string.IsNullOrWhiteSpace(authInformation.Username) || string.IsNullOrWhiteSpace(authInformation.Password))
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "Username or password cannot be empty."));
                    return;
                }

                using var db = new ServerBrowserDbContext();
                var user = await db.GetUserByUserNameAsync(authInformation.Username);

                if (user is null || !PasswordHasher.Verify(authInformation.Password, user.PasswordHash))
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "User not found, please try signing up."));
                    return;
                }

                bool sessionAdded = await db.AddSessionAsync(new ServerBrowserDbContext.SessionContext(user.Id, authUser.Socket.Id));

                if (!sessionAdded)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "User already has an active session."));
                    return;
                }

                authUser.SendMessage(new BrowserStatusResponseMessage(true, "User found, session created."));
                _log.Information("[LoginAuthRequestReceived] Login succeeded for user {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _log.Error(ex, "[LoginAuthRequestReceived] error");
            }
        }

        public static async void RegisterAuthRequestReceived(object? sender, MessageEventArgs e)
        {
            try
            {
                if (sender is not LobbyRoom authRoom) return;
                if (e.Handler is not LobbyUser authUser) return;
                if (e.Message is not BrowserRegisterAuthMessage authInformation) return;
                if (authRoom.Owner is not GameHub) return;

                _log.Information("[RegisterAuthRequestReceived] from {UserId}", authUser.Id);

                if (
                    string.IsNullOrWhiteSpace(authInformation.PlayerName) ||
                    string.IsNullOrWhiteSpace(authInformation.UserName) ||
                    string.IsNullOrWhiteSpace(authInformation.Password)
                    )
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "PlayerName, UserName or password cannot be empty."));
                    return;
                }

                if (authInformation.UserName.Length > MaxUserNameLength)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, $"Username can be max {MaxUserNameLength} character length."));
                    return;
                }

                if (authInformation.PlayerName.Length > MaxPlayerNameLength)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, $"PlayerName can be max {MaxPlayerNameLength} character length."));
                    return;
                }

                if (authInformation.Password.Length > MaxPasswordLength)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, $"Password can be max {MaxPasswordLength} character length."));
                    return;
                }

                using var db = new ServerBrowserDbContext();

                bool userNameExists = await db.BrowserUsers.AnyAsync(u => u.UserName == authInformation.UserName);
                if (userNameExists)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "Username already taken, please choose another."));
                    return;
                }

                bool playerNameExists = await db.BrowserUsers.AnyAsync(u => u.PlayerName == authInformation.PlayerName);
                if (playerNameExists)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "PlayerName already taken, please choose another."));
                    return;
                }

                string passwordHash = PasswordHasher.Hash(authInformation.Password);
                var isUserRegistered = await db.AddUserAsync(new ServerBrowserDbContext.UserContext(authInformation.PlayerName, authInformation.UserName, passwordHash));

                if (isUserRegistered)
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(true, "Registration successful."));
                    _log.Information("[RegisterAuthRequestReceived] New user registered: {UserName} - {UserId}", authInformation.UserName, authUser.Id);
                }
                else
                {
                    authUser.SendMessage(new BrowserStatusResponseMessage(false, "Registration failed."));
                    _log.Information("[RegisterAuthRequestReceived] Registration failed for: {UserName} - {UserId}", authInformation.UserName, authUser.Id);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "[RegisterAuthRequestReceived] error");
            }
        }
    }
}