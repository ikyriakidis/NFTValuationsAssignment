using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NFTValuations.Domain;
using NFTValuations.Domain.Models;
using NFTValuations.Domain.Services;
using NFTValuations.Contracts.ERC721.ContractDefinition;

namespace NFTValuations
{
    public class EntryPoint
    {
        private readonly ILogger<EntryPoint> _logger;
        private readonly AppSettings _appSettings;
        private readonly HttpClient _httpClient;
        private readonly IServicePool _servicePool;

        public EntryPoint(
            ILogger<EntryPoint> logger,
            IOptionsMonitor<AppSettings> appSettings,
            HttpClient httpClient,
            IServicePool servicePool)
        {
            _logger = logger;
            _appSettings = appSettings.CurrentValue;
            _httpClient = httpClient;
            _servicePool = servicePool;
        }
        public async Task RunAsync(string[] args)
        {
            //var requestList = new List<RequestDto>();
            foreach (var argument in args)
            {
                var contractAddress = argument.Split(",")[0];
                int.TryParse(argument.Split(",")[1], out int tokenId);


                var tokenURIFunction = new TokenURIFunction();
                tokenURIFunction.TokenId = tokenId;

                var result = string.Empty;

                try
                {
                    var service = _servicePool.GetserviceInstance(contractAddress);
                    result = await service.TokenURIQueryAsync(tokenURIFunction);
                    _logger.LogInformation(result);


                    ResponseDto ret = null;

                    if (result.StartsWith("data:application/json;base64"))
                    {
                        HandleData(result);
                    }

                    if (result.StartsWith("ipfs://"))
                    {
                        ret = await HandleIpfsRequestAsync(result);
                    }

                    if (result.StartsWith("http://") || result.StartsWith("https://"))
                    {
                        ret = await HandleHttpRequestAsync(result);
                    }

                    _logger.LogInformation(JsonConvert.SerializeObject(ret));

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            Console.WriteLine("Press any button to exit");
            Console.ReadLine();
        }

        private void HandleData(string result)
        {
            _logger.LogInformation($"Request could not be handed: {result}");
        }

        private async Task<ResponseDto> HandleHttpRequestAsync(string httpUrl)
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

        private async Task<ResponseDto> HandleIpfsRequestAsync(string ipfsUrl)
        {
            if (ipfsUrl.StartsWith("ipfs://"))
            {
                ipfsUrl = $"https://ipfs.io/ipfs/{ipfsUrl.Replace("ipfs://", string.Empty)}";
            }
            return await HandleHttpRequestAsync(ipfsUrl);
        }

        private ResponseDto HandleJsonResponse(string inputString)
        {
            dynamic parsedObject = JsonConvert.DeserializeObject(inputString);

            var responseDto = new ResponseDto();
            responseDto.Description = parsedObject["description"];
            responseDto.ExternalUrl = parsedObject["animation_url"] ?? parsedObject["animation_url"];
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
            ResponseDto ret = null;
            if (inputString.Contains("<meta http-equiv=\"refresh"))
            {
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

                ret = await HandleIpfsRequestAsync(ipfsUri);

            }

            return ret;
        }
    }
}
