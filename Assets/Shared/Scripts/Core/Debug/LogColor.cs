using System.Collections.Generic;
using TimiShared.Extensions;

namespace TimiShared.Debug {
    public class LogColor {
        private LogColor(string colorName) {
            this._prefix = "<color=" + colorName + ">";
        }

        private string _prefix = null;
        public string Prefix {
            get {
                return this._prefix;
            }
        }
        private static string _postfix = "</color>";
        public string Postfix {
            get {
                return _postfix;
            }
        }

        private static Dictionary<string, LogColor> _objects = new Dictionary<string, LogColor>();

        #region Supported Colors
        public static LogColor red      { get { return _objects.GetOrAdd("red",     new LogColor("red"));       } }
        public static LogColor blue     { get { return _objects.GetOrAdd("blue",    new LogColor("blue"));      } }
        public static LogColor cyan     { get { return _objects.GetOrAdd("cyan",    new LogColor("cyan"));      } }
        public static LogColor black    { get { return _objects.GetOrAdd("black",   new LogColor("black"));     } }
        public static LogColor brown    { get { return _objects.GetOrAdd("brown",   new LogColor("brown"));     } }
        public static LogColor green    { get { return _objects.GetOrAdd("green",   new LogColor("green"));     } }
        public static LogColor grey     { get { return _objects.GetOrAdd("grey",    new LogColor("grey"));      } }
        public static LogColor lime     { get { return _objects.GetOrAdd("lime",    new LogColor("lime"));      } }
        public static LogColor maroon   { get { return _objects.GetOrAdd("maroon",  new LogColor("maroon"));    } }
        public static LogColor navy     { get { return _objects.GetOrAdd("navy",    new LogColor("navy"));      } }
        public static LogColor orange   { get { return _objects.GetOrAdd("orange",  new LogColor("orange"));    } }
        public static LogColor purple   { get { return _objects.GetOrAdd("purple",  new LogColor("purple"));    } }
        public static LogColor silver   { get { return _objects.GetOrAdd("silver",  new LogColor("silver"));    } }
        public static LogColor white    { get { return _objects.GetOrAdd("white",   new LogColor("white"));     } }
        public static LogColor yellow   { get { return _objects.GetOrAdd("yellow",  new LogColor("yellow"));    } }
        public static LogColor magenta  { get { return _objects.GetOrAdd("magenta", new LogColor("magenta"));   } }
        public static LogColor teal     { get { return _objects.GetOrAdd("teal",    new LogColor("teal"));      } }
        #endregion
    }

}
