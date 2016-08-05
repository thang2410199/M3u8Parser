using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace M3u8Parser
{
    public class M3u8Parser : IM3u8Parser
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<List<M3u8Media>> Parse()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (m3u8 == null)
                throw new Exception("run Load first");
            var lines = m3u8.Replace("\r", "").Split('\n');
            var result = new List<M3u8Media>();
            if (lines.Any())
            {
                var firstLine = lines[0];
                if (firstLine != "#EXTM3U")
                {
                    throw new InvalidOperationException(
                        "The provided URL does not link to a well-formed M3U8 playlist.");
                }
                bool mediaDetected = false;
                M3u8Media streamInfo = null;
                for (var i = 1; i < lines.Length; i++)
                {
                    var line = lines[i];
                    if (line.StartsWith("#"))
                    {
                        var lineData = line.Substring(1);

                        var split = lineData.Split(':');

                        var name = split[0];
                        var suffix = split[1];

                        if (name == "EXT-X-MEDIA")
                        {
                            mediaDetected = true;
                            streamInfo = new M3u8Media();
                        }
                        var attributes = suffix.Split(',');
                        foreach (var item in attributes)
                        {
                            var keyvalue = item.Split('=');
                            if (keyvalue.Any())
                                switch (keyvalue[0])
                                {
                                    case "TYPE":
                                        streamInfo.Type = keyvalue[1].Trim('"');
                                        break;
                                    case "NAME":
                                        streamInfo.Name = keyvalue[1].Trim('"');
                                        break;
                                    case "BANDWIDTH":
                                        streamInfo.Bandwidth = long.Parse(keyvalue[1], CultureInfo.InvariantCulture);
                                        break;
                                    case "RESOLUTION":
                                        try
                                        {
                                            streamInfo.Resolution = new Resolution();
                                            var size = keyvalue[1].Split('x');
                                            if (size != null)
                                            {
                                                streamInfo.Resolution.Width = int.Parse(size[0], CultureInfo.InvariantCulture);
                                                streamInfo.Resolution.Height = int.Parse(size[1], CultureInfo.InvariantCulture);
                                            }
                                        }
                                        catch
                                        {

                                        }
                                        break;
                                    case "CODECS":
                                        streamInfo.Codecs = keyvalue[1].Trim('"');
                                        break;
                                    case "VIDEO":
                                        streamInfo.Video = keyvalue[1].Trim('"');
                                        break;
                                }
                        }
                    }
                    else
                    {
                        //Url ?
                        if (mediaDetected)
                        {
                            streamInfo.Url = line;
                            mediaDetected = false;
                            result.Add(streamInfo);
                        }
                    }
                }
            }

            return result;
        }
        string m3u8;
        public void Load(string text)
        {
            if (text == null)
                throw new Exception("can not load null");
            m3u8 = text;
        }
    }
}
