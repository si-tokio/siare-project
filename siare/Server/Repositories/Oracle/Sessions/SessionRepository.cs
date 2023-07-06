using siare.Shared.Entities;
using siare.Server.Helpers;

namespace siare.Server.Repositories.Oracle.Sessions
{
  public class SessionRepository : ISessionRepository
  {
    public async Task CreateSessionAsync(string sessionId, int userId, DateTime expiresAt)
    {
      var sql = $@"
INSERT INTO sessions(session_id, user_id, expires_at)
VALUES(:{nameof(sessionId)}, :{nameof(userId)}, :{nameof(expiresAt)})
";
      await OracleHelper.ExecuteAsync(sql, new { sessionId, userId, expiresAt });
    }

    public async Task DeleteSessionAsync(string sessionId)
    {
      var sql = $"DELETE FROM sessions WHERE session_id = :{nameof(sessionId)}";
      await OracleHelper.ExecuteAsync(sql, new { sessionId });
    }

    public async Task DeleteSessionAsync(string sessionId, string userId)
    {
      var sql = $"DELETE FROM sessions WHERE session_id = :{nameof(sessionId)} AND  user_id = :{nameof(userId)}";
      await OracleHelper.ExecuteAsync(sql, new { sessionId, userId });
    }

    public async Task<IEnumerable<Session>> GetAllSessionsAsync()
    {
      var sql = "SELECT * FROM sessions";
      var sessions = await OracleHelper.GetAsync<Session>(sql);
      return sessions;
    }

    public async Task<Session> GetSessionAsync(string sessionId)
    {
      var sql = $"SELECT * FROM sessions WHERE session_id = :{nameof(sessionId)}";
      var session = await OracleHelper.GetOneAsync<Session, object>(sql, new { sessionId });
      return session;
    }

    public async Task UpdateSessionAsync(string sessionId, int userId, DateTime expiresAt)
    {
      var sql = $@"
UPDATE sessions 
SET user_id = :{nameof(userId)}, last_updated = SYSDATE, expires_at = :{nameof(expiresAt)} 
WHERE session_id = :{nameof(sessionId)}
";
      await OracleHelper.ExecuteAsync(sql, new { sessionId, userId, expiresAt });
    }
  }
}
