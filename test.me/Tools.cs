using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using testme.Models;

namespace testme
{
    public static class Tools
    {
        public static bool isSessionActual (IMemoryCache _cache) {
            string sessionId;
            return _cache.TryGetValue("sessionId", out sessionId);
        }

        public static Guid getCurrentSessionId(IMemoryCache _cache)
        {
            string sessionId;
            _cache.TryGetValue("sessionId", out sessionId);

            Guid sessionIdGuid;
            Guid.TryParse(sessionId, out sessionIdGuid);

            return sessionIdGuid;
        }

        public static Session getCurrentSession(IMemoryCache _cache)
        {
            string sessionId;
            _cache.TryGetValue("sessionId", out sessionId);

            Guid sessionIdGuid;
            Guid.TryParse(sessionId, out sessionIdGuid);

            using(ApplicationContext db = new ApplicationContext())
            {
                return db.Sessions.FirstOrDefault(x => x.isActual == true && x.SessionID == sessionIdGuid);
            }
        }

        public static User getCurrentUser(IMemoryCache _cache)
        {
            Session currentSession = getCurrentSession(_cache);

            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Users.FirstOrDefault(x => x.Id == currentSession.UserId);
            }
        }
    }
}
