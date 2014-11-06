using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTools.Web
{
    public class MvcWrapper : IDisposable
    {
        private bool _disposed;
        private readonly FormContext _originalFormContext;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;
        private readonly string _partialViewName;
        private readonly object _model;

        public MvcWrapper(ViewContext viewContext, string partialViewName, object model)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }

            _viewContext = viewContext;
            _partialViewName = partialViewName;
            _model = model;
            _writer = viewContext.Writer;
            _originalFormContext = viewContext.FormContext;
            viewContext.FormContext = new FormContext();

            Begin();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Begin()
        {
            var partialViewName = string.Format("Wrappers/{0}_Begin", _partialViewName);
            var partialView = RenderPartialViewToString(partialViewName, _model);

            _writer.Write(partialView);
        }

        private void End()
        {
            var partialViewName = string.Format("Wrappers/{0}_End", _partialViewName);
            var partialView = RenderPartialViewToString(partialViewName, _model);

            _writer.Write(partialView);
        }

        public string RenderPartialViewToString(string viewName, object model)
        {
            _viewContext.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(_viewContext, viewName);
                var viewContext = new ViewContext(_viewContext, viewResult.View, _viewContext.ViewData, _viewContext.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                End();

                if (_viewContext != null)
                {
                    _viewContext.OutputClientValidation();
                    _viewContext.FormContext = _originalFormContext;
                }
            }
        }

        public void EndForm()
        {
            Dispose(true);
        }
    }
}
