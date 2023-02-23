using System.Linq;
using Xunit;

namespace Commentaries.Data.Test;

public class CommentDbContextTests
{
    private readonly CommentariesContext _context;

    public CommentDbContextTests(CommentariesContext context)
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
