using System;

namespace Nanomsg2.Sharp
{
    using Xunit.Abstractions;

    public abstract class BehaviorDrivenTestFixtureBase : TestFixtureBase
    {
        protected delegate void ScenarioDelegate(Action action);

        protected delegate void BehaviorDrivenDelegate(string title, Action action);

        protected readonly BehaviorDrivenDelegate Given;

        protected readonly BehaviorDrivenDelegate Section;

        private int _indent;

        private static string GetIndent(int indent)
        {
            return string.Empty.PadLeft(indent, ' ');
        }

        protected void Report(string message)
        {
            Out.WriteLine($"{GetIndent(_indent)}{message}");
        }

        protected BehaviorDrivenTestFixtureBase(ITestOutputHelper @out)
            : base(@out)
        {

            Given = (title, action) =>
            {
                Out.WriteLine($"{GetIndent(_indent)}Given {title}.");

                _indent += 2;

                action();

                _indent -= 2;
            };

            Section = (title, action) =>
            {
                Out.WriteLine($"{GetIndent(_indent)}And {title}.");

                _indent += 2;

                action();

                _indent -= 2;
            };
        }
    }
}
