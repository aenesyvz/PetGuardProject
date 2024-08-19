using Application.Features.PetAds.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.PetAds.Commands.Delete;

public class DeletePetAdCommand : IRequest<DeletedPetAdResponse>
{
    public Guid Id { get; set; }


    public class DeletePetAdCommandHandler : IRequestHandler<DeletePetAdCommand, DeletedPetAdResponse>
    {
        private readonly IPetAdRepository _petAdRepository;
        private readonly IMapper _mapper;
        private readonly PetAdBusinesRules _petAdBusinesRules;

        public DeletePetAdCommandHandler(IPetAdRepository petAdRepository, IMapper mapper, PetAdBusinesRules petAdBusinesRules)
        {
            _petAdRepository = petAdRepository;
            _mapper = mapper;
            _petAdBusinesRules = petAdBusinesRules;
        }

        public async Task<DeletedPetAdResponse> Handle(DeletePetAdCommand request, CancellationToken cancellationToken)
        {
            PetAd? pet = await _petAdRepository.GetAsync(x => x.Id == request.Id,enableTracking:false,cancellationToken:cancellationToken);

            await _petAdBusinesRules.PetAdExistsWhenSelected(pet);

            await _petAdRepository.DeleteAsync(pet!);

            DeletedPetAdResponse response = _mapper.Map<DeletedPetAdResponse>(pet);

            return response;
        }
    }
}