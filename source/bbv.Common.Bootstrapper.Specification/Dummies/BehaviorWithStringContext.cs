namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System.Collections.Generic;

    public class BehaviorWithStringContext : IBehavior<ICustomExtension>
    {
        private string input;

        private readonly string addition;

        public BehaviorWithStringContext(string input, string addition)
        {
            this.addition = addition;
            this.input = input;
        }

        public void Behave(IEnumerable<ICustomExtension> extensions)
        {
            this.input += " " + this.addition;
        }
    }
}