using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Football.Database
{
    public class ServerBrowserDbContext : DbContext
    {
        public DbSet<UserContext> BrowserUsers => Set<UserContext>();
        public DbSet<SessionContext> BrowserSessions => Set<SessionContext>();
        public DbSet<MatchContext> ActiveMatches => Set<MatchContext>();

        private static readonly object _initLock = new();
        private static bool _dbInitialized;

        public ServerBrowserDbContext(DbContextOptions<ServerBrowserDbContext> options) : base(options)
        {
            EnsureDatabaseCreated();
        }

        public ServerBrowserDbContext()
        {
            EnsureDatabaseCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (options.IsConfigured)
                return;

            var dbFolder = Path.Combine(AppContext.BaseDirectory, "localDatabase");
            Directory.CreateDirectory(dbFolder);

            var dbPath = Path.Combine(dbFolder, "browserDatabase.db");
            options.UseSqlite($"Data Source={dbPath}");
        }

        private void EnsureDatabaseCreated()
        {
            if (_dbInitialized) return;
            lock (_initLock)
            {
                if (_dbInitialized) return;
                Database.EnsureCreated();
                _dbInitialized = true;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserContext>(e =>
            {
                e.HasIndex(u => u.UserName).IsUnique();
                e.Property(u => u.PlayerName).IsRequired().HasMaxLength(64);
                e.Property(u => u.UserName).IsRequired().HasMaxLength(64);
                e.Property(u => u.PasswordHash).IsRequired();

                e.OwnsOne(u => u.Appearance, b =>
                {
                    b.Property(a => a.Parts).HasColumnName("AppearanceParts");
                });
            });

            modelBuilder.Entity<SessionContext>(e =>
            {
                e.HasIndex(s => s.SessionId).IsUnique();
                e.HasOne<UserContext>()
                 .WithOne()
                 .HasForeignKey<SessionContext>(s => s.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MatchContext>(e =>
            {
                e.HasIndex(m => m.MatchId).IsUnique();
                e.Property(m => m.MatchName).IsRequired();

                e.HasOne<UserContext>()
                 .WithOne()
                 .HasForeignKey<MatchContext>(m => m.OwnerId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }

        // --- Data Access Methods ---

        public async Task<bool> AddUserAsync(UserContext user, CancellationToken ct = default)
        {
            BrowserUsers.Add(user);
            return await SaveChangesSafeAsync(ct);
        }

        public Task<UserContext?> GetUserAsync(Guid id, CancellationToken ct = default)
            => BrowserUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public Task<bool> RemoveUserAsync(Guid id, CancellationToken ct = default)
            => BrowserUsers.Where(u => u.Id == id).ExecuteDeleteAsync(ct).ContinueWith(t => t.Result > 0, ct);

        public async Task<bool> AddSessionAsync(SessionContext session, CancellationToken ct = default)
        {
            BrowserSessions.Add(session);
            return await SaveChangesSafeAsync(ct);
        }

        public Task<SessionContext?> GetSessionAsync(Guid id, CancellationToken ct = default)
            => BrowserSessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);

        public Task<bool> RemoveSessionAsync(Guid id, CancellationToken ct = default)
            => BrowserSessions.Where(s => s.Id == id).ExecuteDeleteAsync(ct).ContinueWith(t => t.Result > 0, ct);

        public async Task<bool> AddMatchAsync(MatchContext match, CancellationToken ct = default)
        {
            ActiveMatches.Add(match);
            return await SaveChangesSafeAsync(ct);
        }

        public Task<MatchContext?> GetMatchAsync(Guid id, CancellationToken ct = default)
            => ActiveMatches.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);

        public Task<bool> RemoveMatchAsync(Guid id, CancellationToken ct = default)
            => ActiveMatches.Where(m => m.Id == id).ExecuteDeleteAsync(ct).ContinueWith(t => t.Result > 0, ct);

        // --- Lookup Methods ---

        public Task<UserContext?> GetUserByUserNameAsync(string userName, CancellationToken ct = default)
            => BrowserUsers.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == userName, ct);

        public Task<MatchContext?> GetMatchByMatchIdAsync(Guid matchId, CancellationToken ct = default)
            => ActiveMatches.AsNoTracking().FirstOrDefaultAsync(m => m.MatchId == matchId, ct);

        public Task<SessionContext?> GetSessionBySocketAsync(Guid socketSessionId, CancellationToken ct = default)
            => BrowserSessions.AsNoTracking().FirstOrDefaultAsync(s => s.SessionId == socketSessionId, ct);

        private async Task<bool> SaveChangesSafeAsync(CancellationToken ct)
        {
            try
            {
                return await SaveChangesAsync(ct) > 0;
            }
            catch (DbUpdateException)
            {
                // TODO: inject ILogger and log here once logging is wired up.
                return false;
            }
        }

        public class UserContext
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();

            public string PlayerName { get; set; } = string.Empty;
            public string UserName { get; set; } = string.Empty;
            public string PasswordHash { get; set; } = string.Empty;

            public PlayerAppearanceMessage Appearance { get; set; } = new();

            public UserContext() { }
            public UserContext(string playerName, string userName, string passwordHash)
            {
                PlayerName = playerName;
                UserName = userName;
                PasswordHash = passwordHash;
            }
        }

        public class SessionContext
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid UserId { get; set; }
            public Guid SessionId { get; set; }

            public SessionContext() { }
            public SessionContext(Guid userId, Guid sessionId)
            {
                UserId = userId;
                SessionId = sessionId;
            }
        }

        public class MatchContext
        {
            [Key]
            public Guid Id { get; set; } = Guid.NewGuid();

            public Guid OwnerId { get; set; }

            public Guid MatchId { get; set; }
            public string MatchName { get; set; } = string.Empty;
            public string MatchPasswordHash { get; set; } = string.Empty;

            public MatchContext() { }
            public MatchContext(Guid ownerUserId, Guid matchId, string matchName, string matchPasswordHash)
            {
                OwnerId = ownerUserId;
                MatchId = matchId;
                MatchName = matchName;
                MatchPasswordHash = matchPasswordHash;
            }
        }
    }
}