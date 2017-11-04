using System;

namespace Nanomsg2.Sharp
{
    using Xunit.Abstractions;

    public abstract class TestFixtureBase
    {
        protected ITestOutputHelper Out { get; }

        private int _indent;

        protected delegate void BehaviorDrivenDelegate(string title, Action action);

        private static string GetIndent(int indent)
        {
            return string.Empty.PadLeft(indent, ' ');
        }

        protected void Report(string message)
        {
            Out.WriteLine($"{GetIndent(_indent)}{message}");
        }

        protected TestFixtureBase(ITestOutputHelper @out)
        {
            Out = @out;

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

        protected readonly BehaviorDrivenDelegate Given;

        protected readonly BehaviorDrivenDelegate Section;
    }
}
