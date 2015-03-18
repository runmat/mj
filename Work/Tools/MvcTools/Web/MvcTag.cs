using System;
using System.IO;
using System.Web.Mvc;

namespace MvcTools.Web
{
    public class MvcTag : IDisposable
    {
        public const string FormPersistenceModeErrorPrefix = "__FormPersistenceMode__";

        public const string CR = "\r\n";

        private bool _disposed;
        private readonly FormContext _originalFormContext;
        private readonly ViewContext _viewContext;
        private readonly TextWriter _writer;
        private readonly string _tagName;
        private readonly string _cssClass;
        private readonly string _forAttribute;
        private readonly string _style;
        private readonly string _src;
        private readonly int? _paddingLeft;
        private readonly int? _paddingTop;
        private readonly int? _paddingBottom;
        private readonly int? _marginLeft;
        private readonly int? _marginTop;
        private readonly int? _marginBottom;
        private readonly string _id;
        private readonly int? _width;
        private readonly int? _height;
        private readonly string _color;
        private readonly string _onClick;

        public MvcTag(ViewContext viewContext, string tagName, string cssClass = null, string id = null, int? width = null, int? height = null, string forAttribute = null, string style = null, string src = null, 
                            int? paddingLeft = null, int? paddingTop = null, int? paddingBottom = null,
                            int? marginLeft = null, int? marginTop = null, int? marginBottom = null, string color = null, string onClick = null)
        {
            _tagName = tagName;
            _cssClass = cssClass;
            _id = id;
            _width = width;
            _height = height;
            _forAttribute = forAttribute;
            _viewContext = viewContext;
            _style = style;
            _src = src;
            _color = color;
            _paddingLeft = paddingLeft;
            _paddingTop = paddingTop;
            _paddingBottom = paddingBottom;
            _marginLeft = marginLeft;
            _marginTop = marginTop;
            _marginBottom = marginBottom;
            _onClick = onClick;
            if (_viewContext != null)
            {
                _writer = viewContext.Writer;
                _originalFormContext = viewContext.FormContext;
                viewContext.FormContext = new FormContext();
            }

            Begin();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string Begin()
        {
            var tag = new TagBuilder(_tagName);

            if (!string.IsNullOrEmpty(_cssClass))
                tag.AddCssClass(_cssClass);

            if (!string.IsNullOrEmpty(_forAttribute))
                tag.Attributes.Add("for", _forAttribute);

            if (_id != null)
                tag.Attributes.Add("id", _id);

            var style = (string.IsNullOrEmpty(_style) ? "" : _style);
            if (_width != null)
                style += string.Format("width:{0}px; ", _width);
            if (_height != null)
                style += string.Format("height:{0}px; ", _height);

            if (_paddingLeft != null)
                style += string.Format("padding-left:{0}px; ", _paddingLeft);
            if (_paddingTop != null)
                style += string.Format("padding-top:{0}px; ", _paddingTop);
            if (_paddingBottom != null)
                style += string.Format("padding-bottom:{0}px; ", _paddingBottom);

            if (_marginLeft != null)
                style += string.Format("margin-left:{0}px; ", _marginLeft);
            if (_marginTop != null)
                style += string.Format("margin-top:{0}px; ", _marginTop);
            if (_marginBottom != null)
                style += string.Format("margin-bottom:{0}px; ", _marginBottom);
            if (_color != null)
                style += string.Format("color:{0}; ", _color);

            if (!string.IsNullOrEmpty(style))
                tag.Attributes.Add("style", style);

            if (!string.IsNullOrEmpty(_onClick))
                tag.Attributes.Add("onclick", _onClick);

            if (!string.IsNullOrEmpty(_src))
                tag.Attributes.Add("src", _src);

            var result = tag.ToString(TagRenderMode.StartTag) + CR;
            if (_writer != null)
                _writer.Write(result);
            return result;
        }

        public string End()
        {
            var result = string.Format("</{0}>{1}", _tagName, CR);
            if (_writer != null)
                _writer.Write(result);
            return result;
        }

        public string Render()
        {
            return Begin() + End();
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
