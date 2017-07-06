namespace DFlow.Tenants.Lib.Tests.Helpers
{
    public class TenantData
    {
        public TenantData(string owner)
        {
            Owner = owner;
        }

        public string Owner { get; set; }
    }
}
