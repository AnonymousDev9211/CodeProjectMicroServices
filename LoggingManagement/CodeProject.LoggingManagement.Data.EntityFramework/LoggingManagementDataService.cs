﻿using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.LoggingManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.LoggingManagement.Data.Entities;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Data.SqlClient;
using System.Data.Common;

namespace CodeProject.LoggingManagement.Data.EntityFramework
{
	public class LoggingManagementDataService : EntityFrameworkRepository, ILoggingManagementDataService
	{
		/// <summary>
		/// Create Acknowledgements Queue
		/// </summary>
		/// <param name="acknowledgementsQueue"></param>
		/// <returns></returns>
		public async Task CreateAcknowledgementsQueue(AcknowledgementsQueue acknowledgementsQueue)
		{
			DateTime dateCreated = DateTime.UtcNow;
			acknowledgementsQueue.DateCreated = dateCreated;
			
			await dbConnection.AcknowledgementsQueue.AddAsync(acknowledgementsQueue);
		}
		/// <summary>
		/// Create Messages Sent
		/// </summary>
		/// <param name="messagesSent"></param>
		/// <returns></returns>
		public async Task CreateMessagesSent(MessagesSent messagesSent)
		{
			DateTime dateCreated = DateTime.UtcNow;
			messagesSent.DateCreated = dateCreated;
			messagesSent.DateUpdated = dateCreated;

			await dbConnection.MessagesSent.AddAsync(messagesSent);

		}
		/// <summary>
		/// Create Messages Received
		/// </summary>
		/// <param name="messagesReceived"></param>
		/// <returns></returns>
		public async Task CreateMessagesReceived(MessagesReceived messagesReceived)
		{
			DateTime dateCreated = DateTime.UtcNow;
			messagesReceived.DateCreated = dateCreated;
			
			await dbConnection.MessagesReceived.AddAsync(messagesReceived);

		}

		/// <summary>
		/// Get Message Sent
		/// </summary>
		/// <param name="senderId"></param>
		/// <param name="exchangeName"></param>
		/// <param name="transactionCode"></param>
		/// <returns></returns>
		public async Task<MessagesSent> GetMessageSent(int senderId, string exchangeName, string transactionCode)
		{
			MessagesSent messagesSent = await dbConnection.MessagesSent.Where(x =>
				x.SenderTransactionQueueId == senderId && 
				x.ExchangeName == exchangeName && 
				x.TransactionCode == transactionCode)
					.FirstOrDefaultAsync();

			return messagesSent;
		}
		/// <summary>
		/// Get Message Received
		/// </summary>
		/// <param name="senderId"></param>
		/// <param name="exchangeName"></param>
		/// <param name="transactionCode"></param>
		/// <param name="queueName"></param>
		/// <returns></returns>
		public async Task<MessagesReceived> GetMessageReceived(int senderId, string exchangeName, string transactionCode, string queueName)
		{
			MessagesReceived messagesReceived = await dbConnection.MessagesReceived.Where(x =>
				x.SenderTransactionQueueId == senderId &&
				x.ExchangeName == exchangeName &&
				x.TransactionCode == transactionCode &&
				x.QueueName == queueName)
					.FirstOrDefaultAsync();

			return messagesReceived;
		}

		/// <summary>
		/// Update Messages Sent
		/// </summary>
		/// <param name="messagesSent"></param>
		/// <returns></returns>
		public async Task UpdateMessagesSent(MessagesSent messagesSent)
		{
			await Task.Delay(0);
			DateTime dateUpdated = DateTime.UtcNow;
			messagesSent.DateUpdated = dateUpdated;
		}

		/// <summary>
		/// Process Acknowledgements Queue
		/// </summary>
		/// <returns></returns>
		public async Task<List<AcknowledgementsQueue>> ProcessAcknowledgementsQueue()
		{ 
			List<AcknowledgementsQueue> acknowledgementsQueue = await dbConnection.AcknowledgementsQueue.OrderBy(x => x.AcknowledgementsQueueId).ToListAsync();
			return acknowledgementsQueue;
		}

		/// <summary>
		/// Delete Acknowledgements Queue Item
		/// </summary>
		/// <param name="acknowledgementsQueueId"></param>
		/// <returns></returns>
		public async Task DeleteAcknowledgementsQueue(int acknowledgementsQueueId)
		{
			AcknowledgementsQueue acknowledgementsQueue = await dbConnection.AcknowledgementsQueue.Where(x => x.AcknowledgementsQueueId == acknowledgementsQueueId).FirstOrDefaultAsync();
			dbConnection.AcknowledgementsQueue.Remove(acknowledgementsQueue);
		}

	}
}