
namespace Company.Domain.Models;

public class Company
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public DateTime CreateDate { get; set; } = DateTime.Now;
    public int BranchID { get; set; }
    public bool IsDeleted { get; set; } = false;
    public Branch Branch { get; set; }
  
}
