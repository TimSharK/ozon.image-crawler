namespace Domain
{
    public class LoadedImage
    {
        public string Url { get; set; }

        public byte[] Data { get; set; }

        //public string FileName
        //{
        //    get
        //    {
        //        var uri = new Uri(Url);

        //        if (!uri.IsFile)
        //            return null;

        //        var filename = System.IO.Path.GetFileName(uri.LocalPath);
        //        return filename;

        //    }
        //}
    }
}