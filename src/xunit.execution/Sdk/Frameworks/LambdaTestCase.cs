using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Xunit.Sdk
{
    /// <summary>
    /// A simple implementation of <see cref="XunitTestCase"/> wherein the running of the
    /// test case can be represented by an <see cref="Action"/>. Useful for emitting test
    /// cases which later evaluate to error messages (since throwing error messages during
    /// discovery is often the wrong thing to do). See <see cref="TheoryDiscoverer"/> for
    /// a use of this test case to emit an error message when a theory method is found
    /// that has no test data associated with it.
    /// </summary>
    public class LambdaTestCase : XunitTestCase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LambdaTestCase"/> class.
        /// </summary>
        /// <param name="testCollection">The test collection this test case belongs to.</param>
        /// <param name="assembly">The test assembly.</param>
        /// <param name="testClass">The test class.</param>
        /// <param name="testMethod">The test method.</param>
        /// <param name="factAttribute">The instance of <see cref="FactAttribute"/>.</param>
        /// <param name="lambda">The code to run for the test.</param>
        public LambdaTestCase(ITestCollection testCollection,
                              IAssemblyInfo assembly,
                              ITypeInfo testClass,
                              IMethodInfo testMethod,
                              IAttributeInfo factAttribute,
                              Action lambda)
            : base(testCollection, assembly, testClass, testMethod, factAttribute)
        {
            Lambda = lambda;
        }

        /// <summary>
        /// Gets the lambda that this test case will run.
        /// </summary>
        public Action Lambda { get; private set; }

        /// <inheritdoc/>
        public override Task<RunSummary> RunAsync(IMessageBus messageBus,
                                                  object[] constructorArguments,
                                                  ExceptionAggregator aggregator,
                                                  CancellationTokenSource cancellationTokenSource)
        {
            return new LambdaTestCaseRunner(this, messageBus, aggregator, cancellationTokenSource).RunAsync();
        }
    }
}