using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Mn.NewsCms.Robot
{
    public class CSSClassParser
    {
        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CSSClassParser() { }

        #endregion
        #region Constants

        private const string _CSS_CLASS_REGEX = "class=\"[a-zA-Z./:&\\d_]+\"";

        #endregion
        #region Private Instance Fields

        private List<string> _classes = new List<string>();

        #endregion
        #region Public Properties

        public List<string> Classes
        {
            get { return _classes; }
            set { _classes = value; }
        }

        #endregion

        /// <summary>
        /// Parses the page looking for css classes that are in use.
        /// </summary>
        /// <param name="page">The page to parse.</param>
        public void ParseForCssClasses(Page page)
        {
            MatchCollection matches = Regex.Matches(page.Content, _CSS_CLASS_REGEX);

            for (int i = 0; i <= matches.Count - 1; i++)
            {
                Match classMatch = matches[i];
                string[] classesArray = classMatch.Value.Substring(classMatch.Value.IndexOf('"') + 1, classMatch.Value.LastIndexOf('"') - classMatch.Value.IndexOf('"') - 1).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(string classValue in classesArray)
                {
                    if (!_classes.Contains(classValue))
                    {
                        _classes.Add(classValue);
                    }
                }
            }
        }
    }
}
