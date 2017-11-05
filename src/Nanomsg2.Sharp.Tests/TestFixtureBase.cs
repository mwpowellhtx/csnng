using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    //using static Math;
    using static Imports;
    using static SocketAddressFamily;

    public abstract class TestFixtureBase : IRequiresUniquePort
    {
        // TODO: TBD: potentially belonging in its own "session" class... possibly needing to also be thread local storage?
        [DllImport(NngDll, EntryPoint = "nng_fini", CallingConvention = Cdecl)]
        private static extern void Fini();

        protected ITestOutputHelper Out { get; }

        private static readonly object Sync = new object();
        private static long _count = 0;

        protected TestFixtureBase(ITestOutputHelper @out)
        {
            lock (Sync) _count++;
            Out = @out;
        }

        ~TestFixtureBase()
        {
            //// TODO: TBD: seems to me like this is better; but still an issue in the runner?
            //lock (Sync)
            //{
            //    --_count;
            //    if (_count == 0) Fini();
            //}
            //// TODO: TBD: could it be this was causing a premature shutdown? running multiple tests? doesn't seem like it...
            //Fini();
        }

        protected static void VerifyDefaultMessage(Message m)
        {
            Assert.NotNull(m);
            Assert.True(m.HasOne);
            Assert.True(m.Size == 0ul);
        }

        protected static Message CreateMessage()
        {
            var message = new Message();
            VerifyDefaultMessage(message);
            return message;
        }

        protected static void DisposeAll(params IDisposable[] items)
        {
            (items??new IDisposable[0]).ToList().ForEach(d => d?.Dispose());
        }
    }

    internal static class PortExtensionMethods
    {
        private const ushort MinPort = 10000;
        //private const ushort MaxPort = 10999;

        private static readonly object Sync = new object();

        //private static ushort? _port;

        private static readonly IDictionary<Type, ushort> CurrentPorts;

        static PortExtensionMethods()
        {
            var current = MinPort;
            const ushort delta = 0x10;

            var requiresUniquePort = typeof(IRequiresUniquePort);

            var types = typeof(PortExtensionMethods).Assembly.GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract
                            && requiresUniquePort.IsAssignableFrom(x)).ToArray();

            CurrentPorts = new ConcurrentDictionary<Type, ushort>(
                types.ToDictionary(k => k, x => current += delta)
            );
        }

        private static ushort GetPort(Type rootType, ushort delta)
        {
            //_port = _port ?? 0;
            //_port += delta;
            //// ReSharper disable once PossibleInvalidOperationException
            //return (_port = Max(MinPort, Min(_port.Value, MaxPort))).Value;

            var requiresUniquePort = typeof(IRequiresUniquePort);

            if (!requiresUniquePort.IsAssignableFrom(rootType))
            {
                throw new ArgumentException($"Unable to service '{rootType.FullName}'", nameof(rootType));
            }

            var port = CurrentPorts[rootType];

            CurrentPorts[rootType] = (ushort) (port + delta);

            return port;
        }

        public static string WithPort(this string addr, Type rootType, ushort delta = 1000)
        {
            return $"{addr}:{GetPort(rootType, delta)}";
        }

        public static string BuildAddress(this SocketAddressFamily family, Type rootType)
        {
            /* Guard against potentially parallel execution paths. We want unique
             * addresses regardless of where/when the request originated. */
            lock (Sync)
            {
                var uuid = Guid.NewGuid();
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (family)
                {
                    case InProcess:
                        return $"inproc://{uuid}";

                    case InterProcess:
                        return $"ipc://pipe/{uuid}";

                    case IPv4:
                        return $"tcp://127.0.0.1".WithPort(rootType);

                    case IPv6:
                        return $"tcp://[::1]".WithPort(rootType);

                    case Unspecified:
                    default:
                        throw new ArgumentException($"Address invalid for family '{family}'", nameof(family));
                }
            }
        }
    }
}
