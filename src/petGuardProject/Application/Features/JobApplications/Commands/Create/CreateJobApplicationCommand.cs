using Application.Features.Backers.Commands.Create;
using Application.Features.Backers.Rules;
using Application.Features.JobApplications.Rules;
using Application.Features.PetAds.Rules;
using Application.Features.PetOwners.Rules;
using Application.Services.BackerService;
using Application.Services.PetAdsService;
using Application.Services.PetOwnersService;
using Application.Services.Repositories;
using AutoMapper;
using Core.Mailing;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Application.Features.JobApplications.Commands.Create;
public class CreateJobApplicationCommand:IRequest<CreatedJobApplicationResponse>
{
    public Guid PetAdId { get; set; }
    public Guid BackerId { get; set; }


    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand, CreatedJobApplicationResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IJobApplicationRepository _jobApplicationRepository;
        private readonly IMapper _mapper;
        private readonly IPetAdService _petAdService;
        private readonly IBackerService _backerService;
        private readonly IPetOwnerService _petOwnerService;
        private readonly JobApplicationBusinessRules _jobApplicationBusinesRules;
        private readonly PetAdBusinesRules _petAdBusinesRules;
        private readonly BackerBusinessRules _backerBusinessRules;
        private readonly PetOwnerBusinessRules _petOwnerBusinessRules;


        private IConnection _connection;
        private IModel _channel;
        private const string _exchangeName = "emailExchange";
        private const string _queueName = "emailQueue";

        public CreateJobApplicationCommandHandler(IConfiguration configuration,IJobApplicationRepository jobApplicationRepository, IMapper mapper, IPetAdService petAdService,IBackerService backerService,IPetOwnerService petOwnerService,JobApplicationBusinessRules jobApplicationBusinesRules, PetAdBusinesRules petAdBusinesRules,BackerBusinessRules backerBusinessRules,PetOwnerBusinessRules petOwnerBusinessRules)
        {
            _configuration = configuration;
            _jobApplicationRepository = jobApplicationRepository;
            _mapper = mapper;
            _petAdService = petAdService;
            _backerService = backerService;
            _petOwnerService = petOwnerService;
            _jobApplicationBusinesRules = jobApplicationBusinesRules;
            _petAdBusinesRules = petAdBusinesRules;
            _backerBusinessRules = backerBusinessRules;
            _petOwnerBusinessRules = petOwnerBusinessRules;
        }

        public async Task<CreatedJobApplicationResponse> Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            await _jobApplicationBusinesRules.CheckIfBackerHasAlreadyApplied(request.PetAdId, request.BackerId);

            PetAd? petAd = await _petAdService.GetAsync(x => x.Id == request.PetAdId, include: m => m.Include(m => m.PetOwner),enableTracking:false);
            await _petAdBusinesRules.PetAdExistsWhenSelected(petAd);

            Backer? backer = await _backerService.GetAsync(x => x.Id == request.BackerId, include:m => m.Include(m => m.User),enableTracking: false);
            await _backerBusinessRules.BackerShouldExistWhenSelected(backer);

            PetOwner? petOwner = await _petOwnerService.GetAsync(x => x.Id == petAd!.PetOwnerId, include: m => m.Include(m => m.User), enableTracking: false);
            await _petOwnerBusinessRules.PetOwnerExistsWhenSelected(petOwner);

            JobApplication jobApplication = _mapper.Map<JobApplication>(request);

            await _jobApplicationRepository.AddAsync(jobApplication);

            CreatedJobApplicationResponse response = _mapper.Map<CreatedJobApplicationResponse>(jobApplication);

            
            Mail mailForBacker = CreateEmailTemplateForBacker(backer!);
            Mail mailForAdversementOwner = CreateEmailTemplateForPetOwner(backer!,petOwner!,petAd!);

            SendEmails(new List<Mail>{ mailForBacker, mailForAdversementOwner});

            return response;
        }


        private void SendEmails(List<Mail> mails)
        {
            var uri = _configuration.GetValue<string>("RabbitMQConfiguration:Uri");

            var factory = new ConnectionFactory
            {
                Uri = new Uri(uri!),
                UserName = _configuration.GetValue<string>("RabbitMQConfiguration:Username"),
                Password = _configuration.GetValue<string>("RabbitMQConfiguration:Password"),
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout, true, false);
            _channel.QueueDeclare(queue: _queueName,
                     durable: true,
            exclusive: false,
                     autoDelete: false,
                     arguments: null);
            _channel.QueueBind(queue: _queueName,
                              exchange: _exchangeName,
            routingKey: "");


            foreach (var mail in mails)
            {
                string mailSerialize = JsonConvert.SerializeObject(mail);
                byte[] mailByte = Encoding.UTF8.GetBytes(mailSerialize);

                var properties = _channel.CreateBasicProperties();
                properties.Persistent = true;

                _channel.BasicPublish(exchange: _exchangeName, routingKey: "", basicProperties: properties, body: mailByte);
            }
        }


        private Mail CreateEmailTemplateForBacker(Backer backer)
        {
            Mail mail = new Mail();
            mail.ToList = new List<MailboxAddress>
                {
                    new MailboxAddress($"{backer.FirstName} {backer.LastName}", address: backer.User.Email)
                };

            mail.Subject = "Application Received - PETGUARD";
            mail.HtmlBody = $@"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Application Received - PETGUARD</title>
                        <style>
                            body {{
                                font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                                background-color: #f0f0f0;
                                margin: 0;
                                padding: 20px;
                                display: flex;
                                justify-content: center;
                                align-items: center;
                                min-height: 100vh;
                            }}
                            .container {{
                                background-color: #ffffff;
                                padding: 30px;
                                border-radius: 15px;
                                box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
                                max-width: 600px;
                                width: 100%;
                            }}
                            .welcome-text {{
                                font-size: 24px;
                                font-weight: bold;
                                color: #3498db;
                                margin-bottom: 20px;
                                text-align: center;
                            }}
                            .info-text {{
                                font-size: 18px;
                                color: #333333;
                                margin-bottom: 30px;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <p class='welcome-text'>Application Received</p>
                            <p class='info-text'>Dear {backer.FirstName} {backer.LastName},</p>
                            <p class='info-text'>Thank you for submitting your application to PETGUARD. We have received your application and will review it shortly. We will get back to you with further details.</p>
                            <p class='info-text'>Best regards,<br>The PETGUARD Team</p>
                        </div>
                    </body>
                    </html>";

            return mail;
        }


        private Mail CreateEmailTemplateForPetOwner(Backer backer, PetOwner petOwner, PetAd petAd)
        {
            Mail mail = new Mail();
            mail.ToList = new List<MailboxAddress>
            {
                new MailboxAddress($"{petOwner.FirstName} {petOwner.LastName}", address: petOwner.User.Email)
            };

            mail.Subject = "New Application Received - PETGUARD";
            mail.HtmlBody = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>New Application Received - PETGUARD</title>
                <style>
                    body {{
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        background-color: #f0f0f0;
                        margin: 0;
                        padding: 20px;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        min-height: 100vh;
                    }}
                    .container {{
                        background-color: #ffffff;
                        padding: 30px;
                        border-radius: 15px;
                        box-shadow: 0 0 15px rgba(0, 0, 0, 0.1);
                        max-width: 600px;
                        width: 100%;
                    }}
                    .header-text {{
                        font-size: 24px;
                        font-weight: bold;
                        color: #3498db;
                        margin-bottom: 20px;
                        text-align: center;
                    }}
                    .info-text {{
                        font-size: 18px;
                        color: #333333;
                        margin-bottom: 30px;
                        text-align: center;
                    }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <p class='header-text'>New Application Received</p>
                    <p class='info-text'>Dear {petOwner.FirstName},</p>
                    <p class='info-text'>We wanted to let you know that a new application has been submitted for your job advertisement titled <strong>{petAd.Title}</strong>.</p>
                    <p class='info-text'>Applicant Details:</p>
                    <p class='info-text'><strong>Name:</strong> {backer.FirstName} {backer.LastName}</p>
                    <p class='info-text'><strong>Email:</strong> {backer.User.Email}</p>
                    <p class='info-text'>Please review the application in your admin panel for more details.</p>
                    <p class='info-text'>Best regards,<br>The PETGUARD Team</p>
                </div>
            </body>
            </html>";

            return mail;
        }

    }
}
