using System.Linq;
using Castle.DynamicProxy;
using NLog;
using RefImplBibl.Interfaces;

namespace RefImplBibl.Logging
{
    public class SapInterceptor : IInterceptor
    {
        private readonly SapLogger _logger;
        private readonly IAnwenderInfoProvider _anwenderInfoProvider;

        public SapInterceptor(SapLogger logger, IAnwenderInfoProvider anwenderInfoProvider)
        {
            _logger = logger;
            _anwenderInfoProvider = anwenderInfoProvider;
        }

        /// <summary>
        /// Abgefangener Aufruf
        /// </summary>
        /// <param name="invocation">Beinhaltet sowohl das Ziel als auch die Paramter des abgefangenen Aufrufs</param>
        public void Intercept(IInvocation invocation)
        {
            var anmeldename = _anwenderInfoProvider.GetAnwender().Anmeldename;
            var bapi = invocation.Method.Name;
            var @params = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());

            _logger.Log(anmeldename, bapi, @params);

            invocation.Proceed();

        }
    }
}