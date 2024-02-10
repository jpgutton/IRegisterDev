using AutoMapper;
using DashBoardDev.DAL;
using DashBoardDev.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DashBoardDev.Models;
using Newtonsoft.Json;

namespace DashBoardDev.Services
{
    public class AzureQueues
    {
        #region Fields

        private readonly IOptions<AppSettings> appSettings;
        private readonly IMapper _mapper;

        private string _queueConnection;

        private CloudQueue newNPOQueue;

        #endregion

        #region Constructors

        public AzureQueues(
            IOptions<AppSettings> appSettings,
            IOptions<AzureQueuesOptions> queueConnection,
            IMapper mapper)
        {
            this.appSettings = appSettings;
            _queueConnection = queueConnection.Value.AzQueueConnection;
            _mapper = mapper;
            InitializeStorage();
        }

        #endregion

        /// <summary>
        /// Initialize storage for Queues
        /// </summary>
        private void InitializeStorage()
        {
            var storageAccount = CloudStorageAccount.Parse(_queueConnection);

            // Get context object for working with queues, and 
            // set a default retry policy appropriate for a web user interface.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

            // Get a reference to the queue.
            newNPOQueue = queueClient.GetQueueReference("newnpo");
        }


        /// <summary>
        /// Add a new queue containing a JSON of the new NPO (to be used to send email)
        /// </summary>
        /// <param name="newNPO"></param>
        /// <returns></returns>
        public async Task AddNewNPO(NPO newNPO)
        {
            var serializedNewNPO = JsonConvert.SerializeObject(newNPO);

            var queueMessage = new CloudQueueMessage(serializedNewNPO);
            await newNPOQueue.AddMessageAsync(queueMessage);
        }
    }
}
