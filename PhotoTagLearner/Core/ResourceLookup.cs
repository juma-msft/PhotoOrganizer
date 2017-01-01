using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace PhotoTagLearner.Core
{
    static class ResourceLookup
    {
        public static ResourceDictionary GenericXaml
        {
            get
            {
                if (_styles == null)
                {
                    _styles = Application.Current.Resources;
                }

                return _styles;
            }
        }

        private static ResourceDictionary _styles;
    }
}
