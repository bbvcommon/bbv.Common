namespace bbv.Common.Bootstrapper.Specification
{
    using bbv.Common.Bootstrapper.Specification.Dummies;

    using Machine.Specifications;

    public class BootstrapperWithBehaviorSpecification
    {
        protected static CustomExtensionWithBehaviorStrategy Strategy;

        protected static CustomExtensionBase First;

        protected static CustomExtensionBase Second;

        protected static IBootstrapper<ICustomExtension> Bootstrapper;

        Establish context = () =>
        {
            Bootstrapper = new DefaultBootstrapper<ICustomExtension>();

            Strategy = new CustomExtensionWithBehaviorStrategy();
            First = new FirstExtension();
            Second = new SecondExtension();
        };
    }
}