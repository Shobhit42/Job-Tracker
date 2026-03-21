using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Register.Command
{
    public class RegisterCommand : IRequest<Guid>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
