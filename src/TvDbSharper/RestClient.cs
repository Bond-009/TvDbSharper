namespace TvDbSharper
{
    using System;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;

    using TvDbSharper.JsonSchemas;

    public class RestClient : IRestClient
    {
        private const string DefaultBaseUrl = "https://api.thetvdb.com";

        public RestClient(IHttpJsonClient jsonClient)
        {
            this.JsonClient = jsonClient;

            if (this.JsonClient.BaseUrl == null)
            {
                this.JsonClient.BaseUrl = DefaultBaseUrl;
            }
        }

        private IHttpJsonClient JsonClient { get; }

        public async Task AuthenticateAsync(AuthenticationRequest authenticationRequest, CancellationToken cancellationToken)
        {
            if (authenticationRequest == null)
            {
                throw new ArgumentNullException(nameof(authenticationRequest));
            }

            var response = await this.JsonClient.PostJsonAsync<AuthenticationResponse>("/login", authenticationRequest, cancellationToken);

            this.JsonClient.AuthorizationHeader = new AuthenticationHeaderValue("Bearer", response.Token);
        }

        public async Task<ActorResponse[]> GetSeriesActorsAsync(int seriesId, CancellationToken cancellationToken)
            => await this.GetDataAsync<ActorResponse[]>($"/series/{seriesId}/actors", cancellationToken);

        public async Task<SeriesResponse> GetSeriesAsync(int seriesId, CancellationToken cancellationToken)
            => await this.GetDataAsync<SeriesResponse>($"/series/{seriesId}", cancellationToken);

        public async Task<EpisodeResponse[]> GetSeriesEpisodesAsync(int seriesId, CancellationToken cancellationToken)
            => await this.GetDataAsync<EpisodeResponse[]>($"/series/{seriesId}/episodes", cancellationToken);

        public async Task RefreshTokenAsync(CancellationToken cancellationToken)
        {
            var response = await this.JsonClient.GetJsonAsync<AuthenticationResponse>("/refresh_token", cancellationToken);

            this.JsonClient.AuthorizationHeader = new AuthenticationHeaderValue("Bearer", response.Token);
        }

        // public async Task<SearchResponse[]> SearchSeriesAsync(string name, CancellationToken cancellationToken)
        // {
        // return await this.GetDataAsync<SearchResponse[]>($"/search/series?name={Uri.EscapeDataString(name)}", cancellationToken);
        // }
        private async Task<T> GetDataAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            return (await this.JsonClient.GetJsonDataAsync<T>(requestUri, cancellationToken)).Data;
        }
    }
}