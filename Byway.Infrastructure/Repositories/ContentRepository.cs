using Byway.Domain.Entities;
using Byway.Domain.Interfaces;
using Byway.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Infrastructure.Repositories
{
    public class ContentRepository: GenericRepository<Content>, IContentRepository
    {
        public ContentRepository(BywayDbContext context) : base(context)
        {
        }
    }
}
