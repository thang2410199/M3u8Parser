using System.Collections.Generic;
using System.Threading.Tasks;

namespace M3u8Parser
{
    public interface IM3u8Parser
    {
        Task<List<M3u8Media>> Parse();

        void Load(string text);
    }

    public class M3u8Media
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public long Bandwidth { get; set; }
        public Resolution Resolution { get; set; }
        public string Codecs { get; set; }
        public string Video { get; set; }
        public string Url { get; set; }
    }

    public class Resolution
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}