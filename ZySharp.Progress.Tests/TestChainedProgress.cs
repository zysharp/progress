using System;

using Xunit;

using ZySharp.Progress.Builder;

namespace ZySharp.Progress.Tests
{
    public class TestChainedProgress
    {
        [Fact]
        public void Test_Basic()
        {
            var value = 0;

            var handler3 = new LambdaProgress<int>(x => value = x);
            var handler2 = new LambdaChainedProgress<int>(handler3, _ => { /* nothing to do here */ });
            var handler1 = new LambdaChainedProgress<int>(handler2, _ => { /* nothing to do here */ });

            ReportRandomIntProgress(handler1, out var expected);
            Assert.Equal(expected, value);
        }

        [Fact]
        public void Test_Builder()
        {
            var value = 0;

            var handler = ProgressHandlerChain
                .Create<int>()
                .Call(_ => { /* nothing to do here */ })
                .Call(_ => { /* nothing to do here */ })
                .Build(x => value = x);

            ReportRandomIntProgress(handler, out var expected);
            Assert.Equal(expected, value);
        }

        private static void ReportRandomIntProgress(IProgress<int> handler, out int expected)
        {
            var r = new Random();
            expected = r.Next(0, 1000);
            handler.Report(expected);
        }
    }
}