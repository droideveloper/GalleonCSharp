using GalleonApplication.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GalleonApplication.Extra.Net {
    
    public class AccessTokenHttpClient : HttpClientHandler {

        private const string TOKEN_KEY = "X-Auth-Token";

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken) {
            object t = App.Current.Properties[TOKEN_KEY];
            string token = (t ?? "").ToString();//it might return null so let's make it sure we put it pretty place
            //do not add it if it's empty or null
            if(!string.IsNullOrEmpty(token)) {
                request.Headers.Add(TOKEN_KEY, token);
            }

            Debug.WriteLine("Request:\n");
            Debug.WriteLine(request.ToString());
            if(request.Content != null) {
                Debug.WriteLine(await request.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

            Debug.WriteLine("Response:\n");
            Debug.WriteLine(response.ToString());
            if(response.Content != null) {
                Debug.WriteLine(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }

            return response;            
        }
    }
}
