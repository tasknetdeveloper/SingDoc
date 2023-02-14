using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using SigningDocSpace;

namespace SigningDoc.Controllers
{
    [Authorize]
    [Route("api/doc")]
    [ApiController]
    public class DocumentApiController : ControllerBase
    {
        private Logic? logic = null;
        
        public DocumentApiController(IConfiguration configuration) {
            Uri? uri = null;

            //read config
            var DirRepository = configuration.GetValue<string>("DirRepository");
            var BaseUri = configuration.GetValue<string>("BaseUri");
            var ConnectionDb = configuration.GetValue<string>("ConnectionDb");
            Uri.TryCreate(BaseUri, UriKind.Absolute, out uri);

            if (uri != null && !string.IsNullOrEmpty(ConnectionDb) && !string.IsNullOrEmpty(DirRepository))
            {
                logic = new(uri, DirRepository, ConnectionDb);
            }            
        }      

        [HttpPost]
        public async Task<string>? GetDocUrl(Doc item)
        {
            return await new Task<string>(() => {
                Uri? u = new("");
                if (logic == null || item==null) return "";                
                u = logic.GetFileUri(item);

                return (u!=null)
                        ? u.AbsolutePath.ToString()
                        : "";
            });
        }

        [HttpPost]
        public async Task<bool> SingDoc(Doc item)
        {
            return await new Task<bool>(() => {                
                if (logic == null || item == null) return false;
                return logic.SingDoc(item);                
            });
        }

        [HttpPost]
        public async Task<Doc>? GetDoc(string url)
        {
            return await new Task<Doc>(() => {
                Doc result = new();
                if (logic == null || string.IsNullOrEmpty(url)) return result;                
                return logic.GetFilebyUri(url) ?? result;
            });
        }
    }
}
