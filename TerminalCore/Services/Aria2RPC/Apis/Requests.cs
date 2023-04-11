﻿using System.Diagnostics;
using System.Net;
using System.Text;

using Aria2NET.Exceptions;
using Newtonsoft.Json;
namespace Aria2NET.Apis;

internal class Requests
{
    private readonly HttpClient _httpClient;
    private readonly Store _store;
    public static Process m_ariaDemonProcess { get; set; } = null;
    public Requests(HttpClient httpClient, Store store)
    {
        _httpClient = httpClient;
        _store = store;
        if (!IsRunning(m_ariaDemonProcess))
        {
            m_ariaDemonProcess = new Process
            {
 
                StartInfo = new ProcessStartInfo
                {
                    FileName = "aria2c",
                    Arguments = "--enable-rpc --user-agent=dw-extracinema-player",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
 
            };
            m_ariaDemonProcess.Start();
        }
    }
    private bool IsRunning(Process process)
    {
        

        try
        {
            if (process == null)
                return false;//throw new ArgumentNullException("process");
            Process.GetProcessById(process.Id);
        }
        catch (ArgumentException)
        {
            return false;
        } 
        catch (InvalidOperationException)
        {
            return false;
        }

        catch (Exception)
        {
            return false;
        }
        return true;
    }
    private async Task<String> Request(String method, String? secret, CancellationToken cancellationToken, params Object?[]? parameters)
    {
        var requestUrl = $"{_store.Aria2Url}";

        var request = new Request
        {
            Id = "aria2net",
            Jsonrpc = "2.0",
            Method = method,
            Parameters = new List<Object?>()
        };

        if (!String.IsNullOrWhiteSpace(secret))
        {
            request.Parameters.Add($"token:{secret}");
        }

        if (parameters != null && parameters.Length > 0)
        {
            foreach (var parameter in parameters.Where(m => m != null))
            {
                request.Parameters.Add(parameter);
            }
        }

        var jsonRequest = JsonConvert.SerializeObject(request);

        var content = new StringContent(jsonRequest);

        var retryCount = 0;
        while (true)
        {
            try
            {
                var response = await _httpClient.PostAsync(requestUrl, content, cancellationToken);

                var buffer = await response.Content.ReadAsByteArrayAsync();
                var text = Encoding.UTF8.GetString(buffer);

                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    text = null;
                }

                if (!response.IsSuccessStatusCode)
                {
                    var aria2Exception = ParseAria2Exception(text);

                    if (aria2Exception != null)
                    {
                        throw aria2Exception;
                    }

                    throw new Exception(text);
                }

                if (String.IsNullOrEmpty(text))
                {
                    throw new Exception("No response received");
                }

                return text!;
            }
            catch (Aria2Exception)
            {
                throw;
            }
            catch
            {
                if (retryCount < _store.RetryCount)
                {
                    retryCount++;

                    await Task.Delay(1000 * retryCount, cancellationToken);
                }
                else
                {
                    throw;
                }
            }
        }
    }
        
    private async Task<T> Request<T>(String url, CancellationToken cancellationToken, params Object?[] parameters)
        where T : class, new()
    {
        var result = await Request(url, _store.Secret, cancellationToken, parameters);

        try
        {
            return JsonConvert.DeserializeObject<T>(result) ?? throw new JsonSerializationException();
        }
        catch (JsonSerializationException ex)
        {
            throw new JsonSerializationException($"Unable to deserialize Aria2 API response to {typeof(T).Name}. Response was: {result}", ex);
        }
    }
        
    public async Task GetRequestAsync(String url, CancellationToken cancellationToken)
    {
        await Request(url, _store.Secret, cancellationToken);
    }

    public async Task GetRequestAsync(String url, CancellationToken cancellationToken, params Object[] parameters)
    {
        await Request(url, _store.Secret, cancellationToken, parameters);
    }

    public async Task<T> GetRequestAsync<T>(String url, CancellationToken cancellationToken)
    {
        var aria2Result = await Request<RequestResult<T>>(url, cancellationToken);

        if (aria2Result.Result == null)
        {
            throw new Exception("No response data received");
        }

        return aria2Result.Result;
    }

    public async Task<T> GetRequestAsync<T>(String url, CancellationToken cancellationToken, params Object?[] parameters)
    {
        var aria2Result = await Request<RequestResult<T>>(url, cancellationToken, parameters);

        if (aria2Result.Result == null)
        {
            throw new Exception("No response data received");
        }

        return aria2Result.Result;
    }

    public async Task<List<Object>> MultiRequestAsync(CancellationToken cancellationToken, params Object?[] parameters)
    {
        var parameterList = new List<MulticallRequest>();

        foreach (var param in parameters)
        {
            if (param is not Object[] methodCall)
            {
                throw new Exception($"Parameter must be of type Object[]");
            }

            var actualParameters = new List<Object>
            {
                $"token:{_store.Secret}"
            };
            actualParameters.AddRange(methodCall.Skip(1));

            if (methodCall[0] is not String methodName)
            {
                throw new Exception("Invalid method name in the first object");
            }

            var multicallRequest = new MulticallRequest
            {
                MethodName = methodName,
                Parameters = actualParameters.ToList()
            };

            parameterList.Add(multicallRequest);
        }

        var requestResult = await Request("system.multicall", null, cancellationToken, parameterList);

        try
        {
            var result = JsonConvert.DeserializeObject<RequestResult<List<List<Object>>>>(requestResult) ?? throw new JsonSerializationException();

            if (result.Result == null)
            {
                throw new JsonSerializationException();
            }

            return result.Result.Select(m => m.First()).ToList();
        }
        catch (JsonSerializationException ex)
        {
            throw new JsonSerializationException($"Unable to deserialize Aria2 API response. Response was: {requestResult}", ex);
        }
    }
        
    private static Aria2Exception? ParseAria2Exception(String? text)
    {
        try
        {
            if (text == null)
            {
                return null;
            }

            var requestError = JsonConvert.DeserializeObject<RequestResult<RequestError>>(text);

            if (requestError?.Error != null)
            {
                return new Aria2Exception(requestError.Error.Code, requestError.Error.Message);
            }

            return null;
        }
        catch
        {
            return null;
        }
    }
}