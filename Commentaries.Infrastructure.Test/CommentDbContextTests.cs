using Commentaries.Infrastructure.SecondaryAdapters.Db;
using System.Linq;
using Xunit;

namespace Commentaries.Infrastructure.Test;

public class CommentDbContextTests
{
    private readonly CommentariesDbContext _context;

    public CommentDbContextTests(CommentariesDbContext context)
    {
        _context = context;
    }

    [Fact]
    public void TestRead()
    {
        _context.Comments.FirstOrDefault();
        _context.CommentStates.FirstOrDefault();
        _context.ObjectTypes.FirstOrDefault();
        _context.CommentFiles.FirstOrDefault();

        Assert.True(true);
    }
}
