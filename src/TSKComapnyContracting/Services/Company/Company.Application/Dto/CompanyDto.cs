﻿
namespace Company.Application.Dto;
using FluentValidation;
public class CompanyDto
{
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public int BranchID { get; set; }
    public string BranchName { get; set; }
}
