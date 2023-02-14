using Model;
using SingSpace;
using LoggerSpace;
using DBWork;

namespace SigningDocSpace
{
    internal class LogisUtils {
        protected bool SaveFile(byte[] data, string filePath)
        {
            var result = false;
            if (string.IsNullOrEmpty(filePath) || data == null) return result;
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
            result = true;
            return result;
        }
    }
    internal class Logic: LogisUtils
    {
        private Uri baseUri = new("");
        private Log log = new(true);
        private string FileDir = "";
        private QRCodeUtil? qrcodeUtil = null;
        private ReposiotoryWork? repo = null;

        internal Logic(Uri baseUri, string FileDir, string ConnectionDb) {
            this.baseUri = baseUri;            
            this.FileDir = FileDir;
            qrcodeUtil = new(log);
            repo = new(ConnectionDb);
        }

        internal Doc? GetFilebyUri(string url)
        {
            Doc? result = null;
            if (repo == null) return result;

            result = repo.GetDoc(x => x.Url == url);
            return result;
        }

        internal Uri? GetFileUri(Doc item)
        {
            Uri? result = new("");

            if(repo==null || qrcodeUtil==null || string.IsNullOrEmpty(FileDir) || item==null || item.Data==null)
                return result;

            try
            {
                var path = Path.Combine(this.FileDir,Path.GetFileName(item.FileName));                
                
                //save file to disk
                if (SaveFile(item.Data, path))
                {
                    if (Uri.TryCreate(baseUri.ToString() + Guid.NewGuid(), UriKind.Absolute, out result))
                    {
                        item.Url = result.ToString();
                        repo.Add(item);//save to db
                    }                    
                }
            }
            catch (Exception exp)
            {
                log.Error($"GetFileUri/ {exp.Message}");
            }
            return result;
        }

        internal bool SingDoc(Doc item)
        {
            var result = false;

            if (qrcodeUtil == null || string.IsNullOrEmpty(FileDir) || item == null || item.Data == null)
                return result;

            try
            {
                var path = Path.Combine(this.FileDir, Path.GetFileName(item.FileName));

                qrcodeUtil.AddCodeToFile(path,
                                         new string[] { item.FIO, item.IIN_BIN, item.Phone },
                                         item.XqrCode, item.YqrCode);
            }
            catch (Exception exp)
            {
                log.Error($"GetFileUri/ {exp.Message}");
            }
            return result;
        }
    }
}
