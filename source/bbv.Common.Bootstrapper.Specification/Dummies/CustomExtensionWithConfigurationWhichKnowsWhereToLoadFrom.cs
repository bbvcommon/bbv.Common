namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System;
    using System.Configuration;

    using bbv.Common.Bootstrapper.Configuration;

    public class CustomExtensionWithConfigurationWhichKnowsWhereToLoadFrom : ICustomExtensionWithConfiguration, ILoadConfigurationSection
    {
        public string SectionAcquired { get; private set; }

        public FakeConfigurationSection AppliedSection { get; private set; }

        public void Dispose()
        {
        }

        public void Apply(ConfigurationSection section)
        {
            this.AppliedSection = section as FakeConfigurationSection;
        }

        public ConfigurationSection GetSection(string sectionName)
        {
            this.SectionAcquired = sectionName;

            return new FakeConfigurationSection("KnowsLoading");
        }
    }
}