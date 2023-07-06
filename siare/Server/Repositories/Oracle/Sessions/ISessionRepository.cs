using siare.Shared.Entities;

namespace siare.Server.Repositories.Oracle.Sessions
{
  public interface ISessionRepository
  {
    // Create a new session
    Task CreateSessionAsync(string sessionId, int userId, DateTime expiresAt);

    // Read a session by its ID
    Task<Session> GetSessionAsync(string sessionId);

    // Read all sessions
    Task<IEnumerable<Session>> GetAllSessionsAsync();

    // Update a session
    Task UpdateSessionAsync(string sessionId, int userId, DateTime expiresAt);

    // Delete a session
    Task DeleteSessionAsync(string sessionId);

    // Delete a session
    Task DeleteSessionAsync(string sessionId, string userId);
  }
}
