using System.Collections.Immutable;

namespace Company.Domain.Models;

public class Branch
{
    
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; } = false;
}
