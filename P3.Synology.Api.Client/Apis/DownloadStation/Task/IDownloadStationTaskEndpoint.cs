﻿using System.Threading.Tasks;
using P3.Synology.Api.Client.Apis.DownloadStation.Task.Models;

namespace P3.Synology.Api.Client.Apis.DownloadStation.Task
{
    public interface IDownloadStationTaskEndpoint
    {
        Task<DownloadStationTaskListResponse> ListAsync();
    }
}
