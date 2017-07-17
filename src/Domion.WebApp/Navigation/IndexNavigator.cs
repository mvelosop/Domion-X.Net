using Microsoft.AspNetCore.Http;

namespace Domion.WebApp.Navigation
{
    public class IndexNavigator
    {
        private readonly ISession Session;

        public IndexNavigator(ISession session)
        {
            Session = session;
        }

        
    }
}
