using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SilverLeaf.Common.Helpers
{
    public static class GoogleImages
    {
        public static List<string> Random(string subject)
        {
            string url = "https://www.google.com/search?q=" + subject + "&tbm=isch";
            string html = "";

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return new List<string>();
                using (var sr = new StreamReader(dataStream))
                {
                    html = sr.ReadToEnd();
                }
            }

            var urls = new List<string>();

            int ndx = html.IndexOf("\"ou\"", StringComparison.Ordinal);

            while (ndx >= 0)
            {
                ndx = html.IndexOf("\"", ndx + 4, StringComparison.Ordinal);
                ndx++;
                int ndx2 = html.IndexOf("\"", ndx, StringComparison.Ordinal);
                url = html.Substring(ndx, ndx2 - ndx);
                if (url.EndsWith(".jpg") || url.EndsWith(".jpeg"))
                {
                    urls.Add(url);
                }
                ndx = html.IndexOf("\"ou\"", ndx2, StringComparison.Ordinal);
            }
            return urls;
        }
    }
}
