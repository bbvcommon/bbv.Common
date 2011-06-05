namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System.Configuration;

    using bbv.Common.Bootstrapper.Configuration;

    public class CustomExtensionWithConfigurationWhichKnowsName : ICustomExtensionWithConfiguration, IConsumeConfigurationSection, IHaveConfigurationSectionName
    {
        public bool SectionNameAcquired { get; private set; }

        public FakeConfigurationSection AppliedSection { get; private set; }

        public string SectionName
        {
            get
            {
                this.SectionNameAcquired = true;

                return "FakeConfigurationSection";
            }
        }

        public void Apply(ConfigurationSection section)
        {
            this.AppliedSection = section as FakeConfigurationSection;
        }

        public void Dispose()
        {
        }
    }
}