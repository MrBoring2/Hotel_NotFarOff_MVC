using Hotel_NotFarOff.Models;
using System.Collections.Concurrent;

namespace Hotel_NotFarOff.Services
{
    public class ConnectingService
    {
        public static ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();
        public static void AddUser(User user)
        {
            ConnectedUsers.TryAdd(user.Id, user.Login);
        }
        public static bool RemoveUser(string userId)
        {
            string value;
            return ConnectedUsers.TryRemove(userId, out value);
        }
        public static bool IsUserAuth(string userId)
        {
            string value;
            ConnectedUsers.TryGetValue(userId, out value);
            if (value == "" || value == null)
            {
                return false;
            }
            else return true;
        }
    }
}
