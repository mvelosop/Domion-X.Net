using System;

namespace Domion.Lib.Data
{
    public class RepositoryUpdateConcurrencyException : Exception
    {
        public RepositoryUpdateConcurrencyException(string message)
            : base(message)
        {
        }
    }
}
