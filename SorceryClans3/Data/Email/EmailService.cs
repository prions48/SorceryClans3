using System;
using Azure;
using Azure.Communication.Email;

namespace SorceryClans3.Data.Email
{
    public class EmailService
    {
        private EmailClient emailClient { get; set; }
        private readonly IEmailSettings _settings;

        public EmailService(IEmailSettings settings)
        {
            _settings = settings;
            emailClient = new EmailClient(_settings.GetConnString());
        }
        /// <summary>
        /// Returns status code in string format.  Returns "Succeeded" if successful.
        /// </summary>
        /// <param name="toemail">Email to send to</param>
        /// <param name="subject">Subject of email</param>
        /// <param name="body">Body (HTML)</param>
        /// <returns></returns>
		public async Task<string> SendEmail(string toemail, string subject, string body)
        {
            string ret = "";
            try
            {
                EmailSendOperation emailSendOperation = await emailClient.SendAsync(
                    Azure.WaitUntil.Completed,
                    _settings.GetSender(),
                    toemail,
                    subject,
                    body);
                EmailSendResult statusMonitor = emailSendOperation.Value;

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;

                ret = emailSendOperation.Value.Status.ToString();
            }
            catch (RequestFailedException ex)
            {
                /// OperationID is contained in the exception message and can be used for troubleshooting purposes
                //write to DB log?
                return ex.ErrorCode ?? "Unknown Error";
            }
            return ret;
        }
    }
    
}