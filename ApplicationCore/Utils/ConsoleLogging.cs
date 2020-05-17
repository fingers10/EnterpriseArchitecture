namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Utils
{
    public sealed class ConsoleLogging
    {
        public ConsoleLogging(bool enable)
        {
            Enable = enable;
        }

        public bool Enable { get; }
    }
}
