using Aria2NET;
using Microsoft.AspNetCore.Mvc;
using FileResult = Aria2NET.FileResult;

namespace StreamingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadManagerController : ControllerBase
    {
        Aria2NetClient client;
        private string uploadPath { get; set; } 
        public DownloadManagerController()
        {
            client = new Aria2NetClient(Setup.URL, Setup.Secret, null, 1);
            uploadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Upload");
        }
        
        [HttpGet(nameof(GetVersion))]
        public async Task<ActionResult<VersionResult>> GetVersion()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret, null, 5);

            VersionResult result = await client.GetVersionAsync();
            return Ok(result);
            //Assert.Equal("1.36.0", result.Version);
        }

        [HttpGet(nameof(AddUri))]
        public async Task<ActionResult<string>> AddUri(string urlDownload, string outFileName = "nope")
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);



            var parameters = new Dictionary<String, Object>
                                                  {
                                                      { "dir", uploadPath}
                                                  };
            if(outFileName != "nope")
                parameters.Add("out",outFileName);


            string result = await client.AddUriAsync(new List<String>
                                                  {
                                                      urlDownload
                                                  },
                                                  parameters
                                                  , 0);

            return Ok(result);
        }

        [HttpGet(nameof(AddTorrentFile))]
        public async Task<ActionResult<string>> AddTorrentFile(string torrentFile)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

             
            var file = await System.IO.File.ReadAllBytesAsync(torrentFile);

            string result = await client.AddTorrentAsync(file,
                                                      null,
                                                      new Dictionary<String, Object>
                                                      {
                                                          { "dir", uploadPath}
                                                      }, 0);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(AddMetalink))]
        public async Task<ActionResult<List<string>>> AddMetalink(string metalink)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

 
            var file = await System.IO.File.ReadAllBytesAsync(metalink);

            List<string> result = await client.AddMetalinkAsync(file,
                                                       new Dictionary<String, Object>
                                                       {
                                                           { "dir", uploadPath}
                                                       }, 0);
            return result;
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(Remove))]
        public async Task<ActionResult<string>> Remove(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            string result = await client.RemoveAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(ForceRemove))]
        public async Task<ActionResult<string>> ForceRemove(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            string result = await client.ForceRemoveAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(TellStatus))]
        public async Task<ActionResult<DownloadStatusResult>> TellStatus(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret, null, 1);

            DownloadStatusResult result = await client.TellStatusAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(TellActive))]
        public async Task<ActionResult<IList<DownloadStatusResult>>> TellActive()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<DownloadStatusResult> result = await client.TellActiveAsync();
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(TellStopped))]
        public async Task<ActionResult<IList<DownloadStatusResult>>> TellStopped()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<DownloadStatusResult> result = await client.TellStoppedAsync(0, 1000);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(TellWaiting))]
        public async Task<ActionResult<IList<DownloadStatusResult>>> TellWaiting()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<DownloadStatusResult> result = await client.TellWaitingAsync(0, 1000);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(TellAll))]
        public async Task<ActionResult<IList<DownloadStatusResult>>> TellAll()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<DownloadStatusResult> result = await client.TellAllAsync();
            string VHSPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "VHS");
            foreach (var item in result)
            {
                var tt = item;
                if (tt.Status.Equals("complete"))
                foreach(var file in item.Files)
                {

                }
            }
            return Ok(result);
        }

        [HttpGet(nameof(GetUris))]
        public async Task<ActionResult<IList<UriResult>>> GetUris(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<UriResult> result = await client.GetUrisAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(GetFiles))]
        public async Task<ActionResult<IList<FileResult>>> GetFiles(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<FileResult> result = await client.GetFilesAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(GetPeers))]
        public async Task<ActionResult<IList<PeerResult>>> GetPeers(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<PeerResult> result = await client.GetPeersAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(GetServers))]
        public async Task<ActionResult<IList<ServerResult> >>
            GetServers(string guid)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IList<ServerResult> result = await client.GetServersAsync(guid);
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(GetGlobalOption))]
        public async Task<ActionResult<IDictionary<string, string>>> GetGlobalOption()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            IDictionary<string, string> result = await client.GetGlobalOptionAsync();
            return Ok(result);
            //Assert.NotNull(result);
        }

        [HttpGet(nameof(ChangeGlobalOption))]
        public async Task ChangeGlobalOption(string option, string value)
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            await client.ChangeGlobalOptionAsync(new Dictionary<String, String>
            {
                //{"bt-max-peers", "60"}
                {option,value }
            });
        }

        [HttpGet(nameof(GetSessionInfo))]
        public async Task<ActionResult<SessionResult>> GetSessionInfo()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            SessionResult sessionId = await client.GetSessionInfo();
            return Ok(sessionId);
            //Assert.NotNull(sessionId);
        }

        [HttpGet(nameof(GetGlobalStat))]
        public async Task<ActionResult<GlobalStatResult>> GetGlobalStat()
        {
            var client = new Aria2NetClient(Setup.URL, Setup.Secret);

            GlobalStatResult globalStats = await client.GetGlobalStatAsync();
            return Ok(globalStats);
            //Assert.NotNull(globalStats);
        }
     }
}
