using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Security.Policy;


namespace nomorepass_net
{
    /* 
    NoMorePass class: C# interface for sending/retrieving credentials
    using nomorepass.com cybersecurity services.
    */

    public class NoMorePass
    {

        public string _apikey = "FREEAPI";
        public string _server = "api.nomorepass.com";
        public string _base = "https://api.nomorepass.com";
        public string getidUrl = "";
        public string checkUrl = "";
        public string referenceUrl = "";
        public string grantUrl = "";
        public string pingUrl = "";
        public bool stopped = false;
        public string ticket = "";
        public string token = "";
        NmpCrypto nmpc = new NmpCrypto();
        public int? expiry = null;


        /// <summary>
        /// * Constructor class *
        /// </summary>
        /// <param name="server"></param>
        /// <param name="apikey"></param>
        public NoMorePass(string server = null, string apikey = null)
        {
            if (server == null)
                server = "api.nomorepass.com";

            if (apikey != null)
                _apikey = apikey;
            else
                apikey = "FREEAPI";

            this._apikey = apikey;
            this._server = server;
            this._base = "https://" + _server;
            this.getidUrl = this._base + "/api/getid.php";
            this.checkUrl = this._base + "/api/check.php";
            this.referenceUrl = this._base + "/api/reference.php";
            this.grantUrl = this._base + "/api/grant.php";
            this.pingUrl = this._base + "/api/ping.php";
        }


        /// <summary>
        /// Generate a random Token, returns a random string token.
        /// </summary>
        /// <param name="size"></param>
        public string NewToken(int? size = null)
        {
            Random random = new Random();
            string charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string retVal = "";
            if (size == null)
                size = 12;

            for (int i = 0; i < size; i++)
            {
                retVal += charset[random.Next(0, charset.Length - 1)];
            }
            return retVal;
        }

        /// <summary>
        /// Set expiry time.
        /// </summary>
        /// <param name="expiry"></param>
        public void SetExpiry(int? expiry)
        {
            this.expiry = expiry;
        }

        /// <summary>
        /// Stop the fuction.
        /// </summary>
        public void Stop()
        {
            this.stopped = true;
        }

        /// <summary>
        /// Generate a QR to get the User´s credentials.
        /// </summary>
        /// <param name="site"></param>
        public async Task<string> GetQrText(string site)
        {
            Dictionary<string, string> data = new Dictionary<string, string>() { { "site", site } };

            if (this.expiry != null)
                data["expiry"] = this.expiry.ToString();

            var headers = new Dictionary<string, string>() { { "User-Agent", "NoMorePass-Net/1.0" }, { "apikey", this._apikey } };

            HttpClient httpClient = new HttpClient();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var url = new Uri(this.getidUrl).ToString();
            var resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var body = await resp.Content.ReadAsStringAsync();
                var _data = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);
                if (_data["resultado"] == "ok")
                {
                    this.ticket = _data["ticket"];
                    this.token = this.NewToken();
                    var text = "nomorepass://" + this.token + this.ticket + site;
                    return text;
                }
            }
            return null;
        }

        /// <summary>
        /// GetQrNomorekeys fuction
        /// </summary>
        /// <param name="site"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="type"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public async Task<string> GetQrNomorekeys(string site, string user, string password, string type, Dictionary<string, object> extra = null)
        {

            if (type != "SOUNDKEY" && type != "LIGHTKEY" && type != "BLEKEY")
                type = "KEY";

            if (site == null)
                site = "WEBDEVICE";

            Dictionary<string, string> data = new Dictionary<string, string>() { { "site", site } };

            if (this.expiry != null)
                data["expiry"] = this.expiry.ToString();

            var headers = new Dictionary<string, string>() { { "User-Agent", "NoMorePass-Net/1.0" }, { "apikey", this._apikey } };

            HttpClient httpClient = new HttpClient();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var url = new Uri(this.getidUrl).ToString();
            var resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var body = resp.Content.ReadAsStringAsync().Result;
                var datos = JsonConvert.DeserializeObject<Dictionary<string, string>>(body);

                if (datos["resultado"] == "ok")
                {
                    var token = NewToken();
                    this.token = token;
                    this.ticket = datos["ticket"];

                    if (type == "SOUNDKEY")
                        password = password.PadRight(14).Substring(0, 14);
                    else if (type == "LIGHTKEY")
                        password = (int.Parse(password) % 65536).ToString();

                    var ep = nmpc.Encrypt(password, token);
                    string extraStr = "";
                    if (extra != null)
                    {
                        if (!extra.ContainsKey("type"))
                            extra["type"] = type.ToLowerInvariant();

                        if (extra.ContainsKey("extra"))
                        {
                            string jsonExtra = JsonConvert.SerializeObject(extra);
                            var theExtra = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonExtra);

                            if (theExtra.ContainsKey("secret"))
                            {
                                theExtra["secret"] = nmpc.Encrypt((string)theExtra["secret"], token);

                                if (!theExtra.ContainsKey("type"))
                                    theExtra["type"] = type.ToLower();
                            }
                            else
                            {
                                if (!theExtra.ContainsKey("type"))
                                    theExtra["type"] = type.ToLowerInvariant();

                                extra["extra"] = theExtra.ToDictionary(kvp => kvp.Key, kvp => int.TryParse(kvp.Value.ToString(), out int result) ? result : kvp.Value);
                            }
                            extraStr = JsonConvert.SerializeObject(extra);
                        }
                        else
                        {
                            extra = new Dictionary<string, object> { { "extra", new Dictionary<string, object> { { "type", type.ToLower() } } } };
                            var extrastr = JsonConvert.SerializeObject(extra);

                        }
                    }
                    var _params = new Dictionary<string, string>
                    {
                        { "grant", "grant" },
                        { "ticket", this.ticket },
                        { "user", user },
                        { "password", ep },
                        { "extra", extraStr }
                    };

                    Console.WriteLine(_params);

                    url = new Uri(this.grantUrl).ToString();
                    var _resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(_params));

                    if (_resp.StatusCode == HttpStatusCode.OK)
                    {
#if NET472 || NET48
                        var dat = JsonConvert.DeserializeObject<JObject>(await resp.Content.ReadAsStringAsync());
                        if (dat["resultado"].Value<string>() == "ok")
                        {
                            var text = "nomorekeys://" + type + token + dat["ticket"] + site;
                            return text;
                        }
#elif NETSTANDARD2_0_OR_GREATER
                        var json = await resp.Content.ReadAsStringAsync();
                        var dat = JObject.Parse(json);
                        if (dat["resultado"].Value<string>() == "ok")
                        {
                            var text = "nomorekeys://" + type + token + dat["ticket"] + site;
                            return text;
                        }
#else
                        var dat = JsonConvert.DeserializeObject<dynamic>(await resp.Content.ReadAsStringAsync());
                        if (dat["resultado"] == "ok")
                        {
                            var text = "nomorekeys://" + type + token + dat["ticket"] + site;
                            return text;
                        }
#endif
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Start the pooling process to receive the password.
        /// </summary>
        public async Task<Dictionary<string, string>> Start()
        {
            while (this.stopped == false)
            {
                var data = new Dictionary<string, string> { { "ticket", this.ticket } };
                var headers = new Dictionary<string, string>
                {
                    { "User-Agent", "NoMorePass-Net/1.0" },
                    { "apikey", this._apikey }
                };
                HttpClient httpClient = new HttpClient();
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                var url = new Uri(this.checkUrl);
                var resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(data));

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var cuerpo = await resp.Content.ReadAsStringAsync();
                    var decoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(cuerpo);
                    if (decoded["resultado"] == "ok")
                    {
                        if (decoded["grant"] == "deny")
                            return new Dictionary<string, string> { { "error", "denied" } };
                        else
                        {
                            if (decoded["grant"] == "grant")
                            {
                                var res = new Dictionary<string, string>
                                {
                                    { "user", decoded["usuario"] },
                                    { "password", nmpc.Decrypt(decoded["password"], this.token) },
                                    { "extra", decoded["extra"] }
                                };
                                return res;
                            }
                            else
                            {
                                if (decoded["grant"] == "expired")
                                    return new Dictionary<string, string> { { "error", "expired" } };
                                else
                                    await Task.Delay(TimeSpan.FromSeconds(4));
                            }
                        }
                    }
                    else
                        return new Dictionary<string, string> { { "error", decoded["error"] } };
                }
                else
                    return new Dictionary<string, string> { { "error", "network error" } };
            }
            this.stopped = false;
            return new Dictionary<string, string> { { "error", "stopped" } };
        }

        /// <summary>
        /// Generate a QR to send the User´s credentials.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="extra"></param>
        public async Task<string> GetQrSend(string site, string user, string password, Dictionary<string, object> extra)
        {
            if (site == null)
                site = "WEBDEVICE";

            var data = new Dictionary<string, string> { { "site", site } };

            if (this.expiry != null)
                data["expiry"] = this.expiry.ToString();

            var headers = new Dictionary<string, string>
            {
                { "User-Agent", "NoMorePass-Net/1.0" },
                { "apikey", this._apikey }
            };

            HttpClient httpClient = new HttpClient();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var url = new Uri(this.getidUrl);
            var content = new FormUrlEncodedContent(data);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var resp = await httpClient.PostAsync(url, content);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var body = await resp.Content.ReadAsStringAsync();
                var datos = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                if (datos["resultado"] as string == "ok")
                {
                    var token = this.NewToken();
                    this.token = token;
                    this.ticket = datos["ticket"] as string;
                    var ep = this.nmpc.Encrypt(password, token);
                    string extrastr = "";

                    if (extra != null)
                    {
                        if (extra.ContainsKey("extra"))
                        {
                            var theExtra = ((Dictionary<string, object>)extra["extra"]).ToDictionary(kvp => kvp.Key, GetValue);

                            if (theExtra.ContainsKey("secret"))
                            {
                                if (extra.TryGetValue("extra", out object extraValue))
                                {
                                    var extraDict = (Dictionary<string, object>)extraValue;

                                    if (extraDict.TryGetValue("secret", out object secretValue))
                                    {
                                        extraDict["secret"] = this.nmpc.Encrypt((string)secretValue, token);
                                        extraDict["type"] = "pwd";
                                    }
                                }
                            }
                            else
                            {
                                theExtra["type"] = "pwd";
                                extra["extra"] = theExtra.ToDictionary(kvp => kvp.Key, kvp => int.TryParse(kvp.Value.ToString(), out int result) ? result : kvp.Value);
                            }
                        }
                        var _params = new Dictionary<string, string>
                        {
                            { "grant", "grant" },
                            { "ticket", this.ticket },
                            { "user", user },
                            { "password", ep },
                            { "extra", extrastr }
                        };

                        url = new Uri(this.grantUrl);
                        resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(_params));

                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            var dat = JsonConvert.DeserializeObject<Dictionary<string, object>>(await resp.Content.ReadAsStringAsync());
                            if (dat["resultado"] as string == "ok")
                            {
                                var text = $"nomorepass://SENDPASS{token}{dat["ticket"]}{site}";
                                return text;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Check if the sent password has been received.
        /// Only for when a positive or negative response is received
        /// or it is checked that stopped = true
        /// </summary>
        public async Task<Dictionary<string, object>> Send()
        {
            while (stopped == false)
            {
                var parameters = new Dictionary<string, string>
                {
                    {"device", "WEBDEVICE" },
                    {"ticket", ticket }
                };

                var headers = new Dictionary<string, string>
                {
                    {"User-Agent", "NoMorePass-Net/1.0" },
                    {"apikey", _apikey }
                };

                HttpClient httpClient = new HttpClient();
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var url = new Uri(pingUrl);
                var response = await httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var dat = JsonConvert.DeserializeObject<Dictionary<string, object>>(await response.Content.ReadAsStringAsync());

                    if (dat["resultado"] as string == "ok" && dat["ping"] as string == "ok")
                        await Task.Delay(TimeSpan.FromSeconds(4));

                    else
                        return dat;
                }
            }
            return new Dictionary<string, object> { { "error", "stopped" } };
        }

        /// <summary>
        /// SendRemotePassToDevice fuction - 
        /// Send the User´s Passwords to a remote device.
        /// </summary>
        /// <param name="cloud"></param>
        /// <param name="deviceid"></param>
        /// <param name="secret"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="extra"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> SendRemotePassToDevice(string cloud, string deviceid,
                                        string secret, string username, string password, Dictionary<string, object> extra)
        {
            string cloudurl = "https://api.nmkeys.com/extern/send_ticket";

            if (cloud != null && cloud != "")
                cloudurl = cloud;

            var token = secret;
            var @params = new Dictionary<string, string> { { "site", "Send remote pass" } };

            if (this.expiry != null)
                @params["expiry"] = this.expiry.ToString();

            var headers = new Dictionary<string, string>
            {
                { "User-Agent", "NoMorePass-Net/1.0" },
                { "apikey", this._apikey }
            };

            var url = new Uri(this.getidUrl);
            HttpClient httpClient = new HttpClient();
            foreach (var header in headers)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            var resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(@params));

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var body = await resp.Content.ReadAsStringAsync();
                var dat = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                if ((string)dat["resultado"] == "ok")
                {
                    var ticket = (string)dat["ticket"];
                    var ep = nmpc.Encrypt(password, token);
                    var tExtra = new Dictionary<string, object> { { "type", "remote" } };

                    if (extra != null)
                        tExtra = extra;

                    @params = new Dictionary<string, string>
                    {
                        { "grant", "grant" },
                        { "ticket", ticket },
                        { "user", username },
                        { "password", ep },
                        { "extra", JsonConvert.SerializeObject(tExtra) }
                    };

                    url = new Uri(this.grantUrl);
                    resp = await httpClient.PostAsync(url, new FormUrlEncodedContent(@params));

                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        var _body = await resp.Content.ReadAsStringAsync();
                        var _dat = JsonConvert.DeserializeObject<Dictionary<string, object>>(_body);

                        if ((string)dat["resultado"] == "ok")
                        {
                            var _params = new Dictionary<string, object>
                            {
                                { "hash", token.Substring(0, 10) },
                                { "ticket", ticket },
                                { "deviceid", deviceid }
                            };

                            url = new Uri(cloudurl);
                            resp = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(_params), Encoding.UTF8, "application/json"));

                            if (resp.StatusCode == HttpStatusCode.OK)
                            {
                                dat = JsonConvert.DeserializeObject<Dictionary<string, object>>(await resp.Content.ReadAsStringAsync());
                                return dat;
                            }
                            else
                                return new Dictionary<string, object> { { "error", $"error calling {cloudurl}" } };
                        }
                        else
                            return new Dictionary<string, object> { { "error", dat } };
                    }
                    else
                        return new Dictionary<string, object> { { "error", "error calling granturl" } };
                }
                else
                    return new Dictionary<string, object> { { "error", dat } };
            }
            else
                return new Dictionary<string, object> { { "error", "error calling getid" } };
        }

        /// <summary>
        /// Takes in a KeyValuePair object that contains a string key and an object value, and then attempts to parse the value as an integer.
        /// </summary>
        /// <param name="kvp"></param>
        /// <returns></returns>
        private static object GetValue(KeyValuePair<string, object> kvp)
        {
            return int.TryParse(kvp.Value.ToString(), out int result) ? result : kvp.Value;
        }
    }
}


