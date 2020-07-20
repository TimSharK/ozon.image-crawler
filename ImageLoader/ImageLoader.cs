using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace ImageLoader
{
    public class MyImageLoader : IImageLoader
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _url;

        public MyImageLoader(IServiceProvider serviceProvider, IServiceScopeFactory scopeFactory, string url)
        {
            _serviceProvider = serviceProvider;
            _scopeFactory = scopeFactory;
            _url = url;
        }

        public async Task Load()
        {
            var sources = GetAllImagesSources(_url);
            var tasks = new List<Task>();

            await foreach (var source in sources)
            {
                var task = LoadImage(source);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }

        private async Task LoadImage(string src)
        {
            if (IsBase64Source(src))
                throw new NotImplementedException();

            var client = new HttpClient();
            var response = await client.GetAsync(src);
            var data = await response.Content.ReadAsByteArrayAsync();

            using (_scopeFactory.CreateScope())
            {
                var imgSaver = _serviceProvider.GetService<IImageSaver>();
                await imgSaver.SaveAsync(data, Path.GetFileName(src));
            }
        }

        private bool IsBase64Source(string src)
        {
            // todo TBD
            return false;
        }

        private async IAsyncEnumerable<string> GetAllImagesSources(string url)
        {
            var client = new HttpClient();
            var source = await client.GetStringAsync(url);
            var document = new HtmlAgilityPack.HtmlDocument();

            document.LoadHtml(source);

            var imgs = document.DocumentNode.Descendants("img");
            var srcAttrs = imgs.Select(i => i.Attributes["src"]);

            foreach (var link in srcAttrs)
            {
                yield return link.Value;
            }
        }
    }
}
