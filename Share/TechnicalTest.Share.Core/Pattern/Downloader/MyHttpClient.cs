using System;
using System.Net;
using System.IO;
using System.Xml;
using TechnicalTest.Share.Core.Model;
using System.Threading.Tasks;

namespace TechnicalTest.Share.Core
{
    public class MyHttpClient : IMyHttpClient
    {
        private const string URL_PATTERN = "http://www.colourlovers.com/api/patterns/random";
        private const string URL_COLOR = "http://www.colourlovers.com/api/colors/random";

        public async Task<Pattern> DownloadPatternAsync()
        {
            try
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URL_PATTERN));
                request.ContentType = "application/xml";
                request.Method = "GET";
                string rawUrl = "";
                byte[] bytes = null;
                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            int startReading = 0;
                            while (reader.Read())
                            {
                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        if (reader.Name == "imageUrl")
                                            startReading = 1;
                                        break;
                                    case XmlNodeType.CDATA:
                                        if (startReading > 0)
                                        {
                                            rawUrl = reader.Value;
                                            startReading = -1;
                                        }

                                        break;

                                }
                                if (startReading == -1) break;
                            }
                        }
                    }
                }

                HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(new Uri(rawUrl));
                using (var lxResponse = await lxRequest.GetResponseAsync())
                {
                    using (BinaryReader reader = new BinaryReader(lxResponse.GetResponseStream()))
                    {
                        bytes = reader.ReadBytes(1 * 1024 * 1024 * 10);
                    }
                }

                return new Pattern() { PatternType = PatternType.Image, ImageBytes = bytes };
            }
            catch (Exception)
            {

            }

            Random rand = new Random();
            return new Pattern() { PatternType = PatternType.Color, ColorARBG = new Tuple<int, int, int, int>(rand.Next(255), rand.Next(255), rand.Next(255), 0) };
        }

        public async Task<Pattern> DownloadColorAsync()
        {
            int red = 0, green = 0, blue = 0;
            try
            {

                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(URL_COLOR));
                request.ContentType = "application/xml";
                request.Method = "GET";

                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (XmlReader reader = XmlReader.Create(stream))
                        {
                            int startReading = 0;

                            while (reader.Read())
                            {

                                switch (reader.NodeType)
                                {
                                    case XmlNodeType.Element:
                                        if (reader.Name == "red")
                                        {

                                            startReading = 1;
                                        }
                                        else if (reader.Name == "green")
                                        {

                                            startReading = 2;
                                        }
                                        else if (reader.Name == "blue")
                                        {

                                            startReading = 3;
                                        }
                                        break;

                                    case XmlNodeType.Text:
                                        if (startReading == 1)
                                            red = Convert.ToInt32(reader.Value);
                                        else if (startReading == 2)
                                            green = Convert.ToInt32(reader.Value);
                                        else if (startReading == 3)
                                        {
                                            blue = Convert.ToInt32(reader.Value);
                                            startReading = -1;
                                        }
                                        break;
                                }

                                if (startReading == -1) break;
                            }
                        }
                    }
                }

                return new Pattern() { PatternType = PatternType.Color, ColorARBG = new Tuple<int, int, int, int>(red, green, blue, 0) };

            }
            catch (Exception)
            {

            }

            Random rand = new Random();
            return new Pattern() { PatternType = PatternType.Color, ColorARBG = new Tuple<int, int, int, int>(rand.Next(255), rand.Next(255), rand.Next(255), 0) };
        }

        private string getURL(string rawUrl)
        {

            return rawUrl.Substring(8, rawUrl.Length - 11);
        }

    }
}