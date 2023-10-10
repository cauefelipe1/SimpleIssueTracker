using System.Globalization;
using IssueTrackerModels.Models;
using MediatR;

namespace SitemateIssueTrackerApp.Issue;

public class IssueMediator
{
    private static IssueDto BuildDto(IssueModel model)
    {
        var dto = new IssueDto
        {
            IssueId = model.Id,
            Title = model.Title,
            Description = model.Description
        };

        return dto;
    }

    #region Create
    public class CreateIssueCommand : IRequest<Guid>
    {
        public IssueModel Input { get; set; }

        public CreateIssueCommand(IssueModel input) => Input = input;
    }

    public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, Guid>
    {
        private readonly IIssueRepository _repo;

        public CreateIssueCommandHandler(IIssueRepository repo) => _repo = repo;
        
        public Task<Guid> Handle(CreateIssueCommand request, CancellationToken _)
        {
            var dto = BuildDto(request.Input);

            var issueId = _repo.CreateIssue(dto);

            return Task.FromResult(issueId);
        }
    }
    #endregion Create

    #region Get
    public class GetIssueCommand : IRequest<IssueModel?>
    {
        public Guid IssueId { get; set; }

        public GetIssueCommand(Guid issueId) => IssueId = issueId;
    }

    public class GetIssueCommandHandler : IRequestHandler<GetIssueCommand, IssueModel?>
    {
        private readonly IIssueRepository _repo;

        public GetIssueCommandHandler(IIssueRepository repo) => _repo = repo;
        
        public Task<IssueModel?> Handle(GetIssueCommand request, CancellationToken _)
        {
            var dto = _repo.GetIssue(request.IssueId);
            
            if (dto is null)
                return Task.FromResult<IssueModel?>(null);

            var model = BuildModel(dto);

            return Task.FromResult<IssueModel?>(model);
        }
        
        private static IssueModel BuildModel(IssueDto dto)
        {
            var model = new IssueModel()
            {
                Id = dto.IssueId,
                Title = dto.Title,
                Description = dto.Description
            };

            return model;
        }
    }
    #endregion Get
    
    #region Update
    public class UpdateIssueCommand : IRequest
    {
        public IssueModel Input { get; set; }

        public UpdateIssueCommand(IssueModel input) => Input = input;
    }

    public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand>
    {
        private readonly IIssueRepository _repo;

        public UpdateIssueCommandHandler(IIssueRepository repo) => _repo = repo;
        
        public Task Handle(UpdateIssueCommand request, CancellationToken _)
        {
            var dto = BuildDto(request.Input);

            _repo.UpdateIssue(dto);
            
            return Task.CompletedTask;
        }
    }
    #endregion Update
    
    #region Delete
    public class DeleteIssueCommand : IRequest
    {
        public Guid IssueId { get; set; }

        public DeleteIssueCommand(Guid issueId) => IssueId = issueId;
    }

    public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand>
    {
        private readonly IIssueRepository _repo;

        public DeleteIssueCommandHandler(IIssueRepository repo) => _repo = repo;
        
        public Task Handle(DeleteIssueCommand request, CancellationToken _)
        {
            _repo.DeleteIssue(request.IssueId);
            
            return Task.CompletedTask;
        }
    }
    #endregion Delete
}