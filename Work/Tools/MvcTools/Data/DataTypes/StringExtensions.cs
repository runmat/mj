using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MvcTools
{
    public static class StringExtensions
    {
        public static string TruncateHTMLSafeishChar(this string text, int charCount)
        {
            var inTag = false;
            var cntr = 0;
            var cntrContent = 0;

            // loop through html, counting only viewable content
            foreach (var c in text)
            {
                if (cntrContent == charCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag) cntrContent++;
            }

            var substr = text.Substring(0, cntr);

            //search for nonclosed tags        
            var openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            var closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            var opentagsStack = new Stack<string>();
            var closedtagsStack = new Stack<string>();

            // to be honest, this seemed like a good idea then I got lost along the way 
            // so logic is probably hanging by a thread!! 
            foreach (Match tag in openedTags)
            {
                var openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    var tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr;
        }

        public static string TruncateHTMLSafeishWord(this string text, int wordCount)
        {
            var inTag = false;
            var cntr = 0;
            var cntrWords = 0;
            var lastc = ' ';

            // loop through html, counting only viewable content
            foreach (var c in text)
            {
                if (cntrWords == wordCount) break;
                cntr++;
                if (c == '<')
                {
                    inTag = true;
                    continue;
                }

                if (c == '>')
                {
                    inTag = false;
                    continue;
                }
                if (!inTag)
                {
                    // do not count double spaces, and a space not in a tag counts as a word
                    if (c == 32 && lastc != 32)
                        cntrWords++;
                }
            }

            var substr = text.Substring(0, cntr) + " ...";

            //search for nonclosed tags        
            var openedTags = new Regex("<[^/](.|\n)*?>").Matches(substr);
            var closedTags = new Regex("<[/](.|\n)*?>").Matches(substr);

            // create stack          
            var opentagsStack = new Stack<string>();
            var closedtagsStack = new Stack<string>();

            foreach (Match tag in openedTags)
            {
                var openedtag = tag.Value.Substring(1, tag.Value.Length - 2);
                // strip any attributes, sure we can use regex for this!
                if (openedtag.IndexOf(" ") >= 0)
                {
                    openedtag = openedtag.Substring(0, openedtag.IndexOf(" "));
                }

                // ignore brs as self-closed
                if (openedtag.Trim() != "br")
                {
                    opentagsStack.Push(openedtag);
                }
            }

            foreach (Match tag in closedTags)
            {
                string closedtag = tag.Value.Substring(2, tag.Value.Length - 3);
                closedtagsStack.Push(closedtag);
            }

            if (closedtagsStack.Count < opentagsStack.Count)
            {
                while (opentagsStack.Count > 0)
                {
                    string tagstr = opentagsStack.Pop();

                    if (closedtagsStack.Count == 0 || tagstr != closedtagsStack.Peek())
                    {
                        substr += "</" + tagstr + ">";
                    }
                    else
                    {
                        closedtagsStack.Pop();
                    }
                }
            }

            return substr;
        }
    }
}