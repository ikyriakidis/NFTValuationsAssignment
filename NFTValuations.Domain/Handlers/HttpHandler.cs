using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFTValuations.Domain.Models;

namespace NFTValuations.Domain.Handlers
{
  public class HttpHandler : IHandler
  {
    private readonly HttpClient _httpClient;
    
    public HttpHandler(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public virtual async Task<ResponseDto> Handle(string uri)
    {
      return await HandleHttpRequestAsync(uri);
    }

    protected async Task<ResponseDto> HandleHttpRequestAsync(string httpUrl)
    {
      var response = await _httpClient.GetAsync(httpUrl);
      var filetype = response.Content.Headers.ContentType.MediaType;

      ResponseDto ret = null;

      if (filetype == Constants.ContentTypes.Json)
      {
        var json = await response.Content.ReadAsStringAsync();

        ret = HandleJsonResponse(json);
      }

      if (filetype == Constants.ContentTypes.Html)
      {
        var html = await response.Content.ReadAsStringAsync();

        ret = await HandleHtmlResponseAsync(html);
      }

      return ret;
    }

    private ResponseDto HandleJsonResponse(string inputString)
    {
      dynamic parsedObject = JsonConvert.DeserializeObject(inputString);

      var responseDto = new ResponseDto();
      responseDto.Description = parsedObject["description"];
      responseDto.ExternalUrl = parsedObject["animation_url"];
      responseDto.Media = parsedObject["image"];
      responseDto.Name = parsedObject["name"];

      List<PropertyValuePair> properties = ((JArray)parsedObject["attributes"]).Select(x => new PropertyValuePair
      {
        Category = (string)x["trait_type"],
        Property = (string)x["value"]
      }).ToList();

      responseDto.Properties = properties;

      return responseDto;
    }

    private async Task<ResponseDto> HandleHtmlResponseAsync(string inputString)
    {
      if (!inputString.Contains("<meta http-equiv=\"refresh"))
      {
        return null;
      }

      Regex re = new Regex(@"<meta(?: [^>]+)?>", RegexOptions.IgnoreCase);

      var redirectMetaTag = re.Matches(inputString).FirstOrDefault(x => x.Groups[0].ToString().StartsWith("<meta http-equiv=\"refresh")).ToString();
      var urlIndex = redirectMetaTag.IndexOf("url=") + 5;
      var ipfsUri = redirectMetaTag.Substring(urlIndex, redirectMetaTag.Length - urlIndex - 4);

      if (ipfsUri.StartsWith("ipfs://"))
      {
        ipfsUri = $"https://ipfs.io/ipfs/{ipfsUri.Replace("ipfs://", string.Empty)}";
      }

      if (ipfsUri.StartsWith("ipfs/"))
      {
        ipfsUri = $"https://ipfs.io/ipfs/{ipfsUri.Replace("ipfs/", string.Empty)}";
      }

      return await Handle(ipfsUri);
    }
  }
}