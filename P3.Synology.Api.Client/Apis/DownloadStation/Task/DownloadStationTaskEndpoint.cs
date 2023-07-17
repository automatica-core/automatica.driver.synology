﻿using System.Collections.Generic;
using System.Threading.Tasks;
using P3.Synology.Api.Client.ApiDescription;
using P3.Synology.Api.Client.Apis.DownloadStation.Task.Models;
using P3.Synology.Api.Client.Session;

namespace P3.Synology.Api.Client.Apis.DownloadStation.Task
{
    public class DownloadStationTaskEndpoint : IDownloadStationTaskEndpoint
    {
        private readonly ISynologyHttpClient _synologyHttpClient;
        private readonly IApiInfo _apiInfo;
        private readonly ISynologySession _session;

        public DownloadStationTaskEndpoint(ISynologyHttpClient synologyHttpClient, IApiInfo apiInfo, ISynologySession session)
        {
            _synologyHttpClient = synologyHttpClient;
            _apiInfo = apiInfo;
            _session = session;
        }

        public Task<DownloadStationTaskListResponse> ListAsync()
        {
            var queryParams = new Dictionary<string, string>
            {
                { "additional",  "detail,file" }
            };

            return _synologyHttpClient.GetAsync<DownloadStationTaskListResponse>(_apiInfo, "list", queryParams, _session);
        }
    }
}
